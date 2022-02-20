using System;

namespace Server.Items
{
    public class CrystallineRing : GoldRing
	{
		public override int LabelNumber { get { return 1075096; } } // Crystalline Ring
		public override bool IsArtifact { get { return true; } }
		
        [Constructable]
        public CrystallineRing()
        {
            Hue = 0x480;			
            SkillBonuses.SetValues(0, SkillName.Magery, 2);
        }

        public CrystallineRing(Serial serial)
            : base(serial)
        {
        }
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
