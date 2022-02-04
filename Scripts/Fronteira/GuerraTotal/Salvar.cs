using Server.Guilds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.GuerraTotal
{
    public class Salvar
    {
        private static string FilePath = Path.Combine("Saves/GuerraTotal", "Pontos.bin");

        public static void Configure()
        {
            Console.WriteLine("Inicializado Guerra Total");
            //EventSink.WorldSave += OnSave;
            //EventSink.WorldLoad += OnLoad;
        }

        private static void Salva(GenericWriter writer)
        {
            writer.Write((int)1);
            var participantes = Guerra.Dados.GetParticipantes();
            writer.Write((int)participantes.Count);
            foreach(var guilda in participantes.Values)
            {
                writer.Write(guilda.Id);
                writer.Write(guilda.PontosPlayers.Count);
                foreach(var playerId in guilda.PontosPlayers.Keys)
                {
                    writer.Write(playerId);
                    writer.Write((int)guilda.PontosPlayers[playerId]);
                }
            }
        }

        private static void Carrega(GenericReader reader)
        {
            var dados = Guerra.Dados;
            var versao = reader.ReadInt();
            var qtd = reader.ReadInt();
            for (var x = 0; x < qtd; x++)
            {
                var guildId = reader.ReadInt();
                var playersQtd = reader.ReadInt();
                var guilda = new GuildaParticipante();
                guilda.Id = guildId;

                for (var p = 0; p < playersQtd; p++)
                {
                    Serial playerId = reader.ReadInt();
                    var pontos = reader.ReadInt();
                    guilda.PontosPlayers[playerId] = pontos;
                    dados.Participantes[guildId] = guilda;
                }
            }
            Shard.Info("Guerra Total Carregada ! " + dados.Participantes.Count + " guildas carregadas !");
            Divisoes.CalculaDivisoes(dados);
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Console.WriteLine("Salvando Guerra Total");
            Persistence.Serialize(FilePath, Salva);
        }

        public static void OnLoad()
        {
            Console.WriteLine("Carregando Guerra Total");
            Persistence.Deserialize(FilePath, Carrega);
        }

    }
}
