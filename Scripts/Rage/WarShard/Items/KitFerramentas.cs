using System;

namespace Server.Items
{
    public class KitFerramentas : Backpack
    {
        [Constructable]
        public KitFerramentas()
            : this(50)
        {
        }

        [Constructable]
        public KitFerramentas(int amount)
        {
            this.Name = "Kit Ferramentas de Trabalho";
            this.Hue = 100;
            this.DropItem(new SmithyHammer());
            this.DropItem(new SewingKit(amount));
            this.DropItem(new Pickaxe());
            this.DropItem(new Axe());
            this.DropItem(new Hammer(amount));
            this.DropItem(new MortarPestle(amount));
            this.DropItem(new TinkerTools(amount));
            this.DropItem(new FletcherTools(amount));
            this.DropItem(new MapmakersPen(amount));
            this.DropItem(new Skillet(amount));
            this.DropItem(new ScribesPen(amount));
            this.DropItem(new Scissors());
        }

        public KitFerramentas(Serial serial)
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
