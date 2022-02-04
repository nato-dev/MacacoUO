using System;
using Server;

namespace Server.Items
{
	public class CandleLongExp : BaseLight
	{
		public override int LitItemID{ get { return 0x1430; } }
		public override int UnlitItemID{ get { return 0x1433; } }
        public static new readonly bool Burnout = true;

        [Constructable]
		public CandleLongExp() : base( 0x1433 )
		{
            Duration = Burnout ? TimeSpan.FromMinutes( 2 ) : TimeSpan.Zero;

            Burning = false;
			Light = LightType.Circle150;
			Name = "vela longa";
			Weight = 1.0;
		}

		public CandleLongExp( Serial serial ) : base( serial )
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
