using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Ziden;
using Server.Engines.Points;

namespace Server.Gumps
{
    public class GumpRankTrampo : Gump
    {
        public GumpRankTrampo() : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(186, 99, 225, 354, 9200);
            AddHtml(192, 109, 212, 334, @"", (bool)true, (bool)false);
            AddButton(200, 122, 1154, 1154, (int)Buttons.Geral, GumpButtonType.Reply, 0);
            AddHtml(239, 155, 153, 19, @"Ferreiro", (bool)false, (bool)false);
            AddButton(200, 190, 1154, 1154, (int)Buttons.Alq, GumpButtonType.Reply, 0);
            AddButton(200, 155, 1154, 1154, (int)Buttons.Ferr, GumpButtonType.Reply, 0);
            AddButton(199, 225, 1154, 1154, (int)Buttons.Carp, GumpButtonType.Reply, 0);
            AddButton(199, 262, 1154, 1154, (int)Buttons.Alfa, GumpButtonType.Reply, 0);
            AddHtml(239, 192, 153, 19, @"Alquimista", (bool)false, (bool)false);
            AddHtml(240, 228, 153, 19, @"Carpinteiro", (bool)false, (bool)false);
            AddHtml(240, 262, 153, 19, @"Alfaiate", (bool)false, (bool)false);
            AddHtml(238, 122, 153, 19, @"Geral", (bool)false, (bool)false);
            AddHtml(240, 297, 153, 19, @"Cozinheiro", (bool)false, (bool)false);
            AddButton(199, 332, 1154, 1154, (int)Buttons.Fish, GumpButtonType.Reply, 0);
            AddButton(199, 297, 1154, 1154, (int)Buttons.Cook, GumpButtonType.Reply, 0);
            AddButton(198, 368, 1154, 1154, (int)Buttons.Miner, GumpButtonType.Reply, 0);
            AddButton(198, 404, 1154, 1154, (int)Buttons.Lenha, GumpButtonType.Reply, 0);
            AddHtml(238, 334, 153, 19, @"Pescador", (bool)false, (bool)false);
            AddHtml(239, 370, 153, 19, @"Minerador", (bool)false, (bool)false);
            AddHtml(240, 404, 153, 19, @"Lenhador", (bool)false, (bool)false);
        }

        public enum Buttons
        {
            Nada,
            Geral,
            Alq,
            Ferr,
            Carp,
            Alfa,
            Fish,
            Cook,
            Miner,
            Lenha,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            var ctx = new RankCtx();
            ctx.Page = 0;

            switch (info.ButtonID)
            {
                case (int)Buttons.Geral:
                    {
                        ctx.Title = "Trabalho Geral";
                        ctx.Rank = PointsSystem.PontosTrabalho.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Alq:
                    {
                        ctx.Title = "Alquimistas";
                        ctx.Rank = PointsSystem.PontosAlquimista.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Ferr:
                    {
                        ctx.Title = "Ferreiros";
                        ctx.Rank = PointsSystem.PontosFerreiro.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Carp:
                    {
                        ctx.Title = "Carpinteiros";
                        ctx.Rank = PointsSystem.PontosCarpinteiro.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Alfa:
                    {
                        ctx.Title = "Alfaiates";
                        ctx.Rank = PointsSystem.PontosAlfaiate.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Fish:
                    {
                        ctx.Title = "Pescadores";
                        ctx.Rank = PointsSystem.PontosPescador.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Cook:
                    {
                        ctx.Title = "Cozinheiros";
                        ctx.Rank = PointsSystem.PontosCozinha.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Miner:
                    {
                        ctx.Title = "Mineradores";
                        ctx.Rank = PointsSystem.PontosMinerador.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Lenha:
                    {
                        ctx.Title = "Lenhadores";
                        ctx.Rank = PointsSystem.PontosLenhador.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }

            }
        }
    }
}
