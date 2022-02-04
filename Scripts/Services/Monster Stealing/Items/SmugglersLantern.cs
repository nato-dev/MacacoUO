using System;
using Server;

namespace Server.Items
{
    public class SmugglersLantern : Lantern
    {

        [Constructable] 
        public SmugglersLantern()
        {
            Name = "Lanterna Magica";
            Hue = Utility.RandomMinMax(192, 291);
            LootType = LootType.Blessed;
        }

        public override bool AllowEquipedCast(Mobile from)
        {
            return true;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Permite usar Magias"); // Spell Channeling
        }

        public SmugglersLantern(Serial serial)
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
