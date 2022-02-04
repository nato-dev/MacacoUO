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
    public class Skillock
    {
        public static void Initialize()
        {
            CommandSystem.Register("skillock", AccessLevel.Administrator, OnAction);
            CommandSystem.Register("skilllock", AccessLevel.Administrator, OnAction);
            CommandSystem.Register("statlock", AccessLevel.Administrator, OnAction);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Para travar/destravar skills ou stats, abra a janela de skills ou stats e clique na setinha para subir/descer/travar skills.");
        }
    }
}
