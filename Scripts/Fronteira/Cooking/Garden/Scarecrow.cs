using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute( 7732, 7733 )]
	public class Scarecrow : Item
	{
		[Constructable]
		public Scarecrow() : base( 7732 )
		{
			Name = "espantalho";
			Weight = 30.0;
		}

		public Scarecrow( Serial serial ) : base( serial ) { }
		public override bool IsAccessibleTo( Mobile check )
		{
				return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage(" ... sinistro ...");
		}

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
}
