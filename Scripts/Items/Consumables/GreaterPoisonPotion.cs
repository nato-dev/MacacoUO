using System;

namespace Server.Items
{
    public class GreaterPoisonPotion : BasePoisonPotion
    {
		public override string DefaultName
        {
            get { return "Poção de Veneno Forte"; }            
        }
		
        [Constructable]
        public GreaterPoisonPotion()
            : base(PotionEffect.VenenoForte)
        {
        }

        public GreaterPoisonPotion(Serial serial)
            : base(serial)
        {
        }

        public override Poison Poison
        {
            get
            {
                return Poison.Greater;
            }
        }
        public override double MinPoisoningSkill
        {
            get
            {
                return 75.0;
            }
        }
        public override double MaxPoisoningSkill
        {
            get
            {
                return 99.0;
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
