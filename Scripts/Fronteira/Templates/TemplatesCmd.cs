using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class Templates
    {
        public static void Initialize()
        {
            CommandSystem.Register("Templates", AccessLevel.Player, new CommandEventHandler(Cmd));
        }

        [Usage("Templates")]
        [Description("Templates.")]
        public static void Cmd(CommandEventArgs arg)
        {
            if(arg.Mobile.RP)
            {
                arg.Mobile.SendMessage("Voce nao pode usar este comando sendo um personagem RP");
                return;
            }
            arg.Mobile.CloseGump(typeof(TemplatesGump));
            arg.Mobile.SendGump(new TemplatesGump((PlayerMobile)arg.Mobile));
        }
    }
}
