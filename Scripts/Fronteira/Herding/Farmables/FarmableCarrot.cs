using System;

namespace Server.Items
{
    public class FarmableCarrot : BaseFarmable
    {
        [Constructable]
        public FarmableCarrot()
            : base(GetCropID())
        {
            Name = "Planta de Cenoura";
        }

        public FarmableCarrot(Serial serial)
            : base(serial)
        {
        }

        public static int GetCropID()
        {
            return 3190;
        }

        public override Item GetCropObject()
        {
            Carrot carrot = new Carrot();

            carrot.ItemID = Utility.Random(3191, 2);

            return carrot;
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
