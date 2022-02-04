
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{
	/// <summary>
	///     The highly skilled warrior can use this special attack to make two quick swings in succession.
	///     Landing both blows would be devastating!
	/// </summary>
	public class DoubleStrike : WeaponAbility
	{
		public override int BaseMana { get { return 30; } }
		public override double DamageScalar { get { return 0.9; } }

        public override bool OnBeforeDamage(Mobile attacker, Mobile defender)
        {
            BaseWeapon wep = attacker.Weapon as BaseWeapon;

            if (wep != null)
                wep.InDoubleStrike = true;

            return true;
        }

        public override Talento TalentoParaUsar { get { return Talento.Hab_DoubleStrike; } }

        public override bool Validate(Mobile from)
        {
            if ((from.Weapon is BaseRanged))
            {
                from.SendLocalizedMessage("Voce nao pode fazer isto com esta arma"); // You can only execute this attack while mounted!
                ClearCurrentAbility(from);
                return true;
            }
            return base.Validate(from);
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (!Validate(attacker) || !CheckMana(attacker, true))
			{
				return;
			}

			ClearCurrentAbility(attacker);

			BaseWeapon weapon = attacker.Weapon as BaseWeapon;

			if (weapon == null)
			{
				return;
			}

            // If no combatant, wrong map, one of us is a ghost, or cannot see, or deleted, then stop combat
            if (defender.Deleted || attacker.Deleted || defender.Map != attacker.Map || !defender.Alive ||
                !attacker.Alive || !attacker.CanSee(defender))
            {
                weapon.InDoubleStrike = false;
                attacker.Combatant = null;
                return;
            }

			if (!attacker.InRange(defender, weapon.MaxRange))
			{
                weapon.InDoubleStrike = false;
				return;
			}

            attacker.SendLocalizedMessage("Voce atacou na velocidade da luz"); // You attack with lightning speed!
            defender.SendLocalizedMessage("Seu oponente lhe atacou em uma velocidade rapida"); // Your attacker strikes with lightning speed!

            defender.PlaySound(0x3BB);
            defender.FixedEffect(0x37B9, 244, 25);

			if (attacker.InLOS(defender))
			{
				attacker.RevealingAction();
                if (Shard.TROCA_ARMA_RAPIDA)
                {
                    attacker.NextCombatTimes[weapon.GetType()] = Core.TickCount + (int)weapon.OnSwing(attacker, defender).TotalMilliseconds;
                }
                else
                {
                    attacker.NextCombatTime = Core.TickCount + (int)weapon.OnSwing(attacker, defender).TotalMilliseconds;
                } 
			}

            weapon.InDoubleStrike = false;
		}
	}
}
