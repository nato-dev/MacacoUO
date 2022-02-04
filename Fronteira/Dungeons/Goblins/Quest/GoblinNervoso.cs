using Server.Engines.Quests;
using Server.Items;
using System;

namespace Server.Ziden.Quests.Engenheiro
{
    public class GoblinNervoso : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] {  typeof(MatarBoss) }; } }

        [Constructable]
        public GoblinNervoso()
            : base("Goblin", "")
        {
            Body = 723;
            BaseSoundID = 0x600;
            this.SetSkill(SkillName.Tinkering, 100);
            this.SetSkill(SkillName.Blacksmith, 100);
            this.SetSkill(SkillName.Carpentry, 100);
        }

        public GoblinNervoso(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("* parece nervoso porem nao agressivo *");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
            this.CantWalk = true;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            switch (Utility.Random(3))
            {
                case 0:
                    SetWearable(new FancyShirt(GetRandomHue()));
                    break;
                case 1:
                    SetWearable(new Doublet(GetRandomHue()));
                    break;
                case 2:
                    SetWearable(new Shirt(GetRandomHue()));
                    break;
            }

            SetWearable(new Shoes(GetShoeHue()));


            int hairHue = GetHairHue();

            Utility.AssignRandomHair(this, hairHue);
            Utility.AssignRandomFacialHair(this, hairHue);

            if (Body == 0x191)
            {
                FacialHairItemID = 0;
            }

            if (Body == 0x191)
            {
                switch (Utility.Random(6))
                {
                    case 0:
                        SetWearable(new ShortPants(GetRandomHue()));
                        break;
                    case 1:
                    case 2:
                        SetWearable(new Kilt(GetRandomHue()));
                        break;
                    case 3:
                    case 4:
                    case 5:
                        SetWearable(new Skirt(GetRandomHue()));
                        break;
                }
            }
            else
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        SetWearable(new LongPants(GetRandomHue()));
                        break;
                    case 1:
                        SetWearable(new ShortPants(GetRandomHue()));
                        break;
                }
            }

            if (!Siege.SiegeShard)
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
