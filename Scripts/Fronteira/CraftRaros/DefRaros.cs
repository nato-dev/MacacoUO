using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Server;
using Server.Custom.RaresCrafting;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.RaresCrafting
{
    ////////////////////////////////////////////////////////////////
    // All craftable rares
    ////////////////////////////////////////////////////////////////
    public class RareDefinitions
    {
        public static ICraftableRare DecoRandom()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.ItemID,
                Result = new CraftableEntry() { m_Name = "caixa misteriosa", m_ItemId = 0xA761 },
                Ingredients = new CraftableEntry[2]
                {
                    new CraftableEntry(){ m_Name = "caixa misteriosa", m_AmountRequired = 1, m_ItemId = 0x9F64 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }

        public static ICraftableRare DecoRandom2()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.ItemID,
                Result = new CraftableEntry() { m_Name = "decoracao aleatoria", m_ItemId = 0xA761 },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "lingotes de ferro", m_AmountRequired = 1000, m_ItemId = 0x1BF2 },
                    new CraftableEntry(){ m_Name = "tabuas", m_AmountRequired = 1000, m_ItemId = 0x1BD7 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 20, m_ItemId = 0x5745 }
                },
            };
        }


        ////////////////////////////////////////////////////////////////
        // Alchemy
        public static ICraftableRare AlchemyFlask1()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Alchemy,
                Result = new CraftableEntry() { m_Name = "frasco", m_ItemId = 0x182A },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "tubo", m_AmountRequired = 1, m_ItemId = 0x21FE },
                    new CraftableEntry(){ m_Name = "frasco", m_AmountRequired = 1, m_ItemId = 0x182D },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 },
                },
            };
        }
        public static ICraftableRare AlchemyFlask2()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Alchemy,
                Result = new CraftableEntry() { m_Name = "frasco", m_ItemId = 0x182B },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "tubo", m_AmountRequired = 1, m_ItemId = 0x21FE },
                    new CraftableEntry(){ m_Name = "frasco", m_AmountRequired = 1, m_ItemId = 0x182D },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        public static ICraftableRare AlchemyFlask3()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Alchemy,
                Result = new CraftableEntry() { m_Name = "frasco", m_ItemId = 0x182C },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "tubo", m_AmountRequired = 1, m_ItemId = 0x21FE },
                    new CraftableEntry(){ m_Name = "frasco", m_AmountRequired = 1, m_ItemId = 0x182D },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        ////////////////////////////////////////////////////////////////
        // Bowcrafting
        public static ICraftableRare DecorativeBowAndArrows()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Carpentry,
                Result = new CraftableEntry() { m_Name = "arma decorativa", m_ItemId = 0x155c },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "flechas", m_AmountRequired = 5, m_ItemId = 0x0f40 },
                    new CraftableEntry(){ m_Name = "tabua de encaixe", m_AmountRequired = 1, m_ItemId = 0x0c39 },
                    new CraftableEntry(){ m_Name = "arco", m_AmountRequired = 1, m_ItemId = 0x13B2 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 2, m_ItemId = 0x5745 }
                },
                DispOffsetY = -25,
                DispOffsetX = -20,
            };
        }
        public static ICraftableRare BundleOfArrows()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Carpentry,
                Result = new CraftableEntry() { m_Name = "monte de flechas", m_ItemId = 0x0f41 },
                Ingredients = new CraftableEntry[2]
                {
                    new CraftableEntry(){ m_Name = "flechas", m_AmountRequired = 3, m_ItemId = 0x0f40 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        public static ICraftableRare BundleOfBolts()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Carpentry,
                Result = new CraftableEntry() { m_Name = "monte de dardos", m_ItemId = 0x1bfd },
                Ingredients = new CraftableEntry[2]
                {
                    new CraftableEntry(){ m_Name = "dardos", m_AmountRequired = 3, m_ItemId = 0x1bfc },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        ////////////////////////////////////////////////////////////////
        // Blacksmithing
        public static ICraftableRare DecorativeHalberd()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Blacksmith,
                Result = new CraftableEntry() { m_Name = "arma decorativa", m_ItemId = 0x1560 },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "corda", m_AmountRequired = 2, m_ItemId = 0x14f8 },
                    new CraftableEntry(){ m_Name = "tabua de encaixe", m_AmountRequired = 1, m_ItemId = 0x0c39 },
                    new CraftableEntry(){ m_Name = "alabarda", m_AmountRequired = 1, m_ItemId = 0x143E },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 2, m_ItemId = 0x5745 }
                },
                DispOffsetY = -25,
                DispOffsetX = -20,
            };
        }
        public static ICraftableRare HangingChainmailLeggings()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Blacksmith,
                Result = new CraftableEntry() { m_Name = "calcas de malha", m_ItemId = 0x13BC },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "correntes", m_AmountRequired = 1, m_ItemId = 0x1a07 },
                    new CraftableEntry(){ m_Name = "tabua de encaixe", m_AmountRequired = 1, m_ItemId = 0x0c39 },
                    new CraftableEntry(){ m_Name = "calcas de malha", m_AmountRequired = 1, m_ItemId = 0x13BE },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 3, m_ItemId = 0x5745 }
                },
                DispOffsetY = -10,
            };
        }
        public static ICraftableRare GoldIngots()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Blacksmith,
                Result = new CraftableEntry() { m_Name = "lingotes de ouro", m_ItemId = 0x1BEE },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "lingotes de ouro", m_AmountRequired = 5, m_ItemId = 0x1BEA },
                    new CraftableEntry(){ m_Name = "cera de abelha", m_AmountRequired = 1, m_ItemId = 0x1426 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        public static ICraftableRare CopperIngots()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Blacksmith,
                Result = new CraftableEntry() { m_Name = "lingotes de cobre", m_ItemId = 0x1BE5 },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "lingotes de cobre", m_AmountRequired = 5, m_ItemId = 0x1BE4 },
                    new CraftableEntry(){ m_Name = "cera de abelha", m_AmountRequired = 1, m_ItemId = 0x1426 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        ////////////////////////////////////////////////////////////////
        // Carpentry
        public static ICraftableRare DartboardWithAxe()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Carpentry,
                SecondReqSkillId = SkillName.Lumberjacking,
                Result = new CraftableEntry() { m_Name = "dardos", m_ItemId = 0x1E30 },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "dardos", m_AmountRequired = 1, m_ItemId = 0x1E2E },
                    new CraftableEntry(){ m_Name = "machado", m_AmountRequired = 2, m_ItemId = 0xf49 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 2, m_ItemId = 0x5745 }
                },
                DispOffsetX = -10,
            };
        }
        public static ICraftableRare RuinedBookcase()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Carpentry,
                Result = new CraftableEntry() { m_Name = "livros velhos", m_ItemId = 0x0c15 },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "livro", m_AmountRequired = 10, m_ItemId = 0x0FBD },
                    new CraftableEntry(){ m_Name = "martelo de ferreiro", m_AmountRequired = 2, m_ItemId = 0xfb5 },
                    new CraftableEntry(){ m_Name = "armario", m_AmountRequired = 1, m_ItemId = 0xa4f },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 3, m_ItemId = 0x5745 }
                },
                DispOffsetY = -25,
            };
        }
        public static ICraftableRare CoveredChair()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Carpentry,
                SecondReqSkillId = SkillName.Tailoring,
                Result = new CraftableEntry() { m_Name = "covered chair", m_ItemId = 0x0c17 },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "lencol", m_AmountRequired = 4, m_ItemId = 0x0A92 },
                    new CraftableEntry(){ m_Name = "cadeira quebrada", m_AmountRequired = 2, m_ItemId = 0x0C1C },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 4, m_ItemId = 0x5745 }

                },
                DispOffsetY = -15,
                DispOffsetX = -40,
            };
        }
        public static ICraftableRare LogPileLarge()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Carpentry,
                SecondReqSkillId = SkillName.Lumberjacking,
                Result = new CraftableEntry() { m_Name = "pilha de toras", m_ItemId = 0x1BE2 },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "toras", m_AmountRequired = 3, m_ItemId = 0x1BE1 },
                    new CraftableEntry(){ m_Name = "toras", m_AmountRequired = 20, m_ItemId = 0x1BDD },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
                DispOffsetY = -10,
            };
        }
        ////////////////////////////////////////////////////////////////
        // Cooking
        public static ICraftableRare PotOfWax()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Cooking,
                Result = new CraftableEntry() { m_Name = "potes de cera", m_ItemId = 0x142B },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "pote", m_AmountRequired = 1, m_ItemId = 0x09E0 },
                    new CraftableEntry(){ m_Name = "cera de abelha", m_AmountRequired = 2, m_ItemId = 0x1426 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        public static ICraftableRare KettleOfWax()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Cooking,
                Result = new CraftableEntry() { m_Name = "pote com cera", m_ItemId = 0x142A },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "pote", m_AmountRequired = 1, m_ItemId = 0x9ED },
                    new CraftableEntry(){ m_Name = "cera de abelha", m_AmountRequired = 5, m_ItemId = 0x1426 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 2, m_ItemId = 0x5745 }
                },
            };
        }
        public static ICraftableRare DirtyPan()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Cooking,
                Result = new CraftableEntry() { m_Name = "panela suja", m_ItemId = 0x9DE },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "frigideira", m_AmountRequired = 1, m_ItemId = 0x97f },
                    new CraftableEntry(){ m_Name = "coco de cavalo", m_AmountRequired = 2, m_ItemId = 0x0F3B },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        ////////////////////////////////////////////////////////////////
        // Inscription
        public static ICraftableRare DamagedBooks()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Inscribe,
                Result = new CraftableEntry() { m_Name = "livros danificados", m_ItemId = 0x0C16 },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "livro", m_AmountRequired = 10, m_ItemId = 0x0FBD },
                    new CraftableEntry(){ m_Name = "tesoura", m_AmountRequired = 2, m_ItemId = 0xf9f },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 2, m_ItemId = 0x5745 }
                },
                DispOffsetY = -10,
                DispOffsetX = -30,
            };
        }
        public static ICraftableRare BookPile1()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Inscribe,
                Result = new CraftableEntry() { m_Name = "livros", m_ItemId = 0x1E21 },
                Ingredients = new CraftableEntry[2]
                {
                    new CraftableEntry(){ m_Name = "livro", m_AmountRequired = 2, m_ItemId = 0x0FF4 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
                DispOffsetX = -10,
            };
        }
        public static ICraftableRare BookPile2()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Inscribe,
                Result = new CraftableEntry() { m_Name = "livros", m_ItemId = 0x1E25 },
                Ingredients = new CraftableEntry[2]
                {
                    new CraftableEntry(){ m_Name = "livro", m_AmountRequired = 5, m_ItemId = 0x0FF4 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
            };
        }
        public static ICraftableRare ForbiddenWritings()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Inscribe,
                SecondReqSkillId = SkillName.Magery,
                Result = new CraftableEntry() { m_Name = "escrituras proibidas", m_ItemId = 0x2253 },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "palavras proibidas", m_AmountRequired = 10, m_ItemId = 0x2265 },
                    new CraftableEntry(){ m_Name = "livro de magias", m_AmountRequired = 1, m_ItemId = 0x0EFA },
                    new CraftableEntry(){ m_Name = "tocha", m_AmountRequired = 1, m_ItemId = 0xf6b },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 2, m_ItemId = 0x5745 }
                },
            };
        }
        ////////////////////////////////////////////////////////////////
        // Tailoring
        public static ICraftableRare LargeFishingNet()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Tailoring,
                Result = new CraftableEntry() { m_Name = "rede", m_ItemId = 0x1EA5 },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "rede", m_AmountRequired = 10, m_ItemId = 0x0DCA },
                    new CraftableEntry(){ m_Name = "rede", m_AmountRequired = 10, m_ItemId = 0x0DCB },
                    new CraftableEntry(){ m_Name = "tesoura", m_AmountRequired = 2, m_ItemId = 0xf9f },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 3, m_ItemId = 0x5745 }
                },
                DispOffsetY = -40,
                DispOffsetX = -40,
            };
        }
        public static ICraftableRare DyeableCurtainEast()
        {
            return new CraftableCurtainEast()
            {
                FirstReqSkillId = SkillName.Tailoring,
                Result = new CraftableEntry() { m_Name = "cortina (pintavel)", m_ItemId = 0x160D },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "lencol", m_AmountRequired = 4, m_ItemId = 0x0A92 },
                    new CraftableEntry(){ m_Name = "arame de ferro", m_AmountRequired = 1, m_ItemId = 0x1876 },
                    new CraftableEntry(){ m_Name = "kit de costura", m_AmountRequired = 1, m_ItemId = 0x0F9D },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 4, m_ItemId = 0x5745 }
                },
                DispOffsetY = -40,
                DispOffsetX = -40,
            };
        }
        public static ICraftableRare DyeableCurtainSouth()
        {
            return new CraftableCurtainSouth()
            {
                FirstReqSkillId = SkillName.Tailoring,
                Result = new CraftableEntry() { m_Name = "cortina (pintavel)", m_ItemId = 0x160E },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "lencol", m_AmountRequired = 4, m_ItemId = 0x0A92 },
                    new CraftableEntry(){ m_Name = "arame de ferro", m_AmountRequired = 1, m_ItemId = 0x1876 },
                    new CraftableEntry(){ m_Name = "kit de costura", m_AmountRequired = 1, m_ItemId = 0x0F9D },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 4, m_ItemId = 0x5745 }
                },
                DispOffsetY = -40,
                DispOffsetX = -40,
            };
        }
        ////////////////////////////////////////////////////////////////
        // Tinkering
        public static ICraftableRare Anchor()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Tinkering,
                Result = new CraftableEntry() { m_Name = "ancora", m_ItemId = 0x14F7 },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "corda", m_AmountRequired = 2, m_ItemId = 0x14f8 },
                    new CraftableEntry(){ m_Name = "lingotes de ferro", m_AmountRequired = 5, m_ItemId = 0x1BF2 },
                    new CraftableEntry(){ m_Name = "arame de ferro", m_AmountRequired = 2, m_ItemId = 0x1876 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 3, m_ItemId = 0x5745 }
                },
                DispOffsetY = -20,
            };
        }
        public static ICraftableRare HangingSkeleton1()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Tinkering,
                Result = new CraftableEntry() { m_Name = "esqueleto", m_ItemId = 0x1B7F },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "correntes", m_AmountRequired = 4, m_ItemId = 0x1a07 },
                    new CraftableEntry(){ m_Name = "taboa de encaixe", m_AmountRequired = 1, m_ItemId = 0x0c39 },
                    new CraftableEntry(){ m_Name = "esqueleto", m_AmountRequired = 1, m_ItemId = 0x1D8F },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 5, m_ItemId = 0x5745 }
                },
                DispOffsetY = -30,
            };
        }
        public static ICraftableRare Hook()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Tinkering,
                Result = new CraftableEntry() { m_Name = "gancho", m_ItemId = 0x1E9A },
                Ingredients = new CraftableEntry[3]
                {
                    new CraftableEntry(){ m_Name = "corda", m_AmountRequired = 2, m_ItemId = 0x14f8 },
                    new CraftableEntry(){ m_Name = "lingotes de ferro", m_AmountRequired = 2, m_ItemId = 0x1BF2 },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 1, m_ItemId = 0x5745 }
                },
                DispOffsetY = -10,
                DispOffsetX = -40,
            };
        }
        public static ICraftableRare HangingCauldron()
        {
            return new CraftableRare()
            {
                FirstReqSkillId = SkillName.Tinkering,
                SecondReqSkillId = SkillName.Blacksmith,
                Result = new CraftableEntry() { m_Name = "caldeirao", m_ItemId = 0x0975 },
                Ingredients = new CraftableEntry[4]
                {
                    new CraftableEntry(){ m_Name = "correntes", m_AmountRequired = 2, m_ItemId = 0x1a07 },
                    new CraftableEntry(){ m_Name = "lingotes de ferro", m_AmountRequired = 5, m_ItemId = 0x1BF2 },
                    new CraftableEntry(){ m_Name = "pote", m_AmountRequired = 1, m_ItemId = 0x9ED },
                    new CraftableEntry(){ m_Name = "pozinho transformador", m_AmountRequired = 5, m_ItemId = 0x5745 }
                },
                DispOffsetY = -35,
            };
        }
    }

    /// <summary>
    /// Common definition for all craftable rares
    /// </summary>
    class CraftableRare : ICraftableRare
    {
        // total hack, using ninjutsu as the "not set" value. Because ninjas are cool.
        private static SkillName NO_SKILL_REQUIREMENT = SkillName.ItemID;
        private SkillName m_FirstRequiredSkillId;
        private SkillName m_SecondRequiredSkillId;
        private int m_NumDustsRequired;

        public SkillName FirstReqSkillId
        {
            set
            {
                m_FirstRequiredSkillId = value;
                m_FirstRequiredSkill = value != NO_SKILL_REQUIREMENT ? SkillInfo.Table[(int)value].Name : "";
            }

            get
            {
                return m_FirstRequiredSkillId;
            }
        }
        public SkillName SecondReqSkillId
        {
            set
            {
                m_SecondRequiredSkillId = value;
                m_SecondRequiredSkill = value != NO_SKILL_REQUIREMENT ? SkillInfo.Table[(int)value].Name : "";
            }

            get
            {
                return m_SecondRequiredSkillId;
            }
        }

        public CraftableEntry[] Ingredients;
        public CraftableEntry Result;

        public CraftableRare()
        {
            FirstReqSkillId = NO_SKILL_REQUIREMENT;
            SecondReqSkillId = NO_SKILL_REQUIREMENT;
        }
        public override bool MeetsRequiredSkillLevel_1(Mobile mob)
        {
            return FirstReqSkillId == NO_SKILL_REQUIREMENT ? true : mob.Skills[FirstReqSkillId].BaseFixedPoint >= 1000;
        }
        public override bool MeetsRequiredSkillLevel_2(Mobile mob)
        {
            return FirstReqSkillId == NO_SKILL_REQUIREMENT ? true : mob.Skills[SecondReqSkillId].BaseFixedPoint >= 1000;
        }

        public override CraftableEntry[] GetIngredients()
        {
            return Ingredients;
        }
        public override CraftableEntry GetResult()
        {
            return Result;
        }
        public override Item GenerateCraftedItem()
        {
            if (Result.m_ItemId == 0xA761) // random
            {
                return Decos.SuperRandomDeco();
            }
            Item crafted_item = new Item(Result.m_ItemId);
            crafted_item.Name = Result.m_Name;
            return crafted_item;
        }
    }

    class CraftableCurtainEast : CraftableRare
    {
        public override Item GenerateCraftedItem()
        {
            Item crafted_item = new Server.Items.DyeableCurtainEast();
            crafted_item.Name = Result.m_Name;
            return crafted_item;
        }
    }

    class CraftableCurtainSouth : CraftableRare
    {
        public override Item GenerateCraftedItem()
        {
            Item crafted_item = new Server.Items.DyeableCurtainSouth();
            crafted_item.Name = Result.m_Name;
            return crafted_item;
        }
    }

}
