/////////////////////////////////////////////////
//                                             //
// Automatically generated by the              //
// AddonGenerator script by Arya               //
//                                             //
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class gardenwellAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new gardenwellAddonDeed();
			}
		}

		[ Constructable ]
		public gardenwellAddon()
		{
			AddonComponent ac;
			ac = new AddonComponent( 3272 );
			AddComponent( ac, 2, -2, 1 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, -2, 1, 1 );
			ac = new AddonComponent( 9 );
			AddComponent( ac, 2, 1, 14 );
			ac = new AddonComponent( 4944 );
			AddComponent( ac, 3, 1, 5 );
			ac = new AddonComponent( 3272 );
			AddComponent( ac, -1, 1, 1 );
			ac = new AddonComponent( 3351 );
			AddComponent( ac, -2, 3, 1 );
			ac = new AddonComponent( 9 );
			AddComponent( ac, 1, 1, 1 );
			ac = new AddonComponent( 3149 );
			AddComponent( ac, 0, 3, 1 );
			ac = new AddonComponent( 3205 );
			AddComponent( ac, -3, -2, 1 );
			ac = new AddonComponent( 3272 );
			AddComponent( ac, -2, -2, 1 );
			ac = new AddonComponent( 3209 );
			AddComponent( ac, -1, 3, 1 );
			ac = new AddonComponent( 3205 );
			AddComponent( ac, 3, -1, 1 );
			ac = new AddonComponent( 9036 );
			AddComponent( ac, 3, 1, 1 );
			ac = new AddonComponent( 1475 );
			AddComponent( ac, 2, 2, 24 );
			ac = new AddonComponent( 22 );
			AddComponent( ac, 1, 1, 1 );
			ac = new AddonComponent( 4945 );
			AddComponent( ac, 3, 0, 5 );
			ac = new AddonComponent( 3149 );
			AddComponent( ac, 2, 3, 1 );
			ac = new AddonComponent( 3264 );
			AddComponent( ac, 2, 1, 1 );
			ac = new AddonComponent( 1476 );
			AddComponent( ac, 1, 1, 24 );
			ac = new AddonComponent( 22 );
			AddComponent( ac, 1, 0, 1 );
			ac = new AddonComponent( 3263 );
			AddComponent( ac, -2, 3, 1 );
			ac = new AddonComponent( 3342 );
			AddComponent( ac, 3, 2, 1 );
			ac = new AddonComponent( 9 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 3149 );
			AddComponent( ac, -1, -2, 1 );
			ac = new AddonComponent( 13425 );
			AddComponent( ac, 1, 1, 1 );
			ac = new AddonComponent( 3205 );
			AddComponent( ac, -3, -1, 1 );
			ac = new AddonComponent( 3149 );
			AddComponent( ac, -1, 1, 1 );
			ac = new AddonComponent( 9 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 3265 );
			AddComponent( ac, -2, 2, 1 );
			ac = new AddonComponent( 3223 );
			AddComponent( ac, 0, 0, 1 );
			ac = new AddonComponent( 3234 );
			AddComponent( ac, 2, 0, 1 );
			ac = new AddonComponent( 3212 );
			AddComponent( ac, 3, 3, 1 );
			ac = new AddonComponent( 3236 );
			AddComponent( ac, 1, -1, 1 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, 0, -2, 1 );
			ac = new AddonComponent( 1475 );
			AddComponent( ac, 2, 1, 24 );
			ac = new AddonComponent( 3239 );
			AddComponent( ac, 3, -2, 1 );
			ac = new AddonComponent( 3149 );
			AddComponent( ac, 3, 0, 1 );
			ac = new AddonComponent( 1476 );
			AddComponent( ac, 1, 2, 24 );
			ac = new AddonComponent( 3209 );
			AddComponent( ac, 0, 2, 1 );
			ac = new AddonComponent( 6814 );
			AddComponent( ac, 2, 1, 1 );
			ac = new AddonComponent( 21 );
			AddComponent( ac, 0, 1, 1 );
			ac = new AddonComponent( 3223 );
			AddComponent( ac, -1, 2, 1 );
			ac = new AddonComponent( 3208 );
			AddComponent( ac, 1, -2, 1 );
			ac = new AddonComponent( 3235 );
			AddComponent( ac, -1, 0, 1 );
			ac = new AddonComponent( 3205 );
			AddComponent( ac, -2, -1, 1 );
			ac = new AddonComponent( 21 );
			AddComponent( ac, 1, 1, 1 );
			ac = new AddonComponent( 3262 );
			AddComponent( ac, 2, 2, 1 );
			ac = new AddonComponent( 3212 );
			AddComponent( ac, 2, 1, 3 );
			ac = new AddonComponent( 3204 );
			AddComponent( ac, -2, 2, 1 );
			ac = new AddonComponent( 9037 );
			AddComponent( ac, 1, 2, 1 );
			ac = new AddonComponent( 4090 );
			AddComponent( ac, 1, 2, 13 );
			ac = new AddonComponent( 3272 );
			AddComponent( ac, -2, 0, 1 );

		}

		public gardenwellAddon( Serial serial ) : base( serial )
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

	public class gardenwellAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new gardenwellAddon();
			}
		}

		[Constructable]
		public gardenwellAddonDeed()
		{
			Name = "gardenwell";
		}

		public gardenwellAddonDeed( Serial serial ) : base( serial )
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