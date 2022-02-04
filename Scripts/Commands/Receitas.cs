using System;
using Server.Gumps;
using Server.Targeting;

namespace Server.Commands
{
    public class Camp
    {
        public static void Initialize()
        {
            CommandSystem.Register("camp", AccessLevel.GameMaster, new CommandEventHandler(CampCmd));
        }

        [Usage("camp")]
        [Description("Camping Menu.")]
        public static void CampCmd(CommandEventArgs arg)
        {
            GoGump.DisplayToCampfire(arg.Mobile);
        }
    }
}
