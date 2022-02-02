using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a goblin corpse")]
    public class GreenGoblin2 : BaseCreature
    {
        [Constructable]
        public GreenGoblin2()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "goblin";
            Body = 723;
            BaseSoundID = 0x600;

            SetStr(252, 343);
            SetDex(60, 74);
            SetInt(117, 148);

            SetHits(70, 90);
            SetStam(60, 74);
            SetMana(117, 148);

            SetDamage(6, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 39);
            SetResistance(ResistanceType.Cold, 27, 35);
            SetResistance(ResistanceType.Poison, 11, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 120.5, 128.8);
            SetSkill(SkillName.Tactics, 80.9, 89.9);
            SetSkill(SkillName.Anatomy, 83.1, 89.6);
            SetSkill(SkillName.Wrestling, 93.0, 108.3);

            Fame = 700;
            Karma = -700;

            VirtualArmor = 28;

            switch ( Utility.Random(20) )
            {
                case 0:
                    PackItem(new Scimitar());
                    break;
                case 1:
                    PackItem(new Katana());
                    break;
                case 2:
                    PackItem(new WarMace());
                    break;
                case 3:
                    PackItem(new WarHammer());
                    break;
                case 4:
                    PackItem(new Kryss());
                    break;
                case 5:
                    PackItem(new Pitchfork());
                    break;
            }

            PackItem(new ThighBoots());

            switch ( Utility.Random(3) )
            {
                case 0:
                    PackItem(new Ribs());
                    break;
                case 1:
                    PackItem(new Shaft());
                    break;
                case 2:
                    PackItem(new Candle());
                    break;
            }

            if (0.1 > Utility.RandomDouble())
                PackItem(new MechanicalComponent());
        }

        public GreenGoblin2(Serial serial)
            : base(serial)
        {
        }

        public override int GetAngerSound() { return 0x600; }
        public override int GetIdleSound() { return 0x600; }
        public override int GetAttackSound() { return 0x5FD; }
        public override int GetHurtSound() { return 0x5FF; }
        public override int GetDeathSound() { return 0x5FE; }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 1; } }
        public override int Meat { get { return 1; } }
        public override TribeType Tribe { get { return TribeType.GoblinVerde; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV3);
            this.AddPackedLoot(LootPack.MeagerProvisions, typeof(Backpack));
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
