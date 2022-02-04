using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Points
{
	public class ShameCrystals : PointsSystem
	{
		public override PointsType Loyalty { get { return PointsType.ShameCrystals; } }
		public override TextDefinition Name { get { return m_Name; } }
		public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
		
		private TextDefinition m_Name = new TextDefinition("Elementais");
		
		public ShameCrystals()
		{
		}
		
		public override void SendMessage(PlayerMobile from, double old, double points, bool quest)
		{
            from.SendLocalizedMessage(String.Format("Voce ganhou {0} pontos elementais. Total: \t{1}", ((int)points).ToString(), ((int)old + points).ToString())); // You gain ~1_AMT~ dungeon points for ~2_NAME~. Your total is now ~3_TOTAL~.
		}
		
		public override TextDefinition GetTitle(PlayerMobile from)
		{
            return new TextDefinition("Elementalista");
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            if (Version >= 2)
            {
                int version = reader.ReadInt();

                // all deserialize code in here
            }
        }
	}
}
