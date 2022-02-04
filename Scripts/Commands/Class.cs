using System;
using Server.Gumps;
using Server.Targeting;

namespace Server.Commands
{
    public class Class
    {
        public static void Initialize()
        {
            CommandSystem.Register("class", AccessLevel.GameMaster, new CommandEventHandler(Cmd));
        }

        [Usage("class")]
        [Description("test.")]
        public static void Cmd(CommandEventArgs arg)
        {
            arg.Mobile.SendGump(new NonRPClassGump());
        }
    }
}
