using Server.Gumps;

namespace Server.Items
{
    public class PedraClasses : Item
    {
        [Constructable]
        public PedraClasses()
            : base(0xED4)
        {
            this.Name = "Pedra de Classes";
            this.Movable = false;
            this.Weight = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this, 8))
            {
                from.SendGump(new PedraClassesGump(from));
            }
            else
            {
                from.SendMessage(0x00FE, "Você está muito longe.");
            }
        }

        public PedraClasses(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
