using System;
using Server.Mobiles;
using Server.Multis;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Regions;

namespace Server.Commands
{
    public class Debugar
    {
        public static void Initialize()
        {
            CommandSystem.Register("debug", AccessLevel.Administrator, DebugarCmd);
        }

        public static void DebugarCmd(CommandEventArgs t)
        {
            Shard.DebugGritando = !Shard.DebugGritando;

            t.Mobile.SendMessage("DebugMode " + (Shard.DebugGritando? "Ligado" : "Desligado"));
        }
    }
}
