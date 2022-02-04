using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class DPS
    {
        public static void Initialize()
        {
            CommandSystem.Register("dp", AccessLevel.Player, new CommandEventHandler(CMD));
            CommandSystem.Register("mortes", AccessLevel.Player, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            if (pl == null)
                return;

            if(!pl.RP)
            {
                pl.SendMessage("Apenas Personagens RP devem se preocupar com isto");
                return;
            }
            pl.SendMessage(pl.Deaths + "/" + PlayerMobile.MAX_MORTES);
        }
    }
}
