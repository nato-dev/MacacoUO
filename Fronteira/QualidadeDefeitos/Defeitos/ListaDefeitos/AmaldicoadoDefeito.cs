using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Fronteira.QualidadeDefeitos.ListaDefeitos
{
    public class AmaldicoadoDefeito
    {
        public static void Initialize()
        {
            //TODO Verificar uma forma melhor de fazer essa checagem quando o gump de criacao for finalizado
            EventSink.Movement += new MovementEventHandler(OnMovement);
        }

        private static void OnMovement(MovementEventArgs e)
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
            if (defeitosXmlAttachment != null && defeitosXmlAttachment.Defeitos.TemDefeito(Defeito.Amaldicoado))
            {
                playerMobile.Followers = 0; //Nao pode ter animais
            }
        }
    }
}
