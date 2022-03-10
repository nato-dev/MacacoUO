using System;
using Server.Items;

namespace Server.Mobiles
{
    public class WanderingHealer : BaseHealer
    {
        [Constructable]
        public WanderingHealer()
        {
            this.Title = "o andarilho curandeiro";

            this.AddItem(new GnarledStaff());

            this.SetSkill(SkillName.Camping, 80.0, 100.0);
            this.SetSkill(SkillName.Forensics, 80.0, 100.0);
            this.SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);

            BaseHealer.healers.Add(this);

        }

        public WanderingHealer(Serial serial)
            : base(serial)
        {
        }

        public override bool CanTeach
        {
            get
            {
                return true;
            }
        }
        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }// Do not display title in OnSingleClick
        public override bool CheckTeach(SkillName skill, Mobile from)
        {
            if (!base.CheckTeach(skill, from))
                return false;

            return (skill == SkillName.Anatomy) ||
                   (skill == SkillName.Camping) ||
                   (skill == SkillName.Forensics) ||
                   (skill == SkillName.Healing) ||
                   (skill == SkillName.SpiritSpeak);
        }

        public override bool CheckResurrect(Mobile m)
        {
            if (m.Criminal)
            {
                this.Say(501222); // você é um criminoso. Eu não te ressuscitarei.
                return true;
            }
            else if (m.Murderer)
            {
                this.Say(501223); // Você não é uma pessoa decente e boa. Eu não te ressuscitarei.
                return true;
            }
            else if (m.Karma < 0)
            {
                this.Say(501224); // Você se desviou do caminho da virtude, mas ainda merece uma segunda chance.
            }

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
