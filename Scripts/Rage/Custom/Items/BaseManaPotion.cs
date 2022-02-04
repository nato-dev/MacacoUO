using System;
using Server.Network;
using Server.Services;
using VitaNex.Modules.AutoPvP;

namespace Server.Items
{
    public abstract class BaseManaPotion : BasePotion
    {
        public BaseManaPotion(PotionEffect effect)
            : base(0x0F01, effect)
        {
            Stackable = true;
        }

        public BaseManaPotion(Serial serial)
            : base(serial)
        {
            Stackable = true;
        }

        public abstract int MinMana { get; }
        public abstract int MaxMana { get; }
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

        public void DoMana(Mobile from)
        {
            int min = Scale(from, this.MinMana);
            int max = Scale(from, this.MaxMana);

            var amt = Utility.RandomMinMax(min, max);
            from.Mana += amt;
            DamageNumbers.ShowDamage(-amt, from, from, 2124);
        }

        public override bool OnValidateDrink(Mobile from)
        {
            if (from.BeginAction(this.GetType()))
            {
                Timer.DelayCall(TimeSpan.FromSeconds(Delay), (f) => { f.EndAction(this.GetType()); }, from);
                return true;
            }
            return false;
        }

        public override void Drink(Mobile from)
        {
            if(!Shard.POL_STYLE)
                return;

            DoMana(from);
            PlayDrinkEffect(from);

            if (!(from.Region is PvPRegion))
                Consume();
          
            Timer.DelayCall(TimeSpan.FromSeconds(Delay), (f) => { f.EndAction(this.GetType()); }, from);
        }

    }
}
