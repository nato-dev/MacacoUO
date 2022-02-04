using Server.Mobiles;
using Server.Gumps;
using Server.Guilds;
using Server.Network;
using Server.Ziden.GuerraTotal;

namespace Server.Misc.Custom.GuerraTotal
{
    public class EntrarGuerraTotalGump : Gump
    {
        public PlayerMobile User { get; set; }

        public EntrarGuerraTotalGump(PlayerMobile pm)
            : base(50, 50)
        {
            User = pm;

            AddBackground(0, 0, 360, 300, 83);

            AddHtml(10, 25, 360, 20, "Guerra Infinita", 0xFFFF, false, false);

            AddHtml(10, 55, 340, 210, @"Voce esta prestes a participar da Guerra Infinita ! Se voce aceitar entrar, sua guilda podera ser atacada em Tretopolis" +
             "<br> Voce podera conquistar os altares de Tretopolis para ganhar recompensas. <br>Deseja Participar da Guerra Infinita ?" +
             "", 0xFFFF, false, false);

            /*
            AddButton(115, 230, 0x2622, 0x2623, 1, GumpButtonType.Reply, 0);
            AddHtml(140, 230, 150, 20, "Saber mais", 0xFFFF, false, false); // Learn more about VvV!
            */

            AddButton(10, 268, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
            AddHtml(45, 268, 100, 20, "Sim", 0xFFFF, false, false); // I Accept!

            AddButton(325, 268, 0xFB1, 0xFB3, 0, GumpButtonType.Reply, 0);
            AddHtml(285, 268, 100, 20, "<basefont color=#FFFFFF>Nao", false, false);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0: break;
                case 1:
                    User.SendGump(new EntrarGuerraTotalGump(User));
                    break;
                case 2:
                    Guild g = User.Guild as Guild;

                    if (g != null)
                    {
                        Guerra.AddGuilda(g);
                    }
                    break;
            }
        }
    }
}
