namespace Server.Items
{
    public class PedraElementalSuprema : Item
    {
        public override int LabelNumber { get { return 1151811; } } // Whetstone of Enervation

        [Constructable]
        public PedraElementalSuprema()
            : this(1)
        {
        }

        [Constructable]
        public PedraElementalSuprema(int amount) : base(0x1368)
        {
            this.Hue = 2611;
            this.Weight = 10;
            Name = "Pedra Elemental Suprema";
            Stackable = true;
            Amount = amount;
            Stackable = false;
        }

        public PedraElementalSuprema(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Voce sente uma energia muito forte vindo desta pedra...");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (ItemID != 0x1368)
                ItemID = 0x1368;
        }
    }
}
