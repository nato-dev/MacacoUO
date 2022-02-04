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
using Server.Items;
using Server.Mobiles;
using Server.Network;
#endregion

namespace Server.Commands
{
    public class ConfrariaCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("removeconfraria", AccessLevel.Administrator, OnAction);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            foreach(var p in PlayerMobile.Instances)
            {
                if (p.NpcGuild != NpcGuild.None)
                    p.NpcGuild = NpcGuild.None;
            }
        }
    }
}
