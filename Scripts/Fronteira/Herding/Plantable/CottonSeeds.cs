using System;

namespace Server.Items
{
    public class CottonSeeds : BasePlantable
    {
        [Constructable]
        public CottonSeeds()
            : base(0x0F24)
        {
            this.Name = "Semente de Algodao";
        }

        public CottonSeeds(Serial serial)
          : base(serial)
        {
        }

        public override int GetMinSkill()
        {
            return base.GetMinSkill();
        }

        public override Item GetToPlant()
        {
            return new FarmableCotton();
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
