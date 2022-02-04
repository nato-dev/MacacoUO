using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Guilds;
using Server.Network;
using System.Collections.Generic;
using System.Linq;

namespace Server.Engines.VvV
{
    public enum Filter
    {
        Score,
        Kills,
        ReturnedSigils
    }

    public class ViceVsVirtueLeaderboardGump : Gump
    {
        public static int PerPage = 10;

        public PlayerMobile User { get; set; }
        public Filter Filter { get; set; }

        public ViceVsVirtueLeaderboardGump(PlayerMobile pm, Filter filter = Filter.Score) : base(50, 50)
        {
            User = pm;
            Filter = filter;

            AddPage(0);
            AddBackground(0, 0, 920, 320, 5054);
            AddImageTiled(10, 10, 900, 300, 2624);

            AddHtml(0, 12, 920, 20, "<CENTER>Guerra Infinita</CENTER>", 0xFFFF, false, false); // <DIV ALIGN=CENTER>Vice Vs Virtue - Player Rankings</DIV>

            AddHtml(10, 55, 65, 20, Center("#"), 0xFFFF, false, false); // <DIV ALIGN=CENTER>#:</DIV>
            AddHtml(70, 55, 160, 20, Center("Nome"), 0xFFFF, false, false); // <DIV ALIGN=LEFT>Name:</DIV>
            AddHtml(230, 55, 70, 20, Center("Guilda"), 0xFFFF, false, false); // <DIV ALIGN=CENTER>Guild:</DIV>
            AddHtml(300, 55, 100, 20, Center("Pontos"), Filter == Filter.Score ? Server.Engines.Quests.BaseQuestGump.C32216(0x00FA9A) : 0xFFFF, false, false); // <DIV ALIGN=RIGHT>Score:</DIV>
            AddHtml(420, 55, 55, 20, Center("Kills"), Filter == Filter.Kills ? Server.Engines.Quests.BaseQuestGump.C32216(0x00FA9A) : 0xFFFF, false, false); // <DIV ALIGN=RIGHT>Kills:</DIV>
            AddHtml(480, 55, 55, 20, Center("Mortes"), 0xFFFF, false, false); // <DIV ALIGN=RIGHT>Deaths:</DIV>
            AddHtml(540, 55, 55, 20, Center("Assists"), 0xFFFF, false, false); // <DIV ALIGN=RIGHT>Assists:</DIV>
            AddHtml(610, 55, 90, 20, Center("Sigilos"), Filter == Filter.ReturnedSigils ? Server.Engines.Quests.BaseQuestGump.C32216(0x00FA9A) : 0xFFFF, false, false); // <DIV ALIGN=RIGHT>Returned Sigil:</DIV>
            AddHtml(710, 55, 100, 20, Center("Armadilhas"), 0xFFFF, false, false); // <DIV ALIGN=RIGHT>Disarmed Traps:</DIV>
            AddHtml(810, 55, 80, 20, Center("Roubos"), 0xFFFF, false, false); // <DIV ALIGN=RIGHT>Stolen Sigil:</DIV>

            if (Filter != Filter.Score)
                AddButton(400, 55, 2437, 2438, 1, GumpButtonType.Reply, 0);
            else
                AddImage(400, 55, 10006);

            if (Filter != Filter.Kills)
                AddButton(475, 55, 2437, 2438, 2, GumpButtonType.Reply, 0);
            else
                AddImage(475, 55, 10006);

            if (Filter != Filter.ReturnedSigils)
                AddButton(700, 55, 2437, 2438, 3, GumpButtonType.Reply, 0);
            else
                AddImage(700, 55, 10006);

            AddButton(280, 290, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddHtml(315, 290, 150, 20, "Rank da Guilda", 0xFFFF, false, false); // Guild Rankings

            List<VvVPlayerEntry> list = new List<VvVPlayerEntry>(ViceVsVirtueSystem.Instance.PlayerTable.OfType<VvVPlayerEntry>());

            switch (Filter)
            {
                default:
                case Filter.Score: list = list.OrderBy(e => -e.Score).ToList(); break;
                case Filter.Kills: list = list.OrderBy(e => -e.Kills).ToList(); break;
                case Filter.ReturnedSigils: list = list.OrderBy(e => -e.ReturnedSigils).ToList(); break;
            }

            int pages = (int)Math.Ceiling((double)list.Count / PerPage);
            int y = 75;
            int page = 1;
            int pageindex = 0;

            if (pages < 1)
                pages = 1;

            AddPage(page);
            AddHtmlLocalized(60, 290, 150, 20, 1153561, String.Format("{0}\t{1}", page.ToString(), pages.ToString()), 0xFFFF, false, false); // Page ~1_CUR~ of ~2_MAX~

            for (int i = 0; i < list.Count; i++)
            {
                VvVPlayerEntry entry = list[i];

                if(entry.Player == null)
                    continue;

                Guild g = entry.Player.Guild as Guild;

                AddHtml(10, y, 65, 20, CenterGray((i + 1).ToString() + "."), false, false);
                AddHtml(70, y, 160, 20, LeftGray(entry.Player.Name), false, false);
                AddHtml(230, y, 70, 20, CenterGray(g == null ? "None" : g.Abbreviation), false, false);
                AddHtml(300, y, 100, 20, Filter == Filter.Score ? RightGreen(entry.Score.ToString()) : RightGray(entry.Score.ToString()), false, false);
                AddHtml(420, y, 55, 20, Filter == Filter.Kills ? RightGreen(entry.Kills.ToString()) : RightGray(entry.Kills.ToString()), false, false);
                AddHtml(480, y, 55, 20, RightGray(entry.Deaths.ToString()), false, false);
                AddHtml(540, y, 55, 20, RightGray(entry.Assists.ToString()), false, false);
                AddHtml(610, y, 90, 20, Filter == Filter.ReturnedSigils ? RightGreen(entry.ReturnedSigils.ToString()) : RightGray(entry.ReturnedSigils.ToString()), false, false);
                AddHtml(710, y, 100, 20, RightGray(entry.DisarmedTraps.ToString()), false, false);
                AddHtml(810, y, 80, 20, RightGray(entry.StolenSigils.ToString()), false, false);

                y += 20;
                pageindex++;

                if (pageindex == PerPage)
                {
                    AddHtmlLocalized(60, 290, 150, 20, 1153561, String.Format("{0}\t{1}", page.ToString(), pages.ToString()), 0xFFFF, false, false); // Page ~1_CUR~ of ~2_MAX~

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
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                default: break;
                case 1:
                case 2:
                case 3:
                    Filter f = (Filter)info.ButtonID - 1;
                    User.SendGump(new ViceVsVirtueLeaderboardGump(User, f));
                    break;
                case 4:
                    User.SendGump(new GuildLeaderboardGump(User));
                    break;
            }
        }

        private string Center(string format)
        {
            return String.Format("<DIV ALIGN=CENTER>{0}</DIV>", format);
        }

        private string CenterGray(string format)
        {
            return String.Format("<basefont color=#A9A9A9><DIV ALIGN=CENTER>{0}</DIV>", format);
        }

        private string RightGray(string format)
        {
            return String.Format("<basefont color=#A9A9A9><DIV ALIGN=RIGHT>{0}</DIV>", format);
        }

        private string LeftGray(string format)
        {
            return String.Format("<basefont color=#A9A9A9><DIV ALIGN=LEFT>{0}</DIV>", format);
        }

        private string RightGreen(string format)
        {
            return String.Format("<basefont color=#00FA9A><DIV ALIGN=RIGHT>{0}</DIV>", format);
        }
    }
}
