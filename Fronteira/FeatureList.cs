using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;

namespace Server.Scripts
{
    /* Format is:
     * // See Story: [story number here]
     * // [Short description of the feature]
     * public static class [NameOfFeature]
     * {
     *      public static readonly bool Enabled = [whatever];
     *      
     *      [Your variables here, so they can be adjusted by non coders]
     * }
     * 
     * Then in your code you go:
     * if (FeatureList.MyCoolFeature.Enabled)
     * {
     *      // Do some cool shit
     * }
     * 
     * This should make searching for what changes what easier, and help the non-coders change shit.
     * Remember that you don't have to nuke every trace of the related code behind ugly ifs, just the parts
     * where it has an effect on gameplay.
     * */

    public static class FeatureList
    {
        // See story 24
        // Adds a 'violent swing' mechanic which gives dexers triggerable burst damage.
        public static class ViolentSwingMechanic
        {
            public static readonly bool Enabled = true;

            public static readonly int StaminaRequired = 20;
            //public static readonly int DamageBonusPercentage = 75;  old mechanic
            public static readonly int MinDamageRollModifier = 0; //the number we ADD to max roll for a violent swing, go negative for weakining, positive for overpowering

            public static readonly string OnActivateText = "Preparing a violent swing...";
            public static readonly string OnHitText = "You swing violently at your opponent, inflicting tremendous pain!";
            public static readonly string NotEnoughStamText = "You are too weak to attempt a violent swing.";
            public static readonly string TimerNotElapsedText = "You must wait before attempting another violent swing.";
            public static readonly string NotEnoughSkillText = "You lack the skill to perform a violent swing.";
        }

        // See story 41
        // Adds the ability for healer mages to stun punch people.
        public static class StunPunch
        {
            public static readonly bool Enabled = true;

            public static readonly int MinimumStunDuration = 2; // seconds
            public static readonly int MaximumStunDuration = 4; // seconds
            public static readonly int ManaActivationCost = 10;
            public static readonly int DelayUntilReuse = 30; // seconds
            public static readonly double WrestlingSkillNeeded = 80.0;

            public static readonly double BaseDuration = 4.75;
            public static readonly double DurationReductionPerDex = 0.03;

            public static readonly string OnActivateText = "You prepare to strike a stunning blow.";
            public static readonly string OnHitText = "Your opponent is stunned!";
            public static readonly string OnFailText = "You fail to stun your opponent.";
            public static readonly string NotEnoughManaText = "You are not focused enough to stun your opponent.";
            public static readonly string TimerNotElapsedText = "You must wait before attempting another stunning blow.";
            public static readonly string HandsNotFreeMessage = "You must have your hands free to attempt to stun your opponent.";

            public static SkillName[] SkillsRequiredToAttempt = { SkillName.Wrestling, SkillName.Healing, SkillName.Anatomy };
            public static double MinimumSkillRequired = 60;
            public static readonly string NotEnoughSkillText = "You lack the skill to attempt a stunning blow.";
            public static readonly double MaxChance = 0.5; // 50%
        }

        // See story 53
        // Scribe mages y'all
        public static class ScribeMage
        {
            public static readonly bool Enabled = true;

            // Magic Reflect
            public static readonly int MinimumCirclesToReflect = 1;      // Circles absorbed at 0% Scribe/SpiritSpeak
            public static readonly int MaximumCirclesToReflect = 8;     // Circles absorbed at GM Scribe/SpiritSpeak
            public static readonly int ReflectReuseDelay = 120;          // Seconds after first casting

            public static readonly string ReflectCooldownText = "You must wait before gaining magical reflection again.";

            // Reactive Armor
            public static readonly int MinimumArmorBoost = 20;           // % damage absorb bonus at 0% Scribe/SpiritSpeak
            public static readonly int MaximumArmorBoost = 80;          // % damage absorb bonus at GM Scribe/SpiritSpeak
            public static readonly int ArmorReuseDelay = 120;           // Seconds after first casting

            public static readonly string ArmorCooldownText = "You must wait before gaining reactive armor again.";

            // Protection
            public static readonly int MinimumInterruptResistChance = 0;           // at 0% Scribe/SpiritSpeak
            public static readonly int MaximumInterruptResistChance = 85;          // at GM Scribe/SpiritSpeak
            public static readonly int ProtectionDuration = 60;                 // Seconds
            public static readonly int ProtectionReuseDelay = 150;              // Seconds after first casting

            public static readonly string ProtectionCooldownText = "You must wait before gaining magical protection again.";
        }

        // See story: 39
        // Adds changes to enable the "nox mage" template
        public static class NoxMages
        {
            public static readonly bool Enabled = true;
        }

        //See story: 42
        //Adds the ability to imbue an item with a magic effect
        public static class ArtifactCrafting
        {
            public static readonly double BaseSuccessChance = 0.5;
            public static readonly int ImbuingToolUses = 10;
            public static readonly int ImbuingToolIngotCost = 500;
            public static readonly string ImbuingToolDoubleClickMessage = "That is not a specific type of imbuing tool!";
            public static readonly double Level1ContractSkillRequired = 70;
            public static readonly double Level2ContractSkillRequired = 80;
            public static readonly double Level3ContractSkillRequired = 90;
        }

        public static class ArtifactDropRate
        {
            //tierXwealth
            public static double T0Meager = 0.50;
            public static double SlayerMeager = 0.10;

            public static double T0Avg = 2.50;
            public static double T1Avg = 0.50;
            public static double SlayerAvg = 0.50;

            public static double T1Rich = 2.50;
            public static double T2Rich = 0.5;
            public static double SlayerRich = 10.0;

            public static double T2FilthyRich = 10.0;
            public static double T3FilthyRich = 5.0;
            public static double SlayerFilthyRich = 25.0;

