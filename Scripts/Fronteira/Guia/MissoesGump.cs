using System;
using System.Collections.Generic;
using System.Linq;
using Server.Commands;
using Server.Engines.Craft;
using Server.Engines.Quests;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Regions;

namespace Server.Items
{
    public class MissoesGump : Gump
    {
        private List<MondainQuester> m_List;


        public static void Initialize()
        {
            CommandSystem.Register("vermissoes", AccessLevel.Player, new CommandEventHandler(_OnCommand));
        }

        [Usage("vermissoes")]
        [Description("Makes a call to your custom gump.")]
        public static void _OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(MissoesGump)))
                e.Mobile.CloseGump(typeof(MissoesGump));
            e.Mobile.SendGump(new MissoesGump(e.Mobile));
        }

        private int m_Page;

        private const int LabelColor = 0xFFFFFF;

        public bool CheckFilter(MondainQuester recipe)
        {
            if (recipe != null && recipe.Map != Map.Trammel)
                return false;

            if (filtro == null || filtro == "")
                return true;

            if ((recipe.Name != null && recipe.Name.ToLower().Contains(filtro)) || (recipe.Title != null && recipe.Title.ToLower().Contains(filtro.ToLower())))
                return true;
            else
                return false;
        
        }

        public int GetIndexForPage(int page)
        {
            int index = 0;

            while (page-- > 0)
                index += GetCountForIndex(index);

            return index;
        }

        public BaseQuest GetQuest(MondainQuester quester, PlayerMobile from)
        {
            foreach(var quest in quester.Quests)
            {
                var q = MondainQuester.GetCachedQuest(from, quest);
                if (q != null && QuestHelper.CanOffer(from, q, false))
                    return q;
            }

            foreach(var quest in from.Quests)
            {
                if (quest.Quester == quester)
                    return quest;
            }

            return null;
        }

        public int GetCountForIndex(int index)
        {
            int slots = 0;
            int count = 0;

            List<MondainQuester> list = m_List;

            for (int i = index; i >= 0 && i < list.Count; ++i)
            {
                var recipe = list[i];

                if (CheckFilter(recipe))
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

        public MissoesGump(Mobile from)
            : this(from, 0, null)
        {
        }

        private string filtro;

        public MissoesGump(Mobile from, int page, List<MondainQuester> list, string filtro=null)
            : base(12, 24)
        {
            from.CloseGump(typeof(CurrentRecipesGump));
            from.CloseGump(typeof(RecipeScrollFilterGump));

            var player = from as PlayerMobile;

            if (filtro != null && filtro != "")
                this.filtro = filtro;
            else if (filtro == null)
                this.filtro = "";

            if (player == null)
                return;

            m_Page = page;

            if (list == null)
            {
                list = new List<MondainQuester>();
                foreach (var quester in MondainQuester.Questers)
                {
                    // nao mostra os escortables no menu
                    if (quester is BaseEscort)
                        continue;
                    list.Add(quester);
                }
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
            //AddImageTiled(231, 64, 80, 352, 200);
            AddImageTiled(313, 64, 100, 352, 1416);
            AddImageTiled(415, 64, 76, 352, 200);

            //list = list.OrderBy(x => x.ID).ToList();

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
            //AddImage(width - 15, 5, 10460);
            AddImage(0, 429, 10460);
            AddImage(width - 15, 429, 10460);

            AddHtml(266, 32, 200, 32, "Aventuras", LabelColor, false, false); // Recipe Book

            // FILTRO
            //AddHtml(80 - 3, 32 - 3, 100, 20, "", true, false);
            //AddTextEntry(80, 32, 100, 32, 0, 0, filtro);
          
            //AddBackground(80, 32, 200, 32, 9359);
            AddButton(35, 28, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddHtml(147, 64, 200, 32, "Npc", LabelColor, false, false); // Item
            AddHtml(246, 64, 200, 32, "Aventura", LabelColor, false, false); // Expansion
            //AddHtml(336, 64, 200, 32, "", LabelColor, false, false); // Crafting
            AddHtml(429, 64, 100, 32, "Localizar", LabelColor, false, false); // Amount

            // AddHtmlLocalized(70, 32, 200, 32, 1062476, LabelColor, false, false); // Set Filter
            // AddButton(35, 32, 4005, 4007, 1, GumpButtonType.Reply, 0);

            /*
            RecipeScrollFilter f = book.Filter;

            if (f.IsDefault)
                AddHtmlLocalized(canPrice ? 470 : 386, 32, 120, 32, 1062475, 16927, false, false); // Using No Filter
            else if (((PlayerMobile)from).UseOwnFilter)
                AddHtmlLocalized(canPrice ? 470 : 386, 32, 120, 32, 1062451, 16927, false, false); // Using Your Filter
            else
                AddHtmlLocalized(canPrice ? 470 : 386, 32, 120, 32, 1062230, 16927, false, false); // Using Book Filter
            */

            AddButton(375, 416, 4017, 4018, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(410, 416, 120, 20, 1011441, LabelColor, false, false); // EXIT

            /*
            if (canDrop)
                AddHtmlLocalized(26, 64, 50, 32, 1062212, LabelColor, false, false); // Drop

            if (canPrice)
            {
                AddHtmlLocalized(516, 64, 200, 32, 1062218, LabelColor, false, false); // Price

                if (canBuy)
                    AddHtmlLocalized(576, 64, 200, 32, 1062219, LabelColor, false, false); // Buy
                else
                    AddHtmlLocalized(576, 64, 200, 32, 1062227, LabelColor, false, false); // Set
            }
            */

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
                    var quest = this.GetQuest(element, player);
                    if (quest == null)
                        continue;

                    int y = 96 + (tableIndex++ * 32);

                    //if (recipe.Amount > 0 && (canDrop || canLocked))
                    //    AddButton(35, y + 2, 5602, 5606, 4 + (i * 2), GumpButtonType.Reply, 0);


                    AddLabel(61, y, 0x480, String.Format("{0}", i));

                    AddHtml(103, y, 230, 32, element.Name, 0xFFFFFF, false, false); // ~1_val~

                    //AddLabel(235, y, 0x480, ""+recipe.RecipeID);
                    if (quest.Title is String)
                    {
                        AddHtml(235, y, 200, 20, (String)quest.Title, 0xFFFFFF, false, false); // ~1_val~
                    }
                    else
                    {
                        AddHtmlLocalized(235, y, 100, 20, (int)quest.Title, 0xFFFFFF, false, false); // ~1_val~
                    }


                    AddButton(421, y - 8, 0x1196, 0x1196, i + 100, GumpButtonType.Reply, 0);
                    //AddLabel(421, y, 0x480, "BUTAUM");
                } catch(Exception e)
                {
                    Shard.Erro("Cagou p carregar quest ?");
                    Shard.Erro(e.StackTrace);
                }

            }


            /*
            if (canDrop || (canBuy && recipe.Price > 0))
            {
                AddButton(579, y + 2, 2117, 2118, 5 + (i * 2), GumpButtonType.Reply, 0);
                AddLabel(495, y, 1152, recipe.Price.ToString("N0"));
            }
            */
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            int index = info.ButtonID;

            var novoFiltro = "";//info.TextEntries[0].Text;

            if (index >= 100)
            {
                var idx = index - 100;
                var npc = m_List[idx];
                from.QuestArrow = new QuestArrow(from, npc.Location);
                from.QuestArrow.Update();
                from.CloseAllGumps();
                from.SendMessage("Voce esta indo ao seu destino "+npc.Location.ToString()+" no mapa "+npc.Map.Name);
                if(npc.Region != null && npc.Region.Name != null)
                {
                    from.SendMessage("Este NPC fica na regiao de " + npc.Region.Name);
                }
            }

            switch (index)
            {
                case 1:
                    {
                        from.SendGump(new MissoesGump(from, m_Page, m_List, novoFiltro));
                        break;
                    }
                case 2:
                    {
                        if (m_Page > 0)
                            from.SendGump(new MissoesGump(from, m_Page - 1, m_List, novoFiltro));

                        return;
                    }
                case 3:
                    {
                        if (GetIndexForPage(m_Page + 1) < m_List.Count)
                            from.SendGump(new MissoesGump(from, m_Page + 1, m_List, novoFiltro));

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
