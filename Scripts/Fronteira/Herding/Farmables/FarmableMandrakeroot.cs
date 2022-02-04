using System;

namespace Server.Items
{
    public class FarmableMandrakeroot : BaseFarmable
    {
        [Constructable]
        public FarmableMandrakeroot()
            : base(GetCropID())
        {
            this.Name = "Planta de Mandrake";
        }

        public FarmableMandrakeroot(Serial serial)
            : base(serial)
        {
        }

        public static int GetCropID()
        {
            return 0x18DF;
        }

        public override BasePlantable GetSeed()
        {
            return new MandrakeSeeds();
        }

        public override int GetMaxSkill()
        {
            return 90;
        }

        public override int GetMinSkill()
        {
            return 50;
        }

        public override Item GetCropObject()
        {
            return new MandrakeRoot();
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
