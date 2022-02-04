
using System;
using Server;
using Server.Items;

namespace fronteira
{
	public class ScriberStorage : BaseStorage
	{
		public ScriberStorage() : base() { }
		public ScriberStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Scriber Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(SpellScroll), typeof(BlankScroll) };
		public override Type[] DefaultStoredTypes { get { return ScriberStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new ScriberStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return ScriberStorage.Storage; }
	}
	public class ScriberStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public ScriberStorageDeed() : base(ScriberStorage.Storage) { }
		public ScriberStorageDeed(Serial serial) : base(serial) { }
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
