using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Ziden;

namespace Server.Spells.Necromancy
{
    public abstract class NecromancerSpell : Spell
    {
        public NecromancerSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
        public override SkillName CastSkill
        {
            get
            {
                return SkillName.Necromancy;
            }
        }
        public override SkillName DamageSkill
        {
            get
            {
                return SkillName.SpiritSpeak;
            }
        }
        //public override int CastDelayBase{ get{ return base.CastDelayBase; } } // Reference, 3
        public override bool ClearHandsOnCast
        {
            get
            {
                return false;
            }
        }

        public override TimeSpan GetCastDelay()
        {
            var d = base.GetCastDelay();
            return TimeSpan.FromSeconds(0.4) + d;
        }

        public override double CastDelayFastScalar
        {
            get
            {
                return (Core.SE ? base.CastDelayFastScalar : 0);
            }
        }// Necromancer spells are not affected by fast cast items, though they are by fast cast recovery
        public override int ComputeKarmaAward()
        {
            //TODO: Verify this formula being that Necro spells don't HAVE a circle.
            //int karma = -(70 + (10 * (int)Circle));
            int karma = -(40 + (int)(10 * (this.CastDelayBase.TotalSeconds / this.CastDelaySecondsPerTick)));

            if (Core.ML) // Pub 36: "Added a new property called Increased Karma Loss which grants higher karma loss for casting necromancy spells."
                karma += AOS.Scale(karma, AosAttributes.GetValue(this.Caster, AosAttribute.IncreasedKarmaLoss));

            return karma;
        }

        public override bool ValidateCast(Mobile from)
        {
            if (from.IsPlayer() && from.RP)
            {
                var anel = from.FindItemOnLayer(Layer.Ring);
                if (!(anel is AnelNecro))
                {
                    return false;
                }
            }
            return true;
        }

        public virtual bool CheckResisted(Mobile target, int circle)
        {

            if (target == Caster)
                return false;

            circle -= 1;
            double n = GetResistPercentForCircle(target, circle);

            n /= 100.0;

            if (n <= 0.0)
                return false;

            if (n >= 1.0)
                return true;

            int maxSkill = (1 + circle * 10);
            maxSkill += (1 + (circle / 6)) * 25;

            if (target.Skills[SkillName.MagicResist].Value < maxSkill && target != Caster)
                target.CheckSkillMult(SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap);

            var resisted = (n >= Utility.RandomDouble());

            if (resisted)
            {
                Caster.PlaySound(0x1E6);
                target.FixedEffect(0x42CF, 10, 5);
            }
            return resisted;
        }

        /*
        public override void SayMantra()
        {
            if (Info.Mantra != null && Info.Mantra.Length > 0 && (m_Caster.Player || (m_Caster is BaseCreature && ((BaseCreature)m_Caster).ShowSpellMantra)))
            {
                m_Caster.PublicOverheadMessage(MessageType.Regular, 723, false, Info.Mantra);
            }
        }
        */

        public override void GetCastSkills(out double min, out double max)
        {
            min = this.RequiredSkill;
            max = this.Scroll != null ? min : this.RequiredSkill + 40.0;
            if (max > 100)
                max = 100;
        }

        public virtual double GetResistPercentForCircle(Mobile target, int circle)
        {
            if (!Shard.POL_STYLE)
            {
                double value = GetResistSkill(target);
                double firstPercent = value / 5.0;
                double secondPercent = value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);
                return (firstPercent > secondPercent ? firstPercent : secondPercent) / 1.5;
            }
            else
            {
                var resist = target.Skills[SkillName.MagicResist].Value;
                var cap = resist / 5;

                var magery = Caster.Skills[CastSkill].Value;
                var circ = 1 + (double)circle;

                var chance = ((magery * 2) / 10 + circ * circ);

                if (Shard.DebugEnabled)
                    Shard.Debug("Chance Base: " + chance + " circulo " + circ);

                chance = resist - chance;
                if (chance < cap)
                    chance = cap;

                if (Shard.SPHERE_STYLE)
                    chance *= 0.5; // sem pre cast mais dificil de resistir
                else
                    chance *= 0.85;

                if (Caster is BaseCreature && target is PlayerMobile)
                    chance /= 1.5;

                Shard.Debug("Chance RS: " + chance, target);

                return chance;
            }
        }

        public override bool ConsumeReagents()
        {
            if (base.ConsumeReagents())
                return true;

            if (ArcaneGem.ConsumeCharges(this.Caster, 1))
                return true;

            return false;
        }

        public override int GetMana()
        {
            return this.RequiredMana;
        }
    }
}
