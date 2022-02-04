using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Guilds;
using Server.Network;

namespace Server.Engines.VvV
{
    public class ConfirmSignupGump : Gump
    {
        public PlayerMobile User { get; set; }

        public ConfirmSignupGump(PlayerMobile pm)
            : base(50, 50)
        {
            User = pm;

            AddBackground(0, 0, 360, 300, 83);

            AddHtml(0, 25, 360, 20, "Guerra Infinita", 0xFFFF, false, false); // Vice vs Virtue Signup

            if (ViceVsVirtueSystem.EnhancedRules)
            {
                AddHtml(10, 55, 340, 165, _EnhancedRulesMessage, false, true);
            }
            else
            {
                AddHtml(10, 55, 340, 210, @"Saudações! Você está prestes a se juntar a Guerra Infinita ! GI é um emocionante PvP, experiência com a qual você pode se divertir, quer tenha horas ou apenas alguns minutos para" +
                 " pular para a ação!Esteja avisado, depois de se juntar a Guerra Infinita, você será livremente atacável" +
                 " por outros participantes da Guerra Infinita. < br > < br > Você atende a chamada" +
                 " e deseja liderar sua guilda à vitória ?", 0xFFFF, false, false);
                /*Greetings! You are about to join Vice vs Virtue! VvV is an exhilarating Player vs Player 
                 * experience that you can have fun with whether you have hours or only a few minutes to 
                 * jump into the action!  Be forewarned, once you join VvV you will be freely attackable 
                 * by other VvV participants in non-consensual PvP facets.<br><br>Will you answer the call
                 * and lead your guild to victory?*/
            }

            /*
            AddButton(115, 230, 0x2622, 0x2623, 1, GumpButtonType.Reply, 0);
            AddHtml(140, 230, 150, 20, "Saber mais", 0xFFFF, false, false); // Learn more about VvV!
            */

            AddButton(10, 268, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
            AddHtml(45, 268, 100, 20, "Aceito", 0xFFFF, false, false); // I Accept!

            AddButton(325, 268, 0xFB1, 0xFB3, 0, GumpButtonType.Reply, 0);
            AddHtml(285, 268, 100, 20, "<basefont color=#FFFFFF>Cancelar", false, false);
        }

        private string _EnhancedRulesMessage =
           "<basefont color = # FFFFFF> Saudações! Você está prestes a se juntar ao Vice vs Virtue! VvV é um emocionante Player vs Player" +
"experiência com a qual você pode se divertir, quer tenha horas ou apenas alguns minutos para" +
"entre em ação! Esteja avisado que, quando você entrar no VvV, você poderá atacar livremente" +
"por outros participantes do VvV em <b> qualquer </b> lugar. <br> <br> Você atenderá a chamada" +
"e levara sua guilda à vitória? Observe as regras aprimoradas ligeiramente diferentes às quais você não pode estar acostumado: <br> <br>" +
"- VvV Combat em qualquer faceta <br> - Redução de prata durante batalhas na cidade quando não contestada <br> - Combate restrições de viagens quando na VvV Combat Zone";
        public override void OnResponse(NetState state, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0: break; 
                case 1:
                    //User.LaunchBrowser("http://uo.com/wiki/ultima-online-wiki/publish-notes/publish-86/");
                    User.SendGump(new ConfirmSignupGump(User));
                    break;
                case 2:
                    Guild g = User.Guild as Guild;

                    if (g != null)
                    {
                        ViceVsVirtueSystem.Instance.AddPlayer(User);
                    }
                    break;
            }
        }
    }
}
