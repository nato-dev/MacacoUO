using System;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class FixVendors
    {
        public static void Initialize()
        {
            CommandSystem.Register("fixvendors", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        [Usage("voltayoung")]
        [Description("Volta todos players q devem ser young pra young.")]
        public static void CMD(CommandEventArgs arg) { 

            foreach(var gm in BaseVendor.AllVendors)
            {
                gm.CanMove = true;
            }
            arg.Mobile.SendMessage("Foi");
        }
    }
}
