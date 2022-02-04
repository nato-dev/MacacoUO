using System;

namespace Server.Items
{
    public class LesserExplosionPotion : BaseExplosionPotion
    {
		public override string DefaultName
        {
            get { return "Poção de Explosão Fraca"; }            
        }
		
        [Constructable]
        public LesserExplosionPotion()
            : base(PotionEffect.ExplosaoFraca)
        {
        }

        public LesserExplosionPotion(Serial serial)
            : base(serial)
        {
        }

        public override int MinDamage
        {
            get
            {
                return 1;
            }
        }
        public override int MaxDamage
        {
            get
            {
                return 8;
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
