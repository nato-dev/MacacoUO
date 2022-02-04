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
using Server.Engines.UOStore;
using Server.Items;
using Server.Mobiles;
using Server.Network;
#endregion

namespace Server.Commands
{
    public class LojaCMD
    {
        public static void Initialize()
        {
            CommandSystem.Register("loja", AccessLevel.Player, OnAction);
        }

        [Usage("loja")]
        private static void OnAction(CommandEventArgs e)
        {
            UltimaStore.OpenStore((PlayerMobile)e.Mobile);
        }
    }
}
