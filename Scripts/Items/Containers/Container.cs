using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Accounting;
using System.Linq;
using Server.Engines.Craft;

namespace Server.Items
{
    public abstract class BaseContainer : Container, IEngravable
    {
        public BaseContainer(int itemID)
            : base(itemID)
        {
        }

        public BaseContainer(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultMaxWeight
        {
            get
            {
                if (IsSecure)
                    return 0;
                return base.DefaultMaxWeight;
            }
        }

        private string m_EngravedText = string.Empty;

        [CommandProperty(AccessLevel.GameMaster)]
        public string EngravedText
        {
            get { return m_EngravedText; }
            set
            {
                if (value != null)
                    m_EngravedText = value;
                else
                    m_EngravedText = string.Empty;
                InvalidateProperties();
            }
        }

        public override bool IsAccessibleTo(Mobile m)
        {
            if (!BaseHouse.CheckAccessible(m, this))
                return false;

            return base.IsAccessibleTo(m);
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (IsSecure && !BaseHouse.CheckHold(m, this, item, message, checkItems, plusItems, plusWeight))
                return false;

            return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            if (IsDecoContainer && item is BaseBook)
                return true;

            return base.CheckItemUse(from, item);
        }

        public virtual bool Security { get { return true; } }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (Security)
            {
                SetSecureLevelEntry.AddTo(from, this, list);
            }
        }

        public override void GetChildContextMenuEntries(Mobile from, List<ContextMenuEntry> list, Item item)
        {
            if (IsLockedDown)
            {
                BaseHouse house = BaseHouse.FindHouseAt(this);

                if (house != null && house.IsOwner(from) && house.IsLockedDown(this) && house.IsLockedDown(item))
                {
                    list.Add(new ReleaseEntry(from, item, house));
                }
            }
            else
            {
                base.GetChildContextMenuEntries(from, list, item);
            }
        }

        public virtual void DropItemStacked(Item dropped)
        {
            List<Item> list = Items;

            ItemFlags.SetTaken(dropped, true);

            for (int i = 0; i < list.Count; ++i)
            {
                Item item = list[i];

                if (!(item is Container) && item.StackWith(null, dropped, false))
                    return;
            }

            DropItem(dropped);
        }

        public override int PesoMaximo
        {
            get
            {
                if (this is IResource)
                {
                    var mat = ((IResource)this).Resource;
                    if (mat == CraftResource.Eucalipto)
                        return base.PesoMaximo * 2;
                }
                return base.PesoMaximo;
            }
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            if (!CheckHold(from, dropped, sendFullMessage, !CheckStack(from, dropped)))
                return false;

            if (dropped.QuestItem && from.Backpack != this)
            {
                from.SendLocalizedMessage(1074769); // An item must be in your backpack (and not in a container within) to be toggled as a quest item.
                return false;
            }

            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && house.IsLockedDown(this))
            {
                if (dropped is VendorRentalContract || (dropped is Container && ((Container)dropped).FindItemByType(typeof(VendorRentalContract)) != null))
                {
                    from.SendLocalizedMessage(1062492); // You cannot place a rental contract in a locked down container.
                    return false;
                }

                if (!house.LockDown(from, dropped, false))
                    return false;
            }

            List<Item> list = Items;

            for (int i = 0; i < list.Count; ++i)
            {
                Item item = list[i];

                if (!(item is Container) && item.StackWith(from, dropped, false))
                    return true;
            }

            DropItem(dropped);

            ItemFlags.SetTaken(dropped, true);

            var honestySocket = dropped.GetSocket<HonestyItemSocket>();

            if (honestySocket != null && honestySocket.HonestyPickup == DateTime.MinValue)
            {
                honestySocket.HonestyPickup = DateTime.UtcNow;
                honestySocket.StartHonestyTimer();

                if (honestySocket.HonestyOwner == null)
                    Server.Services.Virtues.HonestyVirtue.AssignOwner(honestySocket);

                from.SendLocalizedMessage(1151536); // You have three hours to turn this item in for Honesty credit, otherwise it will cease to be a quest item.
            }

