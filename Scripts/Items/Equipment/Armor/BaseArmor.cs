using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Factions;
using Server.Network;
using AMA = Server.Items.ArmorMeditationAllowance;
using AMT = Server.Items.ArmorMaterialType;
using System.Linq;
using Server.Gumps;
using Server.Mobiles;
using Server.Fronteira.Talentos;

namespace Server.Items
{
    public abstract class BaseArmor : Item, IScissorable, IFactionItem, ICraftable, IWearableDurability, IResource, ISetItem, IVvVItem, IOwnerRestricted, ITalismanProtection, IEngravable, IArtifact, ICombatEquipment
    {
        #region Factions
        private FactionItem m_FactionState;

        public FactionItem FactionItemState
        {
            get
            {
                return m_FactionState;
            }
            set
            {
                m_FactionState = value;

                if (m_FactionState == null)
                    Hue = CraftResources.GetHue(Resource);

                LootType = (m_FactionState == null ? LootType.Regular : LootType.Blessed);
            }
        }
        #endregion

        #region IEngravable
        private string _EngravedText;

        [CommandProperty(AccessLevel.GameMaster)]
        public string EngravedText { get { return _EngravedText; } set { _EngravedText = value; InvalidateProperties(); } }
        #endregion

        private bool _VvVItem;
        private Mobile _Owner;
        private string _OwnerName;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsVvVItem
        {
            get { return _VvVItem; }
            set { _VvVItem = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return _Owner; }
            set { _Owner = value; if (_Owner != null) _OwnerName = _Owner.Name; InvalidateProperties(); }
        }

        public virtual string OwnerName
        {
            get { return _OwnerName; }
            set { _OwnerName = value; InvalidateProperties(); }
        }

        /* Armor internals work differently now (Jun 19 2003)
        * 
        * The attributes defined below default to -1.
        * If the value is -1, the corresponding virtual 'Aos/Old' property is used.
        * If not, the attribute value itself is used. Here's the list:
        *  - ArmorBase
        *  - StrBonus
        *  - DexBonus
        *  - IntBonus
        *  - StrReq
        *  - DexReq
        *  - IntReq
        *  - MeditationAllowance
        */

        // Instance values. These values must are unique to each armor piece.
        private int m_MaxHitPoints;
        private int m_HitPoints;
        private Mobile m_Crafter;
        private ItemQuality m_Quality;
        private ArmorDurabilityLevel m_Durability;
        private ArmorProtectionLevel m_Protection;
        private CraftResource m_Resource;
        private bool m_Identified, m_PlayerConstructed;
        private int m_PhysicalBonus, m_FireBonus, m_ColdBonus, m_PoisonBonus, m_EnergyBonus;

        #region Runic Reforging
        private ElementoPvM m_elementoPvm;
        private ReforgedPrefix m_ReforgedPrefix;
        private ReforgedSuffix m_ReforgedSuffix;
        #endregion

        #region Stygian Abyss
        private int m_GorgonLenseCharges;
        private LenseType m_GorgonLenseType;

        private bool m_Altered;

        private int m_TimesImbued;
        private bool m_IsImbued;
        private int m_PhysNonImbuing;
        private int m_FireNonImbuing;
        private int m_ColdNonImbuing;
        private int m_PoisonNonImbuing;
        private int m_EnergyNonImbuing;
        #endregion

        private AosAttributes m_AosAttributes;
        private AosArmorAttributes m_AosArmorAttributes;
        private AosSkillBonuses m_AosSkillBonuses;
        private SAAbsorptionAttributes m_SAAbsorptionAttributes;
        private NegativeAttributes m_NegativeAttributes;

        private TalismanAttribute m_TalismanProtection;

        // Overridable values. These values are provided to override the defaults which get defined in the individual armor scripts.
        private int m_ArmorBase = -1;
        private int m_StrBonus = -1, m_DexBonus = -1, m_IntBonus = -1;
        private int m_StrReq = -1, m_DexReq = -1, m_IntReq = -1;
        private AMA m_Meditate = (AMA)(-1);

        public virtual bool AllowMaleWearer
        {
            get
            {
                return true;
            }
        }
        public virtual bool AllowFemaleWearer
        {
            get
            {
                return true;
            }
        }

        public abstract AMT MaterialType { get; }

        public virtual int RevertArmorBase
        {
            get
            {
                return ArmorBase;
            }
        }
        public virtual int ArmorBase
        {
            get
            {
                return 0;
            }
        }

        public virtual AMA DefMedAllowance
        {
            get
            {
                return AMA.None;
            }
        }
        public virtual AMA AosMedAllowance
        {
            get
            {
                return DefMedAllowance;
            }
        }
        public virtual AMA OldMedAllowance
        {
            get
            {
                return DefMedAllowance;
            }
        }

        public virtual int AosStrBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosDexBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosIntBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosStrReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosDexReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosIntReq
        {
            get
            {
                return 0;
            }
        }

        public virtual int OldStrBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldDexBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldIntBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldStrReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldDexReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldIntReq
        {
            get
            {
                return 0;
            }
        }

        public virtual bool CanFortify
        {
            get
            {
                return !IsImbued && NegativeAttributes.Antique < 4;
            }
        }

        public virtual bool CanRepair
        {
            get
            {
                return m_NegativeAttributes.NoRepair == 0;
            }
        }

        public virtual bool CanAlter { get { return true; } }

        public virtual bool UseIntOrDexProperty
        {
            get
            {
                return false;
            }
        }
        public virtual int IntOrDexPropertyValue
        {
            get
            {
                return 0;
            }
        }

        public override void OnAfterDuped(Item newItem)
        {
            BaseArmor armor = newItem as BaseArmor;

            if (armor == null)
                return;

            armor.m_AosAttributes = new AosAttributes(newItem, m_AosAttributes);
            armor.m_AosArmorAttributes = new AosArmorAttributes(newItem, m_AosArmorAttributes);
            armor.m_AosSkillBonuses = new AosSkillBonuses(newItem, m_AosSkillBonuses);
            armor.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(newItem, m_SAAbsorptionAttributes);
            armor.m_NegativeAttributes = new NegativeAttributes(newItem, m_NegativeAttributes);
            armor.m_TalismanProtection = new TalismanAttribute(m_TalismanProtection);

            armor.m_SetAttributes = new AosAttributes(newItem, m_SetAttributes);
            armor.m_SetSkillBonuses = new AosSkillBonuses(newItem, m_SetSkillBonuses);

            base.OnAfterDuped(newItem);
        }

        #region Personal Bless Deed
        private Mobile m_BlessedBy;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BlessedBy
        {
            get
            {
                return m_BlessedBy;
            }
            set
            {
                m_BlessedBy = value;
                InvalidateProperties();
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (BlessedFor == from && BlessedBy == from && RootParent == from)
            {
                list.Add(new UnBlessEntry(from, this));
            }
        }

        private class UnBlessEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly BaseArmor m_Item;

            public UnBlessEntry(Mobile from, BaseArmor item)
                : base(6208, -1)
            {
                m_From = from;
                m_Item = item;
            }

            public override void OnClick()
            {
                m_Item.BlessedFor = null;
                m_Item.BlessedBy = null;

                Container pack = m_From.Backpack;

                if (pack != null)
                {
                    pack.DropItem(new PersonalBlessDeed(m_From));
                    m_From.SendLocalizedMessage(1062200); // A personal bless deed has been placed in your backpack.
                }
            }
        }
        #endregion

