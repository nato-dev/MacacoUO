using System;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a lizardman corpse")]
    public class LizardmanDefender : BaseCreature
    {
        [Constructable]
        public LizardmanDefender()
            : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = NameList.RandomName("lizardman");
            this.Title = "o  defensor";
            this.Body = Utility.RandomList(35, 36);
            this.BaseSoundID = 417;
            this.Hue = 1949;

            SetStr(157, 180);
            SetDex(105, 108);
            SetInt(50, 57);

            SetHits(300, 400);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);

            SetSkill(SkillName.Parry, 65.1, 90.0);
            SetSkill(SkillName.MagicResist, 65.1, 90.0);
            SetSkill(SkillName.Tactics, 95.1, 120.0);
            SetSkill(SkillName.Wrestling, 95.1, 120.0);

            Fame = 11000;
            Karma = -11000;

            VirtualArmor = 90;
        }

        public LizardmanDefender(Serial serial)
            : base(serial)
        {
        }

        public override InhumanSpeech SpeechType { get { return InhumanSpeech.Lizardman; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Spined; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 4);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);
            Orc.TentaAtacarMaster(this, from);
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
