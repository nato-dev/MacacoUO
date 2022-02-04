using System;

namespace Server.Items
{

    

    [Flipable(0x1055, 0x1056)]
    public class Hinge : Item
    {
        [Constructable]
        public Hinge()
            : this(1)
        {
        }

        [Constructable]
        public Hinge(int amount)
            : base(0x1055)
        {
            Name = "dobradiça";
            this.Stackable = true;
            this.Amount = amount;
            this.Weight = 1.0;
        }

        public Hinge(Serial serial)
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

    public class EstruturaDeLivro : Item
    {
        [Constructable]
        public EstruturaDeLivro()
            : base(0x1E22)
        {
            Name = "estrutura de livro";
            this.Stackable = false;
            this.Weight = 1.0;
        }

        public EstruturaDeLivro(Serial serial)
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
