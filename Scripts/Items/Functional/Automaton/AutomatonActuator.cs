using System;
using Server.Mobiles;

namespace Server.Items
{
    public class AutomatonActuator : Item
    {
        [Constructable]
        public AutomatonActuator()
            : base(0x9CE9)
        {
            Name = "Forno Mecanico";
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Quem sabe voce possa usar isto para fabricar joias, tente usar uma ferramenta de joalheiro com isto");
            base.OnDoubleClick(from);
        }

        public AutomatonActuator(Serial serial)
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
