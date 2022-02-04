using Server;
using System;
 
namespace Server.Mobiles
{
	[CorpseName("a lizardman corpse")]
	public class LizardmanWitchdoctor : CovetousCreature
	{
		[Constructable]
		public LizardmanWitchdoctor() : base(AIType.AI_Mage)
		{
			Name = "lagarto mandingueiro";
			Body = Utility.RandomList(35, 36);
            BaseSoundID = 417;
		}
		
		[Constructable]
		public LizardmanWitchdoctor(int level, bool voidSpawn) : base(AIType.AI_Mage, level, voidSpawn)
		{
            Name = "lagarto mandingueiro";
			Body = Utility.RandomList(35, 36);
            BaseSoundID = 417;
		}
		
		public LizardmanWitchdoctor(Serial serial) : base(serial)
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


	[CorpseName("an orcish corpse")]
	public class OrcFootSoldier : CovetousCreature
	{
		[Constructable]
		public OrcFootSoldier() : base(AIType.AI_Melee)
		{
			Name = "soldado orc";
			Body = 17;
            BaseSoundID = 0x45A;
		}
		
		[Constructable]
		public OrcFootSoldier(int level, bool voidSpawn) : base(AIType.AI_Melee, level, voidSpawn)
		{
			Name = "soldado orc";
			Body = 17;
            BaseSoundID = 0x45A;
		}
		
		public OrcFootSoldier(Serial serial) : base(serial)
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
	
	[CorpseName("a ratman corpse")]
	public class RatmanAssassin : CovetousCreature
	{
		[Constructable]
		public RatmanAssassin() : base(AIType.AI_Melee)
		{
			Name = NameList.RandomName("ratman") + " o ratassassino";
			Body = 42;
            BaseSoundID = 437;
		}
		
		[Constructable]
		public RatmanAssassin(int level, bool voidSpawn) : base(AIType.AI_Melee, level, voidSpawn)
		{
            Name = NameList.RandomName("ratman") + " o ratassassino";
            Body = 42;
            BaseSoundID = 437;
		}
		
		public RatmanAssassin(Serial serial) : base(serial)
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


	[CorpseName("a titan corpse")]
	public class TitanRockHunter : CovetousCreature
	{
		[Constructable]
		public TitanRockHunter() : base(AIType.AI_Mage)
		{
			Name = "titan cacador de pedras";
			Body = 76;
            BaseSoundID = 609;
		}
		
		[Constructable]
		public TitanRockHunter(int level, bool voidSpawn) : base(AIType.AI_Mage, level, voidSpawn)
		{
			Name = "titan cacador de pedras";
			Body = 76;
            BaseSoundID = 609;
		}
		
		public TitanRockHunter(Serial serial) : base(serial)
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
