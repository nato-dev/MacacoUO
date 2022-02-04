
using Server.Engines.Points;
using Server.Network;
using System;

namespace Server.Ziden.Kills
{
    public class PontosRP : PointsSystem
    {
        public override TextDefinition Name { get { return "Pontos RP"; } }
        public override PointsType Loyalty { get { return PointsType.RP; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;

        public override void Serialize(GenericWriter writer)
        {
            Shard.Debug("Salvando pontos de RP");
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            Shard.Debug("Carregando pontos de RP");
            base.Deserialize(reader);
            this.GetOrCalculateRank();
        }
    }
}
