#region AuthorHeader
//
//	Auction version 2.1, by Xanthos and Arya
//
//  Based on original ideas and code by Arya
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Savings;

namespace Server.Leilaum
{
	/// <summary>
	/// Summary description for AuctionGoldCheck.
	/// </summary>
	public class AuctionGoldCheck : AuctionCheck
	{
		private static int OutbidHue = 2107;
		private static int SoldHue = 2125;

		private int m_GoldAmount;

		/// <summary>
		/// Creates a check delivering gold for the auction system
		/// </summary>
		/// <param name="auction">The auction originating this check</param>
		/// <param name="result">Specifies the reason for the creation of this check</param>
		public AuctionGoldCheck( AuctionItem auction, AuctionResult result, bool playerShop = false )
		{
			Name = AuctionSystem.ST[ 122 ];
			m_Auction = auction.ID;
			m_ItemName = auction.ItemName;

            Shard.Debug("Result: " + result.ToString());

            if (result == AuctionResult.RefundCost)
            {
                m_GoldAmount = (int)(Math.Max(auction.MinBid, auction.Reserve) * AuctionConfig.CostOfAuction);
                Shard.Debug("Min-Reserve: " + auction.MinBid + " - " + auction.Reserve);
                Shard.Debug("Gold " + m_GoldAmount);
            }
			else if ( result != AuctionResult.BuyNow && auction.HighestBid != null)
				m_GoldAmount = auction.HighestBid.Amount;
			else
				m_GoldAmount = auction.BuyNow;

			switch ( result )
			{
                case AuctionResult.RefundCost:
                    m_Owner = auction.Owner;
                    Hue = 2125;
                    m_Message = "Voce recebeu de volta o dinheiro gasto com o leilao pois seu item nao foi vendido";
                    break;

                case AuctionResult.Outbid:
				case AuctionResult.SystemStopped:
				case AuctionResult.PendingRefused:
				case AuctionResult.ReserveNotMet:
				case AuctionResult.PendingTimedOut:
				case AuctionResult.StaffRemoved:
                case AuctionResult.ItemDeleted:

					m_Owner = auction.HighestBid.Mobile;
					Hue = OutbidHue;

					switch ( result )
					{
						case AuctionResult.Outbid :
							m_Message = string.Format( AuctionSystem.ST[ 123 ] , m_ItemName, m_GoldAmount.ToString( "#,0" ));
							break;

						case AuctionResult.SystemStopped:
							m_Message = string.Format( AuctionSystem.ST[ 124 ] , m_ItemName, m_GoldAmount.ToString( "#,0" ) );
							break;

						case AuctionResult.PendingRefused:
							m_Message = string.Format( AuctionSystem.ST[ 125 ] , m_ItemName ) ;
							break;

						case AuctionResult.ReserveNotMet:
							m_Message = string.Format( AuctionSystem.ST[ 126 ] , m_GoldAmount.ToString( "#,0" ), m_ItemName );
							break;

						case AuctionResult.PendingTimedOut:
							m_Message = AuctionSystem.ST[ 127 ] ;
							break;

						case AuctionResult.ItemDeleted:
							m_Message = AuctionSystem.ST[ 128 ] ;
							break;

						case AuctionResult.StaffRemoved:
							m_Message = AuctionSystem.ST[ 202 ];
							break;
                        case AuctionResult.RefundCost:
                            m_Message = "Voce recebeu de volta o dinheiro gasto com o leilao pois seu item nao foi vendido";
                            break;
                    }
					break;

				case AuctionResult.PendingAccepted:
				case AuctionResult.Succesful:
				case AuctionResult.BuyNow:

					m_Owner = auction.Owner;
					Hue = SoldHue;
					m_Message = string.Format( AuctionSystem.ST[ 129 ] , m_ItemName, m_GoldAmount.ToString( "#,0" ) );
					break;

				default:

					throw new Exception( string.Format( "{0} is not a valid reason for an auction gold check", result.ToString() ) );
			}
		}

		public AuctionGoldCheck( Serial serial ) : base( serial )
		{
		}

		public override string ItemName
		{
			get
			{
				return string.Format( "{0} Gold Coins", m_GoldAmount.ToString( "#,0" ));
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060659, "Gold\t{0}", m_GoldAmount.ToString( "#,0" ));
		}

		public override bool Deliver( Mobile to )
		{
            Shard.Debug("Dando gold a "+m_Owner.Name);
            if (Delivered)
            {
                Shard.Debug("Delivered");
                return true;
            }

			if (!Server.Mobiles.Banker.Deposit( m_Owner, m_GoldAmount ))
			{
				m_Owner.SendMessage( AuctionConfig.MessageHue, AuctionSystem.ST[ 212 ] );
				return false;
			}
			else	// Success
			{				
				DeliveryComplete();
				Delete();
				m_Owner.SendMessage( AuctionConfig.MessageHue, AuctionSystem.ST[ 117 ] );
				return true;
			}
		}

		#region Serialization

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize (writer);

			writer.Write( 0 ); // Version
			
			writer.Write( m_GoldAmount );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize (reader);

			int version = reader.ReadInt();

			m_GoldAmount = reader.ReadInt();
		}

		#endregion
	}
}
