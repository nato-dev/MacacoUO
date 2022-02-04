using System;
using Server.Items;
using Server.Network;
using Server.SkillHandlers;

namespace Server.Spells.Ninjitsu
{
    public class Backstab : NinjaMove
    {
        public Backstab()
        {
        }

        public override int BaseMana
        {
            get
            {
                return 30;
            }
        }
        public override double RequiredSkill
        {
            get
            {
                return 40.0;
            }
        }
        public override TextDefinition AbilityMessage
        {
            get
            {
                return new TextDefinition("Voce se prepara para apunhalar alguem");
            }
        }// You prepare to Backstab your opponent.
        public override bool ValidatesDuringHit
        {
            get
            {
                return false;
            }
        }
        public override double GetDamageScalar(Mobile attacker, Mobile defender)
        {
            if (attacker.Weapon is BaseRanged)
                return 1;

            return 1.0 + attacker.Skills.Ninjitsu.Value / 200;
        }

        public override bool Validate(Mobile from)
        {
            if (!from.Hidden || from.AllowedStealthSteps <= 0)
            {
                from.SendLocalizedMessage(1063087); // You must be in stealth mode to use this ability.
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

            attacker.SendMessage("Voce apunhalou o alvo"); // You quickly stab your opponent as you come out of hiding!

            defender.FixedParticles(0x37B9, 1, 5, 0x251D, 0x651, 0, EffectLayer.Waist);
            defender.SendMessage(attacker.Name + " te apunhalou !");
            attacker.RevealingAction(false);
            attacker.NonlocalOverheadMessage(MessageType.Regular, 0, false, "* apunhalou *");

            this.CheckGain(attacker);
        }

        public override void OnMiss(Mobile attacker, Mobile defender)
        {
            ClearCurrentMove(attacker);

            attacker.SendLocalizedMessage(1063161); // You failed to properly use the element of surprise.

            attacker.RevealingAction();
        }
    }
}
