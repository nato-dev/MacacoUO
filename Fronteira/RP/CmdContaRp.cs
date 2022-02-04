#region References
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Server.Accounting;
using Server.Commands.Generic;
using Server.Engines.BulkOrders;
using Server.Engines.Points;
using Server.Items;
using Server.Network;
using Server.Targeting;
#endregion

namespace Server.Commands
{
    public class CmdContaRP
    {
        public static void Initialize()
        {
            CommandSystem.Register("contarp", AccessLevel.Administrator, Cmd);
        }

        [Usage("Action")]
        private static void Cmd(CommandEventArgs e)
        {
            if (e.Arguments.Length != 1)
            {
                e.Mobile.SendMessage("Use .contarp <login> para tornar uma conta RP (podera criar personagems RP)");
                return;
            }
            var conta = e.GetString(0);
            var acc = Accounts.GetAccount(conta) as Account;

            if(acc == null)
            {
                e.Mobile.SendMessage("Conta nao existe");
                return;
            }
            e.Mobile.SendMessage("A conta " + conta + " agora pode criar personagems RP");
            acc.RP = true;
            var mob = acc.GetOnlineMobile();
            if(mob != null)
            {
                mob.SendMessage(78, "Sua conta agora pode criar personagens RP !");
            }
        }
    }
}
