using System;

namespace Server.Items
{
    public class GlacialStaff : BlackStaff
	{
        [Constructable]
        public GlacialStaff()
        {
            Hue = 0x480;
            Name = "Cajado Negro Colorido";
        }

        public GlacialStaff(Serial serial)
            : base(serial)
        {
        }

        //TODO: Pre-AoS stuff
        public override int LabelNumber
        {
            get
            {
                return 1017413;
            }
        }// Glacial Staff
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
