using System;
using Server.Items;

namespace Server.Engines.Craft
{
    #region Mondain's Legacy
    public enum BowRecipes
    {
        //magical
        BarbedLongbow = 200,
        SlayerLongbow = 201,
        FrozenLongbow = 202,
        LongbowOfMight = 203,
        RangersShortbow = 204,
        LightweightShortbow = 205,
        MysticalShortbow = 206,
        AssassinsShortbow = 207,

        // arties
        BlightGrippedLongbow = 250,
        FaerieFire = 251,
        SilvanisFeywoodBow = 252,
        MischiefMaker = 253,
        TheNightReaper = 254,
    }
    #endregion

    public class DefBowFletching : CraftSystem
    {
        public override SkillName MainSkill
        {
            get
            {
                return SkillName.Bowcraft;
            }
        }

        public override int GumpTitleNumber
        {
            get
            {
                return 1044006;
            }// <CENTER>BOWCRAFT AND FLETCHING MENU</CENTER>
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefBowFletching();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.5; // 50%
        }

        private DefBowFletching()
            //: base(1, 1, 1.25)// base( 1, 2, 1.7 )
            : base(DefBlacksmithy.MIN_ANIM, DefBlacksmithy.MAX_ANIM, DefBlacksmithy.DELAY)
        {
        }

        public override void PlayCraftEffect(Mobile from)
        {
            // no animation
            //if ( from.Body.Type == BodyType.Human && !from.Mounted )
            //	from.Animate( 9, 5, 1, true, false, 0 );
            from.PlayAttackAnimation(); //Adicionada animação
            from.PlaySound(0x23D);
        }


        public override int CanCraft(Mobile from, ITool tool, Type itemType)
        {
            int num = 0;

            if (tool == null || tool.Deleted || tool.UsesRemaining <= 0)
                return 1044038; // You have worn out your tool!
            else if (!tool.CheckAccessible(from, ref num))
                return num; // The tool must be on your person to use.

            return 0;
        }

      

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (failed)
            {
                if (lostMaterial)
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                else
                    return 1044157; // You failed to create the item, but no materials were lost.
            }
            else
            {
                if (quality == 0)
                    return 502785; // You were barely able to make this item.  It's quality is below average.
                else if (makersMark && quality == 2)
                    return 1044156; // You create an exceptional quality item and affix your maker's mark.
                else if (quality == 2)
                    return 1044155; // You create an exceptional quality item.
                else
                    return 1044154; // You create the item.
            }
        }

        public override CraftECA ECA
        {
            get
            {
                return CraftECA.FiftyPercentChanceMinusTenPercent;
            }
        }

