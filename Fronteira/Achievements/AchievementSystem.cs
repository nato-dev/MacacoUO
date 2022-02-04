using Server;
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Misc;
using Scripts.Mythik.Systems.Achievements.Gumps;
using Server.Ziden.Items;
using Server.Ziden.Achievements;
using System.Linq;
using Scripts.Mythik.Systems.Achievements.AchieveTypes;
using Server.Ziden.Dungeons.Goblins.Quest;

namespace Scripts.Mythik.Systems.Achievements
{
    //TODO
    //page limit?
    /*thought of eating a lemon (and other foods), consume pots,
     *  craft a home, 
     *  own home (more for larger homes), 
     *  loot x amount of gold, 
     *  find a uni, 
     *  kill each mob in the game,
     *   enter an event,
     *    tame all tamables,
     *     use a max powerscroll (or skill stone), 
     *     ride each type of mount
     */
    public class AchievementSystem
    {
        public class AchievementCategory
        {
            public int ID { get; set; }
            public int Parent { get; set; }
            public string Name;


            public AchievementCategory(int id, int parent, string v3)
            {
                ID = id;
                Parent = parent;
                Name = v3;
            }
        }
        public static List<BaseAchievement> Achievements = new List<BaseAchievement>();
        public static List<AchievementCategory> Categories = new List<AchievementCategory>();

        private static Dictionary<Serial, Dictionary<int, AchieveData>> m_featData = new Dictionary<Serial, Dictionary<int, AchieveData>>();
        private static Dictionary<Serial, int> m_pointsTotal = new Dictionary<Serial, int>();
        private static int GetPlayerPointsTotal(PlayerMobile m)
        {
            if (!m_pointsTotal.ContainsKey(m.Serial))
                m_pointsTotal.Add(m.Serial, 0);
            return m_pointsTotal[m.Serial];
        }

        private static void AddPoints(PlayerMobile m, int points)
        {
            if (!m_pointsTotal.ContainsKey(m.Serial))
                m_pointsTotal.Add(m.Serial, 0);
            m_pointsTotal[m.Serial] += points;
        }

