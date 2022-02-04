using System;

namespace Server.Items
{
    public class FarmableFlax : BaseFarmable
    {
        [Constructable]
        public FarmableFlax()
            : base(GetCropID())
        {
            Name = "Planta de Algodao";
        }

        public FarmableFlax(Serial serial)
            : base(serial)
        {
        }

        public static int GetCropID()
        {
            return Utility.Random(6809, 3);
        }

        public override Item GetCropObject()
        {
            Flax flax = new Flax();

            flax.ItemID = Utility.Random(6812, 2);

            return flax;
        }

        public override int GetPickedID()
        {
            return 0x1014;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
