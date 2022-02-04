using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace fronteira
{
    public class MasterStorageAddTypeTarget : Target
    {
		public enum SELECTION { ITEM_SELECTION, TYPE_SELECTION }
		private MasterStorage backpack;
		private SELECTION selection;
		private Type previousType;
		
		public MasterStorageAddTypeTarget(MasterStorage backpack, Mobile from, SELECTION selection) : this(backpack, from, selection, null) { }
		public MasterStorageAddTypeTarget(MasterStorage backpack, Mobile from, SELECTION selection, Type previousType )
			: base(18, false, TargetFlags.None)
		{
			this.backpack = backpack;
			this.previousType = previousType;
			this.selection = selection;
			if ( selection == SELECTION.ITEM_SELECTION ) 
				from.SendMessage("Please select an item to loot.");
			else if ( selection == SELECTION.TYPE_SELECTION && previousType == null ) 
				from.SendMessage("Please select the first item in order to look for common loot type.");
			else if ( selection == SELECTION.TYPE_SELECTION && previousType != null ) 
				from.SendMessage("Please select the second item in order to look for common loot type.");

		}
		
		protected override void OnTarget(Mobile from, object targeted )
		{
			if ( from as PlayerMobile == null || backpack == null || backpack.Deleted )
				return;
			if ( targeted == null )
				return;
			Type targetType = targeted.GetType();
			if ( selection == SELECTION.ITEM_SELECTION )
			{
				if ( backpack.isTypeLootable(targetType) )
					from.SendMessage("You already loot that type.");
				else 
					backpack.AddLootableItemType(targetType);
			}
			else if ( previousType != null )
			{
				if ( previousType == targetType )
				{
					from.SendMessage("Both items have the same type.");
					return;
				}
				Type common = findCommonType(targetType, previousType);
				if ( common == null )
					from.SendMessage("The selected items doesn't have a common type.");
				else
				{
					string typeName = common.ToString();
					int dotIndex = typeName.LastIndexOf(".");
					if ( dotIndex >= 0 )
						typeName = typeName.Substring(dotIndex+1);
					from.SendMessage("You successfully added " + typeName + " to your loot list.");
					backpack.AddLootableBaseType(common);
				}
			}
			else
			{
				from.Target = new MasterStorageAddTypeTarget(backpack, from as PlayerMobile, selection, targetType);
				return;
			}
			MasterStorageSetupGump.SendGump(from, backpack, -1);
		}
			
		private Type findCommonType(Type first, Type second)
		{
			while ( first != typeof(Item) && second != typeof(Item) )
			{
				if ( first == second )
					return first;
				if ( first.IsSubclassOf(second) )
					return second;
				if ( second.IsSubclassOf(first) )
					return first;
				first = first.BaseType;
				second = second.BaseType;
			}
			return null;
		}
    }
	
	public class MasterStorageAddCurrencyTarget : Target
    {
		private MasterStorage backpack;
		
		public MasterStorageAddCurrencyTarget(Mobile from, MasterStorage backpack)
			: base(18, false, TargetFlags.None)
		{
			this.backpack = backpack;
		}
		
		protected override void OnTarget(Mobile from, object targeted )
		{
			if ( !(from is PlayerMobile) )
				return;
			if ( targeted is Item && backpack.AddCurrency(targeted as Item) )
			{
				from.SendMessage("You added the currency.");
				MasterStorageLedgerGump.SendGump(from, backpack);
				from.Target = new MasterStorageAddCurrencyTarget(from, backpack);
			}
			else
				from.SendMessage("Unable to add that.");
		}
    }
}