        public static void Initialize()
        {
            Categories.Add(new AchievementCategory(1, 0, "Exploracao"));
            Categories.Add(new AchievementCategory(2, 1, "Cidades"));
            Categories.Add(new AchievementCategory(3, 1, "Dungeons"));
            Categories.Add(new AchievementCategory(4, 1, "Pontos de Interesse"));

            Categories.Add(new AchievementCategory(1000, 0, "Crafting"));
            Categories.Add(new AchievementCategory(1001, 1000, "Alquimia"));
            Categories.Add(new AchievementCategory(1002, 1000, "Ferraria"));
            Categories.Add(new AchievementCategory(1003, 1000, "Funilaria"));

            Categories.Add(new AchievementCategory(2000, 0, "Coletar Recursos"));
            Categories.Add(new AchievementCategory(3000, 0, "PvM"));
            Categories.Add(new AchievementCategory(4000, 0, "Personagem"));
            Categories.Add(new AchievementCategory(5000, 0, "Outro"));

            //Achievements.Add(new DiscoveryAchievement(8888, 1, 0x14EB, false, null, "General expo!", "General expo!", 5, "Green Acres"));
            /*
            Achievements.Add(new DiscoveryAchievement(0, 3, 0x14EB, false, null, "Exodo!", "Descubra a caverna do Exodo", 5, "ExodoDungeon"));
            Achievements.Add(new DiscoveryAchievement(1, 3, 0x14EB, false, null, "Goblins ?", "Encontre uma caverna Goblin", 5, "Edgewich"));
            Achievements.Add(new DiscoveryAchievement(2, 3, 0x14EB, false, null, "Caminho do Necromante", "Encontre a Cripta Necromante", 5, "Blood Dungeon"));
            Achievements.Add(new DiscoveryAchievement(3, 3, 0x14EB, false, null, "Laboratorio Goblin", "Ache o Laboratorio Goblin", 5, "Exodus Dungeon"));
            Achievements.Add(new DiscoveryAchievement(4, 3, 0x14EB, false, null, "Caverna Orc", "Encontre a Caverna Orc", 5, "Orc Cave"));
            Achievements.Add(new DiscoveryAchievement(5, 3, 0x14EB, false, null, "Forte Orc", "Encontre o Forte Orc", 5, "OrcCompound"));
            Achievements.Add(new DiscoveryAchievement(6, 3, 0x14EB, false, null, "Bandoleeeeiiroooos", "Encontre o forte dos bandoleiros", 5, "HumanFort"));
            Achievements.Add(new DiscoveryAchievement(6, 3, 0x14EB, false, null, "Elementalismo", "Encontre a Caverna Elemental", 5, "Shame"));
            Achievements.Add(new DiscoveryAchievement(7, 3, 0x14EB, false, null, "A Prisao", "Encontre a Prisao", 5, "Wrong"));
            Achievements.Add(new DiscoveryAchievement(8, 3, 0x14EB, false, null, "Caverna dos Dragoes", "Encontre a Caverna dos Dragoes", 5, "Destard"));
            Achievements.Add(new DiscoveryAchievement(9, 3, 0x14EB, false, null, "Templo da Medusa", "Encontre o Templo da Medusa", 5, "Pulma"));
            Achievements.Add(new DiscoveryAchievement(10, 3, 0x14EB, false, null, "Masmorra de Arenito", "Encontre a Masmorra de Arenito", 5, "Khalmer"));
            Achievements.Add(new DiscoveryAchievement(11, 3, 0x14EB, false, null, "Abismo", "Encontre o Abismo", 5, "Doom"));
            Achievements.Add(new DiscoveryAchievement(11, 3, 0x14EB, false, null, "Formigueiro", "Encontre o Formigueiro", 5, "Solen Hives"));

            Achievements.Add(new DiscoveryAchievement(1, 1, 0x14EB, false, null, "Vila dos Druidas", "Encontre a Vila dos Druidas", 5, "Arbor"));
            Achievements.Add(new DiscoveryAchievement(2, 1, 0x14EB, false, null, "Forte Fofnolsaern", "Encontre o Forte Fofnolsaern", 5, "Bowan"));
            Achievements.Add(new DiscoveryAchievement(3, 1, 0x14EB, false, null, "Ratown", "Va a vila dos Ratos", 5, "SavageCamp"));
            Achievements.Add(new DiscoveryAchievement(4, 1, 0x14EB, false, null, "Tretonia", "Encontre Tretonia", 5, "Tretonia"));
            */

            /*
            Achievements.Add(new DiscoveryAchievement(1, 2, 0x14EB, false, null, "Britain!", "Discover the City Britain", 5, "Britain"));
            Achievements.Add(new DiscoveryAchievement(2, 2, 0x14EB, false, null, "Minoc!", "Discover the Minoc Township", 5, "Minoc"));
            Achievements.Add(new DiscoveryAchievement(3, 2, 0x14EB, false, null, "Ocllo!", "Discover the Ocllo Township", 5, "Ocllo"));
            Achievements.Add(new DiscoveryAchievement(4, 2, 0x14EB, false, null, "Trinsic!", "Discover the City of Trinsic", 5, "Trinsic"));
            Achievements.Add(new DiscoveryAchievement(5, 2, 0x14EB, false, null, "Vesper!", "Discover the City of Vesper", 5, "Vesper"));
            Achievements.Add(new DiscoveryAchievement(6, 2, 0x14EB, false, null, "Yew!", "Discover the Yew Township", 5, "Yew"));
            Achievements.Add(new DiscoveryAchievement(7, 2, 0x14EB, false, null, "Wind!", "Discover the City of Wind", 5, "Wind"));
            Achievements.Add(new DiscoveryAchievement(8, 2, 0x14EB, false, null, "Serpent's Hold!", "Discover the City of Serpent's Hold", 5, "Serpent's Hold"));
            Achievements.Add(new DiscoveryAchievement(9, 2, 0x14EB, false, null, "Skara Brae!", "Discover the Island of Skara Brae", 5, "Skara Brae"));
            Achievements.Add(new DiscoveryAchievement(10, 2, 0x14EB, false, null, "Nujel'm!", "Discover the Island of Nujel'm", 5, "Nujel'm"));
            Achievements.Add(new DiscoveryAchievement(11, 2, 0x14EB, false, null, "Moonglow!", "Discover the City of Moonglow", 5, "Moonglow"));
            Achievements.Add(new DiscoveryAchievement(12, 2, 0x14EB, false, null, "Magincia!", "Discover the City of Magincia", 5, "Magincia"));
            Achievements.Add(new DiscoveryAchievement(13, 2, 0x14EB, false, null, "Buccaneer's Den!", "Discover the Secrets of Buccaneer's Den", 5, "Buccaneer's Den"));

            Achievements.Add(new DiscoveryAchievement(25, 3, 0x14EB, false, null, "Covetous!", "Discover the dungeon of Covetous", 5, "Covetous"));
            Achievements.Add(new DiscoveryAchievement(26, 3, 0x14EB, false, null, "Deceit!", "Discover the dungeon of Deceit", 5, "Deceit"));
            Achievements.Add(new DiscoveryAchievement(27, 3, 0x14EB, false, null, "Despise!", "Discover the dungeon of Despise", 5, "Despise"));
            Achievements.Add(new DiscoveryAchievement(28, 3, 0x14EB, false, null, "Destard!", "Discover the dungeon of Destard", 5, "Destard"));
            Achievements.Add(new DiscoveryAchievement(29, 3, 0x14EB, false, null, "Hythloth!", "Discover the dungeon of Hythloth", 5, "Hythloth"));
            Achievements.Add(new DiscoveryAchievement(30, 3, 0x14EB, false, null, "Wrong!", "Discover the dungeon of Wrong", 5, "Wrong"));
            Achievements.Add(new DiscoveryAchievement(31, 3, 0x14EB, false, null, "Shame!", "Discover the dungeon of Shame", 5, "Shame"));

            Achievements.Add(new DiscoveryAchievement(100, 4, 0x14EB, false, null, "Cotton!", "Discover A Cotton Field in Moonglow", 5, "A Cotton Field in Moonglow"));
            Achievements.Add(new DiscoveryAchievement(101, 4, 0x14EB, false, null, "Carrots!", "Discover A Carrot Field in Skara Brae", 5, "A Carrot Field in Skara Brae"));
            */
            //these two show examples of adding a reward or multiple rewards

            var achieve2 = new ItemCraftedAchievement(500, 1002, 0xF5C, false, null, 30, "Macas", "Crie 30 Macas de Ferro", 5, typeof(Mace), typeof(SacolaMinerios));
            Achievements.Add(achieve2);

            achieve2 = new ItemCraftedAchievement(501, 1002, 0x13FF, false, null, 30, "Katanas Lindas", "Crie 30 Katanas", 5, typeof(Katana), typeof(SacolaMinerios));
            Achievements.Add(achieve2);

            achieve2 = new ItemCraftedAchievement(502, 1002, 0x13BF, false, null, 30, "Tunicas de Malha", "Crie 30 Tunicas de Malha", 5, typeof(ChainChest), typeof(SacolaMinerios));
            Achievements.Add(achieve2);


            achieve2 = new ItemCraftedAchievement(503, 1001, 0xF0A, false, null, 50, "Erva Venenosa", "Crie 50 Pocoes de Veneno Fraco", 5, typeof(LesserPoisonPotion), typeof(BagOfReagents));
            Achievements.Add(achieve2);

            achieve2 = new ItemCraftedAchievement(504, 1001, 0xF0D, false, null, 10, "Kaboom", "Crie 10 Pocoes de Explosao Fraca", 5, typeof(LesserExplosionPotion), typeof(BagOfReagents), typeof(BagOfReagents));
            Achievements.Add(achieve2);

            achieve2 = new ItemCraftedAchievement(505, 1001, 0xF0D, false, achieve2, 30, "Kaboomzinho", "Crie 30 Pocoes de Explosao Media", 5, typeof(ExplosionPotion), typeof(BagOfReagents), typeof(BagOfReagents));
            Achievements.Add(achieve2);

            achieve2 = new ItemCraftedAchievement(506, 1001, 0xF0D, false, achieve2, 200, "Kaboom", "Crie 200 Pocoes de Explosao Media", 5, typeof(ExplosionPotion), typeof(BagOfReagents), typeof(BagOfReagents));
            Achievements.Add(achieve2);

            achieve2 = new ItemCraftedAchievement(507, 1001, 0xF0D, false, achieve2, 200, "Kabommzaum", "Crie 200 Pocoes de Explosao Forte", 20, typeof(GreaterExplosionPotion), typeof(BagOfReagents), typeof(BagOfReagents), typeof(BagOfReagents), typeof(BagOfReagents), typeof(BagOfReagents));
            Achievements.Add(achieve2);

            achieve2 = new ItemCraftedAchievement(507, 1001, 0x2FC4, false, null, 1, "Botas Para Pantanos", "Crie uma Bota Elfica", 5, typeof(ElvenBoots), typeof(SacolaTecidos));
            Achievements.Add(achieve2);

            //achieve = new HarvestAchievement(501, 1002, 0x0E85, false, achieve, 500, "500 Minerios de Ferro", "Minere 500 minerios de ferro", 5, typeof(IronOre), typeof(SturdyPickaxe), typeof(SacolaMinerios));
            //Achievements.Add(achieve);
            //Achievements.Add(new HarvestAchievement(502, 1002, 0x0E85, false, achieve, 5000, "5000 Minerios de Ferro", "Minere 5000 minerios de ferro", 5, typeof(IronOre), typeof(SturdyPickaxe), typeof(SacolaMineriosGrande)));

            var achieve = new HarvestAchievement(500, 2000, 0x0E85, false, null, 50, "50 Minerios de Ferro", "Minere 50 Minerios de Ferro", 5, typeof(IronOre), typeof(SturdyPickaxe), typeof(SacolaMinerios));
            Achievements.Add(achieve);
            achieve = new HarvestAchievement(501, 2000, 0x0E85, false, achieve, 500, "500 Minerios de Ferro", "Minere 500 minerios de ferro", 5, typeof(IronOre), typeof(SturdyPickaxe), typeof(SacolaMinerios));
            Achievements.Add(achieve);
            Achievements.Add(new HarvestAchievement(502, 2000, 0x0E85, false, achieve, 5000, "5000 Minerios de Ferro", "Minere 5000 minerios de ferro", 5, typeof(IronOre), typeof(SturdyPickaxe), typeof(SacolaMineriosGrande)));

            achieve = new HarvestAchievement(503, 2000, 0xF43, false, null, 50, "50 Madeiras", "Colete 50 Madeiras", 5, typeof(Log), typeof(SacolaMadeiras));
            Achievements.Add(achieve);
            Achievements.Add(new HarvestAchievement(504, 2000, 0xF43, false, achieve, 500, "500 Madeiras", "Colete 500 Madeiras", 5, typeof(Log), typeof(SacolaMadeiras)));
            Achievements.Add(new HarvestAchievement(505, 2000, 0xF43, false, achieve, 5000, "5000 Madeiras", "Colete 5000 Madeiras", 5, typeof(Log), typeof(SacolaMadeirasGrande)));

            Achievements.Add(new HunterAchievement(1001, 3000, 0x429A, false, null, 1, "Sem Medo", "Mate um Lich", 5, typeof(Lich)));
            Achievements.Add(new HunterAchievement(1002, 3000, 0x429A, false, null, 50, "Matador de Liches", "Mate 50 Liches", 5, typeof(Lich), typeof(LivroAntigo)));
            Achievements.Add(new HunterAchievement(1003, 3000, 0x4292, false, null, 50, "Odeio Goblins", "Mate 50 Goblins ", 6, typeof(GreenGoblin), typeof(SacolaMinerios)));
            Achievements.Add(new HunterAchievement(1004, 3000, 0x20FD, false, null, 100, "Aranhas Malditas", "Mate 100 Aranhas Gigantes ", 5, typeof(GiantSpider), typeof(BagOfAllReagents)));
            Achievements.Add(new HunterAchievement(1005, 3000, 0x20EC, false, null, 5, "Mortos Fiquem Mortos", "Mate 5 Zumbi ", 2, typeof(Zombie)));
            Achievements.Add(new HunterAchievement(1006, 3000, 0x20EC, false, null, 1, "Ladrao de Sapato", "Mate o Mato Putrido ", 3, typeof(MagoPodre)));
            var sebo = new HunterAchievement(1007, 3000, 0x2608, false, null, 1, "Criatura do Pantano", "Mate um sebo do pantano ", 5, typeof(BogThing), typeof(TribalBerry));
            Achievements.Add(sebo);
            Achievements.Add(new HunterAchievement(1008, 3000, 0x2608, false, sebo, 10, "Sebosos Demais, eca", "Mate 10 Sebos do Pantano ", 7, typeof(BogThing), typeof(ElvenBoots)));
            
            Achievements.Add(new HunterAchievement(1009, 3000, 0x25B0, false, null, 30, "Selvaaaaagem", "Mate 30 Selvagens", 5, typeof(Savage)));
            Achievements.Add(new HunterAchievement(1010, 3000, 0x25B0, false, null, 100, "Shamanismo", "Mate 100 Selvagens Shaman", 5, typeof(SavageShaman), typeof(BearMask)));
            Achievements.Add(new HunterAchievement(1011, 3000, 0x25B0, false, null, 30, "Druidas Malignos", "Mate 5 Druidas Vis", 5, typeof(BearDruid), typeof(LivroAntigo)));
            Achievements.Add(new HunterAchievement(1012, 3000, 0x429A, false, null, 1, "Quanto mais velho, mais sabio", "Mate um Lich Anciao", 8, typeof(LichLord), typeof(LivroAntigo)));
            Achievements.Add(new HunterAchievement(1013, 3000, 0x429A, false, null, 1, "[BOSS] WoW - A Ira do Lich Rei", "Mate o Lich Rei", 20, typeof(AncientLichRenowned), typeof(LivroAntigo)));
            Achievements.Add(new HunterAchievement(1013, 3000, 0x20CB, false, null, 50, "Ogro, logro ?", "Mate 50 Ogrologros", 5, typeof(Ettin)));
            Achievements.Add(new HunterAchievement(1014, 3000, 0x42A0, false, null, 10, "[BOSS] O Selo do Mal", "Mate o boss EXODO", 30, typeof(ClockworkExodus)));
            Achievements.Add(new HunterAchievement(1015, 3000, 0x42A0, false, null, 10, "[BOSS] Tick, Tak, Boom", "Mate o boss Goblinzord", 30, typeof(GolemMecanico)));
            Achievements.Add(new HunterAchievement(1016, 3000, 0x25B7, false, null, 100, "Squick Squick", "Mate 100 Ratosos", 5, typeof(Ratman)));
            Achievements.Add(new HunterAchievement(1017, 3000, 0x2608, false, sebo, 100, "Pra que santo sebo ?", "Mate 100 Sebos do Pantano ", 20, typeof(BogThing), typeof(BotaSebosa)));
            Achievements.Add(new HunterAchievement(1017, 3000, 0x2608, false, null, 50, "Toma um banho de Lama", "Mate 50 Lamosos", 5, typeof(MudPie), typeof(BagOfAllReagents)));

            Achievements.Add(new HunterAchievement(1018, 3000, 0x2608, false, null, 1, "[BOSS] A Chama Do Amor", "Mate o guardiao do fogo", 10, typeof(FlameElemental), typeof(CaixaDeGold)));
            Achievements.Add(new HunterAchievement(1019, 3000, 0x2608, false, null, 1, "[BOSS] A Brisa do Sossego", "Mate o guardiao do vento",10, typeof(WindElemental), typeof(CaixaDeGold)));
            Achievements.Add(new HunterAchievement(1020, 3000, 0x2608, false, null, 1, "[BOSS] A Solidez da Alma", "Mate o guardiao de quartzo", 10, typeof(QuartzElemental), typeof(CaixaDeGold)));

            Achievements.Add(new HunterAchievement(1022, 3000, 0x20D6, false, null, 1, "Eh Facil Quando ta Jovem", "Mate um Dragao Jovem", 5, typeof(YoungDragon)));
            Achievements.Add(new HunterAchievement(1023, 3000, 0x20D6, false, null, 10, "Joias de Dragoes", "Mate 10 Dragoes", 10, typeof(Dragon), typeof(DecoDragonsBlood)));
            Achievements.Add(new HunterAchievement(1024, 3000, 0x20D6, false, null, 10, "Mestre dos Dragoes", "Mate 100 Dragoes", 100, typeof(Dragon), typeof(DragonHead), typeof(DragonLamp), typeof(DecoDragonsBlood2)));

            Achievements.Add(new HunterAchievement(1021, 3000, 0x429A, false, null, 1, "Carcereiros Malditos", "Mate 10 Carcereiro Malogros", 10, typeof(DemonicJailor), typeof(CaixaDeGold)));

            Achievements.Add(new HunterAchievement(1022, 3000, 0x4298, false, null, 1, "[BOSS] Medusa me Seduza", "Mate a Medusa", 10, typeof(Medusa), typeof(CaixaDeGold), typeof(MedusaDarkScales)));
            Achievements.Add(new HunterAchievement(1023, 3000, 0x20D7, false, null, 1, "[BOSS] Fique longe do Drack", "Mata o Pedroso de Drack", 10, typeof(Pedroso), typeof(CaixaDeGold), typeof(DecoRocks2)));
            Achievements.Add(new HunterAchievement(1024, 3000, 0x258B, false, null, 1, "[BOSS] Neira nao ta de zoeira", "Mate o champion Neira", 10, typeof(Neira), typeof(CaixaDeGold)));
            Achievements.Add(new HunterAchievement(1025, 3000, 0x2602, false, null, 1, "Formigas Vermelhas", "Mate 10 Formigas Trabalhadoras Vermelhas", 10, typeof(RedSolenWorker), typeof(BagOfReagents)));
            Achievements.Add(new HunterAchievement(1026, 3000, 0x2602, false, null, 1, "Formigas Pretas", "Mate 10 Formigas Trabalhadoras Pretas", 10, typeof(BlackSolenWorker), typeof(BagOfReagents)));
            Achievements.Add(new HunterAchievement(1027, 3000, 0x2602, false, null, 1, "Formigas Guerreiras Vermelhas", "Mate 50 Formigas Guerreiras Vermelhas", 50, typeof(RedSolenWarrior), typeof(SacolaDeOuro3000)));
            Achievements.Add(new HunterAchievement(1028, 3000, 0x2602, false, null, 1, "Formigas Guerreiras Pretas", "Mate 50 Formigas Guerreiras Pretas", 50, typeof(BlackSolenWarrior), typeof(SacolaDeOuro3000)));
            Achievements.Add(new HunterAchievement(1029, 3000, 0x2602, false, null, 5, "Plantas Carnivoras", "Mate 5 Plantas Carnivoras", 5, typeof(Corpser), typeof(SacolaDeOuro3000)));

            // SKILLS
            // BS
            var ach = new SkillProgressAchievement(2001, 4000, 0x13E3, false, null, 60, "Ferreiro Noob", "Obtenha 60 Blacksmithy", SkillName.Blacksmith, 5, typeof(SacolaMinerios));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2002, 4000, 0x13E3, false, ach, 70, "Ferreiro Perito", "Obtenha 70 Blacksmithy", SkillName.Blacksmith, 5, typeof(SacolaMinerios));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2003, 4000, 0x13E3, false, ach, 80, "Ferreiro Profissional", "Obtenha 80 Blacksmithy", SkillName.Blacksmith, 20, typeof(SacolaMinerios));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2004, 4000, 0x13E3, false, ach, 90, "Ferreiro Mestre", "Obtenha 90 Blacksmithy", SkillName.Blacksmith, 30, typeof(SacolaMinerios));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2005, 4000, 0x13E3, false, ach, 100, "Ferreiro Grande Mestre", "Obtenha 100 Blacksmithy", SkillName.Blacksmith, 50, typeof(SacolaMinerios));
            Achievements.Add(ach);

            // ALCH
            ach = new SkillProgressAchievement(2006, 4000, 0xE9B, false, null, 60, "Alquimista Noob", "Obtenha 60 Alchemy", SkillName.Alchemy, 5, typeof(BagOfAllReagents));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2007, 4000, 0xE9B, false, ach, 70, "Alquimista Perito", "Obtenha 70 Alchemy", SkillName.Alchemy, 5, typeof(BagOfAllReagents));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2008, 4000, 0xE9B, false, ach, 80, "Alquimista Profissional", "Obtenha 80 Alchemy", SkillName.Alchemy, 20, typeof(BagOfAllReagents));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2009, 4000, 0xE9B, false, ach, 90, "Alquimista Mestre", "Obtenha 90 Alchemy", SkillName.Alchemy, 30, typeof(BagOfAllReagents), typeof(BagOfAllReagents), typeof(BagOfAllReagents));
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(2010, 4000, 0xE9B, false, ach, 100, "Alquimista Grande Mestre", "Obtenha 100 Alchemy", SkillName.Alchemy, 50, typeof(BagOfAllReagents), typeof(BagOfAllReagents), typeof(BagOfAllReagents), typeof(BagOfAllReagents));
            Achievements.Add(ach);

            // BOW
            var nome = "Bowcrafter";
            var skill = "Bowcraft";
            var skillName = SkillName.Fletching;
            Type recompensa = typeof(SacolaMadeiras);
            var icone = 0x13B2;
            var id = 2011;
            ach = new SkillProgressAchievement(id++, 4000, icone, false, null, 60, nome+" Noob", "Obtenha 60 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 70, nome + " Perito", "Obtenha 70 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 80, nome + " Profissional", "Obtenha 80 " + skill, skillName, 20, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 90, nome + " Mestre", "Obtenha 90 " + skill, skillName, 30, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, nome + " Grande Mestre", "Obtenha 100 "+ skill, skillName, 50, recompensa, recompensa, recompensa, recompensa, recompensa);
            Achievements.Add(ach);

            // CARP
            nome = "Carpinteiro";
            skill = "Carpentry";
            skillName = SkillName.Carpentry;
            recompensa = typeof(SacolaMadeiras);
            icone = 0x1034;
            ach = new SkillProgressAchievement(id++, 4000, icone, false, null, 60, nome + " Noob", "Obtenha 60 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 70, nome + " Perito", "Obtenha 70 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 80, nome + " Profissional", "Obtenha 80 " + skill, skillName, 20, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 90, nome + " Mestre", "Obtenha 90 " + skill, skillName, 30, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, nome + " Grande Mestre", "Obtenha 100 " + skill, skillName, 50, recompensa, recompensa, recompensa, recompensa, recompensa);
            Achievements.Add(ach);

            // TINKER
            nome = "Funileiro";
            skill = "Tinkering";
            skillName = SkillName.Tinkering;
            recompensa = typeof(SacolaMinerios);
            icone = 0x1EB8;
            ach = new SkillProgressAchievement(id++, 4000, icone, false, null, 60, nome + " Noob", "Obtenha 60 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 70, nome + " Perito", "Obtenha 70 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 80, nome + " Profissional", "Obtenha 80 " + skill, skillName, 20, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 90, nome + " Mestre", "Obtenha 90 " + skill, skillName, 30, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, nome + " Grande Mestre", "Obtenha 100 " + skill, skillName, 50, recompensa, recompensa, recompensa, recompensa, recompensa);
            Achievements.Add(ach);

            // TAILOR
            nome = "Alfaiate";
            skill = "Tailoring";
            skillName = SkillName.Tailoring;
            recompensa = typeof(SacolaTecidos);
            icone = 0xF9D;
            ach = new SkillProgressAchievement(id++, 4000, icone, false, null, 60, nome + " Noob", "Obtenha 60 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 70, nome + " Perito", "Obtenha 70 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 80, nome + " Profissional", "Obtenha 80 " + skill, skillName, 20, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 90, nome + " Mestre", "Obtenha 90 " + skill, skillName, 30, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, nome + " Grande Mestre", "Obtenha 100 " + skill, skillName, 50, recompensa, recompensa, recompensa, recompensa, recompensa);
            Achievements.Add(ach);

            // TAMER
            nome = "Domador";
            skill = "Taming";
            skillName = SkillName.AnimalTaming;
            recompensa = typeof(CaixaDeGold);
            icone = 0x1374;
            ach = new SkillProgressAchievement(id++, 4000, icone, false, null, 60, nome + " Noob", "Obtenha 60 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 70, nome + " Perito", "Obtenha 70 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 80, nome + " Profissional", "Obtenha 80 " + skill, skillName, 20, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 90, nome + " Mestre", "Obtenha 90 " + skill, skillName, 30, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, nome + " Grande Mestre", "Obtenha 100 " + skill, skillName, 50, recompensa, recompensa, recompensa, recompensa, recompensa);
            Achievements.Add(ach);

            // JEWEL
            nome = "Joalheiro";
            skill = "Jewelcrafing";
            skillName = SkillName.TasteID;
            recompensa = typeof(SacolaJoias);
            icone = 0x1374;
            ach = new SkillProgressAchievement(id++, 4000, icone, false, null, 60, nome + " Noob", "Obtenha 60 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 70, nome + " Perito", "Obtenha 70 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 80, nome + " Profissional", "Obtenha 80 " + skill, skillName, 20, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 90, nome + " Mestre", "Obtenha 90 " + skill, skillName, 30, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, nome + " Grande Mestre", "Obtenha 100 " + skill, skillName, 50, recompensa, recompensa, recompensa, recompensa, recompensa);
            Achievements.Add(ach);


            // COOK
            // TAMER
            nome = "Cozinheiro";
            skill = "Cooking";
            skillName = SkillName.Cooking;
            recompensa = typeof(CaixaDeGold);
            icone = 0x97F;
            ach = new SkillProgressAchievement(id++, 4000, icone, false, null, 60, nome + " Noob", "Obtenha 60 " + skill, skillName, 5);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 70, nome + " Perito", "Obtenha 70 " + skill, skillName, 5, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 80, nome + " Profissional", "Obtenha 80 " + skill, skillName, 20, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 90, nome + " Mestre", "Obtenha 90 " + skill, skillName, 30, recompensa, recompensa);
            Achievements.Add(ach);
            ach = new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, nome + " Grande Mestre", "Obtenha 100 " + skill, skillName, 50, recompensa, recompensa, recompensa, recompensa, recompensa);
            Achievements.Add(ach);

         

            icone = 0x227C;
            // GMS
            ach = null;
            skillName = SkillName.Swords;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em "+skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Magery;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Imbuing;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Fencing;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Anatomy;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Mining;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Lumberjacking;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Herding;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Begging;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Stealing;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Macing;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Healing;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.MagicResist;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Chivalry;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Necromancy;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Ninjitsu;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Bushido;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Mysticism;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Spellweaving;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Poisoning;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Parry;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Lockpicking;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            skillName = SkillName.Wrestling;
            Achievements.Add(new SkillProgressAchievement(id++, 4000, icone, false, ach, 100, "Grande Mestre em " + skillName.ToString(), "Obtenha 100 " + skillName.ToString(), skillName, 10));

            //Achievements = Achievements.OrderBy(a => a.RewardPoints).ToList();

            CommandSystem.Register("conquistas", AccessLevel.Player, new CommandEventHandler(OpenGumpCommand));
            EventSink.WorldSave += EventSink_WorldSave;
            LoadData();
        }

        private static void LoadData()
        {
            Persistence.Deserialize(
                          "Saves//Achievements.bin",
                          reader =>
                          {
                              int count = reader.ReadInt();

                              for (int i = 0; i < count; ++i)
                              {
                                  m_pointsTotal.Add(reader.ReadInt(), reader.ReadInt());
                              }

                              count = reader.ReadInt();

                              for (int i = 0; i < count; ++i)
                              {
                                  var id = reader.ReadInt();
                                  var dict = new Dictionary<int, AchieveData>();
                                  int iCount = reader.ReadInt();
                                  if (iCount > 0)
                                  {
                                      for (int x = 0; x < iCount; x++)
                                      {
                                          dict.Add(reader.ReadInt(), new AchieveData(reader));
                                      }

                                  }
                                  m_featData.Add(id, dict);
                              }
                              System.Console.WriteLine("Loaded Achievements store: " + m_featData.Count);
                          });
        }

        private static void EventSink_WorldSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(
                          "Saves//Achievements.bin",
                          writer =>
                          {
                              writer.Write(m_pointsTotal.Count);
                              foreach (var kv in m_pointsTotal)
                              {
                                  writer.Write(kv.Key);
                                  writer.Write(kv.Value);
                              }

                              writer.Write(m_featData.Count);
                              foreach (var kv in m_featData)
                              {
                                  writer.Write(kv.Key);

                                  writer.Write(kv.Value.Count);

                                  foreach (var ckv in kv.Value)
                                  {
                                      writer.Write(ckv.Key);
                                      ckv.Value.Serialize(writer);
                                  }
                              }
                          });
        }

        public static void OpenGump(Mobile from, Mobile target)
        {
            if (from == null || target == null)
                return;
            if (target as PlayerMobile != null)
            {

                var player = target as PlayerMobile;
                if (!m_featData.ContainsKey(player.Serial))
                    m_featData.Add(player.Serial, new Dictionary<int, AchieveData>());
                var achieves = m_featData[player.Serial];
                var total = GetPlayerPointsTotal(player);

                from.SendGump(new AchievementGump(achieves, total));
            }
        }
        [Usage("conquistas"), Aliases("ach", "achievement", "achs", "achievements")]
        [Description("Opens the Achievements gump")]
        private static void OpenGumpCommand(CommandEventArgs e)
        {
            OpenGump(e.Mobile, e.Mobile);
        }

        internal static void SetAchievementStatus(PlayerMobile player, BaseAchievement ach, int progress)
        {

            if (!m_featData.ContainsKey(player.Serial))
                m_featData.Add(player.Serial, new Dictionary<int, AchieveData>());
            var achieves = m_featData[player.Serial];

            if (achieves.ContainsKey(ach.ID))
            {
                if (achieves[ach.ID].Progress >= ach.CompletionTotal)
                    return;
                achieves[ach.ID].Progress += progress;
            }
            else
            {
                achieves.Add(ach.ID, new AchieveData() { Progress = progress });
            }

            if (achieves[ach.ID].Progress >= ach.CompletionTotal)
            {
                player.SendMessage(78, "Voce completou uma conquista ! Digite .conquistas para ver suas conquistas !");
                player.SendGump(new AchievementObtainedGump(ach), false);
                achieves[ach.ID].CompletedOn = DateTime.UtcNow;

                if(ach.RewardPoints >= 10)
                {
                    foreach(var pl in PlayerMobile.Instances)
                    {
                        if(pl!=null && pl.NetState != null)
                        {
                            pl.SendMessage(78, player.Name + " completou a conquista " + ach.Title);
                        }
                    }
                }

                AddPoints(player, ach.RewardPoints);

                if (ach.RewardItems != null && ach.RewardItems.Length > 0)
                {
                    try
                    {
                        player.SendAsciiMessage("Voce recebeu um premio por completar a conquista!");
                        var item = (Item)Activator.CreateInstance(ach.RewardItems[0]);
                        if (!WeightOverloading.IsOverloaded(player))
                            player.Backpack.DropItem(item);
                        else
                            player.BankBox.DropItem(item);
                    }
                    catch { }
                }
            }
        }


    }
}
