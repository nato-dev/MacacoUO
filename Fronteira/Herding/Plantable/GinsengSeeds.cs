using System;

namespace Server.Items
{
    public class GinsengSeeds : BasePlantable
    {
        [Constructable]
        public GinsengSeeds()
            : base(0x0F24)
        {
            this.Name = "Semente de Ginseng";
            this.Hue = 266;
        }

        public GinsengSeeds(Serial serial)
          : base(serial)
        {
        }

        public override int GetMinSkill()
        {
            return 40;
        }

        public override int GetMaxSkill()
        {
            return 70;
        }

        public override Item GetToPlant()
        {
            return new FarmableGinseng();
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
