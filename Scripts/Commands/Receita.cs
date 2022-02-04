using System;
using Server.Engines.Craft;
using Server.Gumps;
using Server.Targeting;

namespace Server.Commands
{
    public class Receita
    {
        public static void Initialize()
        {
            CommandSystem.Register("receita", AccessLevel.GameMaster, new CommandEventHandler(OnCommand));
      
        }

        [Usage("receita")]
        [Description("Camping Menu.")]
        public static void OnCommand(CommandEventArgs arg)
        {
            
        }
    }
}
