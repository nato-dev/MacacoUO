using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class PedraMontariasGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraMontariasGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
            this.AddBackground(66, 9, 305, 190, 3600);
            this.AddLabel(165, 25, 253, @"Pedra de Montarias");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
            this.AddLabel(210, 70, 1153, @"Cavalo");
			this.AddLabel(210, 100, 1153, @"Lhama");
            this.AddRadio(180, 71, 210, 211, true, 1);
            this.AddRadio(180, 101, 210, 211, false, 2);
            this.AddButton(185, 150, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                EtherealMount montaria = null;

                if (info.IsSwitched(1))
                {
                    montaria = new EtherealHorse();
                }
                else
                {
                    montaria = new EtherealLlama();
                }
                montaria.Transparent = false;
                sender.Mobile.AddToBackpack(montaria);
                sender.Mobile.SendMessage(0x00FE, "A montaria selecionada foi criada na sua mochila.");                
            }
            else
            {
                sender.Mobile.SendMessage(0x00FE, "Cancelado.");
            }
        }
    }
}
