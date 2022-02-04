using System;
using Server;

namespace fronteira
{
	public class ItemStorage : BaseStorage
	{
		public ItemStorage() : base() { }
		public ItemStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Everything"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(Item) };
		public override Type[] DefaultStoredTypes { get { return ItemStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new ItemStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return ItemStorage.Storage; }
	}
}
