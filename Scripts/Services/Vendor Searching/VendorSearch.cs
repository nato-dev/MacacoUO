using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.SkillHandlers;
using Server.ContextMenus;
using Server.Commands;
using Server.Targeting;
using Server.Regions;
using Server.Gumps;

namespace Server.Engines.VendorSearching
{
	public class VendorSearch
	{
        public static string FilePath = Path.Combine("Saves/Misc", "VendorSearch.bin");
        public static StringList StringList { get; private set; }

        public static List<VendorItem> DoSearch(Mobile m, SearchCriteria criteria)
        {
            if (criteria == null || PlayerVendor.PlayerVendors == null || PlayerVendor.PlayerVendors.Count == 0)
                return null;

            List<VendorItem> list = new List<VendorItem>();
            bool excludefel = criteria.Details.FirstOrDefault(d => d.Attribute is Misc && (Misc)d.Attribute == Misc.ExcludeFel) != null;

            foreach (PlayerVendor pv in PlayerVendor.PlayerVendors.Where(pv => pv.Map != Map.Internal &&
                                                                               pv.Map != null &&
                                                                               pv.Backpack != null && 
                                                                              pv.Backpack.Items.Count > 0 && 
                                                                              (!excludefel || pv.Map != Map.Felucca)))
            {
                List<Item> items = GetItems(pv);

                foreach (Item item in items.Where(it => CheckMatch(pv.GetVendorItem(it), criteria)))
                {
                    list.Add(pv.GetVendorItem(item));
                }

                items.Clear();
                items.TrimExcess();
            }

            switch (criteria.SortBy)
            {
                case SortBy.LowToHigh: list = list.OrderBy(vi => vi.Price).ToList(); break;
                case SortBy.HighToLow: list = list.OrderBy(vi => -vi.Price).ToList(); break;
            }

            return list;
        }
		
