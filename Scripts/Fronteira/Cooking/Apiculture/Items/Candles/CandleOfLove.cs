using System;
using Server;

namespace Server.Items
{
	public class CandleOfLove : BaseLight
	{
        public override int LitItemID { get { return 0x1C14; } }
        public override int UnlitItemID { get { return 0x1C16; } }
        public static new readonly bool Burnout = true;

        [Constructable]
		public CandleOfLove() : base( 0x1C16 )
		{
			Duration = Burnout ? TimeSpan.FromMinutes( 2 ) : TimeSpan.Zero;

            Burning = false;
			Light = LightType.Circle150;
			Weight = 1.0;
			Name = "Vela do Amor";
		}

		public CandleOfLove( Serial serial ) : base( serial )
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
