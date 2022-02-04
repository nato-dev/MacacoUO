using System;

namespace Server.Items
{
    public class FarmableGarlic : BaseFarmable
    {
        [Constructable]
        public FarmableGarlic()
            : base(GetCropID())
        {
            this.Name = "Planta de Garlic";
        }

        public FarmableGarlic(Serial serial)
            : base(serial)
        {
        }

        public override BasePlantable GetSeed()
        {
            return new GarlicSeeds();
        }

        public static int GetCropID()
        {
            return 0x18E1;
        }

        public override Item GetCropObject()
        {
            return new Garlic();
        }

        public override int GetMaxSkill()
        {
            return 100;
        }

        public override int GetMinSkill()
        {
            return 70;
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
