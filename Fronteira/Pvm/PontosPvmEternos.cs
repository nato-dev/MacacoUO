using Server.Commands;
using Server.Engines.Points;
using Server.Engines.VvV;
using Server.Gumps;
using Server.Mobiles;
using Server.Spells;
using System;

namespace Server.Ziden.Kills
{
    public class PontosPvmEternos : PointsSystem
    {
        public override TextDefinition Name { get { return "PvM Rank"; } }
        public override PointsType Loyalty { get { return PointsType.PvMEterno; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;

        public override void Serialize(GenericWriter writer)
        {
           
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
