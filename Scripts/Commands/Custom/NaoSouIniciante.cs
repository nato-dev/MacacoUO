using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Commands
{
    public class NaoSouIniciante
    {

        public static void Initialize()
        {
            CommandSystem.Register("NaoSouIniciante", AccessLevel.Player, NaoSouIniciante_OnCommand);
        }

        public static void NaoSouIniciante_OnCommand(CommandEventArgs t)
        {
            Mobile from = t.Mobile;

            if (!from.HasGump(typeof(RenounceYoungGump)))
            {
                if (from is PlayerMobile && ((PlayerMobile)from).Young)
                {
                    from.SendGump(new RenounceYoungGump());
                }
                else
                {
                    t.Mobile.SendMessage(0x00FE, "Você não é mais iniciante.");
                }
            }
            else
            {
                t.Mobile.SendMessage(0x00FE, "Você deve responder no gump que já está aberto.");
            }
        }
    }
}
