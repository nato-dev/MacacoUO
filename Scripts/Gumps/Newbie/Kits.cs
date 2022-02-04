using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Gumps.Newbie
{
    public class StarterKits
    {

        public class Kit
        {
            public String Name;
            public String Desc;
            public Dictionary<SkillName, int> Skills = new Dictionary<SkillName, int>();
            public List<Item> items = new List<Item>();
            public List<Item> equips = new List<Item>();
            public int Code = 0;
            public int Str;
            public int Dex;
            public int Int;
        }

        private int option = 0;

        public static Item GetRandomHat(int color)
        {
            switch (Utility.Random(9))
            {
                case 0: return new Bandana(color);
                case 1: return new FeatheredHat(color);
                case 2: return new Bonnet(color);
                case 3: return new WizardsHat(color);
                case 4: return new Kasa(color);
                case 5: return new Cap(color);
                case 8: return new SkullCap(color);
            }
            return new SkullCap(color);
        }

        public static int GetNoobColor()
        {
            switch(Utility.Random(4))
            {
                case 0:
                    return Utility.RandomGreenHue();
                case 1:
                    return Utility.RandomBlueHue();
                case 3:
                    return Utility.RandomBirdHue();
                default:
                    return Utility.RandomRedHue();

            }
   
        }

        public static List<Kit> Kits = null;

        public static Kit GetKit(int cod)
        {
            foreach(var kit in Kits)
            {
                if (kit.Code == cod)
                    return kit;
            }
            return null; 
        }

        public static int ARCHER = 4;
        public static int WAR = 1;
        public static int BS = 2;
        public static int MERC = 7;
        public static int BARD = 3;
        public static int MAGE = 6;
        public static int TAMER = 5;

        public static void BuildKits()
        {
            var noobColor = 78;
            if (Kits == null)
            {
                Kits = new List<Kit>();

                var archer = new Kit()
                {
                    Name = "Arqueiro",
                    Code =  4,
                    Desc = "Use arcos e bestas como arma. Esconda-se nas sombras para nao ser detectado",
                    Str = 40,
                    Dex = 50,
                    Int = 20
                };
                archer.Skills.Add(SkillName.Hiding, 50);
                archer.Skills.Add(SkillName.Archery, 40);
                archer.Skills.Add(SkillName.Tactics, 40);
                archer.Skills.Add(SkillName.Anatomy, 35);
                archer.Skills.Add(SkillName.Healing, 50);
                archer.Skills.Add(SkillName.MagicResist, 50);
                archer.Skills.Add(SkillName.Fencing, 40);
                archer.equips.Add(new Bow());
                archer.items.Add(new Arrow(300));
                archer.items.Add(new Bandage(100));
                archer.items.Add(new Dagger());
                archer.equips.Add(new StuddedChest());
                archer.equips.Add(new StuddedLegs());
                archer.equips.Add(new StuddedArms());
                archer.equips.Add(new StuddedGloves());
                archer.equips.Add(new BodySash(noobColor));
                archer.equips.Add(new FeatheredHat(noobColor));
                archer.equips.Add(new Cloak(noobColor));
                archer.equips.Add(new Shoes(noobColor));
                Kits.Add(archer);

                var guerreiro = new Kit()
                {
                    Str = 60,
                    Dex = 40,
                    Int = 20,
                    Name = "Guerreiro",
                    Code = 1,
                    Desc = @"[Boa para iniciantes] Focado em combate corpo a corpo, usa espadas e machados. Usa bandagens ou magia para se curar."
                };
                guerreiro.Skills.Add(SkillName.Swords, 50);
                guerreiro.Skills.Add(SkillName.Anatomy, 40);
                guerreiro.Skills.Add(SkillName.Healing, 50);
                guerreiro.Skills.Add(SkillName.MagicResist, 50);
                guerreiro.Skills.Add(SkillName.Parry, 35);
                guerreiro.Skills.Add(SkillName.Magery, 50);
                guerreiro.equips.Add(new VikingSword());
                guerreiro.equips.Add(new MetalShield());
                guerreiro.items.Add(new Bandage(100));
                guerreiro.equips.Add(new ChainChest());
                guerreiro.equips.Add(new ChainGloves());
                guerreiro.equips.Add(new ChainLegs());
                guerreiro.equips.Add(new ChainCoif());
                guerreiro.equips.Add(new Cloak(noobColor));
                guerreiro.equips.Add(new Boots());
                guerreiro.equips.Add(new BodySash(noobColor));
                Kits.Add(guerreiro);

                var blacksmith = new Kit()
                {
                    Str = 60,
                    Dex = 40,
                    Int = 20,
                    Name = "Ferreiro",
                    Code = 2,
                    Desc = @"Pode fabricar armas e armaduras alem de ter bonus de dano com macas, sendo forte em combate."
                
                };
                blacksmith.Skills.Add(SkillName.Blacksmith, 50);
                blacksmith.Skills.Add(SkillName.Mining, 40);
                blacksmith.Skills.Add(SkillName.Tinkering, 35);
                blacksmith.Skills.Add(SkillName.Camping, 40);
                blacksmith.Skills.Add(SkillName.Anatomy, 40);
                blacksmith.Skills.Add(SkillName.Macing, 50);
                blacksmith.Skills.Add(SkillName.Healing, 50);
                blacksmith.Skills.Add(SkillName.Tactics, 30);
                blacksmith.items.Add(new SledgeHammer());
                blacksmith.items.Add(new Pickaxe());
                blacksmith.items.Add(new Bandage(100));
                blacksmith.equips.Add(new WarHammer());
                blacksmith.items.Add(new TinkerTools());
                blacksmith.items.Add(new IronIngot(50));
                blacksmith.equips.Add(new ChainChest());
                blacksmith.equips.Add(new ChainGloves());
                blacksmith.equips.Add(new ChainLegs());
                blacksmith.equips.Add(new SkullCap(noobColor));
                blacksmith.equips.Add(new FullApron(noobColor));
                blacksmith.equips.Add(new Boots());
                blacksmith.items.Add(new Dagger());
                Kits.Add(blacksmith);

                var merchant = new Kit()
                {
                    Str = 60,
                    Dex = 40,
                    Int = 20,
                    Code = 7,
                    Name = "Mercador",
                    Desc = "Especialista em crafting, pode fabricar diversas utilidades. Nao possui aptidao alguma para combate."
                };
                merchant.Skills.Add(SkillName.Carpentry, 50);
                merchant.Skills.Add(SkillName.Lumberjacking, 35);
                merchant.Skills.Add(SkillName.Fletching, 50);
                merchant.Skills.Add(SkillName.Tailoring, 50);
                merchant.Skills.Add(SkillName.Herding, 35);
                merchant.Skills.Add(SkillName.Tinkering, 35);
                merchant.Skills.Add(SkillName.Camping, 50);
                merchant.Skills.Add(SkillName.Mining, 35);
                merchant.Skills.Add(SkillName.Blacksmith, 50);
                merchant.items.Add(new SewingKit());
                merchant.items.Add(new Hatchet());
                merchant.items.Add(new FletcherTools());
                merchant.items.Add(new TinkerTools());
                merchant.items.Add(new Pickaxe());
                merchant.items.Add(new Bandage(100));
                merchant.items.Add(new SledgeHammer());
                merchant.items.Add(new IronIngot(30));
                merchant.items.Add(new Board(30));
                merchant.items.Add(new Dagger());
                merchant.equips.Add(new FancyShirt(noobColor));
                merchant.equips.Add(new LongPants(noobColor));
                merchant.equips.Add(new Bonnet(noobColor));
                merchant.equips.Add(new Shoes(noobColor));
                Kits.Add(merchant);

                var bard = new Kit()
                {
                    Str = 60,
                    Dex = 40,
                    Int = 20,
                    Code = 3,
                    Name = "Bardo",
                    Desc = "Pode acalmar e provocar criaturas alem de ter habilidades com lancas e adagas. Otimo contra monstros. "
                };
                bard.Skills.Add(SkillName.Musicianship, 50);
                bard.Skills.Add(SkillName.Peacemaking, 50);
                bard.Skills.Add(SkillName.Provocation, 50);
                bard.Skills.Add(SkillName.Discordance, 35);
                bard.Skills.Add(SkillName.Fencing, 40);
                bard.Skills.Add(SkillName.Healing, 50);
                bard.Skills.Add(SkillName.Anatomy, 35);
                bard.equips.Add(new Dagger());
                bard.equips.Add(new StuddedChest());
                bard.equips.Add(new StuddedLegs());
                bard.equips.Add(new StuddedArms());
                bard.equips.Add(new StuddedGloves());
                bard.equips.Add(new Shoes(noobColor));
                bard.equips.Add(new Cap(noobColor));
                bard.equips.Add(new Cloak(noobColor));
                bard.items.Add(new Bandage(100));
                bard.items.Add(new Spear());
                bard.items.Add(new Lute());
                Kits.Add(bard);

                var mage = new Kit()
                {
                    Str = 40,
                    Dex = 30,
                    Int = 50,
                    Code = 6,
                    Name = "Mago",
                    Desc = "Usuario de magia, tanto beneficas quanto maleficas. Magos sao muito versateis. Precisa de reagentes para usar magias."
                };
                mage.Skills.Add(SkillName.Magery, 50);
                mage.Skills.Add(SkillName.EvalInt, 40);
                mage.Skills.Add(SkillName.Meditation, 50);
                mage.Skills.Add(SkillName.Inscribe, 50);
                mage.Skills.Add(SkillName.Alchemy, 50);
                mage.Skills.Add(SkillName.Macing, 40);
                mage.Skills.Add(SkillName.Tactics, 25);
                mage.Skills.Add(SkillName.MagicResist, 50);
                mage.items.Add(new BlankScroll(10));
                mage.items.Add(new PenAndInk());
                mage.items.Add(new AlchemyBag());
                mage.equips.Add(new QuarterStaff());
                mage.items.Add(new Bandage(100));
                mage.equips.Add(new Robe(noobColor));
                mage.equips.Add(new WizardsHat(noobColor));
                mage.equips.Add(new Sandals());
                mage.equips.Add(new LeatherChest());
                mage.equips.Add(new LeatherLegs());
                mage.equips.Add(new LeatherArms());
                mage.equips.Add(new LeatherGloves());
                Kits.Add(mage);

                var tamer = new Kit()
                {
                    Str = 40,
                    Dex = 40,
                    Int = 40,
                    Code = 5,
                    Name = "Domador de Animais",
                    Desc = "Usa criaturas como arma. Dificil de treinar e tem de treinar as criaturas tambem mas podem extremamente forte."
                };
                tamer.Skills.Add(SkillName.AnimalTaming, 50);
                tamer.Skills.Add(SkillName.AnimalLore, 40);
                tamer.Skills.Add(SkillName.Veterinary, 50);
                tamer.Skills.Add(SkillName.Healing, 50);
                tamer.Skills.Add(SkillName.Magery, 50);
                tamer.Skills.Add(SkillName.Fencing, 40);
                tamer.Skills.Add(SkillName.Anatomy, 35);
                tamer.items.Add(new Bandage(100));
                tamer.equips.Add(new StuddedChest());
                tamer.equips.Add(new StuddedLegs());
                tamer.equips.Add(new StuddedArms());
                tamer.equips.Add(new StuddedGloves());
                tamer.equips.Add(new Boots());
                tamer.equips.Add(new Spear());
                tamer.equips.Add(new Bandana(noobColor));
                tamer.equips.Add(new BodySash(noobColor));
                Kits.Add(tamer);

            }
        }
    }
}
