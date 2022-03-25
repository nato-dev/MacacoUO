using Server.Items;
using Server.Menus.Questions;
using System;

namespace Server.Mobiles
{
    [CorpseName("an ophidian corpse")]
    [TypeAlias("Server.Mobiles.OphidianJusticar", "Server.Mobiles.OphidianZealot")]
    public class OphidianArchmage : BaseCreature
    {

        public static void Converte(BaseCreature b)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
            {
                if (b == null || !b.Alive || b.Deleted || !StuckMenu.IsInSecondAgeArea(b))
                    return;

                b.HitsMaxSeed += 800;
                b.Hits += 800;
                b.VirtualArmor += 60;
                b.Fame *= 2;
                foreach(var skill in b.Skills)
                {
                    if (skill.Base > 30 && skill.Base < 120)
                        skill.Base = 120;
                }
                b.PackItem(new Gold(Utility.Random(300, 300)));
                b.DamageMin = (int)(b.DamageMin * 1.5);
                b.DamageMax = (int)(b.DamageMax * 1.5);
                if (Utility.RandomDouble() < 0.05)
                    b.AddItem(BaseEssencia.RandomEssencia());
            });
        }

        [Constructable]
        public OphidianArchmage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "arquimago ophidiano";
            this.Body = 85;
            this.BaseSoundID = 639;

            this.SetStr(281, 305);
            this.SetDex(191, 215);
            this.SetInt(226, 250);

            this.SetHits(169, 183);
            this.SetStam(36, 45);

            this.SetDamage(5, 10);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 40, 45);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 35, 40);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            this.SetSkill(SkillName.EvalInt, 95.1, 100.0);
            this.SetSkill(SkillName.Magery, 95.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 75.0, 97.5);
            this.SetSkill(SkillName.Tactics, 65.0, 87.5);
            this.SetSkill(SkillName.Wrestling, 20.2, 60.0);

            this.Fame = 11500;
            this.Karma = -11500;

            this.VirtualArmor = 44;

            this.PackReg(5, 15);
            this.PackNecroReg(5, 15);
            OphidianArchmage.Converte(this);
        }

        public OphidianArchmage(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 1;
            }
        }

        public override TribeType Tribe { get { return TribeType.Ofidiano; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.TerathansAndOphidians;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV4);
            this.AddLoot(LootPack.MedScrolls, 2);
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
