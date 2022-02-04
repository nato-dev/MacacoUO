using System;

namespace Server.Items
{
    public class BagOfBlackPearl : Bag
    {
        [Constructable]
        public BagOfBlackPearl()
            : this(50)
        {
            this.Name = "Sacola de reagentes para magia";
        }

        [Constructable]
        public BagOfBlackPearl(int amount)
        {
            this.DropItem(new BlackPearl(amount));
            this.DropItem(new Bloodmoss(amount));
            this.DropItem(new Garlic(amount));
            this.DropItem(new Ginseng(amount));
            this.DropItem(new MandrakeRoot(amount));
            this.DropItem(new Nightshade(amount));
            this.DropItem(new SulfurousAsh(amount));
            this.DropItem(new SpidersSilk(amount));
        }

        public BagOfBlackPearl(Serial serial)
            : base(serial)
        {
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
