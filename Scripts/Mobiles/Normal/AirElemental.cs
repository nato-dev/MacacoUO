using System;
using Server.Items;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Spells.Sixth;

namespace Server.Mobiles
{
    [CorpseName("an air elemental corpse")]
    public class AirElemental : BaseCreature
    {
        public override Spell ChooseSpell()
        {
            if (Utility.RandomBool())
            {
                return new LightningSpell(this, null);
            }
            return null;
        }

        [Constructable]
        public AirElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental do ar";
            this.Body = 13;
            this.Hue = 0x4001;
            this.BaseSoundID = 655;

            this.SetStr(126, 155);
            this.SetDex(166, 185);
            this.SetInt(101, 125);

            this.SetHits(76, 93);

            this.SetDamage(8, 10);

            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Cold, 40);
            this.SetDamageType(ResistanceType.Energy, 40);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            this.SetSkill(SkillName.EvalInt, 0, 10);
            this.SetSkill(SkillName.Magery, 60.1, 75.0);
            this.SetSkill(SkillName.MagicResist, 60.1, 75.0);
            this.SetSkill(SkillName.Tactics, 60.1, 80.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 80.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 40;

			switch (Utility.Random(24))
            {
                case 0: PackItem(new PainSpikeScroll()); break;
                case 1: PackItem(new PoisonStrikeScroll()); break;
                case 2: PackItem(new StrangleScroll()); break;
                case 3: PackItem(new VengefulSpiritScroll()); break;
			}

            this.ControlSlots = 2;
        }

        public AirElemental(Serial serial)
            : base(serial)
        {
        }

        public override double DispelDifficulty
        {
            get
            {
                return 167.5;
            }
        }
        public override double DispelFocus
        {
            get
            {
                return 45.0;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV3);
            this.AddLoot(LootPack.LV2);
            this.AddLoot(LootPack.LowScrolls);
            this.AddLoot(LootPack.MedScrolls);
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

            if (this.BaseSoundID == 263)
                this.BaseSoundID = 655;
        }
    }
}
