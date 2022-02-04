
using Server;
using Server.Mobiles;

namespace fronteira
{ 
   	public class MasterStorageKeepItemsDeed : Item 
   	{ 
      	[Constructable] 
      	public MasterStorageKeepItemsDeed() : base( 0x14F0 ) 
      	{ 
			Weight = 1.0;  
        	Movable = true;
        	Name="Master Storage Keep Items deed";   
      	}

      	public MasterStorageKeepItemsDeed( Serial serial ) : base( serial ) {  } 
		
		public override void OnDoubleClick( Mobile from ) 
      	{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
			{
				MasterStorage backpack = MasterStorageUtils.GetMasterStorage(from as PlayerMobile);
				if ( backpack == null )
					from.SendMessage("You must have your Master Storage in your backpack!");
				else if ( backpack.KeepItemsOnDeath )
					from.SendMessage("You already keep your items when you die.");
				else if ( !this.Deleted && !backpack.Deleted )
				{
					backpack.KeepItemsOnDeath = true;
					this.Delete();
					from.SendMessage("From now on you'll keep your items when you'll die.");
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
