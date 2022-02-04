using System;
using System.Collections.Generic;
using System.Linq;

using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Necromancy
{
    public class WitherSpell : NecromancerSpell
    {
        public override DamageType SpellDamageType { get { return DamageType.SpellAOE; } }

        private static readonly SpellInfo m_Info = new SpellInfo(
            "Wither", "Kal Vas An Flam",
            203,
            9031,
            Reagent.NoxCrystal,
            Reagent.GraveDust,
            Reagent.PigIron);
        public WitherSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds(1.5);
            }
        }
        public override double RequiredSkill
        {
            get
            {
                return 60.0;
            }
        }
        public override int RequiredMana
        {
            get
            {
                return 23;
            }
        }
        public override bool DelayedDamage
        {
            get
            {
                return false;
            }
        }
        public override void OnCast()
        {
            if (this.CheckSequence())
            {
                /* Creates a withering frost around the Caster,
                * which deals Cold Damage to all valid targets in a radius of 5 tiles.
                */
                Map map = this.Caster.Map;

                if (map != null)
                {
                    Effects.PlaySound(this.Caster.Location, map, 0x1FB);
                    Effects.PlaySound(this.Caster.Location, map, 0x10B);
                    Effects.SendLocationParticles(EffectItem.Create(this.Caster.Location, map, EffectItem.DefaultDuration), 0x37CC, 1, 40, 97, 3, 9917, 0);

                    foreach (var id in AcquireIndirectTargets(Caster.Location, 5))
                    {
                        Mobile target = id as Mobile;

                        if (target!=null && target.IsControlledBy(this.Caster))
                            continue;

                        var targetBc = target as BaseCreature;
                        if (targetBc != null && targetBc.ControlMaster == this.Caster)
                            continue;

                        if (target.Party != null && target.Party == this.Caster.Party)
                            continue;

                        var thisBc = this.Caster as BaseCreature;
                        if (thisBc != null && thisBc.ControlMaster == target)
                            continue;

                        if (thisBc != null && targetBc != null && thisBc.ControlMaster != null && thisBc.ControlMaster == targetBc.ControlMaster)
                            continue;

                        this.Caster.DoHarmful(id);

                        if (target != null)
                        {
                            //m.FixedParticles(0x374A, 1, 15, 9502, 97, 3, (EffectLayer)255);
                            Caster.MovingParticles(
                                target,//IEntity to
                                0x374A,//int itemID,
                                10, // int speed,
                                17, // int duration,
                                true, // bool fixedDirection,
                                true, // bool explodes,
                                97, // int hue,
                                9502, // int renderMode,
                                9502, // int effect,
                                9502, // int explodeEffect,
                                0x160, // int explodeSound,
                                0 // unkw
                                );
                        }
                        else
                        {
                            Effects.SendLocationParticles(id, 0x374A, 1, 30, 97, 3, 9502, 0);
                        }

                        double damage = Utility.RandomMinMax(10, 20);
                        damage *= GetDamageScalar(target, ElementoPvM.Gelo);

                        int karma = target != null ? target.Karma / 100 : 0;

                        damage *= 300 + karma + (this.GetDamageSkill(this.Caster) * 10);
                        damage /= 1000;
                        damage *= 0.75;

                        if(target != null)
                        {
                            if (CheckResisted(target, 7))
                            {
                                target.SendMessage("Voce sente seu corpo resistindo a magia");
                                damage *= 0.55;
                            }
                        }

                        int sdiBonus;

                        if (Core.SE)
                        {
                            if (Core.SA)
                            {
                                sdiBonus = SpellHelper.GetSpellDamageBonus(Caster, target, CastSkill, target is PlayerMobile);
                            }
                            else
                            {
                                sdiBonus = AosAttributes.GetValue(this.Caster, AosAttribute.SpellDamage);

                                // PvP spell damage increase cap of 15% from an item’s magic property in Publish 33(SE)
                                if (id is PlayerMobile && this.Caster.Player && sdiBonus > 15)
                                    sdiBonus = 15;
                            }
                        }
                        else
                        {
                            sdiBonus = AosAttributes.GetValue(this.Caster, AosAttribute.SpellDamage);
                        }

                        damage *= (100 + sdiBonus);
                        damage /= 100;

                        var mobile = (Mobile)id;
                        mobile.Freeze(TimeSpan.FromSeconds(2));
                        mobile.SendMessage("A aura gelida te causa calafrios e seu corpo congela");
                        SpellHelper.Damage(this, id, damage, 0, 0, 100, 0, 0, ElementoPvM.Gelo);
                    }
                }
            }

            this.FinishSequence();
        }
    }
}
