using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Fronteira.QualidadeDefeitos.ListaDefeitos
{
    public class AsmaDefeito
    {
        public static void Initialize()
        {
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

            if (!playerMobile.Mounted && playerMobile.Correndo())
            {
                DefeitosXmlAttachment defeitosXmlAttachment =
                    XmlAttach.FindAttachment(playerMobile, typeof(DefeitosXmlAttachment)) as DefeitosXmlAttachment;
                if (defeitosXmlAttachment != null && defeitosXmlAttachment.Defeitos.TemDefeito(Defeito.Asma))
                {
                    playerMobile.Stam -= 2; //Perde mais 30% de estamina ao correr a pe
                }
            }
        }
    }
}
