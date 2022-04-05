#region AuthorHeader
//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//
#endregion AuthorHeader
using System;
using System.Collections;

using Server;
using Server.Gumps;
using Leilaum.Utilities;

namespace Server.Leilaum
{
	/// <summary>
	/// The main gump for the auction house
	/// </summary>
	public class AuctionGump : Gump
	{
		public AuctionGump( Mobile user ) : base( 250, 450 )
		{
			user.CloseGump( typeof( AuctionGump ) );
			MakeGump();
		}

		private void MakeGump()
		{
			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;


            AddPage(0);
            this.AddBackground(63, -310, 358, 353, 3500);
            this.AddImage(73, -300, 1557);

			//AddImageTiled(49, 39, 402, 197, 3004);
			//AddImageTiled(50, 40, 400, 195, 2624);
			AddBackground(50, 40, 402, 195, 9200);

            //
            var b = 50;
            this.AddBackground(442+b, 44, 128, 191, 3500);
            this.AddItem(496 + b, 76, 3644);
            this.AddItem(497 + b, 57, 3644);
            this.AddItem(518 + b, 97, 3644);
            this.AddItem(519 + b, 78, 3644);
            this.AddItem(504 + b, 104, 2879);
            this.AddItem(483 + b, 125, 2879);
            this.AddItem(460 + b, 147, 2879);
            this.AddItem(462 + b, 156, 5049);
            this.AddItem(470 + b, 127, 5042);
            this.AddItem(497 + b, 123, 5115);
            this.AddItem(534 + b, 125, 5096);
            this.AddItem(522 + b, 139, 5088);
            this.AddItem(481 + b, 174, 5098);
            this.AddItem(452 + b, 156, 3643);
            this.AddItem(496 + b, 111, 3639);
            this.AddItem(506 + b, 106, 3639);
            this.AddItem(497 + b, 104, 3639);

            //

            AddImage(165, 65, 10452);
			AddImage(-1, 20, 10400);
			AddImage(-1, 185, 10402);
			AddImage(35, 20, 10420);
			AddImage(421, 20, 10410);
			AddImage(410, 20, 10430);
			AddImageTiled(90, 32, 323, 16, 10254);
			AddImage(420, 185, 10412);

			AddLabel(230, 45, 151, AuctionSystem.ST[ 8 ] );

			// Create new auction: B1
			AddLabel(100, 130, LUtils.kLabelHue, AuctionSystem.ST[ 9 ] );
			AddButton(60, 130, 4005, 4006, 1, GumpButtonType.Reply, 0);

			// View all auctions: B2
			AddLabel(285, 130, LUtils.kLabelHue, AuctionSystem.ST[ 10 ] );
			AddButton(245, 130, 4005, 4006, 2, GumpButtonType.Reply, 0);

			// View your auctions: B3
			AddLabel(100, 165, LUtils.kLabelHue, AuctionSystem.ST[ 11 ] );
			AddButton(60, 165, 4005, 4006, 3, GumpButtonType.Reply, 0);

			// View your bids: B4
			AddLabel(285, 165, LUtils.kLabelHue, "Ver seus Lances" );
			AddButton(245, 165, 4005, 4006, 4, GumpButtonType.Reply, 0);

			// View pendencies: B5
			AddButton( 60, 200, 4005, 4006, 5, GumpButtonType.Reply, 0 );
			AddLabel( 100, 200, LUtils.kLabelHue, AuctionSystem.ST[ 13 ] );

			// Exit: B0
			AddLabel(285, 205, LUtils.kLabelHue, AuctionSystem.ST[ 14 ] );
			AddButton(245, 200, 4017, 4018, 0, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( ! AuctionSystem.Running )
			{
				sender.Mobile.SendMessage( AuctionConfig.MessageHue, AuctionSystem.ST[ 15 ] );
				return;
			}

			switch ( info.ButtonID )
			{
				case 0: // Exit
					break;

				case 1: // Create auction
					AuctionSystem.AuctionRequest( sender.Mobile );
					break;

				case 2: // View all auctions
					sender.Mobile.SendGump( new AuctionListing( sender.Mobile, AuctionSystem.Auctions, true, true ) );
					break;

				case 3: // View your auctions

					sender.Mobile.SendGump( new AuctionListing( sender.Mobile, AuctionSystem.GetAuctions( sender.Mobile ), true, true ) );
					break;

				case 4: // View your bids

					sender.Mobile.SendGump( new AuctionListing( sender.Mobile, AuctionSystem.GetBids( sender.Mobile ), true, true ) );
					break;

				case 5: // View pendencies

					sender.Mobile.SendGump( new AuctionListing( sender.Mobile, AuctionSystem.GetPendencies( sender.Mobile ), true, true ) );
					break;
			}
		}

	}
}