            public static double T3UltraRich = 25.0;
            public static double SlayerUltraRich = 50.0;

            public static double T3SuperBoss = 100.0;
            public static double SlayerSuperBoss = 100.0;
        }
        //See story 35 and 37
        public static class Citizenship
        {
            public static readonly float LeaveTimeInHours = 48.0f;
            public static readonly float JoinBlockTimeInHours = 2.0f;
            public static readonly float PendingTimeInDays = 11.0f; //time when the mayor rules 
            public static readonly float CampaigningTimeInHours = 48.0f;
            public static readonly float VotingTimeInHours = 24.0f;

            public static readonly double ExileTimeInDays = 7.0f;

            public static readonly int StealingMaxRollUnderWatchfulGuards = 250; //base outside watchful guards is 150


            public static readonly float SkillChanceBonus = 1.35f; //50% greater chance to gain skill

            public static readonly int HuntingResourceBonus = 2; //feathers and whatnot
            public static readonly int ResourceRollMax = 4; //changes the chance a resource bonus will be applied, currently 1 in 4
            public static readonly int AdventurerResourceBonus = 10; //percent gold bonus
            public static readonly int FameResourceBonus = 10;
            public static readonly int MiningResourceBonus = 2;
            public static readonly int LumberjackResourceBonus = 5;
            public static readonly int FishingResourceBonus = 1;
            public static readonly int SilverResourceBonus = 10;

            public static readonly float MagicDamageReductionBonus = 0.9f; //lowers the damage by 10%
            public static readonly float MagicDamageBonus = 1.1f; //increases damage by 10%
            public static readonly float WeaponDamageBonus = 1.1f;

            public static readonly float SiegeToolsDamageBonus = 1.1f;

            public static readonly float AbsentHeroResourceBonusReduction = 0.50f; //how much we reduce the resource bonus when the hero is not home
            public static readonly float ResourceBonusIncreasePerCapturedHero = 0.50f; //increase by 15%

            public static readonly float AbsentHeroSkillBonusReduction = 0.50f; //reduction of 15%
            public static readonly float SkillBonusIncreasePerCapturedHero = 0.50f; //increase by 15%

            public static readonly float AbsentHeroHeroBonusReduction = 0.50f;
            public static readonly float HeroBonusIncreasePerCapturedHero = 0.50f;

            public static readonly int TagTextHueModifier = -2; //the number we add to secondary militia hue for citizenship tag text

            public static readonly double BuyDiscount = 0.90; //reduces prices by 10%
        }
        //See story 55
        public static class Militias
        {
            public static readonly float LeaveTimeInDays = 3.0f;
            public static readonly string LillanoMilitiaAboutText = "Lillano Militia, protectors of that hero over there.";
            public static readonly string PedranMilitiaAboutText = "Pedran Militia, protectors of that hero over there.";
            public static readonly string ArborMilitiaAboutText = "Arbor Militia, protectors of that hero over there.";
            public static readonly string CalorMilitiaAboutText = "Calor Militia, protectors of that hero over there.";
            public static readonly string VermellMilitiaAboutText = "Vermell Militia, protectors of that hero over there.";

            public static readonly float YieldSilverBlockTimeInMinutes = 20; //the time after dying and giving silver before the player yields silver again

            public static readonly float SkillLossTimeInMinutes = 5;

            //rank titles
            public static readonly string Tier0TitleText = "Conscript";
            public static readonly string Tier1TitleText = "Guard";
            public static readonly string Tier2TitleText = "Defender";
            public static readonly string Tier3TitleText = "Sentinel";
            public static readonly string Tier4TitleText = "Knight";
            public static readonly string Tier5TitleText = "Champion"; //the rank given to the player with the highest points

            public static readonly int SilverRewardForKill = 30;
            public static readonly int SilverRewardForHeroKidnap = 80;
            public static readonly int SilverRewardForHeroCapture = 80;
            public static readonly int SilverRewardForHeroRecapture = 80;
            public static readonly int SilverRewardForHeroRescue = 80;
            public static readonly float MilitiaItemExperationInDays = 7;

            public static readonly float SilverTaxChangeBlockTimeInHours = 24;

            public static readonly int HenchmanGuardSilverCost = 500;
            public static readonly int MercenaryGuardSilverCost = 1000;
            public static readonly int SorceressGuardSilverCost = 1000;
            public static readonly int WizardGuardSilverCost = 1500;
            public static readonly int KnightGuardSilverCost = 1500;
            public static readonly int PaladinGuardSilverCost = 2000;

            public static readonly float CommanderBroadcastPeriodInMin = 5; //two broadcasts are allowed in this period
            public static readonly int CommanderBroadcastSilverCost = 50;


            public static readonly int MaximumTraps = 5;
            public static readonly int TrapDeedConstructionSilverCost = 250;
            public static readonly float TrapExpirationTimeInDays = 1.0f;

            public static readonly TimeSpan CivilianAssistanceFlagDuration = TimeSpan.FromMinutes(2);  // How long after healing a militia member are you flagged as a militia member
        }

        // See story: 60
        // Player bounty hunters
        public static class BountyHunters
        {
            public static readonly string[] EnrolText = { "enrol", "enroll", "join", "register" }; // What players say to a bounty hunter NPC

            public static readonly int RegistrationCost = 500;
            public static readonly string RegistrationOfferText = "It costs 500 gold pieces to register as a bounty hunter.";
            public static readonly string AlreadyBountyHunterText = "You are already a registered bounty hunter!";
            public static readonly string JoinText = "All right, you're now registered to collect bounties. Good luck!";

            public static readonly TimeSpan ResignDelay = TimeSpan.FromHours(0.25);
            public static readonly string ResignText = "i resign as a bounty hunter";
            public static readonly string ConfirmResignationText = "You will cease to be registered as a bounty hunter in 15 minutes, after I finish this paperwork.";
            public static readonly string InformResignationText = "You are no longer a registered bounty hunter.";
        }

