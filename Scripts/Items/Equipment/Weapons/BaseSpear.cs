using System;

namespace Server.Items
{
    public abstract class BaseSpear : BaseMeleeWeapon
    {
        public BaseSpear(int itemID)
            : base(itemID)
        {
        }

        public BaseSpear(Serial serial)
            : base(serial)
        {
        }

        public override int DefHitSound
        {
            get
            {
                return 0x23C;
            }
        }
        public override int DefMissSound
        {
            get
            {
                return 0x238;
            }
        }
        public override SkillName DefSkill
        {
            get
            {
                return SkillName.Fencing;
            }
        }
        public override WeaponType DefType
        {
            get
            {
                return WeaponType.Piercing;
            }
        }
        public override WeaponAnimation DefAnimation
        {
            get
            {
                return WeaponAnimation.Pierce2H;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnHit(Mobile attacker, IDamageable defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            // Desabilitado powerblow
            /*
            if (!Core.AOS && defender is Mobile && this.Layer == Layer.TwoHanded && (attacker.Skills[SkillName.Anatomy].Value / 400.0) >= Utility.RandomDouble())
            {
                ((Mobile)defender).SendMessage("Voce recebe um golpe paralizante!"); // Is this not localized?
                ((Mobile)defender).Freeze(TimeSpan.FromSeconds(2.0));

                attacker.SendMessage("Voce causa um golpe paralizante!"); // Is this not localized?
                attacker.PlaySound(0x11C);
            }
            */

            if (WeaponAbility.GetCurrentAbility(attacker) is InfectiousStrike)
                return;

            if (!Core.AOS && defender is Mobile && this.Poison != null && this.PoisonCharges > 0)
            {
                if (Utility.RandomDouble() >= 0.1)
                {
                    --this.PoisonCharges;
                    if (attacker.Skills[SkillName.Poisoning].Value < 80)
                    {
                        if (this.Poison == Poison.Lethal)
                            ((Mobile)defender).ApplyPoison(attacker, Poison.Greater);
                        if (this.Poison == Poison.Deadly)
                            ((Mobile)defender).ApplyPoison(attacker, Poison.Regular);
                        else
                            ((Mobile)defender).ApplyPoison(attacker, Poison.Lesser);
                    }
                    else
                    {
                        ((Mobile)defender).ApplyPoison(attacker, this.Poison);
                    }
                }
            }
        }
    }
}