            if (Siege.SiegeShard && this != from.Backpack && from is PlayerMobile && ((PlayerMobile)from).BlessedItem != null && ((PlayerMobile)from).BlessedItem == dropped)
            {
                ((PlayerMobile)from).BlessedItem = null;
                dropped.LootType = LootType.Regular;

                from.SendLocalizedMessage(1075292, dropped.Name != null ? dropped.Name : "#" + dropped.LabelNumber.ToString()); // ~1_NAME~ has been unblessed.
            }

            if (!EnchantedHotItemSocket.CheckDrop(from, this, dropped))
                return false;

            return true;
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            if (!CheckHold(from, item, true, true))
                return false;

            if (item.QuestItem && from.Backpack != this)
            {
                from.SendLocalizedMessage(1074769); // An item must be in your backpack (and not in a container within) to be toggled as a quest item.
                return false;
            }

            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && house.IsLockedDown(this))
            {
                if (item is VendorRentalContract || (item is Container && ((Container)item).FindItemByType(typeof(VendorRentalContract)) != null))
                {
                    from.SendLocalizedMessage(1062492); // You cannot place a rental contract in a locked down container.
                    return false;
                }

                if (!house.LockDown(from, item, false))
                    return false;
            }

            item.Location = new Point3D(p.X, p.Y, 0);

            AddItem(item);

            from.SendSound(GetDroppedSound(item), GetWorldLocation());

            ItemFlags.SetTaken(item, true);

            var honestySocket = item.GetSocket<HonestyItemSocket>();

            if (honestySocket != null && honestySocket.HonestyPickup == DateTime.MinValue)
            {
                honestySocket.HonestyPickup = DateTime.UtcNow;
                honestySocket.StartHonestyTimer();

                if (honestySocket.HonestyOwner == null)
                    Server.Services.Virtues.HonestyVirtue.AssignOwner(honestySocket);

                from.SendLocalizedMessage(1151536); // You have three hours to turn this item in for Honesty credit, otherwise it will cease to be a quest item.
            }

            if (Siege.SiegeShard && this != from.Backpack && from is PlayerMobile && ((PlayerMobile)from).BlessedItem != null && ((PlayerMobile)from).BlessedItem == item)
            {
                ((PlayerMobile)from).BlessedItem = null;
                item.LootType = LootType.Regular;

                from.SendLocalizedMessage(1075292, item.Name != null ? item.Name : "#" + item.LabelNumber.ToString()); // ~1_NAME~ has been unblessed.
            }

            if (!EnchantedHotItemSocket.CheckDrop(from, this, item))
                return false;

