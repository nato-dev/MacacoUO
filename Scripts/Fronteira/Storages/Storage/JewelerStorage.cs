using System;
using Server;
using Server.Items;

namespace fronteira
{
	public class JewelerStorage : BaseStorage
	{
		public JewelerStorage() : base() { }
		public JewelerStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Jeweler Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(Amber), typeof(Amethyst), typeof(Citrine), typeof(Diamond), typeof(Emerald), typeof(Ruby), typeof(Sapphire), typeof(StarSapphire), typeof(Tourmaline) };
		public override Type[] DefaultStoredTypes { get { return JewelerStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new JewelerStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return JewelerStorage.Storage; }
	}
	public class JewelerStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public JewelerStorageDeed() : base(JewelerStorage.Storage) { }
		public JewelerStorageDeed(Serial serial) : base(serial) { }
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
