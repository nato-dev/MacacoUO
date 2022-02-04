
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Ziden;
using Server.Items;

namespace Server.Gumps
{
    public class GumpEscolherElemento : Gump
    {

        public GumpEscolherElemento() : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(112, 89, 285, 162, 9200);
            AddHtml(122, 99, 71, 19, @"Fogo", (bool)false, (bool)false);
            AddHtml(194, 100, 71, 19, @"Raio", (bool)false, (bool)false);
            AddHtml(127, 177, 71, 19, @"Gelo", (bool)false, (bool)false);
            AddHtml(196, 177, 86, 19, @"Agua", (bool)false, (bool)false);
            AddHtml(334, 101, 71, 19, @"Terra", (bool)false, (bool)false);
            AddHtml(260, 101, 71, 19, @"Vento", (bool)false, (bool)false);
            AddHtml(265, 178, 71, 19, @"Luz", (bool)false, (bool)false);
            AddHtml(318, 178, 90, 19, @"Escuridao", (bool)false, (bool)false);
            AddButton(261, 196, 20742, 20742, (int)Buttons.Luz, GumpButtonType.Reply, 0);
            AddButton(193, 123, 2288, 2289, (int)Buttons.Raio, GumpButtonType.Reply, 0);
            AddButton(122, 195, 21002, 21002, (int)Buttons.Gelo, GumpButtonType.Reply, 0);
            AddButton(195, 195, 2303, 2303, (int)Buttons.Agua, GumpButtonType.Reply, 0);
            AddButton(331, 124, 2294, 2294, (int)Buttons.Terra, GumpButtonType.Reply, 0);
            AddButton(261, 123, 20740, 2267, (int)Buttons.Vento, GumpButtonType.Reply, 0);
            AddButton(123, 124, 2245, 2267, (int)Buttons.Fogo, GumpButtonType.Reply, 0);
            AddButton(334, 197, 20495, 20495, (int)Buttons.Escuridao, GumpButtonType.Reply, 0);



        }

        public enum Buttons
        {
            Nada,
            Luz,
            Raio,
            Gelo,
            Agua,
            Terra,
            Vento,
            Fogo,
            Escuridao,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.Luz:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Luz";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Luz];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Raio:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Raio";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Raio];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Gelo:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Gelo";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Gelo];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Agua:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Agua";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Agua];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Terra:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Terra";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Terra];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Vento:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Vento";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Vento];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Fogo:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Fogo";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Fogo];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Escuridao:
                    {
                        var ctx = new RankCtx();
                        ctx.Page = 0;
                        ctx.Title = "Escuridao";
                        ctx.Rank = RankElementos.RankPorElemento[ElementoPvM.Escuridao];
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }

            }
        }
    }
}
