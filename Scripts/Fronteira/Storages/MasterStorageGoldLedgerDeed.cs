using Server;
using Server.Mobiles;

namespace fronteira
{ 
   	public class MasterStorageGoldLedgerDeed : Item 
   	{ 
      	[Constructable] 
      	public MasterStorageGoldLedgerDeed() : base( 0x14F0 ) 
      	{ 
			Weight = 1.0;  
        	Movable = true;
        	Name="Master Storage Gold Ledger deed";   
      	}

      	public MasterStorageGoldLedgerDeed( Serial serial ) : base( serial ) {  } 
		
		public override void OnDoubleClick( Mobile from ) 
      	{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
			{
				MasterStorage backpack = MasterStorageUtils.GetMasterStorage(from as PlayerMobile);
				if ( backpack == null )
					from.SendMessage("You must have your Master Storage in your backpack!");
				else if ( backpack.GoldLedger )
					from.SendMessage("You already have gold ledger enabled on your Master Storage backpack.");
				else if ( !this.Deleted && !backpack.Deleted )
				{
					backpack.GoldLedger = true;
					this.Delete();
					from.SendMessage("You enabled the gold ledger on your Master Storage backpack.");
				}
			}
		}
		
		public override void Serialize( GenericWriter writer ) 
      	{ 
			base.Serialize( writer ); 

         	writer.Write( (int) 0 ); 
		} 

      	public override void Deserialize( GenericReader reader ) 
      	{ 
        	base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		}
   	} 
} 
