using System;

namespace Server.Items
{
    public class EssenciaEscuridao : BaseEssencia, ICommodity
    {
        public override ElementoPvM Elemento { get { return ElementoPvM.Escuridao; } }

        [Constructable]
        public EssenciaEscuridao()
            : this(1)
        {
        }

        [Constructable]
        public EssenciaEscuridao(int amount)
            : base(0x571C)
        {
            Stackable = true;
            Amount = amount;
        }

        public EssenciaEscuridao(Serial serial)
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
