using System;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class VoltaYoung
    {
        public static void Initialize()
        {
            CommandSystem.Register("voltayoung", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            var gm = arg.Mobile;
            gm.SendMessage("Indo...");
            foreach(var pl in PlayerMobile.Instances)
            {
                if(!pl.Young)
                {
                    var ts = Account.YoungDuration - pl.Account.TotalGameTime;
                    if(ts.Minutes > 0)
                    {
                        pl.Young = true;
                        pl.Account.Young = true;
                        gm.SendMessage(pl.Name);
                    }
                }
            }
        }
    }
}