        public override void InitCraftList()
        {
            int index = -1;

            // Materials
            if (Core.SA)
            {
                index = AddCraft(typeof(ElvenFletching), 1044457, 1113346, 90.0, 130.0, typeof(Feather), 1044562, 20, 1044563);
                AddRes(index, typeof(FaeryDust), 1113358, 1, 1044253);
            }

            this.AddCraft(typeof(Kindling), 1044457, 1023553, 0.0, 00.0, typeof(Board), 1044041, 1, 1044351);

            index = this.AddCraft(typeof(Shaft), 1044457, 1027124, 0.0, 65.0, typeof(Board), 1044041, 10, 1044351);
            //this.SetUseAllRes(index, true);

            // Ammunition
            index = this.AddCraft(typeof(Arrow), 1044565, 1023903, 0.0, 40.0, typeof(Shaft), 1044560, 1, 1044561);
            this.AddRes(index, typeof(Feather), 1044562, 1, 1044563);
            this.SetUseAllRes(index, true);

            index = this.AddCraft(typeof(Bolt), 1044565, 1027163, 0.0, 40.0, typeof(Shaft), 1044560, 1, 1044561);
            this.AddRes(index, typeof(Feather), 1044562, 1, 1044563);
            this.SetUseAllRes(index, true);

            if (Core.SE)
            {
                index = AddCraft(typeof(FukiyaDarts), 1044565, 1030246, 50.0, 90.0, typeof(Board), 1044041, 1, 1044351);
                this.SetUseAllRes(index, true);
            }

            // Weapons
            this.AddCraft(typeof(PraticeBow), 1044566, 1025042, 30.0, 70.0, typeof(Board), 1044041, 20, 1044351);
            this.AddCraft(typeof(Bow), 1044566, 1025042, 30.0, 70.0, typeof(Board), 1044041, 7, 1044351);
            this.AddCraft(typeof(Crossbow), 1044566, 1023919, 60.0, 90, typeof(Board), 1044041, 7, 1044351);
            this.AddCraft(typeof(HeavyCrossbow), 1044566, 1025117, 70.0, 100, typeof(Board), 1044041, 10, 1044351);

            index = this.AddCraft(typeof(CompositeBow), 1044566, 1029922, 65.0, 105, typeof(Board), 1044041, 8, 1044351);
            this.AddRes(index, typeof(Cloth), "Pano", 5, "Voce precisa de Pano para fazer isto");

            this.AddCraft(typeof(RepeatingCrossbow), 1044566, 1029923, 80.0, 100, typeof(Board), 1044041, 10, 1044351);
            index = AddCraft(typeof(Yumi), 1044566, 1030224, 70.0, 95, typeof(Board), 1044041, 10, 1044351);


            #region Mondain's Legacy
            if (Core.ML)
            {
                index = AddCraft(typeof(ElvenCompositeLongbow), 1044566, 1031562, 95.0, 145.0, typeof(Board), 1044041, 20, 1044351);
                index = AddCraft(typeof(MagicalShortbow), 1044566, 1031551, 85.0, 135.0, typeof(Board), 1044041, 15, 1044351);

                index = AddCraft(typeof(BlightGrippedLongbow), 1044566, 1072907, 75.0, 125.0, typeof(Board), 1044041, 20, 1044351);
                AddRes(index, typeof(LardOfParoxysmus), 1032681, 1, 1053098);
                AddRes(index, typeof(Blight), 1032675, 10, 1053098);
                AddRes(index, typeof(Corruption), 1032676, 10, 1053098);
                AddRecipe(index, (int)BowRecipes.BlightGrippedLongbow);
                ForceNonExceptional(index);

                index = AddCraft(typeof(FaerieFire), 1044566, 1072908, 75.0, 125.0, typeof(Board), 1044041, 20, 1044351);
                AddRes(index, typeof(LardOfParoxysmus), 1032681, 1, 1053098);
                AddRes(index, typeof(Putrefication), 1032678, 10, 1053098);
                AddRes(index, typeof(Taint), 1032679, 10, 1053098);
                AddRecipe(index, (int)BowRecipes.FaerieFire);
                ForceNonExceptional(index);

                index = AddCraft(typeof(SilvanisFeywoodBow), 1044566, 1072955, 75.0, 125.0, typeof(Board), 1044041, 20, 1044351);
                AddRes(index, typeof(LardOfParoxysmus), 1032681, 1, 1053098);
                AddRes(index, typeof(Scourge), 1032677, 10, 1053098);
                AddRes(index, typeof(Muculent), 1032680, 10, 1053098);
                AddRecipe(index, (int)BowRecipes.SilvanisFeywoodBow);
                ForceNonExceptional(index);

                index = AddCraft(typeof(MischiefMaker), 1044566, 1072910, 75.0, 125.0, typeof(Board), 1044041, 15, 1044351);
                AddRes(index, typeof(DreadHornMane), 1032682, 1, 1053098);
                AddRes(index, typeof(Corruption), 1032676, 10, 1053098);
                AddRes(index, typeof(Putrefication), 1032678, 10, 1053098);
                AddRecipe(index, (int)BowRecipes.MischiefMaker);
                ForceNonExceptional(index);

                index = AddCraft(typeof(TheNightReaper), 1044566, 1072912, 75.0, 125.0, typeof(Board), 1044041, 10, 1044351);
                AddRes(index, typeof(DreadHornMane), 1032682, 1, 1053098);
                AddRes(index, typeof(Blight), 1032675, 10, 1053098);
                AddRes(index, typeof(Scourge), 1032677, 10, 1053098);
                AddRecipe(index, (int)BowRecipes.TheNightReaper);
                ForceNonExceptional(index);

                index = AddCraft(typeof(BarbedLongbow), 1044566, 1073505, 75.0, 125.0, typeof(Board), 1044041, 20, 1044351);
                AddRes(index, typeof(FireRuby), 1026254, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.BarbedLongbow);

                index = AddCraft(typeof(SlayerLongbow), 1044566, 1073506, 75.0, 125.0, typeof(Board), 1044041, 20, 1044351);
                AddRes(index, typeof(BrilliantAmber), 1026256, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.SlayerLongbow);

                index = AddCraft(typeof(FrozenLongbow), 1044566, 1073507, 75.0, 125.0, typeof(Board), 1044041, 20, 1044351);
                AddRes(index, typeof(Turquoise), 1026250, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.FrozenLongbow);

                index = AddCraft(typeof(LongbowOfMight), 1044566, 1073508, 75.0, 125.0, typeof(Board), 1044041, 10, 1044351);
                AddRes(index, typeof(BlueDiamond), 1026255, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.LongbowOfMight);

                index = AddCraft(typeof(RangersShortbow), 1044566, 1073509, 75.0, 125.0, typeof(Board), 1044041, 15, 1044351);
                AddRes(index, typeof(PerfectEmerald), 1026251, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.RangersShortbow);

                index = AddCraft(typeof(LightweightShortbow), 1044566, 1073510, 75.0, 125.0, typeof(Board), 1044041, 15, 1044351);
                AddRes(index, typeof(WhitePearl), 1026253, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.LightweightShortbow);

                index = AddCraft(typeof(MysticalShortbow), 1044566, 1073511, 75.0, 125.0, typeof(Board), 1044041, 15, 1044351);
                AddRes(index, typeof(EcruCitrine), 1026252, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.MysticalShortbow);

                index = AddCraft(typeof(AssassinsShortbow), 1044566, 1073512, 75.0, 125.0, typeof(Board), 1044041, 15, 1044351);
                AddRes(index, typeof(DarkSapphire), 1026249, 1, 1053098);
                AddRecipe(index, (int)BowRecipes.AssassinsShortbow);
            }

            this.SetSubRes(typeof(Board), 1072643);

            // Add every material you want the player to be able to choose from
            // This will override the overridable material	TODO: Verify the required skill amount
            AddSubRes(typeof(Board), "Cedro", 00.0, "Voce nao sabe usar esta madeira");
            AddSubRes(typeof(OakBoard), "Carvalho", 60.0, "Voce nao sabe usar esta madeira");
            AddSubRes(typeof(AshBoard), "Pinho", 70.0, "Voce nao sabe usar esta madeira");
            AddSubRes(typeof(YewBoard), "Mogno", 80.0, "Voce nao sabe usar esta madeira");
            AddSubRes(typeof(HeartwoodBoard), "Eucalipto", 90.0, "Voce nao sabe usar esta madeira");
            AddSubRes(typeof(BloodwoodBoard), "Carmesim", 101.0, "Voce nao sabe usar esta madeira");
            AddSubRes(typeof(FrostwoodBoard), "Gelo", 106.0, "Voce nao sabe usar esta madeira");
            #endregion

            this.Resmelt = true;
            this.MarkOption = true;
            this.Repair = true;
            this.CanEnhance = Core.ML;
        }
    }
}
