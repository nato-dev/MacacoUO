using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Fronteira.Idade
{
    public class IdadeUtilidade
    {
        public static double IdadeEmDias(DateTime dataCriacao)
        {
            return (DateTime.UtcNow - dataCriacao).TotalDays;
        }

        public static double IdadeEmMeses(DateTime dataCriacao)
        {
            return Math.Truncate((IdadeEmDias(dataCriacao) % 365) / 30);
        }

        public static string GeraMotivoMorte()
        {
            var random = new Random();
            var listaFrases = new List<string>
            {
                "Voce teve um infarto e morreu", "Voce teve um aneurisma e morreu",
                "Voce teve uma unha encravada infeccionada e morreu", "Voce faleceu de causas naturais"
            };
            int index = random.Next(listaFrases.Count);
            return listaFrases[index];
        }

        public static bool IsTempoLimiteVida(PlayerMobile playerMobile)
        {
            /*
              jovem = 6 a 7 meses out game
                maduro = 4 a 5 meses out game
                velho = 2 a 3 meses out game
         */
            return //Deixei com 100 pra n ficar atrapalhando durante o desenvolvimento
                IdadeEmMeses(playerMobile.CreationTime) >= 100; //TODO Adicionar a parte do char ser jovem, maduro e velho
        }
    }
}
