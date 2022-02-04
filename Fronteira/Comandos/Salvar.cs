using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class Salvar
    {
        public static void Initialize()
        {
            CommandSystem.Register("salvar", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        public static void CMD(CommandEventArgs arg)
        {
            
        }
    }
}
