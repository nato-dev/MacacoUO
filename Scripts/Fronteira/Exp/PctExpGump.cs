using Server.Commands;
using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Gumps
{
    public class PctExpGump : Gump
    {
        public PctExpGump(PlayerMobile caller, double pct, string text) : base(240, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(10, 40, 220, 54, 9200);
          

            AddHtml(20, 45, 200, 25, text + " " + String.Format("{0:0.00}", pct), true, false);
            AddImageTiled(20, 72, 110, 12, 2053);
            AddImageTiled(20, 72, (int)pct, 12, 2054);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {

                        break;
                    }

            }
        }
    }
}
