using System;
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// Send two arrows flying at your opponent if you're mounted. Requires Bushido or Ninjitsu skill.
    /// </summary>
    public class DoubleShot : WeaponAbility
    {
        public DoubleShot()
        {
        }

        public override int BaseMana
        {
            get
            {
                return Core.TOL ? 30 : 35;
            }
        }

        public override bool OnBeforeDamage(Mobile attacker, Mobile defender)
        {
            BaseWeapon wep = attacker.Weapon as BaseWeapon;

            if (wep != null)
                wep.ProcessingMultipleHits = true;

            return true;
        }

        public override SkillName GetSecondarySkill(Mobile from)
        {
            return SkillName.Tactics;
        }

        public override Talento TalentoParaUsar { get { return Talento.Hab_Doubleshot; } }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            Use(attacker, defender);
        }

        public override void OnMiss(Mobile attacker, Mobile defender)
        {
            Use(attacker, defender);
        }

        public override bool Validate(Mobile from)
        {

            if (!(from.Weapon is BaseRanged))
            {
                from.SendLocalizedMessage("Voce nao pode fazer isto com esta arma"); // You can only execute this attack while mounted!
                ClearCurrentAbility(from);
                return true;
            }
               

            if (base.Validate(from))
            {
                /*
                if (from.Mounted)
                    return true;
                else
                {
                    from.SendLocalizedMessage("Voce precisa estar montado para isto"); // You can only execute this attack while mounted!
                    ClearCurrentAbility(from);
                }
                */
                return true;
            }

            return false;
        }

        public void Use(Mobile attacker, Mobile defender)
        {
            if (!Validate(attacker) || !CheckMana(attacker, true) || attacker.Weapon == null)	//sanity
                return;

            ClearCurrentAbility(attacker);

            attacker.SendLocalizedMessage("Voce da um tiro duplo"); // You launch two shots at once!
            defender.SendLocalizedMessage("Voce foi atacado por um tiro duplo"); // You're attacked with a barrage of shots!

            defender.FixedParticles(0x37B9, 1, 19, 0x251D, EffectLayer.Waist);

            attacker.Weapon.OnSwing(attacker, defender);

            if (attacker.Weapon is BaseWeapon)
                ((BaseWeapon)attacker.Weapon).ProcessingMultipleHits = false;
        }
    }
}
