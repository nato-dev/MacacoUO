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
    public class SaldoCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("saldo", AccessLevel.Player, OnAction);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Seu saldo no banco: " + Banker.GetBalance(e.Mobile));

        }
    }
}
