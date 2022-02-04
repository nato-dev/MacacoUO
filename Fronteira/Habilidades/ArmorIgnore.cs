using System;
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// This special move allows the skilled warrior to bypass his target's physical resistance, for one shot only.
    /// The Armor Ignore shot does slightly less damage than normal.
    /// Against a heavily armored opponent, this ability is a big win, but when used against a very lightly armored foe, it might be better to use a standard strike!
    /// </summary>
    public class ArmorIgnore : Habilidade
    {
        public ArmorIgnore()
        {
        }

        public override int BaseMana
        {
            get
            {
                return 30;
            }
        }
        public override double DamageScalar
        {
            get
            {
                return 0.9;
            }
        }

        public override Talento TalentoParaUsar { get { return Talento.Hab_ArmorIgnore; } }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!this.Validate(attacker) || !this.CheckMana(attacker, true))
                return;

            ClearCurrentAbility(attacker);

            attacker.SendLocalizedMessage("Seu ataque ignorou a armadura"); // Your attack penetrates their armor!
            defender.SendLocalizedMessage("O ataque ignorou sua armadura"); // The blow penetrated your armor!

            defender.PlaySound(0x56);
            defender.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
        }
    }
}