            return true;
        }

        public override bool OnDroppedInto(Mobile from, Container target, Point3D p)
        {
            bool canDrop = base.OnDroppedInto(from, target, p);

            if (canDrop && target is BankBox)
            {
                CheckBank((BankBox)target, from);
            }

            return canDrop;
        }

        public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            base.UpdateTotal(sender, type, delta);

            if (type == TotalType.Weight && RootParent is Mobile)
                ((Mobile)RootParent).InvalidateProperties();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.IsStaff() || RootParent is PlayerVendor ||
                (from.InRange(GetWorldLocation(), 2) && (Parent != null || (Z >= from.Z - 8 && Z <= from.Z + 16))))
            {
                Open(from);
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);

            if (!String.IsNullOrEmpty(EngravedText))
            {
                list.Add(1072305, Utility.FixHtml(EngravedText)); // Engraved: ~1_INSCRIPTION~
            }
        }

        public override bool DropToWorld(Mobile m, Point3D p)
        {
            Server.Engines.Despise.WispOrb.CheckDrop(this, m);

            return base.DropToWorld(m, p);
        }

        public virtual void Open(Mobile from)
        {
            DisplayTo(from);
        }

        public void CheckBank(BankBox bank, Mobile from)
        {
            if (AccountGold.Enabled && bank.Owner == from && from.Account != null)
            {
                List<BankCheck> checks = new List<BankCheck>(Items.OfType<BankCheck>());

                foreach (BankCheck check in checks)
                {
                    if (from.Account.DepositGold(check.Worth))
                    {
                        from.SendLocalizedMessage(1042672, true, check.Worth.ToString("#,0"));
                        check.Delete();
                    }
                    else
                    {
                        from.AddToBackpack(check);
                    }
                }

                checks.Clear();
                checks.TrimExcess();

                UpdateTotals();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1000); // Version
            writer.Write(m_EngravedText);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.PeekInt();
            switch (version)
            {
                case 1000:
                    reader.ReadInt();
                    m_EngravedText = reader.ReadString();
                    break;
            }
        }
    }

    public class CreatureBackpack : Backpack	//Used on BaseCreature
    {
        [Constructable]
        public CreatureBackpack(string name)
        {
            Name = name;
            Layer = Layer.Backpack;
            Hue = 5;
            Weight = 3.0;
        }

        public CreatureBackpack(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (Name != null)
                list.Add(1075257, Name); // Contents of ~1_PETNAME~'s pack.
            else
                base.AddNameProperty(list);
        }

        public override void OnItemRemoved(Item item)
        {
            if (Items.Count == 0)
                Delete();

            base.OnItemRemoved(item);
        }

        public override bool OnDragLift(Mobile from)
        {
            if (from.IsPlayer())
                return true;

            from.SendLocalizedMessage(500169); // You cannot pick that up.
            return false;
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            return false;
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Weight = 13.0;
        }
    }

    public class StrongBackpack : Backpack	//Used on Pack animals
    {
        [Constructable]
        public StrongBackpack()
        {
            Layer = Layer.Backpack;
            Weight = 13.0;
        }

        public StrongBackpack(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultMaxWeight
        {
            get
            {
                return 1600;
            }
        }
        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            return base.CheckHold(m, item, false, checkItems, plusItems, plusWeight);
        }

        public override bool CheckContentDisplay(Mobile from)
        {
            object root = RootParent;

            if (root is BaseCreature && ((BaseCreature)root).Controlled && ((BaseCreature)root).ControlMaster == from)
                return true;

            return base.CheckContentDisplay(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Weight = 13.0;
        }
    }

    public class Backpack : BaseContainer, IDyable
    {
        [Constructable]
        public Backpack()
            : base(0xE75)
        {
            Layer = Layer.Backpack;
            Weight = 3.0;
        }

        public Backpack(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultMaxWeight
        {
            get
            {

                var m = ParentEntity as PlayerMobile;
                if (m != null && m.Player && m.Backpack == this)
                {
                    return m.MaxWeight;

                }
                else
                {
                    return base.DefaultMaxWeight;
                }
            }
        }
        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && ItemID == 0x9B2)
                ItemID = 0xE75;
        }
    }

    public class TrappedPouch : TrapableContainer
    {
        [Constructable]
        public TrappedPouch()
            : base(0xE79)
        {
            this.TrapLevel = 0;
            this.TrapPower = 1;
            this.Hue = 38;
            this.Name = "Bauzinho Explosivo";
            this.TrapType = TrapType.MagicTrap;
            Weight = 1.0;
        }

        public TrappedPouch(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Pouch : TrapableContainer
    {
        [Constructable]
        public Pouch()
            : base(0xE79)
        {
            Weight = 1.0;
        }

        public Pouch(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public abstract class BaseBagBall : BaseContainer, IDyable
    {
        public BaseBagBall(int itemID)
            : base(itemID)
        {
            Weight = 1.0;
        }

        public BaseBagBall(Serial serial)
            : base(serial)
        {
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmallBagBall : BaseBagBall
    {
        [Constructable]
        public SmallBagBall()
            : base(0x2256)
        {
        }

        public SmallBagBall(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LargeBagBall : BaseBagBall
    {
        [Constructable]
        public LargeBagBall()
            : base(0x2257)
        {
        }

        public LargeBagBall(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Bag : BaseContainer, IDyable
    {
        [Constructable]
        public Bag()
            : base(0xE76)
        {
            Weight = 2.0;
        }

        public Bag(Serial serial)
            : base(serial)
        {
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Barrel : BaseContainer
    {
        [Constructable]
        public Barrel()
            : base(0xE77)
        {
            Weight = 25.0;
        }

        public void Pour(Mobile from, BaseBeverage beverage)
        {
            if (beverage.Content == BeverageType.Water)
            {
                if (Items.Count > 0)
                {
                    from.SendLocalizedMessage(500848); // Couldn't pour it there.  It was already full.
                    beverage.PrivateOverheadMessage(Server.Network.MessageType.Regular, 0, 500841, from.NetState); // that has somethign in it.
                }
                else
                {
                    var barrel = new WaterBarrel();
                    barrel.Movable = false;
                    barrel.MoveToWorld(Location, Map);

                    beverage.Pour_OnTarget(from, barrel);
                    Delete();
                }
            }
        }

        public Barrel(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Keg : BaseContainer, IResource
    {
        [Constructable]
        public Keg()
            : base(0xE7F)
        {
            Weight = 15.0;
            this.Name = "Barril de Pocoes";
        }

        public Keg(Serial serial)
            : base(serial)
        {
        }

        private CraftResource m_Resource;
        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set
            {
                m_Resource = value;
                Hue = CraftResources.GetHue(m_Resource);
                InvalidateProperties();
            }
        }

        public ItemQuality Quality { get { return ItemQuality.Normal; } set { } }

        public bool PlayerConstructed { get { return false; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write((int)Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            if (version >= 1)
            {
                m_Resource = (CraftResource)reader.ReadInt();
            }
        }

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            if (typeRes != null)
            {
                CraftResource thisResource = CraftResources.GetFromType(typeRes);
                Shard.Debug(thisResource.ToString());
                if (thisResource == CraftResource.Eucalipto && this.GetType().Name.Contains("Chest") || this.GetType().Name.Contains("Armario"))
                {
                    this.MaxItems += 100;

                    if (this.Quality == ItemQuality.Exceptional)
                        this.MaxItems += 50;

                    if (from.Skills.Carpentry.Value >= 100)
                    {
                        this.MaxItems += 100;
                    }
                }
            }
            return quality;
        }
    }

    public class KegGrande : BaseContainer, IResource
    {
        [Constructable]
        public KegGrande()
            : base(0xE7F)
        {
            Weight = 15.0;
            this.Name = "Barril de Pocoes Gigante";
        }

        public KegGrande(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Usado por Tinker");
        }

        private CraftResource m_Resource;
        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set
            {
                m_Resource = value;
                Hue = CraftResources.GetHue(m_Resource);
                InvalidateProperties();
            }
        }

        public ItemQuality Quality { get { return ItemQuality.Normal; } set { } }

        public bool PlayerConstructed { get { return false; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write((int)Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            if (version >= 1)
            {
                m_Resource = (CraftResource)reader.ReadInt();
            }
        }

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            if (typeRes != null)
            {
                CraftResource thisResource = CraftResources.GetFromType(typeRes);
                Shard.Debug(thisResource.ToString());
                if (thisResource == CraftResource.Eucalipto && this.GetType().Name.Contains("Chest") || this.GetType().Name.Contains("Armario"))
                {
                    this.MaxItems += 100;

                    if (this.Quality == ItemQuality.Exceptional)
                        this.MaxItems += 50;

                    if (from.Skills.Carpentry.Value >= 100)
                    {
                        this.MaxItems += 100;
                    }
                }
            }
            return quality;
        }
    }

    public class PicnicBasket : BaseContainer
    {
        [Constructable]
        public PicnicBasket()
            : base(0xE7A)
        {
            Weight = 2.0; // Stratics doesn't know weight
        }

        public PicnicBasket(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Basket : BaseContainer
    {
        [Constructable]
        public Basket()
            : base(0x990)
        {
            Weight = 1.0; // Stratics doesn't know weight
        }

        public Basket(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x9AA, 0xE7D)]
    public class WoodenBox : LockableContainer
    {
        [Constructable]
        public WoodenBox()
            : base(0x9AA)
        {
            Weight = 4.0;
        }

        public WoodenBox(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x9A9, 0xE7E)]
    public class SmallCrate : LockableContainer
    {
        [Constructable]
        public SmallCrate()
            : base(0x9A9)
        {
            Weight = 2.0;
        }

        public SmallCrate(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xE3F, 0xE3E)]
    public class MediumCrate : LockableContainer
    {
        [Constructable]
        public MediumCrate()
            : base(0xE3F)
        {
            Weight = 2.0;
        }

        public MediumCrate(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xE3D, 0xE3C)]
    public class LargeCrate : LockableContainer
    {
        [Constructable]
        public LargeCrate()
            : base(0xE3D)
        {
            Weight = 1.0;
        }

        public LargeCrate(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [DynamicFliping]
    [Flipable(0x9A8, 0xE80)]
    public class MetalBox : LockableContainer
    {
        [Constructable]
        public MetalBox()
            : base(0x9A8)
        {
        }

        public MetalBox(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight { get { return 5; } }
        public override int LabelNumber { get { return 1022472; } } // metal box

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [DynamicFliping]
    [Flipable(0x9AB, 0xE7C)]
    public class MetalChest : LockableContainer
    {
        [Constructable]
        public MetalChest()
            : base(0x9AB)
        {
        }

        public MetalChest(Serial serial)
            : base(serial)
        {
            Name = "Bau de Metal";
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SOSChest : LockableContainer, IFlipable
    {
        public override int LabelNumber
        {
            get
            {
                if (ItemID >= 0xA306)
                {
                    return 1015097; // Chest
                }

                return base.LabelNumber;
            }
        }

        [Constructable]
        public SOSChest(int id)
            : base(id)
        {
        }

        public override int PesoMaximo { get { return DefaultMaxWeight * 4; } }

        public void OnFlip(Mobile m)
        {
            switch (ItemID)
            {
                case 0xE40: ItemID++; break;
                case 0xE41: ItemID--; break;
                case 0xE42: ItemID++; break;
                case 0xE43: ItemID--; break;
                case 0xA306: ItemID++; break;
                case 0xA307: ItemID--; break;
                case 0xA308: ItemID++; break;
                case 0xA309: ItemID--; break;
                case 0xA30A: ItemID++; break;
                case 0xA30B: ItemID--; break;
            }
        }

        public SOSChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }

    [DynamicFliping]
    [Flipable(0xE41, 0xE40)]
    public class MetalGoldenChest : LockableContainer
    {
        [Constructable]
        public MetalGoldenChest()
            : base(0xE41)
        {
        }

        public MetalGoldenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xe43, 0xe42)]
    public class WoodenChest : LockableContainer
    {
        [Constructable]
        public WoodenChest()
            : base(0xe43)
        {
            Weight = 2.0;
        }

        public WoodenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x280B, 0x280C)]
    public class PlainWoodenChest : LockableContainer
    {
        [Constructable]
        public PlainWoodenChest()
            : base(0x280B)
        {
        }

        public PlainWoodenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x280D, 0x280E)]
    public class OrnateWoodenChest : LockableContainer
    {
        [Constructable]
        public OrnateWoodenChest()
            : base(0x280D)
        {
        }

        public OrnateWoodenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x280F, 0x2810)]
    public class GildedWoodenChest : LockableContainer
    {
        [Constructable]
        public GildedWoodenChest()
            : base(0x280F)
        {
        }

        public GildedWoodenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x2811, 0x2812)]
    public class WoodenFootLocker : LockableContainer
    {
        [Constructable]
        public WoodenFootLocker()
            : base(0x2811)
        {
            GumpID = 0x10B;
        }

        public WoodenFootLocker(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 15)
                Weight = -1;

            if (version < 2)
                GumpID = 0x10B;
        }
    }

    [Furniture]
    [Flipable(0x2813, 0x2814)]
    public class FinishedWoodenChest : LockableContainer
    {
        [Constructable]
        public FinishedWoodenChest()
            : base(0x2813)
        {
        }

        public FinishedWoodenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [FlipableAttribute(0x4026, 0x4025)]
    public class GargishChest : LockableContainer
    {
        [Constructable]
        public GargishChest()
            : base(0x4026)
        {
            Weight = 1.0;
        }

        public GargishChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [FlipableAttribute(0xA99, 0xA97)]
    public class AcademicBookCase : BaseContainer
    {
        public override int LabelNumber { get { return 1071213; } } // academic bookcase
        public override int DefaultGumpID { get { return 0x4D; } }

        [Constructable]
        public AcademicBookCase()
            : base(0xA99)
        {
            Weight = 11.0;
        }

        public AcademicBookCase(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [FlipableAttribute(0xA0DB, 0xA0DC)]
    public class EnchantedPicnicBasket : BaseContainer
    {
        public override int LabelNumber { get { return 1158333; } } // enchanted picnic basket

        public override int DefaultGumpID { get { return 0x108; } }

        [Constructable]
        public EnchantedPicnicBasket()
            : base(0xA0DB)
        {
            DropItem(new PicnicBlanketDeed());
            DropItem(new Hamburger(3));
            DropItem(new Sausage(3));
            DropItem(new HotDog(3));
        }

        public EnchantedPicnicBasket(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
