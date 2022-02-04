namespace Server.Items
{
    public class LancaGoblin : Spear
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public LancaGoblin()
        {
            Name = "Lanca Mecanica";
            Quality = ItemQuality.Exceptional;
            Hue = 0xA91;
            Attributes.BonusStr = 10;
        }

        public LancaGoblin(Serial serial) : base(serial)
        {
        }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

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
