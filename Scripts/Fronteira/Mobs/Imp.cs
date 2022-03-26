using System;
using Server.Engines.Craft;
using Server.Items;
using Server.Menus.Questions;
using System;

namespace Server.Mobiles
{
    [CorpseName("an imp corpse")]
    public class Imp : BaseCreature
    {
        public static void Converte(BaseCreature b, int hue = 1175)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
            {
                if (b == null || !b.Alive || b.Deleted || !StuckMenu.IsInSecondAgeArea(b))
                    return;

                if (Utility.RandomDouble() < 0.05)
                    b.AddItem(BaseEssencia.RandomEssencia());

                if (b is BaseOrc || b is BaseRatman)
                {
                    b.Name += " albino";
                    b.Hue = 1153;
                    if (b is BaseOrc && Utility.RandomBool())
                        b.PackItem(new EnhancedBandage());
                }
                else if (b is Gazer)
                {
                    if(Utility.RandomBool())
                        b.PackItem(new RecipeScroll((int)SmithRecipes.TalismanElemental));
                    b.Name += " do sol";
                    b.Hue = 1161;
                    b.Skills.Magery.Base = 180;
                }
                else
                {
                    b.Name += " das terras perdidas";
                    b.Hue = hue;
                }



                b.HitsMaxSeed += 1000;
                b.Hits += 1000;
                b.VirtualArmor += 60;
                b.Fame *= 6;
                foreach (var skill in b.Skills)
                {
                    if (skill.Base > 30 && skill.Base < 100)
                        skill.Base = 100;
                }

                if(Utility.RandomBool())
                    b.AddToBackpack(new CristalDoPoder());

                if (Utility.RandomDouble() < 0.1)
                    b.AddToBackpack(DefCookingExp.GetReceitaRandom());

                var gold = b.Backpack.FindItemByType(typeof(Gold));
                if(gold != null)
                {
                    gold.Amount += 100;
                }

                if (Utility.RandomDouble() < 0.01)
                {
                    b.PackItem(DefJewelcrafting.GetReceitaPower());
                }
                b.DamageMin = (int)(b.DamageMin * 1.5);
                b.DamageMax = (int)(b.DamageMax * 2);
                if (b.DamageMax > 65)
                    b.DamageMax = 65;
            });
        }

        [Constructable]
        public Imp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "garguleta";
            Body = 74;
            BaseSoundID = 422;

            SetStr(91, 115);
            SetDex(61, 80);
            SetInt(86, 105);

            SetHits(55, 70);

            SetDamage(10, 14);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.EvalInt, 20.1, 30.0);
            SetSkill(SkillName.Magery, 60, 80);
            SetSkill(SkillName.MagicResist, 30.1, 50.0);
            SetSkill(SkillName.Tactics, 42.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 44.0);
            SetSkill(SkillName.Necromancy, 20);
            SetSkill(SkillName.SpiritSpeak, 20);

            Fame = 2500;
            Karma = -2500;
            if(Shard.NECRO)
            {
                switch (Utility.Random(354))
                {
                    case 0: PackItem(new BloodOathScroll()); break;
                    case 1: PackItem(new CorpseSkinScroll()); break;
                    case 2: PackItem(new CurseWeaponScroll()); break;
                    case 3: PackItem(new EvilOmenScroll()); break;
                    case 4: PackItem(new HorrificBeastScroll()); break;
                    case 5: PackItem(new LichFormScroll()); break;
                    case 6: PackItem(new MindRotScroll()); break;
                    case 7: PackItem(new PainSpikeScroll()); break;
                    case 8: PackItem(new PoisonStrikeScroll()); break;
                    case 9: PackItem(new StrangleScroll()); break;
                    case 10: PackItem(new SummonFamiliarScroll()); break;
                    case 11: PackItem(new WitherScroll()); break;
                    case 12: PackItem(new WraithFormScroll()); break;
                }
            }

            VirtualArmor = 30;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 83.1;
            Converte(this);
        }

        public Imp(Serial serial)
            : base(serial)
        {
        }

        public override bool IsSmart { get { return false; } }

        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override int Hides
        {
            get
            {
                return 6;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Spined;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Daemon;
            }
        }
        public override bool CanFly
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV2);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
