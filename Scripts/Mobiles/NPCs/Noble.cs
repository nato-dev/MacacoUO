using System;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Mobiles
{
    public class Noble : TownEscortable
    {
        [Constructable]
        public Noble()
        {
            this.Title = "o nobre";

            this.SetSkill(SkillName.Parry, 80.0, 100.0);
            this.SetSkill(SkillName.Swords, 80.0, 100.0);
            this.SetSkill(SkillName.Tactics, 80.0, 100.0);
        }

        public Noble(Serial serial)
            : base(serial)
        {
        }
        // Do not display 'the noble' when single-clicking
        public override void InitOutfit()
        {
            if (this.Female)
                this.AddItem(new FancyDress());
            else
                this.AddItem(new FancyShirt(GetRandomHue()));

            int lowHue = GetRandomHue();

            this.AddItem(new ShortPants(lowHue));

            if (this.Female)
                this.AddItem(new ThighBoots(lowHue));
            else
                this.AddItem(new Boots(lowHue));

            if (!this.Female)
                this.AddItem(new BodySash(lowHue));

            this.AddItem(new Cloak(GetRandomHue()));

            if (!this.Female)
                this.AddItem(new Longsword());

            Utility.AssignRandomHair(this);

            this.PackGold(200, 250);
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

        private static int GetRandomHue()
        {
            switch ( Utility.Random(6) )
            {
                default:
                case 0:
                    return 0;
                case 1:
                    return Utility.RandomBlueHue();
                case 2:
                    return Utility.RandomGreenHue();
                case 3:
                    return Utility.RandomRedHue();
                case 4:
                    return Utility.RandomYellowHue();
                case 5:
                    return Utility.RandomNeutralHue();
            }
        }
    }
}
