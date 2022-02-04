using Server.Engines.Craft;
using System;

namespace Server.Items
{
    public class Shaft : Item, ICommodity, ICraftable
    {
        [Constructable]
        public Shaft()
            : this(1)
        {
        }

        [Constructable]
        public Shaft(int amount)
            : base(0x1BD4)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public Shaft(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get
            {
                return 0.1;
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

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            this.Amount += (int)(from.Skills[SkillName.Fletching].Value / 3);
            return quality;
        }
    }
}
