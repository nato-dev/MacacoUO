using System;
using System.Collections.Generic;
using Server.Events.Halloween;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Events
{
    public class TrickOrTreat
    {
        public static TimeSpan OneSecond = TimeSpan.FromSeconds(1);
        public static void Initialize()
        {
            DateTime now = DateTime.UtcNow;

            if (DateTime.UtcNow >= HolidaySettings.StartHalloween && DateTime.UtcNow <= HolidaySettings.FinishHalloween)
            {
                EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
            }
        }

        public static void Bleeding(Mobile m_From)
        {
            if (TrickOrTreat.CheckMobile(m_From))
            {
                if (m_From.Location != Point3D.Zero)
                {
                    int amount = Utility.RandomMinMax(3, 7);

                    for (int i = 0; i < amount; i++)
                    {
                        new Blood(Utility.RandomMinMax(0x122C, 0x122F)).MoveToWorld(RandomPointOneAway(m_From.X, m_From.Y, m_From.Z, m_From.Map), m_From.Map);
                    }
                }
            }
        }

        public static void RemoveHueMod(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                target.SolidHueOverride = -1;
            }
        }

        public static void SolidHueMobile(Mobile target)
        {
            if (CheckMobile(target))
            {
                target.SolidHueOverride = Utility.RandomMinMax(2501, 2644);

                Timer.DelayCall<Mobile>(TimeSpan.FromSeconds(10), new TimerStateCallback<Mobile>(RemoveHueMod), target);
            }
        }

        public static void MakeTwin(Mobile m_From)
        {
            List<Item> m_Items = new List<Item>();

            if (CheckMobile(m_From))
            {
                Mobile twin = new NaughtyTwin(m_From);

                if (twin != null && !twin.Deleted)
                {
                    foreach (Item item in m_From.Items)
                    {
                        if (item.Layer != Layer.Backpack && item.Layer != Layer.Mount && item.Layer != Layer.Bank)
                        {
                            m_Items.Add(item);
                        }
                    }

                    if (m_Items.Count > 0)
                    {
                        for (int i = 0; i < m_Items.Count; i++) /* dupe exploits start out like this ... */
                        {
                            Item item = null;

                            try
                            {
                                item = Activator.CreateInstance(m_Items[i].GetType()) as Item;
                            }
                            catch { continue; }

                            if (item != null)
                            {
                                item.Hue = m_Items[i].Hue;
                                item.Name = m_Items[i].Name;
                                item.ItemID = m_Items[i].ItemID;
                                item.LootType = m_Items[i].LootType;

                                twin.AddItem(item);

                                if (item.Layer != Layer.Backpack && item.Layer != Layer.Mount && item.Layer != Layer.Bank)
                                {
                                    item.Movable = false;
                                }
                            }
                        }
                    }

                    twin.Hue = m_From.Hue;
                    twin.BodyValue = m_From.BodyValue;
                    twin.Kills = m_From.Kills;

                    Point3D point = RandomPointOneAway(m_From.X, m_From.Y, m_From.Z, m_From.Map);

                    twin.MoveToWorld(m_From.Map.CanSpawnMobile(point) ? point : m_From.Location, m_From.Map);

                    Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerStateCallback<Mobile>(DeleteTwin), twin);
                }
            }
        }

        public static void DeleteTwin(Mobile m_Twin)
        {
            if (TrickOrTreat.CheckMobile(m_Twin))
            {
                m_Twin.Delete();
            }
        }

        public static Point3D RandomPointOneAway(int x, int y, int z, Map map)
        {
            Point3D loc = new Point3D(x + Utility.Random(-1, 3), y + Utility.Random(-1, 3), 0);

            loc.Z = (map.CanFit(loc, 0)) ? map.GetAverageZ(loc.X, loc.Y) : z;

            return loc;
        }

        public static bool CheckMobile(Mobile mobile)
        {
            return (mobile != null && mobile.Map != null && !mobile.Deleted && mobile.Alive && mobile.Map != Map.Internal);
        }

        private static void EventSink_Speech(SpeechEventArgs e)
        {
            if (Insensitive.Contains(e.Speech, "doces ou travessuras"))
            {
                e.Mobile.Target = new TrickOrTreatTarget();
                e.Mobile.SendLocalizedMessage(1076764);  /* Pick someone to Trick or Treat. */
            }
        }

        private class TrickOrTreatTarget : Target
        {
            public TrickOrTreatTarget()
                : base(15, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (targ != null && CheckMobile(from))
                {
                    if (!(targ is Mobile))
                    {
                        from.SendLocalizedMessage(1076781); /* There is little chance of getting candy from that! */
                        return;
                    }
                    if (!(targ is BaseVendor) || ((BaseVendor)targ).Deleted)
                    {
                        from.SendLocalizedMessage(1076765); /* That doesn't look friendly. */
                        return;
                    }

                    DateTime now = DateTime.UtcNow;

                    BaseVendor m_Begged = targ as BaseVendor;

                    if (CheckMobile(m_Begged))
                    {
                        if (m_Begged.NextTrickOrTreat > now)
                        {
                            from.SendLocalizedMessage(1076767); /* That doesn't appear to have any more candy. */
                            return;
                        }

                        m_Begged.NextTrickOrTreat = now + TimeSpan.FromMinutes(Utility.RandomMinMax(5, 10));

                        if (from.Backpack != null && !from.Backpack.Deleted)
                        {
                            if (Utility.RandomDouble() > .10)
                            {
                                switch( Utility.Random(3) )
                                {
                                    case 0:
                                        m_Begged.Say(1076768);
                                        break; /* Oooooh, aren't you cute! */
                                    case 1:
                                        m_Begged.Say(1076779);
                                        break; /* All right...This better not spoil your dinner! */
                                    case 2:
                                        m_Begged.Say(1076778);
                                        break; /* Here you go! Enjoy! */
                                    default:
                                        break;
                                }

                                if (Utility.RandomDouble() <= .06 && from.Skills.Begging.Value >= 100)
                                {
                                    from.AddToBackpack(HolidaySettings.RandomGMBeggerItem);

                                    from.SendLocalizedMessage(1076777); /* You receive a special treat! */
                                }
                                else
                                {
                                    from.AddToBackpack(HolidaySettings.RandomTreat);

                                    from.SendLocalizedMessage(1076769);   /* You receive some candy. */
                                }
                            }
                            else
                            {
                                m_Begged.Say("Travessura !!"); /* TRICK! */

                                int m_Action = Utility.Random(4);

                                if (m_Action == 0)
                                {
                                    Timer.DelayCall<Mobile>(OneSecond, OneSecond, 10, new TimerStateCallback<Mobile>(Bleeding), from);
                                }
                                else if (m_Action == 1)
                                {
                                    Timer.DelayCall<Mobile>(TimeSpan.FromSeconds(2), new TimerStateCallback<Mobile>(SolidHueMobile), from);
                                }
                                else
                                {
                                    Timer.DelayCall<Mobile>(TimeSpan.FromSeconds(2), new TimerStateCallback<Mobile>(MakeTwin), from);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class NaughtyTwin : BaseCreature
    {
       
        private readonly Mobile m_From;
        public NaughtyTwin(Mobile from)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            if (TrickOrTreat.CheckMobile(from))
            {
                this.Body = from.Body;

                this.m_From = from;
                this.Name = String.Format("{0}\'s Gemeo Do Mal", from.Name);

                Timer.DelayCall<Mobile>(TrickOrTreat.OneSecond, new TimerStateCallback<Mobile>(StealCandy), this.m_From);
            }
        }

        public NaughtyTwin(Serial serial)
            : base(serial)
        {
        }

        public static Item FindCandyTypes(Mobile target)
        {
            Type[] types = { typeof(WrappedCandy), typeof(Lollipops), typeof(NougatSwirl), typeof(Taffy), typeof(JellyBeans) };

            if (TrickOrTreat.CheckMobile(target))
            {
                for (int i = 0; i < types.Length; i++)
                {
                    Item item = target.Backpack.FindItemByType(types[i]);

                    if (item != null)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public static void StealCandy(Mobile target)
        {
            if (TrickOrTreat.CheckMobile(target))
            {
                Item item = FindCandyTypes(target);

                target.SendLocalizedMessage(1113967); /* Your naughty twin steals some of your candy. */

                if (item != null && !item.Deleted)
                {
                    item.Delete();
                }
            }
        }

        public override void OnThink()
        {
            if (this.m_From == null || this.m_From.Deleted)
            {
                this.Delete();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
