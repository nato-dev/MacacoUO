using System;
using Server.Gumps;
using Server.Items;
using Server.Targeting;

namespace Server.Commands
{
    public class Receitas
    {
        public static void Initialize()
        {
            CommandSystem.Register("receitas", AccessLevel.Player, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            arg.Mobile.SendGump(new CurrentRecipesGump(arg.Mobile));
        }
    }
}
