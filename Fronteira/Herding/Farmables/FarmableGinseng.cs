using System;

namespace Server.Items
{
    public class FarmableGinseng : BaseFarmable
    {
        [Constructable]
        public FarmableGinseng()
            : base(GetCropID())
        {
            this.Name = "Planta de Ginseng";
        }

        public FarmableGinseng(Serial serial)
            : base(serial)
        {
        }

        public override BasePlantable GetSeed()
        {
            return new GinsengSeeds();
        }

        public static int GetCropID()
        {
            return 0x18E9;
        }

        public override Item GetCropObject()
        {
            return new Ginseng();
        }

        public override int GetMaxSkill()
        {
            return 70;
        }

        public override int GetMinSkill()
        {
            return 40;
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
