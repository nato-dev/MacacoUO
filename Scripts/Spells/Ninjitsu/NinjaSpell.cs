using System;
using Server.Mobiles;
using Server.Ziden;

namespace Server.Spells.Ninjitsu
{
    public abstract class NinjaSpell : Spell
    {
        public NinjaSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
        public override SkillName CastSkill
        {
            get
            {
                return SkillName.Ninjitsu;
            }
        }
        public override SkillName DamageSkill
        {
            get
            {
                return SkillName.Ninjitsu;
            }
        }
        public override bool RevealOnCast
        {
            get
            {
                return false;
            }
        }
        public override bool ClearHandsOnCast
        {
            get
            {
                return false;
            }
        }
        public override bool ShowHandMovement
        {
            get
            {
                return false;
            }
        }
        public override bool BlocksMovement
        {
            get
            {
                return false;
            }
        }
        //public override int CastDelayBase{ get{ return 1; } }
        public override int CastRecoveryBase
        {
            get
            {
                return 7;
            }
        }

        public override bool ValidateCast(Mobile from)
        {
            from.SendMessage("Ninjutsu temporariamente desabilitado :(");
            return false;

            if (from.IsPlayer() && from.RP)
            {
                var anel = from.FindItemOnLayer(Layer.Ring);
                if (!(anel is AnelNinja))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool CheckCast()
        {
            int mana = 0; //this.AjustaMana(this.RequiredMana/4);
            int stamina = this.ScaleStamina(this.RequiredMana);

            if (!base.CheckCast())
                return false;

            if (this.Caster.Skills[this.CastSkill].Value < this.RequiredSkill)
            {
                string args = String.Format("{0}\t{1}\t ", this.RequiredSkill.ToString("F1"), this.CastSkill.ToString());
                this.Caster.SendMessage("Voce precisa ter pelo menos "+ this.RequiredSkill+" de skill para fazer isto"); // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.
                return false;
            }
            else if (this.Caster.Mana < mana)
            {
                this.Caster.SendMessage("Voce precisa ter pelo menos "+ mana.ToString()+" mana para fazer isto"); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            } else if(this.Caster.Stam < stamina)
            {
                this.Caster.SendMessage("Voce precisa ter pelo menos " + stamina.ToString() + " stamina para fazer isto"); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            return true;
        }

        public override bool CheckFizzle()
        {
            int mana = this.AjustaMana(this.RequiredMana / 4);
            int stamina = this.ScaleStamina(this.RequiredMana);

            if (this.Caster.Skills[this.CastSkill].Value < this.RequiredSkill)
            {
                this.Caster.SendMessage("Voce precisa ter pelo menos " + this.RequiredSkill.ToString("F1") + " de skill para fazer isto"); // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.hat attack!
                return false;
            }
            else if (this.Caster.Mana < mana)
            {
                this.Caster.SendMessage("Voce precisa ter pelo menos " + mana.ToString() + " mana para fazer isto"); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }
            else if (this.Caster.Stam < stamina)
            {
                this.Caster.SendMessage("Voce precisa ter pelo menos " + stamina.ToString() + " stamina para fazer isto"); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            if (!base.CheckFizzle())
                return false;

            this.Caster.Mana -= mana;
            this.Caster.Stam -= stamina;

            return true;
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = this.RequiredSkill - 12.5;	//Per 5 on friday 2/16/07
            max = this.RequiredSkill + 37.5;
        }

        public override int GetMana()
        {
            return 0;
        }
    }
}
