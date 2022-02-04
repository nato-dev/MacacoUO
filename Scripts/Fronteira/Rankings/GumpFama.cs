using Server.Mobiles;
using Server.Engines.Points;
using Server.Network;
using Server.Ziden;

namespace Server.Gumps
{
    public class FamaGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public FamaGump(Mobile player)
            : base(GumpOffsetX, GumpOffsetY)
        {
            var pm = player as PlayerMobile;

            var ladder = RankFama.Rank;
            var points = pm.Fame;
            var rank = pm.RankingFama;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(28, 18, 384, 455, 2620);
            this.AddLabel(187, 25, 253, @"Ranking de Fama");
            this.AddLabel(33, 40, 1153, @"--------------------------------------------------------------");
            this.AddLabel(45, 55, 36, @"Posição");
            this.AddLabel(145, 55, 36, @"Jogador");
            this.AddLabel(264, 55, 36, @"Pontos"); //1 dígito X = 264 | 2 dígitos X = 267 | 3 dígitos X = 270
            this.AddLabel(33, 350, 1153, @"--------------------------------------------------------------");
            this.AddLabel(90, 380, 63, @"Sua Posição:");
            this.AddLabel(90, 405, 63, @"Pontos:");
            this.AddImageTiled(285, 130, 143, 136, 40082);
            //this.AddImage(285, 130, 30501); //outra imagem: death
            this.AddImage(285, 380, 9804);

            var r = 0;
            var y = 90;
            foreach (var entry in ladder)
            {
                r++;
                if (r > 10) break;
                if (entry.Points > 0)
                {
                    this.AddLabel(60, y, 1153, @r.ToString());
                    this.AddLabel(120, y + 1, 1153, @entry.Player.Name);
                    this.AddLabel(270, y + 1, 1153, @entry.Points.ToString());
                    y += 25;
                }
            }
            if (points > 0)
            {
                this.AddLabel(180, 381, 1153, (@rank + 1).ToString());
                this.AddLabel(180, 406, 1153, @points.ToString());
            }
            else
            {
                this.AddLabel(180, 381, 1153, @"-");
                this.AddLabel(180, 406, 1153, @"0");
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);
        }
    }
}
