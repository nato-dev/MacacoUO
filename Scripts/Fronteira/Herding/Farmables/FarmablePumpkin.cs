using System;

namespace Server.Items
{
    public class FarmablePumpkin : BaseFarmable
    {
        [Constructable]
        public FarmablePumpkin()
            : base(GetCropID())
        {
            Name = "Planta de Abobora";
        }

        public FarmablePumpkin(Serial serial)
            : base(serial)
        {
        }

        public static int GetCropID()
        {
            return Utility.Random(3166, 3);
        }

        public override Item GetCropObject()
        {
            Pumpkin pumpkin = new Pumpkin();

            pumpkin.ItemID = Utility.Random(3178, 3);

            return pumpkin;
        }

        public override int GetPickedID()
        {
            return Utility.Random(3166, 3);
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
