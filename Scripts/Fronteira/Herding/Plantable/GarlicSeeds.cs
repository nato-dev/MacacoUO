using System;

namespace Server.Items
{
    public class GarlicSeeds : BasePlantable
    {
        [Constructable]
        public GarlicSeeds()
            : base(0x0F24)
        {
            this.Name = "Semente de Garlic";
            this.Hue = 266;
        }

        public GarlicSeeds(Serial serial)
          : base(serial)
        {
        }

        public override int GetMinSkill()
        {
            return 60;
        }

        public override int GetMaxSkill()
        {
            return 100;
        }

        public override Item GetToPlant()
        {
            return new FarmableGarlic();
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
