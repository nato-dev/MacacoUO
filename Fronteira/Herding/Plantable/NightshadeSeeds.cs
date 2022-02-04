using System;

namespace Server.Items
{
    public class NightshadeSeeds : BasePlantable
    {
        [Constructable]
        public NightshadeSeeds()
            : base(0x0F24)
        {
            this.Name = "Semente de Nightshade";
            this.Hue = 0x30A;
        }

        public NightshadeSeeds(Serial serial)
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
            return new FarmableNightShade();
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
