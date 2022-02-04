using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public class ArmsLore
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.ArmsLore].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();

            m.SendLocalizedMessage(500349); // What item do you wish to get information about?

            return TimeSpan.FromSeconds(1.0);
        }

        [PlayerVendorTarget]
        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(2, false, TargetFlags.None)
            {
                this.AllowNonlocal = true;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseWeapon)
                {
                    if (from.CheckTargetSkillMinMax(SkillName.ArmsLore, targeted, 0, 100))
                    {
                        BaseWeapon weap = (BaseWeapon)targeted;

                        if (weap.MaxHitPoints != 0)
                        {
                            int hp = (int)((weap.HitPoints / (double)weap.MaxHitPoints) * 10);

                            if (hp < 0)
                                hp = 0;
                            else if (hp > 9)
                                hp = 9;

                            from.SendMessage("Durabilidade: " + weap.GetDurabString());
                        }

                        int damage = (weap.MaxDamage + weap.MinDamage) / 2;
                        int hand = (weap.Layer == Layer.OneHanded ? 0 : 1);

                        from.SendMessage("Dano: " + weap.MinDamage + "-" + weap.MaxDamage);
                        from.SendMessage("Velocidade: " + weap.Speed);

                        if (weap.Poison != null && weap.PoisonCharges > 0)
                            from.SendMessage("Aparentemente uma arma envenada com "+weap.PoisonCharges+" cargas de veneno"); // It appears to have poison smeared on it.
                    }
                    else
                    {
                        from.SendMessage("Voce nao tem certeza..."); // You are not certain...
                    }
                }
                else if (targeted is BaseArmor)
                {
                    if (from.CheckTargetSkillMinMax(SkillName.ArmsLore, targeted, 0, 100))
                    {
                        BaseArmor arm = (BaseArmor)targeted;

                        if (arm.MaxHitPoints != 0)
                        {
                            int hp = (int)((arm.HitPoints / (double)arm.MaxHitPoints) * 10);

                            if (hp < 0)
                                hp = 0;
                            else if (hp > 9)
                                hp = 9;

                            from.SendMessage("HP da armadura:" + hp);
                        }

                        from.SendMessage("Rating de armadura :" + (int)Math.Ceiling(Math.Min(arm.ArmorRating, 35) / 5.0));
                        /*
                        if ( arm.ArmorRating < 1 )
                        from.SendLocalizedMessage( 1038295 ); // This armor offers no defense against attackers.
                        else if ( arm.ArmorRating < 6 )
                        from.SendLocalizedMessage( 1038296 ); // This armor provides almost no protection.
                        else if ( arm.ArmorRating < 11 )
                        from.SendLocalizedMessage( 1038297 ); // This armor provides very little protection.
                        else if ( arm.ArmorRating < 16 )
                        from.SendLocalizedMessage( 1038298 ); // This armor offers some protection against blows.
                        else if ( arm.ArmorRating < 21 )
                        from.SendLocalizedMessage( 1038299 ); // This armor serves as sturdy protection.
                        else if ( arm.ArmorRating < 26 )
                        from.SendLocalizedMessage( 1038300 ); // This armor is a superior defense against attack.
                        else if ( arm.ArmorRating < 31 )
                        from.SendLocalizedMessage( 1038301 ); // This armor offers excellent protection.
                        else
                        from.SendLocalizedMessage( 1038302 ); // This armor is superbly crafted to provide maximum protection.
                        * */
                    }
                    else
                    {
                        from.SendLocalizedMessage(500353); // You are not certain...
                    }
                }
                else if (targeted is SwampDragon && ((SwampDragon)targeted).HasBarding)
                {
                    SwampDragon pet = (SwampDragon)targeted;

                    if (from.CheckTargetSkillMinMax(SkillName.ArmsLore, targeted, 0, 100))
                    {
                        int perc = (4 * pet.BardingHP) / pet.BardingMaxHP;

                        if (perc < 0)
                            perc = 0;
                        else if (perc > 4)
                            perc = 4;

                        pet.PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1053021 - perc, from.NetState);
                    }
                    else
                    {
                        from.SendLocalizedMessage(500353); // You are not certain...
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500352); // This is neither weapon nor armor.
                }
            }
        }
    }
}