        // See story: 56
        // Bounties and reporting
        public static class MurderSystem
        {
            public static readonly int AutomaticBountyPerKill = 250;
            public static readonly int MaxVictimBounty = 1000;          // Max bounty a victim can place on their killer

            // public static readonly double StatlossPenaltyDurationPerKill = 120;       // minutes (OLD SYSTEM)
            public static readonly double ParolePenaltyDurationPerKill = 60;  // minutes
            // public static readonly int BribeForRemovingOneHourOfStatloss = 250;    // gold pieces (OLD SYSTEM)
            public static readonly int BribeForRemovingOneHourOfParole = 500;    // gold pieces (OLD SYSTEM)

            public static readonly double SkillPercentageToLoseInStatloss = 40;

            public static readonly double TemporaryStatlossDuration = 10;              // minutes

            public static readonly TimeSpan TimeToStayOnBountyBoard = TimeSpan.FromDays(14);        // After X days without a kill, you get removed from the board
        }

        public static class Heroes
        {
            public static readonly float HeroDoorHealthRegenPerMinute = 0.10f; //regens 10% of health every 1 minute when a protecting militia member is present
            public static readonly float InactiveTimerLengthInSeconds = 60f; // isn't used anymore
            public static readonly float WindowOfOpportunityTimerLengthInMinutes = 30f;
            public static readonly float CapturedTimerLengthInHours = 18.0f; // 18 hours
            public static readonly float CaptureImmunityTimerLengthInHours = 0.5f;
            public static readonly float HeroDoorRegenTimerInMinutes = 15.0f;
            public static readonly string OnWaitingAroundForCaptureSayText = "Help! The doors have been destroyed! I can be captured!";
            public static readonly string OnWaitingAroundForRescueSayText = "Thank you, I can now be rescued!";
            public static readonly string OnFollowSayText = "I will follow you if I must.";
            public static readonly string OnCaptureSayText = "Oh no, I have been captured. To the cells I go.";
            public static readonly string OnRescueSayText = "Thank the gods, I have been rescued from those dogs.";
            public static readonly string OnReturnHomeSayText = "Oh look! My chance to escape.";
            public static readonly string OnReturnHomeAfterCaptureSayText = "I have been in the cells long enough, I'm going home and I'll be capture immune for a while.";
            public static readonly string OnCaptureImmunityEndSayText = "I have been immune to capture long enough, I can now be captured again.";
            public static readonly string OnAbandonedByPlayerSay = "Fine then, I'll wait here! I'll be rescued soon enough.";
            public static readonly string DoorReportAttackMessage = "To arms, the doors protecting the hero are under siege!";
            public static readonly string OtherFactionDoorReportAttackMessage = "doors protecting the hero are under siege!";

            public static readonly double DefaultActiveSpeed = 0.35;
            public static readonly double IntimidatedActiveSpeedAdjuster = -0.1;
            public static readonly double DefiantActiveSpeedAdjuster = 0.1;
        }

        //see story 54
        public static class ArmorChanges
        {
            public static readonly int CraftedARBonus = 1; //AR bonus to Exceptional AR (low quality armor is reduced by this amount, regular has no change)
            public static readonly int ArtifactARBonusPerLevel = 1; //AR bonus to Armor per Artifact Level 
            public static readonly double ArmorDamageReductionScalar = 1.0; //Percent damage reduction per randomly determed AR...AR will be randomized between half to full total Virtual Armor value
            public static readonly double DurablilityImpactScalar = 0.0; //% of AR reduced per % of missing armor hitpoints (higher = more Ar loss) 
            public static readonly double DurablilityLossChance = .25; //Percent chance on hit to lose durability
            public static readonly double DurablilityLossScalar = .02; //How much to change raw damage purely for determining armor durability loss
            public static readonly double ParryBaseReductionPercent = .50; //Base percent of damage reduced by any successful parry
            public static readonly double ParryReductionARMultiplier = 0.5; //Is multiplied by Shield AR to determine how much additional percent of damage is reduced
            public static readonly double ShieldARWithoutParry = 1.0; //Base Shield AR granted to players with no Parrying
            public static readonly double ShieldARParryBonus = .000; //Amount of Shield AR granted to players with Parrying
            public static readonly int HitPointsBreakWarning = 3; //Warning of possible breakage when armor below this number of hitpoints
        }

        public static class ParryChanges
        {
            public static readonly double ParryBreathChanceDivisor = 2; //Divisor to get chance to parry monster breath
            public static readonly double ParryMeleeChanceDivisor = 4; //Divisor to get chance to parry melee attacks
            public static readonly double ParryMagicChanceDivisor = 5; //Divisor to get chance to parry spell damage (calculation is 100%/4=25%)
            public static readonly bool ParryMagicPVP = true; //Whether parrying spells works against other players
            public static readonly bool CastThroughOCShields = true; // Casting with O/C shields equipped
        }

        //see story 66
        public static class SkillChanges
        {
            public static void Initialize()
            {
                SkillTableCreation();
            }

            public static readonly double BaseSkillGainChance = 0.3;
            public static readonly double SkillGainDivisionFactor = 11.0; //greater this is, the lower the chance of gain
            public static readonly double CraftingSkillGainFactor = 0.5;
            public static readonly double StatGainDivisionFactor = 2.0; //greater this is, the lower the chance of gain

            public static readonly double StealthStepSuccessBasePercent = 50; //Base % chance to enter stealth / successfully move step
            public static readonly double StealthStepSkillBonusDivider = 2; //Divided by Skill = Bonus % chance to enter stealth / successfully move steps
            public static readonly double StealthStepFreeStepsDivider = 5; //Divided by Skill (floor) = Free Stealth Steps (not tested for skill) on Entering Stealth
            public static readonly double TrackingRangeScalar = 0.8; //trackingRange = TrackingRangeScale * playerTrackingSkill 
            public static readonly double TrackingDetectHiddenStartingRangeDivider = 10.0; //Divide tracking distance by this to find range to find hidden targets with 0 DetectHidden

