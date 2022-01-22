using System;
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
            CommandSystem.Register("wipegeral", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            var gm = arg.Mobile;
            gm.SendMessage("Indo...");
            PointsSystem.Exp.Clear();
            gm.SendMessage("Wipei exp da galera");
        }
    }
}
