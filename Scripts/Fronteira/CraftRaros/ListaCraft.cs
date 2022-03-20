using Server.Commands;
using Server.Custom.RaresCrafting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.CraftRaros
{
    public class ListaCraft
    {
        public static void Initialize()
        {
            CommandSystem.Register("rctest", AccessLevel.Player, new CommandEventHandler(RaresCraftingSystem.ShowRareCraftGump));

            RaresCraftingSystem.Random = new List<ICraftableRare>()
            {
                RareDefinitions.DecoRandom(),
                RareDefinitions.DecoRandom2(),
            };

            RaresCraftingSystem.AlchemyCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.AlchemyFlask1(),
                RareDefinitions.AlchemyFlask2(),
                RareDefinitions.AlchemyFlask3(),
                RareDefinitions.Build(SkillName.Alchemy, "confetes", 0x9F89, 10, new Ingr(11699, 100, "fragmento de reliquia"),  new Ingr(3972, 1000, "garlic")),
                RareDefinitions.Build(SkillName.Alchemy, "frasco elegante", 0xA1DA, 4, new Ingr(0x182C, 1, "frasco"),  new Ingr(4586, 300, "areia")),
                RareDefinitions.Build(SkillName.Alchemy, "frasco elegante", 0xA1E7, 4, new Ingr(0x182C, 1, "frasco"),  new Ingr(4586, 300, "areia")),
                RareDefinitions.Build(SkillName.Alchemy, "reliquia alquimista", 0xA1DC, 4, new Ingr(0x1BF2, 500, "lingotes de ferro"),  new Ingr(4586, 300, "areia")),
            };

            RaresCraftingSystem.BowcraftingCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.BundleOfArrows(),
                RareDefinitions.BundleOfBolts(),
                RareDefinitions.DecorativeBowAndArrows(),
            };

            RaresCraftingSystem.BlacksmithingCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.DecorativeHalberd(),
                RareDefinitions.HangingChainmailLeggings(),
                RareDefinitions.GoldIngots(),
                RareDefinitions.CopperIngots(),
            };

            RaresCraftingSystem.CarpentryCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.DartboardWithAxe(),
                RareDefinitions.RuinedBookcase(),
                RareDefinitions.CoveredChair(),
                RareDefinitions.LogPileLarge()
            };

            RaresCraftingSystem.CookingCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.PotOfWax(),
                RareDefinitions.KettleOfWax(),
                RareDefinitions.DirtyPan(),
            };

            RaresCraftingSystem.InscriptionCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.BookPile1(),
                RareDefinitions.BookPile2(),
                RareDefinitions.DamagedBooks(),
                RareDefinitions.ForbiddenWritings()
            };

            RaresCraftingSystem.TailoringCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.LargeFishingNet(),
                RareDefinitions.DyeableCurtainEast(),
                RareDefinitions.DyeableCurtainSouth(),
            };

            RaresCraftingSystem.TinkeringCraftables = new List<ICraftableRare>()
            {
                RareDefinitions.Anchor(),
                RareDefinitions.HangingSkeleton1(),
                RareDefinitions.Hook(),
                RareDefinitions.HangingCauldron(),
            };
        }
    }
}
