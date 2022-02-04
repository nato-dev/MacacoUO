using System;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class WipaPratinhas
    {
        public static void Initialize()
        {
            CommandSystem.Register("#" +
                "", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg) { 

            foreach(var gm in PlayerMobile.Instances)
            {
                if (gm == null || gm.Young)
                    continue;

                var acc = gm.Account as Account;
                if (acc.TotalGameTime < Account.YoungDuration)
                {
                    acc.Young = true;
                    gm.Young = true;
                    arg.Mobile.SendMessage(gm.Name + "Virou Young");
                }
            }
            arg.Mobile.SendMessage("Foi");
        }
    }
}
