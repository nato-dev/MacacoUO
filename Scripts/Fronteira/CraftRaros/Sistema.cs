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

}
