using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Server.Commands;
using Server.Commands.Generic;
using Server.Engines.Craft;
using Server.Engines.Quests;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Regions;

namespace Server.Items
{
    public class ListaComandosGump : Gump
    {
        private List<DocCommandEntry> m_List;

        private int m_Page;

        private const int LabelColor = 0xFFFFFF;

        public virtual bool Filter(DocCommandEntry i) { return i != null && player.AccessLevel >= i.AccessLevel && ((filtro == null || filtro == "") || (i.Name != null && i.Name.ToLower().Contains(filtro.ToLower()) || (i.Description != null && i.Description.Contains(filtro.ToLower())))); }

        public virtual List<CommandEntry> GetLista() { return new List<CommandEntry>(CommandSystem.Entries.Values); }

        public static List<DocCommandEntry> Docs = new List<DocCommandEntry>();

        public static void Initialize()
        {
            CommandSystem.Register("comandos", AccessLevel.Player, new CommandEventHandler(_OnCommand));
        }

        public static void FillTable()
        {
            List<CommandEntry> commands = new List<CommandEntry>(CommandSystem.Entries.Values);
            List<DocCommandEntry> list = new List<DocCommandEntry>();

            commands.Sort();
            commands.Reverse();

            for (int i = 0; i < commands.Count; ++i)
            {
                CommandEntry e = commands[i];

                MethodInfo mi = e.Handler.Method;

                object[] attrs = mi.GetCustomAttributes(typeof(UsageAttribute), false);

                if (attrs.Length == 0)
                    continue;

                UsageAttribute usage = attrs[0] as UsageAttribute;

                attrs = mi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length == 0)
                    continue;

                DescriptionAttribute desc = attrs[0] as DescriptionAttribute;

                if (usage == null || desc == null)
                    continue;

                attrs = mi.GetCustomAttributes(typeof(AliasesAttribute), false);

                AliasesAttribute aliases = (attrs.Length == 0 ? null : attrs[0] as AliasesAttribute);

                string descString = desc.Description.Replace("<", "(").Replace(">", ")");

                if (aliases == null)
                    list.Add(new DocCommandEntry(e.AccessLevel, e.Command, null, usage.Usage, descString));
                else
                {
                    list.Add(new DocCommandEntry(e.AccessLevel, e.Command, aliases.Aliases, usage.Usage, descString));

                    for (int j = 0; j < aliases.Aliases.Length; j++)
                    {
                        string[] newAliases = new string[aliases.Aliases.Length];

                        aliases.Aliases.CopyTo(newAliases, 0);

                        newAliases[j] = e.Command;

                        list.Add(new DocCommandEntry(e.AccessLevel, aliases.Aliases[j], newAliases, usage.Usage, descString));
                    }
                }
            }

            for (int i = 0; i < TargetCommands.AllCommands.Count; ++i)
            {
                BaseCommand command = TargetCommands.AllCommands[i];

                string usage = command.Usage;
                string desc = command.Description;

                if (usage == null || desc == null)
                    continue;

                string[] cmds = command.Commands;
                string cmd = cmds[0];
                string[] aliases = new string[cmds.Length - 1];

                for (int j = 0; j < aliases.Length; ++j)
                    aliases[j] = cmds[j + 1];

                desc = desc.Replace("<", "(").Replace(">", ")");

                if (command.Supports != CommandSupport.Single)
                {
                    StringBuilder sb = new StringBuilder(50 + desc.Length);

                    sb.Append("Modifiers: ");

                    if ((command.Supports & CommandSupport.Global) != 0)
                        sb.Append("<i>Global</i>, ");

                    if ((command.Supports & CommandSupport.Online) != 0)
                        sb.Append("<i>Online</i>, ");

                    if ((command.Supports & CommandSupport.Region) != 0)
                        sb.Append("<i>Region</i>, ");

                    if ((command.Supports & CommandSupport.Contained) != 0)
                        sb.Append("<i>Contained</i>, ");

                    if ((command.Supports & CommandSupport.Multi) != 0)
                        sb.Append("<i>Multi</i>, ");

                    if ((command.Supports & CommandSupport.Area) != 0)
                        sb.Append("<i>Area</i>, ");

                    if ((command.Supports & CommandSupport.Self) != 0)
                        sb.Append("<i>Self</i>, ");

                    sb.Remove(sb.Length - 2, 2);
                    sb.Append("<br>");
                    sb.Append(desc);

                    desc = sb.ToString();
                }

                list.Add(new DocCommandEntry(command.AccessLevel, cmd, aliases, usage, desc));

                for (int j = 0; j < aliases.Length; j++)
                {
                    string[] newAliases = new string[aliases.Length];

                    aliases.CopyTo(newAliases, 0);

                    newAliases[j] = cmd;

                    list.Add(new DocCommandEntry(command.AccessLevel, aliases[j], newAliases, usage, desc));
                }
            }

            List<BaseCommandImplementor> commandImpls = BaseCommandImplementor.Implementors;

