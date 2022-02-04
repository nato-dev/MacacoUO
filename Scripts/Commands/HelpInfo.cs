using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Network;

using DocCommandEntrySorter = Server.Commands.Docs.CommandEntrySorter;

namespace Server.Commands
{
    public class HelpInfo
    {
        private static readonly Dictionary<string, DocCommandEntry> m_HelpInfos = new Dictionary<string, DocCommandEntry>();
        private static List<DocCommandEntry> m_SortedHelpInfo = new List<DocCommandEntry>();//No need for SortedList cause it's only sorted once at creation...
        public static Dictionary<string, DocCommandEntry> HelpInfos
        {
            get
            {
                return m_HelpInfos;
            }
        }
        public static List<DocCommandEntry> SortedHelpInfo
        {
            get
            {
                return m_SortedHelpInfo;
            }
        }
        [CallPriority(100)]
        public static void Initialize()
        {
            CommandSystem.Register("HelpInfo", AccessLevel.Player, new CommandEventHandler(HelpInfo_OnCommand));

            FillTable();
        }

        public static void FillTable()
        {
            List<CommandEntry> commands = new List<CommandEntry>(CommandSystem.Entries.Values);
            List<DocCommandEntry> list = new List<DocCommandEntry>();

            commands.Sort();
            commands.Reverse();
            Docs.Clean(commands);

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

            list.Sort(new DocCommandEntrySorter());

            m_SortedHelpInfo = list;

            foreach (DocCommandEntry c in m_SortedHelpInfo)
            {
                if (!m_HelpInfos.ContainsKey(c.Name.ToLower()))
                    m_HelpInfos.Add(c.Name.ToLower(), c);
            }
        }

        [Usage("HelpInfo [<command>]")]
        [Description("Gives information on a specified command, or when no argument specified, displays a gump containing all commands")]
        private static void HelpInfo_OnCommand(CommandEventArgs e)
        {
            if (e.Length > 0)
            {
                string arg = e.GetString(0).ToLower();
                DocCommandEntry c;

                if (m_HelpInfos.TryGetValue(arg, out c))
                {
                    Mobile m = e.Mobile;

                    if (m.AccessLevel >= c.AccessLevel)
                        m.SendGump(new DocCommandEntryGump(c));
                    else
                        m.SendMessage("You don't have access to that command.");

                    return;
                }
                else
                    e.Mobile.SendMessage(String.Format("Command '{0}' not found!", arg));
            }

            e.Mobile.SendGump(new CommandListGump(0, e.Mobile, null));
        }

