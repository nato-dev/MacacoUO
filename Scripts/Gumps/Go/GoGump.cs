using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class GoGump : Gump
    {
        public static readonly LocationTree Felucca = new LocationTree("felucca.xml", Map.Felucca);
        public static readonly LocationTree Trammel = new LocationTree("trammel.xml", Map.Trammel);
        public static readonly LocationTree Ilshenar = new LocationTree("ilshenar.xml", Map.Ilshenar);
        public static readonly LocationTree Malas = new LocationTree("malas.xml", Map.Malas);
        public static readonly LocationTree Tokuno = new LocationTree("tokuno.xml", Map.Tokuno);

        public static readonly LocationTree CampingPoints = new LocationTree("campingPoints.xml", Map.Felucca);
        #region SA
        public static readonly LocationTree TerMur = new LocationTree("termur.xml", Map.TerMur);
        #endregion

        public static bool OldStyle = PropsConfig.OldStyle;

        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;

        public static readonly int TextHue = PropsConfig.TextHue;
        public static readonly int TextOffsetX = PropsConfig.TextOffsetX;

        public static readonly int OffsetGumpID = PropsConfig.OffsetGumpID;
        public static readonly int HeaderGumpID = PropsConfig.HeaderGumpID;
        public static readonly int EntryGumpID = PropsConfig.EntryGumpID;
        public static readonly int BackGumpID = PropsConfig.BackGumpID;
        public static readonly int SetGumpID = PropsConfig.SetGumpID;

        public static readonly int SetWidth = PropsConfig.SetWidth;
        public static readonly int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
        public static readonly int SetButtonID1 = PropsConfig.SetButtonID1;
        public static readonly int SetButtonID2 = PropsConfig.SetButtonID2;

        public static readonly int PrevWidth = PropsConfig.PrevWidth;
        public static readonly int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
        public static readonly int PrevButtonID1 = PropsConfig.PrevButtonID1;
        public static readonly int PrevButtonID2 = PropsConfig.PrevButtonID2;

        public static readonly int NextWidth = PropsConfig.NextWidth;
        public static readonly int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
        public static readonly int NextButtonID1 = PropsConfig.NextButtonID1;
        public static readonly int NextButtonID2 = PropsConfig.NextButtonID2;

        public static readonly int OffsetSize = PropsConfig.OffsetSize;

        public static readonly int EntryHeight = PropsConfig.EntryHeight;
        public static readonly int BorderSize = PropsConfig.BorderSize;

        private static readonly bool PrevLabel = false;

        private static readonly bool NextLabel = false;

        private static readonly int PrevLabelOffsetX = PrevWidth + 1;
        private static readonly int PrevLabelOffsetY = 0;

        private static readonly int NextLabelOffsetX = -29;
        private static readonly int NextLabelOffsetY = 0;

        private static readonly int EntryWidth = 180;
        private static readonly int EntryCount = 15;

        private static readonly int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize;
        private static readonly int TotalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (EntryCount + 1));

        private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;
        private static readonly int BackHeight = BorderSize + TotalHeight + BorderSize;

        public static void DisplayTo(Mobile from)
        {
            LocationTree tree;

            if (from.Map == Map.Ilshenar)
                tree = Ilshenar;
            else if (from.Map == Map.Felucca)
                tree = Felucca;
            else if (from.Map == Map.Trammel)
                tree = Trammel;
            else if (from.Map == Map.Malas)
                tree = Malas;
            #region SA
            else if (from.Map == Map.Tokuno)
                tree = Tokuno;
            else
                tree = TerMur;
            #endregion

            ParentNode branch = null;
            tree.LastBranch.TryGetValue(from, out branch);

            if (branch == null)
                branch = tree.Root;

            if (branch != null)
                from.SendGump(new GoGump(0, from, tree, branch));
        }

        public static void DisplayToCampfire(Mobile from)
        {
            ParentNode branch = null;

            if (branch == null)
                branch = CampingPoints.Root;

            if (branch != null)
                from.SendGump(new GoGump(0, from, CampingPoints, branch, true));
        }

        private readonly LocationTree m_Tree;
        private readonly ParentNode m_Node;
        private readonly int m_Page;
        private bool camp = false;

        private static List<ChildNode> GetChildren(ParentNode node, List<ChildNode> childList)
        {
            foreach (var children in node.Children)
            {
                if (children is ParentNode)
                {
                    GetChildren((ParentNode)children, childList);
                }
                else if (children is ChildNode)
                {
                    childList.Add((ChildNode)children);
                }
            }
            return childList;
        }

        public static List<ChildNode> GetAllChildsCamping()
        {
            List<ChildNode> allChilds = new List<ChildNode>();
            var tree = GoGump.CampingPoints.Root;
            foreach (var children in tree.Children)
            {
                if (children is ParentNode)
                {
                    GetChildren((ParentNode)children, allChilds);
                }
                else if (children is ChildNode)
                {
                    allChilds.Add((ChildNode)children);
                }
            }
            return allChilds;
        }

        public static void DiscoverLocation(PlayerMobile from)
        {
            var discobertas = from.CampfireLocations.Split(';').ToList(); 
            var childs = GetAllChildsCamping();

            double menorDist = 999999999999999999;

            ChildNode prox = null;

            foreach (var loc in childs)
            {

                var distancia = from.GetDistanceToSqrt(loc.Location);
                if (distancia < 500)
                {
                    if (distancia < menorDist)
                    {
                        menorDist = distancia;
                        prox = loc;
                    }
                }
            }
            if (prox != null)
            {
                if (discobertas.Contains(prox.Name))
                {
                    return;
                }
                from.CampfireLocations += prox.Name + ";";
                from.Emote("Local de Camping Descoberto: " + prox.Name);
            }
        }

        private GoGump(int page, Mobile from, LocationTree tree, ParentNode node, bool campfire = false)
            : base(50, 50)
        {
            from.CloseGump(typeof(GoGump));

            this.camp = campfire;

            tree.LastBranch[from] = node;

            this.m_Page = page;
            this.m_Tree = tree;
            this.m_Node = node;

            int x = BorderSize + OffsetSize;
            int y = BorderSize + OffsetSize;

            int count = node.Children.Length - (page * EntryCount);

            if (count < 0)
                count = 0;
            else if (count > EntryCount)
                count = EntryCount;

            int totalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

            this.AddPage(0);

            this.AddBackground(0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID);
            this.AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), totalHeight, OffsetGumpID);

            if (OldStyle)
                this.AddImageTiled(x, y, TotalWidth - (OffsetSize * 3) - SetWidth, EntryHeight, HeaderGumpID);
            else
                this.AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

            if (node.Parent != null)
            {
                this.AddButton(x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0);

                if (PrevLabel)
                    this.AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous");
            }

            x += PrevWidth + OffsetSize;

            int emptyWidth = TotalWidth - (PrevWidth * 2) - NextWidth - (OffsetSize * 5) - (OldStyle ? SetWidth + OffsetSize : 0);

            if (!OldStyle)
                this.AddImageTiled(x - (OldStyle ? OffsetSize : 0), y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0), EntryHeight, EntryGumpID);

            this.AddHtml(x + TextOffsetX, y, emptyWidth - TextOffsetX, EntryHeight, String.Format("<center>{0}</center>", node.Name), false, false);

            x += emptyWidth + OffsetSize;

            if (OldStyle)
                this.AddImageTiled(x, y, TotalWidth - (OffsetSize * 3) - SetWidth, EntryHeight, HeaderGumpID);
            else
                this.AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

            if (page > 0)
            {
                this.AddButton(x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 2, GumpButtonType.Reply, 0);

                if (PrevLabel)
                    this.AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous");
            }

            x += PrevWidth + OffsetSize;

            if (!OldStyle)
                this.AddImageTiled(x, y, NextWidth, EntryHeight, HeaderGumpID);

            if ((page + 1) * EntryCount < node.Children.Length)
            {
                this.AddButton(x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 3, GumpButtonType.Reply, 1);

                if (NextLabel)
                    this.AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next");
            }

            var campsLiberados = ((PlayerMobile)from).CampfireLocations.Split(';').ToList();

            for (int i = 0, index = page * EntryCount; i < EntryCount && index < node.Children.Length; ++i, ++index)
            {

                object child = node.Children[index];
                string name = "";

                if (child is ParentNode)
                    name = ((ParentNode)child).Name;
                else if (child is ChildNode)
                    name = ((ChildNode)child).Name;

                if (name != "Camping" && camp && !campsLiberados.Contains(name))
                {
                    continue; 
                }
  
                x = BorderSize + OffsetSize;
                y += EntryHeight + OffsetSize;

                this.AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
                this.AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, name);

                x += EntryWidth + OffsetSize;

                if (SetGumpID != 0)
                    this.AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

                this.AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, index + 4, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 1:
                    {
                        if (this.m_Node.Parent != null)
                            from.SendGump(new GoGump(0, from, this.m_Tree, this.m_Node.Parent, camp));

                        break;
                    }
                case 2:
                    {
                        if (this.m_Page > 0)
                            from.SendGump(new GoGump(this.m_Page - 1, from, this.m_Tree, this.m_Node, camp));

                        break;
                    }
                case 3:
                    {
                        if ((this.m_Page + 1) * EntryCount < this.m_Node.Children.Length)
                            from.SendGump(new GoGump(this.m_Page + 1, from, this.m_Tree, this.m_Node, camp));

                        break;
                    }
                default:
                    {
                        int index = info.ButtonID - 4;
                        if (index >= 0 && index < this.m_Node.Children.Length)
                        {
                            object o = this.m_Node.Children[index];

                            if (o is ParentNode)
                            {
                                from.SendGump(new GoGump(0, from, this.m_Tree, (ParentNode)o, camp));
                            }
                            else
                            {
                                ChildNode n = (ChildNode)o;
                                if (camp)
                                {
                                    var timer = new CampingTimer(from, this.m_Tree, n.Location);
                                    timer.Start();
                                }
                                else
                                {
                                    from.MoveToWorld(n.Location, this.m_Tree.Map);
                                }


                            }
                        }

                        break;
                    }
            }
        }

        public class CampingTimer : Timer
        {
            private Mobile player;
            LocationTree locTree;
            Point3D point;
            public CampingTimer(Mobile player, LocationTree locTree, Point3D point)
                : base(TimeSpan.FromSeconds(Core.AOS ? 3.0 : 2.5))
            {
                this.locTree = locTree;
                this.point = point;
                this.player = player;
                Priority = TimerPriority.FiftyMS;
                player.Freeze(TimeSpan.FromSeconds(3));
                player.SendMessage("Voce esta indo ao seu destino");
            }

            protected override void OnTick()
            {
                BaseCreature.TeleportPets(player, point, locTree.Map);
                player.MoveToWorld(point, locTree.Map);
                player.SendMessage("Voce chegou ao seu destino");
            }
        }
    }
}
