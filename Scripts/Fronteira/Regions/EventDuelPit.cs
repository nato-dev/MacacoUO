using System;
using System.Xml;

namespace Server.Regions
{
    class EventDuelPitsRegion : NoHousingRegion  // this region will flag people gray. made for duel pits and whatever else you need to flag everyone gray in. (runuo shard/data/regions.xml)
    {
        public EventDuelPitsRegion(XmlElement element, Map map, Region parent) : base(element, map, parent) { }

        public override void OnEnter(Mobile m)
        {

            RoacheCriminalTimer criminalTimer = new RoacheCriminalTimer(m);
            criminalTimer.Start();

            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
        }

        private class RoacheCriminalTimer : Timer
        {
            private Mobile player;

            public RoacheCriminalTimer(Mobile m)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                player = m;
            }

            protected override void OnTick()
            {
                if (player.Region.IsPartOf("EventDuelPits"))
                    player.Criminal = true;
                else
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(MakeUnCriminal));
                    this.Stop();
                }
            }

            private void MakeUnCriminal()
            {
                player.Criminal = false;
            }
        }
    }
}
