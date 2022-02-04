using System;

namespace Server.Items
{
    public class BraceletOfResilience : GoldBracelet
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public BraceletOfResilience()
        {

        }

        public BraceletOfResilience(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1077627;
            }
        }// Bracelet of Resilience
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
