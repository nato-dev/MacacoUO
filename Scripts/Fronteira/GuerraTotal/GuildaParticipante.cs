using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.GuerraTotal
{
    public class GuildaParticipante
    {
        public Divisao Divisao = 0;
        public int Id;
        public Dictionary<Serial, int> PontosPlayers = new Dictionary<Serial, int>();

        public Divisao GetDivisao()
        {
            return Divisoes.DivisoesIndexadas[Id];
        }

        public int GetPontos(Mobile m)
        {
            return PontosPlayers[m.Serial];
        }

        public void SetPontos(Mobile m, int n)
        {
            PontosPlayers[m.Serial] = n;
        }

        public int GetMediaPontos()
        {
            var media = 0;
            foreach(var ponto in PontosPlayers.Values)
            {
                media += ponto;
            }
            media /= PontosPlayers.Count;
            return media;
        }
    }
}
