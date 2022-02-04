using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute( 0x1420, 0x1421 )]
	public class WeaversSpool : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefWeaving.CraftSystem; } }

		[Constructable]
		public WeaversSpool() : base( 0x1420 )
		{
			Name = "Weavers Spool";
			Weight = 2.0;
		}

		[Constructable]
		public WeaversSpool( int uses ) : base( uses, 0x1420 )
		{
			Weight = 2.0;
		}

		public WeaversSpool( Serial serial ) : base( serial )
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