            public static readonly int HiddenTextHue = 0; // the color of the overhead text when someone speaks while hidden

            public static readonly double DungeonSkillGainChanceIncrease = 1.20; // Dungeon skill gain bonus

            //the following gives duration via duration = BaseParalyzeDuration + magerySkill*MaxMageryParalyzeDuration - resistSkill*MaxResistParalyzeDuration
            public static readonly double BaseParalyzeDuration = 1.0;
            public static readonly double MaxMageryParalyzeDuration = 5.0;
            public static readonly double MaxResistParalyzeDuration = 2.0;
            public static readonly double ResistedParalyzeDurationReduction = 0.5; //how much we reduce the duration on resist, this happens after duration calculation

            public static readonly double ArcheryInitialBaseShotTime = 2.0;
            public static readonly double ArcheryShotTimeDexReduction = 0.015;
            public static readonly double ArcheryContinuousBaseShotTime = 2.75;

            public static readonly double HerdingDifficultyDivider = 25;
            public static readonly double HerdingMinTamingSkillMod = 25;

            public static readonly double RemoveTrapDifficultyDivider = 25;

            public static readonly double SpiritSpeakGhostVisibilityDuration = 30; //Seconds after attempting to spirit speak you are visible to ghosts

            //Skill Gain Ranges (Range from First Number to +5 Skill, Hours Needed to Gain Entire Range in Skill)
            public static readonly double[,] SkillRanges = {{ 0, .25 },
                                                            { 5, .5 },
                                                            { 10, .75 },
                                                            { 15, 1.0 },
                                                            { 20, 1.25 },
                                                            { 25, 1.5 },
                                                            { 30, 2.0 },
                                                            { 35, 2.5 },
                                                            { 40, 3.0 },
                                                            { 45, 3.5 },
                                                            { 50, 4.0 },
                                                            { 55, 6.0 },
                                                            { 60, 8.0 },
                                                            { 65, 10.0 },
                                                            { 70, 12.0 },
                                                            { 75, 14.0 },
                                                            { 80, 18.0 },
                                                            { 85, 20.0 },
                                                            { 90, 26.0 },
                                                            { 95, 34.0 }
                                                           };

            //Skill Gain Location Factors
            public static readonly double TownFactor = 1.0;
            public static readonly double HouseFactor = 1.0;
            public static readonly double WildernessFactor = 1.0;
            public static readonly double DungeonFactor = 1.25;

            public static Dictionary<SkillName, double> SkillIntervals = new Dictionary<SkillName, double>();
            public static Dictionary<SkillName, double> SkillConstantMod = new Dictionary<SkillName, double>();

