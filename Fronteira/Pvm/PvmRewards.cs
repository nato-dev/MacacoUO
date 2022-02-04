using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using System.Collections.Generic;
using Server.Network;
using Server.Guilds;
using System.Linq;
using Server.Engines.Points;
using Server.Items.Functional.Pergaminhos;
using Server.Ziden.Achievements;
using Server.Ziden;
using Server.Ziden.Dungeons.Goblins.Quest;

namespace Server.Engines.VvV
{

    public static class PvmRewards
    {
        public static List<CollectionItem> Rewards { get; set; }

        public static void Initialize()
        {
            Rewards = new List<CollectionItem>();
            Rewards.Add(new CollectionItem(typeof(KrampusMinionBoots), 0xA28D, "Botas Natalinas", 0, 20000));  // Greater Stam
            Rewards.Add(new CollectionItem(typeof(KrampusMinionHat), 0xA28F, "Chapeu Natalino", 0, 20000));  // Greater Stam
            Rewards.Add(new CollectionItem(typeof(TintaPreta), 0xFAB, "Tinta Preta", TintaPreta.COR, 10000));  // Greater Stam
            Rewards.Add(new CollectionItem(typeof(TintaBranca), 0xFAB, "Tinta Branca", TintaBranca.COR, 10000));  // Greater Stam
            Rewards.Add(new CollectionItem(typeof(PergaminhoSagrado), 0x14F0, "Transforma roupa newbie por 1 mes", 1154, 50000));  // Greater Stam
            if (!Shard.EXP)
                Rewards.Add(new CollectionItem(typeof(CombatSkillBook), 0xEFA, "Livro de Combate<br>Sobe 0.5 em uma skill de combate", 788, 1000));  // Greater Stam
            Rewards.Add(new CollectionItem(typeof(SacolaBands), 0xE21, "Sacola com 50 Bandagens", 0, 500));
            Rewards.Add(new CollectionItem(typeof(ScolaDizimo), 0xE21, "Sacola com 100 Dizimos de Paladino", 0, 500));
            Rewards.Add(new CollectionItem(typeof(BagOfNecroReagents), 0xE76, "Sacola com 50 Reagentes Necro", 0, 600));
            Rewards.Add(new CollectionItem(typeof(BagOfArrows), 0xE76, "100 Flechas", 0, 500));
            Rewards.Add(new CollectionItem(typeof(BagOfBolts), 0xE76, "100 Dardos", 0, 500));

            Rewards.Add(new CollectionItem(typeof(BagOfReagents), 0xE76, "Sacola com 50 Reagentes", 0, 500));
            Rewards.Add(new CollectionItem(typeof(HealPotion), 0xF0C, "Pocao de Vida", 0, 100));
            Rewards.Add(new CollectionItem(typeof(CurePotion), 0xF0C, "Pocao de Curar Veneno", 0, 200));
            Rewards.Add(new CollectionItem(typeof(SacolaDeOuro), 0xE76, "Sacola com 300 Moedas", 0, 400));
            Rewards.Add(new CollectionItem(typeof(SacolaDeOuro3000), 0xE76, "Sacola com 3000 Moedas", 0, 4000));
            Rewards.Add(new CollectionItem(typeof(CaixaDeGold), 0xE7E, "Caixa com 10000 Moedas", 0, 12000));
            Rewards.Add(new CollectionItem(typeof(PergaminhoRunebook), 0x1F35, "Pergaminho de Runebook<br>Recarrega um runebook", 0, 1000));
        }

    }
}
