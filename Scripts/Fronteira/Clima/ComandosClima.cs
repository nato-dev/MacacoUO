using Server.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Clima
{
    public class ComandosClima
    {

        public static int MAX = 10;
        public static int MIN = -10;

        public static void Initialize()
        {
            CommandSystem.Register("settemperatura", AccessLevel.Administrator, new CommandEventHandler(CMD));
            CommandSystem.Register("temperatura", AccessLevel.Administrator, new CommandEventHandler(CMD2));
        }

        [Usage("")]
        [Description(".")]
        public static void CMD2(CommandEventArgs arg)
        {
            var mobile = arg.Mobile;
            if (mobile.Region == null)
            {
                mobile.SendMessage("Vc precisa tar numa regiao p usar issaki");
                return;
            }
            var temperatura = Clima.GetTemperatura(arg.Mobile.Region);
            mobile.SendMessage("Temperatura da regiao local: " + temperatura);
            mobile.SendMessage(String.Format ("Sua protecao contra frio: {0}", Clima.GetProtecao(mobile)));
        }

        [Usage("")]
        [Description(".")]
        public static void CMD(CommandEventArgs arg)
        {
            var mobile = arg.Mobile;
            if(mobile.Region == null)
            {
                mobile.SendMessage("Vc precisa tar numa regiao p usar issaki");
                return;
            }

            if(mobile.Region.Name == null)
            {
                mobile.SendMessage("Vc precisa tar numa regiao definida com nome p usar issaki");
                return;
            }

            if(arg.Arguments.Length < 1)
            {
                mobile.SendMessage("Vc precisa usar tipo .settemperatura 5 pra setar 5 graus");
                return;
            }

            try
            {
                int graus = arg.GetInt32(0);
                Clima.SetTemperatura(mobile.Region, graus);
                mobile.SendMessage("Temperatura setada para " + graus);

            }
            catch(Exception e)
            {
                mobile.SendMessage("Vc precisa usar tipo .settemperatura 5 pra setar 5 graus");
            }
          
        }
    }
}