            //Skill Intervals
            public static void SkillTableCreation()
            {
                //Intervals
                SkillIntervals.Add(SkillName.Anatomy, 1.0);
                SkillIntervals.Add(SkillName.AnimalLore, 1.0);
                SkillIntervals.Add(SkillName.ArmsLore, 1.0);
                SkillIntervals.Add(SkillName.DetectHidden, 6.0);
                SkillIntervals.Add(SkillName.Discordance, 18.0);
                SkillIntervals.Add(SkillName.EvalInt, 1.0);
                SkillIntervals.Add(SkillName.Forensics, 1.0);
                SkillIntervals.Add(SkillName.Hiding, 10.0);
                SkillIntervals.Add(SkillName.ItemID, 1.0);
                SkillIntervals.Add(SkillName.Meditation, 5.0);
                SkillIntervals.Add(SkillName.Poisoning, 7.0);
                SkillIntervals.Add(SkillName.RemoveTrap, 3.0);
                SkillIntervals.Add(SkillName.Snooping, 1.0);
                SkillIntervals.Add(SkillName.SpiritSpeak, 1.0);
                SkillIntervals.Add(SkillName.Stealing, 10.0);
                SkillIntervals.Add(SkillName.TasteID, 1.0);
                SkillIntervals.Add(SkillName.Tracking, 10.0);
                SkillIntervals.Add(SkillName.Lockpicking, 3.0);
                SkillIntervals.Add(SkillName.Herding, 3.0);
                SkillIntervals.Add(SkillName.Camping, 3.0);
                SkillIntervals.Add(SkillName.Veterinary, 5.0);

                SkillIntervals.Add(SkillName.Alchemy, 1.25);
                SkillIntervals.Add(SkillName.Blacksmith, 1.25);
                SkillIntervals.Add(SkillName.Fletching, 1.25);
                SkillIntervals.Add(SkillName.Carpentry, 1.25);
                SkillIntervals.Add(SkillName.Cartography, 1.25);
                SkillIntervals.Add(SkillName.Cooking, 1.25);
                SkillIntervals.Add(SkillName.Inscribe, 1.25);
                SkillIntervals.Add(SkillName.Tailoring, 1.25);
                SkillIntervals.Add(SkillName.Tinkering, 1.25);

                SkillIntervals.Add(SkillName.Provocation, 10.0);
                SkillIntervals.Add(SkillName.Peacemaking, 10.0);
                SkillIntervals.Add(SkillName.Stealth, 10.0);

                SkillIntervals.Add(SkillName.AnimalTaming, 12.0);
                SkillIntervals.Add(SkillName.Begging, 10.0);

                SkillIntervals.Add(SkillName.Lumberjacking, 4.0);
                SkillIntervals.Add(SkillName.Mining, 4.0);
                SkillIntervals.Add(SkillName.Fishing, 4.0);

                SkillIntervals.Add(SkillName.Magery, 2.0);
                SkillIntervals.Add(SkillName.Musicianship, 5.0);

                SkillIntervals.Add(SkillName.Parry, 0.0);
                SkillIntervals.Add(SkillName.MagicResist, 0.0);

                SkillIntervals.Add(SkillName.Healing, -1.0);
                SkillIntervals.Add(SkillName.Tactics, -1.0);
                SkillIntervals.Add(SkillName.Archery, -1.0);
                SkillIntervals.Add(SkillName.Swords, -1.0);
                SkillIntervals.Add(SkillName.Macing, -1.0);
                SkillIntervals.Add(SkillName.Fencing, -1.0);
                SkillIntervals.Add(SkillName.Wrestling, -1.0);

                //Constant Modifiers
                SkillConstantMod.Add(SkillName.Anatomy, 8.0);
                SkillConstantMod.Add(SkillName.AnimalLore, 8.0);
                SkillConstantMod.Add(SkillName.ArmsLore, 6.0);
                SkillConstantMod.Add(SkillName.DetectHidden, 6.0);
                SkillConstantMod.Add(SkillName.Discordance, 4.0);
                SkillConstantMod.Add(SkillName.EvalInt, 10.0);
                SkillConstantMod.Add(SkillName.Forensics, 4.0);
                SkillConstantMod.Add(SkillName.Hiding, 5.0);
                SkillConstantMod.Add(SkillName.ItemID, 5.0);
                SkillConstantMod.Add(SkillName.Meditation, 50.0);
                SkillConstantMod.Add(SkillName.Poisoning, 8.0);
                SkillConstantMod.Add(SkillName.RemoveTrap, 20.0);
                SkillConstantMod.Add(SkillName.Snooping, 12.0);
                SkillConstantMod.Add(SkillName.SpiritSpeak, 5.0);
                SkillConstantMod.Add(SkillName.Stealing, 3.0);
                SkillConstantMod.Add(SkillName.TasteID, 8.0);
                SkillConstantMod.Add(SkillName.Tracking, 5.0);
                SkillConstantMod.Add(SkillName.Lockpicking, 6.0);
                SkillConstantMod.Add(SkillName.Herding, 8.0);
                SkillConstantMod.Add(SkillName.Camping, 4.0);
                SkillConstantMod.Add(SkillName.Veterinary, 6.0);
                SkillConstantMod.Add(SkillName.Alchemy, 9.0);
                SkillConstantMod.Add(SkillName.Blacksmith, 5.0);
                SkillConstantMod.Add(SkillName.Fletching, 6.0);
                SkillConstantMod.Add(SkillName.Carpentry, 6.0);
                SkillConstantMod.Add(SkillName.Cartography, 15.0);
                SkillConstantMod.Add(SkillName.Cooking, 20.0);
                SkillConstantMod.Add(SkillName.Inscribe, 30.0);
                SkillConstantMod.Add(SkillName.Tailoring, 8.0);
                SkillConstantMod.Add(SkillName.Tinkering, 13.0);
                SkillConstantMod.Add(SkillName.Provocation, 8.0);
                SkillConstantMod.Add(SkillName.Peacemaking, 12.0);
                SkillConstantMod.Add(SkillName.Stealth, 6.0);
                SkillConstantMod.Add(SkillName.AnimalTaming, 7.0);
                SkillConstantMod.Add(SkillName.Begging, 8.0);
                SkillConstantMod.Add(SkillName.Lumberjacking, 3.0);
                SkillConstantMod.Add(SkillName.Mining, 3.0);
                SkillConstantMod.Add(SkillName.Fishing, 4.0);
                SkillConstantMod.Add(SkillName.Magery, 40.0);
                SkillConstantMod.Add(SkillName.Musicianship, 10.0);
                SkillConstantMod.Add(SkillName.Parry, 8.0);
                SkillConstantMod.Add(SkillName.MagicResist, 80.0);
                SkillConstantMod.Add(SkillName.Healing, 10.0);
                SkillConstantMod.Add(SkillName.Tactics, 7.0);
                SkillConstantMod.Add(SkillName.Archery, 13.0);
                SkillConstantMod.Add(SkillName.Swords, 8.0);
                SkillConstantMod.Add(SkillName.Macing, 8.0);
                SkillConstantMod.Add(SkillName.Fencing, 8.0);
                SkillConstantMod.Add(SkillName.Wrestling, 8.0);
            }
        }

        public static class DisarmingMove
        {
            public static readonly double DelayUntilReuse = 20.0;
            public static readonly int ManaCost = 10;

            public static readonly double WrestlingSkillNeeded = 0.0;
            public static readonly double ArmsLoreSkillNeeded = 50.0;

            public static readonly string OnActivateText = "You prepare to disarm your opponent.";
            public static readonly string NotEnoughManaText = "You are not focused enough to disarm your opponent.";
            public static readonly string TimerNotElapsedText = "You must wait before attempting another disarming move.";
            public static readonly string SkillTooLowMessage = "You have no chance of disarming your opponent.";
            public static readonly string HandsNotFreeMessage = "You must have your hands free to attempt to disarm your opponent.";
        }

        public static class TreasureMapChest
        {
            public static readonly double RareChancePerLevel = 0.025;
        }

        // see story 46
        public static class AntiRail
        {
            public static readonly bool Enabled = true;
            public static readonly int PopUpFrequency = 200;    // Gump appears every X resource gathers
            public static readonly int MaxIncorrectGuesses = 2; // After X guesses, punish the player

            public static readonly bool RandomHue = false;
            public static readonly int MovementOffset = 8;
        }

