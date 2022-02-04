using System;
using Server;

namespace Server.Items
{



    public class DragonSpellbook : Spellbook
    {
        [Constructable]
        public DragonSpellbook()
        {
            this.Slayer = SlayerName.Dragoes;
            this.Hue = TintaPreta.COR;
        }

        public DragonSpellbook(Serial serial) : base(serial)
        {
            this.Name = "Livro Draconico";
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    [FlipableAttribute(0x2234, 0x2235)]
    public class DragonHead : Item
    {
        [Constructable]
        public DragonHead(int hue) : base(0x2234)
        {
            Hue = hue;
            Stackable = true;
            this.Name = "Cabeca de Dragao";
        }

        [Constructable]
        public DragonHead() : this(0)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Apenas uma cabeca de dragao, talvez possa ser usada por Jewelcrafting para criar uma joia");
            base.OnDoubleClick(from);
        }

        public DragonHead(Serial serial) : base(serial)
        {
           
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    public class DragonStone : Item
    {
        [Constructable]
        public DragonStone() : base(12695)
        {
            Hue = 38;
            Stackable = true;
            this.Name = "Pedra do Dragao";
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Isto pode ser usado por escribas para fazer um livro de magias draconico.");
            base.OnDoubleClick(from);
        }

        public DragonStone(Serial serial) : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
