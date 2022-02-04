using System;

namespace Server.Items
{
    public class CandleLongColor : BaseLight
	{
        public override int LitItemID { get { return 0x1430; } }
        public override int UnlitItemID { get { return 0x1433; } }

        public static new readonly bool Burnout = true;

        [Constructable]
		public CandleLongColor () : base( 0x1433 )
        {
            Duration = Burnout ? TimeSpan.FromMinutes(2) : TimeSpan.Zero;

            Burning = false;
			Light = LightType.Circle150;
			Weight = 1.0;
            Name = "vela longa";
            Hue = GetRandomHue();
		}

        protected static int GetRandomHue()
		{
			switch ( Utility.Random( 5 ) )
			{
				default:
				case 0: return Utility.RandomBlueHue();
				case 1: return Utility.RandomNeutralHue();
				case 2: return Utility.RandomGreenHue();
				case 3: return Utility.RandomYellowHue();
				case 4: return Utility.RandomRedHue();
			}
		}


        public CandleLongColor(Serial serial) : base(serial)
		{
		}

        public override void Serialize(GenericWriter writer)
        {
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

        public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

            int version = reader.ReadInt();
		}
	}
}
