using System;

namespace Server.Items
{
    public class EssenciaLuz : BaseEssencia, ICommodity
    {
        public override ElementoPvM Elemento { get { return ElementoPvM.Luz; } }

        [Constructable]
        public EssenciaLuz()
            : this(1)
        {
        }

        [Constructable]
        public EssenciaLuz(int amount)
            : base(0x571C)
        {
            Stackable = true;
            Amount = amount;
        }

        public EssenciaLuz(Serial serial)
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
