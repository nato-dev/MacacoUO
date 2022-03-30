using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class AntiPoisonTreat : TastyTreat
    {
        public override double Bonus { get { return 0.05; } }
        public override TimeSpan Duration { get { return TimeSpan.FromMinutes(30); } }
        public override TimeSpan CoolDown { get { return TimeSpan.FromMinutes(3); } }

        [Constructable]
        public AntiPoisonTreat() : this(1)
        {
        }


        [Constructable]
        public AntiPoisonTreat(int amount)
        {
            Name = "Biscoito de Pet Delicioso";
            Stackable = true;
            Amount = amount;
        }


        public AntiPoisonTreat(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = (InheritsItem ? 0 : reader.ReadInt()); //Required for TastyTreat Insertion
        }
    }
}
