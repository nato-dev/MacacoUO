using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a gargoyle corpse")]
    public class Gargoyle : BaseCreature
    {
        [Constructable]
        public Gargoyle()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "gargula";
            this.Body = 4;
            this.BaseSoundID = 372;

            this.SetStr(146, 175);
            this.SetDex(76, 95);
            this.SetInt(81, 105);

            this.SetHits(288, 305);

            this.SetDamage(7, 14);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 30, 35);
            this.SetResistance(ResistanceType.Fire, 25, 35);
            this.SetResistance(ResistanceType.Cold, 5, 10);
            this.SetResistance(ResistanceType.Poison, 15, 25);

            this.SetSkill(SkillName.EvalInt, 70.1, 85.0);
            this.SetSkill(SkillName.Magery, 70.1, 85.0);
            this.SetSkill(SkillName.MagicResist, 70.1, 85.0);
            this.SetSkill(SkillName.Tactics, 50.1, 70.0);
            this.SetSkill(SkillName.Wrestling, 40.1, 80.0);

            this.Fame = 3500;
            this.Karma = -3500;

            this.VirtualArmor = 32;

            if (0.005 > Utility.RandomDouble())
                this.PackItem(new GargoylesPickaxe());

			switch (Utility.Random(6))
            {
                case 0: PackItem(new PainSpikeScroll()); break;
			}

        }

        public Gargoyle(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 1;
            }
        }
        public override int Meat
        {
            get
            {
                return 1;
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
            this.AddLoot(LootPack.LV3);
            this.AddLoot(LootPack.MedScrolls);
            this.AddLoot(LootPack.Gems, 1);
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
