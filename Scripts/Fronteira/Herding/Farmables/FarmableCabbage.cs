using System;

namespace Server.Items
{
    public class FarmableCabbage : BaseFarmable
    {
        [Constructable]
        public FarmableCabbage()
            : base(GetCropID())
        {
            Name = "Planta de Repolho";
        }

        public FarmableCabbage(Serial serial)
            : base(serial)
        {
        }

        public static int GetCropID()
        {
            return 3254;
        }

        public override Item GetCropObject()
        {
            Cabbage cabbage = new Cabbage();

            cabbage.ItemID = Utility.Random(3195, 2);

            return cabbage;
        }

        public override int GetPickedID()
        {
            return 3254;
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
