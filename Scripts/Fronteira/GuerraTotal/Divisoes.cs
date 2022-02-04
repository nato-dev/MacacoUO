using System;
using System.Collections.Generic;

namespace Server.Ziden.GuerraTotal
{
    public enum Divisao
    {
        PRIMEIRA = 1,
        SEGUNDA = 2,
        TERCEIRA = 3
    }

    public class Divisoes
    {
        public static int[,] CONFIG = new int[,] {
        //  pct , minElo
            { 10, 2500 },
            { 30, 2200},
        };

        public static Dictionary<int, Divisao> DivisoesIndexadas = new Dictionary<int, Divisao>();

        public static void CalculaDivisoes(DadosGuerra dados)
        {
            var agora = DateTime.UtcNow;
            Shard.Info("Calculando divisoes Guerra Total");
            DivisoesIndexadas = new Dictionary<int, Divisao>();
            var lista = new SortedList<int, GuildaParticipante>();
            foreach(var guilda in dados.GetParticipantes().Values)
            {
                var rankGuilda = guilda.GetMediaPontos();
                while (lista.ContainsKey(rankGuilda))
                {
                    rankGuilda--;
                }
                lista.Add(rankGuilda, guilda);
            }

            var maxRank = lista.Count;
            var rank = 0;

            var primeira = new List<GuildaParticipante>();
            var segunda = new List<GuildaParticipante>();
            var terceira = new List<GuildaParticipante>();

            foreach(var elo in lista.Keys)
            {
                var guilda = lista[elo];
                var pct = (rank / maxRank) * 100;
                Shard.Debug("Guilda " + guilda.Id + " Rank " + rank + " pct " + pct);

                if (pct > CONFIG[0, 0] && elo > CONFIG[1, 0])
                {
                    primeira.Add(guilda);
                    guilda.Divisao = Divisao.PRIMEIRA;
                } else if (pct > CONFIG[0, 1] && elo > CONFIG[1, 1])
                {
                    segunda.Add(guilda);
                    guilda.Divisao = Divisao.SEGUNDA;
                } else 
                {
                    terceira.Add(guilda);
                    guilda.Divisao = Divisao.TERCEIRA;
                }
                rank++;
            }

            Shard.Debug("==== Divisoes Calculadas ====");
            Shard.Debug("| Serie A:          |");
            foreach(var g in primeira)
            {
                Shard.Debug(""+g.Id);
            }
            Shard.Debug("----------------------");
            Shard.Debug("| Serie B:          |");
            foreach (var g in primeira)
            {
                Console.WriteLine(g.Id);
            }
            Shard.Debug("----------------------");
            Shard.Debug("| Serie C:          |");
            foreach (var g in primeira)
            {
                Shard.Debug("" + g.Id);
            }
         
            foreach (var g in primeira)
                DivisoesIndexadas[g.Id] = Divisao.PRIMEIRA;
            foreach (var g in segunda)
                DivisoesIndexadas[g.Id] = Divisao.SEGUNDA;
            foreach (var g in terceira)
                DivisoesIndexadas[g.Id] = Divisao.TERCEIRA;

            var levou = (DateTime.UtcNow - agora).TotalMilliseconds;
            Shard.Info("Calcular divisoes guerra total demorou " + levou + "ms");
        }
    }
}
