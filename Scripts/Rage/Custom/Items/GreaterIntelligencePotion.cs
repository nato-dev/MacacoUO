using System;

namespace Server.Items
{
    public class GreaterIntelligencePotion : BaseIntelligencePotion
    {
        public override string DefaultName
        {
            get { return "Poção de Inteligência Maior"; }
        }

        [Constructable]
        public GreaterIntelligencePotion()
            : base(PotionEffect.InteligenciaMaior)
        {
        }

        public GreaterIntelligencePotion(Serial serial)
            : base(serial)
        {
        }

        public override int IntOffset
        {
            get
            {
                return 30;
            }
        }
        public override TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromMinutes(2.0);
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
