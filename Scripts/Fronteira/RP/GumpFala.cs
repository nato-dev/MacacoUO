


using Server.Network;
using Server.Commands;
using System;

namespace Server.Gumps
{

    public enum Faces
    {
        GM_PRETO = 2741
    }

    public class GumpFala : Gump
    {
        private Action<int> Callback;


        public GumpFala(Action<int> callback, Faces face = Faces.GM_PRETO, params string [] lines) : base(0, 0)
        {
            this.Callback = callback;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(81, 29, 627, 256, 3000);
            //AddHtml(248, 269, 411, 21, titulo, (bool)false, (bool)false);
            AddBackground(86, 33, 154, 148, 3500);
            AddBackground(238, 35, 457, 146, 3500);
            AddHtml(259, 51, 416, 110, string.Join("</br>", lines), (bool)false, (bool)false);
            AddImage(108, 52, (int)face);
            AddImage(187, 172, 1520);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if(this.Callback != null)
                this.Callback(info.ButtonID);
        }
    }
}
