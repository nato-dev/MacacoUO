using System;

namespace Server.Items
{
    public class MandrakeSeeds : BasePlantable
    {
        [Constructable]
        public MandrakeSeeds()
            : base(0x0F24)
        {
            this.Name = "Semente de Mandrake";
            this.Hue = 266;
        }

        public MandrakeSeeds(Serial serial)
          : base(serial)
        {
        }

        public override int GetMinSkill()
        {
            return 50;
        }

        public override int GetMaxSkill()
        {
            return 90;
        }

        public override Item GetToPlant()
        {
            return new FarmableMandrakeroot();
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
