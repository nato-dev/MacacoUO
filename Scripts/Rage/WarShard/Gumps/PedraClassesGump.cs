using Server.Fronteira.Classes;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
	public class PedraClassesGump : Gump
	{     
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
		public PedraClassesGump(Mobile from)
			: base(GumpOffsetX, GumpOffsetY)
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);			
            this.AddBackground(66, 9, 305, 220, 3600);
            this.AddLabel(200, 25, 253, @"Classes (Altera soh no seu char)");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
            this.AddLabel(210, 70, 1153, @"Ativar Sistema de Classes");
            this.AddLabel(210, 100, 1153, @"Desativar Sistema de Classes");

            this.AddLabel(210, 130, 1153, @"Guerreiro");
            this.AddLabel(210, 160, 1153, @"Ladino");
            this.AddLabel(210, 190, 1153, @"Mago");

            this.AddButton(180, 71, 210, 211, 1, GumpButtonType.Reply, 0);
            this.AddButton(180, 100, 210, 211, 2, GumpButtonType.Reply, 0);

            this.AddButton(180, 130, 210, 211, 3, GumpButtonType.Reply, 0);
            this.AddButton(180, 160, 210, 211, 4, GumpButtonType.Reply, 0);
            this.AddButton(180, 190, 210, 211, 5, GumpButtonType.Reply, 0);


            this.AddButton(185, 180, 247, 248, 10, GumpButtonType.Reply, 0);
		}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var pl = (PlayerMobile)sender.Mobile;
            if (info.ButtonID == 1)
            {
                pl.RP = true;
                pl.SendMessage("Sistema de classes ativado no seu personagem");
            }
            else if (info.ButtonID == 2)
            {
                pl.RP = false;
                pl.SendMessage("Sistema de classes desativado no seu personagem");
            }
            else if(info.ButtonID == 3)
            {
                if(!pl.RP)
                {
                    pl.SendMessage("Ative o sistema de classes primeiro");
                    return;
                }
                var classe = ClassDef.GetClasse(1);
                classe.ViraClasse(pl);
                pl.Nivel = 1;
                pl.PontosTalento = 1;
                pl.Talentos.Wipa();
                pl.SendMessage("Voce foi upado ao level 10. Use .talento para aprender seus talentos");
            }
            else if (info.ButtonID == 3)
            {
                if (!pl.RP)
                {
                    pl.SendMessage("Ative o sistema de classes primeiro");
                    return;
                }
                var classe = ClassDef.GetClasse(2);
                classe.ViraClasse(pl);
                pl.Nivel = 1;
                pl.PontosTalento = 1;
                pl.Talentos.Wipa();
                pl.SendMessage("Voce foi upado ao level 10. Use .talento para aprender seus talentos");
            }
            else if (info.ButtonID == 3)
            {
                if (!pl.RP)
                {
                    pl.SendMessage("Ative o sistema de classes primeiro");
                    return;
                }
                var classe = ClassDef.GetClasse(3);
                classe.ViraClasse(pl);
                pl.Nivel = 1;
                pl.PontosTalento = 1;
                pl.Talentos.Wipa();
                pl.SendMessage("Voce foi upado ao level 10. Use .talento para aprender seus talentos");
            }
        }
    }
}
