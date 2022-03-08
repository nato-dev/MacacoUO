using Server.Guilds;

namespace Server.Ziden.GuerraTotal
{
    public class Guerra
    {
        public static DadosGuerra Dados = new DadosGuerra();

        public static void AddGuilda(Guild g)
        {
            Dados.AddGuilda(g);
            foreach(var player in g.OnlineMembers)
            {
                player.SendMessage("Sua guilda se juntou a Guerra Total !!!");
                player.SendMessage("Voce podera atacar e ser atacado por guildas da mesma divisao que sua guilda. Cuidado !");
            }
        }

        public static void OnPlayerKill(Mobile matou, Mobile morreu)
        {
            if(matou.Guild != null && morreu.Guild != null)
            {
                if(Dados.TaParticipando(matou.Guild) && Dados.TaParticipando(morreu.Guild))
                {
                    Dados.AlteraPontuacao(matou, morreu);
                }
            }
        }

    }
}
