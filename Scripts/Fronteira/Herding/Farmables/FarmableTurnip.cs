using System;

namespace Server.Items
{
    public class FarmableTurnip : BaseFarmable
    {
        [Constructable]
        public FarmableTurnip()
            : base(GetCropID())
        {
            Name = "Planta de Rabanete";
        }

        public FarmableTurnip(Serial serial)
            : base(serial)
        {
        }

        public static int GetCropID()
        {
            return Utility.Random(3169, 3);
        }

        public override Item GetCropObject()
        {
            Turnip turnip = new Turnip();

            turnip.ItemID = Utility.Random(3385, 2);

            return turnip;
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
