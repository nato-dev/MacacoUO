using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Gumps;
using Server.Misc.Templates;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class TemplateHotkey : Gump
    {
        private int color = 0xFFFFFF;

        public TemplateHotkey(PlayerMobile player)
            : base(0, 0)
        {

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = false;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(5, 27, 140, 29, 9200);
            this.AddHtml(34, 31, 86, 20, @"Templates", (bool)false, (bool)false);
            this.AddButton(121, 31, 5540, 5540, 1, GumpButtonType.Reply, 0);
            this.AddItem(3, 29, 4030);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);
            if (sender==null || sender.Mobile==null || !sender.Mobile.Alive)
            {
                return;
            }
            var player = (PlayerMobile)sender.Mobile;
            if (info != null && info.ButtonID == 1)
            {
                player.SendGump(new TemplatesGump(player));
            }
        }

    }
}