        public static class SpellChanges
        {
            public static readonly double RecallCastTime = 3.5;         // seconds
            public static readonly int GateTravelCastTime = 4;          // seconds
            public static readonly int MindBlastDamage = 15;            // flat damage
            public static readonly int WallOfStoneRecastDelay = 20;     // seconds
            public static readonly int TeleportRecastDelay = 12;        // seconds
            public static readonly int TeleportRange = 8;               // tiles
            public static readonly int EigthLevelSummonCastTime = 5;
            public static readonly bool AOEAndFieldSpellsInTown = true;
            public static readonly double FireballCreatureDamageMod = 2.0; //Damage multiplier for players casting against creature
            public static readonly double MeteorSwarmMinDamagePerTarget = 15;
            public static readonly double MeteorSwarmDamageReductionPerTarget = 5;
            public static readonly double ChainLightningMinDamagePerTarget = 10;
            public static readonly double ChainLightningPercentDamageOneTileDistance = 0.75;
            public static readonly double ChainLightningPercentDamageTwoTileDistance = 0.5;

            public static readonly double MageryDamageMod = .35; //Change in spell damage per point of magery
            public static readonly double EvalIntDamageMod = .45; //Change in spell damage per point of eval int
            public static readonly double MagicResistDamageReductionMod = .25; //Reduction in spell damage per point of resist
            public static readonly double ResistFullModifier = 1.0; //How much each point of Resist Contributes towards earning Full ResistChance Amount
            public static readonly double BaseCreatureSpellDamageMultiplier = 2.0; //Multiplier of spell damage to BaseCreatures
            public static readonly double BaseCreatureResistMultiplier = 0.25; //Multiplier for BaseCreatures to Resist Chance for Spells With MagicResist
            public static readonly double BaseCreatureParalyzeMultiplier = 3.0; //Multipliers Length of Duration against Creatures (compared to players)
        }

        public static class WorldWars
        {
            public static readonly double BlockTimeStartAfterMidnightInHours = 2.0;
            public static readonly double BlockTimeInHours = 11.0;
            public static readonly double HoursBetweenEvents = 36.0;
            public static readonly double HourReductionPerEvent = 4.0;
            public static readonly double MinimumHoursBetweenEventsForReset = 13.0;

            public static readonly double GameLengthInHours = 1.0;
            public static readonly double RaiseFlagTimeInSeconds = 60.0;
        }

        public static class ExploPotions
        {
            public static readonly int MinDelay = 3;
            public static readonly int MaxDelay = 5;
        }

        public static class SpellDamageReductions
        {
            public static readonly double Lightning = 0.80;
            public static readonly double EnergyBolt = 0.80; //20 % decrease
            public static readonly double Explosion = 0.80;
            public static readonly double FlameStrike = 0.80;
            public static readonly double Fireball = 0.75;
        }

        public static class WeaponChanges
        {
            public static readonly double SwingDelayIncrease = 1.0;
            public static readonly double MinDamageIncrease = 1.0;
            public static readonly double MaxDamageDecrease = 1.0;

            public static readonly double StrBase = 100; //Assumed Str of Avg Player
            public static readonly double StrDamageImpact = .2; //How much variation from StrBase contributes to weapon Damage Bonus
            public static readonly double TacticsBase = 100; //Assumed Tactics of Avg Player
            public static readonly double TacticsDamageImpact = 0.75; //How much variation from TacticsBase contributes to weapon Damage Bonus
            public static readonly double AnatomyBase = 0; //Assumed Anatomy of Avg Player
            public static readonly double AnatomyDamageImpact = .2; //How much variation from AnatomyBase contributes to weapon Damage Bonus

            public static readonly int WeaponBaseDamageDefault = 20; //Temporary: Instead of using from Weapon
            public static readonly int WeaponVariationDefault = 4; //Temporary: Instead of using from Weapon   

            public static readonly double WeaponDamageModifier = 1.0; //How much finalized damage is modified

            public static readonly int ArmorDrainMaximum = 20; //Maximum AR penalty that can be applied to a mobile from ArmorDrain    
            public static readonly double ArmorDrainTimer = 30.0;//Seconds that must pass before mobile's ArmorDrain expires
            public static readonly double ArmorDrainModifier = 1.0; //Multiplier to ArmorDrain amounts            
            public static readonly double BaseCreatureArmorDrainModifier = 0.5; //Percentage of normal ArmorDrain BaseCreatures cause

            public static readonly double StaminaDrainModifier = 1.0; //Multiplier to StaminaDrain amounts
            public static readonly double BaseCreatureStaminaDrainModifier = 0.2; //Percentage of normal StaminaDrain BaseCreatures cause

            public static readonly double WeaponDurabilityLossChance = .05; //Percent chance weapon loses durability on hit
            public static readonly int HitPointsBreakWarning = 3; //Warning of possible breakage when weapon below this number of hitpoints

            public static readonly bool UseSwingAdjustment = true; //Use the SwingAdjustment mechanic for missed attacks
            public static readonly bool UseSwingAdjustmentPVP = false; //Whether SwingAdjustments work against other players
            public static readonly double SwingAdjustmentAccuracyBonusPerMiss = .05; //Bonus to hit per consecutive missed Swing            
            public static readonly double SwingAdjustmentTimer = 30.0; //Length of time SwingAdjustment is in effect after missed swing
            public static readonly int SwingAdjustmentMaxStackableBonuses = 5; //Maximum number of stackable bonuses

            public static readonly bool UseStealthAttack = true; //Whether to use Stealth Attacks

            public static readonly bool StealthAttackAccuracyPVP = false; //use Initial Attack Accuracy Bonus against players
            public static readonly bool StealthAttackDamagePVP = false;  //Add Timer Damage Bonus against players
            public static readonly double StealthAttackAccuracyBonus = .25; //Initial Attack Swing Accuracy Bonus           
            public static readonly double StealthAttack1HandedWeaponDamageBonus = 300; //1 Handed Stealth attack damage bonus
            public static readonly double StealthAttack2HandedWeaponDamageBonus = 200; //2 Handed Stealth attack damage bonus
            public static readonly double StealthAttackTimer = 0.5; //Length of Damage bonus in seconds           
        }

