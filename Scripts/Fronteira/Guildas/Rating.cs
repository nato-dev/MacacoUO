using System;

namespace Server.Ziden.GuerraTotal
{
    public class Rating
    {
        public static int PESO_RATING = 15;

        public static int GetDeltaPontos(int pontosMatador, int pontosPerdedor)
        {
            return (int)(PESO_RATING * ((int)1 - ChanceGanhar(pontosMatador, pontosPerdedor)));
        }

        private static double ChanceGanhar(int pontosP1, int pontosP2)
        {
            return 1 / (1 + Math.Pow(10, (pontosP2 - pontosP1) / 400.0));
        }
    }
}
