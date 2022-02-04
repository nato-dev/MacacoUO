using System;
using Server; 
using Server.Items;
using Server.Network;
using Server.Scripts; 


namespace Server.Items 
{ 
   public class HoneycombProcessingKettle : Item 
   { 

      [Constructable] 
      public HoneycombProcessingKettle() : base( 0x9ED ) 
      { 
         Name = "Caldeira de Colmeia"; 
         Weight = 10.0;             
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
         Container pack = from.Backpack; 

	if( from.InRange( this.GetWorldLocation(), 1 ) )
	{

         if (pack != null && pack.ConsumeTotal( typeof( HoneyComb ), 1 ) ) 
         { 
            from.SendMessage( "*Voce centrifuga a colmeia e separa a cera do mel*" ); 
                
               { 
					
                from.AddToBackpack( new RawBeeswax() ); 
                from.AddToBackpack( new JarHoney() );
               } 
         } 

         else 
         { 
            from.SendMessage( "Voce precisa de uma colmeia" ); 
            return; 
         } 
      } 



         else 
         { 
            from.SendMessage( "Muito longe" ); 
            return; 
         } 

	}
      public HoneycombProcessingKettle( Serial serial ) : base( serial )
		{
		}



	public override void Serialize( GenericWriter writer ) 
      { 
         base.Serialize( writer ); 

         writer.Write( (int) 0 ); // version 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
         base.Deserialize( reader ); 

         int version = reader.ReadInt(); 
      } 
   } 
  
} 

