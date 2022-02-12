using System;

namespace Server.Items
{
    public class EssenciaAgua : BaseEssencia, ICommodity
    {
        public override ElementoPvM Elemento { get { return ElementoPvM.Agua; } }

        [Constructable]
        public EssenciaAgua()
            : this(1)
        {
        }

        [Constructable]
        public EssenciaAgua(int amount)
            : base(0x571C)
        {
            Stackable = true;
            Amount = amount;
        }

        public EssenciaAgua(Serial serial)
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