        [CommandProperty(AccessLevel.GameMaster)]
        public AMA MeditationAllowance
        {
            get
            {
                return (m_Meditate == (AMA)(-1) ? Core.AOS ? AosMedAllowance : OldMedAllowance : m_Meditate);
            }
            set
            {
                m_Meditate = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BaseArmorRating
        {
            get
            {
                if (m_ArmorBase == -1)
                    return ArmorBase;
                else
                    return m_ArmorBase;
            }
            set
            {
                m_ArmorBase = value;
                Invalidate();
            }
        }

        public double BaseArmorRatingScaled
        {
            get
            {
                return (BaseArmorRating * ArmorScalar);
            }
        }

        public virtual double ArmorRating
        {
            get
            {
                int ar = BaseArmorRating;

                if (m_Protection != ArmorProtectionLevel.Regular) {
                    if(this is BaseShield)
                        ar += 1 + (int)m_Protection;
                    else
                        ar += 1 + (2 * (int)m_Protection);
                }
                   
                switch (m_Resource)
                {
                    case CraftResource.Eucalipto:
                    case CraftResource.Cedro:
                    case CraftResource.Carvalho:
                        ar += 1;
                        break;
                    case CraftResource.Carmesim:
                    case CraftResource.Mogno:
                        ar += 3;
                        break;
                    case CraftResource.Gelo:
                        ar += 5;
                        break;

                    case CraftResource.Cobre:
                        ar += 8;
                        break;
                    case CraftResource.Bronze:
                        ar += 9;
                        break;
                    case CraftResource.Dourado:
                        ar += 14;
                        break;
                    case CraftResource.Adamantium:
                    case CraftResource.Niobio:
                        ar += 12;
                        break;
                    case CraftResource.Lazurita:
                        ar += 12;
                        break;
                    case CraftResource.Quartzo:
                        ar += 16;
                        break;
                    case CraftResource.Berilo:
                        ar += 10;
                        break;
                    case CraftResource.Vibranium:
                        ar += 18;
                        break;

                    case CraftResource.EscamasNegras:
                        ar += 15;
                        break;
                    case CraftResource.EscamasBrancas:
                        ar += 13;
                        break;
                    case CraftResource.EscamasAmarelas:
                        ar += 10;
                        break;
                    case CraftResource.EscamasAzuis:
                        ar += 8;
                        break;
                    case CraftResource.EscamasVermelhas:
                        ar += 5;
                        break;
                    case CraftResource.EscamasVerdes:
                        ar += 3;
                        break;

                    case CraftResource.CouroSpinned:
                        ar += 7;
                        break;
                    case CraftResource.CouroHorned:
                        ar += 12;
                        break;
                    case CraftResource.CouroBarbed:
                        ar += 18;
                        break;
                }
                ar += -4 + (4 * (int)m_Quality);
                return ScaleArmorByDurability(ar);
            }
        }

        public double ArmorRatingScaled
        {
            get
            {
                return (ArmorRating * ArmorScalar);
            }
        }

        #region Publish 81 Armor Refinement
        private int m_RefinedPhysical;
        private int m_RefinedFire;
        private int m_RefinedCold;
        private int m_RefinedPoison;
        private int m_RefinedEnergy;

        [CommandProperty(AccessLevel.GameMaster)]
        public int RefinedPhysical { get { return m_RefinedPhysical; } set { m_RefinedPhysical = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RefinedFire { get { return m_RefinedFire; } set { m_RefinedFire = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RefinedCold { get { return m_RefinedCold; } set { m_RefinedCold = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RefinedPoison { get { return m_RefinedPoison; } set { m_RefinedPoison = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RefinedEnergy { get { return m_RefinedEnergy; } set { m_RefinedEnergy = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RefinedDefenseChance { get { return -(m_RefinedPhysical + m_RefinedFire + m_RefinedCold + m_RefinedPoison + m_RefinedEnergy); } }

        public static int GetRefinedResist(Mobile from, ResistanceType attr)
        {
            int value = 0;

            foreach (var armor in from.Items.OfType<BaseArmor>())
            {
                switch (attr)
                {
                    case ResistanceType.Physical: value += armor.m_RefinedPhysical; break;
                    case ResistanceType.Fire: value += armor.m_RefinedFire; break;
                    case ResistanceType.Cold: value += armor.m_RefinedCold; break;
                    case ResistanceType.Poison: value += armor.m_RefinedPoison; break;
                    case ResistanceType.Energy: value += armor.m_RefinedEnergy; break;
                }
            }

            return value;
        }

        public static int GetRefinedDefenseChance(Mobile from)
        {
            int value = 0;

            foreach (var armor in from.Items.OfType<BaseArmor>())
            {
                value += armor.RefinedDefenseChance;
            }

            return value;
        }

        public static bool HasRefinedResist(Mobile from)
        {
            return from.Items.OfType<BaseArmor>().Any(armor => armor.m_RefinedPhysical > 0 ||
                                                               armor.m_RefinedFire > 0 ||
                                                               armor.m_RefinedCold > 0 ||
                                                               armor.m_RefinedPoison > 0 ||
                                                               armor.m_RefinedEnergy > 0);
        }

        public override void AddResistanceProperties(ObjectPropertyList list)
        {
            /*
            if (ColdResistance != 0)
            {
                list.AddTwoValues("DEBUG: RESIST FRIO", "+" + ColdResistance.ToString());
            }
            */
            /*
            if(ColdResistance != 0)
            {
                list.AddTwoValues("Resistencia a Frio", "+" + ColdResistance.ToString());
            }
            */
            /*
            if (PhysicalResistance != 0 || m_RefinedPhysical != 0)
            {
                if (m_RefinedPhysical != 0)
                    list.Add(1153735, String.Format("{0}\t{1}\t{2}", PhysicalResistance.ToString(), "", m_RefinedPhysical.ToString()));// physical resist ~1_val~% / ~2_symb~~3_val~% Max
                else
                    list.Add(1060448, PhysicalResistance.ToString()); // physical resist ~1_val~%
            }

            if (FireResistance != 0 || m_RefinedFire != 0)
            {
                if (m_RefinedFire != 0)
                    list.Add(1153737, String.Format("{0}\t{1}\t{2}", FireResistance.ToString(), "", m_RefinedFire.ToString()));// physical resist ~1_val~% / ~2_symb~~3_val~% Max
                else
                    list.Add(1060447, FireResistance.ToString()); // physical resist ~1_val~%
            }

            if (ColdResistance != 0 || m_RefinedCold != 0)
            {
                if (m_RefinedCold != 0)
                    list.Add(1153739, String.Format("{0}\t{1}\t{2}", ColdResistance.ToString(), "", m_RefinedCold.ToString()));// physical resist ~1_val~% / ~2_symb~~3_val~% Max
                else
                    list.Add(1060445, ColdResistance.ToString()); // physical resist ~1_val~%
            }

            if (PoisonResistance != 0 || m_RefinedPoison != 0)
            {
                if (m_RefinedPoison != 0)
                    list.Add(1153736, String.Format("{0}\t{1}\t{2}", PoisonResistance.ToString(), "", m_RefinedPoison.ToString()));// physical resist ~1_val~% / ~2_symb~~3_val~% Max
                else
                    list.Add(1060449, PoisonResistance.ToString()); // physical resist ~1_val~%
            }

            if (EnergyResistance != 0 || m_RefinedEnergy != 0)
            {
                if (m_RefinedEnergy != 0)
                    list.Add(1153738, String.Format("{0}\t{1}\t{2}", EnergyResistance.ToString(), "", m_RefinedEnergy.ToString()));// physical resist ~1_val~% / ~2_symb~~3_val~% Max
                else
                    list.Add(1060446, EnergyResistance.ToString()); // physical resist ~1_val~%
            }

            if (RefinedDefenseChance != 0)
                list.Add(1153733, String.Format("{0}\t{1}", "", RefinedDefenseChance.ToString()));
                */
        }

        public static int GetInherentLowerManaCost(Mobile from)
        {
            if (!Core.SA)
            {
                return 0;
            }

            int toReduce = 0;

            foreach (BaseArmor armor in from.Items.OfType<BaseArmor>())
            {
                if (armor.ArmorAttributes.MageArmor > 0 || armor.MaterialType == ArmorMaterialType.Wood || armor is BaseShield)
                    continue;

                switch (armor.MaterialType)
                {
                    case ArmorMaterialType.Studded:
                    case ArmorMaterialType.Bone:
                    case ArmorMaterialType.Stone:
                        toReduce += 3;
                        break;
                    case ArmorMaterialType.Ringmail:
                    case ArmorMaterialType.Chainmail:
                    case ArmorMaterialType.Plate:
                    case ArmorMaterialType.Dragon:
                        toReduce += 1;
                        break;
                }
            }

            return Math.Min(15, toReduce);
        }

        public static double GetInherentStaminaLossReduction(Mobile from)
        {
           
            double toReduce = 0.0;
            int count = 0;

            foreach (var armor in from.Items.OfType<BaseArmor>().OrderBy(arm => -GetArmorRatingReduction(arm)))
            {
                if (count == 5)
                    break;

                toReduce += GetArmorRatingReduction(armor);
                count++;
            }

            return toReduce;
        }

        public static double GetArmorRatingReduction(BaseArmor armor)
        {
            switch (armor.MaterialType)
            {
                default: return 0.0;
                case ArmorMaterialType.Cloth:
                case ArmorMaterialType.Leather:
                    return .2;
                case ArmorMaterialType.Wood:
                case ArmorMaterialType.Stone:
                case ArmorMaterialType.Studded:
                case ArmorMaterialType.Bone:
                case ArmorMaterialType.Ringmail:
                    return .3;
                case ArmorMaterialType.Chainmail:
                    return .9;
                case ArmorMaterialType.Plate:
                case ArmorMaterialType.Dragon:
                    return 1.2;
            }
        }
        #endregion

        #region Stygian Abyss
        [CommandProperty(AccessLevel.GameMaster)]
        public int GorgonLenseCharges
        {
            get { return m_GorgonLenseCharges; }
            set { m_GorgonLenseCharges = value; if (value == 0) m_GorgonLenseType = LenseType.None; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public LenseType GorgonLenseType
        {
            get { return m_GorgonLenseType; }
            set { m_GorgonLenseType = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TimesImbued
        {
            get { return m_TimesImbued; }
            set { m_TimesImbued = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsImbued
        {
            get
            {
                if (TimesImbued >= 1 && !m_IsImbued)
                    m_IsImbued = true;

                return m_IsImbued;
            }
            set
            {
                if (TimesImbued >= 1)
                    m_IsImbued = true;
                else
                    m_IsImbued = value; InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysNonImbuing
        {
            get { return m_PhysNonImbuing; }
            set { m_PhysNonImbuing = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireNonImbuing
        {
            get { return m_FireNonImbuing; }
            set { m_FireNonImbuing = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdNonImbuing
        {
            get { return m_ColdNonImbuing; }
            set { m_ColdNonImbuing = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonNonImbuing
        {
            get { return m_PoisonNonImbuing; }
            set { m_PoisonNonImbuing = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyNonImbuing
        {
            get { return m_EnergyNonImbuing; }
            set { m_EnergyNonImbuing = value; }
        }

        public virtual int[] BaseResists
        {
            get
            {
                var list = new int[5];

                list[0] = BasePhysicalResistance;
                list[1] = BaseFireResistance;
                list[2] = BaseColdResistance;
                list[3] = BasePoisonResistance;
                list[4] = BaseEnergyResistance;

                return list;
            }
        }

        public virtual void OnAfterImbued(Mobile m, int mod, int value)
        {
        }
        #endregion

        #region Runic Reforging
        [CommandProperty(AccessLevel.GameMaster)]
        public ReforgedPrefix ReforgedPrefix
        {
            get { return m_ReforgedPrefix; }
            set { m_ReforgedPrefix = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ReforgedSuffix ReforgedSuffix
        {
            get { return m_ReforgedSuffix; }
            set { m_ReforgedSuffix = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ElementoPvM Elemento
        {
            get { return m_elementoPvm; }
            set { m_elementoPvm = value; InvalidateProperties(); }
        }
        #endregion

        [CommandProperty(AccessLevel.GameMaster)]
        public int StrBonus
        {
            get
            {
                return (m_StrBonus == -1 ? Core.AOS ? AosStrBonus : OldStrBonus : m_StrBonus);
            }
            set
            {
                m_StrBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexBonus
        {
            get
            {
                return (m_DexBonus == -1 ? Core.AOS ? AosDexBonus : OldDexBonus : m_DexBonus);
            }
            set
            {
                m_DexBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntBonus
        {
            get
            {
                return (m_IntBonus == -1 ? Core.AOS ? AosIntBonus : OldIntBonus : m_IntBonus);
            }
            set
            {
                m_IntBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StrRequirement
        {
            get
            {
                if (m_NegativeAttributes.Massive > 0)
                {
                    return 125;
                }

                return m_StrReq == -1 ? (Core.AOS ? AosStrReq : OldStrReq) : m_StrReq;
            }
            set
            {
                m_StrReq = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexRequirement
        {
            get
            {
                return (m_DexReq == -1 ? Core.AOS ? AosDexReq : OldDexReq : m_DexReq);
            }
            set
            {
                m_DexReq = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntRequirement
        {
            get
            {
                return (m_IntReq == -1 ? Core.AOS ? AosIntReq : OldIntReq : m_IntReq);
            }
            set
            {
                m_IntReq = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified
        {
            get
            {
                return m_Identified;
            }
            set
            {
                m_Identified = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get
            {
                return m_PlayerConstructed;
            }
            set
            {
                m_PlayerConstructed = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual CraftResource Resource
        {
            get
            {
                return m_Resource;
            }
            set
            {
                if (m_Resource != value || m_Resource == DefaultResource)
                {
                    UnscaleDurability();
                    CraftResource old = m_Resource;

                    m_Resource = value;

                    ApplyResourceResistances(old);

                    if (CraftItem.RetainsColor(GetType()))
                    {
                        Hue = CraftResources.GetHue(m_Resource);
                    }

                    Invalidate();
                    InvalidateProperties();

                    if (Parent is Mobile)
                        ((Mobile)Parent).UpdateResistances();

                    ScaleDurability();
                }
            }
        }

        public virtual double ArmorScalar
        {
            get
            {
                int pos = (int)BodyPosition;

                if (pos >= 0 && pos < m_ArmorScalars.Length)
                    return m_ArmorScalars[pos];

                return 1.0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHitPoints
        {
            get
            {
                return m_MaxHitPoints;
            }
            set
            {
                m_MaxHitPoints = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitPoints
        {
            get
            {
                return m_HitPoints;
            }
            set
            {
                if (value != m_HitPoints && MaxHitPoints > 0)
                {
                    m_HitPoints = value;

                    if (m_HitPoints < 0)
                        Delete();
                    else if (m_HitPoints > MaxHitPoints)
                        m_HitPoints = MaxHitPoints;

                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get
            {
                return m_Crafter;
            }
            set
            {
                m_Crafter = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemQuality Quality
        {
            get
            {
                return m_Quality;
            }
            set
            {
                UnscaleDurability();
                m_Quality = value;
                Invalidate();
                InvalidateProperties();
                ScaleDurability();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorDurabilityLevel Durability
        {
            get
            {
                return m_Durability;
            }
            set
            {
                UnscaleDurability();
                m_Durability = value;
                ScaleDurability();
                InvalidateProperties();
            }
        }

        public virtual int ArtifactRarity
        {
            get
            {
                return 0;
            }
        }

        public override bool DisplayWeight
        {
            get
            {
                if (IsVvVItem)
                    return true;

                return base.DisplayWeight;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorProtectionLevel ProtectionLevel
        {
            get
            {
                return m_Protection;
            }
            set
            {
                if (m_Protection != value)
                {
                    m_Protection = value;

                    Invalidate();
                    InvalidateProperties();

                    if (Parent is Mobile)
                        ((Mobile)Parent).UpdateResistances();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes Attributes
        {
            get
            {
                return m_AosAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosArmorAttributes ArmorAttributes
        {
            get
            {
                return m_AosArmorAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SkillBonuses
        {
            get
            {
                return m_AosSkillBonuses;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SAAbsorptionAttributes AbsorptionAttributes
        {
            get
            {
                return m_SAAbsorptionAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public NegativeAttributes NegativeAttributes
        {
            get
            {
                return m_NegativeAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TalismanAttribute Protection
        {
            get { return m_TalismanProtection; }
            set { m_TalismanProtection = value; InvalidateProperties(); }
        }

        public override double DefaultWeight
        {
            get
            {
                if (NegativeAttributes == null || NegativeAttributes.Unwieldly == 0)
                    return base.DefaultWeight;

                return 50;
            }
        }

        public int ComputeStatReq(StatType type)
        {
            int v;

            if (type == StatType.Str)
                v = StrRequirement;
            else if (type == StatType.Dex)
                v = DexRequirement;
            else
                v = IntRequirement;

            return AOS.Scale(v, 100 - GetLowerStatReq());
        }

        public int ComputeStatBonus(StatType type)
        {
            if (type == StatType.Str)
                return StrBonus + Attributes.BonusStr;
            else if (type == StatType.Dex)
                return DexBonus + Attributes.BonusDex;
            else
                return IntBonus + Attributes.BonusInt;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalBonus
        {
            get
            {
                return m_PhysicalBonus;
            }
            set
            {
                m_PhysicalBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireBonus
        {
            get
            {
                return m_FireBonus;
            }
            set
            {
                m_FireBonus = value;
                InvalidateProperties();
            }
        }

        private int GetBaseColdBonus()
        {
            return 0;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdBonus
        {
            get
            {
                return m_ColdBonus + GetBaseColdBonus();
            }
            set
            {
                m_ColdBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonBonus
        {
            get
            {
                return m_PoisonBonus;
            }
            set
            {
                m_PoisonBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyBonus
        {
            get
            {
                return m_EnergyBonus;
            }
            set
            {
                m_EnergyBonus = value;
                InvalidateProperties();
            }
        }

        public virtual int BasePhysicalResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BaseFireResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BaseColdResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BasePoisonResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BaseEnergyResistance
        {
            get
            {
                return 0;
            }
        }

        public override int PhysicalResistance
        {
            get
            {
                // return BasePhysicalResistance + GetProtOffset() + m_PhysicalBonus;
                return GetResistenciaBase() + m_PhysicalBonus;
            }
        }

        public override int FireResistance
        {
            get
            {
                return GetResistenciaBase() + FireBonus;
            }
        }

        public override int ColdResistance
        {
            get
            {
                return GetResistenciaBase() + ColdBonus;
            }
        }

        public override int PoisonResistance
        {
            get
            {
                return GetResistenciaBase() + m_PoisonBonus;
            }
        }

        public override int EnergyResistance
        {
            get
            {
                return GetResistenciaBase() + m_EnergyBonus;
            }
        }

        public virtual int InitMinHits
        {
            get
            {
                return 0;
            }
        }
        public virtual int InitMaxHits
        {
            get
            {
                return 0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorBodyType BodyPosition
        {
            get
            {
                switch (Layer)
                {
                    default:
                    case Layer.Neck:
                        return ArmorBodyType.Gorget;
                    case Layer.TwoHanded:
                        return ArmorBodyType.Shield;
                    case Layer.Gloves:
                        return ArmorBodyType.Gloves;
                    case Layer.Helm:
                        return ArmorBodyType.Helmet;
                    case Layer.Arms:
                        return ArmorBodyType.Arms;

                    case Layer.InnerLegs:
                    case Layer.OuterLegs:
                    case Layer.Pants:
                        return ArmorBodyType.Legs;

                    case Layer.InnerTorso:
                    case Layer.OuterTorso:
                    case Layer.Shirt:
                        return ArmorBodyType.Chest;
                }
            }
        }

        // base pra todas armaduras, 0
        public int GetResistenciaBase()
        {
            /*
            switch (m_Protection)
            {
                case ArmorProtectionLevel.Guarding:
                    return 1;
                case ArmorProtectionLevel.Hardening:
                    return 2;
                case ArmorProtectionLevel.Fortification:
                    return 3;
                case ArmorProtectionLevel.Invulnerability:
                    return 4;
            }
            */
            return 0;
        }

        public void UnscaleDurability()
        {
            int scale = 100 + GetDurabilityBonus();

            m_HitPoints = ((m_HitPoints * 100) + (scale - 1)) / scale;
            m_MaxHitPoints = ((m_MaxHitPoints * 100) + (scale - 1)) / scale;

            InvalidateProperties();
        }

        public virtual int MaxMageryCircle { get { return 8; } }

        public void ScaleDurability()
        {
            int scale = 100 + GetDurabilityBonus();

            m_HitPoints = ((m_HitPoints * scale) + 99) / 100;
            m_MaxHitPoints = ((m_MaxHitPoints * scale) + 99) / 100;

            if (m_MaxHitPoints > 255)
                m_MaxHitPoints = 255;

            if (m_HitPoints > 255)
                m_HitPoints = 255;

            InvalidateProperties();
        }

        public virtual int GetDurabilityBonus()
        {
            int bonus = 0;

            if (m_Quality == ItemQuality.Exceptional && !(this is GargishLeatherWingArmor))
                bonus += 20;

            switch (m_Durability)
            {
                case ArmorDurabilityLevel.Durable:
                    bonus += 20;
                    break;
                case ArmorDurabilityLevel.Substantial:
                    bonus += 50;
                    break;
                case ArmorDurabilityLevel.Massive:
                    bonus += 70;
                    break;
                case ArmorDurabilityLevel.Fortified:
                    bonus += 100;
                    break;
                case ArmorDurabilityLevel.Indestructible:
                    bonus += 120;
                    break;
            }

            if (Core.AOS)
            {
                bonus += m_AosArmorAttributes.DurabilityBonus;

                if (m_Resource == CraftResource.Eucalipto)
                    return bonus;

                CraftResourceInfo resInfo = CraftResources.GetInfo(m_Resource);
                CraftAttributeInfo attrInfo = null;

                if (resInfo != null)
                    attrInfo = resInfo.AttributeInfo;

                if (attrInfo != null)
                    bonus += attrInfo.ArmorDurability;
            }

            return bonus;
        }

        public virtual bool Scissor(Mobile from, Scissors scissors)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack.
                return false;
            }

            if (Ethics.Ethic.IsImbued(this))
            {
                from.SendLocalizedMessage(502440); // Scissors can not be used on that to produce anything.
                return false;
            }

            CraftSystem system = DefTailoring.CraftSystem;

            CraftItem item = system.CraftItems.SearchFor(GetType());

            if (item != null && item.Resources.Count == 1 && item.Resources.GetAt(0).Amount >= 2)
            {
                try
                {
                    Item res = (Item)Activator.CreateInstance(CraftResources.GetInfo(m_Resource).ResourceTypes[0]);

                    ScissorHelper(from, res, m_PlayerConstructed ? (item.Resources.GetAt(0).Amount / 2) : 1);
                    return true;
                }
                catch
                {
                }
            }

            from.SendLocalizedMessage(502440); // Scissors can not be used on that to produce anything.
            return false;
        }

        private static double[] m_ArmorScalars = { 0.07, 0.07, 0.14, 0.15, 0.22, 0.35 };

        public static double[] ArmorScalars
        {
            get
            {
                return m_ArmorScalars;
            }
            set
            {
                m_ArmorScalars = value;
            }
        }

        public static void ValidateMobile(Mobile m)
        {
            for (int i = m.Items.Count - 1; i >= 0; --i)
            {
                if (i >= m.Items.Count)
                    continue;

                Item item = m.Items[i];

                if (item is BaseArmor)
                {
                    BaseArmor armor = (BaseArmor)item;

                    if (m.Race == Race.Gargoyle && !armor.CanBeWornByGargoyles)
                    {
                        m.SendLocalizedMessage(1111708); // Gargoyles can't wear 
                        m.AddToBackpack(armor);
                    }
                    if (armor.RequiredRace != null && m.Race != armor.RequiredRace && armor.RequiredRace != Race.Elf)
                    {
                        if (armor.RequiredRace == Race.Gargoyle)
                            m.SendLocalizedMessage(1111707); // Only gargoyles can wear 
                        else
                            m.SendMessage("Only {0} may use this.", armor.RequiredRace.PluralName);

                        m.AddToBackpack(armor);
                    }
                    else if (!armor.AllowMaleWearer && !m.Female && m.AccessLevel < AccessLevel.GameMaster)
                    {
                        if (armor.AllowFemaleWearer)
                            m.SendLocalizedMessage(1010388); // Only females can wear 
                        else
                            m.SendMessage("You may not wear this.");

                        m.AddToBackpack(armor);
                    }
                    else if (!armor.AllowFemaleWearer && m.Female && m.AccessLevel < AccessLevel.GameMaster)
                    {
                        if (armor.AllowMaleWearer)
                            m.SendLocalizedMessage(1063343); // Only males can wear 
                        else
                            m.SendMessage("You may not wear this.");

                        m.AddToBackpack(armor);
                    }
                }
            }
        }

        public int GetLowerStatReq()
        {
            if (!Core.AOS)
                return 0;

            int v = m_AosArmorAttributes.LowerStatReq;

            if (m_Resource == CraftResource.Eucalipto)
                return v;

            CraftResourceInfo info = CraftResources.GetInfo(m_Resource);

            if (info != null)
            {
                CraftAttributeInfo attrInfo = info.AttributeInfo;

                if (attrInfo != null)
                    v += attrInfo.ArmorLowerRequirements;
            }

            if (v > 100)
                v = 100;

            return v;
        }

        public override void OnAdded(object parent)
        {
            if (parent is Mobile)
            {
                Mobile from = (Mobile)parent;

                m_AosSkillBonuses.AddTo(from);
                if (this.Elemento != ElementoPvM.None && from.Elemento != this.Elemento)
                {
                    if(from is PlayerMobile && ((PlayerMobile)from).Nivel > 1)
                    {
                        var count = SetHelper.CountElemento(from, this.Elemento);
                        if (count >= 4 && from.Elemento != this.Elemento)
                        {
                            from.PrivateOverheadMessage("* energizado *");
                            from.SendMessage("Voce sente seu corpo se energizando com energia elemental de " + this.Elemento.ToString());
                            from.Elemento = this.Elemento;
                            from.FixedEffect(0x375A, 10, 15);
                            from.PlaySound(0x1E7);
                        } else if(from.Elemento != this.Elemento)
                        {
                            from.SendMessage("Pecas do elemento " + this.Elemento.ToString() + " " + count + "/4");
                        }
                    } else
                    {
                        from.SendMessage("Voce sente energia ao equipar esta armadura, porem nao entende como usa-la.");
                        from.SendMessage("Procure a estatua elemental no centro de New Haven para aprender a usar elementos");
                    }
                
                }

                #region Mondain's Legacy Sets
                if (IsSetItem)
                {
                    m_SetEquipped = SetHelper.FullSetEquipped(from, SetID, Pieces);

                    if (m_SetEquipped)
                    {
                        m_LastEquipped = true;
                        SetHelper.AddSetBonus(from, SetID);
                    }
                }
                #endregion

                from.Delta(MobileDelta.Armor); // Tell them armor rating has changed
            }
        }

        public virtual double ScaleArmorByDurability(double armor)
        {
            int scale = 100;

            if (m_MaxHitPoints > 0 && m_HitPoints < m_MaxHitPoints)
                scale = 50 + ((50 * m_HitPoints) / m_MaxHitPoints);

            return (armor * scale) / 100;
        }

        protected void Invalidate()
        {
            if (Parent is Mobile)
                ((Mobile)Parent).Delta(MobileDelta.Armor); // Tell them armor rating has changed
        }

        public BaseArmor(Serial serial)
            : base(serial)
        {
        }

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            Attributes = 0x00000001,
            ArmorAttributes = 0x00000002,
            PhysicalBonus = 0x00000004,
            FireBonus = 0x00000008,
            ColdBonus = 0x00000010,
            PoisonBonus = 0x00000020,
            EnergyBonus = 0x00000040,
            Identified = 0x00000080,
            MaxHitPoints = 0x00000100,
            HitPoints = 0x00000200,
            Crafter = 0x00000400,
            Quality = 0x00000800,
            Durability = 0x00001000,
            Protection = 0x00002000,
            Resource = 0x00004000,
            BaseArmor = 0x00008000,
            StrBonus = 0x00010000,
            DexBonus = 0x00020000,
            IntBonus = 0x00040000,
            StrReq = 0x00080000,
            DexReq = 0x00100000,
            IntReq = 0x00200000,
            MedAllowance = 0x00400000,
            SkillBonuses = 0x00800000,
            PlayerConstructed = 0x01000000,
            xAbsorptionAttributes = 0x02000000,
            //TimesImbued = 0x04000000,
            NegativeAttributes = 0x08000000,
            Altered = 0x10000000,
            TalismanProtection = 0x20000000,
            EngravedText = 0x40000000
        }

        #region Mondain's Legacy Sets
        private static void SetSaveFlag(ref SetFlag flags, SetFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SetFlag flags, SetFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SetFlag
        {
            None = 0x00000000,
            Attributes = 0x00000001,
            ArmorAttributes = 0x00000002,
            SkillBonuses = 0x00000004,
            PhysicalBonus = 0x00000008,
            FireBonus = 0x00000010,
            ColdBonus = 0x00000020,
            PoisonBonus = 0x00000040,
            EnergyBonus = 0x00000080,
            Hue = 0x00000100,
            LastEquipped = 0x00000200,
            SetEquipped = 0x00000400,
            SetSelfRepair = 0x00000800,
        }
        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)14); // version

            // Version 14 - removed VvV Item (handled in VvV System) and BlockRepair (Handled as negative attribute)

            writer.Write(_Owner);
            writer.Write(_OwnerName);

            //Version 11
            writer.Write(m_RefinedPhysical);
            writer.Write(m_RefinedFire);
            writer.Write(m_RefinedCold);
            writer.Write(m_RefinedPoison);
            writer.Write(m_RefinedEnergy);

            //Version 10
            writer.Write((bool)m_IsImbued);

            // Version 9
            #region Runic Reforging
            writer.Write((int)m_ReforgedPrefix);
            writer.Write((int)m_ReforgedSuffix);
            writer.Write((int)m_elementoPvm);
            #endregion

            #region Stygian Abyss
            writer.Write(m_GorgonLenseCharges);
            writer.Write((int)m_GorgonLenseType);

            writer.Write(m_PhysNonImbuing);
            writer.Write(m_FireNonImbuing);
            writer.Write(m_ColdNonImbuing);
            writer.Write(m_PoisonNonImbuing);
            writer.Write(m_EnergyNonImbuing);

            // Version 8
            writer.Write((int)m_TimesImbued);

            #endregion

            writer.Write((Mobile)m_BlessedBy);

            SetFlag sflags = SetFlag.None;

            SetSaveFlag(ref sflags, SetFlag.Attributes, !m_SetAttributes.IsEmpty);
            SetSaveFlag(ref sflags, SetFlag.SkillBonuses, !m_SetSkillBonuses.IsEmpty);
            SetSaveFlag(ref sflags, SetFlag.PhysicalBonus, m_SetPhysicalBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.FireBonus, m_SetFireBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.ColdBonus, m_SetColdBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.PoisonBonus, m_SetPoisonBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.EnergyBonus, m_SetEnergyBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.Hue, m_SetHue != 0);
            SetSaveFlag(ref sflags, SetFlag.LastEquipped, m_LastEquipped);
            SetSaveFlag(ref sflags, SetFlag.SetEquipped, m_SetEquipped);
            SetSaveFlag(ref sflags, SetFlag.SetSelfRepair, m_SetSelfRepair != 0);

            writer.WriteEncodedInt((int)sflags);

            if (GetSaveFlag(sflags, SetFlag.Attributes))
                m_SetAttributes.Serialize(writer);

            if (GetSaveFlag(sflags, SetFlag.SkillBonuses))
                m_SetSkillBonuses.Serialize(writer);

            if (GetSaveFlag(sflags, SetFlag.PhysicalBonus))
                writer.WriteEncodedInt((int)m_SetPhysicalBonus);

            if (GetSaveFlag(sflags, SetFlag.FireBonus))
                writer.WriteEncodedInt((int)m_SetFireBonus);

            if (GetSaveFlag(sflags, SetFlag.ColdBonus))
                writer.WriteEncodedInt((int)m_SetColdBonus);

            if (GetSaveFlag(sflags, SetFlag.PoisonBonus))
                writer.WriteEncodedInt((int)m_SetPoisonBonus);

            if (GetSaveFlag(sflags, SetFlag.EnergyBonus))
                writer.WriteEncodedInt((int)m_SetEnergyBonus);

            if (GetSaveFlag(sflags, SetFlag.Hue))
                writer.WriteEncodedInt((int)m_SetHue);

            if (GetSaveFlag(sflags, SetFlag.LastEquipped))
                writer.Write((bool)m_LastEquipped);

            if (GetSaveFlag(sflags, SetFlag.SetEquipped))
                writer.Write((bool)m_SetEquipped);

            if (GetSaveFlag(sflags, SetFlag.SetSelfRepair))
                writer.WriteEncodedInt((int)m_SetSelfRepair);

            // Version 7
            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.EngravedText, !String.IsNullOrEmpty(_EngravedText));
            SetSaveFlag(ref flags, SaveFlag.TalismanProtection, !m_TalismanProtection.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.NegativeAttributes, !m_NegativeAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.Attributes, !m_AosAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.ArmorAttributes, !m_AosArmorAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.PhysicalBonus, m_PhysicalBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.FireBonus, m_FireBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.ColdBonus, m_ColdBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.PoisonBonus, m_PoisonBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.EnergyBonus, m_EnergyBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.Identified, m_Identified != false);
            SetSaveFlag(ref flags, SaveFlag.MaxHitPoints, m_MaxHitPoints != 0);
            SetSaveFlag(ref flags, SaveFlag.HitPoints, m_HitPoints != 0);
            SetSaveFlag(ref flags, SaveFlag.Crafter, m_Crafter != null);
            SetSaveFlag(ref flags, SaveFlag.Quality, m_Quality != ItemQuality.Normal);
            SetSaveFlag(ref flags, SaveFlag.Durability, m_Durability != ArmorDurabilityLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.Protection, m_Protection != ArmorProtectionLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.Resource, m_Resource != DefaultResource);
            SetSaveFlag(ref flags, SaveFlag.BaseArmor, m_ArmorBase != -1);
            SetSaveFlag(ref flags, SaveFlag.StrBonus, m_StrBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.DexBonus, m_DexBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.IntBonus, m_IntBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.StrReq, m_StrReq != -1);
            SetSaveFlag(ref flags, SaveFlag.DexReq, m_DexReq != -1);
            SetSaveFlag(ref flags, SaveFlag.IntReq, m_IntReq != -1);
            SetSaveFlag(ref flags, SaveFlag.MedAllowance, m_Meditate != (AMA)(-1));
            SetSaveFlag(ref flags, SaveFlag.SkillBonuses, !m_AosSkillBonuses.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.PlayerConstructed, m_PlayerConstructed != false);
            SetSaveFlag(ref flags, SaveFlag.xAbsorptionAttributes, !m_SAAbsorptionAttributes.IsEmpty);
            //SetSaveFlag(ref flags, SaveFlag.TimesImbued, m_TimesImbued != 0);
            SetSaveFlag(ref flags, SaveFlag.Altered, m_Altered);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.EngravedText))
                writer.Write(_EngravedText);

            if (GetSaveFlag(flags, SaveFlag.TalismanProtection))
                m_TalismanProtection.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.NegativeAttributes))
                m_NegativeAttributes.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.Attributes))
                m_AosAttributes.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.ArmorAttributes))
                m_AosArmorAttributes.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.PhysicalBonus))
                writer.WriteEncodedInt((int)m_PhysicalBonus);

            if (GetSaveFlag(flags, SaveFlag.FireBonus))
                writer.WriteEncodedInt((int)m_FireBonus);

            if (GetSaveFlag(flags, SaveFlag.ColdBonus))
                writer.WriteEncodedInt((int)m_ColdBonus);

            if (GetSaveFlag(flags, SaveFlag.PoisonBonus))
                writer.WriteEncodedInt((int)m_PoisonBonus);

            if (GetSaveFlag(flags, SaveFlag.EnergyBonus))
                writer.WriteEncodedInt((int)m_EnergyBonus);

            if (GetSaveFlag(flags, SaveFlag.MaxHitPoints))
                writer.WriteEncodedInt((int)m_MaxHitPoints);

            if (GetSaveFlag(flags, SaveFlag.HitPoints))
                writer.WriteEncodedInt((int)m_HitPoints);

            if (GetSaveFlag(flags, SaveFlag.Crafter))
                writer.Write((Mobile)m_Crafter);

            if (GetSaveFlag(flags, SaveFlag.Quality))
                writer.WriteEncodedInt((int)m_Quality);

            if (GetSaveFlag(flags, SaveFlag.Durability))
                writer.WriteEncodedInt((int)m_Durability);

            if (GetSaveFlag(flags, SaveFlag.Protection))
                writer.WriteEncodedInt((int)m_Protection);

            if (GetSaveFlag(flags, SaveFlag.Resource))
                writer.WriteEncodedInt((int)m_Resource);

            if (GetSaveFlag(flags, SaveFlag.BaseArmor))
                writer.WriteEncodedInt((int)m_ArmorBase);

            if (GetSaveFlag(flags, SaveFlag.StrBonus))
                writer.WriteEncodedInt((int)m_StrBonus);

            if (GetSaveFlag(flags, SaveFlag.DexBonus))
                writer.WriteEncodedInt((int)m_DexBonus);

            if (GetSaveFlag(flags, SaveFlag.IntBonus))
                writer.WriteEncodedInt((int)m_IntBonus);

            if (GetSaveFlag(flags, SaveFlag.StrReq))
                writer.WriteEncodedInt((int)m_StrReq);

            if (GetSaveFlag(flags, SaveFlag.DexReq))
                writer.WriteEncodedInt((int)m_DexReq);

            if (GetSaveFlag(flags, SaveFlag.IntReq))
                writer.WriteEncodedInt((int)m_IntReq);

            if (GetSaveFlag(flags, SaveFlag.MedAllowance))
                writer.WriteEncodedInt((int)m_Meditate);

            if (GetSaveFlag(flags, SaveFlag.SkillBonuses))
                m_AosSkillBonuses.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.xAbsorptionAttributes))
                m_SAAbsorptionAttributes.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 14:
                case 13:
                case 12:
                    {
                        if (version == 13)
                            reader.ReadBool();

                        _Owner = reader.ReadMobile();
                        _OwnerName = reader.ReadString();
                        goto case 11;
                    }
                case 11:
                    {
                        m_RefinedPhysical = reader.ReadInt();
                        m_RefinedFire = reader.ReadInt();
                        m_RefinedCold = reader.ReadInt();
                        m_RefinedPoison = reader.ReadInt();
                        m_RefinedEnergy = reader.ReadInt();
                        goto case 10;
                    }
                case 10:
                    {
                        m_IsImbued = reader.ReadBool();
                        goto case 9;
                    }
                case 9:
                    {
                        #region Runic Reforging
                        m_ReforgedPrefix = (ReforgedPrefix)reader.ReadInt();
                        m_ReforgedSuffix = (ReforgedSuffix)reader.ReadInt();
                        m_elementoPvm = (ElementoPvM)reader.ReadInt();

                        if (version == 13 && reader.ReadBool())
                        {
                            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                            {
                                m_NegativeAttributes.NoRepair = 1;
                            });
                        }
                        #endregion

                        #region Stygian Abyss
                        m_GorgonLenseCharges = reader.ReadInt();
                        m_GorgonLenseType = (LenseType)reader.ReadInt();

                        m_PhysNonImbuing = reader.ReadInt();
                        m_FireNonImbuing = reader.ReadInt();
                        m_ColdNonImbuing = reader.ReadInt();
                        m_PoisonNonImbuing = reader.ReadInt();
                        m_EnergyNonImbuing = reader.ReadInt();
                        goto case 8;
                    }
                case 8:
                    {
                        m_TimesImbued = reader.ReadInt();

                        #endregion

                        m_BlessedBy = reader.ReadMobile();

                        SetFlag sflags = (SetFlag)reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.Attributes))
                            m_SetAttributes = new AosAttributes(this, reader);
                        else
                            m_SetAttributes = new AosAttributes(this);

                        if (GetSaveFlag(sflags, SetFlag.ArmorAttributes))
                            m_SetSelfRepair = (new AosArmorAttributes(this, reader)).SelfRepair;

                        if (GetSaveFlag(sflags, SetFlag.SkillBonuses))
                            m_SetSkillBonuses = new AosSkillBonuses(this, reader);
                        else
                            m_SetSkillBonuses = new AosSkillBonuses(this);

                        if (GetSaveFlag(sflags, SetFlag.PhysicalBonus))
                            m_SetPhysicalBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.FireBonus))
                            m_SetFireBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.ColdBonus))
                            m_SetColdBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.PoisonBonus))
                            m_SetPoisonBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.EnergyBonus))
                            m_SetEnergyBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.Hue))
                            m_SetHue = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.LastEquipped))
                            m_LastEquipped = reader.ReadBool();

                        if (GetSaveFlag(sflags, SetFlag.SetEquipped))
                            m_SetEquipped = reader.ReadBool();

                        if (GetSaveFlag(sflags, SetFlag.SetSelfRepair))
                            m_SetSelfRepair = reader.ReadEncodedInt();

                        goto case 5;
                    }
                case 7:
                case 6:
                case 5:
                    {
                        SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.EngravedText))
                            _EngravedText = reader.ReadString();

                        if (GetSaveFlag(flags, SaveFlag.TalismanProtection))
                            m_TalismanProtection = new TalismanAttribute(reader);
                        else
                            m_TalismanProtection = new TalismanAttribute();

                        if (GetSaveFlag(flags, SaveFlag.NegativeAttributes))
                            m_NegativeAttributes = new NegativeAttributes(this, reader);
                        else
                            m_NegativeAttributes = new NegativeAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.Attributes))
                            m_AosAttributes = new AosAttributes(this, reader);
                        else
                            m_AosAttributes = new AosAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.ArmorAttributes))
                            m_AosArmorAttributes = new AosArmorAttributes(this, reader);
                        else
                            m_AosArmorAttributes = new AosArmorAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.PhysicalBonus))
                            m_PhysicalBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.FireBonus))
                            m_FireBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.ColdBonus))
                            m_ColdBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.PoisonBonus))
                            m_PoisonBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.EnergyBonus))
                            m_EnergyBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Identified))
                            m_Identified = (version >= 7 || reader.ReadBool());

                        if (GetSaveFlag(flags, SaveFlag.MaxHitPoints))
                            m_MaxHitPoints = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.HitPoints))
                            m_HitPoints = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Crafter))
                            m_Crafter = reader.ReadMobile();

                        if (GetSaveFlag(flags, SaveFlag.Quality))
                            m_Quality = (ItemQuality)reader.ReadEncodedInt();
                        else
                            m_Quality = ItemQuality.Normal;

                        if (version == 5 && m_Quality == ItemQuality.Low)
                            m_Quality = ItemQuality.Normal;

                        if (GetSaveFlag(flags, SaveFlag.Durability))
                        {
                            m_Durability = (ArmorDurabilityLevel)reader.ReadEncodedInt();

                            if (m_Durability > ArmorDurabilityLevel.Indestructible)
                                m_Durability = ArmorDurabilityLevel.Durable;
                        }

                        if (GetSaveFlag(flags, SaveFlag.Protection))
                        {
                            m_Protection = (ArmorProtectionLevel)reader.ReadEncodedInt();

                            if (m_Protection > ArmorProtectionLevel.Invulnerability)
                                m_Protection = ArmorProtectionLevel.Defense;
                        }

                        if (GetSaveFlag(flags, SaveFlag.Resource))
                            m_Resource = (CraftResource)reader.ReadEncodedInt();
                        else
                            m_Resource = DefaultResource;

                        if (m_Resource == CraftResource.None)
                            m_Resource = DefaultResource;

                        if (GetSaveFlag(flags, SaveFlag.BaseArmor))
                            m_ArmorBase = reader.ReadEncodedInt();
                        else
                            m_ArmorBase = -1;

                        if (GetSaveFlag(flags, SaveFlag.StrBonus))
                            m_StrBonus = reader.ReadEncodedInt();
                        else
                            m_StrBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.DexBonus))
                            m_DexBonus = reader.ReadEncodedInt();
                        else
                            m_DexBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.IntBonus))
                            m_IntBonus = reader.ReadEncodedInt();
                        else
                            m_IntBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.StrReq))
                            m_StrReq = reader.ReadEncodedInt();
                        else
                            m_StrReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.DexReq))
                            m_DexReq = reader.ReadEncodedInt();
                        else
                            m_DexReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.IntReq))
                            m_IntReq = reader.ReadEncodedInt();
                        else
                            m_IntReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.MedAllowance))
                            m_Meditate = (AMA)reader.ReadEncodedInt();
                        else
                            m_Meditate = (AMA)(-1);

                        if (GetSaveFlag(flags, SaveFlag.SkillBonuses))
                            m_AosSkillBonuses = new AosSkillBonuses(this, reader);

                        if (GetSaveFlag(flags, SaveFlag.PlayerConstructed))
                            m_PlayerConstructed = true;

                        if (version > 7 && GetSaveFlag(flags, SaveFlag.xAbsorptionAttributes))
                            m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this, reader);
                        else
                            m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.Altered))
                            m_Altered = true;

                        break;
                    }
                case 4:
                    {
                        m_AosAttributes = new AosAttributes(this, reader);
                        m_AosArmorAttributes = new AosArmorAttributes(this, reader);
                        goto case 3;
                    }
                case 3:
                    {
                        m_PhysicalBonus = reader.ReadInt();
                        m_FireBonus = reader.ReadInt();
                        m_ColdBonus = reader.ReadInt();
                        m_PoisonBonus = reader.ReadInt();
                        m_EnergyBonus = reader.ReadInt();
                        goto case 2;
                    }
                case 2:
                case 1:
                    {
                        m_Identified = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        m_ArmorBase = reader.ReadInt();
                        m_MaxHitPoints = reader.ReadInt();
                        m_HitPoints = reader.ReadInt();
                        m_Crafter = reader.ReadMobile();
                        m_Quality = (ItemQuality)reader.ReadInt();
                        m_Durability = (ArmorDurabilityLevel)reader.ReadInt();
                        m_Protection = (ArmorProtectionLevel)reader.ReadInt();

                        AMT mat = (AMT)reader.ReadInt();

                        if (m_ArmorBase == RevertArmorBase)
                            m_ArmorBase = -1;

                        /*m_BodyPos = (ArmorBodyType)*/
                        reader.ReadInt();

                        if (version < 4)
                        {
                            m_AosAttributes = new AosAttributes(this);
                            m_AosArmorAttributes = new AosArmorAttributes(this);
                        }

                        if (version < 3 && m_Quality == ItemQuality.Exceptional)
                            DistributeExceptionalBonuses(null, 6);

                        if (version >= 2)
                        {
                            m_Resource = (CraftResource)reader.ReadInt();
                        }
                        else
                        {
                            OreInfo info;

                            switch (reader.ReadInt())
                            {
                                default:
                                case 0:
                                    info = OreInfo.Iron;
                                    break;
                                case 1:
                                    info = OreInfo.DullCopper;
                                    break;
                                case 2:
                                    info = OreInfo.ShadowIron;
                                    break;
                                case 3:
                                    info = OreInfo.Copper;
                                    break;
                                case 4:
                                    info = OreInfo.Bronze;
                                    break;
                                case 5:
                                    info = OreInfo.Gold;
                                    break;
                                case 6:
                                    info = OreInfo.Agapite;
                                    break;
                                case 7:
                                    info = OreInfo.Verite;
                                    break;
                                case 8:
                                    info = OreInfo.Valorite;
                                    break;
                            }

                            m_Resource = CraftResources.GetFromOreInfo(info, mat);
                        }

                        m_StrBonus = reader.ReadInt();
                        m_DexBonus = reader.ReadInt();
                        m_IntBonus = reader.ReadInt();
                        m_StrReq = reader.ReadInt();
                        m_DexReq = reader.ReadInt();
                        m_IntReq = reader.ReadInt();

                        if (m_StrBonus == OldStrBonus)
                            m_StrBonus = -1;

                        if (m_DexBonus == OldDexBonus)
                            m_DexBonus = -1;

                        if (m_IntBonus == OldIntBonus)
                            m_IntBonus = -1;

                        if (m_StrReq == OldStrReq)
                            m_StrReq = -1;

                        if (m_DexReq == OldDexReq)
                            m_DexReq = -1;

                        if (m_IntReq == OldIntReq)
                            m_IntReq = -1;

                        m_Meditate = (AMA)reader.ReadInt();

                        if (m_Meditate == OldMedAllowance)
                            m_Meditate = (AMA)(-1);

                        if (m_Resource == CraftResource.None)
                        {
                            if (mat == ArmorMaterialType.Studded || mat == ArmorMaterialType.Leather)
                                m_Resource = CraftResource.RegularLeather;
                            else if (mat == ArmorMaterialType.Spined)
                                m_Resource = CraftResource.CouroSpinned;
                            else if (mat == ArmorMaterialType.Horned)
                                m_Resource = CraftResource.CouroHorned;
                            else if (mat == ArmorMaterialType.Barbed)
                                m_Resource = CraftResource.CouroBarbed;
                            else
                                m_Resource = CraftResource.Ferro;
                        }

                        if (m_MaxHitPoints == 0 && m_HitPoints == 0)
                            m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits);

                        break;
                    }


            }

            #region Mondain's Legacy Sets
            if (m_SetAttributes == null)
                m_SetAttributes = new AosAttributes(this);

            if (m_SetSkillBonuses == null)
                m_SetSkillBonuses = new AosSkillBonuses(this);
            #endregion

            if (m_AosSkillBonuses == null)
                m_AosSkillBonuses = new AosSkillBonuses(this);

            if (Parent is Mobile)
                m_AosSkillBonuses.AddTo((Mobile)Parent);

            int strBonus = ComputeStatBonus(StatType.Str);
            int dexBonus = ComputeStatBonus(StatType.Dex);
            int intBonus = ComputeStatBonus(StatType.Int);

            if (Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0))
            {
                Mobile m = (Mobile)Parent;

                string modName = Serial.ToString();

                if (strBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                if (dexBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                if (intBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
            }

            if (Parent is Mobile)
                ((Mobile)Parent).CheckStatTimers();

            if (version < 7)
                m_PlayerConstructed = true; // we don't know, so, assume it's crafted

            if (version < 13)
                ApplyResourceResistances(CraftResource.None);

            // SOH AGORA
            // this.Attributes.Reset();
        }

        public virtual CraftResource DefaultResource
        {
            get
            {
                return CraftResource.Ferro;
            }
        }

        public BaseArmor(int itemID)
            : base(itemID)
        {
            m_Quality = ItemQuality.Normal;
            m_Durability = ArmorDurabilityLevel.Regular;
            m_Crafter = null;

            m_Resource = DefaultResource;
            Hue = CraftResources.GetHue(m_Resource);

            m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits);

            Layer = (Layer)ItemData.Quality;

            m_AosAttributes = new AosAttributes(this);
            m_AosArmorAttributes = new AosArmorAttributes(this);
            m_AosSkillBonuses = new AosSkillBonuses(this);

            m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this);

            #region Mondain's Legacy Sets
            m_SetAttributes = new AosAttributes(this);
            m_SetSkillBonuses = new AosSkillBonuses(this);
            #endregion

            m_AosSkillBonuses = new AosSkillBonuses(this);
            m_NegativeAttributes = new NegativeAttributes(this);
            m_TalismanProtection = new TalismanAttribute();
        }

        public override bool AllowSecureTrade(Mobile from, Mobile to, Mobile newOwner, bool accepted)
        {
            if (!Ethics.Ethic.CheckTrade(from, to, newOwner, this))
                return false;

            return base.AllowSecureTrade(from, to, newOwner, accepted);
        }

        public virtual Race RequiredRace
        {
            get
            {
                return null;
            }
        }

        public virtual bool CanBeWornByGargoyles
        {
            get
            {
                return false;
            }
        }

        public override bool CanEquip(Mobile from)
        {
            if (!Ethics.Ethic.CheckEquip(from, this))
                return false;

            if (from.IsPlayer())
            {
                if (_Owner != null && _Owner != from)
                {
                    from.SendLocalizedMessage(501023); // You must be the owner to use this item.
                    return false;
                }

                if (this is IAccountRestricted && ((IAccountRestricted)this).Account != null)
                {
                    Accounting.Account acct = from.Account as Accounting.Account;

                    if (acct == null || acct.Username != ((IAccountRestricted)this).Account)
                    {
                        from.SendLocalizedMessage(1071296); // This item is Account Bound and your character is not bound to it. You cannot use this item.
                        return false;
                    }
                }

                if (IsVvVItem && !Engines.VvV.ViceVsVirtueSystem.IsVvV(from))
                {
                    from.SendLocalizedMessage(1155496); // This item can only be used by VvV participants!
                    return false;
                }

                bool morph = from.FindItemOnLayer(Layer.Earrings) is MorphEarrings;

                if (from.Race == Race.Gargoyle && !CanBeWornByGargoyles)
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1111708); // Gargoyles can't wear this.
                    return false;
                }
                if (RequiredRace != null && from.Race != RequiredRace && !morph && RequiredRace != Race.Elf)
                {
                    if (RequiredRace == Race.Gargoyle)
                        from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1111707); // Only gargoyles can wear this.
                    else
                        from.SendMessage("Only {0} may use this.", RequiredRace.PluralName);

                    return false;
                }
                else if (!AllowMaleWearer && !from.Female)
                {
                    if (AllowFemaleWearer)
                        from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1010388); // Only females can wear this.
                    else
                        from.SendMessage("You may not wear this.");

                    return false;
                }
                else if (!AllowFemaleWearer && from.Female)
                {
                    if (AllowMaleWearer)
                        from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1063343); // Only males can wear this.
                    else
                        from.SendMessage("You may not wear this.");

                    return false;
                }
                #region Personal Bless Deed
                else if (BlessedBy != null && BlessedBy != from)
                {
                    from.SendLocalizedMessage(1075277); // That item is blessed by another player.

                    return false;
                }
                #endregion
                else
                {
                    int strBonus = ComputeStatBonus(StatType.Str), strReq = ComputeStatReq(StatType.Str);
                    int dexBonus = ComputeStatBonus(StatType.Dex), dexReq = ComputeStatReq(StatType.Dex);
                    int intBonus = ComputeStatBonus(StatType.Int), intReq = ComputeStatReq(StatType.Int);

                    if (from.Dex < dexReq || (from.Dex + dexBonus) < 1)
                    {
                        from.SendLocalizedMessage(502077); // You do not have enough dexterity to equip this item.
                        return false;
                    }
                    else if (from.Str < strReq || (from.Str + strBonus) < 1)
                    {
                        from.SendLocalizedMessage(500213); // You are not strong enough to equip that.
                        return false;
                    }
                    else if (from.Int < intReq || (from.Int + intBonus) < 1)
                    {
                        from.SendMessage("You are not smart enough to equip that.");
                        return false;
                    }
                }
            }

            if (!Server.Engines.XmlSpawner2.XmlAttach.CheckCanEquip(this, from))
                return false;
            else
                return base.CanEquip(from);
        }

        public override bool CheckPropertyConfliction(Mobile m)
        {
            if (base.CheckPropertyConfliction(m))
                return true;

            if (Layer == Layer.Pants)
                return (m.FindItemOnLayer(Layer.InnerLegs) != null);

            if (Layer == Layer.Shirt)
                return (m.FindItemOnLayer(Layer.InnerTorso) != null);

            return false;
        }

        public override bool OnEquip(Mobile from)
        {
            from.CheckStatTimers();

            int strBonus = ComputeStatBonus(StatType.Str);
            int dexBonus = ComputeStatBonus(StatType.Dex);
            int intBonus = ComputeStatBonus(StatType.Int);

            if (from.RP && dexBonus < 0 && from is PlayerMobile && ((PlayerMobile)from).Talentos.Tem(Talento.ArmaduraPesada))
                dexBonus = 0;

            if (strBonus != 0 || dexBonus != 0 || intBonus != 0)
            {
                string modName = Serial.ToString();

                if (strBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                if (dexBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                if (intBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
            }

            Server.Engines.XmlSpawner2.XmlAttach.CheckOnEquip(this, from);

            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            if (parent is Mobile)
            {
                Mobile m = (Mobile)parent;
                string modName = Serial.ToString();

                m.RemoveStatMod(modName + "Str");
                m.RemoveStatMod(modName + "Dex");
                m.RemoveStatMod(modName + "Int");

                m_AosSkillBonuses.Remove();

                ((Mobile)parent).Delta(MobileDelta.Armor); // Tell them armor rating has changed
                m.CheckStatTimers();

                #region Mondain's Legacy Sets
                if (IsSetItem && m_SetEquipped)
                    SetHelper.RemoveSetBonus(m, SetID, this);
                #endregion

                if (m.Elemento != ElementoPvM.None)
                {
                    if (Elemento == m.Elemento && Layer != Layer.Arms && Layer != Layer.Neck)
                    {
                        var count = SetHelper.CountElemento(m, m.Elemento);
                        if (count <= 4)
                        {
                            m.SendMessage("Retirando a armadura, voce perde o poder do elemento " + m.Elemento.ToString());
                            m.Elemento = ElementoPvM.None;
                            m.FixedParticles(0x3779, 10, 15, 5002, EffectLayer.Head);
                            m.PlaySound(0x1DF);

                        }
                    }
                }
            }

            Server.Engines.XmlSpawner2.XmlAttach.CheckOnRemoved(this, parent);



            base.OnRemoved(parent);
        }

        public DateTime NextSelfRepair { get; set; }

        public virtual int OnHit(BaseWeapon weapon, int damageTaken)
        {
            var random = Utility.RandomDouble();
            int Absorbed = (int)(ArmorRating / 3);

            /*
            Shard.Debug("Random: " + random, weapon.Owner);
            Shard.Debug("Armor Rating: " + ArmorRating, weapon.Owner);
            Shard.Debug("Armor Absorb: " + Absorbed, weapon.Owner);

            damageTaken -= Absorbed;

            if (damageTaken < 0)
                damageTaken = 0;
             */
            if (Absorbed < 1)
                Absorbed = 1;

            double chance = NegativeAttributes.Antique > 0 ? 80 : 10;

            if (chance >= Utility.Random(100)) // 25% chance to lower durability
            {
                int selfRepair = !Core.AOS ? 0 : m_AosArmorAttributes.SelfRepair + (IsSetItem && m_SetEquipped ? m_SetSelfRepair : 0);

                if (selfRepair > 0 && NextSelfRepair < DateTime.UtcNow)
                {
                    HitPoints += selfRepair;

                    NextSelfRepair = DateTime.UtcNow + TimeSpan.FromSeconds(60);
                }
                else
                {
                    int wear = 1;

                    if (weapon.Type == WeaponType.Bashing)
                        wear = Absorbed / 2;

                    if (wear > 0 && m_MaxHitPoints > 0)
                    {
                        if (m_HitPoints >= wear)
                        {
                            HitPoints -= wear;
                            wear = 0;
                        }
                        else
                        {
                            wear -= HitPoints;
                            HitPoints = 0;
                        }

                        if (wear > 0)
                        {
                            if (m_MaxHitPoints > wear)
                            {
                                MaxHitPoints -= wear;

                                if (Parent is Mobile)
                                    ((Mobile)Parent).SendMessage("Sua armadura esta muito danificada"); // Your equipment is severely damaged.
                            }
                            else
                            {
                                Delete();
                            }
                        }
                    }
                }
            }

            return damageTaken;
        }

        private string GetNameString()
        {
            string name = Name;

            if (name == null)
                name = String.Format("#{0}", LabelNumber);

            return name;
        }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get
            {
                return base.Hue;
            }
            set
            {
                base.Hue = value;
                InvalidateProperties();
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            int oreType;

            switch (m_Resource)
            {

                case CraftResource.Cobre: oreType = 1053108; break; // copper
                case CraftResource.Bronze: oreType = 1053107; break; // bronze
                case CraftResource.Dourado: oreType = 1053106; break; // golden
                case CraftResource.Niobio: oreType = 1053105; break; // agapite
                case CraftResource.Lazurita: oreType = 1053104; break; // verite
                case CraftResource.Quartzo: oreType = 1053103; break; // valorite
                case CraftResource.Berilo: oreType = 1053102; break; // dull copper
                case CraftResource.Vibranium: oreType = 1053101; break; // shadow iron
                case CraftResource.CouroSpinned: oreType = 1061118; break; // spined
                case CraftResource.CouroHorned: oreType = 1061117; break; // horned
                case CraftResource.CouroBarbed: oreType = 1061116; break; // barbed
                case CraftResource.EscamasVermelhas: oreType = 1060814; break; // red
                case CraftResource.EscamasAmarelas: oreType = 1060818; break; // yellow
                case CraftResource.EscamasNegras: oreType = 1060820; break; // black
                case CraftResource.EscamasVerdes: oreType = 1060819; break; // green
                case CraftResource.EscamasBrancas: oreType = 1060821; break; // white
                case CraftResource.EscamasAzuis: oreType = 1060815; break; // blue
                case CraftResource.Carvalho: oreType = 1072533; break; // oak
                case CraftResource.Pinho: oreType = 1072534; break; // ash
                case CraftResource.Mogno: oreType = 1072535; break; // yew
                case CraftResource.Eucalipto: oreType = 1072536; break; // heartwood
                case CraftResource.Carmesim: oreType = 1072538; break; // bloodwood
                case CraftResource.Gelo: oreType = 1072539; break; // frostwood
                default: oreType = 0; break;
            }

            if (m_ReforgedPrefix != ReforgedPrefix.None || m_ReforgedSuffix != ReforgedSuffix.None)
            {
                if (m_ReforgedPrefix != ReforgedPrefix.None)
                {
                    int prefix = RunicReforging.GetPrefixName(m_ReforgedPrefix);

                    if (m_ReforgedSuffix == ReforgedSuffix.None)
                        list.Add(1151757, String.Format("#{0}\t{1}", prefix, GetNameString())); // ~1_PREFIX~ ~2_ITEM~
                    else
                        list.Add(1151756, String.Format("#{0}\t{1}\t#{2}", prefix, GetNameString(), RunicReforging.GetSuffixName(m_ReforgedSuffix))); // ~1_PREFIX~ ~2_ITEM~ of ~3_SUFFIX~
                }
                else if (m_ReforgedSuffix != ReforgedSuffix.None)
                {
                    RunicReforging.AddSuffixName(list, m_ReforgedSuffix, GetNameString());
                }
            }
            else
            {
                var nome = Name;
                if (Name != null)
                {
                    if (this.Quality == ItemQuality.Exceptional)
                        nome += " excepcional";
                    if (this.Quality == ItemQuality.Low)
                        nome += " de baixa qualidade";
                    if (m_Resource != CraftResource.Ferro && m_Resource != CraftResource.RegularLeather && m_Resource != CraftResource.Cedro)
                        nome += " de " + m_Resource;
                }

                if (Name == null)
                    list.Add(LabelNumber);
                else
                    list.Add(nome);
            }

            if (!String.IsNullOrEmpty(_EngravedText))
            {
                list.AddTwoValues(Gump.Cor("Escrito", "orange"), Gump.Cor(Utility.FixHtml(_EngravedText), "orange"));
            }
        }

        public override bool AllowEquipedCast(Mobile from)
        {
            if (base.AllowEquipedCast(from))
                return true;

            return (m_AosAttributes.SpellChanneling != 0 || Enhancement.GetValue(from, AosAttribute.SpellChanneling) != 0);
        }

        public virtual int GetLuckBonus()
        {
            CraftAttributeInfo attrInfo = GetResourceAttrs(Resource);

            if (attrInfo == null || Resource == CraftResource.Eucalipto)
                return 0;

            return attrInfo.ArmorLuck;
        }

        public override void AddWeightProperty(ObjectPropertyList list)
        {
            base.AddWeightProperty(list);

            if (IsVvVItem)
                list.Add(1154937); // VvV Item
        }

        public virtual void AddDamageTypeProperty(ObjectPropertyList list)
        {
        }

        public bool Magico()
        {
            return m_Durability != ArmorDurabilityLevel.Regular || m_Protection != ArmorProtectionLevel.Regular;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (!Identified)
            {
                if (Magico())
                    list.Add("[ Nao Identificado ]");
            }
            else
            {
                if (ProtectionLevel > 0)
                {
                    list.Add("Fortificado +" + (int)ProtectionLevel);
                }
                if (Durability > 0)
                {
                    list.Add("Durabilidade +" + (int)Durability);
                }
            }

            var ar = ArmorRatingScaled;
            if (ar > 0)
                list.AddTwoValues("Armadura", ar.ToString("0.0"));

            if (m_Crafter != null)
                list.AddTwoValues("Feito por ", m_Crafter.TitleName); // crafted by ~1_NAME~

            if (StrRequirement > 0)
                list.Add("Forca necessaria: " + StrRequirement);

            if (MaxHitPoints > 0)
            {
                double durabPct = HitPoints / (double)MaxHitPoints;
                if (durabPct < 0.2)
                {
                    list.Add("Pessimo estado");
                }
                else if (durabPct < 0.5)
                {
                    list.Add("Mau estado");
                }
                else if (durabPct < 0.75)
                {
                    list.Add("Bom estado");
                }
                else if (durabPct < 0.99)
                {
                    list.Add("Quase novo");
                }
                else if (durabPct >= 1)
                {
                    list.Add("Novo");
                }
            }

            if (m_GorgonLenseCharges > 0)
                list.AddTwoValues("Cargas Gorgon", m_GorgonLenseCharges); //Gorgon Lens Charges: ~1_val~         

            AddResistanceProperties(list);

            if (RP)
                list.Add("[ RP ]");

            if (!Core.AOS)
                return;

            if (OwnerName != null)
            {
                list.Add(1153213, OwnerName);
            }

            //if (m_Crafter != null)
            //    list.Add(1050043, m_Crafter.TitleName); // crafted by ~1_NAME~

            //if (m_Quality == ItemQuality.Exceptional)
            //    list.Add(1060636); // Exceptional

            if (IsImbued)
                list.Add(1080418); // (Imbued)

            if (m_Altered)
                list.Add(1111880); // Altered

            #region Factions
            FactionEquipment.AddFactionProperties(this, list);
            #endregion

            //if (m_GorgonLenseCharges > 0)
            //    list.Add(1112590, m_GorgonLenseCharges.ToString()); //Gorgon Lens Charges: ~1_val~         

            #region Mondain's Legacy Sets
            if (IsSetItem)
            {
                if (MixedSet)
                    list.Add(1073491, Pieces.ToString()); // Part of a Weapon/Armor Set (~1_val~ pieces)
                else
                    list.Add(1072376, Pieces.ToString()); // Part of an Armor Set (~1_val~ pieces)

                if (SetID == SetItem.Bestial)
                    list.Add(1151541, BestialSetHelper.GetTotalBerserk(this).ToString()); // Berserk ~1_VAL~

                if (BardMasteryBonus)
                    list.Add(1151553); // Activate: Bard Mastery Bonus x2<br>(Effect: 1 min. Cooldown: 30 min.)

                if (m_SetEquipped)
                {
                    if (MixedSet)
                        list.Add(1073492); // Full Weapon/Armor Set Present
                    else
                        list.Add(1072377); // Full Armor Set Present

                    GetSetProperties(list);
                }
            }
            #endregion

            AddDamageTypeProperty(list);

            if (RequiredRace == Race.Elf)
                list.Add(1075086); // Elves Only
            else if (RequiredRace == Race.Gargoyle)
                list.Add(1111709); // Gargoyles Only

            if (this is SurgeShield && ((SurgeShield)this).Surge > SurgeType.None)
                list.Add(1116176 + ((int)((SurgeShield)this).Surge));

            m_NegativeAttributes.GetProperties(list, this);
            m_AosSkillBonuses.GetProperties(list);

            int prop;

            if ((prop = ArtifactRarity) > 0)
                list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

            if (m_TalismanProtection != null && !m_TalismanProtection.IsEmpty && m_TalismanProtection.Amount > 0)
                list.Add(1072387, "{0}\t{1}", m_TalismanProtection.Name != null ? m_TalismanProtection.Name.ToString() : "Unknown", m_TalismanProtection.Amount); // ~1_NAME~ Protection: +~2_val~%

            if ((prop = m_AosArmorAttributes.SoulCharge) != 0)
                list.Add(1113630, prop.ToString()); // Soul Charge ~1_val~%

            if ((prop = m_AosArmorAttributes.ReactiveParalyze) != 0)
                list.Add(1112364); // reactive paralyze

            if ((prop = m_SAAbsorptionAttributes.EaterFire) != 0)
                list.Add(1113593, prop.ToString()); // Fire Eater ~1_Val~%

            if ((prop = m_SAAbsorptionAttributes.EaterCold) != 0)
                list.Add(1113594, prop.ToString()); // Cold Eater ~1_Val~%

            if ((prop = m_SAAbsorptionAttributes.EaterPoison) != 0)
                list.Add(1113595, prop.ToString()); // Poison Eater ~1_Val~%

            if ((prop = m_SAAbsorptionAttributes.EaterEnergy) != 0)
                list.Add(1113596, prop.ToString()); // Energy Eater ~1_Val~%

            if ((prop = m_SAAbsorptionAttributes.EaterKinetic) != 0)
                list.Add(1113597, prop.ToString()); // Kinetic Eater ~1_Val~%

            if ((prop = m_SAAbsorptionAttributes.EaterDamage) != 0)
                list.Add(1113598, prop.ToString()); // Damage Eater ~1_Val~%

            if ((prop = m_SAAbsorptionAttributes.ResonanceFire) != 0)
                list.Add(1113691, prop.ToString()); // Fire Resonance ~1_val~%

            if ((prop = m_SAAbsorptionAttributes.ResonanceCold) != 0)
                list.Add(1113692, prop.ToString()); // Cold Resonance ~1_val~%

            if ((prop = m_SAAbsorptionAttributes.ResonancePoison) != 0)
                list.Add(1113693, prop.ToString()); // Poison Resonance ~1_val~%

            if ((prop = m_SAAbsorptionAttributes.ResonanceEnergy) != 0)
                list.Add(1113694, prop.ToString()); // Energy Resonance ~1_val~%

            if ((prop = m_SAAbsorptionAttributes.ResonanceKinetic) != 0)
                list.Add(1113695, prop.ToString()); // Kinetic Resonance ~1_val~%

            if ((prop = m_SAAbsorptionAttributes.CastingFocus) != 0)
                list.Add(1113696, prop.ToString()); // Casting Focus ~1_val~%

            if ((prop = m_AosAttributes.SpellChanneling) != 0)
                list.Add(1060482); // spell channeling

            if ((prop = m_AosArmorAttributes.SelfRepair) != 0)
                list.Add(1060450, prop.ToString()); // self repair ~1_val~

            if ((prop = m_AosAttributes.NightSight) != 0)
                list.Add(1060441); // night sight

            if ((prop = m_AosAttributes.BonusStr) != 0)
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

            if ((prop = m_AosAttributes.BonusDex) != 0)
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

            if ((prop = m_AosAttributes.BonusInt) != 0)
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

            if ((prop = m_AosAttributes.BonusHits) != 0)
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

            if ((prop = m_AosAttributes.BonusStam) != 0)
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

            if ((prop = m_AosAttributes.BonusMana) != 0)
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~

            if ((prop = m_AosAttributes.RegenHits) != 0)
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

            if ((prop = m_AosAttributes.RegenStam) != 0)
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

            if ((prop = m_AosAttributes.RegenMana) != 0)
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~


            if ((prop = m_AosAttributes.EnhancePotions) != 0)
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

            if ((prop = m_AosAttributes.ReflectPhysical) != 0)
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

            if (this is SurgeShield && ((SurgeShield)this).Surge > SurgeType.None)
                list.Add(1153098, ((SurgeShield)this).Charges.ToString());

            if ((prop = m_AosAttributes.AttackChance) != 0)
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

            if ((prop = m_AosAttributes.WeaponSpeed) != 0)
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

            if ((prop = m_AosAttributes.WeaponDamage) != 0)
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

            if ((prop = m_AosAttributes.DefendChance) != 0)
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

            if ((prop = m_AosAttributes.CastRecovery) != 0)
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

            if ((prop = m_AosAttributes.Resistence) != 0)
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~

            if ((prop = m_AosAttributes.SpellDamage) != 0)
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

            if ((prop = m_AosAttributes.LowerManaCost) != 0)
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

            if ((prop = m_AosAttributes.LowerRegCost) != 0)
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

            if (Core.ML && (prop = m_AosAttributes.IncreasedKarmaLoss) != 0)
                list.Add(1075210, prop.ToString()); // Increased Karma Loss ~1val~%

            AddResistanceProperties(list);

            if ((prop = m_AosArmorAttributes.MageArmor) != 0)
                list.Add(1060437); // mage armor

            if ((prop = GetLowerStatReq()) != 0)
                list.Add(1060435, prop.ToString()); // lower requirements ~1_val~%

            if ((prop = ComputeStatReq(StatType.Str)) > 0)
                list.Add(1061170, prop.ToString()); // strength requirement ~1_val~

            if (m_HitPoints >= 0 && m_MaxHitPoints > 0)
                list.Add(1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints); // durability ~1_val~ / ~2_val~

            Server.Engines.XmlSpawner2.XmlAttach.AddAttachmentProperties(this, list);

            if (IsSetItem && !m_SetEquipped)
            {
                list.Add(1072378); // <br>Only when full set is present:				
                GetSetProperties(list);
            }
        }

        public static string CorElemento(ElementoPvM elemento)
        {
            switch (elemento)
            {
                case ElementoPvM.Agua: return "blue";
                case ElementoPvM.Escuridao: return "gray";
                case ElementoPvM.Fogo: return "orange";
                case ElementoPvM.Gelo: return "cyan";
                case ElementoPvM.Vento: return "yellow";
                case ElementoPvM.Raio: return "purple";
                case ElementoPvM.Luz: return "white";
                case ElementoPvM.Terra: return "brown";
                default: return "white";
            }
        }

        public static int HueElemento(ElementoPvM elemento)
        {
            switch (elemento)
            {
                case ElementoPvM.Agua: return 1264;
                case ElementoPvM.Escuridao: return 2022;
                case ElementoPvM.Fogo: return 1258;
                case ElementoPvM.Gelo: return 1266;
                case ElementoPvM.Vento: return 1281;
                case ElementoPvM.Raio: return 1275;
                case ElementoPvM.Luz: return 1153;
                case ElementoPvM.Terra: return 632;
                default: return 0;
            }
        }

        public override void AddItemPowerProperties(ObjectPropertyList list)
        {
            if (m_elementoPvm != ElementoPvM.None)
            {
                list.AddTwoValues("Elemento", Gump.Cor(m_elementoPvm.ToString(), CorElemento(m_elementoPvm)));
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            List<EquipInfoAttribute> attrs = new List<EquipInfoAttribute>();

            if (DisplayLootType)
            {
                if (LootType == LootType.Blessed)
                    attrs.Add(new EquipInfoAttribute(1038021)); // blessed
                else if (LootType == LootType.Cursed)
                    attrs.Add(new EquipInfoAttribute(1049643)); // cursed
            }

            #region Factions
            if (m_FactionState != null)
                attrs.Add(new EquipInfoAttribute(1041350)); // faction item
            #endregion

            if (m_Quality == ItemQuality.Exceptional)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)m_Quality));

            if (m_Identified || from.AccessLevel >= AccessLevel.GameMaster)
            {
                if (m_Durability != ArmorDurabilityLevel.Regular)
                    attrs.Add(new EquipInfoAttribute(1038000 + (int)m_Durability));

                if (m_Protection > ArmorProtectionLevel.Regular && m_Protection <= ArmorProtectionLevel.Invulnerability)
                    attrs.Add(new EquipInfoAttribute(1038005 + (int)m_Protection));
            }
            else if (m_Durability != ArmorDurabilityLevel.Regular || (m_Protection > ArmorProtectionLevel.Regular && m_Protection <= ArmorProtectionLevel.Invulnerability))
                attrs.Add(new EquipInfoAttribute(1038000)); // Unidentified

            int number;

            if (Name == null)
            {
                number = LabelNumber;
            }
            else
            {
                LabelTo(from, Name);
                number = 1041000;
            }

            if (attrs.Count == 0 && Crafter == null && Name != null)
                return;

            EquipmentInfo eqInfo = new EquipmentInfo(number, m_Crafter, false, attrs.ToArray());

            from.Send(new DisplayEquipmentInfo(this, eqInfo));
        }

        public override bool DropToWorld(Mobile from, Point3D p)
        {
            bool drop = base.DropToWorld(from, p);

            EnchantedHotItemSocket.CheckDrop(from, this);

            return drop;
        }

        #region ICraftable Members

        public virtual int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            Quality = (ItemQuality)quality;

            if (makersMark)
                Crafter = from;

            #region Mondain's Legacy
            if (!Core.ML || !craftItem.ForceNonExceptional)
            {
                if (typeRes == null)
                    typeRes = craftItem.Resources.GetAt(0).ItemType;

                Resource = CraftResources.GetFromType(typeRes);
            }
            #endregion

            if (typeRes == null || craftItem.ForceNonExceptional)
                typeRes = craftItem.Resources.GetAt(0).ItemType;

            PlayerConstructed = true;

            if (Quality == ItemQuality.Exceptional && !craftItem.ForceNonExceptional)
            {
                DistributeExceptionalBonuses(from, (tool is BaseRunicTool ? 6 : Core.SE ? 15 : 14)); // Not sure since when, but right now 15 points are added, not 14.
            }

            if (Core.AOS && tool is BaseRunicTool && !craftItem.ForceNonExceptional)
                ((BaseRunicTool)tool).ApplyAttributesTo(this);

            #region Mondain's Legacy
            if (Core.ML && !craftItem.ForceNonExceptional)
            {
                CraftResourceInfo resInfo = CraftResources.GetInfo(m_Resource);

                if (resInfo == null)
                    return quality;

                CraftAttributeInfo attrInfo = resInfo.AttributeInfo;

                if (attrInfo == null)
                    return quality;

                DistributeMaterialBonus(attrInfo);
            }
            #endregion

            return quality;
        }

        public virtual void DistributeExceptionalBonuses(Mobile from, int amount)
        {
            // Exceptional Bonus
            for (int i = 0; i < amount; ++i)
            {
                switch (Utility.Random(5))
                {
                    case 0: ++m_PhysicalBonus; break;
                    case 1: ++m_FireBonus; break;
                    case 2: ++m_ColdBonus; break;
                    case 3: ++m_PoisonBonus; break;
                    case 4: ++m_EnergyBonus; break;
                }
            }

            // Arms Lore Bonus
            if (Core.ML && from != null)
            {
                double div = Siege.SiegeShard ? 12.5 : 20;
                int bonus = (int)(from.Skills.ArmsLore.Value / div);

                for (int i = 0; i < bonus; i++)
                {
                    switch (Utility.Random(5))
                    {
                        case 0: m_PhysicalBonus++; break;
                        case 1: m_FireBonus++; break;
                        case 2: m_ColdBonus++; break;
                        case 3: m_EnergyBonus++; break;
                        case 4: m_PoisonBonus++; break;
                    }
                }

                from.CheckSkillMult(SkillName.ArmsLore, 0, 100);
            }

            // Imbuing needs to keep track of what is natrual, what is imbued bonuses
            #region Stygian Abyss
            m_PhysNonImbuing = m_PhysicalBonus;
            m_FireNonImbuing = m_FireBonus;
            m_ColdNonImbuing = m_ColdBonus;
            m_PoisonNonImbuing = m_PoisonBonus;
            m_EnergyNonImbuing = m_EnergyBonus;
            #endregion

            // Gives MageArmor property for certain armor types
            if (Core.SA && m_AosArmorAttributes.MageArmor <= 0 && IsMageArmorType(this))
            {
                m_AosArmorAttributes.MageArmor = 1;
            }

            InvalidateProperties();
        }

        protected virtual void ApplyResourceResistances(CraftResource oldResource)
        {
            CraftAttributeInfo info;

            if (oldResource > CraftResource.None)
            {
                info = GetResourceAttrs(oldResource);

                // Remove old bonus
                m_PhysicalBonus = Math.Max(0, m_PhysicalBonus - info.ArmorPhysicalResist);
                m_FireBonus = Math.Max(0, m_FireBonus - info.ArmorFireResist);
                m_ColdBonus = Math.Max(0, m_ColdBonus - info.ArmorColdResist);
                m_PoisonBonus = Math.Max(0, m_PoisonBonus - info.ArmorPoisonResist);
                m_EnergyBonus = Math.Max(0, m_EnergyBonus - info.ArmorEnergyResist);

                m_PhysNonImbuing = Math.Max(0, PhysNonImbuing - info.ArmorPhysicalResist);
                m_FireNonImbuing = Math.Max(0, m_FireNonImbuing - info.ArmorFireResist);
                m_ColdNonImbuing = Math.Max(0, m_ColdNonImbuing - info.ArmorColdResist);
                m_PoisonNonImbuing = Math.Max(0, m_PoisonNonImbuing - info.ArmorPoisonResist);
                m_EnergyNonImbuing = Math.Max(0, m_EnergyNonImbuing - info.ArmorEnergyResist);
            }

            info = GetResourceAttrs(m_Resource);

            // add new bonus
            m_PhysicalBonus += info.ArmorPhysicalResist;
            m_FireBonus += info.ArmorFireResist;
            m_ColdBonus += info.ArmorColdResist;
            m_PoisonBonus += info.ArmorPoisonResist;
            m_EnergyBonus += info.ArmorEnergyResist;

            m_PhysNonImbuing += info.ArmorPhysicalResist;
            m_FireNonImbuing += info.ArmorFireResist;
            m_ColdNonImbuing += info.ArmorColdResist;
            m_PoisonNonImbuing += info.ArmorPoisonResist;
            m_EnergyNonImbuing += info.ArmorEnergyResist;
        }

        public virtual void DistributeMaterialBonus(CraftAttributeInfo attrInfo)
        {
            if (m_Resource != CraftResource.Eucalipto)
            {
                m_AosAttributes.WeaponDamage += attrInfo.ArmorDamage;
                m_AosAttributes.AttackChance += attrInfo.ArmorHitChance;
                m_AosAttributes.RegenHits += attrInfo.ArmorRegenHits;
                //m_AosArmorAttributes.MageArmor += attrInfo.ArmorMage;
            }
            else
            {
                switch (Utility.Random(4))
                {
                    case 0: m_AosAttributes.WeaponDamage += attrInfo.ArmorDamage; break;
                    case 1: m_AosAttributes.AttackChance += attrInfo.ArmorHitChance; break;
                    //case 2: m_AosArmorAttributes.MageArmor += attrInfo.ArmorMage; break;
                    case 2: m_AosAttributes.Luck += attrInfo.ArmorLuck; break;
                    case 3: m_AosArmorAttributes.LowerStatReq += attrInfo.ArmorLowerRequirements; break;
                }
            }
        }

        public CraftAttributeInfo GetResourceAttrs(CraftResource res)
        {
            CraftResourceInfo info = CraftResources.GetInfo(res);

            if (info == null)
                return CraftAttributeInfo.Blank;

            return info.AttributeInfo;
        }

        public static bool IsMageArmorType(BaseArmor armor)
        {
            Type t = armor.GetType();

            foreach (Type type in _MageArmorTypes)
            {
                if (type == t || t.IsSubclassOf(type))
                {
                    return true;
                }
            }

            return false;
        }

        public static Type[] _MageArmorTypes = new Type[]
        {
            typeof(HeavyPlateJingasa),  typeof(LightPlateJingasa),
            typeof(PlateMempo),         typeof(PlateDo),
            typeof(PlateHiroSode),      typeof(PlateSuneate),
            typeof(PlateHaidate)
        };
        #endregion

        #region Mondain's Legacy Sets
        public override bool OnDragLift(Mobile from)
        {
            if (Parent is Mobile && from == Parent)
            {
                if (IsSetItem && m_SetEquipped)
                    SetHelper.RemoveSetBonus(from, SetID, this);

                

            }

            return base.OnDragLift(from);
        }

        public virtual SetItem SetID
        {
            get
            {
                return SetItem.None;
            }
        }
        public virtual bool MixedSet
        {
            get
            {
                return false;
            }
        }
        public virtual int Pieces
        {
            get
            {
                return 0;
            }
        }

        public virtual bool BardMasteryBonus
        {
            get
            {
                return (SetID == SetItem.Virtuoso);
            }
        }

        public bool IsSetItem
        {
            get
            {
                return (SetID != SetItem.None);
            }
        }

        private int m_SetHue;
        private bool m_SetEquipped;
        private bool m_LastEquipped;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetHue
        {
            get
            {
                return m_SetHue;
            }
            set
            {
                m_SetHue = value;
                InvalidateProperties();
            }
        }

        public bool SetEquipped
        {
            get
            {
                return m_SetEquipped;
            }
            set
            {
                m_SetEquipped = value;
            }
        }

        public bool LastEquipped
        {
            get
            {
                return m_LastEquipped;
            }
            set
            {
                m_LastEquipped = value;
            }
        }

        private AosAttributes m_SetAttributes;
        private AosSkillBonuses m_SetSkillBonuses;
        private int m_SetSelfRepair;

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes SetAttributes
        {
            get
            {
                return m_SetAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SetSkillBonuses
        {
            get
            {
                return m_SetSkillBonuses;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetSelfRepair
        {
            get
            {
                return m_SetSelfRepair;
            }
            set
            {
                m_SetSelfRepair = value;
                InvalidateProperties();
            }
        }

        private int m_SetPhysicalBonus, m_SetFireBonus, m_SetColdBonus, m_SetPoisonBonus, m_SetEnergyBonus;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetPhysicalBonus
        {
            get
            {
                return m_SetPhysicalBonus;
            }
            set
            {
                m_SetPhysicalBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetFireBonus
        {
            get
            {
                return m_SetFireBonus;
            }
            set
            {
                m_SetFireBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetColdBonus
        {
            get
            {
                return m_SetColdBonus;
            }
            set
            {
                m_SetColdBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetPoisonBonus
        {
            get
            {
                return m_SetPoisonBonus;
            }
            set
            {
                m_SetPoisonBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetEnergyBonus
        {
            get
            {
                return m_SetEnergyBonus;
            }
            set
            {
                m_SetEnergyBonus = value;
                InvalidateProperties();
            }
        }

        public virtual void GetSetProperties(ObjectPropertyList list)
        {
            SetHelper.GetSetProperties(list, this);

            if (!m_SetEquipped)
            {
                if (m_SetPhysicalBonus != 0)
                    list.Add(1072382, m_SetPhysicalBonus.ToString()); // physical resist +~1_val~%

                if (m_SetFireBonus != 0)
                    list.Add(1072383, m_SetFireBonus.ToString()); // fire resist +~1_val~%

                if (m_SetColdBonus != 0)
                    list.Add(1072384, m_SetColdBonus.ToString()); // cold resist +~1_val~%

                if (m_SetPoisonBonus != 0)
                    list.Add(1072385, m_SetPoisonBonus.ToString()); // poison resist +~1_val~%

                if (m_SetEnergyBonus != 0)
                    list.Add(1072386, m_SetEnergyBonus.ToString()); // energy resist +~1_val~%		
            }
            else if (m_SetEquipped && SetHelper.ResistsBonusPerPiece(this) && RootParent is Mobile)
            {
                Mobile m = (Mobile)RootParent;

                if (m_SetPhysicalBonus != 0)
                    list.Add(1080361, SetHelper.GetSetTotalResist(m, ResistanceType.Physical).ToString()); // physical resist ~1_val~% (total)

                if (m_SetFireBonus != 0)
                    list.Add(1080362, SetHelper.GetSetTotalResist(m, ResistanceType.Fire).ToString()); // fire resist ~1_val~% (total)

                if (m_SetColdBonus != 0)
                    list.Add(1080363, SetHelper.GetSetTotalResist(m, ResistanceType.Cold).ToString()); // cold resist ~1_val~% (total)

                if (m_SetPoisonBonus != 0)
                    list.Add(1080364, SetHelper.GetSetTotalResist(m, ResistanceType.Poison).ToString()); // poison resist ~1_val~% (total)

                if (m_SetEnergyBonus != 0)
                    list.Add(1080365, SetHelper.GetSetTotalResist(m, ResistanceType.Energy).ToString()); // energy resist ~1_val~% (total)
            }
            else
            {
                if (this.m_SetPhysicalBonus != 0)
                    list.Add(1080361, ((BasePhysicalResistance * Pieces) + m_SetPhysicalBonus).ToString()); // physical resist ~1_val~% (total)

                if (this.m_SetFireBonus != 0)
                    list.Add(1080362, ((BaseFireResistance * Pieces) + m_SetFireBonus).ToString()); // fire resist ~1_val~% (total)

                if (this.m_SetColdBonus != 0)
                    list.Add(1080363, ((BaseColdResistance * Pieces) + m_SetColdBonus).ToString()); // cold resist ~1_val~% (total)

                if (this.m_SetPoisonBonus != 0)
                    list.Add(1080364, ((BasePoisonResistance * Pieces) + m_SetPoisonBonus).ToString()); // poison resist ~1_val~% (total)

                if (this.m_SetEnergyBonus != 0)
                    list.Add(1080365, ((BaseEnergyResistance * Pieces) + m_SetEnergyBonus).ToString()); // energy resist ~1_val~% (total)
            }

            int prop;

            if ((prop = m_SetSelfRepair) != 0 && m_AosArmorAttributes.SelfRepair == 0)
                list.Add(1060450, prop.ToString()); // self repair ~1_val~
        }

        public int SetResistBonus(ResistanceType resist)
        {
            if (SetHelper.ResistsBonusPerPiece(this))
            {
                switch (resist)
                {
                    case ResistanceType.Physical: return m_SetEquipped ? PhysicalResistance + m_SetPhysicalBonus : PhysicalResistance;
                    case ResistanceType.Fire: return m_SetEquipped ? FireResistance + m_SetFireBonus : FireResistance;
                    case ResistanceType.Cold: return m_SetEquipped ? ColdResistance + m_SetColdBonus : ColdResistance;
                    case ResistanceType.Poison: return m_SetEquipped ? PoisonResistance + m_SetPoisonBonus : PoisonResistance;
                    case ResistanceType.Energy: return m_SetEquipped ? EnergyResistance + m_SetEnergyBonus : EnergyResistance;
                }
            }
            else
            {
                switch (resist)
                {
                    case ResistanceType.Physical: return m_SetEquipped ? LastEquipped ? (PhysicalResistance * Pieces) + m_SetPhysicalBonus : 0 : PhysicalResistance;
                    case ResistanceType.Fire: return m_SetEquipped ? LastEquipped ? (FireResistance * Pieces) + m_SetFireBonus : 0 : FireResistance;
                    case ResistanceType.Cold: return m_SetEquipped ? LastEquipped ? (ColdResistance * Pieces) + m_SetColdBonus : 0 : ColdResistance;
                    case ResistanceType.Poison: return m_SetEquipped ? LastEquipped ? (PoisonResistance * Pieces) + m_SetPoisonBonus : 0 : PoisonResistance;
                    case ResistanceType.Energy: return m_SetEquipped ? LastEquipped ? (EnergyResistance * Pieces) + m_SetEnergyBonus : 0 : EnergyResistance;
                }
            }

            return 0;
        }
        #endregion

        public virtual void SetProtection(Type type, TextDefinition name, int amount)
        {
            m_TalismanProtection = new TalismanAttribute(type, name, amount);
        }

        public static void Initialize()
        {
            EventSink.Login += (ev) =>
            {
                var armor = ev.Mobile.ChestArmor as BaseArmor;
                if (armor != null && armor.Elemento != ElementoPvM.None && ev.Mobile.Elemento != armor.Elemento)
                {
                    var count = SetHelper.CountElemento(ev.Mobile, armor.Elemento);
                    if (count >= 4)
                    {
                        ev.Mobile.Elemento = armor.Elemento;
                    }
                }
            };
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Altered
        {
            get { return m_Altered; }
            set
            {
                m_Altered = value;
                InvalidateProperties();
            }
        }
    }
}
