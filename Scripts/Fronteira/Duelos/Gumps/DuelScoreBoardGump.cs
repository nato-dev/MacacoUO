using System;
using Server;
using Server.Gumps;

namespace Server.Dueling
{
	public class DuelScoreBoardGump : Gump
	{
		public DuelScoreBoardGump()
			: base( 200, 200 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(36, 25, 732, 375, 3500);
			this.AddButton(733, 46, 3, 4, (int)Buttons.CopyofcloseBtn, GumpButtonType.Reply, 0);
			this.AddLabel(292, 39, 36, @"Duelos - Top10");
			this.AddHtml( 49, 102, 142, 23, @"", (bool)true, (bool)false);
			this.AddLabel(104, 82, 36, @"Nome");
			this.AddLabel(590, 82, 36, @"Tot. Vitorias");
			this.AddLabel(674, 82, 36, @"Tot. Derrotas");
			this.AddLabel(343, 82, 36, @"Mais Rapido");
			this.AddLabel(476, 82, 36, @"Mais Longo");
			this.AddLabel(371, 60, 36, @"Tipo: 1vs1");
			this.AddLabel(207, 82, 36, @"Vitorias");
			this.AddLabel(270, 82, 36, @"Derrotas");
			this.AddHtml( 193, 102, 61, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 256, 102, 61, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 319, 102, 130, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 450, 102, 130, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 582, 102, 85, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 668, 102, 85, 23, @"", (bool)true, (bool)false);
			this.AddLabel(292, 38, 36, @"Duelos - Top10");
			this.AddLabel(104, 81, 36, @"Nome");
			this.AddLabel(590, 81, 36, @"Tot. Vitorias");
			this.AddLabel(674, 81, 36, @"Tot. Derrotas");
			this.AddLabel(343, 81, 36, @"Mais Rapido");
			this.AddLabel(476, 81, 36, @"Mais Longo");
			this.AddLabel(371, 59, 36, @"Tipo: 1vs1");
			this.AddLabel(207, 81, 36, @"Vitorias");
			this.AddLabel(270, 81, 36, @"Derrotas");
			this.AddButton(291, 368, 5603, 5607, (int)Buttons.prevBtn, GumpButtonType.Reply, 0);
			this.AddButton(494, 368, 5601, 5605, (int)Buttons.nextBtn, GumpButtonType.Reply, 0);
			this.AddLabel(314, 366, 36, @"Anterior");
			this.AddLabel(461, 366, 36, @"Proximo");

		}
		
		public enum Buttons
		{
			CopyofcloseBtn,
			prevBtn,
			nextBtn,
		}

	}
}
