using Server.Engines.Craft;
using Server.Fronteira.Armory;
using System;

namespace Server.Items
{
    public class FurnitureContainer : BaseContainer, IResource
    {
        #region Old Item Serialization Vars
        /* DO NOT USE! Only used in serialization of old furniture that originally derived from BaseContainer */
        private bool m_InheritsItem;

        protected bool InheritsItem
        {
            get
            {
                return this.m_InheritsItem;
            }
        }
        #endregion

        private Mobile m_Crafter;
        private CraftResource m_Resource;
        private ItemQuality m_Quality;
        private bool m_PlayerConstructed;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get { return m_Crafter; }
            set
            {
                m_Crafter = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemQuality Quality
        {
            get { return m_Quality; }
            set { m_Quality = value; InvalidateProperties(); }
        }

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

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get { return m_PlayerConstructed; }
            set
            {
                m_PlayerConstructed = value;
                InvalidateProperties();
            }
        }

        public FurnitureContainer(int id) : base(id)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Crafter != null)
            {
                list.Add(1050043, m_Crafter.Name); // crafted by ~1_NAME~
            }

            if (Quality == ItemQuality.Exceptional)
            {
                list.Add(1060636); // Exceptional
            }

            if (m_Resource > CraftResource.Ferro)
            {
                list.Add("Feito de "+ m_Resource.ToString()); // ~1_val~
            }
        }

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            PlayerConstructed = true;

            Quality = (ItemQuality)quality;

            if (makersMark)
            {
                Crafter = from;
            }

            if (!craftItem.ForceNonExceptional)
            {
                if (typeRes == null)
                {
                    typeRes = craftItem.Resources.GetAt(0).ItemType;
                }

                Resource = CraftResources.GetFromType(typeRes);
            }

            CraftResource thisResource = CraftResources.GetFromType(typeRes);
            Shard.Debug(thisResource.ToString());
            if (thisResource == CraftResource.Eucalipto)
            {
                this.MaxItems += 50;
                if (from.Skills.Carpentry.Value >= 100)
                {
                    this.MaxItems += 50;
                }
                if (this is ArmarioBonito)
                    this.MaxItems += 100;
            }
            if(this is ArmarioBonito)
                this.MaxItems += 100;

            return quality;
        }

        public FurnitureContainer(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version

            writer.Write(m_PlayerConstructed);
            writer.Write((int)m_Resource);
            writer.Write((int)m_Quality);
            writer.Write(m_Crafter);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                case 1:
                    if (version == 1 && this is EmptyBookcase)
                    {
                        m_InheritsItem = true;
                        break;
                    }

                    m_PlayerConstructed = reader.ReadBool();
                    m_Resource = (CraftResource)reader.ReadInt();
                    m_Quality = (ItemQuality)reader.ReadInt();
                    m_Crafter = reader.ReadMobile();
                    break;
                case 0:
                    m_InheritsItem = true;
                    break;
            }
        }
    }
}
