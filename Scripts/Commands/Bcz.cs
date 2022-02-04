using System;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class Anuncio
    {
        public static void Initialize()
        {
            CommandSystem.Register("anuncio", AccessLevel.Seer, new CommandEventHandler(CMD));
        }

        [Usage("receitas")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            var msg = arg.ArgString;
            Anuncia(msg);
        }

        public static void Anuncia(string msg)
        {
            foreach (var mobile in PlayerMobile.Instances)
            {
                if (mobile != null && mobile.NetState != null)
                {
                    mobile.SendGump(new AnuncioGump(mobile as PlayerMobile, msg));
                }
            }
        }
    }
}
