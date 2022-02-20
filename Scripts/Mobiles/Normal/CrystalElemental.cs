using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crystal elemental corpse")]
    public class CrystalElemental : BaseCreature
    {
        [Constructable]
        public CrystalElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "elemental de cristal";
            Body = 300;
            BaseSoundID = 278;

            SetStr(136, 160);
            SetDex(51, 65);
            SetInt(86, 110);

            SetHits(1000);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 55, 70);

            SetSkill(SkillName.EvalInt, 70.1, 75.0);
            SetSkill(SkillName.Magery, 100, 100);
            SetSkill(SkillName.Meditation, 65.1, 75.0);
            SetSkill(SkillName.Parry, 50, 50);
            SetSkill(SkillName.MagicResist, 0, 0);
            SetSkill(SkillName.Tactics, 75.1, 85.0);
            SetSkill(SkillName.Wrestling, 65.1, 75.0);

            Fame = 6500;
            Karma = -6500;

            VirtualArmor = 100;
            AddItem(new CristalElemental());
            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
            SetSpecialAbility(SpecialAbility.StealLife);
        }

        public CrystalElemental(Serial serial)
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
        public override int TreasureMapLevel
        {
            get
            {
                return 1;
            }
        }

        public override void GenerateLoot()
        {
           
            AddLoot(LootPack.LV4);
            AddLoot(LootPack.LV3);
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
