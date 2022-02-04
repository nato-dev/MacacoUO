
using System;
using Server;
using Server.Items;

namespace fronteira
{
	public class TailorStorage : BaseStorage
	{
		public TailorStorage() : base() { }
		public TailorStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Tailor Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(BaseLeather), typeof(BaseHides), typeof(BaseScales), typeof(Cloth), typeof(UncutCloth), typeof(BoltOfCloth), typeof(BaseClothMaterial), typeof(Cotton), typeof(Wool), typeof(Flax) };
		public override Type[] DefaultStoredTypes { get { return TailorStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new TailorStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return TailorStorage.Storage; }
	}
	public class TailorStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public TailorStorageDeed() : base(TailorStorage.Storage) { }
		public TailorStorageDeed(Serial serial) : base(serial) { }
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
