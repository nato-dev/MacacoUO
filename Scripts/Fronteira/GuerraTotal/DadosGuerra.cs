using Server.Guilds;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Ziden.GuerraTotal
{
    public class DadosGuerra
    {
        public int HONRA_INICIAL = 2000;

        public Dictionary<int, GuildaParticipante> Participantes = new Dictionary<int, GuildaParticipante>();

        public bool TaParticipando(BaseGuild g)
        {
            return Participantes.ContainsKey(g.Id);
        }

        public Divisao GetDivisao(Mobile m)
        {
            return GetParticipante(m.Guild).Divisao;
        }

        public GuildaParticipante GetParticipante(BaseGuild g)
        {
            return Participantes[g.Id];
        }

        public Dictionary<int, GuildaParticipante> GetParticipantes()
        {
            return Participantes;
        }

        private int GetPontos(Mobile m)
        {
            var guilda = m.Guild;
            if (guilda == null)
            {
                Shard.Erro("Player nao tem guilda mas tentou ver pontos", m);
                return 0;
            }
            var participante = GetParticipante(guilda);
            return participante.GetPontos(m);
        }

        private void SetPontos(Mobile m, int pontos)
        {
            var guilda = m.Guild;
            if (guilda == null)
            {
                Shard.Erro("Player nao tem guilda mas tentou setar pontos", m);
                return;
            }
            var participante = GetParticipante(guilda);
            participante.SetPontos(m, pontos);
        }

        public void AlteraPontuacao(Mobile matou, Mobile morreu)
        {
            var pontosMatou = GetPontos(matou);
            var pontosMorreu = GetPontos(morreu);
            var delta = Rating.GetDeltaPontos(pontosMatou, pontosMorreu);
            matou.SendMessage("Ganhou Pontos: " + delta);
            morreu.SendMessage("Perdeu Pontos: " + delta);
            SetPontos(matou, pontosMatou + delta);
        }

        public void AddGuilda(Guild guild)
        {
            var guildaParticipante = new GuildaParticipante();
            guildaParticipante.Id = guild.Id;
            foreach (var membro in guild.Members)
            {
                guildaParticipante.PontosPlayers.Add(membro.Serial, HONRA_INICIAL);
            }
            Participantes.Add(guild.Id, guildaParticipante);
        }
    }
}
