using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a poison elementals corpse")]
    public class PoisonElemental : BaseCreature
    {
        [Constructable]
        public PoisonElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental do veneno";
            this.Body = 162;
            this.BaseSoundID = 263;

            this.SetStr(80, 88);
            this.SetDex(80, 88);
            this.SetInt(80, 88);

            this.SetHits(50, 80);

            this.SetDamage(12, 18);

            this.SetDamageType(ResistanceType.Physical, 10);
            this.SetDamageType(ResistanceType.Poison, 90);

            this.SetResistance(ResistanceType.Physical, 60, 70);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 100);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 80.1, 95.0);
            this.SetSkill(SkillName.Magery, 40, 60);
            this.SetSkill(SkillName.Meditation, 80.2, 120.0);
            this.SetSkill(SkillName.Poisoning, 40, 60);
            this.SetSkill(SkillName.MagicResist, 85.2, 115.0);
            this.SetSkill(SkillName.Tactics, 80.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 70.1, 90.0);

            this.Fame = 12500;
            this.Karma = -12500;

            this.VirtualArmor = 70;

            this.PackItem(new Nightshade(15));
            this.PackItem(new LesserPoisonPotion());
        }

        public PoisonElemental(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override Poison HitPoison
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override double HitPoisonChance
        {
            get
            {
                return 0.75;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 5;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV4);
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
        }
    }
}
