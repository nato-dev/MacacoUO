using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Items;
using Server.Mobiles;

using Server.Custom.RaresCrafting;

namespace Server.Custom.RaresCrafting
{
    public enum ECraftableRare
    {
        RareId1 = 1000, // reusing this ID in the gumps which is why it's starting at 1000
        RareId2,
        RareId3,
        RareId4,
    }

    public enum ECraftableRareCategory
    {
        None,
        Alchemy,
        Bowcrafting,
        Blacksmithing,
        Carpentry,
        Cooking,
        Inscription,
        Tailoring,
        Tinkering,
        Random
    }

    public class CraftableEntry
    {
        public string m_Name;
        public int m_ItemId;
        public int m_AmountRequired;
    }

    public abstract class ICraftableRare
    {
        public string m_FirstRequiredSkill;
        public string m_SecondRequiredSkill;

        public int DispOffsetX; // for gumps
        public int DispOffsetY; // for gumps

        public abstract bool MeetsRequiredSkillLevel_1(Mobile mob);
        public abstract bool MeetsRequiredSkillLevel_2(Mobile mob);
        public abstract CraftableEntry[] GetIngredients();
        public abstract CraftableEntry GetResult();
        public abstract Item GenerateCraftedItem();
    }

    public static class RaresCraftingSystem
    {
        public static void Initialize()
        {
            CommandSystem.Register("rctest", AccessLevel.Player, new CommandEventHandler(ShowRareCraftGump));

            Random = new List<ICraftableRare>()
            {
                RareDefinitions.DecoRandom(),
                RareDefinitions.DecoRandom2(),
            };

            AlchemyCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.AlchemyFlask1(),
                RareDefinitions.AlchemyFlask2(),
                RareDefinitions.AlchemyFlask3(),
            };

            BowcraftingCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.BundleOfArrows(),
                RareDefinitions.BundleOfBolts(),
                RareDefinitions.DecorativeBowAndArrows(),
            };

            BlacksmithingCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.DecorativeHalberd(),
                RareDefinitions.HangingChainmailLeggings(),
                RareDefinitions.GoldIngots(),
                RareDefinitions.CopperIngots(),
            };

            CarpentryCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.DartboardWithAxe(),
                RareDefinitions.RuinedBookcase(),
                RareDefinitions.CoveredChair(),
                RareDefinitions.LogPileLarge()
            };

            CookingCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.PotOfWax(),
                RareDefinitions.KettleOfWax(),
                RareDefinitions.DirtyPan(),
            };

            InscriptionCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.BookPile1(),
                RareDefinitions.BookPile2(),
                RareDefinitions.DamagedBooks(),
                RareDefinitions.ForbiddenWritings()
            };

            TailoringCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.LargeFishingNet(),
                RareDefinitions.DyeableCurtainEast(),
                RareDefinitions.DyeableCurtainSouth(),
            };

            TinkeringCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.Anchor(),
                RareDefinitions.HangingSkeleton1(),
                RareDefinitions.Hook(),
                RareDefinitions.HangingCauldron(),
            };
        }

        [Usage("[rctest")]
        [Description("Shows the rare crafting gump")]
        private static void ShowRareCraftGump(CommandEventArgs e)
        {
            e.Mobile.SendGump(new RaresCraftingGump(e.Mobile, ECraftableRareCategory.None));
        }

        public static List<ICraftableRare> Random;
        public static List<ICraftableRare> AlchemyCraftables;
        public static List<ICraftableRare> BowcraftingCraftables;
        public static List<ICraftableRare> BlacksmithingCraftables;
        public static List<ICraftableRare> CarpentryCraftables;
        public static List<ICraftableRare> CookingCraftables;
        public static List<ICraftableRare> InscriptionCraftables;
        public static List<ICraftableRare> TailoringCraftables;
        public static List<ICraftableRare> TinkeringCraftables;

        public static List<ICraftableRare> GetCraftables(ECraftableRareCategory cat)
        {
            switch (cat)
            {
                case ECraftableRareCategory.Alchemy: return RaresCraftingSystem.AlchemyCraftables;
                case ECraftableRareCategory.Bowcrafting: return RaresCraftingSystem.BowcraftingCraftables;
                case ECraftableRareCategory.Blacksmithing: return RaresCraftingSystem.BlacksmithingCraftables;
                case ECraftableRareCategory.Carpentry: return RaresCraftingSystem.CarpentryCraftables;
                case ECraftableRareCategory.Cooking: return RaresCraftingSystem.CookingCraftables;
                case ECraftableRareCategory.Inscription: return RaresCraftingSystem.InscriptionCraftables;
                case ECraftableRareCategory.Tailoring: return RaresCraftingSystem.TailoringCraftables;
                case ECraftableRareCategory.Tinkering: return RaresCraftingSystem.TinkeringCraftables;
                case ECraftableRareCategory.Random: return RaresCraftingSystem.Random;
                default:
                    return null;
            }
        }

        public static bool TryCreateItem(Mobile owner, ICraftableRare rare)
        {
            Dictionary<Item, int> to_be_consumed = new Dictionary<Item, int>();

            foreach (CraftableEntry ingredient in rare.GetIngredients())
            {
                int found = 0;
                int need = ingredient.m_AmountRequired;
                foreach (Item bpitem in owner.Backpack.Items)
                {
                    if (bpitem.ItemID == ingredient.m_ItemId)
                    {
                        if(Shard.DebugEnabled)
                        {
                            Shard.Debug($"Achei {bpitem.GetType().Name}");
                        }
                        need -= bpitem.Amount;
                        if (need > 0)
                            to_be_consumed[bpitem] = bpitem.Amount;
                        else
                        {
                            var excesso = -need;
                            to_be_consumed[bpitem] = bpitem.Amount - excesso;
                        }
                        found += bpitem.Amount;
                        if (found == ingredient.m_AmountRequired)
                            break;
                    }
                }
                if (found < ingredient.m_AmountRequired)
                {
                    owner.SendMessage($"Voce precisa de {ingredient.m_AmountRequired} {ingredient.m_Name} para isto");
                    return false;
                }
            }

            // have all ingredients in BP at this point

            // create the rare and try put it in the players backpack

            Item result = rare.GenerateCraftedItem();
            if (owner.Backpack.TryDropItem(owner, result, true))
            {
                owner.SendLocalizedMessage(500442); // You create the item and put it in your backpack.

                // delete reagents
                foreach (Item i in to_be_consumed.Keys)
                    i.Consume(to_be_consumed[i]);
            }
            return true;
        }
    }


    /// <summary>
    /// MAIN GUMP
    /// </summary>
    public class RaresCraftingGump : Gump
    {
        private ECraftableRareCategory m_Category;

        public RaresCraftingGump(Mobile from, ECraftableRareCategory category)
            : base(0, 0)
        {
            from.CloseGump(typeof(RaresCraftingGump));

            m_Category = category;

            // MAIN
            AddPage(0);
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(73, 41, 588, 523, 9200);
            this.AddBackground(206, 75, 441, 467, 9200);
            this.AddLabel(80, 258, 2036, "Inscription");
            this.AddLabel(80, 156, 2036, "Blacksmithing");
            this.AddLabel(80, 192, 2036, "Carpentry");
            this.AddLabel(80, 88, 2036, "Alchemy");
            this.AddLabel(80, 292, 2036, "Tailoring");
            this.AddLabel(80, 326, 2036, "Tinkering");
            this.AddLabel(80, 122, 2036, "Bowcrafting");
            this.AddLabel(80, 224, 2036, "Cooking");
            this.AddLabel(80, 356, 78, "Vale Decoracao");
            this.AddButton(172, 88, 4005, 4007, (int)Buttons.btnAlchemy, GumpButtonType.Reply, 0);
            this.AddButton(172, 122, 4005, 4007, (int)Buttons.btnBowcrafting, GumpButtonType.Reply, 0);
            this.AddButton(172, 156, 4005, 4007, (int)Buttons.btnBlacksmithing, GumpButtonType.Reply, 0);
            this.AddButton(172, 190, 4005, 4007, (int)Buttons.btnCarpentry, GumpButtonType.Reply, 0);
            this.AddButton(172, 224, 4005, 4007, (int)Buttons.btnCooking, GumpButtonType.Reply, 0);
            this.AddButton(172, 258, 4005, 4007, (int)Buttons.btnInscription, GumpButtonType.Reply, 0);
            this.AddButton(172, 292, 4005, 4007, (int)Buttons.btnTailoring, GumpButtonType.Reply, 0);
            this.AddButton(172, 326, 4005, 4007, (int)Buttons.btnTinkering, GumpButtonType.Reply, 0);
            this.AddButton(172, 356, 4005, 4007, (int)Buttons.btnRandom, GumpButtonType.Reply, 0);

            if (m_Category == ECraftableRareCategory.None)
            {
                AddLabel(222, 180, 53, "Leia !");
                string s = " -Todos ingredientes precisam estar na sua mochila.\n -Ingredientes precisam ter o mesmo grafico.\n   -Isto inclui .virar mas nao cor do item\n -O resultado sera colocado em sua mochila e nao eh newbie.\n -Os crafts sao finais, nada de retornos !";
                AddHtml(222, 200, 400, 140, s, true, false);
            }
            List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);
            if (craftables == null)
            {
                return;
            }

            // Craft options
            if (craftables.Count > 0)
            {
                this.AddLabel(250, 85, 2036, craftables[0].GetResult().m_Name);
                this.AddButton(309, 139, 4005, 4007, (int)Buttons.btnCraftItemRangeStart, GumpButtonType.Reply, 0);
                this.AddItem(257 + craftables[0].DispOffsetX, 139 + craftables[0].DispOffsetY, craftables[0].GetResult().m_ItemId);
            }
            if (craftables.Count > 1)
            {
                this.AddLabel(507, 85, 2036, craftables[1].GetResult().m_Name);
                this.AddButton(584, 139, 4005, 4007, (int)Buttons.btnCraftItemRangeStart + 1, GumpButtonType.Reply, 0);
                this.AddItem(509 + craftables[1].DispOffsetX, 139 + craftables[1].DispOffsetY, craftables[1].GetResult().m_ItemId);
            }
            if (craftables.Count > 2)
            {
                this.AddLabel(250, 248, 2036, craftables[2].GetResult().m_Name);
                this.AddButton(309, 299, 4005, 4007, (int)Buttons.btnCraftItemRangeStart + 2, GumpButtonType.Reply, 0);
                this.AddItem(257 + craftables[2].DispOffsetX, 299 + craftables[2].DispOffsetY, craftables[2].GetResult().m_ItemId);
            }
            if (craftables.Count > 3)
            {
                this.AddLabel(507, 248, 2036, craftables[3].GetResult().m_Name);
                this.AddButton(584, 299, 4005, 4007, (int)Buttons.btnCraftItemRangeStart + 3, GumpButtonType.Reply, 0);
                this.AddItem(509 + craftables[3].DispOffsetX, 299 + craftables[3].DispOffsetY, craftables[3].GetResult().m_ItemId);
            }
        }

        public enum Buttons
        {
            btnAlchemy = 1,
            btnBowcrafting,
            btnBlacksmithing,
            btnCarpentry,
            btnCooking,
            btnInscription,
            btnTailoring,
            btnTinkering,
            btnRandom,
            btnCraftItemRangeStart = 1000,
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID >= (int)Buttons.btnCraftItemRangeStart && m_Category != ECraftableRareCategory.None)
            {
                List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);
                int which_item = info.ButtonID - (int)Buttons.btnCraftItemRangeStart;
                if (craftables.Count > which_item)
                    state.Mobile.SendGump(new CraftItemGump(state.Mobile, m_Category, which_item));
            }
            else if (info.ButtonID == (int)Buttons.btnAlchemy)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Alchemy));
            }
            else if (info.ButtonID == (int)Buttons.btnBowcrafting)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Bowcrafting));
            }
            else if (info.ButtonID == (int)Buttons.btnBlacksmithing)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Blacksmithing));
            }
            else if (info.ButtonID == (int)Buttons.btnCarpentry)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Carpentry));
            }
            else if (info.ButtonID == (int)Buttons.btnCooking)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Cooking));
            }
            else if (info.ButtonID == (int)Buttons.btnInscription)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Inscription));
            }
            else if (info.ButtonID == (int)Buttons.btnTailoring)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Tailoring));
            }
            else if (info.ButtonID == (int)Buttons.btnTinkering)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Tinkering));
            }
            else if (info.ButtonID == (int)Buttons.btnRandom)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Random));
            }
        }
    }


    public class CraftItemGump : Gump
    {
        private ECraftableRareCategory m_Category;
        private int m_ItemIndex;
        Mobile m_From;
        public CraftItemGump(Mobile from, ECraftableRareCategory category, int itemidx) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            from.CloseAllGumps();

            m_Category = category;
            List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);

            m_ItemIndex = Math.Min(craftables.Count - 1, Math.Max(itemidx, 0));
            m_From = from;

            ICraftableRare the_rare = craftables[m_ItemIndex];

            this.AddPage(0);
            this.AddBackground(5, 5, 441, 361, 9200);
            this.AddBackground(13, 41, 139, 314, 9200);

            // result
            this.AddLabel(14, 16, 53, "Cratavel: ");
            this.AddItem(66 + the_rare.DispOffsetX, 135 + the_rare.DispOffsetY, the_rare.GetResult().m_ItemId);

            // ingredients
            CraftableEntry[] ingredients = the_rare.GetIngredients();
            this.AddLabel(158, 16, 53, "Ingredientes:");

            this.AddItem(190, 81, ingredients[0].m_ItemId);
            this.AddLabel(249, 81, 2036, String.Format("{0} : {1}", ingredients[0].m_Name, ingredients[0].m_AmountRequired));

            if (ingredients.Length > 1)
            {
                this.AddItem(190, 135, ingredients[1].m_ItemId);
                this.AddLabel(249, 135, 2036, String.Format("{0} : {1}", ingredients[1].m_Name, ingredients[1].m_AmountRequired));
            }
            if (ingredients.Length > 2)
            {
                this.AddItem(190, 189, ingredients[2].m_ItemId);
                this.AddLabel(249, 189, 2036, String.Format("{0} : {1}", ingredients[2].m_Name, ingredients[2].m_AmountRequired));
            }
            if (ingredients.Length > 3) // TRANSFORMATION DUST!
            {
                this.AddItem(190, 275, ingredients[3].m_ItemId);
                this.AddLabel(249, 275, 2036, String.Format("{0} : {1}", ingredients[3].m_Name, ingredients[3].m_AmountRequired));
            }

            // transform
            this.AddLabel(345, 318, 2036, "Criar");
            this.AddButton(404, 317, 4005, 4007, (int)Buttons.ButtonCraft, GumpButtonType.Reply, 0);


            // required skills 
            this.AddLabel(158, 295, 53, "Skill Necessaria:");
            if (the_rare.m_FirstRequiredSkill.Length > 0)
            {
                int hue = the_rare.MeetsRequiredSkillLevel_1(m_From) ? 0x44 : 1643; // green or red
                this.AddLabel(180, 318, 2036, the_rare.m_FirstRequiredSkill);
                this.AddLabel(268, 318, hue, "100.0");
            }
            if (the_rare.m_SecondRequiredSkill.Length > 0)
            {
                int hue = the_rare.MeetsRequiredSkillLevel_2(m_From) ? 0x44 : 1643; // green or red
                this.AddLabel(180, 336, 2036, the_rare.m_SecondRequiredSkill);
                this.AddLabel(268, 336, hue, "100.0");
            }
        }

        public enum Buttons
        {
            ButtonCraft = 1,
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.ButtonCraft)
            {
                List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);
                ICraftableRare the_rare = craftables[m_ItemIndex];
                if (!the_rare.MeetsRequiredSkillLevel_1(state.Mobile) && the_rare.MeetsRequiredSkillLevel_2(state.Mobile))
                {
                    state.Mobile.SendMessage("Voce nao tem habilidade suficiente");
                    return;
                }

                RaresCraftingSystem.TryCreateItem(state.Mobile, the_rare);
            }
            state.Mobile.SendGump(new RaresCraftingGump(m_From, m_Category));
        }

    }
}
