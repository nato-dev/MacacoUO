using System;
using Server.Items;

namespace Server.Mobiles 
{ 
    [CorpseName("an archmage corpse")] 
    public class BrigandCannibalMage : EvilMage
    { 
        [Constructable] 
        public BrigandCannibalMage()
            : base()
        {
            Title = "o mago bandoleiro";

            SetStr(68, 95);
            SetDex(81, 95);
            SetInt(110, 115);

            SetHits(70, 120);
            SetMana(552, 553);

            SetDamage(10, 23);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Hiding, 100, 100);
            SetSkill(SkillName.Stealth, 100, 100);
            SetSkill(SkillName.MagicResist, 96.9, 96.9);
            SetSkill(SkillName.Tactics, 94.0, 94.0);
            SetSkill(SkillName.Wrestling, 54.3, 54.3);
            SetSkill(SkillName.Necromancy, 94.0, 94.0);
            SetSkill(SkillName.SpiritSpeak, 54.3, 54.3);

            Fame = 14500;
            Karma = -14500;
            
            if (Utility.RandomDouble() < 0.75)
            {
                PackItem(new SeveredHumanEars());
            }

            AI = AIType.AI_NecroMage;
            SetSpecialAbility(SpecialAbility.LifeLeech);
        }

        public override bool CanStealth { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();
            if (!this.Hidden && this.Combatant == null)
            {
                this.AllowedStealthSteps = 999;
                this.Hidden = true;
                this.IsStealthing = true;
            }
        }

        public override void OnRevealed()
        {
            base.OnRevealed();
            PublicOverheadMessage(Network.MessageType.Emote, 0, false, "* saiu do esconderijo *");
        }


        public BrigandCannibalMage(Serial serial)
            : base(serial)
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
        }
    }
}
