using Server.Items;

namespace Server.Ziden.Dungeons.Esgoto
{
    public class SapatoLindo : BaseShoes
    {
        [Constructable]
        public SapatoLindo()
            : base(0x170F, 78)
        {
            this.Weight = 2.0;
            this.Name = "Sapato do Zeh";
            this.Hue = 78;
            this.PartyLoot = true;
        }

        public SapatoLindo(Serial serial)
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