        public static class WaterTaxis
        {
            public static readonly double DestinationWaitTimeInMinutes = .75;
        }

        public static class TreasureMaps
        {
            public static readonly int GoldPerLevel = 1000;
            public static readonly int ItemsPerLevel = 2;
            public static readonly int ScrollsPerLevel = 3;
            public static readonly int GemsPerLevel = 3;
            public static readonly int ReagentStacksPerLevel = 2;
            public static readonly int ReagentStackAmountMinPerLevel = 12;
            public static readonly int ReagentStackAmountMaxPerLevel = 25;
        }
        public static class Vendors
        {
            public static readonly double VendorSellPriceDecrease = 3.0;
        }
        public static class Houses
        {
            public static readonly double ReturnPriceReduction = 0.90;
            public static readonly bool DeleteGuildStonesAutomatically = true;
        }

        public static class Harvesting
        {
            public static readonly double OreWeight = 4.0;
            public static readonly double LumberWeight = 1.0;
            public static readonly double HideWeight = 2.5;
            public static readonly double BoardWeight = 0.1;
            public static readonly double MiningSmeltingDifficultyDivider = 25;
        }

        //see story 66        
        public static class RegenRates
        {
            public static readonly double DefaultHitsRate = 6; //Seconds to regen 1 hit
            public static readonly double DefaultStamRate = 4; //Seconds to regen 1 stamina
            public static readonly double DefaultManaRate = 4; //Seconds to regen 1 mana
            public static readonly int StaminaForceWalk = 2; //When at or under amount this stamina, player must walk
        }

        //see story 66        
        public static class BaseCreatureAI
        {
            /* When beyond HomeRange, BaseCreature will walk randomly for RandomWalkLimit number of Actions.
             * Afterwards, BaseCreature will walk towards Spawner for WalkHomeLimit number of Actions.
             * Afterwards, if BaseCreature is still not in it's Spawner HomeRange, it will teleport to within HomeRange
             */
            public static readonly int DefaultHomeRange = 10;
            public static readonly int WalkRandomOutsideHomeLimit = 10; //Length of time creature will walk around outside home
            public static readonly int WalkTowardsHomeLimit = 20; //Length of time creature will attempt to walk home before teleporting
            public static readonly bool EnableSpeedTables = true;
            public static readonly double DecisionTimeDelay = 1.0; //Delay between decisions for AI
            public static readonly double TargetReacquireDelay = 3.0; //Delay required for mobile to pick a new target 
            public static readonly int SpellDelayMin = 1; //Minimum Delay Between Spells (For Randomizer)
            public static readonly int SpellDelayMax = 2; //Maximum Delay Between Spells (For Randomizer)
            public static readonly int CombatSpecialActionDelay = 3; //Minimum Delay Between Special Actions
            public static readonly int CreatureBandageSelfDuration = 10; //Seconds it takes for creature to bandage heal self
            public static readonly int CreatureBandageOtherDuration = 5; //Seconds it takes for creature to bandage heal other
            public static readonly int CombatHealActionDelay = 3; //Minimum Delay Between Heal Actions 
            public static readonly int WanderActionDelay = 10; //Minimum Delay Between Wander Actions   
            public static readonly int CreatureSpellCastRange = 12; //Maximum Casting Distance Allowed for Creature Spells
            public static readonly int CreatureSpellRange = 8; //Spell Range for AI Casting Preference
            public static readonly int CreatureWithdrawRange = 8; //Preferred Minimum Distance for Withdrawing without weapon (otherwise use weapon distance)
            public static readonly double StealthFootprintChance = .25; //Preferred Minimum Distance for Withdrawing without weapon (otherwise use weapon distance)
            public static readonly int BandageTimeoutLength = 10; //Seconds Allowed for a Creature to Try a Bandaging Action: Canceled if never reaches target after this duration
            public static readonly double GuardModeTargetDelay = 0.0; //Delay for creature in guard mode before acting on acquired target (Has 1 second inherent delay)
            public static readonly double WanderModeTargetDelay = 1.5; //Delay for creature in wander mode before acting on acquired target (Has 1 second inherent delay)
            public static readonly double FleeSpeedModifier = 1.5; //Decreases Fleeing Speed by this amount (higher is slower)
            public static readonly double LowManaPercent = 40; //If Creature is below this percent of mana, considered low on mana (will flag melee combat usually)
        }

        //see story 68        
        public static class PlayerMovement
        {
            public static readonly double StationaryRefreshTimer = 1; //Seconds player must be stationary to refresh Running Steps tracker
            public static readonly int RunningStepsForStaminaLoss = 20; //Steps running to lose 1 stamina
        }

        public static class Escortables
        {
            public static readonly int PaymentMin = 250;
            public static readonly int PaymentMax = 400;
            public static readonly double BlockTimeInMinutes = 5.0;
            public static readonly double DeleteTimeAfterCompletion = 0.5; //In Minutes
            public static readonly double DeleteTimeAfterAcceptance = 30; //In Minutes
        }

        public static class BaseCreatureCombat
        {
            public static bool BaseCreaturesGetWeaponDamage = false; //Whether non-players (Npcs) get equipped weapon damage added to base SetDamage values
            public static bool BaseCreaturesGetStrDamage = false; //Whether non-players add Str to damage calculations
            public static bool BaseCreaturesGetAnatomyDamage = true; //Whether non-players add Anatomy bonus to damage calculations
            public static bool BaseCreaturesGetEquippedArmorValues = false; //Whether non-players add AR from equipped armor to Virtual Armor

            public static readonly double CreatureMaxHPBreathDamageScalar = .040; //Damage per 1 MaxHP Creature Has
        }

