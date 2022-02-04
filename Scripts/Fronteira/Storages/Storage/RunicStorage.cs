
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace fronteira
{
#if USE_OWLTR3
	public class RunicStorage : BaseStorage
	{
		public RunicStorage() : base() { }
		public RunicStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Runic Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(BaseRunicTool) };
		public override Type[] DefaultStoredTypes { get { return RunicStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new RunicStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return RunicStorage.Storage; }

        public override bool IsTypeStorable(Type typeToCheck)
        {
            return isValid(typeToCheck) && base.IsTypeStorable(typeToCheck);
        }
        public override bool IsTypeStorable(Type typeToCheck, bool canBeEqual)
        {
            return isValid(typeToCheck) && base.IsTypeStorable(typeToCheck, canBeEqual);
        }
        private bool isValid(Type typeToCheck)
        {
            return typeToCheck != typeof(BaseRunicTool);// && typeToCheck.GetInterface("IUsesRemaining") != null; 
        } 
		public override Dictionary<Type, int> GetStorableTypesFromItem(Item item)
		{
			Dictionary<Type, int> types = base.GetStorableTypesFromItem(item);
			BaseRunicTool runic = item as BaseRunicTool;
			if (runic != null)
			{
				types.Clear();
				//types.Add(runic.GetCraftableType(), runic.UsesRemaining);
			}
			return types;
		}
	}

	public class RunicStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public RunicStorageDeed() : base(RunicStorage.Storage) { }
		public RunicStorageDeed(Serial serial) : base(serial) { }
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
#endif
}
