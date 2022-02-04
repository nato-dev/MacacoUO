using Server.Mobiles;
using System;
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// This powerful ability requires secondary skills to activate.
    /// Successful use of Shadowstrike deals extra damage to the target — and renders the attacker invisible!
    /// Only those who are adept at the art of stealth will be able to use this ability.
    /// </summary>
    public class ShadowStrike : Habilidade
    {
        public ShadowStrike()
        {
        }

        public override int BaseMana
        {
            get
            {
                return 20;
            }
        }
        public override double DamageScalar
        {
            get
            {
                return 1.25;
            }
        }
        public override bool RequiresSecondarySkill(Mobile from)
        {
            return false;
        }

        public override Talento TalentoParaUsar { get { return Talento.Hab_Shadowstrike; } }

        public override bool CheckSkills(Mobile from)
        {
            if (!base.CheckSkills(from))
                return false;

            Skill skill = from.Skills[SkillName.Stealth];

            if (skill != null && skill.Value >= 80.0)
                return true;

            from.SendLocalizedMessage("Voce precisa de 80 stealth para isto"); // You lack the required stealth to perform that attack

            return false;
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!this.Validate(attacker) || !this.CheckMana(attacker, true))
                return;

            ClearCurrentAbility(attacker);

            attacker.SendLocalizedMessage("Voce da um golpe das sombras"); // You strike and hide in the shadows!
            defender.SendLocalizedMessage("Voce foi atacado rapidamente e seu atacante sumiu nas sombras"); // You are dazed by the attack and your attacker vanishes!

            Effects.SendLocationParticles(EffectItem.Create(attacker.Location, attacker.Map, EffectItem.DefaultDuration), 0x376A, 8, 12, 9943);
            attacker.PlaySound(0x482);

            defender.FixedEffect(0x37BE, 20, 25);

            if(defender is BaseCreature && defender.Combatant == null && Utility.RandomDouble() < 0.1)
            {
                defender.Combatant = attacker;
                defender.OverheadMessage("* agarrou *");
                attacker.Hidden = false;
            } else
            {
                attacker.Combatant = null;
                attacker.Warmode = false;
                attacker.Hidden = true;
                if (defender is BaseCreature)
                {
                    defender.Paralyze(TimeSpan.FromSeconds(2));
                    defender.OverheadMessage("* ?? *");
                }
            }
        }
    }
}
