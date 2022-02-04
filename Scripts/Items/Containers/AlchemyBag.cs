using System;

namespace Server.Items
{
    public class AlchemyBag : Bag
    {
        [Constructable]
        public AlchemyBag()
            : this(1)
        {
            this.Movable = true;
            this.Hue = 0x250;
        }

        [Constructable]
        public AlchemyBag(int amount)
        {
            this.DropItem(new MortarPestle(3));
            this.DropItem(new BagOfReagents(30));
            this.DropItem(new Bottle(20));
        }

        public AlchemyBag(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Sacola do Alquimista";
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
