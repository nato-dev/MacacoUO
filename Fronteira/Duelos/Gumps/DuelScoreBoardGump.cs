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
			this.AddLabel(292, 39, 36, @"Onsite Duel System Top 10 Scoreboard");
			this.AddHtml( 49, 102, 142, 23, @"", (bool)true, (bool)false);
			this.AddLabel(104, 82, 36, @"Name");
			this.AddLabel(590, 82, 36, @"Total Wins");
			this.AddLabel(674, 82, 36, @"Total Loses");
			this.AddLabel(343, 82, 36, @"Quickest Duel");
			this.AddLabel(476, 82, 36, @"Longest Duel");
			this.AddLabel(371, 60, 36, @"Duel Type: 1vs1");
			this.AddLabel(207, 82, 36, @"Wins");
			this.AddLabel(270, 82, 36, @"Loses");
			this.AddHtml( 193, 102, 61, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 256, 102, 61, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 319, 102, 130, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 450, 102, 130, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 582, 102, 85, 23, @"", (bool)true, (bool)false);
			this.AddHtml( 668, 102, 85, 23, @"", (bool)true, (bool)false);
			this.AddLabel(292, 38, 36, @"Onsite Duel System Top 10 Scoreboard");
			this.AddLabel(104, 81, 36, @"Name");
			this.AddLabel(590, 81, 36, @"Total Wins");
			this.AddLabel(674, 81, 36, @"Total Loses");
			this.AddLabel(343, 81, 36, @"Quickest Duel");
			this.AddLabel(476, 81, 36, @"Longest Duel");
			this.AddLabel(371, 59, 36, @"Duel Type: 1vs1");
			this.AddLabel(207, 81, 36, @"Wins");
			this.AddLabel(270, 81, 36, @"Loses");
			this.AddButton(291, 368, 5603, 5607, (int)Buttons.prevBtn, GumpButtonType.Reply, 0);
			this.AddButton(494, 368, 5601, 5605, (int)Buttons.nextBtn, GumpButtonType.Reply, 0);
			this.AddLabel(314, 366, 36, @"Previous");
			this.AddLabel(461, 366, 36, @"Next");

		}
		
		public enum Buttons
		{
			CopyofcloseBtn,
			prevBtn,
			nextBtn,
		}

	}
}