        public class CommandListGump : BaseGridGump
        {
            private const int EntriesPerPage = 15;
            readonly int m_Page;
            readonly List<DocCommandEntry> m_List;
            public CommandListGump(int page, Mobile from, List<DocCommandEntry> list)
                : base(30, 30)
            {
                this.m_Page = page;

                if (list == null)
                {
                    this.m_List = new List<DocCommandEntry>();

                    foreach (DocCommandEntry c in m_SortedHelpInfo)
                    {
                        if (from.AccessLevel >= c.AccessLevel)
                            this.m_List.Add(c);
                    }
                }
                else
                    this.m_List = list;

                this.AddNewPage();

                if (this.m_Page > 0)
                    this.AddEntryButton(20, ArrowLeftID1, ArrowLeftID2, 1, ArrowLeftWidth, ArrowLeftHeight);
                else
                    this.AddEntryHeader(20);

                this.AddEntryHtml(160, this.Center(String.Format("Pagina {0} de {1}", this.m_Page + 1, (this.m_List.Count + EntriesPerPage - 1) / EntriesPerPage)));

                if ((this.m_Page + 1) * EntriesPerPage < this.m_List.Count)
                    this.AddEntryButton(20, ArrowRightID1, ArrowRightID2, 2, ArrowRightWidth, ArrowRightHeight);
                else
                    this.AddEntryHeader(20);

                int last = (int)AccessLevel.Player - 1;

                for (int i = this.m_Page * EntriesPerPage, line = 0; line < EntriesPerPage && i < this.m_List.Count; ++i, ++line)
                {
                    DocCommandEntry c = this.m_List[i];
                    if (from.AccessLevel >= c.AccessLevel)
                    {
                        if ((int)c.AccessLevel != last)
                        {
                            this.AddNewLine();

                            this.AddEntryHtml(20 + this.OffsetSize + 160, this.Color(c.AccessLevel.ToString(), 0xFF0000));
                            this.AddEntryHeader(20);
                            line++;
                        }

                        last = (int)c.AccessLevel;

                        this.AddNewLine();

                        this.AddEntryHtml(20 + this.OffsetSize + 160, c.Name);

                        this.AddEntryButton(20, ArrowRightID1, ArrowRightID2, 3 + i, ArrowRightWidth, ArrowRightHeight);
                    }
                }

                this.FinishPage();
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile m = sender.Mobile;
                switch( info.ButtonID )
                {
                    case 0:
                        {
                            m.CloseGump(typeof(DocCommandEntryGump));
                            break;
                        }
                    case 1:
                        {
                            if (this.m_Page > 0)
                                m.SendGump(new CommandListGump(this.m_Page - 1, m, this.m_List));

                            break;
                        }
                    case 2:
                        {
                            if ((this.m_Page + 1) * EntriesPerPage < m_SortedHelpInfo.Count)
                                m.SendGump(new CommandListGump(this.m_Page + 1, m, this.m_List));

                            break;
                        }
                    default:
                        {
                            int v = info.ButtonID - 3;

                            if (v >= 0 && v < this.m_List.Count)
                            {
                                DocCommandEntry c = this.m_List[v];

                                if (m.AccessLevel >= c.AccessLevel)
                                {
                                    m.SendGump(new DocCommandEntryGump(c));
                                    m.SendGump(new CommandListGump(this.m_Page, m, this.m_List));
                                }
                                else
                                {
                                    m.SendMessage("You no longer have access to that command.");
                                    m.SendGump(new CommandListGump(this.m_Page, m, null));
                                }
                            }
                            break;
                        }
                }
            }
        }

        public class DocCommandEntryGump : Gump
        {
            private DocCommandEntry m_Info;

            public DocCommandEntryGump(DocCommandEntry info)
                : this(info, 320, 210)
            {
            }

            public DocCommandEntryGump(DocCommandEntry info, int width, int height)
                : base(300, 50)
            {
                m_Info = info;
                this.AddPage(0);

                this.AddBackground(0, 0, width, height, 9200);

                this.AddHtml(10, 10, width - 20, 20, this.Center(info.Name), false, false);

                this.AddHtml(10, height - 30, 50, 20, "Params:", false, false);
                this.AddAlphaRegion(65, height - 34, 170, 28);
                this.AddTextEntry(70, height - 30, 160, 20, 789, 0, "");
                this.AddHtml(250, height - 30, 30, 20, "Usar", false, false);
                this.AddButton(280, height - 30, 0xFA6, 0xFA7, 99, GumpButtonType.Reply, 0);

                StringBuilder sb = new StringBuilder();

                sb.Append("Uso: ");
                sb.Append(info.Usage.Replace("<", "(").Replace(">", ")"));
                sb.Append("<BR>");

                string[] aliases = info.Aliases;

                if (aliases != null && aliases.Length != 0)
                {
                    sb.Append(String.Format("Atalhos{0}: ", aliases.Length == 1 ? "" : "es"));

                    for (int i = 0; i < aliases.Length; ++i)
                    {
                        if (i != 0)
                            sb.Append(", ");

                        sb.Append(aliases[i]);
                    }

                    sb.Append("<BR>");
                }

                sb.Append("Permissao: ");
                sb.Append(info.AccessLevel.ToString());
                sb.Append("<BR>");
                sb.Append("<BR>");

                sb.Append(info.Description);

                this.AddHtml(10, 40, width - 20, height - 80, sb.ToString(), false, true);
                //AddImageTiled( 10, height - 30, width - 20, 20, 2624 );
                //AddAlphaRegion( 10, height - 30, width - 20, 20 );
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID == 99)
                    try
                    {
                        Mobile from = sender.Mobile;
                        string command = CommandSystem.Prefix + m_Info.Name + " " + info.TextEntries[0].Text;
                        CommandSystem.Handle(from, command);
                    }
                    catch
                    {
                    }
                //else close the gump silently
            }

            public string Color(string text, int color)
            {
                return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
            }

            public string Center(string text)
            {
                return String.Format("<CENTER>{0}</CENTER>", text);
            }
        }
    }
}
