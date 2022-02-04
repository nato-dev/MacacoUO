using System; 
using Server; 
using Server.Gumps; 
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class GardenDGump : Gump
    {
        public GardenDGump(GardenDestroyer gardendestroyer, Mobile owner)
            : base(150, 75)
        {
            m_GardenDestroyer = gardendestroyer;
            owner.CloseGump(typeof(GardenDGump));
            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 445, 250, 9200);
            AddBackground(10, 10, 425, 160, 3500);
            AddLabel(95, 30, 195, @"* Destruir o jardim ? *");
            AddLabel(60, 70, 1359, @"Espero que tenha removido os items do bau");
            AddLabel(60, 90, 1359, @"antes de destruir TUDO. ");

            AddLabel(107, 205, 172, @"Destruir");
            AddLabel(270, 205, 32, @"Nao destruir");
            AddButton(115, 180, 4023, 4024, 1, GumpButtonType.Reply, 0);
            AddButton(295, 180, 4017, 4018, 0, GumpButtonType.Reply, 0);
        }

        private GardenDestroyer m_GardenDestroyer;

        public override void OnResponse(NetState state, RelayInfo info) //Function for GumpButtonType.Reply Buttons 
        {

            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
                    {
                        //Cancel
                        from.SendMessage("Ok.");
                        break;
                    }

                case 1: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
                    {

                        //RePack 
                        m_GardenDestroyer.Delete();
                        from.AddToBackpack(new GardenDeed());
                        from.SendMessage(" Voce destruiu o jardim.");
                        break;
                    }
            }
        }
    }

    public class GardenGump : Gump
    {
        public GardenGump(Mobile owner)
            : base(150, 75)
        {
            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 445, 395, 9200);
            AddBackground(10, 10, 425, 375, 3500);
            AddLabel(40, 140, 1359, @"Estas ferramentas de jardim sao suas.");
            AddLabel(140, 200, 195, @"* CUIDADO *");
            AddLabel(40, 240, 1359, @"Quando voce destruir o jardim, tudo sera removido");
            AddLabel(40, 260, 1359, @"inclusive o bau e seus items.");

            AddButton(310, 330, 9723, 9724, (int)Buttons.Button1, GumpButtonType.Reply, 0);
            AddLabel(135, 50, 195, @"* Voce colocou seu jardim. *");
            AddLabel(40, 80, 1359, @"A placa do jardim pode ser usada para remover o jardim.");

            AddLabel(40, 120, 1359, @"Voce tambem vai ver que existe um bau seguro.");
            AddLabel(140, 335, 32, @"Eu li o aviso");
        }

        public enum Buttons
        {
            Button1,
        }
    }
}
