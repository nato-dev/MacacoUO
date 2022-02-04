


using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class LockRP : Gump
    {
       
        public LockRP() : base(0, 0)
        {
            this.Closable = false;
            this.Disposable = true;
            this.Dragable = false;
            this.Resizable = false;

            AddPage(0);
            AddBackground(81, 29, 627, 276, 3000);
            AddHtml(248, 269, 411, 21, @"Ultima Fronteira...  Esquecida...", (bool)false, (bool)false);
            AddBackground(86, 33, 154, 148, 3500);
            AddBackground(238, 35, 457, 146, 3500);
            AddHtml(259, 51, 416, 110, @"Prezado jogador. <br>O Ultima fronteira esta aberto apenas para staff e testers neste momento. <br> Se deseja participar dos testes procure os meios para se inscrever no discord.", (bool)false, (bool)false);
            AddImage(108, 52, 2741);
            AddImage(187, 172, 1520);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {

                        break;
                    }

            }
        }
    }
}
