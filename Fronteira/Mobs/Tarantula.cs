using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a tarantula corpse")]
    public class TarantulaMount : BaseMount
    {
        private static readonly Hashtable m_Table = new Hashtable();

        public override Habilidade GetWeaponAbility()
        {
            return Habilidade.BleedAttack;
        }


        [Constructable]
        public TarantulaMount()
            : this("tarantula")
        {
        }

        [Constructable]
        public TarantulaMount(string name) : base(name, 0x579, 0x3ECA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 389;

            SetStr(500, 555);
            SetDex(85, 125);
            SetInt(85, 165);

            SetHits(80, 90);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 55, 75);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 25, 45);

            SetSkill(SkillName.Anatomy, 0, 0);
            SetSkill(SkillName.MagicResist, 91.4, 101.4);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 97.3, 105.2);
            SetSkill(SkillName.Poisoning, 95.0, 120.0);
            SetSkill(SkillName.Parry, 95.0, 105.0);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 115.1;
        }

        public TarantulaMount(Serial serial)
            : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.FilthyRich, 4);
            this.AddLoot(LootPack.Gems, 4);
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override int Meat { get { return 16; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

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
