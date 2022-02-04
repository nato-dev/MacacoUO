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
    public class RPCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("modorp", AccessLevel.Player, OnAction);
        }

        [Usage("modorp")]
        private static void OnAction(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Comando desabilitado temporariamente");
            /*
            var pl = e.Mobile as PlayerMobile;
            pl.RP = !pl.RP;
            if (pl.RP)
            {
                pl.SendMessage("Modo RP Ligado");
                pl.OverheadMessage("[RP ON]");
            }
            else
            {
                pl.OverheadMessage("[RP OFF]");
                pl.SendMessage("Modo RP Desligado");
            }
            */
        }
    }
}
