#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.Commands
{
    public class OrganizeMeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("organizar", AccessLevel.Player, OrganizeMe_OnCommand);
        }

        //This command will not move spellbooks, runebooks, blessed items.
        [Usage("organizar")]
        [Description("Organiza as porra tudo na mochila")]
        private static void OrganizeMe_OnCommand(CommandEventArgs arg)
        {
            OrganizePouch weaponPouch = null;
            OrganizePouch armorPouch = null;
            OrganizePouch clothingPouch = null;
            OrganizePouch jewelPouch = null;
            OrganizePouch potionPouch = null;
            OrganizePouch currencyPouch = null;
            OrganizePouch resourcePouch = null;
            OrganizePouch toolPouch = null;
            OrganizePouch regsPouch = null;
            OrganizePouch miscPouch = null;

            Mobile from = arg.Mobile;
            var bp = from.Backpack as Backpack;

            if (@from == null || bp == null)
            {
                return;
            }

            if (bp.TotalWeight >= bp.MaxWeight && from.AccessLevel < AccessLevel.GameMaster)
            {
                if (from is PlayerMobile && from.NetState != null)
                {
                    from.SendMessage("Voce esta muito pesado.");
                }
                return;
            }

            if (bp.TotalItems >= (bp.MaxItems - 10)  && from.AccessLevel < AccessLevel.GameMaster)
            {
                if (from is PlayerMobile && from.NetState != null)
                {
                    from.SendMessage("Voce nao tem espaco em sua mochila.");
                }
                return;
            }

            var backpackitems = new List<Item>(bp.Items);
            var subcontaineritems = new List<Item>();

            foreach (var item in backpackitems.OfType<BaseContainer>())
            {
                var lockable = item as LockableContainer;
                if (lockable != null)
                {
                    if (lockable.CheckLocked(from))
                    {
                        continue;
                    }
                }

                var trapped = item as TrapableContainer;
                if (trapped != null)
                {
                    if (trapped.TrapType != TrapType.None)
                    {
                        continue;
                    }
                }

                // Skip the pouches that are already created
                if (item is OrganizePouch)
                {    
                    if (item.Name == "Armas")
                    {
                        weaponPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Armaduras")
                    {
                        armorPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Roupas")
                    {
                        clothingPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Joias")
                    {
                        jewelPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Pocoes")
                    {
                        potionPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Moedas")
                    {
                        currencyPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Recursos")
                    {
                        resourcePouch = item as OrganizePouch;
                    }
                    if (item.Name == "Ferramentas")
                    {
                        toolPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Reagents")
                    {
                        regsPouch = item as OrganizePouch;
                    }
                    if (item.Name == "Misc")
                    {
                        miscPouch = item as OrganizePouch;
                    }

                    // Skip all the items in the pouches since they should already be organized
                    continue;
                }

                // Add all the subcontainer items, but dont go all the way to comeplete depth
                subcontaineritems.AddRange(item.Items);
            }

            backpackitems.AddRange(subcontaineritems);

            if (weaponPouch == null)
            {
                weaponPouch = new OrganizePouch { Name = "Armas", Hue =92 };
            }
            if (armorPouch == null)
            {
                armorPouch = new OrganizePouch { Name = "Armaduras", Hue = 82 };
            }
            if (clothingPouch == null)
            {
                clothingPouch = new OrganizePouch { Name = "Roupas", Hue = 72 };
            }
            if (jewelPouch == null)
            {
                jewelPouch = new OrganizePouch { Name = "Joias", Hue =62 };
            }
            if (potionPouch == null)
            {
                potionPouch = new OrganizePouch {Name = "Pocoes", Hue =52};
            }
            if (currencyPouch == null)
            {
                currencyPouch = new OrganizePouch {Name = "Moedas", Hue =42};
            }
            if (resourcePouch == null)
            {
                resourcePouch = new OrganizePouch {Name = "Recursos", Hue = 32};
            }
            if (toolPouch == null)
            {
                toolPouch = new OrganizePouch {Name = "Ferramentas", Hue = 22};
            }
            if (regsPouch == null)
            {
                regsPouch = new OrganizePouch {Name = "Reagents", Hue = 12};
            }
            if (miscPouch == null)
            {
                miscPouch = new OrganizePouch { Name = "Misc" };
            }
            var pouches = new List<OrganizePouch>
            {
                weaponPouch,
                armorPouch,
                clothingPouch,
                jewelPouch,
                potionPouch,
                currencyPouch,
                resourcePouch,
                toolPouch,
                regsPouch,
                miscPouch
            };

            foreach (
                Item item in
                    backpackitems.Where(
                        item =>
                            item.LootType != LootType.Blessed && 
                            !(item is Runebook) &&
                            !(item is RecallRune) &&
                            !(item is Key) &&
                            !(item is Spellbook) && 
                            item.Movable && 
                            item.LootType != LootType.Blessed))
            {
                // Lets not add the pouches to themselves
                if (item is OrganizePouch)
                {
                    continue;
                }

                if (item is BaseWeapon)
                {
                    weaponPouch.TryDropItem(from, item, false);
                }
                else if (item is BaseArmor)
                {
                    armorPouch.TryDropItem(from, item, false);
                }
                else if (item is BaseClothing)
                {
                    clothingPouch.TryDropItem(from, item, false);
                }
                else if (item is BaseJewel)
                {
                    jewelPouch.TryDropItem(from, item, false);
                }
                else if (item is BasePotion)
                {
                    potionPouch.TryDropItem(from, item, false);
                }
                else if (item is Gold)
                {
                    currencyPouch.TryDropItem(from, item, false);
                }
                else if (item is BaseIngot || item is BaseOre || item is Feather || item is BaseBoard || item is Log ||
                         item is BaseLeather ||
                         item is Sand || item is BaseGranite)
                {
                    resourcePouch.TryDropItem(from, item, false);
                }
                else if (item is BaseTool)
                {
                    toolPouch.TryDropItem(from, item, false);
                }
                else if (item is BaseReagent)
                {
                    regsPouch.TryDropItem(from, item, false);
                }
                else
                {
                    miscPouch.TryDropItem(from, item, false);
                }
            }

            var x = 45;

            foreach (var pouch in pouches)
            {
                if (pouch.TotalItems <= 0)
                { 
                    continue;
                }
                
                // AddToBackpack doesnt do anything if the item is already in the backpack
                // calls DropItem internally
                
                if (!from.Backpack.Items.Contains(pouch))
                {
                    from.AddToBackpack(pouch);
                }

                pouch.X = x;
                pouch.Y = 65;

                x += 10;
            }
        }
    }
}
