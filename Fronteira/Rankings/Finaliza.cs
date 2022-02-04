using Server.Commands;
using Server.Engines.Points;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Rankingss
{
    public class Finaliza
    {
        /*
         *   AddItem(132, 171, 4648, 0x8A5);
                AddItem(132, 228, 4646);
                AddItem(132, 277, 4644, 2415);
                */

        public static void Initialize()
        {
            CommandSystem.Register("fimranking", AccessLevel.Administrator, new CommandEventHandler(Comando));
        }

        [Usage("Ranking")]
        [Description("Makes a call to your custom gump.")]
        public static void Comando(CommandEventArgs e)
        {
            Termina();
            Anuncio.Anuncia("Rankings Zerados - Premios Dados");
        }

        public static void Termina()
        {
            var rankWork = PointsSystem.PontosTrabalho.GetOrCalculateRank();
            var rankPvm = PointsSystem.PontosPvmEterno.GetOrCalculateRank();

            PrimeiroLugar(rankWork, "Worker");
            PrimeiroLugar(rankPvm, "PvM");

            SegundoLugar(rankWork, "Worker");
            SegundoLugar(rankPvm, "PvM");

            TerceiroLugar(rankWork, "Worker");
            TerceiroLugar(rankPvm, "PvM");

            PointsSystem.PontosTrabalho.Clear();
            PointsSystem.PontosPvmEterno.Clear();
    
        }

        public static void TerceiroLugar(List<PointsEntry> rank, string rankName)
        {
            var trofeu1 = new Item(4644);
            trofeu1.Hue = 2415;
            var player = rank[2].Player;
            var robe = new JesterShoes();
            robe.Owner = player;
            robe.Name = "3o Lugar Ranking " + rankName + " 11/2020 - " + player.Name;
            robe.Hue = Loot.RandomRareDye();
            robe.LootType = LootType.Blessed;

            trofeu1.Name = "3o Lugar Ranking " + rankName + " 11/2020 - " + player.Name;
            player.BankBox.DropItem(trofeu1);
        }

        public static void SegundoLugar(List<PointsEntry> rank, string rankName)
        {
            var trofeu1 = new Item(4646);
            var player = rank[1].Player;
            trofeu1.Name = "2o Lugar Ranking " + rankName + " 11/2020 - " + player.Name;
            player.BankBox.DropItem(trofeu1);
            var robe = new JesterShoes();
            robe.Owner = player;
            robe.LootType = LootType.Blessed;
            robe.Name = "2o Lugar Ranking " + rankName + " 11/2020 - " + player.Name;
            robe.Hue = Loot.RandomRareDye();
            player.BankBox.DropItem(robe);
            player.BankBox.DropItem(new EmbroideryTool());
        }

        public static void PrimeiroLugar(List<PointsEntry> rank, string rankName)
        {
            var trofeu1 = new Item(4648);
            trofeu1.Hue = 0x8A5;
            var player = rank[0].Player;
            trofeu1.Name = "1o Lugar Ranking " + rankName + " 11/2020 - " + player.Name;
            player.BankBox.DropItem(trofeu1);
            var robe = new JesterShoes();
            robe.Owner = player;
            robe.LootType = LootType.Blessed;
            robe.Name = "1o Lugar Ranking " + rankName + " 11/2020 - " + player.Name;
            robe.Hue = Loot.RandomRareDye();
            player.BankBox.DropItem(robe);

            var robe2 = new JesterHat();
            robe2.Owner = player;
            robe2.LootType = LootType.Blessed;
            robe2.Name = "1o Lugar Ranking " + rankName + " 11/2020 - " + player.Name;
            robe2.Hue = Loot.RandomRareDye();
            player.BankBox.DropItem(robe2);


            player.BankBox.DropItem(new EmbroideryTool());
            player.BankBox.DropItem(new WeaponEngravingTool());
            player.BankBox.DropItem(new ArmorEngravingTool());
        }
    }


}
