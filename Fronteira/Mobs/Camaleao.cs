/**********************************************************
RunUO 2.0 AoV C# script file
Official Age of Valor Script :: www.uovalor.com
Last modified by 
Filepath: scripts\_custom\mobiles\chameleon\Chameleon.cs
Lines of code: 74
***********************************************************/


using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a chameleon corpse")]
    public class Chameleon : BaseCreature
    {
        [Constructable]
        public Chameleon() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "camaleao";
            Body = 0xCE;
            Hue = Utility.RandomList(1161);
            BaseSoundID = 0x5A;

            SetStr(126, 150);
            SetDex(56, 75);
            SetInt(11, 20);

            SetHits(100, 120);
            SetMana(0);

            SetDamage(2, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 55.1, 70.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);
            SetSkill(SkillName.DetectHidden, 160.0, 180.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            Tamable = false;

            SetSpecialAbility(SpecialAbility.GraspingClaw);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.OldRich);
            AddLoot(LootPack.Gems, 2);
        }

        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Horned; } }

        public Chameleon(Serial serial) : base(serial)
        {
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
            this.Delete();
        }
    }
}
