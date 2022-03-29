using System;
using System.Collections.Generic;
using System.Linq;
using VitaNex;

namespace Server.Engines.Craft
{
    public class CraftItemCol : System.Collections.CollectionBase
    {
        public CraftItemCol()
        {
        }

        public List<CraftItem> GetSorted()
        {
            if (Shard.DebugEnabled)
                Shard.Debug("Ordenando lista de craft");
            return this.List.CastToList<CraftItem>().OrderBy(e => { return e.NameString ?? Clilocs.GetString(ClilocLNG.ENU, e.NameNumber); }).ToList();
        }

        public int Add(CraftItem craftItem)
        {
            return this.List.Add(craftItem);
        }

        public void Remove(int index)
        {
            if (index > this.Count - 1 || index < 0)
            {
            }
            else
            {
                this.List.RemoveAt(index);
            }
        }

        public CraftItem GetAt(int index)
        {
            return (CraftItem)this.List[index];
        }


        private Dictionary<Type, CraftItem> _searchCacheSub = new Dictionary<Type, CraftItem>();

        public CraftItem SearchForSubclass(Type type)
        {
            CraftItem item;
            if (_searchCacheSub.TryGetValue(type, out item))
                return item;

            for (int i = 0; i < this.List.Count; i++)
            {
                item = (CraftItem)this.List[i];

                if (item.ItemType == type || type.IsSubclassOf(item.ItemType))
                    _searchCacheSub[type] = item;
                    return item;
            }
            return null;
        }

        private Dictionary<Type, CraftItem> _searchCache = new Dictionary<Type, CraftItem>();

        public CraftItem SearchFor(Type type)
        {
            CraftItem item;
            if (_searchCache.TryGetValue(type, out item))
                return item;

            for (int i = 0; i < this.List.Count; i++)
            {
                item = (CraftItem)this.List[i];
                if (item.ItemType == type)
                {
                    _searchCache[type] = item;
                    return item;
                }
            }
            return null;
        }
    }
}
