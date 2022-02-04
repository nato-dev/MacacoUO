
using Server;
using Server.Mobiles;
using Server.ContextMenus;

namespace fronteira
{

	public class MasterStorageStorageContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageStorageContextMenu(Mobile from, Item item) : base(155, 1) //155=My Inventory
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if (backpack == null || !backpack.IsOwner(player))
				return;
			MasterStorageStorageGump.SendGump(player, backpack, 0);
		}
	}

	public class MasterStorageLedgerContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageLedgerContextMenu( Mobile from, Item item ) : base( 123, 1 ) //123=Credits
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if ( backpack == null || !backpack.IsOwner(player)  )
				return;
			MasterStorageLedgerGump.SendGump(player, backpack);
		}
	}

	public class MasterStorageSetupContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageSetupContextMenu( Mobile from, Item item ) : base( 127, 2 ) //97=Options
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if ( backpack == null || !backpack.IsOwner(player)  )
				return;
			MasterStorageSetupGump.SendGump(player, backpack, 0);
		}
	}

	public class MasterStorageFillContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageFillContextMenu( Mobile from, Item item ) : base( 6230, 3 ) //6230=refill from stock
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if ( backpack == null || !backpack.IsOwner(player)  )
				return;
			backpack.RefillStorage(player);
		}
	}
}

