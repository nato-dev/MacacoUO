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
using Server.Engines.Points;
using Server.Items;
using Server.Mobiles;
using Server.Network;
#endregion

namespace Server.Commands
{
    public class PontosCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("pontos", AccessLevel.Player, OnAction);
        }

        [Usage("pontos")]
        [Description("Visualiza diversos pontos que podem ser obtidos in-game.")]
        private static void OnAction(CommandEventArgs e)
        {
            var m_From = e.Mobile as PlayerMobile;
            m_From.CloseGump(typeof(LoyaltyRatingGump));
            m_From.SendGump(new LoyaltyRatingGump(m_From));
        }
    }
}
