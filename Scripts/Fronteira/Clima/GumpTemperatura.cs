
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Fronteira.Clima;

namespace Server.Gumps
{
    public class GumpTemperatura : Gump
    {
        private static int X_CENTRO = 206;
        private static int X_MIN = 90;
        private static int X_MAX = 325;

        private static int VARIACAO = X_MAX - X_MIN;
        private static int MIN_FLAT = 0;

        public GumpTemperatura(Mobile player) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            var temperatura = player.Temperatura;

            // X * 20 = MAX_FLAT * temperatura]

            /*
            MIN = 0
            MAX = 40
            X   = tmp

            x * 40 = MAX * tmp
            X = (MAX * tmp) / 40
             
             
         
            var normalizado = player.Temperatura + Clima.MAX;
            var porcentagem = 

            var posicaoX = X_MIN + (VARIACAO * (temperatura + 20));
               */

            // SETINHA DOMAL
            AddImage(X_MIN, 72, 5600);

            AddPage(0);
            AddBackground(60, 43, 308, 50, 9350);
            AddBackground(89, 70, 252, 10, 9200);

            var texto = "";
            if (temperatura < 0)
                texto = "Frio";
            else if (temperatura > 0)
                texto = "Calor";

            AddHtml(102, 48, 220, 18, texto, (bool)false, (bool)false);
            AddItem(66, 52, 9006);
            AddItem(307, 49, 14030);
        }
    }
}
