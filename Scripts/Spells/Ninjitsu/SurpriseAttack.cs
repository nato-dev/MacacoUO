using System;
using System.Collections;
using Server.Items;
using Server.SkillHandlers;

namespace Server.Spells.Ninjitsu
{
    public class SurpriseAttack : NinjaMove
    {
        private static readonly Hashtable m_Table = new Hashtable();
        public SurpriseAttack()
        {
        }

        public override int BaseMana
        {
            get
            {
                return 20;
            }
        }
        public override double RequiredSkill
        {
            get
            {
                return Core.ML ? 60.0 : 30.0;
            }
        }
        public override TextDefinition AbilityMessage
        {
            get
            {
                return new TextDefinition("Voce se prepara para um ataque surpresa");
            }
        }// You prepare to surprise your prey.
        public override bool ValidatesDuringHit
        {
            get
            {
                return false;
            }
        }
        public static bool GetMalus(Mobile target, ref int malus)
        {
            SurpriseAttackInfo info = m_Table[target] as SurpriseAttackInfo;

            if (info == null)
                return false;

            malus = info.m_Malus;
            return true;
        }

        public override bool Validate(Mobile from)
        {
            if (!from.Hidden || from.AllowedStealthSteps <= 0)
            {
                from.SendMessage("Voce precisa estar em stealth para usar isto"); // You must be in stealth mode to use this ability.
                return false;
            }

            return base.Validate(from);
        }

        public override bool OnBeforeSwing(Mobile attacker, Mobile defender)
        {
            bool valid = this.Validate(attacker) && this.CheckMana(attacker, true);

            if (valid)
            {
                attacker.BeginAction(typeof(Stealth));
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), delegate { attacker.EndAction(typeof(Stealth)); });
            }

            return valid;
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            //Validates before swing
            ClearCurrentMove(attacker);

            attacker.SendMessage("Voce deu um ataque surpresa"); // You catch your opponent off guard with your Surprise Attack!
            defender.SendMessage("Sua defesa esta reduzida pois sofreu um ataque surpresa"); // Your defenses are lowered as your opponent surprises you!

            defender.FixedParticles(0x37B9, 1, 5, 0x26DA, 0, 3, EffectLayer.Head);
            var malus = (int)(attacker.Skills.Ninjitsu.Value / 10);
            if (attacker.Weapon is BaseRanged)
                malus /= 2;
      
            defender.VirtualArmorMod -= malus;
            defender.OverheadMessage("* vulneravel *");
            attacker.RevealingAction();

            SurpriseAttackInfo info;

            if (m_Table.Contains(defender))
            {
                info = (SurpriseAttackInfo)m_Table[defender];

                if (info.m_Timer != null)
                    info.m_Timer.Stop();

                m_Table.Remove(defender);
            }

            info = new SurpriseAttackInfo(defender, malus);
            info.m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(8.0), new TimerStateCallback(EndSurprise), info);

            m_Table[defender] = info;

            this.CheckGain(attacker);
        }

        public override void OnMiss(Mobile attacker, Mobile defender)
        {
            ClearCurrentMove(attacker);

            attacker.SendMessage("Voce nao conseguiu surpreender o alvo"); // You failed to properly use the element of surprise.

            attacker.RevealingAction();
        }

        private static void EndSurprise(object state)
        {
            SurpriseAttackInfo info = (SurpriseAttackInfo)state;

            if (info.m_Timer != null)
                info.m_Timer.Stop();

            info.m_Target.SendMessage("Suas defesas retornaram ao normal"); // Your defenses have returned to normal.
            info.m_Target.VirtualArmorMod += info.m_Malus;
            m_Table.Remove(info.m_Target);
        }

        private class SurpriseAttackInfo
        {
            public readonly Mobile m_Target;
            public readonly int m_Malus;
            public Timer m_Timer;
            public SurpriseAttackInfo(Mobile target, int effect)
            {
                this.m_Target = target;
                this.m_Malus = effect;
            }
        }
    }
}
