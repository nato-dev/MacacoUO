using System;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Necromancy
{
    public class CurseWeaponSpell : NecromancerSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Curse Weapon", "An Sanct Gra Char",
            203,
            9031,
            Reagent.PigIron);

        private static readonly Dictionary<Mobile, ExpireTimer> m_Table = new Dictionary<Mobile, ExpireTimer>();

        public CurseWeaponSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds(0.75);
            }
        }
        public override double RequiredSkill
        {
            get
            {
                return 0.0;
            }
        }
        public override int RequiredMana
        {
            get
            {
                return 7;
            }
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x387);
                Caster.FixedParticles(0x3779, 1, 15, 9905, 32, 2, EffectLayer.Head);
                Caster.FixedParticles(0x37B9, 1, 14, 9502, 32, 5, (EffectLayer)255);
                new SoundEffectTimer(Caster).Start();

                TimeSpan duration = TimeSpan.FromSeconds((Caster.Skills[SkillName.SpiritSpeak].Value / 3.4) + 1.0);

                ExpireTimer t = null;

                if (m_Table.ContainsKey(Caster))
                {
                    t = m_Table[Caster];
                }

                if (t != null)
                    t.Stop();

                m_Table[Caster] = t = new ExpireTimer(Caster, duration);

                t.Start();
                Caster.SendMessage("Voce sente energia negra emanando de suas maos");
                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.CurseWeapon, 1060512, 1153780, duration, Caster));
            }

            FinishSequence();
        }

        public static bool IsCursed(Mobile attacker, BaseWeapon wep)
        {
            return m_Table.ContainsKey(attacker);
        }

        public class ExpireTimer : Timer
        {

            public Mobile Owner { get; private set; }

            public ExpireTimer(Mobile owner, TimeSpan delay)
                : base(delay)
            {
                Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {

                Effects.PlaySound(Owner.Location, Owner.Map, 0xFA);

                if (m_Table.ContainsKey(Owner))
                {
                    m_Table.Remove(Owner);
                }
            }
        }

        private class SoundEffectTimer : Timer
        {
            private readonly Mobile m_Mobile;

            public SoundEffectTimer(Mobile m)
                : base(TimeSpan.FromSeconds(0.75))
            {
                m_Mobile = m;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                m_Mobile.PlaySound(0xFA);
            }
        }
    }
}
