using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class SemVar
    {
        public static void Initialize()
        {
            CommandSystem.Register("semvar", AccessLevel.Administrator, new CommandEventHandler(Sv));
        }

        [Description("Liga/Desliga variacao de dano no shard.")]
        public static void Sv(CommandEventArgs arg)
        {
            BaseWeapon.SemVariar = !BaseWeapon.SemVariar;
            arg.Mobile.SendMessage("Variacao de dano: " + BaseWeapon.SemVariar);
        }
    }
}
