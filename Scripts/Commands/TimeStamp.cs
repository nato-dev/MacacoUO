using System;

namespace Server.Commands
{
    public class TimeStamp
    {
        public static void Initialize()
        {
            CommandSystem.Register("TimeStamp", AccessLevel.Player, new CommandEventHandler(CheckTime_OnCommand));
        }

        [Usage("TimeStamp")]
        [Description("Verifique a data e hora atuais dos seus servidores")]
        public static void CheckTime_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            DateTime now = DateTime.UtcNow;
            m.SendMessage("A data e hora atuais são " + now + "(EST)");
        }
    }
}
