using System;

namespace Server.Items
{
    public class EssenciaRaio : BaseEssencia, ICommodity
    {
        public override ElementoPvM Elemento { get { return ElementoPvM.Raio; } }

        [Constructable]
        public EssenciaRaio()
            : this(1)
        {
        }

        [Constructable]
        public EssenciaRaio(int amount)
            : base(0x571C)
        {
            Stackable = true;
            Amount = amount;
        }

        public EssenciaRaio(Serial serial)
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
