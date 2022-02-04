using Server.Network;
using Server.Engines.Points;

namespace Server.Gumps
{
    public class ZerarKillsGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public ZerarKillsGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
            this.AddPage(0);
            this.AddBackground(20, 32, 250, 120, 9200);
            this.AddLabel(47, 45, 1153, @"Tem certeza que deseja resetar");
            this.AddLabel(47, 65, 1153, @"o rank de Kills? Esta ação NÃO");
            this.AddLabel(47, 85, 1153, @"pode ser revertida.");
            this.AddButton(50, 120, 247, 248, 1, GumpButtonType.Reply, 0);
            this.AddButton(175, 120, 242, 242, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                var s = PointsSystem.PontosKills;
                var ladder = s.GetOrCalculateRank();
                foreach(var entry in ladder)
                {
                    s.DeductPoints(entry.Player, entry.Points);
                }
                sender.Mobile.SendMessage("Rank resetado com sucesso!");
            }
            else
            {
                sender.Mobile.SendMessage("Cancelado.");
            }
        }
    }
}
