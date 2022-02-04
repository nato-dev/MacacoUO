using Server.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Commands
{
    public class Mark
    {
        public static void Initialize()
        {
            CommandSystem.Register("Mark", AccessLevel.GameMaster, new CommandEventHandler(Mark_OnCommand));
        }

        [Usage("Mark [name]")]
        [Description("Cria uma runa marcada em sua localização.")]
        private static void Mark_OnCommand(CommandEventArgs e)
        {
            if (e.Arguments.Length <= 0)
            {
                e.Mobile.SendMessage("Uso: marca [RuneName]");
                return;
            }

            var runeName = e.Arguments[0];

            var rune = new RecallRune();
            rune.Mark(e.Mobile);
            rune.Name = rune.Description = runeName;

            e.Mobile.AddToBackpack(rune);
            e.Mobile.SendMessage(string.Format("Runa {0} adicionado à sua mochila.", runeName));
        }
    }
}