            for (int i = 0; i < commandImpls.Count; ++i)
            {
                BaseCommandImplementor command = commandImpls[i];

                string usage = command.Usage;
                string desc = command.Description;

                if (usage == null || desc == null)
                    continue;

                string[] cmds = command.Accessors;
                string cmd = cmds[0];
                string[] aliases = new string[cmds.Length - 1];

                for (int j = 0; j < aliases.Length; ++j)
                    aliases[j] = cmds[j + 1];

                desc = desc.Replace("<", ")").Replace(">", ")");

                list.Add(new DocCommandEntry(command.AccessLevel, cmd, aliases, usage, desc));

                for (int j = 0; j < aliases.Length; j++)
                {
                    string[] newAliases = new string[aliases.Length];

                    aliases.CopyTo(newAliases, 0);

                    newAliases[j] = cmd;

                    list.Add(new DocCommandEntry(command.AccessLevel, aliases[j], newAliases, usage, desc));
                }
            }

            list.Sort(new Docs.CommandEntrySorter());

            Docs = list;
        }

        [Usage("vercomandos")]
        [Description("Visualiza um gump mostrando todos possiveis comandos.")]
        public static void _OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(ListaComandosGump)))
                e.Mobile.CloseGump(typeof(ListaComandosGump));
            e.Mobile.SendGump(new ListaComandosGump(e.Mobile));
        }

        public void Build()
        {
            foreach (var e in GetLista())
            {
                var mi = e.Handler.Method;

                var attrs = mi.GetCustomAttributes(typeof(UsageAttribute), false);

                if (attrs.Length == 0)
                {
                    continue;
                }

                var usage = attrs[0] as UsageAttribute;

                attrs = mi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length == 0)
                {
                    continue;
                }

                var desc = attrs[0] as DescriptionAttribute;

                if (usage == null || desc == null)
                {
                    continue;
                }

                attrs = mi.GetCustomAttributes(typeof(AliasesAttribute), false);

                var aliases = (attrs.Length == 0 ? null : attrs[0] as AliasesAttribute);

                var descString = desc.Description.Replace("<", "&lt;").Replace(">", "&gt;");
                var doc =
                    aliases == null
                        ? new DocCommandEntry(e.AccessLevel, e.Command, null, usage.Usage, descString)
                        : new DocCommandEntry(e.AccessLevel, e.Command, aliases.Aliases, usage.Usage, descString);
                Docs.Add(doc);
            }
        }

        public bool CheckFilter(DocCommandEntry i)
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

            List<DocCommandEntry> list = m_List;

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

        public ListaComandosGump(Mobile from)
            : this(from, 0, null)
        {
        }

        protected string filtro;
        private PlayerMobile player;

        public ListaComandosGump(Mobile from, int page, List<DocCommandEntry> list, string filtro = null)
            : base(12, 24)
        {

            if (Docs.Count == 0)
            {
                //Build();
                FillTable();
            }
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
                list = Docs.Where(doc => doc.AccessLevel <= from.AccessLevel).ToList();
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

            AddHtml(266, 32, 200, 32, "Comando", LabelColor, false, false); // Recipe Book

            // FILTRO
            AddHtml(80 - 3, 32 - 3, 100, 20, "", true, false);
            AddTextEntry(80, 32, 100, 32, 0, 0, filtro);

            //AddBackground(80, 32, 200, 32, 9359);
            AddButton(35, 28, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddHtml(147, 64, 200, 32, "Desc", LabelColor, false, false); // Item
            //AddHtml(246, 64, 200, 32, cols[2], LabelColor, false, false); // Expansion
            //AddHtml(336, 64, 200, 32, "", LabelColor, false, false); // Crafting
            //AddHtml(429, 64, 100, 32, cols[3], LabelColor, false, false); // Amount

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
                    AddHtml(235, y, 200, 20, element.Description, 0xFFFFFF, false, false); // ~1_val~
                    AddButton(421, y - 8, 0x1196, 0x1196, i + 100, GumpButtonType.Reply, 0);
                }
                catch (Exception e)
                {
                    Shard.Erro("Cagou p carregar quest ?");
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
                DocCommandEntry item = m_List[idx];
                from.SendMessage(item.Name);
                if(item.Usage != null)
                    from.SendMessage("Uso:" + item.Usage);
                if (item.Description != null)
                    from.SendMessage(item.Description);
                from.SendGump(new HelpInfo.DocCommandEntryGump(item));

            }

            switch (index)
            {
                case 1:
                    {
                        from.SendGump(new ListaComandosGump(from, m_Page, m_List, novoFiltro));
                        break;
                    }
                case 2:
                    {
                        if (m_Page > 0)
                            from.SendGump(new ListaComandosGump(from, m_Page - 1, m_List, novoFiltro));

                        return;
                    }
                case 3:
                    {
                        if (GetIndexForPage(m_Page + 1) < m_List.Count)
                            from.SendGump(new ListaComandosGump(from, m_Page + 1, m_List, novoFiltro));

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
