using System;
using Server.Engines.Craft;
using Server.Gumps;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
    [Flipable(0x14F0, 0x14EF)]
    public abstract class BaseAddonDeed : Item, ICraftable
    {
        private CraftResource m_Resource;
        private bool m_ReDeed;

        public BaseAddonDeed()
            : base(0x14F0)
        {
            Weight = 1.0;
        }

        public BaseAddonDeed(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Addon para Casas");
        }

        public abstract BaseAddon Addon { get; }

        public virtual bool UseCraftResource { get { return true; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get
            {
                return m_Resource;
            }
            set
            {
                if (UseCraftResource && m_Resource != value)
                {
                    m_Resource = value;
                    Hue = CraftResources.GetHue(m_Resource);

                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsReDeed
        {
            get { return m_ReDeed; }
            set 
            {
                m_ReDeed = value;

                if (UseCraftResource)
                {
                    if (m_ReDeed && ItemID == 0x14F0)
                    {
                        ItemID = 0x14EF;
                    }
                    else if (!m_ReDeed && ItemID == 0x14EF)
                    {
                        ItemID = 0x14F0;
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            // Version 2
            writer.Write(m_ReDeed);

            // Version 1
            writer.Write((int)m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_ReDeed = reader.ReadBool();
                        goto case 1;
                    }
                case 1:
                    {
                        m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
            }

            if (version == 1 && UseCraftResource && Hue == 0 && m_Resource != CraftResource.None)
            {
                Hue = CraftResources.GetHue(m_Resource);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
                from.Target = new InternalTarget(this);
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        public virtual void DeleteDeed()
        {
            Delete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (!CraftResources.IsStandard(m_Resource))
                list.Add(CraftResources.GetLocalizationNumber(m_Resource));
        }

        public virtual int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            Resource = CraftResources.GetFromType(resourceType);

            CraftContext context = craftSystem.GetContext(from);

            if (context != null && context.DoNotColor)
                Hue = 0;
            else if (Hue == 0)
                Hue = resHue;

            return quality;
        }

        private class InternalTarget : Target
        {
            private readonly BaseAddonDeed m_Deed;
            public InternalTarget(BaseAddonDeed deed)
                : base(-1, true, TargetFlags.None)
            {
                m_Deed = deed;

                CheckLOS = false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                IPoint3D p = targeted as IPoint3D;
                Map map = from.Map;

                if (p == null || map == null || m_Deed.Deleted)
                    return;

                if (m_Deed.IsChildOf(from.Backpack))
                {
                    BaseAddon addon = m_Deed.Addon;

                    Server.Spells.SpellHelper.GetSurfaceTop(ref p);

                    BaseHouse house = null;
                    BaseGalleon boat = null;

                    AddonFitResult res = addon.CouldFit(p, map, from, ref house, ref boat);

                    if (res == AddonFitResult.Valid)
                    {

                        addon.Resource = m_Deed.Resource;

                        if (addon.RetainDeedHue || (m_Deed.Hue != 0 && CraftResources.GetHue(m_Deed.Resource) != m_Deed.Hue))
                            addon.Hue = m_Deed.Hue;

                        addon.MoveToWorld(new Point3D(p), map);

                        if (house != null)
                            house.Addons[addon] = from;
                        else if (boat != null)
                            boat.AddAddon(addon);

                        m_Deed.DeleteDeed();

                        from.SendGump(new GumpOpcoes("Manter Addon ?", (opt) =>
                        {
                            if(opt==1 && from.Alive)
                            {
                                if (!addon.Deleted)
                                {
                                    from.Backpack.DropItem(addon.GetDeed());
                                    addon.Delete();
                                } else
                                {
                                    from.SendMessage("A sagacidade humana nao tem limites");
                                }
                            }

                        }, 0x14F0, 0, "Manter", "Remover"));
      
                    }
                    else if (res == AddonFitResult.Blocked)
                        from.SendLocalizedMessage("Voce nao pode construir ali pois existe algo ali"); // You cannot build that there.
                    else if (res == AddonFitResult.NotInHouse)
                        from.SendLocalizedMessage("Voce so pode colocar isto em sua casa"); // You can only place this in a house that you own!
                    else if (res == AddonFitResult.DoorTooClose)
                        from.SendLocalizedMessage("Muito perto da porta"); // You cannot build near the door.
                    else if (res == AddonFitResult.NoWall)
                        from.SendLocalizedMessage("O objeto precisa estar na parede"); // This object needs to be mounted on something.
					
                    if (res != AddonFitResult.Valid)
                    {
                        addon.Delete();
                    } else
                    {
                        if(!from.IsCooldown("dicamaax"))
                        {
                            from.SetCooldown("dicamaax");
                            from.SendGump(new GumpFala((int n) => { }, Faces.GM_PRETO, "Para retirar addons de sua casa, use um MACHADO neles !"));
                        }
                        from.SendMessage(78, "Para voltar o addon de sua casa a ser uma escritura, use um machado");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
            }
        }
    }
}
