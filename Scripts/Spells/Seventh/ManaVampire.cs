using System;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class ManaVampireSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Mana Vampire", "Ort Sanct",
            221,
            9032,
            Reagent.BlackPearl,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk);
        public ManaVampireSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Seventh;
            }
        }
        public override void OnCast()
        {
            this.Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!this.Caster.CanSee(m))
            {
                this.Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (this.CheckHSequence(m))
            {
                SpellHelper.Turn(this.Caster, m);

                SpellHelper.CheckReflect((int)this.Circle, this.Caster, ref m);

                if (m.Spell != null)
                    m.Spell.OnCasterHurt();

                m.Paralyzed = false;

                int toDrain = 0;

                if (this.CheckResisted(m))
                    m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                else
                    toDrain = m.Mana;

                if (toDrain > (this.Caster.ManaMax - this.Caster.Mana))
                    toDrain = this.Caster.ManaMax - this.Caster.Mana;

                m.Mana -= toDrain;
                this.Caster.Mana += toDrain;
                m.SendMessage("Sua mana foi sugada pela magia Mana Vampire de "+Caster.Name);

                m.FixedParticles(0x374A, 10, 15, 5054, EffectLayer.Head);
                m.PlaySound(0x1F9);

                this.HarmfulSpell(m);
            }

            this.FinishSequence();
        }

        public override double GetResistPercent(Mobile target)
        {
            return target.Skills[SkillName.MagicResist].Value * 0.7;
        }

        private class InternalTarget : Target
        {
            private readonly ManaVampireSpell m_Owner;
            public InternalTarget(ManaVampireSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                this.m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    this.m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                this.m_Owner.FinishSequence();
            }
        }
    }
}
