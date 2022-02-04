using System;
using Server.Mobiles;

namespace Server.Items
{
    public class BlackrockMoonstone : Item
    {
 
        [Constructable]
        public BlackrockMoonstone()
            : base(0x9CAA)
        {
            Name = "Pedra da Lua Negra";
            Hue = 1175;
        }

        public BlackrockMoonstone(Serial serial)
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
