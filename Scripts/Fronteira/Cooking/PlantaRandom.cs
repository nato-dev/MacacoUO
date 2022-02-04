using System;
using Server.Fronteira.Cooking;

namespace Server.Engines.Craft
{
    public class PlantaRandom : Item
    {
        [Constructable]
        public PlantaRandom()
            : base(3253)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (this.Deleted)
                    return;

                try
                {
                    var planta = CookingLoader.GetPlantaRandom();
                    planta.MoveToWorld(this.Location, this.Map);
                } catch(Exception e)
                {

                } finally
                {
                    this.Delete();
                }
            });
        }

        public PlantaRandom(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
