using System;
using Server.Targets;

namespace Server.Items
{
    public abstract class BaseKnife : BaseMeleeWeapon
    {
        public BaseKnife(int itemID)
            : base(itemID)
        {
        }

        public BaseKnife(Serial serial)
            : base(serial)
        {
        }

        public override int DefHitSound
        {
            get
            {
                return 0x23B;
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
                return SkillName.Swords;
            }
        }
        public override WeaponType DefType
        {
            get
            {
                return WeaponType.Slashing;
            }
        }
        public override WeaponAnimation DefAnimation
        {
            get
            {
                return WeaponAnimation.Slash1H;
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

        public override void OnDoubleClick(Mobile from)
        {
            from.SendLocalizedMessage(1010018); // What do you want to use this item on?

            from.Target = new BladedItemTarget(this);
        }

        public override void OnHit(Mobile attacker, IDamageable defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (WeaponAbility.GetCurrentAbility(attacker) is InfectiousStrike)
                return;

            if (!Core.AOS && defender is Mobile && this.Poison != null && this.PoisonCharges > 0)
            {
                if (Utility.RandomDouble() <= 0.1)
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
