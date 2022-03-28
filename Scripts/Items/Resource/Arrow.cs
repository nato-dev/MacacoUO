using Server.Engines.Craft;
using System;

namespace Server.Items
{
    public class Arrow : Item, ICommodity, ICraftable
    {
        [Constructable]
        public Arrow()
            : this(1)
        {
        }

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {

            if (from.Skills[SkillName.Bowcraft].Value >= 100)
            {
                this.Amount += 1;
            }

            if (from.Skills[SkillName.Bowcraft].Value >= 120)
            {
                this.Amount += 1;
            }

            return 0;
        }

        [Constructable]
        public Arrow(int amount)
            : base(0xF3F)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public Arrow(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get
            {
                return 0.08;
            }
        }
        TextDefinition ICommodity.Description
        {
            get
            {
                return this.LabelNumber;
            }
        }
        bool ICommodity.IsDeedable
        {
            get
            {
                return true;
            }
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
