using System;
using System.Collections.Generic;

using Server.Mobiles;
using Server.Items;

namespace Server.Misc
{
    public class WeightOverloading
    {

        public const int OverloadAllowance = 4;// We can be four stones overweight without getting fatigued

        public static void Initialize()
        {
            EventSink.Movement += new MovementEventHandler(EventSink_Movement);
            Mobile.FatigueHandler = FatigueOnDamage;
        }

        public static void FatigueOnDamage(Mobile m, int damage, DFAlgorithm df)
        {
            double fatigue = 0.0;
            var hits = Math.Max(1, m.Hits);

            switch (m.DFA)
            {
                case DFAlgorithm.Standard:
                    {
                        fatigue = (damage * (m.HitsMax / hits) * ((double)m.Stam / m.StamMax)) - 5;
                    }
                    break;
                case DFAlgorithm.PainSpike:
                    {
                        fatigue = (damage * ((m.HitsMax / hits) + ((50.0 + m.Stam) / m.StamMax) - 1.0)) - 5;
                    }
                    break;
            }

            var reduction = BaseArmor.GetInherentStaminaLossReduction(m) + 1;

            if (reduction > 1)
            {
                fatigue = fatigue / reduction;
            }

            if (fatigue > 0)
            {
                // On EA, if follows this special rule to reduce the chances of your stamina being dropped to 0
                if (m.Stam - fatigue <= 10)
                {
                    m.Stam -= (int)(fatigue * ((double)m.Hits / (double)m.HitsMax));
                }
                else
                {
                    m.Stam -= (int)fatigue;
                }
            }
        }

        public static void EventSink_Movement(MovementEventArgs e)
        {
            Mobile from = e.Mobile;
            if (!from.Player)
                return;

            if (!from.Alive || from.IsStaff())
                return;

            int maxWeight = from.MaxWeight + OverloadAllowance;// 4
            int myWeight = (Mobile.BodyWeight + from.TotalWeight);
            int pctPeso = myWeight * 100 / maxWeight;

            if (from.Stam == 0)
            {
                from.SendMessage("Voce esta muito fadigado para se mover"); // Your mount is too fatigued to move. : You are too fatigued to move.
                e.Blocked = true;
                return;
            }

            var pm = from as PlayerMobile;

            if (pm != null)
            {
                if (pctPeso <= 99)
                    return;
                --from.Stam;
            }
        }
        public static bool IsOverloaded(Mobile m)
        {
            if (!m.Player || !m.Alive || m.IsStaff())
                return false;

            return ((Mobile.BodyWeight + m.TotalWeight) > (m.MaxWeight + OverloadAllowance));
        }
    }
}
