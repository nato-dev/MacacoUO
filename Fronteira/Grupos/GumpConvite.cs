
using Server.Network;
using Server.Mobiles;
using Server.Misc.Custom;

namespace Server.Gumps
{
    public class GumpConvite : Gump
    {
        private Mobile convidou;
        private Mobile convidado;

        public GumpConvite(Mobile convidou, Mobile convidado) : base(0, 20)
        {
            this.convidado = convidado;
            this.convidou = convidou;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = false;
            this.Resizable = false;

            AddPage(0);
            AddBackground(-1, 27, 437, 25, 9200);
            AddHtml(28, 30, 321, 22, convidou.Name+" convidou para um grupo.", (bool)false, (bool)false);
            AddButton(372, 30, 4024, 4024, (int)Buttons.OK, GumpButtonType.Reply, 0);
            AddButton(402, 30, 4018, 4024, (int)Buttons.XO, GumpButtonType.Reply, 0);
            AddItem(-6, 29, 3636);

           
        }

        private enum Buttons
        {
            XO,
            OK,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if(info.ButtonID == (int)Buttons.OK)
            {
                PartyCommands.Handler.OnAccept(convidado, convidou);
                convidado.SendMessage("Voce aceitou fazer parte do grupo. Bonus de 20% em ouro de monstros");
            } else
            {
                convidou.SendMessage("Seu convite foi recusado");
            }
        }
    }
}
