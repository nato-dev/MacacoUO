using System;
using Server;

namespace Server.Items
{
	public class CandleSkullExp : BaseLight
	{
		public override int LitItemID
		{
			get
			{
				if ( ItemID == 0x1583 || ItemID == 0x1854 )
					return 0x1853;

				return 0x1858;
			}
		}

		public override int UnlitItemID
		{
			get
			{
				if ( ItemID == 0x1853 || ItemID == 0x1584 )
					return 0x1853;

				return 0x1857;
			}
		}

        public static new readonly bool Burnout = true;

        [Constructable]
		public CandleSkullExp() : base( 0x1857 )
		{
			Duration = Burnout ? TimeSpan.FromMinutes( 2 ) : TimeSpan.Zero;

            Burning = false;
			Light = LightType.Circle150;
			Weight = 5.0;
			Name = "Vela de Caveira";
		}

		public CandleSkullExp( Serial serial ) : base( serial )
		{
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
