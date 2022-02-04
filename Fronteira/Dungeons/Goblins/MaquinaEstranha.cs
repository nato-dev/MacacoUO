namespace Server.Items
{
    public class MaquinaEstranha : Item
    {
        [Constructable]
        public MaquinaEstranha()
            : base(0x9CE9)
        {
            Name = "Maquina Estranha";
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Parece estar faltando uma engrenagem...");
            base.OnDoubleClick(from);
        }

        public MaquinaEstranha(Serial serial)
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
