using Server.Items;
using Server.Mobiles;
using Server.Ziden.Achievements;
using Server.Ziden.Dungeons.Goblins.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.RecompensaLogin
{
    public static class RecompensasLoginGuilda
    {
        public static List<CollectionItem> Rewards { get; set; }

        public static void Initialize()
        {
            Rewards = new List<CollectionItem>();

            Rewards.Add(new CollectionItem(typeof(SacolaBands), 0xE21, "Sacola com 50 Bandagens", 0, 10));
            Rewards.Add(new CollectionItem(typeof(BagOfReagents), 0xE76, "Sacola com 50 Reagentes", 0, 10));
            Rewards.Add(new CollectionItem(typeof(BagOfNecroReagents), 0xE76, "Sacola com 50 Reagentes Necro", 0, 10));
            Rewards.Add(new CollectionItem(typeof(BagOfArrows), 0xE76, "100 Flechas", 0, 10));
            Rewards.Add(new CollectionItem(typeof(BagOfBolts), 0xE76, "100 Dardos", 0, 10));
            Rewards.Add(new CollectionItem(typeof(HealPotion), 0xF0C, "Pocao de Cura", 0, 10));
            Rewards.Add(new CollectionItem(typeof(SacolaDeOuro), 0xE76, "Sacola com 300 Moedas", 0, 10));

            Rewards.Add(new CollectionItem(typeof(SkillBook), 0xEFA, "Livro Cientifico</br>Upa uma skill de 0.1 a 0.5", 0, 24 * 5)); // Yew
            Rewards.Add(new CollectionItem(typeof(CommodityDeedBox), 0x14F0, "Box. de Mercadorias<br>Guarda mercadorias", 0, 24)); // Yew

            Rewards.Add(new CollectionItem(typeof(PergaminhoCarregamento), 0x1F35, "Pergaminho do Carregamento<br>+1 Item na mochila", 0, 24 * 5));
            Rewards.Add(new CollectionItem(typeof(KegH), 0x1940, "Keg de Vida", 0, 24 * 3));
            Rewards.Add(new CollectionItem(typeof(KegMana), 0x1940, "Keg de Mana Fraca", 0, 24 * 3));
            Rewards.Add(new CollectionItem(typeof(KegStamina), 0x1940, "Keg de Stamina", 0, 24 * 4));
            Rewards.Add(new CollectionItem(typeof(KegCure), 0x1940, "Keg de Cura Maior", 0, 24 * 8));

            Rewards.Add(new CollectionItem(typeof(TemporaryForgeDeed), 0xFB1, "Forja Temporaria", 0, 50));
            // Rewards.Add(new CollectionItem(typeof(MagicalFishFinder), 0x14F6, "Encontrador de Peixes Magicos", 2500, 100));
            Rewards.Add(new CollectionItem(typeof(PergaminhoRunebook), 0x1F35, "Pergaminho de Runebook<br>Recarrega um runebook", 0, 10));
        }
    }
}
