using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace fronteira
{
	public class AlchemistStorage : BaseStorage
	{
		public AlchemistStorage() : base() { }
		public AlchemistStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Alchemist Storage"; } }
		private static Type[] defaultStoredTypes = new Type[]{ typeof(Bottle), typeof(PotionKeg), typeof(BasePotion)  };
		public override Type[] DefaultStoredTypes { get { return AlchemistStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new AlchemistStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return AlchemistStorage.Storage; }
		public override Dictionary<Type, int> GetStorableTypesFromItem(Item item)
		{
			Dictionary<Type, int> types = base.GetStorableTypesFromItem(item);
			if ( types == null )
				return new Dictionary<Type,int>(0);
			if (types.Count == 0)
				return types;


			PotionKeg keg = item as PotionKeg;
			if (keg != null && keg.Held > 0)
			{
				BasePotion pot = keg.FillBottle();
				types.Clear();
				types.Add(keg.GetType(), 1);
				types.Add(pot.GetType(), keg.Held);
				pot.Delete();
			}
			else
			{
				BasePotion pot = item as BasePotion;
				if (pot != null)
				{
					types.Clear();
					types.Add(pot.GetType(), pot.Amount);
				}
			}
			return types;
		}
		public override bool CanStoreItemLootType(Item item)
		{
			PotionKeg keg = item as PotionKeg;
			if (keg != null && keg.Held > 0)
				return false;
			return base.CanStoreItemLootType(item);
		}

	}
	public class AlchemistStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public AlchemistStorageDeed() : base(AlchemistStorage.Storage) { }
		public AlchemistStorageDeed(Serial serial) : base(serial) { }
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
