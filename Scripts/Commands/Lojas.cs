#region References
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Server.Commands.Generic;
using Server.Engines.BulkOrders;
using Server.Engines.VendorSearching;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
#endregion

namespace Server.Commands
{
    public class Lojas
    {
        public static void Initialize()
        {
            CommandSystem.Register("lojas", AccessLevel.Player, OnAction);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            if (VendorSearch.CanSearch(e.Mobile))
            {
                BaseGump.SendGump(new VendorSearchGump((PlayerMobile)e.Mobile));
            } else
            {
                e.Mobile.SendMessage("Voce precisa estar na cidade ou em sua casa para isto");
            }
        }
    }
}
