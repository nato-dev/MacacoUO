using Server.Engines.Craft;
using Server.Scripts.Custom.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class Decos
    {
        public class RandomDecoChest : Bag
        {
            [Constructable]
            public RandomDecoChest()
            {
                Name = "Sacola de Decoracao";
                this.AddItem(Decos.RandomDeco());
            }

            public RandomDecoChest(Serial s)
             : base(s)
            {

            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)1); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();
            }
        }

        public static Type[] common_decos = new Type[] {

            typeof(GoblinTopiary), typeof(WaterBarrel),
            typeof(Obelisk), typeof(PaperLantern), typeof(CopperWire), typeof(IronWire), typeof(RoseInAVase), typeof(RuinedTapestry), typeof(RuinedDrawers), typeof(RuinedPainting), typeof(RuinedClock),
            typeof(Globe), typeof(EmptyJars), typeof(GrimWarning), typeof(HeatingStand), typeof(Lever), typeof(MeltedWax), typeof(RandomMonsterStatuette),
            typeof(Brazier), typeof(DecoBridleSouth), typeof(BrokenChair), typeof(Cards), typeof(Cards2), typeof(Cards3), typeof(DecoBrimstone),
            typeof(Cards4), typeof(Checkers), typeof(Chessboard), typeof(Countertop), typeof(CustomizableWallSign), typeof(MagicCrystalBall),
            typeof(DecoBlackmoor), typeof(DecoBloodspawn), typeof(DecoArrowShafts), typeof(DecoBottlesOfLiquor), typeof(DecoCards5), typeof(DecoDeckOfTarot),
            typeof(BurlyBoneDecor), typeof(DecoCrystalBall), typeof(DecoMagicalCrystal), typeof(DecoFlower), typeof(DecoFlower2),
            typeof(DecoGarlic), typeof(DecoGinseng), typeof(DecoGarlicBulb), typeof(DecoGinsengRoot), typeof(DecoGoldIngot), typeof(DecoGoldIngot2), typeof(DecoGoldIngots4),
            typeof(DecoHay), typeof(DecoHay2), typeof(DecoHorseDung), typeof(DecoPumice), typeof(DecorativeAxeNorth), typeof(DecorativeAxeWest), typeof(DecorativeBox),
            typeof(DecorativeDAxeNorth), typeof(DecorativePlant), typeof(DecorativePlantFlax), typeof(DecorativePlantLilypad), typeof(DecorativePlantPoppies),
            typeof(DecorativePlateKabuto), typeof(DecorativeShield1), typeof(DecorativeShield10), typeof(DecorativeShield11), typeof(DecorativeShield2), typeof(DecorativeShield3),
            typeof(DecorativeShield4), typeof(DecorativeShield5), typeof(DecorativeShield6), typeof(DecorativeShield7), typeof(DecorativeShield8), typeof(DecorativeShield9),
            typeof(DecorativeShieldSword1North), typeof(DecorativeShieldSword1West), typeof(DecorativeShieldSword2North), typeof(DecorativeShieldSword2West),
            typeof(DecorativeSwordNorth), typeof(DecorativeTopiary), typeof(DecorativeVines), typeof(DecoRock), typeof(DecoRock2), typeof(DecoRocks), typeof(DecoRocks2),
            typeof(DecoRoseOfTrinsic), typeof(DecoRoseOfTrinsic2), typeof(DecoRoseOfTrinsic3), typeof(DecoSilverIngot), typeof(DecoSilverIngots), typeof(DecoTarot6),
            typeof(DecoTarot5), typeof(DecoTarot3), typeof(DecoTray), typeof(DecoTray2), typeof(SnowPileDeco), typeof(CrystalSkull), typeof(CupidStatue), typeof(EnchantedWheelbarrow) ,typeof(DecorativeShardShield),
            typeof(HearthOfHomeFire), typeof(DecorativeDAxeNorth), typeof(DecorativeDAxeWest), typeof(ElvenAlchemyTable), typeof(EmptyJars), typeof(LargeEmptyPot),  typeof(SmallEmptyFlask),
            typeof(SkullsOnPike), typeof(Snowman), typeof(UltimaBanner), typeof(WallSconce), typeof(HitchingPost), typeof(ElvenPodium), 
            typeof(WallTorch), typeof(WritingDesk), typeof(WritingTable), typeof(BrazierTall), typeof(Tapestry1N), typeof(Tapestry2N),
            typeof(Tapestry2W), typeof(Tapestry3W),  typeof(Tapestry4W), typeof(Tapestry4N),  typeof(Tapestry5N), typeof(StackofIngots), typeof(StackofLogs), typeof(AncientArmor),
            typeof(CannonBall2), typeof(Gunpowder2), typeof(CannonFuse), typeof(CrackedCrystalBall), typeof(SeersPowder), typeof(EnchantedMarble),typeof(minostatueAddonDeed),
            typeof(SmallEmptyPot), typeof(EmptyToolKit), typeof(EmptyToolKit2), typeof(SmallFish), typeof(SnowGlobe), typeof(PottedPlant), typeof(PottedTree), typeof(PottedTree1),
            typeof(OrigamiPaper), typeof(LampPost1), typeof(LampPost2), typeof(LampPost3), typeof(LampPost3),
        };

        public static Item SuperRandomDeco()
        {
            try
            {
                switch (Utility.Random(12))
                {
                    case 0: return DecoRelPor.RandomArty();
                    case 1: return DecoLoader.RandomArty();
                }
                return _RandomDeco();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return SuperRandomDeco();
            }
        }

        public static Item RandomDeco()
        {
            return new ValeDecoracaoRara();
        }

        public static Item _RandomDeco()
        {
            
            var item = common_decos[Utility.Random(common_decos.Length)];
            var itemInstance = Activator.CreateInstance(item) as Item;
            itemInstance.Movable = true;
            itemInstance.LootType = LootType.Regular;
            return itemInstance;
        }

    }
}
