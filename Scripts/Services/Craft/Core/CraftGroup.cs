using System;
using System.Collections.Generic;

namespace Server.Engines.Craft
{
    public class CraftGroup
    {
        private readonly CraftItemCol m_arCraftItem;
        private readonly string m_NameString;
        private readonly int m_NameNumber;
        public CraftGroup(TextDefinition groupName)
        {
            if(groupName.Number > 0)
                this.m_NameNumber = groupName;
            this.m_NameString = groupName;
            this.m_arCraftItem = new CraftItemCol();
        }

        public List<CraftItem> Sorted = null;

        public List<CraftItem> GetSorted()
        {
            if (Sorted == null)
                Sorted = this.m_arCraftItem.GetSorted();
            return Sorted;
        }

        public CraftItemCol CraftItems
        {
            get
            {
             
                return this.m_arCraftItem;
            }
        }
        public string NameString
        {
            get
            {
                return this.m_NameString;
            }
        }
        public int NameNumber
        {
            get
            {
                return this.m_NameNumber;
            }
        }
        public void AddCraftItem(CraftItem craftItem)
        {
            this.m_arCraftItem.Add(craftItem);
        }
    }
}
