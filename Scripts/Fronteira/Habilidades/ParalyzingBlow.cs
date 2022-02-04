using System;
using System.Collections;
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// A successful Paralyzing Blow will leave the target stunned, unable to move, attack, or cast spells, for a few seconds.
    /// </summary>
    public class ParalyzingBlow : WeaponAbility
    {
        public static readonly TimeSpan NPCFreezeDuration = TimeSpan.FromSeconds(6.0);
        public static readonly TimeSpan FreezeDelayDuration = TimeSpan.FromSeconds(8.0);
        // No longer active in pub21:
        private static readonly Hashtable m_Table = new Hashtable();
        public ParalyzingBlow()
        {
        }

        public override int BaseMana
        {
            get
            {
                return 30;
            }
        }
        public static bool IsImmune(Mobile m)
        {
            return m_Table.Contains(m);
        }

        public override Talento TalentoParaUsar { get { return Talento.Hab_ParalizeBlow; } }

        public static void BeginImmunity(Mobile m, TimeSpan duration)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
                t.Stop();

            t = new InternalTimer(m, duration);
            m_Table[m] = t;

            t.Start();
        }

        public static void EndImmunity(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
                t.Stop();

            m_Table.Remove(m);
        }

        public override bool RequiresSecondarySkill(Mobile from)
        {
            BaseWeapon weapon = from.Weapon as BaseWeapon;

            if (weapon == null)
                return true;

            return weapon.Skill != SkillName.Wrestling;
        }

        public override bool OnBeforeSwing(Mobile attacker, Mobile defender)
        {
            if(defender == null)
                return false;
                
            if (defender.Paralyzed)
            {
                if (attacker != null)
                {
                    attacker.SendLocalizedMessage("O alvo ja esta paralizado"); // The target is already frozen.
                }

                return false;
            }

            return true;
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!this.Validate(attacker) || !this.CheckMana(attacker, true))
                return;

            ClearCurrentAbility(attacker);

            if (IsImmune(defender))	//Intentionally going after Mana consumption
            {
                attacker.SendLocalizedMessage("Resistiu a paralizia"); // Your target resists paralysis.
                defender.SendLocalizedMessage("Voce resistiu a um ataque paralizante"); // You resist paralysis.
                return;
            }

            defender.FixedEffect(0x376A, 9, 32);
            defender.PlaySound(0x204);

            attacker.SendLocalizedMessage("Voce da um golpe paralizante"); // You deliver a paralyzing blow!
            defender.SendLocalizedMessage("Seu inimigo acertou um golpe paralizante e voce nao pode se mover"); // The attack has temporarily paralyzed you!

            TimeSpan duration = defender.Player ? TimeSpan.FromSeconds(Utility.Random(2,2)) : NPCFreezeDuration;

            // Treat it as paralyze not as freeze, effect must be removed when damaged.
            defender.Freeze(duration);

            BeginImmunity(defender, duration + FreezeDelayDuration);
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Mobile;
            public InternalTimer(Mobile m, TimeSpan duration)
                : base(duration)
            {
                this.m_Mobile = m;
                this.Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                EndImmunity(this.m_Mobile);
            }
        }
    }
}
