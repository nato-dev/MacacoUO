using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Items;
using Server.Ziden.Traducao;

namespace Server.Engines.BulkOrders
{
    public class SmallBODGump : Gump
    {
        private readonly SmallBOD m_Deed;
        private readonly Mobile m_From;

        public SmallBODGump(Mobile from, SmallBOD deed)
            : base(25, 25)
        {
            this.m_From = from;
            this.m_Deed = deed;

            this.m_From.CloseGump(typeof(LargeBODGump));
            this.m_From.CloseGump(typeof(SmallBODGump));

            this.AddPage(0);

            int height = 0;

            if (BulkOrderSystem.NewSystemEnabled)
            {
                if (deed.RequireExceptional || deed.Material != BulkMaterialType.None)
                    height += 24;

                if (deed.RequireExceptional)
                    height += 24;

                if (deed.Material != BulkMaterialType.None)
                    height += 24;
            }

            this.AddBackground(50, 10, 455, 245 + height, 1579);
            //this.AddImageTiled(58, 20, 438, 226 + height, 2624);
            this.AddAlphaRegion(58, 20, 438, 226 + height);

            this.AddImage(45, 5, 10460);
            this.AddImage(480, 5, 10460);
            this.AddImage(45, 230 + height, 10460);
            this.AddImage(480, 230 + height, 10460);

            this.AddHtmlLocalized(225, 25, 120, 20, 1045133, 1, false, false); // A bulk order

            this.AddHtmlLocalized(75, 48, 250, 20, 1045138, 1, false, false); // Amount to make:
            this.AddLabel(275, 48, 1152, deed.AmountMax.ToString());

            this.AddHtmlLocalized(275, 76, 200, 20, 1045153, 1, false, false); // Amount finished:
            this.AddHtmlLocalized(75, 72, 120, 20, 1045136, 1, false, false); // Item requested:

            this.AddItem(400, 72, deed.Graphic, deed.GraphicHue);
            var nome = Trads.GetNome(deed.Type);
            if (nome != null)
                this.AddHtml(75, 96, 210, 20, nome, 1, false, false);
            else
                this.AddHtmlLocalized(75, 96, 210, 20, deed.Number, 1, false, false);
            this.AddLabel(275, 96, 0x480, deed.AmountCur.ToString());

            int y = 120;

            if (deed.RequireExceptional || deed.Material != BulkMaterialType.None)
            {
                this.AddHtmlLocalized(75, y, 200, 20, 1045140, 1, false, false); // Special requirements to meet:
                y += 24;
            }

            if (deed.RequireExceptional)
            {
                this.AddHtmlLocalized(75, y, 300, 20, 1045141, 1, false, false); // All items must be exceptional.
                y += 24;
            }

            if (deed.Material != BulkMaterialType.None)
            {
                AddHtml(75, y, 300, 20, "Feitos de " + deed.Material, 1, false, false);
                y += 24;
            }

            if (from is PlayerMobile && BulkOrderSystem.NewSystemEnabled)
            {
                BODContext c = BulkOrderSystem.GetContext((PlayerMobile)from);

                int points = 0;
                double banked = 0.0;
                
                BulkOrderSystem.ComputePoints(deed, out points, out banked);

                AddHtml(75, y, 300, 20, String.Format("Vale {0} pontos", banked.ToString("0.00")), 1, false, false); // Worth ~1_POINTS~ turn in points and ~2_POINTS~ bank points.
                y += 24;

                /*
                AddButton(125, y, 4005, 4007, 3, GumpButtonType.Reply, 0);
                AddHtml(160, y, 300, 20, c.PointsMode == PointsMode.Enabled ? "Pontos Habilitados" : c.PointsMode == PointsMode.Disabled ? "Pontos Desabilitados" : "Pontos Automaticos", 1, false, false);  // Banking Points Enabled/Disabled/Automatic
                y += 24;
                */

                AddButton(125, y, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtml(160, y, 300, 20, "Combine com o item pedido", 1, false, false); // Combine this deed with the item requested.
                y += 24;

                this.AddButton(125, y, 4005, 4007, 4, GumpButtonType.Reply, 0);
                this.AddHtml(160, y, 300, 20, "Combine com items contidos", 1, false, false); // Combine this deed with contained items.
                y += 24;
            }
            else
            {
                this.AddButton(125, y, 4005, 4007, 2, GumpButtonType.Reply, 0);
                this.AddHtmlLocalized(160, y, 300, 20, 1045154, 1, false, false); // Combine this deed with the item requested.
                y += 24;
            }

            this.AddButton(125, y, 4005, 4007, 1, GumpButtonType.Reply, 0);
            this.AddHtmlLocalized(160, y, 120, 20, 1011441, 1, false, false); // EXIT
        }

        public static int GetMaterialNumberFor(BulkMaterialType material)
        {
            if (material >= BulkMaterialType.Cobre && material <= BulkMaterialType.Vibranium)
                return 1045142 + (int)(material - BulkMaterialType.Cobre);
            else if (material >= BulkMaterialType.Spined && material <= BulkMaterialType.Barbed)
                return 1049348 + (int)(material - BulkMaterialType.Spined);
            else if (material >= BulkMaterialType.Carvalho && material <= BulkMaterialType.Gelo)
            {
                switch (material)
                {
                    case BulkMaterialType.Carvalho: return 1071428;
                    case BulkMaterialType.Pinho: return 1071429;
                    case BulkMaterialType.Mogno: return 1071430;
                    case BulkMaterialType.Eucalipto: return 1071432;
                    case BulkMaterialType.Carmesin: return 1071431;
                    case BulkMaterialType.Gelo: return 1071433;
                }
            }
            return 0;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (this.m_Deed.Deleted || !this.m_Deed.IsChildOf(this.m_From.Backpack))
                return;

            switch (info.ButtonID)
            {
                case 2: // Combine
                    {
                        this.m_From.SendGump(new SmallBODGump(this.m_From, this.m_Deed));
                        this.m_Deed.BeginCombine(this.m_From);
                        break;
                    }
                case 3: // points mode
                    {
                        BODContext c = BulkOrderSystem.GetContext(m_From);

                        if (c != null)
                        {
                            switch (c.PointsMode)
                            {
                                case PointsMode.Enabled: c.PointsMode = PointsMode.Disabled; break;
                                case PointsMode.Disabled: c.PointsMode = PointsMode.Automatic; break;
                                case PointsMode.Automatic: c.PointsMode = PointsMode.Enabled; break;
                            }
                        }

                        m_From.SendGump(new SmallBODGump(this.m_From, this.m_Deed));
                        break;
                    }
                case 4: // combine from container
                    {
                        m_From.BeginTarget(-1, false, Server.Targeting.TargetFlags.None, (m, targeted) =>
                            {
                                if (!m_Deed.Deleted && targeted is Container)
                                {
                                    List<Item> list = new List<Item>(((Container)targeted).Items);

                                    foreach (Item item in list)
                                    {
                                        m_Deed.EndCombine(m_From, item);
                                    }
                                }
                            });
                        break;
                    }
            }
        }
    }
}
