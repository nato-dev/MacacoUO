using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells.Chivalry;
using Server.Targeting;

namespace Server.Spells.Fifth
{
    public class ParalyzeSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Paralyze", "An Ex Por",
            218,
            9012,
            Reagent.Garlic,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk);
        public ParalyzeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Fifth;
            }
        }
        public override void OnCast()
        {
            this.Caster.Target = new InternalTarget(this);
        }

        public override TimeSpan GetCastDelay()
        {
            if (Shard.SPHERE_STYLE && Caster.Player)
                return TimeSpan.FromSeconds(2.5);
            else
                return base.GetCastDelay();
        }

        public void Target(Mobile m)
        {
            if (!this.Caster.CanSee(m, true))
            {
                this.Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (Core.AOS && (m.Frozen || m.Paralyzed || (m.Spell != null && m.Spell.IsCasting && !(m.Spell is PaladinSpell))))
            {
                this.Caster.SendLocalizedMessage(1061923); // The target is already frozen.
            }
            else if (this.CheckHSequence(m))
            {
                //SpellHelper.Turn(this.Caster, m);

                // mais facil 
                SpellHelper.CheckReflect((int)this.Circle, this.Caster, ref m);

                double duration;
				
                if (Core.AOS)
                {
                    int secs = (int)((this.GetDamageSkill(this.Caster) / 10) - (this.GetResistSkill(m) / 10));
					
                    if (!m.Player)
                        secs *= 3;

                    if (secs < 0)
                        secs = 0;

                    duration = secs;
                }
                else
                {
                    // Algorithm: ((20% of magery) + 7) seconds [- 50% if resisted]
                    duration = Utility.Random(6, 4);
   
                    if (duration <= 0 || this.CheckResisted(m))
                    {
                        duration = 0;
                        m.SendMessage("Voce sente seu corpo resistindo a magia");
                    } 
                }

                if (m is PlagueBeastLord)
                {
                    ((PlagueBeastLord)m).OnParalyzed(this.Caster);
                    duration = 120;
                }

                if (duration == 0)
                    return;

                var limiteParalize = DateTime.UtcNow - TimeSpan.FromSeconds(6);
                if(m.LastParalized > limiteParalize)
                {
                    duration /= 2;
                    this.Caster.SendMessage("O alvo foi paralizado demais e esta mais resistente a magia");
                }

                m.PrivateOverheadMessage("* Paralizado *");

                m.Paralyze(TimeSpan.FromSeconds(duration));


                m.PlaySound(0x204);
                //m.FixedEffect(0x376A, 6, 1);
                Caster.MovingParticles(m, 0x374A, 8, 0, false, false, 9502, 0x374A, 0x204);

                this.HarmfulSpell(m);
            }

            this.FinishSequence();
        }

        public class InternalTarget : Target
        {
            private readonly ParalyzeSpell m_Owner;
            public InternalTarget(ParalyzeSpell owner)
                : base(Spell.RANGE, false, TargetFlags.Harmful)
            {
                this.m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    this.m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                this.m_Owner.FinishSequence();
            }
        }
    }
}
