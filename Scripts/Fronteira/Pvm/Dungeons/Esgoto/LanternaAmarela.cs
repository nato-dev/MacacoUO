using Server.Items;

namespace Server.Ziden.Dungeons.Esgoto
{
    public class LanternaMagica : Item
    {
        [Constructable]
        public LanternaMagica()
            : base(2597)
        {
            this.Weight = 2.0;
            this.Name = "Lanterna Amarela";
            this.Hue = 55;
            this.PartyLoot = true;
        }

        public LanternaMagica(Serial serial)
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
