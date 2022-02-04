using System;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class GenCamps
    {
        public static void Initialize()
        {
            CommandSystem.Register("gencamps", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        [Usage("voltayoung")]
        [Description("Volta todos players q devem ser young pra young.")]
        public static void CMD(CommandEventArgs arg)
        {

           
        }
    }
}
