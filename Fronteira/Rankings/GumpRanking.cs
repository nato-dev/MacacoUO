
//////////////////////////////////////////////////////////////////////

using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Engines.Points;
using System.Collections.Generic;
using Server.Ziden;

namespace Server.Gumps
{
    public class RankCtx
    {
        public int Page = 0;
        public string Title = "Rank";
        public List<PointsEntry> Rank;

        public bool HasNext()
        {
            return (Page + 1) * 8 < Rank.Count;
        }

        public bool HasPrev()
        {
            return Page > 0;
        }
    }

    public class Rankings : Gump
    {
        public static Dictionary<Mobile, RankCtx> contextos = new Dictionary<Mobile, RankCtx>();

        public static void Initialize()
        {
            CommandSystem.Register("Ranking", AccessLevel.Player, new CommandEventHandler(Ranking_OnCommand));
        }

        [Usage("Ranking")]
        [Description("Makes a call to your custom gump.")]
        public static void Ranking_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new Rankings(e.Mobile, null));
        }

        public RankCtx Ctx;

        public Rankings(Mobile m, RankCtx ctx) : base(0, 0)
        {
            if(ctx == null)
            {
                ctx = new RankCtx();
                ctx.Rank = RankFama.Rank;
                ctx.Page = 0;
            }
            this.Ctx = ctx;
            contextos[m] = ctx;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(122, 78, 485, 728, 9200);
            AddBackground(133, 173, 457, 511, 9350);

            if(ctx.Page == 0)
            {
                AddItem(132, 171, 4648, 0x8A5);
                AddItem(132, 228, 4646);
                AddItem(132, 277, 4644, 2415);
            }
          
            var start = ctx.Page * 8;
            var end = start + 8;
  
            var bump = 0;
            for (var i = start; i < end; i++)
            {
                if(i >= ctx.Rank.Count)
                {
                    AddHtml(190, 186 + bump, 224, 17, "-", (bool)false, (bool)false);
                } else
                {
                    var entry = ctx.Rank[i];
                    AddHtml(190, 186 + bump, 224, 17, (i+1) +" - "+entry.Player.Name, (bool)false, (bool)false);
                    AddHtml(426, 187 + bump, 139, 17, entry.Points.ToString(), (bool)false, (bool)false);
                }
                bump += 55;
            }

            AddHtml(132, 139, 146, 24, @" PvM", (bool)true, (bool)false);
            AddButton(247, 140, 4005, 4005, (int)Buttons.PvM, GumpButtonType.Reply, 0);
            AddHtml(132, 107, 146, 24, @"Fama", (bool)true, (bool)false);
            AddButton(247, 108, 4005, 4005, (int)Buttons.Fama, GumpButtonType.Reply, 0);


            AddBackground(208, 65, 284, 30, 9200);
            AddHtml(266, 77, 166, 24, "Ranking de "+ctx.Title, (bool)false, (bool)false);
            AddImage(72, 334, 10440);
            AddImage(575, 320, 10441);
            AddHtml(146, 692, 430, 56, @"Os 3 primeiros jogadores irao ganhar trofeis e recompensas UNICAS !", (bool)true, (bool)false);
            AddHtml(285, 108, 146, 24, @"--", (bool)true, (bool)false);

            AddHtml(446, 108, 146, 24, @"Trabalho", (bool)true, (bool)false);
            AddButton(560, 108, 4005, 4005, (int)Buttons.Trabalho, GumpButtonType.Reply, 0);

            AddButton(247, 140, 4005, 4005, (int)Buttons.PvM, GumpButtonType.Reply, 0);
            AddHtml(285, 138, 146, 24, @"--", (bool)true, (bool)false);
            AddHtml(445, 138, 146, 24, @"--", (bool)true, (bool)false);
            AddButton(566, 767, 4005, 4005, (int)Buttons.Prox, GumpButtonType.Reply, 0);
            AddButton(524, 767, 4005, 4005, (int)Buttons.Ant, GumpButtonType.Reply, 0);
        }

        public enum Buttons
        {
            Fechar,
            PvM,
            Trabalho,
            Fama,
            Prox,
            Ant
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {

            RankCtx ctx = null;
            contextos.TryGetValue(sender.Mobile, out ctx);

            if (ctx == null)
            {
                return;
            }
            var from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.PvM:
                    {
                        ctx.Page = 0;
                        ctx.Title = "PvM";
                        ctx.Rank = PointsSystem.PontosPvmEterno.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }
                case (int)Buttons.Fama:
                    {
                        ctx.Page = 0;
                        ctx.Title = "Fama";
                        ctx.Rank = RankFama.Rank;
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }

                case (int)Buttons.Trabalho:
                    {
                        ctx.Page = 0;
                        ctx.Title = "Trabalho";
                        ctx.Rank = PointsSystem.PontosTrabalho.GetOrCalculateRank();
                        sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        break;
                    }

                case (int)Buttons.Prox:
                    {
                        if(ctx.HasNext())
                        {
                            ctx.Page++;
                            sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        } else
                        {
                            from.SendMessage("Maximo...");
                            sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        }
                        break;
                    }

                case (int)Buttons.Ant:
                    {
                        if (ctx.HasPrev())
                        {
                            ctx.Page--;
                            from.CloseGump(typeof(Rankings));
                            sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                            from.SendMessage("Minimo...");
                        } else
                        {
                            sender.Mobile.SendGump(new Rankings(sender.Mobile, ctx));
                        }
                        break;
                    }


            }
        }
    }
}
