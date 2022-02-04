
using System;
using Server;
using Server.Items;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.ContextMenus;

namespace fronteira
{
    public class BardStorage : BaseStorage
    {
        public BardStorage() : base() { }
        public BardStorage(GenericReader reader) : base(reader) { }
        public override string Name { get { return "Bard Storage"; } }
        private static Type[] defaultStoredTypes = new Type[] { typeof(BaseInstrument) };
        public override Type[] DefaultStoredTypes { get { return BardStorage.defaultStoredTypes; } }
        protected static new BaseStorage singleton;
        public static new BaseStorage Storage { get { if (singleton == null) singleton = new BardStorage(); return singleton; } }
        public override BaseStorage GetStorage() { return BardStorage.Storage; }

    }
	public class BardStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public BardStorageDeed() : base(BardStorage.Storage) { }
		public BardStorageDeed(Serial serial) : base(serial) { }
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
