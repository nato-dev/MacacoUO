using System;
using Server;

namespace Server.Items
{
	public class CandleShortExp : BaseLight
	{
		public override int LitItemID{ get { return 0x142C; } }
		public override int UnlitItemID{ get { return 0x142F; } }

        public static new readonly bool Burnout = true;

        [Constructable]
		public CandleShortExp() : base( 0x142F )
		{
			Duration = Burnout ? TimeSpan.FromMinutes( 2 ) : TimeSpan.Zero;

            Burning = false;
			Light = LightType.Circle150;
			Weight = 1.0;
			Name = "vela curta";
		}

		public CandleShortExp( Serial serial ) : base( serial )
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
