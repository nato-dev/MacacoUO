using System;

namespace Server.Items
{
    public class GreaterAgilityPotion : BaseAgilityPotion
    {
        public override string DefaultName
        {
            get { return "Poção de Agilidade Maior"; }            
        }

        [Constructable]
        public GreaterAgilityPotion()
            : base(PotionEffect.AgilidadeMaior)
        {
        }

        public GreaterAgilityPotion(Serial serial)
            : base(serial)
        {
        }

        public override int DexOffset
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