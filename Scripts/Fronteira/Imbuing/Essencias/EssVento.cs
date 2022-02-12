using System;

namespace Server.Items
{
    public class EssenciaVento : BaseEssencia, ICommodity
    {
        public override ElementoPvM Elemento { get { return ElementoPvM.Vento; } }

        [Constructable]
        public EssenciaVento()
            : this(1)
        {
        }

        [Constructable]
        public EssenciaVento(int amount)
            : base(0x571C)
        {
            Stackable = true;
            Amount = amount;
		
        }

        public EssenciaVento(Serial serial)
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
