#region References
using System;

using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
#endregion

namespace Server.Gumps
{
    public class GumpSeguro : Gump
    {
        public static void Configure()
        {
            CommandSystem.Register("insure", AccessLevel.Player, DisplayTo);
            CommandSystem.Register("insurance", AccessLevel.Player, DisplayTo);
        }

        [Usage("Deixa joias e roupas newbie")]
        public static void DisplayTo(CommandEventArgs e)
        {
            var user = e.Mobile as PlayerMobile;
            if (user == null || user.Deleted || !user.Player || user.NetState == null)
                return;
            user.OpenItemInsuranceMenu();
            /*
            var user = e.Mobile as PlayerMobile;
            if (user == null || user.Deleted || !user.Player || user.NetState == null)
                return;

            user.CloseGump(typeof(GumpSeguro));
            var gump = new GumpSeguro(user);
            user.SendGump(gump);
            */
        }

        private GumpSeguro(PlayerMobile user)
            : base(0, 0)
        {

            Dragable = true;
            Closable = true;
            Resizable = false;
            Disposable = false;

            AddPage(0);
            AddBackground(184, 286, 413, 240, 9200);
            AddBackground(225, 294, 334, 28, 3000);
            AddHtml(230, 297, 333, 24, "<CENTER>Seguro de Items</CENTER>", true, false);
            AddBackground(194, 332, 397, 103, 3000);
            AddHtml(201, 337, 377, 88, "Voce pode colocar seguro em joias e roupas. Se voce morrer com estes items, voce pagara um preco para nao perde-los. Quem lhe matou levara parte deste dinheiro.", true, false);
            AddButton(196, 455, 2151, 248, (int)Buttons.Insure, GumpButtonType.Reply, 0);
            AddBackground(230, 455, 359, 29, 3000);
            AddBackground(231, 490, 359, 29, 3000);
            AddButton(196, 488, 2151, 248, (int)Buttons.Renew, GumpButtonType.Reply, 0);
            AddHtml(237, 460, 298, 27, "Botar Seguro em Item", true, false);
            AddHtml(235, 495, 298, 27, "Auto-Renovar: "+ (user.AutoRenewInsurance ? "Ligado":"Desligado"), true, false);
            AddItem(177, 291, 6226);
            AddItem(553, 291, 6225);
            AddImage(134, 225, 10400);
            AddImage(253, 35, 1418);
            AddImage(560, 524, 10460);
            AddImage(192, 523, 10460);
            AddImage(565, 225, 10410);
            AddImage(565, 469, 10412);
            AddImage(134, 468, 10402);
        }

        public enum Buttons
        {
            Insure = 1,
            Renew = 2,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var pl = sender.Mobile as PlayerMobile;
            if (pl == null)
                return;

            if(info.ButtonID==(int)Buttons.Insure)
            {
                pl.OpenItemInsuranceMenu();
            } else
            {
                pl.AutoRenewInsurance = !pl.AutoRenewInsurance;
                pl.CloseGump(typeof(GumpSeguro));
                pl.SendGump(new GumpSeguro(pl));
            }
        }
    }
}
