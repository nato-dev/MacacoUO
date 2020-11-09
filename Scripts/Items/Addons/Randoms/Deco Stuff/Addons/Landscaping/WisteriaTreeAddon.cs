/////////////////////////////////////////////////
//
// Automatically generated by the
// AddonGenerator script by Arya
//
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WisteriaTreeAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new WisteriaTreeAddonDeed();
			}
		}

		[ Constructable ]
		public WisteriaTreeAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 3480 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 3210 );
			AddComponent( ac, 1, -1, 27 );
			ac = new AddonComponent( 3210 );
			AddComponent( ac, 2, -1, 35 );
			ac = new AddonComponent( 3210 );
			AddComponent( ac, 1, 0, 28 );
			ac = new AddonComponent( 3210 );
			AddComponent( ac, 0, 0, 30 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, 0, 0, 18 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, 2, -1, 26 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, 1, 0, 20 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, 2, 0, 26 );
			ac = new AddonComponent( 3210 );
			AddComponent( ac, 2, -1, 17 );
			ac = new AddonComponent( 3210 );
			AddComponent( ac, 0, 1, 28 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, -1, 1, 14 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, 0, 1, 15 );

		}

		public WisteriaTreeAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class WisteriaTreeAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new WisteriaTreeAddon();
			}
		}

		[Constructable]
		public WisteriaTreeAddonDeed()
		{
			Name = "WisteriaTree";
		}

		public WisteriaTreeAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}