using System;
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{

    public class InfectiousStrike : Habilidade
    {
        public InfectiousStrike()
        {
        }

        public override int BaseMana
        {
            get
            {
                return 20;
            }
        }
        
        public override bool RequiresSecondarySkill(Mobile from)
        {
            return true;
        }
        
        public override SkillName GetSecondarySkill(Mobile from)
        {
            return SkillName.Tactics;
        }

        public override Talento TalentoParaUsar { get { return Talento.Hab_Infectar; } }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!this.Validate(attacker))
                return;

            ClearCurrentAbility(attacker);

            BaseWeapon weapon = attacker.Weapon as BaseWeapon;

            if (weapon == null)
                return;

            Poison weaponPoison = weapon.Poison;

            if (weaponPoison == null || weapon.PoisonCharges <= 0)
            {
                attacker.SendLocalizedMessage("Sua arma precisa estar envenenada"); // Your weapon must have a dose of poison to perform an infectious strike!
                return;
            }

            if (!this.CheckMana(attacker, true))
                return;

            // Skill Masteries
            int noChargeChance = Server.Spells.SkillMasteries.MasteryInfo.NonPoisonConsumeChance(attacker);

            if (noChargeChance == 0 || noChargeChance < Utility.Random(100) || Utility.RandomDouble() < attacker.Skills[SkillName.Poisoning].Value/200)
                --weapon.PoisonCharges;
            else
                attacker.SendLocalizedMessage("Sua maestria permitiu voce usar o veneno sem consumir ele"); // Your mastery of poisoning allows you to use your poison charge without consuming it.

            // Infectious strike special move now uses poisoning skill to help determine potency 
            int maxLevel = 0;
            if (weaponPoison == Poison.DarkGlow)
            {
            	maxLevel = 10 + (attacker.Skills[SkillName.Poisoning].Fixed / 333);
            	if (maxLevel > 13)
            		maxLevel = 13;
            }
            else if (weaponPoison == Poison.Parasitic)
            {
            	maxLevel = 14 + (attacker.Skills[SkillName.Poisoning].Fixed / 250);
            	if (maxLevel > 18)
            		maxLevel = 18;
            }
			else            
			{
				maxLevel = attacker.Skills[SkillName.Poisoning].Fixed / 200;
				if (maxLevel > 5)
					maxLevel = 5;

                if (attacker.Skills[SkillName.Poisoning].Value < 60)
                    maxLevel = 2;
			}
			
            if (maxLevel < 0)
                maxLevel = 0;
            if (weaponPoison.Level > maxLevel) // If they don't have enough Poisoning Skill for the potion strength, lower it.
                weaponPoison = Poison.GetPoison(maxLevel);

            if(Shard.DebugEnabled)
            {
                if (weaponPoison != null)
                    Shard.Debug("Poison level " + weaponPoison.Level + " - Max Level " + maxLevel);
                else
                    Shard.Debug("Poison Null");
            }

            if (attacker.Skills[SkillName.Poisoning].Value < 70)
                weaponPoison = Poison.Lesser;

            if (attacker.Skills[SkillName.Poisoning].Value < 90 && weaponPoison.Level > Poison.Regular.Level)
                weaponPoison = Poison.Regular;

            /*
            if ((attacker.Skills[SkillName.Poisoning].Value / 150.0) > Utility.RandomDouble())
            {
            	if (weaponPoison !=null && weaponPoison.Level + 1 <= maxLevel)
            	{
            		int level = weaponPoison.Level + 1;
                	Poison newPoison = Poison.GetPoison(level);
           	
	                if (newPoison != null)
	                {
                 	   weaponPoison = newPoison;

 	                   attacker.SendLocalizedMessage("Seu golpe preciso aumentou o nivel do veneno"); // Your precise strike has increased the level of the poison by 1
 	                   defender.SendLocalizedMessage("O veneno ficou mais forte"); // The poison seems extra effective!
	                }
            	}
            }
            */

            defender.PlaySound(0xDD);
            defender.FixedParticles(0x3728, 244, 25, 9941, 1266, 0, EffectLayer.Waist);

            if (defender.ApplyPoison(attacker, weaponPoison) != ApplyPoisonResult.Immune)
            {
                attacker.SendLocalizedMessage("Voce envenenou o alvo"); // You have poisoned your target : 
                defender.SendLocalizedMessage("Voce foi envenenado"); //  : poisoned you!
            }
        }
    }
}
