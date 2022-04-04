using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Spells.First
{
    public class HealSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Heal", "In Mani",
            224,
            9061,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.SpidersSilk);
        public HealSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.First;
            }
        }

        public override TimeSpan GetCastDelay()
        {
            if (Shard.SPHERE_STYLE && Caster.Player)
                return TimeSpan.FromSeconds(1.5);
            else if (Shard.POL_STYLE && Caster.Player)
                return TimeSpan.FromSeconds(0.8);
            else
                return base.GetCastDelay();
        }

        public override void OnCast()
        {
            this.Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!this.Caster.CanSee(m, true))
            {
                this.Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (m.IsDeadBondedPet)
            {
                this.Caster.SendLocalizedMessage(1060177); // You cannot heal a creature that is already dead!
            }
            else if (m is BaseCreature && ((BaseCreature)m).IsAnimatedDead)
            {
                this.Caster.SendLocalizedMessage(1061654); // You cannot heal that which is not alive.
            }
            else if (m is IRepairableMobile)
            {
                this.Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500951); // You cannot heal that.
            }
            else if (Server.Items.MortalStrike.IsWounded(m))
            {
                this.Caster.LocalOverheadMessage(MessageType.Regular, 0x22, (this.Caster == m) ? 1005000 : 1010398);
            }
            else if (this.CheckBSequence(m))
            {
                // SpellHelper.Turn(this.Caster, m);

                int toHeal;

                toHeal = (int)(this.Caster.Skills[SkillName.Magery].Value * 0.05);

                if (!Shard.POL_STYLE)
                {
                    toHeal *= 2;
                } else
                {
                    var inscriptBonus = (int)(this.Caster.Skills[SkillName.Inscribe].Value * 0.04);
                    toHeal += inscriptBonus;
                }
                toHeal += Utility.Random(1, 5);
                if(Shard.SPHERE_STYLE && m.Poisoned)
                {
                    toHeal = 0;
                } else
                {
                    var scalar = GreaterHealSpell.GetPoisonScalar(m.Poison);
                    if (scalar < 1 && !m.IsCooldown("poisonmsg"))
                    {
                        m.SetCooldown("poisonmsg");
                        m.SendMessage(78, "Voce curou menos vida por estar envenenado. Quanto mais forte o veneno, mais dificil se curar.");
                    }
                    toHeal = (int)(toHeal * scalar);
                }        

                //m.Heal( toHeal, Caster );
                SpellHelper.Heal(toHeal, m, this.Caster);

                Caster.MovingParticles(m, 0x376A, 7, 0, false, false, 9502, 0x376A, 0x1F2);
                m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                m.PlaySound(0x1F2);
            }

            this.FinishSequence();
        }

        public class InternalTarget : Target
        {
            private readonly HealSpell m_Owner;
            public InternalTarget(HealSpell owner)
                : base(Spell.RANGE, false, TargetFlags.Beneficial)
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
