using System.Collections.Generic;
using Server.Commands;
using Server.Network;

namespace Server.Bounds
{
    public class Online
    {
        public static void Initialize()
        {
            CommandSystem.Register("Online", AccessLevel.Player, new CommandEventHandler(Online_OnCommand));
        }

        public static int GetOnlinePlayers()
        {
            return NetState.GetOnlinePlayers();
        }

        [Usage("Online")]
        [Description("Online")]
        public static void Online_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Densidade demografica nao mensuravel");
            /*
            List<Mobile> list = new List<Mobile>();
            List<NetState> states = NetState.Instances;

            if (GetOnlinePlayers() == 1)
            {
                e.Mobile.SendMessage("Temos " + GetOnlinePlayers() + " jogador online.");
            }
            else if (GetOnlinePlayers() > 1)
            {
                e.Mobile.SendMessage("Temos " + GetOnlinePlayers() + " jogadores online.");
            }
            else
            {
                e.Mobile.SendMessage("Não temos nenhum jogador online no momento.");
            }
            */
        }
    }
}
