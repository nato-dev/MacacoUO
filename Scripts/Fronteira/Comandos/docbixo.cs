using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Fronteira;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class DocBixo
    {
        public static void Initialize()
        {
            CommandSystem.Register("geradocbixo", AccessLevel.Owner, new CommandEventHandler(CMD));
        }

        [Description("Gera documentacao de tds mobs (laga tudo).")]
        public static void CMD(CommandEventArgs arg)
        {
            Monstros.GeraDocMonstros();
        }
    }
}
