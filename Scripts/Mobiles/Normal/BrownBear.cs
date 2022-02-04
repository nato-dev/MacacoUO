using System;

namespace Server.Mobiles
{
    [CorpseName("a bear corpse")]
    public class BrownBear : BaseCreature
    {
        [Constructable]
        public BrownBear()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "um urso marrom";
            this.Body = 167;
            this.BaseSoundID = 0xA3;

            this.SetStr(76, 100);
            this.SetDex(26, 45);
            this.SetInt(23, 47);

            this.SetHits(46, 60);
            this.SetMana(0);

            this.SetDamage(10, 27);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 20, 30);
            this.SetResistance(ResistanceType.Cold, 15, 20);
            this.SetResistance(ResistanceType.Poison, 10, 15);

            this.SetSkill(SkillName.MagicResist, 25.1, 35.0);
            this.SetSkill(SkillName.Tactics, 40.1, 60.0);
            this.SetSkill(SkillName.Wrestling, 40.1, 60.0);

            this.Fame = 450;
            this.Karma = 0;

            this.VirtualArmor = 24;

            this.Tamable = true;
            this.ControlSlots = 2;
            this.MinTameSkill = 51.1;

            SetSpecialAbility(SpecialAbility.Rage);
        }

        public BrownBear(Serial serial)
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
        public override int Hides
        {
            get
            {
                return 20;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Bear;
            }
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
