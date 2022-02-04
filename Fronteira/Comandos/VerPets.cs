using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class VerPets
    {
        public static void Initialize()
        {
            CommandSystem.Register("meuspets", AccessLevel.Player, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            if (pl == null)
                return;

            var followers = new List<Mobile>(pl.AllFollowers);
            foreach(var f in followers)
            {
                var reg = f.Region != null ? f.Region.Name : "Floresta";
                pl.SendMessage(f.Name + " esta em " + f.Location.ToString()+" na regiao "+reg+" mapa "+f.Map);
                if(f.Map == Map.Internal && pl.Mount != f)
                {
                    f.Delete();
                    pl.SendMessage("Um dos pets estava perdido, entao foi removido");
                }
            }
        }
    }
}
