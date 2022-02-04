using System;
using Server.Network;
using VitaNex.Modules.AutoPvP;

namespace Server.Items
{
    public abstract class BaseHealPotion : BasePotion
    {
        public BaseHealPotion(PotionEffect effect)
            : base(0xF0C, effect)
        {
        }

        public BaseHealPotion(Serial serial)
            : base(serial)
        {
        }

        public bool FiverRatio = false;
        public abstract int MinHeal { get; }
        public abstract int MaxHeal { get; }
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

        public void DoHeal(Mobile from)
        {
            int min = Scale(from, this.MinHeal);
            int max = Scale(from, this.MaxHeal);

            if (FiverRatio)
            {
                if (min != 5)
                    min = 5;
                if (max % 5 != 0)
                {
                    max = (max - max % 5);
                }
                var ratios = (int)max / min;
                var randomRatio = Utility.RandomMinMax(1, ratios);
                //Shard.Debug("Fiver Pot: Ratio " + ratios + " min " + min + " max " + max);
                from.Heal(randomRatio * 5);
            }
            else
            {
                from.Heal(Utility.RandomMinMax(min, max));
            }
        }

        private static void ReleaseHealLock(object state)
        {
            ((Mobile)state).EndAction(typeof(BaseHealPotion));
        }

        public override void Drink(Mobile from)
        {
            if (from.Poisoned || MortalStrike.IsWounded(from))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Voce nao pode beber isto estando neste estado"); // You can not heal yourself in your current state.
            }
            else
            {
                if (Shard.POL_STYLE || from.BeginAction(typeof(BaseHealPotion)))
                {
                    DoHeal(from);
                    PlayDrinkEffect(from);
                    if (!(from.Region is PvPRegion))
                        Consume();

                    if(!Shard.POL_STYLE)
                        Timer.DelayCall(TimeSpan.FromSeconds(this.Delay), new TimerStateCallback(ReleaseHealLock), from);
                }
                else
                {
                    from.SendMessage("Voce precisa aguardar para usar isto novamente"); // You must wait 10 seconds before using another healing potion.
                }
            }
        }
    }
}
