using System;

namespace Server.Items
{
    public class FarmableLettuce : BaseFarmable
    {
        [Constructable]
        public FarmableLettuce()
            : base(GetCropID())
        {
            Name = "Planta de Alface";
        }

        public FarmableLettuce(Serial serial)
            : base(serial)
        {
        }

        public static int GetCropID()
        {
            return 3254;
        }

        public override Item GetCropObject()
        {
            Lettuce lettuce = new Lettuce();

            lettuce.ItemID = Utility.Random(3184, 2);

            return lettuce;
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
