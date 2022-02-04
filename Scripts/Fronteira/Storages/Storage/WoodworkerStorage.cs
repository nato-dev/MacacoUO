
using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.Craft;

namespace fronteira
{
	public class WoodworkerStorage : BaseStorage
	{
		public WoodworkerStorage() : base() { }
		public WoodworkerStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Woodworker Storage"; } }

		private static Type[] defaultStoredTypes = new Type[] 
		{ 
			typeof(Arrow), typeof(Bolt), typeof(Feather), typeof(Shaft),
			typeof(FrostwoodBoard),		typeof(FrostwoodLog),
			typeof(HeartwoodBoard),		typeof(HeartwoodLog),
			typeof(BloodwoodBoard),		typeof(BloodwoodLog),
			typeof(YewBoard),			typeof(YewLog),
			typeof(AshBoard),			typeof(AshLog),
			typeof(OakBoard),			typeof(OakLog),
			//NOTE: Make sure Board and Log are the LAST on the list when all the other wood types appear before them!!!
			typeof(Board), typeof(Log)
		};

		public override Type[] DefaultStoredTypes { get { return WoodworkerStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton;
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new WoodworkerStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return WoodworkerStorage.Storage; }

	}
	public class WoodworkerStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public WoodworkerStorageDeed() : base(WoodworkerStorage.Storage) { }
		public WoodworkerStorageDeed(Serial serial) : base(serial) { }
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
