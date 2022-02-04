using Server.Engines.Quests;
using Server.Items;
using System;

namespace Server.Ziden.Quests.Engenheiro
{
    public class SargentoMarrento : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                   typeof(Engenheiro1)
        };
            }
        }


        [Constructable]
        public SargentoMarrento()
            : base("Farrur", "O Sargento Marrento")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public SargentoMarrento(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Hum... o que sera que aconteceu aqui ?");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
            this.Female = false;
            this.CantWalk = true;
            this.Race = Race.Human;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            SetWearable(new PlateArms());
            SetWearable(new PlateChest());
            SetWearable(new PlateHelm());
            SetWearable(new PlateGloves());
            SetWearable(new PlateLegs());
            var capa = new Cloak();
            capa.Hue = 32;
            SetWearable(capa);
            var barda = new Halberd();
            barda.Resource = CraftResource.Adamantium;
            barda.Quality = ItemQuality.Exceptional;
            SetWearable(barda);
            int hairHue = GetHairHue();
            Utility.AssignRandomHair(this, hairHue);
            Utility.AssignRandomFacialHair(this, hairHue);

            if (Body == 0x191)
            {
                FacialHairItemID = 0;
            }

         
            PackGold(100, 200);
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
