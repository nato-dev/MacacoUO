using System;

namespace Server.Items 
{ 
    public class BagOfingots : Bag 
    { 
        [Constructable] 
        public BagOfingots()
            : this(5000)
        { 
        }

        [Constructable] 
        public BagOfingots(int amount) 
        { 
            this.DropItem(new BeriloIngot(amount)); 
            this.DropItem(new VibraniumIngot(amount)); 
            this.DropItem(new CopperIngot(amount)); 
            this.DropItem(new BronzeIngot(amount)); 
            this.DropItem(new SilverIngot(amount)); 
            this.DropItem(new NiobioIngot(amount)); 
            this.DropItem(new LazuritaIngot(amount)); 
            this.DropItem(new QuartzoIngot(amount)); 
            this.DropItem(new IronIngot(amount));
            this.DropItem(new Tongs());
            this.DropItem(new TinkerTools());
        }

        public BagOfingots(Serial serial)
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