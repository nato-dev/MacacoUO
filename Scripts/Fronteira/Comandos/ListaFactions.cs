using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Factions;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class ListaFactionsGump : Gump
    {
        private List<Faction> m_List;

        private int m_Page;

        private const int LabelColor = 0xFFFFFF;

        public virtual bool Filter(Faction i) { return ((filtro == null || filtro == "") || (i.Definition.Name.String.ToLower().Contains(filtro.ToLower()))); }

        public virtual List<Faction> GetLista() { return Faction.Factions; }

        public static void Initialize()
        {
            CommandSystem.Register("verfactions", AccessLevel.Player, new CommandEventHandler(_OnCommand));
        }


        [Usage("verfactions")]
        [Description("Visualiza um gump mostrando as factions e como chegar nelas.")]
        public static void _OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(ListaFactionsGump)))
                e.Mobile.CloseGump(typeof(ListaFactionsGump));
            e.Mobile.SendGump(new ListaFactionsGump(e.Mobile));
        }

 
        public bool CheckFilter(Faction i)
        {
            if (filtro == null || filtro == "")
                return true;
            return Filter(i);
        }

        public int GetIndexForPage(int page)
        {
            int index = 0;

            while (page-- > 0)
                index += GetCountForIndex(index);

            return index;
        }

        public int GetCountForIndex(int index)
        {
            int slots = 0;
            int count = 0;

            List<Faction> list = m_List;

            for (int i = index; i >= 0 && i < list.Count; ++i)
            {
                var item = list[i];

                if (CheckFilter(item))
                {
                    int add;

                    add = 1;

                    if ((slots + add) > 10)
                        break;

                    slots += add;
                }

                ++count;
            }

            return count;
        }

        public ListaFactionsGump(Mobile from)
            : this(from, 0, null)
        {
        }

        protected string filtro;
        private PlayerMobile player;

        public ListaFactionsGump(Mobile from, int page, List<Faction> list, string filtro = null)
            : base(12, 24)
        {

        
            player = from as PlayerMobile;

            if (filtro != null && filtro != "")
                this.filtro = filtro;
            else if (filtro == null)
                this.filtro = "";

            if (player == null)
                return;

            m_Page = page;

            if (list == null)
            {
                list = GetLista();
            }

            m_List = list;

            int index = GetIndexForPage(page);
            int count = GetCountForIndex(index);

            int tableIndex = 0;

            int width = 600;

            width = 516;

            X = (624 - width) / 2;

            AddPage(0);

            AddBackground(10, 10, width, 439, 5054);
            AddImageTiled(18, 20, width - 17, 420, 2624);

            AddImageTiled(58, 64, 36, 352, 200);
            AddImageTiled(96, 64, 233, 352, 1416);
            AddImageTiled(313, 64, 100, 352, 1416);
            AddImageTiled(415, 64, 76, 352, 200);

            for (int i = index; i < (index + count) && i >= 0 && i < list.Count; ++i)
            {
                var recipe = list[i];

                if (!CheckFilter(recipe))
                    continue;
            
                AddImageTiled(24, 94 + (tableIndex * 32), 489, 2, 2624);
                ++tableIndex;
            }

            AddAlphaRegion(18, 20, width - 17, 420);
            AddImage(0, 0, 10460);
        
            AddImage(0, 429, 10460);
            AddImage(width - 15, 429, 10460);

            AddHtml(266, 32, 200, 32, "Faction", LabelColor, false, false); // Recipe Book

            AddHtml(80 - 3, 32 - 3, 100, 20, "", true, false);
            AddTextEntry(80, 32, 100, 32, 0, 0, filtro);

            AddButton(35, 28, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddHtml(147, 64, 200, 32, "", LabelColor, false, false); // Item
            AddButton(375, 416, 4017, 4018, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(410, 416, 120, 20, 1011441, LabelColor, false, false); // EXIT

            tableIndex = 0;

            if (page > 0)
            {
                AddButton(75, 416, 4014, 4016, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(110, 416, 150, 20, 1011067, LabelColor, false, false); // Previous page
            }

            if (GetIndexForPage(page + 1) < list.Count)
            {
                AddButton(225, 416, 4005, 4007, 3, GumpButtonType.Reply, 0);
                AddHtmlLocalized(260, 416, 150, 20, 1011066, LabelColor, false, false); // Next page
            }

            for (int i = index; i < (index + count) && i >= 0 && i < list.Count; ++i)
            {
                var element = list[i];

                if (!CheckFilter(element))
                    continue;

                try
                {
                    int y = 96 + (tableIndex++ * 32);
                    AddLabel(61, y, 0x480, String.Format("{0}", i));
                    AddHtml(103, y, 230, 32, element.Name, 0xFFFFFF, false, false); // ~1_val~
                    AddHtml(235, y, 200, 20, "", 0xFFFFFF, false, false); // ~1_val~
                    AddButton(421, y - 8, 0x1196, 0x1196, i + 100, GumpButtonType.Reply, 0);
                }
                catch (Exception e)
                {
                    Shard.Erro("Cagou p carregar ?");
                    Shard.Erro(e.StackTrace);
                }

            }
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            int index = info.ButtonID;

            var novoFiltro = info.TextEntries[0].Text;

            if (index >= 100)
            {
                var idx = index - 100;
                var faction = m_List[idx];
                if(from.Map != Map.Trammel)
                {
                    from.SendMessage("Voce nao esta no mapa correto para isto");
                    return;
                }
                if(from.IsStaff())
                {
                    from.SendMessage("Como vc eh staff... ja te teleportei pra la !");
                    from.MoveToWorld(faction.Definition.Stronghold.JoinStone, Map.Trammel);
                } else
                {
                    from.QuestArrow = new QuestArrow(from, faction.Definition.Stronghold.JoinStone);
                    from.QuestArrow.Update();
                    from.SendMessage("Voce esta indo para o forte que percente a " + faction.Name);
                }
              
            }

            switch (index)
            {
                case 1:
                    {
                        from.SendGump(new ListaFactionsGump(from, m_Page, m_List, novoFiltro));
                        break;
                    }
                case 2:
                    {
                        if (m_Page > 0)
                            from.SendGump(new ListaFactionsGump(from, m_Page - 1, m_List, novoFiltro));

                        return;
                    }
                case 3:
                    {
                        if (GetIndexForPage(m_Page + 1) < m_List.Count)
                            from.SendGump(new ListaFactionsGump(from, m_Page + 1, m_List, novoFiltro));

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
