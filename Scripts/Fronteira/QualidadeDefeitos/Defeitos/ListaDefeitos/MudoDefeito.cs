using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Fronteira.QualidadeDefeitos.ListaDefeitos
{
    public class MudoDefeito
    {
        public static void Initialize()
        {
            //EventSink.Speech += new SpeechEventHandler(OnSpeech);
        }

        private static void OnSpeech(SpeechEventArgs e)
        {
            if (e.Mobile == null)
                return;

            if (!e.Mobile.Alive || e.Mobile.IsStaff())
                return;

            PlayerMobile playerMobile = e.Mobile as PlayerMobile;

            if (playerMobile == null)
                return;

            DefeitosXmlAttachment defeitosXmlAttachment =
                XmlAttach.FindAttachment(playerMobile, typeof(DefeitosXmlAttachment)) as DefeitosXmlAttachment;

            //Verifica pela existencia do defeito
            if (defeitosXmlAttachment != null && defeitosXmlAttachment.Defeitos.TemDefeito(Defeito.Mudo))
            {
                //Vai sempre sair o say, yell e o whisper em branco
                playerMobile.Say(""); //Nao consegue falar, so usar emotes
                playerMobile.Yell("");
                playerMobile.Whisper("");
            }
        }
    }
}