        public static class CombatTesting
        {
            public static bool ShowDamageFormula = false; //When player hits something it shows message: "Enemy AR: (Initial damage) - (damage amount reduced by armor) - (damage reduced by parry) = Final damage"
        }

        public static class Poison
        {
            public static readonly double PoisoningDifficultyDivider = 25; //  (100 / Divider) = % success chance per [skill - difficulty]
            public static readonly double LesserPoisonCureChance = 2.25; //Chance to cure Lesser Poison per point of Magery
            public static readonly double RegularPoisonCureChance = 1.75; //Chance to cure Poison per point of Magery
            public static readonly double GreaterPoisonCureChance = 1.25; //Chance to cure Greater Poison per point of Magery
            public static readonly double DeadlyPoisonCureChance = .70; //Chance to cure Deadly Poison per point of Magery
            public static readonly double LethalPoisonCureChance = .40; //Chance to cure Lethal Poison per point of Magery

            public static readonly double LesserCurePotionEffectivness = 10.0; //Attempts to cure at this effective level magery
            public static readonly double CurePotionEffectivness = 30.0; //Attempts to cure at this effective level magery
            public static readonly double GreaterCurePotionEffectivness = 90.0; //Attempts to cure at this effective level magery

            public static readonly double ArchCureEffectivenessMod = 2.00; //Attempts to cure at magery * this amount

            public static readonly Boolean TamedCreaturesPoisonCooldown = true; //Whether tamed creatures that inflict poison have a cooldown before they can poison again
            public static readonly double TamedCreaturesPoisonCooldownPerLevel = 5.0; //Seconds per level of poison inflicted before another poison effect can fire off
            public static readonly Boolean CreaturesPoisonCooldown = false; //Whether normal creatures that inflict poison have a cooldown before they can poison again
            public static readonly double CreaturesPoisonCooldownPerLevel = 3.0; //Seconds per level of poison inflicted before another poison effect can fire off
        }

        public static class SpellDelays
        {
            public static readonly TimeSpan DisruptDelay = TimeSpan.FromSeconds(.4);
            public static readonly TimeSpan RecastDelay = TimeSpan.FromSeconds(0.75);
            public static readonly TimeSpan DefaultDamageDelay = TimeSpan.FromSeconds(0.5);
        }

        public static class BardChanges
        {
            public static readonly double BardDifficultyModifier = 25;  // (100 / Mod) = % increase in difficulty for 1 point of skill difference between SkillValue and MinTamingSkill
            public static readonly double BardDifficultySkillRangeMultiplier = 2.5; //MinProvoke Skill * This = Maximum Provoke Value Possible for Skill Gain
            public static readonly double ProvocationDuration = 20; //Maximum number of seconds creatures can be provoked 
            public static readonly double PeacemakingDuration = 20; // Maximum number of seconds creatures can be peaced
            public static readonly double ProvocationMaximumSuccessRate = .95; //Minimum % chance of failure always
            public static readonly double PeacemakingMaximumSuccessRate = .95; //Minimum % chance of failure always
            public static readonly int MaxProvocationDifficulty = 128; //Maximum Difficulty at GM Provoke
            public static readonly int MaxPeacemakingDifficulty = 132; //Maximum Difficulty at GM Peacemaking
            public static readonly int PeacemakingBonus = 10; //Reduces skill needed for peacemaking (should be easier than provocation)
            public static readonly int PeacemakingAggressiveTimerReduction = 1; //Seconds taken off peacemaking timer for doing aggressive actions to creature
        }

        public static class Followers
        {
            public static readonly int DefaultPlayerFollowers = 2;  // Default Control Slots for Players
            public static readonly int DefaultBaseCreatureFollowers = 5;  // Default Control Slots for BaseCreatures
            public static readonly int TotalMaxFollowers = 5;  // Maximum Control Slots For Players (With Bonuses)
            public static readonly int AnimaLoreBonusControlSlotDivisor = 33;  // Bonus Slots Allowed For Divisor of Skill
            public static readonly int SpiritSpeakBonusControlSlotDivisor = 33;  // Bonus Slots Allowed For Divisor of Skill
            public static readonly int SummonBaseDuration = 180; //Base Time Length Summons Exist
            public static readonly int SpiritSpeakSummonDurationBonus = 120; //Bonus Seconds for Summon Duration at 100 Skill (Scaled)
            public static readonly double HerdingDamageBonus = 20; //Percent Bonus to Follower Damage
            public static readonly double HerdingSpellDamageBonus = 20; //Percent Bonus to Follower SpellDamage
            public static readonly double ReleaseDeleteTimer = .0333; //Hours after release that an creature is deleted  
            public static readonly double AnimalTamingDifficultyModifier = 25; // (100 / Mod) = % increase in difficulty for 1 point of skill difference between SkillValue and MinTamingSkill

        }

        public static class Stealing
        {
            public static readonly double StealingMaxSuccessChance = .95; //Maximum success rate
            public static readonly double StackWeightStealDivider = 2; //Amount stolen from a stack is normal weight amount divided by this
        }

        public static class FameKarma
        {
            public static readonly int PartyFameKarmaSharingDisance = 30; //Distance players in party must be to monster corpse to gain fame/karma from it
        }

        public static class Healing
        {
            public static readonly double BandageSlipImpact = .03; //Percent reduction per slip
            public static readonly double BandageMaxSlipImpact = .40; //Max reduction from slips
            public static readonly double RessurrectionSuccessAtGM = .80; //Percent chance of Ressurrection at GM Skill 
            public static readonly double HealingRequiredForDefiniteSuccess = 50;
        }

        public static class Database
        {
            public static readonly bool Active = false; // when debugging disable this or you'll need the database on your system
            public static readonly string WebServerIP = "255.255.255.255";
            public static readonly double CharacterUpdateTimeInSeconds = 120.0;
        }

        public static int CharactersPerAccount = 3;
    }
}
