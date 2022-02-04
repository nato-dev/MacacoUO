
using System;
using Server;
using Server.Items;

namespace fronteira
{
	public class MinerStorage : BaseStorage
	{
		public MinerStorage() : base() { }
		public MinerStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Miner Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(BaseIngot), typeof(BaseGranite), typeof(Sand) };
		public override Type[] DefaultStoredTypes { get { return MinerStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new MinerStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return MinerStorage.Storage; }
	}
	public class MinerStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public MinerStorageDeed() : base(MinerStorage.Storage) { }
		public MinerStorageDeed(Serial serial) : base(serial) { }
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
