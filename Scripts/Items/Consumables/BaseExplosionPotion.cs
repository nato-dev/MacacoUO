#region References
using System;
using System.Linq;
using System.Collections;

using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Dueling;
#endregion

namespace Server.Items
{
    public abstract class BaseExplosionPotion : BasePotion
    {
        private const int ExplosionRange = 2; // How long is the blast radius?
        private Timer m_Timer;
        private ArrayList m_Users;

        public BaseExplosionPotion(PotionEffect effect)
            : base(0xF0D, effect)
        { }

        public BaseExplosionPotion(Serial serial)
            : base(serial)
        { }

        public abstract int MinDamage { get; }
        public abstract int MaxDamage { get; }
        public override bool RequireFreeHand { get { return false; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public virtual object FindParent(Mobile from)
        {
            Mobile m = HeldBy;

            if (m != null && m.Holding == this)
            {
                return m;
            }

            object obj = RootParent;

            if (obj != null)
            {
                return obj;
            }

            if (Map == Map.Internal)
            {
                return from;
            }

            return this;
        }

        public static Dictionary<Mobile, Item> Using = new Dictionary<Mobile, Item>();

        public static bool Throwing(Mobile from)
        {
            if (from.Region.IsPartOf("New Haven"))
            {
                from.SendMessage("Voce nao pode fazer isto aqui");
                return false;
            }

            return Using.ContainsKey(from);
        }

        public override void Drink(Mobile from)
        {

            if (from.Region.IsPartOf("New Haven"))
            {
                from.SendMessage("Voce nao pode fazer isto aqui");
                return;
            }

            if (from.Spell != null && from.Spell.IsCasting)
            {
                from.SendMessage("Voce nao pode usar uma pocao de explosao enquanto conjura magias"); // You can not use a purple potion while paralyzed.
                return;
            }

            ThrowTarget targ = from.Target as ThrowTarget;
            Stackable = false; // Scavenged explosion potions won't stack with those ones in backpack, and still will explode.

            if (targ != null && targ.Potion == this)
            {
                return;
            }

            from.RevealingAction();

            if (m_Users == null)
            {
                m_Users = new ArrayList();
            }

            if (!m_Users.Contains(from))
            {
                m_Users.Add(from);
            }

            from.Target = new ThrowTarget(this);

            if (m_Timer == null)
            {
                from.SendMessage("Aguarde para jogar no tempo correto !"); // You should throw it now!
                Using.Add(from, this);
                var tempo = 4;
                if (Utility.RandomBool())
                    tempo = 3;
                m_Timer = Timer.DelayCall(
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(1),
                        tempo + 1,
                        new TimerStateCallback(Detonate_OnTick),
                        new object[] { from, tempo }
                );

                /*
				if (Core.ML)
				{
					m_Timer = Timer.DelayCall(
						TimeSpan.FromSeconds(1.0),
						TimeSpan.FromSeconds(1.25),
						5,
						new TimerStateCallback(Detonate_OnTick),
						new object[] {from, 3}); // 3.6 seconds explosion delay
				}
				else
				{
					m_Timer = Timer.DelayCall(
						TimeSpan.FromSeconds(0.75),
						TimeSpan.FromSeconds(1.0),
						4,
						new TimerStateCallback(Detonate_OnTick),
						new object[] {from, 3}); // 2.6 seconds explosion delay
				}
                */
            }
        }

        public void Explode(Mobile from, bool direct, Point3D loc, Map map)
        {
            Using.Remove(from);

            if (Deleted)
            {
                return;
            }

            bool damageThrower = false;

            if (from != null)
            {
                if (from.Target is ThrowTarget && ((ThrowTarget)from.Target).Potion == this)
                {
                    Target.Cancel(from);
                }

                if (IsChildOf(from.Backpack) || Parent == from)
                {
                    damageThrower = true;
                }
            }

            Consume();

            for (int i = 0; m_Users != null && i < m_Users.Count; ++i)
            {
                Mobile m = (Mobile)m_Users[i];
                ThrowTarget targ = m.Target as ThrowTarget;

                if (targ != null && targ.Potion == this)
                {
                    Target.Cancel(m);
                }
            }

            if (map == null)
            {
                return;
            }

            Effects.PlaySound(loc, map, 0x307);

            Effects.SendLocationEffect(loc, map, 0x36B0, 9, 10, 0, 0);
            int alchemyBonus = 0;

            var skill = from.Skills.Alchemy.Value;
            if (skill > 100)
                skill = 100;
            alchemyBonus = (int)(skill / 10);

            int min = Scale(from, MinDamage);
            int max = Scale(from, MaxDamage);

            var list = SpellHelper.AcquireIndirectTargets(from, loc, map, ExplosionRange, true, true).OfType<Mobile>().ToList();

            if (from != null && damageThrower && !list.Contains(from))
            {
                list.Add(from);
            }

            if (Shard.DebugEnabled)
            {
                Shard.Debug("Alvos da pot de explo: " + string.Join(",", list.Select(m => m.Name).ToList()));
            }


            foreach (var m in list)
            {
                if (from != null)
                {
                    from.DoHarmful(m);
                }

                int damage = Utility.RandomMinMax(min, max);

                damage += alchemyBonus;

                if (damage > 35)
                {
                    damage = 35;
                }

                //else if (list.Count > 2)
                // {
                //    damage /= list.Count - 1;
                // }

                if (!m.Player && from.Player)
                {
                    damage += (int)(damage * from.GetBonusElemento(ElementoPvM.Agua));
                    var nivelColar = ColarElemental.GetNivel(from, ElementoPvM.Agua);
                    if (nivelColar > 0)
                        damage += 5;
                    damage += nivelColar * 5;
                }

                if (m is BaseCreature)
                {
                    damage *= (int)(2 + from.Skills[SkillName.Alchemy].Value / 33);
                }

                AOS.Damage(m, from, damage, 0, 100, 0, 0, 0, Server.DamageType.SpellAOE);
            }

            list.Clear();
        }

        private void Detonate_OnTick(object state)
        {
            var states = (object[])state;
            Mobile from = (Mobile)states[0];

            if (Deleted)
            {
                Using.Remove(from);
                return;
            }


            int timer = (int)states[1];

            object parent = FindParent(from);

            if (timer == 0)
            {
                Point3D loc;
                Map map;

                if (parent is Item)
                {
                    Item item = (Item)parent;

                    loc = item.GetWorldLocation();
                    map = item.Map;
                }
                else if (parent is Mobile)
                {
                    Mobile m = (Mobile)parent;

                    loc = m.Location;
                    map = m.Map;
                }
                else
                {
                    return;
                }

                Using.Remove(from);
                Explode(from, true, loc, map);
                m_Timer = null;
            }
            else
            {
                if (parent is Item)
                {
                    ((Item)parent).PublicOverheadMessage(MessageType.Regular, 0x22, false, timer.ToString());
                }
                else if (parent is Mobile)
                {
                    ((Mobile)parent).PublicOverheadMessage(MessageType.Regular, 0x22, false, timer.ToString());
                }

                states[1] = timer - 1;
            }
        }

        private void Reposition_OnTick(object state)
        {
            if (Deleted)
            {
                return;
            }

            var states = (object[])state;
            Mobile from = (Mobile)states[0];
            IPoint3D p = (IPoint3D)states[1];
            Map map = (Map)states[2];

            Point3D loc = new Point3D(p);
            MoveToWorld(loc, map);
        }

        private class ThrowTarget : Target
        {
            private readonly BaseExplosionPotion m_Potion;

            public ThrowTarget(BaseExplosionPotion potion)
                : base(8, true, TargetFlags.None)
            {
                m_Potion = potion;
            }

            public BaseExplosionPotion Potion { get { return m_Potion; } }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from.Region.IsPartOf("New Haven"))
                {
                    from.SendMessage("Voce nao pode fazer isto aqui");
                    return;
                }

                if (m_Potion.Deleted || m_Potion.Map == Map.Internal)
                {
                    return;
                }

                IPoint3D p = targeted as IPoint3D;

                if (p == null)
                {
                    return;
                }

                Map map = from.Map;

                if (map == null)
                {
                    return;
                }

                SpellHelper.GetSurfaceTop(ref p);

                from.RevealingAction();

                IEntity to;

                to = new Entity(Serial.Zero, new Point3D(p), map);

                if (p is Mobile)
                {
                    to = (Mobile)p;
                }

                Effects.SendMovingEffect(from, to, m_Potion.ItemID, 12, 0, false, false, m_Potion.Hue, 0);

                if (m_Potion.Amount > 1)
                {
                    Mobile.LiftItemDupe(m_Potion, 1);
                }

                m_Potion.Internalize();
                Timer.DelayCall(
                    TimeSpan.FromSeconds(0.25), new TimerStateCallback(m_Potion.Reposition_OnTick), new object[] { from, p, map });
            }
        }
    }
}
