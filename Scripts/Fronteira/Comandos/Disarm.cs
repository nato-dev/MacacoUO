using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class Disarm
    {
        public static void Initialize()
        {
            CommandSystem.Register("disarm", AccessLevel.Player, new CommandEventHandler(CMD));
        }

        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            pl.ClearHands();
            pl.SendMessage("Voce guardou suas armas");
        }
    }
}
