
using System;
using Server;
using Server.Items;
using Server.Engines.Harvest;
using Server.Engines.Craft;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;

namespace fronteira
{
	public class ToolStorage : BaseStorage
	{
		public ToolStorage() : base() { }
		public ToolStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Tool Storage"; } }
        private static Type[] defaultStoredTypes = new Type[] { typeof(Blowpipe), typeof(DovetailSaw), typeof(DrawKnife), typeof(FletcherTools),
			typeof(FlourSifter), typeof(Froe), typeof(Hammer), typeof(Inshave), typeof(JointingPlane), typeof(MalletAndChisel), typeof(MapmakersPen), 
            typeof(MortarPestle),typeof(MouldingPlane),	typeof(Nails), typeof(RollingPin), typeof(Saw), typeof(Scorp), typeof(ScribesPen),
			typeof(SewingKit),	typeof(Skillet), typeof(SledgeHammer), typeof(SmithHammer), typeof(SmoothingPlane), typeof(TinkerTools), 
            typeof(Tongs), typeof(Pickaxe), typeof(GargoylesPickaxe), typeof(SturdyPickaxe), typeof(Shovel), typeof(SturdyShovel), 
            typeof(ProspectorsTool), typeof(TinkersTools)};
		public override Type[] DefaultStoredTypes { get { return ToolStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new ToolStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return ToolStorage.Storage; }

	}
	public class ToolStorageDeed : BaseStorageDeed
	{
        [Constructable]
        public ToolStorageDeed() : base(ToolStorage.Storage) { }
		public ToolStorageDeed(Serial serial) : base(serial) { }
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
