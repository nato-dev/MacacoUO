using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Guildas
{
    public class GuerrasAtivas : Gump
    {
        public static int PerPage = 10;

        public PlayerMobile User { get; set; }

        public GuerrasAtivas(PlayerMobile pm) : base(50, 50)
        {
            User = pm;

            AddPage(0);
            AddBackground(0, 0, 560, 320, 9200);
            AddBackground(10, 10, 540, 300, 3000);

            AddHtml(0, 12, 560, 20, "<CENTER>Guerras Ativas</CENTER>", 0, false, false); // <DIV ALIGN=CENTER>Vice Vs Virtue - Guild Rankings</DIV>

            AddHtml(10, 55, 60, 20, "<DIV ALIGN=CENTER>#:</DIV>", 0, false, false); // <DIV ALIGN=CENTER>#:</DIV>
            AddHtml(50, 55, 180, 20, "<DIV ALIGN=CENTER>Guerra:</DIV>", 0, false, false); // <DIV ALIGN=CENTER>Guild:</DIV>
            AddHtml(230, 55, 100, 20, "<DIV ALIGN=RIGHT>MaxKills:</DIV>", 0, false, false); // <DIV ALIGN=RIGHT>Score:</DIV>
            AddHtml(330, 55, 85, 20, "<DIV ALIGN=RIGHT>Kills:</DIV>", 0, false, false); // <DIV ALIGN=RIGHT>Kills:</DIV>
            AddHtml(425, 55, 95, 20, "<DIV ALIGN=RIGHT>Horas Restantes:</DIV>", 0, false, false); // <DIV ALIGN=RIGHT>Returned Sigil:</DIV>


            AddImage(330, 55, 10006);


            AddImage(415, 55, 10006);

            AddImage(520, 55, 10006);

            AddButton(280, 290, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddHtml(315, 290, 150, 20, "Ranking de Guerras", 0, false, false); // Guild Rankings

            var list = Guild.Ativas.Where(w => w.WarRequester).ToList();

            int pages = (int)Math.Ceiling((double)list.Count / PerPage);
            int y = 75;
            int page = 1;
            int pageindex = 0;

            if (pages < 1)
                pages = 1;

            AddPage(page);

            AddHtml(60, 290, 150, 20, $"Pag. {page.ToString()}/{pages.ToString()}", 0, false, false); // Page ~1_CUR~ of ~2_MAX~

            var i = 0;
            foreach(var entry in list)
            {
                var op = entry.Opponent.FindActiveWar(entry.Guild);
                if (op == null)
                    continue;

                AddHtml(10, y, 65, 20, CenterGray((i + 1).ToString() + "."), false, false);
                AddHtml(50, y, 180, 20, CenterGray($"{(entry.Guild == null ? "?" : entry.Guild.Abbreviation)} vs {(op.Guild == null ? "?" : op.Guild.Abbreviation)}"), false, false);
                AddHtml(230, y, 100, 20, RightGray($"{entry.MaxKills.ToString()}"), false, false);
                AddHtml(330, y, 85, 20,RightGray($"{entry.Kills.ToString()} vs {op.Kills.ToString()}"), false, false);
                AddHtml(425, y, 95, 20, RightGreen(((int)(entry.TimeRemaining.TotalHours)).ToString()), false, false);

                y += 20;
                pageindex++;

                if (pageindex == PerPage)
                {
                    AddHtmlLocalized(60, 290, 150, 20, 1153561, String.Format("{0}\t{1}", page.ToString(), pages.ToString()), 0, false, false); // Page ~1_CUR~ of ~2_MAX~

                    if (i > 0 && i < list.Count - 1)
                    {
                        AddButton(200, 290, 4005, 4007, 0, GumpButtonType.Page, page + 1);

                        page++;
                        y = 75;
                        pageindex = 0;
                        AddPage(page);

                        AddButton(170, 290, 4014, 4016, 0, GumpButtonType.Page, page - 1);
                    }
                }
            }

            list.Clear();
            list.TrimExcess();
            i++;
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                default: break;
                case 1:
                case 2:
                case 4:
                    User.SendGump(new RankingGuildas(User));
                    break;
            }
        }

        private string CenterGray(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=CENTER>{0}</DIV>", format);
        }

        private string RightGray(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=RIGHT>{0}</DIV>", format);
        }

        private string LeftGray(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=LEFT>{0}</DIV>", format);
        }

        private string RightGreen(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=RIGHT>{0}</DIV>", format);
        }
    }

    public class RankingGuildas : Gump
    {
        public static List<Guild> Rank = null;

        public List<Guild> GetRank()
        {
            if(Rank == null)
            {

                var sources = new HashSet<WarDeclaration>();
                var targets = new HashSet<WarDeclaration>();
                var parzinhos = new Dictionary<WarDeclaration, WarDeclaration>();
                var pontuou = new HashSet<Guild>();

                Shard.Debug("Historico de guildas " + Guild.History.Count);

                foreach(var g in Guild.History)
                {
                    if(g.WarRequester)
                    {
                        sources.Add(g);
                    } else
                    {
                        targets.Add(g);
                    }
                }
                foreach(var g in sources)
                {
                    var op = targets.Where(t => t.Guild == g.Opponent).FirstOrDefault();
                    if (op != null)
                    {
                        targets.Remove(op);
                        parzinhos[g] = op;
                    }
                }

                Shard.Debug("Parzinhos " + parzinhos.Count);
                foreach (var atacante in parzinhos.Keys)
                {
                    if(atacante.Status == WarStatus.Win)
                    {
                         atacante.Guild.PontosRank++;
                        pontuou.Add(atacante.Guild);

                    } else
                    {
                        var def = parzinhos[atacante];
                        def.Guild.PontosRank++;
                        pontuou.Add(def.Guild);
                    }
                }
                Rank = pontuou.OrderByDescending(g => g.PontosRank).ToList();
            }
            return Rank;
        }

        public static int PerPage = 10;

        public PlayerMobile User { get; set; }

        public RankingGuildas(PlayerMobile pm) : base(50, 50)
        {
            User = pm;

            AddPage(0);
            AddBackground(0, 0, 560, 320, 9200);
            AddBackground(10, 10, 540, 300, 3000);

            AddHtml(0, 12, 560, 20, "<CENTER>Ranking Guerras</CENTER>", 0, false, false); // <DIV ALIGN=CENTER>Vice Vs Virtue - Guild Rankings</DIV>

            AddHtml(10, 55, 60, 20, "<DIV ALIGN=CENTER>#:</DIV>", 0, false, false); // <DIV ALIGN=CENTER>#:</DIV>
            AddHtml(50, 55, 180, 20, "<DIV ALIGN=CENTER>Guilda:</DIV>", 0, false, false); // <DIV ALIGN=CENTER>Guild:</DIV>
            AddHtml(230, 55, 100, 20, "<DIV ALIGN=RIGHT>Pontos:</DIV>", 0, false, false); // <DIV ALIGN=RIGHT>Score:</DIV>
            AddHtml(330, 55, 85, 20, "<DIV ALIGN=RIGHT>Tag:</DIV>", 0, false, false); // <DIV ALIGN=RIGHT>Kills:</DIV>
            AddHtml(425, 55, 95, 20, "<DIV ALIGN=RIGHT>-:</DIV>", 0, false, false); // <DIV ALIGN=RIGHT>Returned Sigil:</DIV>


            AddImage(330, 55, 10006);


            AddImage(415, 55, 10006);

            AddImage(520, 55, 10006);

            AddButton(280, 290, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddHtml(315, 290, 150, 20, "Guerras Ativas", 0, false, false); // Guild Rankings

            var list = GetRank();

            int pages = (int)Math.Ceiling((double)list.Count / PerPage);
            int y = 75;
            int page = 1;
            int pageindex = 0;

            if (pages < 1)
                pages = 1;

            AddPage(page);

            AddHtml(60, 290, 150, 20, $"Pag. {page.ToString()}/{pages.ToString()}", 0, false, false); // Page ~1_CUR~ of ~2_MAX~

            var i = 0;
            foreach (var entry in list)
            {


                AddHtml(10, y, 65, 20, CenterGray((i + 1).ToString() + "."), false, false);
                AddHtml(50, y, 180, 20, CenterGray($"{(entry == null ? "?" : entry.Name)}"), false, false);
                AddHtml(230, y, 100, 20, RightGray($"{entry.PontosRank.ToString()}"), false, false);
                AddHtml(330, y, 85, 20, RightGray($"{entry.Abbreviation}"), false, false);
                AddHtml(425, y, 95, 20, RightGreen("-"), false, false);

                y += 20;
                pageindex++;

                if (pageindex == PerPage)
                {
                    AddHtmlLocalized(60, 290, 150, 20, 1153561, String.Format("{0}\t{1}", page.ToString(), pages.ToString()), 0, false, false); // Page ~1_CUR~ of ~2_MAX~

                    if (i > 0 && i < list.Count - 1)
                    {
                        AddButton(200, 290, 4005, 4007, 0, GumpButtonType.Page, page + 1);

                        page++;
                        y = 75;
                        pageindex = 0;
                        AddPage(page);

                        AddButton(170, 290, 4014, 4016, 0, GumpButtonType.Page, page - 1);
                    }
                }
            }

            list.Clear();
            list.TrimExcess();
            i++;
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                default: break;
                case 1:
                case 2:
                case 4:
                    User.SendGump(new GuerrasAtivas(User));
                    break;
            }
        }

        private string CenterGray(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=CENTER>{0}</DIV>", format);
        }

        private string RightGray(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=RIGHT>{0}</DIV>", format);
        }

        private string LeftGray(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=LEFT>{0}</DIV>", format);
        }

        private string RightGreen(string format)
        {
            return String.Format("<basefont color=#000000><DIV ALIGN=RIGHT>{0}</DIV>", format);
        }
    }
}
