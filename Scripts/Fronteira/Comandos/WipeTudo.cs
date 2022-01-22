using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Engines.Points;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class WipeGeral
    {
        public static void Initialize()
        {
            CommandSystem.Register("wipegeral", AccessLevel.Owner, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            arg.Mobile.SendMessage("Wipando");
            var all = new List<IAccount>(Accounts.GetAccounts());
            foreach (var acc in all)
            {
                if (acc.AccessLevel >= AccessLevel.VIP)
                {
                    continue;
                }
                acc.Delete();
            }
            arg.Mobile.SendMessage("Wipado");
        }
    }
}
