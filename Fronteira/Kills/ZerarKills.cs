using Server.Commands;
using Server.Engines.Points;
using Server.Gumps;

namespace Server.Ziden.Kills
{
    public class ZerarKills : PointsSystem
    {
        public override TextDefinition Name { get { return "Zerar Kills"; } }
        public override PointsType Loyalty { get { return PointsType.PVPArena; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public static bool Enabled = Shard.WARSHARD;

        public static void Initialize()
        {
            CommandSystem.Register("ZerarKills", AccessLevel.Administrator, Cmd);
        }

        [Usage("ZerarKills")]
        private static void Cmd(CommandEventArgs e)
        {
           // if (Shard.WARSHARD) e.Mobile.SendGump(new ZerarKillsGump(e.Mobile));
        }
    }
}