		public static bool CheckMatch(VendorItem vitem, SearchCriteria searchCriteria)
		{
			if(vitem == null)
				return false;
			
			Item item = vitem.Item;

            if (searchCriteria.MinPrice > -1 && vitem.Price < searchCriteria.MinPrice)
				return false;

            if (searchCriteria.MaxPrice > -1 && vitem.Price > searchCriteria.MaxPrice)
				return false;
			
			if (!String.IsNullOrEmpty(searchCriteria.SearchName))
			{
				string name = GetItemName(item);
				
				if(name == null)
				{
                    return false; // TODO? REturn null names?
				}

                if (!CheckKeyword(searchCriteria.SearchName, item) && name.ToLower().IndexOf(searchCriteria.SearchName.ToLower()) < 0)
                {
                    return false;
                }
			}

            if (searchCriteria.SearchType != Layer.Invalid && searchCriteria.SearchType != item.Layer)
			{
				return false;
			}
			
			if(searchCriteria.Details.Count == 0)
				return true;
			
            foreach(SearchDetail detail in searchCriteria.Details)
			{
                object o = detail.Attribute;
                int value = detail.Value;

                if (value == 0)
                    value = 1;

				if(o is AosAttribute)
				{
					AosAttributes attrs = RunicReforging.GetAosAttributes(item);

					if(attrs == null || attrs[(AosAttribute)o] < value)
						return false;
				}
				else if (o is AosWeaponAttribute)
				{
					AosWeaponAttributes attrs = RunicReforging.GetAosWeaponAttributes(item);

                    if ((AosWeaponAttribute)o == AosWeaponAttribute.MageWeapon)
                    {
                        if (attrs == null || attrs[(AosWeaponAttribute)o] == 0 || attrs[(AosWeaponAttribute)o] > Math.Max(0, 30 - value))
                            return false;
                    }
                    else if (attrs == null || attrs[(AosWeaponAttribute)o] < value)
                        return false;
				}
				else if (o is SAAbsorptionAttribute)
				{
					SAAbsorptionAttributes attrs = RunicReforging.GetSAAbsorptionAttributes(item);

                    if (attrs == null || attrs[(SAAbsorptionAttribute)o] < value)
                        return false;
				}
				else if(o is AosArmorAttribute)
				{
					AosArmorAttributes attrs = RunicReforging.GetAosArmorAttributes(item);

                    if (attrs == null || attrs[(AosArmorAttribute)o] < value)
                        return false;
				}
				else if(o is SkillName)
				{
					if(detail.Category != Category.RequiredSkill)
					{
						AosSkillBonuses skillbonuses = RunicReforging.GetAosSkillBonuses(item);

                        if (skillbonuses != null)
                        {
                            bool hasSkill = false;

                            for (int i = 0; i < 5; i++)
                            {
                                SkillName check;
                                double bonus;

                                if (skillbonuses.GetValues(i, out check, out bonus) && check == (SkillName)o && bonus >= value)
                                {
                                    hasSkill = true;
                                    break;
                                }
                            }

                            if (!hasSkill)
                                return false;
                        }
                        else if (item is SpecialScroll && value >= 105)
                        {
                            if (((SpecialScroll)item).Skill != (SkillName)o || ((SpecialScroll)item).Value < value)
                                return false;
                        }
                        else
                        {
                            return false;
                        }
					}
					else if(!(item is BaseWeapon) || ((BaseWeapon)item).DefSkill != (SkillName)o)
					{
						return false;
					}
				}
                else if (!CheckSlayer(item, o))
                {
                    return false;
                }
                else if (o is AosElementAttribute)
                {
                    if (item is BaseWeapon)
                    {
                        BaseWeapon wep = item as BaseWeapon;

                        if (detail.Category == Category.DamageType)
                        {
                            int phys, fire, cold, pois, nrgy, chaos, direct;
                            wep.GetDamageTypes(null, out phys, out fire, out cold, out pois, out nrgy, out chaos, out direct);

                            switch ((AosElementAttribute)o)
                            {
                                case AosElementAttribute.Physical: if (phys < value) return false; break;
                                case AosElementAttribute.Fire: if (fire < value) return false; break;
                                case AosElementAttribute.Cold: if (cold < value) return false; break;
                                case AosElementAttribute.Poison: if (pois < value) return false; break;
                                case AosElementAttribute.Energy: if (nrgy < value) return false; break;
                                case AosElementAttribute.Chaos: if (chaos < value) return false; break;
                                case AosElementAttribute.Direct: if (direct < value) return false; break;
                            }
                        }
                        else
                        {
                            switch ((AosElementAttribute)o)
                            {
                                case AosElementAttribute.Physical:
                                    if (wep.WeaponAttributes.ResistPhysicalBonus < value) return false;
                                    break;
                                case AosElementAttribute.Fire:
                                    if (wep.WeaponAttributes.ResistFireBonus < value) return false;
                                    break;
                                case AosElementAttribute.Cold:
                                    if (wep.WeaponAttributes.ResistColdBonus < value) return false;
                                    break;
                                case AosElementAttribute.Poison:
                                    if (wep.WeaponAttributes.ResistPoisonBonus < value) return false;
                                    break;
                                case AosElementAttribute.Energy:
                                    if (wep.WeaponAttributes.ResistEnergyBonus < value) return false;
                                    break;
                            }
                        }
                    }
                    else if (item is BaseArmor && detail.Category == Category.Resists)
                    {
                        BaseArmor armor = item as BaseArmor;

                        switch ((AosElementAttribute)o)
                        {
                            case AosElementAttribute.Physical:
                                if (armor.PhysicalResistance < value) return false;
                                break;
                            case AosElementAttribute.Fire:
                                if (armor.FireResistance < value) return false;
                                break;
                            case AosElementAttribute.Cold:
                                if (armor.ColdResistance < value) return false;
                                break;
                            case AosElementAttribute.Poison:
                                if (armor.PoisonResistance < value) return false;
                                break;
                            case AosElementAttribute.Energy:
                                if (armor.EnergyResistance < value) return false;
                                break;
                        }
                    }
                    else if (detail.Category != Category.DamageType)
                    {
                        AosElementAttributes attrs = RunicReforging.GetElementalAttributes(item);

                        if (attrs == null || attrs[(AosElementAttribute)o] < value)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (o is Misc)
                {
                    switch ((Misc)o)
                    {
                        case Misc.ExcludeFel: break;
                        case Misc.GargoyleOnly:
                            if (!IsGargoyle(item))
                                return false;
                            break;
                        case Misc.NotGargoyleOnly:
                            if (IsGargoyle(item))
                                return false;
                            break;
                        case Misc.ElvesOnly:
                            if (!IsElf(item))
                                return false;
                            break;
                        case Misc.NotElvesOnly:
                            if (IsElf(item))
                                return false;
                            break;
                        case Misc.FactionItem:
                            if (!(item is Server.Factions.IFactionItem))
                                return false;
                            break;
                        case Misc.PromotionalToken:
                            if (!(item is PromotionalToken))
                                return false;
                            break;
                        case Misc.Cursed:
                            if (item.LootType != LootType.Cursed)
                                return false;
                            break;
                        case Misc.NotCursed:
                            if (item.LootType == LootType.Cursed)
                                return false;
                            break;
                        case Misc.CannotRepair:
                            if (CheckCanRepair(item))
                                return false;
                            break;
                        case Misc.NotCannotBeRepaired:
                            if (!CheckCanRepair(item))
                                return false;
                            break;
                        case Misc.Brittle:
                            NegativeAttributes neg2 = RunicReforging.GetNegativeAttributes(item);
                            if (neg2 == null || neg2.Brittle == 0)
                                return false;
                            break;
                        case Misc.NotBrittle:
                            NegativeAttributes neg3 = RunicReforging.GetNegativeAttributes(item);
                            if (neg3 != null && neg3.Brittle > 0)
                                return false;
                            break;
                        case Misc.Antique:
                            NegativeAttributes neg4 = RunicReforging.GetNegativeAttributes(item);
                            if (neg4 == null || neg4.Antique == 0)
                                return false;
                            break;
                        case Misc.NotAntique:
                            NegativeAttributes neg5 = RunicReforging.GetNegativeAttributes(item);
                            if (neg5 != null && neg5.Antique > 0)
                                return false;
                            break;
                    }
                }
                else if (o is string)
                {
                    string str = o as string;

                    if (str == "WeaponVelocity" && (!(item is BaseRanged) || ((BaseRanged)item).Velocity < value))
                        return false;

                    /*if (str == "BalancedWeapon" && (!(item is BaseRanged) || !((BaseRanged)item).Balanced))
                        return false;*/

                    if (str == "SearingWeapon" && (!(item is BaseWeapon) || !((BaseWeapon)item).SearingWeapon))
                        return false;

                    if (str == "ArtifactRarity" && (!(item is IArtifact) || ((IArtifact)item).ArtifactRarity < value))
                    {
                        return false;
                    }
                }
			}

            return true;
		}

        private static bool CheckSlayer(Item item, object o)
        {
            if (o is TalismanSlayerName && (TalismanSlayerName)o == TalismanSlayerName.Undead)
            {
                if (!(item is ISlayer) || ((((ISlayer)item).Slayer != SlayerName.Undeads && ((ISlayer)item).Slayer2 != SlayerName.Undeads)))
                {
                    if (!(item is BaseTalisman) || ((BaseTalisman)item).Slayer != TalismanSlayerName.Undead)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (o is SlayerName && (!(item is ISlayer) || ((((ISlayer)item).Slayer != (SlayerName)o && ((ISlayer)item).Slayer2 != (SlayerName)o))))
                {
                    return false;
                }
                else if (o is TalismanSlayerName && (!(item is BaseTalisman) || ((BaseTalisman)item).Slayer != (TalismanSlayerName)o))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CheckCanRepair(Item item)
        {
            NegativeAttributes neg = RunicReforging.GetNegativeAttributes(item);

            return neg != null && neg.NoRepair != 0;
        }

        private static bool CheckKeyword(string searchstring, Item item)
        {
            return Keywords.ContainsKey(searchstring.ToLower()) && Keywords[searchstring.ToLower()] == item.GetType();
        }

        public static bool IsGargoyle(Item item)
        {
            if (item is BaseArmor)
            {
                return /*((BaseArmor)item).CanBeWornByGargoyles ||*/ ((BaseArmor)item).RequiredRace == Race.Gargoyle;
            }
            else if (item is BaseWeapon)
            {
                return /*((BaseWeapon)item).CanBeWornByGargoyles ||*/ ((BaseWeapon)item).RequiredRace == Race.Gargoyle;
            }
            else if (item is BaseClothing)
            {
                return /*((BaseClothing)item).CanBeWornByGargoyles ||*/ ((BaseClothing)item).RequiredRace == Race.Gargoyle;
            }
            else if (item is BaseJewel)
            {
                return /*((BaseJewel)item).CanBeWornByGargoyles ||*/ ((BaseJewel)item).RequiredRace == Race.Gargoyle;
            }

            return false;
        }

        public static bool IsElf(Item item)
        {
            if (item is BaseArmor)
            {
                return ((BaseArmor)item).RequiredRace == Race.Elf;
            }
            else if (item is BaseWeapon)
            {
                return ((BaseWeapon)item).RequiredRace == Race.Elf;
            }
            else if (item is BaseClothing)
            {
                return ((BaseClothing)item).RequiredRace == Race.Elf;
            }
            else if (item is BaseJewel)
            {
                return ((BaseJewel)item).RequiredRace == Race.Elf;
            }

            return false;
        }

        public static SearchCriteria AddNewContext(PlayerMobile pm)
        {
            SearchCriteria criteria = new SearchCriteria();

            Contexts[pm] = criteria;

            return criteria;
        }

        public static SearchCriteria GetContext(PlayerMobile pm)
        {
            if (Contexts.ContainsKey(pm))
                return Contexts[pm];

            return null;
        }

        public static Dictionary<PlayerMobile, SearchCriteria> Contexts { get; set; }
        public static List<SearchCategory> Categories { get; set; }

        public static Dictionary<string, Type> Keywords { get; set; }

        public static void Configure()
        {
            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(
                FilePath,
                writer =>
                {
                    writer.Write(0);

                    writer.Write(Contexts == null ? 0 : Contexts.Where(kvp => !kvp.Value.IsEmpty).Count());

                    if (Contexts != null)
                    {
                        foreach (var kvp in Contexts.Where(kvp => !kvp.Value.IsEmpty))
                        {
                            writer.Write(kvp.Key);
                            kvp.Value.Serialize(writer);
                        }
                    }
                });
        }

        public static void OnLoad()
        {
            Persistence.Deserialize(
                FilePath,
                reader =>
                {
                    int version = reader.ReadInt();
                    int count = reader.ReadInt();

                    for (int i = 0; i < count; i++)
                    {
                        PlayerMobile pm = reader.ReadMobile() as PlayerMobile;
                        var criteria = new SearchCriteria(reader);

                        if (pm != null)
                        {
                            if (Contexts == null)
                                Contexts = new Dictionary<PlayerMobile, SearchCriteria>();

                            Contexts[pm] = criteria;
                        }
                    }
                });
        }

        public static void Initialize()
        {
            try
            {
                StringList = new StringList("enu");
            }
            catch { }

            CommandSystem.Register("GetOPLString", AccessLevel.Administrator, e =>
                {
                    e.Mobile.BeginTarget(-1, false, TargetFlags.None, (m, targeted) =>
                        {
                            if (targeted is Item)
                            {
                                Console.WriteLine(GetItemName((Item)targeted));
                                e.Mobile.SendMessage(GetItemName((Item)targeted));
                            }
                        });
                });

            Categories = new List<SearchCategory>();

            if (Contexts == null)
            {
                Contexts = new Dictionary<PlayerMobile, SearchCriteria>();
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    SearchCategory price = new SearchCategory(Category.PriceRange);
                    SearchCategory equipment = new SearchCategory(Category.Equipamento);
                    SearchCategory misc = new SearchCategory(Category.Misc);
                    SearchCategory combat = new SearchCategory(Category.Combat);
                    SearchCategory casting = new SearchCategory(Category.Casting);
                    SearchCategory damagetype = new SearchCategory(Category.DamageType);
                    SearchCategory hitspell = new SearchCategory(Category.HitSpell);
                    SearchCategory hitarea = new SearchCategory(Category.HitArea);
                    SearchCategory resists = new SearchCategory(Category.Resists);
                    SearchCategory stats = new SearchCategory(Category.Stats);
                    SearchCategory slayer1 = new SearchCategory(Category.Slayer1);
                    SearchCategory slayer2 = new SearchCategory(Category.Slayer2);
                    SearchCategory slayer3 = new SearchCategory(Category.Slayer3);
                    SearchCategory requiredskill = new SearchCategory(Category.RequiredSkill);
                    SearchCategory skill1 = new SearchCategory(Category.Skill1);
                    SearchCategory skill2 = new SearchCategory(Category.Skill2);
                    SearchCategory skill3 = new SearchCategory(Category.Skill3);
                    SearchCategory skill4 = new SearchCategory(Category.Skill4);
                    SearchCategory skill5 = new SearchCategory(Category.Skill5);
                    SearchCategory skill6 = new SearchCategory(Category.Skill6);
                    SearchCategory sort = new SearchCategory(Category.Sort);

                    object[] enums = new object[15];
                    int[] labels = new int[15];
                    int index = 0;

                    foreach (int i in Enum.GetValues(typeof(Misc)))
                    {
                        enums[index] = (Misc)i;
                        labels[index] = i;
                        index++;
                    }

                    misc.Register(enums, labels);
                    misc.Register(new object[] { AosAttribute.EnhancePotions, AosArmorAttribute.LowerStatReq, AosAttribute.Luck, AosAttribute.ReflectPhysical, AosArmorAttribute.SelfRepair });
                    misc.Register(new object[] { "ArtifactRarity" }, new int[] { 1154693 });

                    equipment.Register(new object[] { Layer.Shoes, Layer.Pants, Layer.Shirt, Layer.Helm, Layer.Gloves, Layer.Ring, Layer.Talisman, Layer.Neck, Layer.Waist, Layer.InnerTorso,
                                                          Layer.Bracelet, Layer.MiddleTorso, Layer.Earrings, Layer.Arms, Layer.Cloak, Layer.OuterTorso, Layer.OuterLegs },
                                       new int[] { 1154602, 1154603, 1154604, 1154605, 1154606, 1154607, 1154608, 1154609, 1154611, 1154612, 1154613, 1154616, 1154617, 1154618, 1154619, 1154621, 1154622 });

                    combat.Register(new object[] { AosAttribute.WeaponDamage, AosAttribute.DefendChance, AosAttribute.AttackChance, AosAttribute.WeaponSpeed, AosArmorAttribute.SoulCharge, 
                                                       AosWeaponAttribute.UseBestSkill, AosWeaponAttribute.ReactiveParalyze, /*TODO: Assassin Honed*/"SearingWeapon", AosWeaponAttribute.BloodDrinker, AosWeaponAttribute.BattleLust, 
                                                       AosAttribute.BalancedWeapon, SAAbsorptionAttribute.CastingFocus, SAAbsorptionAttribute.EaterFire, SAAbsorptionAttribute.EaterCold,
                                                       SAAbsorptionAttribute.EaterPoison, SAAbsorptionAttribute.EaterEnergy, SAAbsorptionAttribute.EaterDamage });

                    casting.Register(new object[] { SAAbsorptionAttribute.ResonanceFire, SAAbsorptionAttribute.ResonanceCold, SAAbsorptionAttribute.ResonancePoison, SAAbsorptionAttribute.ResonanceEnergy,
                                                        SAAbsorptionAttribute.ResonanceKinetic, AosAttribute.SpellDamage, SAAbsorptionAttribute.CastingFocus, AosAttribute.CastRecovery, AosAttribute.Resistence,
                                                        AosAttribute.LowerManaCost, AosAttribute.LowerRegCost, AosWeaponAttribute.MageWeapon, AosArmorAttribute.MageArmor, AosAttribute.SpellChanneling });

                    damagetype.Register(new object[] { AosElementAttribute.Physical, AosElementAttribute.Fire, AosElementAttribute.Cold, AosElementAttribute.Poison, AosElementAttribute.Energy },
                                        new int[] { 1151800, 1151801, 1151802, 1151803, 1151804 });

                    hitspell.Register(new object[] { AosWeaponAttribute.HitDispel, AosWeaponAttribute.HitFireball, AosWeaponAttribute.HitHarm, AosWeaponAttribute.HitCurse, AosWeaponAttribute.HitLeechHits,
                                                         AosWeaponAttribute.HitLightning, "WeaponVelocity", AosWeaponAttribute.HitLowerAttack, AosWeaponAttribute.HitLowerDefend, AosWeaponAttribute.HitMagicArrow,
                                                         AosWeaponAttribute.HitLeechMana, AosWeaponAttribute.HitLeechStam, AosWeaponAttribute.HitFatigue, AosWeaponAttribute.HitManaDrain, AosWeaponAttribute.SplinteringWeapon /*TODO: Bane*/});

                    hitarea.Register(new object[] { AosWeaponAttribute.HitColdArea, AosWeaponAttribute.HitEnergyArea, AosWeaponAttribute.HitFireArea, AosWeaponAttribute.HitPhysicalArea, AosWeaponAttribute.HitPoisonArea });

                    resists.Register(new object[] { AosElementAttribute.Physical, AosElementAttribute.Fire, AosElementAttribute.Cold, AosElementAttribute.Poison, AosElementAttribute.Energy });

                    stats.Register(new object[] { AosAttribute.BonusStr, AosAttribute.BonusDex, AosAttribute.BonusInt, AosAttribute.BonusHits, AosAttribute.BonusStam, AosAttribute.BonusMana, AosAttribute.RegenHits, AosAttribute.RegenStam, AosAttribute.RegenMana });

                    slayer1.Register(new object[] { SlayerName.Repteis, SlayerName.Dragoes, SlayerName.HomensLagarto, SlayerName.Ofidianos, SlayerName.Cobras, SlayerName.Aracnideos, SlayerName.Escorpioes, SlayerName.Aranhas, SlayerName.Terathan });

                    slayer2.Register(new object[] { SlayerName.Humanoides, TalismanSlayerName.Bear, TalismanSlayerName.Beetle, TalismanSlayerName.Bird, TalismanSlayerName.Bovine, TalismanSlayerName.Flame, TalismanSlayerName.Goblin, TalismanSlayerName.Ice, 
                                                        TalismanSlayerName.Mage, SlayerName.Ogros, SlayerName.Orcs, SlayerName.Trolls, TalismanSlayerName.Vermin, TalismanSlayerName.Undead });
                    slayer3.Register(new object[] { SlayerName.Exorcismo, SlayerName.Gargulas, SlayerName.Fadas, SlayerName.Elemental, SlayerName.Vacuum, SlayerName.Sanguinarios, SlayerName.QuedraPedras, SlayerName.Flamejantes, SlayerName.Envenenados, SlayerName.Gelados, SlayerName.Aquosos });

                    requiredskill.Register(new object[] { SkillName.Swords, SkillName.Macing, SkillName.Fencing, SkillName.Archery, SkillName.Throwing });

                    skill1.Register(new object[] { SkillName.Swords, SkillName.Fencing, SkillName.Macing, SkillName.Magery, SkillName.Musicianship });
                    skill2.Register(new object[] { SkillName.Wrestling, SkillName.Tactics, SkillName.AnimalTaming, SkillName.Provocation, SkillName.SpiritSpeak });
                    skill3.Register(new object[] { SkillName.Stealth, SkillName.Parry, SkillName.Meditation, SkillName.AnimalLore, SkillName.Discordance, SkillName.Focus });
                    skill4.Register(new object[] { SkillName.Stealing, SkillName.Anatomy, SkillName.EvalInt, SkillName.Veterinary, SkillName.Necromancy, SkillName.Bushido, SkillName.Mysticism });
                    skill5.Register(new object[] { SkillName.Healing, SkillName.MagicResist, SkillName.Peacemaking, SkillName.Archery, SkillName.Chivalry, SkillName.Ninjitsu, SkillName.Throwing });
                    skill6.Register(new object[] { SkillName.Lumberjacking, SkillName.Snooping, SkillName.Mining });

                    Categories.Add(price);
                    //Categories.Add(misc);
                    Categories.Add(equipment);
                    Categories.Add(slayer1);
                    Categories.Add(slayer2);
                    Categories.Add(slayer3);
                    /*
                    Categories.Add(combat);
                    Categories.Add(casting);
                    Categories.Add(damagetype);
                    Categories.Add(hitspell);
                    Categories.Add(hitarea);
                    Categories.Add(resists);
                    Categories.Add(stats);
                    Categories.Add(requiredskill);
                    Categories.Add(skill1);
                    Categories.Add(skill2);
                    Categories.Add(skill3);
                    Categories.Add(skill4);
                    Categories.Add(skill5);
                    Categories.Add(skill6);
                    Categories.Add(sort);
                    */
                });

            Keywords = new Dictionary<string, Type>();

            Keywords["power scroll"] = typeof(PowerScroll);
            Keywords["stat scroll"] = typeof(StatCapScroll);
        }

        public static string GetItemName(Item item)
        {
            if (StringList == null || item.Name != null)
                return item.Name;

            ObjectPropertyList opl = new ObjectPropertyList(item);
            item.GetProperties(opl);

            if (opl == null)
            {
                //if there was a problem with this process, just return null
                return null;
            }

            //since the object property list is based on a packet object, the property info is packed away in a packet format
            byte[] data = opl.UnderlyingStream.UnderlyingStream.ToArray();

            int index = 15; // First localization number index
            string basestring = null;

            //reset the number property
            uint number = 0;

            //if there's not enough room for another record, quit
            if (index + 4 >= data.Length)
            {
                return null;
            }

            //read number property from the packet data
            number = (uint)(data[index++] << 24 | data[index++] << 16 | data[index++] << 8 | data[index++]);

            //reset the length property
            ushort length = 0;

            //if there's not enough room for another record, quit
            if (index + 2 > data.Length)
            {
                return null;
            }

            //read length property from the packet data
            length = (ushort)(data[index++] << 8 | data[index++]);

            //determine the location of the end of the string
            int end = index + length;

            //truncate if necessary
            if (end >= data.Length)
            {
                end = data.Length - 1;
            }

            //read the string into a StringBuilder object

            StringBuilder s = new StringBuilder();
            while (index + 2 <= end + 1)
            {
                short next = (short)(data[index++] | data[index++] << 8);

                if (next == 0)
                    break;

                s.Append(Encoding.Unicode.GetString(BitConverter.GetBytes(next)));
            }

            basestring = StringList.GetString((int)number);
            string args = s.ToString();

            if (args == null || args == String.Empty)
            {
                return basestring;
            }

            string[] parms = args.Split('\t');

            try
            {
                if (parms.Length > 1)
                {
                    for (int i = 0; i < parms.Length; i++)
                    {
                        parms[i] = parms[i].Trim(' ');

                        if (parms[i].IndexOf("#") == 0)
                        {
                            parms[i] = StringList.GetString(Convert.ToInt32(parms[i].Substring(1, parms[i].Length - 1)));
                        }
                    }
                }
                else if (parms.Length == 1 && parms[0].IndexOf("#") == 0)
                {
                    parms[0] = StringList.GetString(Convert.ToInt32(args.Substring(1, parms[0].Length - 1)));
                }
            }
            catch
            {
                return null;
            }

            StringEntry entry = StringList.GetEntry((int)number);

            if (entry != null)
            {
                return entry.Format(parms);
            }

            return basestring;
        }

        private static List<Item> GetItems(PlayerVendor pv)
        {
            List<Item> list = new List<Item>();

            foreach (Item item in pv.Items)
                if (item.Movable && item != pv.Backpack && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair)
                    list.Add(item);

            if (pv.Backpack != null)
            {
                GetItems(pv.Backpack, list);
            }

            return list;
        }

        public static void GetItems(Container c, List<Item> list)
        {
            if (c == null || c.Items.Count == 0)
                return;

            foreach (Item item in c.Items)
            {
                if (item is Container && !IsSearchableContainer(item.GetType()))
                    GetItems((Container)item, list);
                else
                    list.Add(item);
            }
        }

        public static bool HasValue(object o, SearchCategory category)
        {
            if (o is AosAttribute && (AosAttribute)o == AosAttribute.Resistence)
                return true;

            if (category.Category == Category.RequiredSkill)
                return false;

            if (o is string && (string)o == "ArtifactRarity")
                return true;

            return Imbuing.GetMaxValue(o) > 1;
        }

        public static bool CanSearch(Mobile m)
        {
            Region r = m.Region;

            if (r.GetLogoutDelay(m) == TimeSpan.Zero)
                return true;

            return r is GuardedRegion && !((GuardedRegion)r).Disabled;
        }

        private static bool IsSearchableContainer(Type type)
        {
            return _SearchableContainers.Any(t => t == type || type.IsSubclassOf(t));
        }

        private static Type[] _SearchableContainers =
        {
            typeof(BaseQuiver),                     typeof(BaseResourceSatchel),
            typeof(FishBowl),                       typeof(FirstAidBelt),
            typeof(Server.Engines.Plants.SeedBox),  typeof(BaseSpecialScrollBook),
            typeof(GardenShedBarrel),               
            typeof(JewelryBox),
        };
	}

    public enum SortBy
    {
        LowToHigh,
        HighToLow
    }

    public enum Category
    {
        PriceRange = 1154512,
        Misc = 1154647,
        Equipamento = 1154531,
        Combat = 1154541,
        Casting = 1154538,
        DamageType = 1154535,
        HitSpell = 1154536,
        HitArea = 1154537,
        Resists = 1154539,
        Stats = 1154540,
        Slayer1 = 1154683,
        Slayer2 = 1154684,
        Slayer3 = 1154685,
        RequiredSkill = 1154543,
        Skill1 = 1114255,
        Skill2 = 1114256,
        Skill3 = 1114257,
        Skill4 = 1114258,
        Skill5 = 1114259,
        Skill6 = 1114260,
        Sort = 1154695,
    }

    public enum Misc
    {
        ExcludeFel = 1154646,
        GargoyleOnly = 1154648,
        NotGargoyleOnly = 1154704,
        ElvesOnly = 1154650,
        NotElvesOnly = 1154703,
        FactionItem = 1154661,
        PromotionalToken = 1154682,
        Cursed = 1116639,
        NotCursed = 1154701,
        CannotRepair = 1151826,
        NotCannotBeRepaired = 1154705,
        Brittle = 1116209,
        NotBrittle = 1154702,
        Antique = 1152714,
        NotAntique = 1156479
    }

    public enum SpecialSearch
    {
        PowerScroll,
        StatScroll,
        Transcendence,
        Alacrity,
        UsesRemaining
    }

    public class SearchCategory
    {
        public Category Category { get; private set; }
        public int Label { get { return (int)Category; } }

        public List<Tuple<object, int>> Objects { get; private set; }

        public SearchCategory(Category category)
        {
            Category = category;

            Objects = new List<Tuple<object, int>>();
        }

        public void Register(object o)
        {
            if (Objects.FirstOrDefault(t => t.Item1 == o) == null)
                Objects.Add(new Tuple<object, int>(o, Imbuing.GetAttributeName(o)));
        }

        public void Register(object[] o)
        {
            foreach (object obj in o)
            {
                if (Objects.FirstOrDefault(t => t.Item1 == o) == null)
                    Objects.Add(new Tuple<object, int>(obj, Imbuing.GetAttributeName(obj)));
            }
        }

        public void Register(object o, int label)
        {
            if (Objects.FirstOrDefault(t => t.Item1 == o) == null)
            {
                Objects.Add(new Tuple<object, int>(o, label));
            }
        }

        public void Register(object[] o, int[] labels)
        {
            if (o.Length != labels.Length)
            {
                Console.WriteLine("ERROR: {0} has bad registration entry", this.Category.ToString());
            }

            for (int i = 0; i < o.Length; i++)
            {
                if (Objects.FirstOrDefault(t => t.Item1 == o[i]) == null)
                    Objects.Add(new Tuple<object, int>(o[i], labels[i]));
            }
        }
    }

    public class SearchCriteria
    {
        public Layer SearchType { get; set; }
        public string SearchName { get; set; }
        public SortBy SortBy { get; set; }
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }

        public bool EntryPrice { get; set; }

        public List<SearchDetail> Details { get; set; }

        public SearchCriteria()
        {
            Details = new List<SearchDetail>();

            MinPrice = 0;
            MaxPrice = 175000000;
            SearchType = Layer.Invalid;
        }

        public void Reset()
        {
            Details.Clear();
            Details.TrimExcess();
            Details = new List<SearchDetail>();

            MinPrice = 0;
            MaxPrice = 175000000;
            SortBy = SortBy.LowToHigh;
            SearchName = null;
            SearchType = Layer.Invalid;
            EntryPrice = false;
        }

        public int GetValueForDetails(object o)
        {
            SearchDetail detail = Details.FirstOrDefault(d => d.Attribute == o);

            return detail != null ? detail.Value : 0;
        }

        public void TryAddDetails(object o, int name, int value, Category cat)
        {
            SearchDetail d = Details.FirstOrDefault(det => det.Attribute == o);

            if (o is Layer)
            {
                SearchDetail layer = Details.FirstOrDefault(det => det.Attribute is Layer && (Layer)det.Attribute != (Layer)o);

                if (layer != null)
                {
                    Details.Remove(layer);
                }

                Details.Add(new SearchDetail(o, name, value, cat));
                SearchType = (Layer)o;
            }
            else if (d == null)
            {
                d = new SearchDetail(o, name, value, cat);

                Details.Add(d);
            }
            else if (d.Value != value)
            {
                d.Value = value;
            }

            /*if (d.Attribute is TalismanSlayerName && (TalismanSlayerName)d.Attribute == TalismanSlayerName.Undead)
            {
                TryAddDetails(SlayerName.Silver, name, value, cat);
            }*/
        }

        public bool IsEmpty
        {
            get { return Details.Count == 0 && MinPrice == 0 && MaxPrice == 175000000 && String.IsNullOrEmpty(SearchName) && SearchType == Layer.Invalid; }
        }

        public SearchCriteria(GenericReader reader)
        {
            int version = reader.ReadInt();

            Details = new List<SearchDetail>();

            if (version != 0)
                EntryPrice = reader.ReadBool();

            SearchType = (Layer)reader.ReadInt();
            SearchName = reader.ReadString();
            SortBy = (SortBy)reader.ReadInt();
            MinPrice = reader.ReadLong();
            MaxPrice = reader.ReadLong();

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Details.Add(new SearchDetail(reader));
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(1);

            writer.Write((bool)EntryPrice);
            writer.Write((int)SearchType);
            writer.Write(SearchName);
            writer.Write((int)SortBy);
            writer.Write(MinPrice);
            writer.Write(MaxPrice);

            writer.Write(Details.Count);

            for (int i = 0; i < Details.Count; i++)
            {
                Details[i].Serialize(writer);
            }
        }
    }

    public class SearchDetail
    {
        public enum AttributeID
        {
            None = 0,
            AosAttribute,
            AosArmorAttribute,
            AosWeaponAttribute,
            AosElementAttribute,
            SkillName,
            SAAbosorptionAttribute,
            ExtendedWeaponAttribute,
            NegativeAttribute,
            SlayerName,
            String,
            TalismanSlayerName,
            TalismanSkill,
            TalismanRemoval,
            Int,
        }

        public object Attribute { get; set; }
        public int Label { get; set; }
        public int Value { get; set; }
        public Category Category { get; set; }

        public SearchDetail(object o, int label, int value, Category category)
        {
            Attribute = o;
            Label = label;
            Value = value;
            Category = category;
        }

        public SearchDetail(GenericReader reader)
        {
            reader.ReadInt(); // version

            ReadAttribute(reader);

            Label = reader.ReadInt();
            Value = reader.ReadInt();
            Category = (Category)reader.ReadInt();
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            WriteAttribute(writer);

            writer.Write(Label);
            writer.Write(Value);
            writer.Write((int)Category);
        }

        private void WriteAttribute(GenericWriter writer)
        {
            int attrID = GetAttributeID(Attribute);
            writer.Write(attrID);

            switch (attrID)
            {
                case 0: break;
                case 1: writer.Write((int)(AosAttribute)Attribute); break;
                case 2: writer.Write((int)(AosArmorAttribute)Attribute); break;
                case 3: writer.Write((int)(AosWeaponAttribute)Attribute); break;
                case 4: writer.Write((int)(AosElementAttribute)Attribute); break;
                case 5: writer.Write((int)(SkillName)Attribute); break;
                case 6: writer.Write((int)(SAAbsorptionAttribute)Attribute); break;
                case 7: writer.Write((int)(ExtendedWeaponAttribute)Attribute); break;
                case 8: writer.Write((int)(NegativeAttribute)Attribute); break;
                case 9: writer.Write((int)(SlayerName)Attribute); break;
                case 10: writer.Write((string)Attribute); break;
                case 11: writer.Write((int)(TalismanSlayerName)Attribute); break;
                case 12: writer.Write((int)(TalismanSkill)Attribute); break;
                case 13: writer.Write((int)(TalismanRemoval)Attribute); break;
                case 14: writer.Write((int)Attribute); break;
            }
        }

        private void ReadAttribute(GenericReader reader)
        {
            switch (reader.ReadInt())
            {
                case 0: break;
                case 1: Attribute = (AosAttribute)reader.ReadInt(); break;
                case 2: Attribute = (AosArmorAttribute)reader.ReadInt(); break;
                case 3: Attribute = (AosWeaponAttribute)reader.ReadInt(); break;
                case 4: Attribute = (AosElementAttribute)reader.ReadInt(); break;
                case 5: Attribute = (SkillName)reader.ReadInt(); break;
                case 6: Attribute = (SAAbsorptionAttribute)reader.ReadInt(); break;
                case 7: Attribute = (ExtendedWeaponAttribute)reader.ReadInt(); break;
                case 8: Attribute = (NegativeAttribute)reader.ReadInt(); break;
                case 9: Attribute = (SlayerName)reader.ReadInt(); break;
                case 10: Attribute = reader.ReadString(); break;
                case 11: Attribute = (TalismanSlayerName)reader.ReadInt(); break;
                case 12: Attribute = (TalismanSkill)reader.ReadInt(); break;
                case 13: Attribute = (TalismanRemoval)reader.ReadInt(); break;
                case 14: Attribute = reader.ReadInt(); break;
            }
        }

        public static int GetAttributeID(object o)
        {
            if (o is AosAttribute)
                return (int)AttributeID.AosAttribute;

            if (o is AosArmorAttribute)
                return (int)AttributeID.AosArmorAttribute;

            if (o is AosWeaponAttribute)
                return (int)AttributeID.AosWeaponAttribute;

            if (o is AosElementAttribute)
                return (int)AttributeID.AosElementAttribute;

            if (o is SkillName)
                return (int)AttributeID.SkillName;

            if (o is SAAbsorptionAttribute)
                return (int)AttributeID.SAAbosorptionAttribute;

            if (o is ExtendedWeaponAttribute)
                return (int)AttributeID.ExtendedWeaponAttribute;

            if (o is NegativeAttribute)
                return (int)AttributeID.NegativeAttribute;

            if (o is SlayerName)
                return (int)AttributeID.SlayerName;

            if (o is TalismanSlayerName)
                return (int)AttributeID.TalismanSlayerName;

            if (o is string)
                return (int)AttributeID.String;

            if (o is TalismanSkill)
                return (int)AttributeID.TalismanSkill;

            if (o is TalismanRemoval)
                return (int)AttributeID.TalismanRemoval;

            if (o is int)
                return (int)AttributeID.Int;

            return (int)AttributeID.None;
        }
    }

    public class SearchVendors : ContextMenuEntry
    {
        public PlayerMobile Player { get; set; }

        public SearchVendors(PlayerMobile pm)
            : base(1154679, -1)
        {
            Player = pm;

            Enabled = VendorSearch.CanSearch(pm);
        }

        public override void OnClick()
        {
            if (VendorSearch.CanSearch(Player))
            {
                BaseGump.SendGump(new VendorSearchGump(Player));
            }
        }
    }
}
