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
using Server.Network;
#endregion

namespace Server.Commands
{
    public class ActionCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("Action", AccessLevel.Administrator, OnAction);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            var action = e.GetInt32(0);
            e.Mobile.Animate(AnimationType.Attack, action);
        }
    }
}
