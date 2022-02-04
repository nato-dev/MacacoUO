using System;

namespace Server.Items
{
    public class FarmableNightShade : BaseFarmable
    {
        [Constructable]
        public FarmableNightShade()
            : base(GetCropID())
        {
            this.Name = "Planta de Nightshade";
        }

        public FarmableNightShade(Serial serial)
            : base(serial)
        {
        }

        public override BasePlantable GetSeed()
        {
            return new NightshadeSeeds();
        }

        public static int GetCropID()
        {
            return 0x18E5;
        }

        public override Item GetCropObject()
        {
            return new Nightshade();
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
