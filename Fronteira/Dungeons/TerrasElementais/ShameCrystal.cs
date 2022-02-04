using Server;
using System;
using Server.Mobiles;
using Server.Engines.Points;

namespace Server.Items
{
	public class ShameCrystal : Item
	{

		[Constructable]
		public ShameCrystal() : this(1)
		{
		}
	
		[Constructable]
		public ShameCrystal(int amount) : base(16395)
		{
            Name = "Cristais Elementais";
			Stackable = true;
			Amount = amount;

            Hue = 2611;
		}
		
		public override void OnDoubleClick(Mobile from)
		{
			if(IsChildOf(from.Backpack))
			{
				PointsSystem.ShameCrystals.AwardPoints(from, this.Amount);
				Delete();
			}
		}
		
		public ShameCrystal(Serial serial) : base(serial)
		{
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
