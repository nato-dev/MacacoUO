using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Spells.Eighth
{
    public class EarthquakeSpell : MagerySpell
    {
        public override DamageType SpellDamageType { get { return DamageType.SpellAOE; } }

        private static readonly SpellInfo m_Info = new SpellInfo(
            "Earthquake", "In Vas Por",
            233,
            9012,
            false,
            Reagent.Bloodmoss,
            Reagent.Ginseng,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh);

        public EarthquakeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Eighth;
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
            if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
            {
                var targets = AcquireIndirectTargets(Caster.Location, 1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0));
                bool foi = false;
                foreach (var id in targets)
                {
                    if (!Caster.InLOS(id))
                        continue;

                    Mobile m = id as Mobile;

                    int damage;

                    if (Core.AOS)
                    {
                        damage = id.Hits / 2;

                        if (m == null || !m.Player)
                            damage = Math.Max(Math.Min(damage, 100), 15);
                        damage += Utility.RandomMinMax(0, 15);
                    }
                    else
                    {
                        damage = (id.Hits * 6) / 10;

                        if ((m == null || !m.Player) && damage < 40)
                            damage = 40;
                        else if (id is PlayerMobile && damage > 75)
                            damage = 75;
                        else if (damage > 100)
                            damage = 100;
                    }

                    var mob = id as Mobile;
                    Timer.DelayCall(TimeSpan.FromSeconds(0.6), () =>
                    {
                        mob.Freeze(TimeSpan.FromSeconds(1));
                        mob.OverheadMessage("* atordoado *");
                    });

                    Caster.DoHarmful(id);
                    id.PlaySound(0x20D);
                    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(id.X, id.Y, id.Z + 20), id.Map), id, 0x11B6, 5, 20, true, true, 0, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    SpellHelper.Damage(this, id, damage, 100, 0, 0, 0, 0, Items.ElementoPvM.Terra);
                    foi = true;
                }

                if (!foi)
                {
                    Caster.PlaySound(0x20D);
                }

            }

            FinishSequence();
        }
    }
}
