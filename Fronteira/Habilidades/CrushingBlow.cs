using System;
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// Also known as the Haymaker, this attack dramatically increases the damage done by a weapon reaching its mark.
    /// </summary>
    public class CrushingBlow : Habilidade
    {
        public CrushingBlow()
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
                return 1.5;
            }
        }

        public override Talento TalentoParaUsar { get { return Talento.Hab_CrushingBlow; } }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!this.Validate(attacker) || !this.CheckMana(attacker, true))
                return;

            ClearCurrentAbility(attacker);

            attacker.SendLocalizedMessage("Voce usou um golpe especial"); // You have delivered a crushing blow!
            defender.SendLocalizedMessage("Voce tomou dano extra do ataque pesado"); // You take extra damage from the crushing attack!

            defender.PlaySound(0x1E1);
            defender.FixedParticles(0, 1, 0, 9946, EffectLayer.Head);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 50), defender.Map), new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 20), defender.Map), 0xFB4, 1, 0, false, false, 0, 3, 9501, 1, 0, EffectLayer.Head, 0x100);
        }
    }
}
