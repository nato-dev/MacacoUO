using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.Fronteira.Weeklies.Weekly;

namespace Server.Fronteira.Weeklies
{
    public class SaveWeekly
    {
        private static string FilePath = Path.Combine("Saves/Dungeons", "Semanal.bin");

        public static bool Carregado = false;

        public static void Configure()
        {
            Console.WriteLine("Inicializando save das weeklies");
            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        public static int SEMANA_ATUAL = 0;
        public static List<CombinacaoKill> Kills = new List<CombinacaoKill>();

        private static void Salva(GenericWriter writer)
        {
            Console.WriteLine("Salvando weeklies");
            writer.Write((int)1);

        }

        private static void Carrega(GenericReader reader)
        {
            var ver = reader.ReadInt();



        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(FilePath, Salva);
        }

        public static void OnLoad()
        {
            if (!Carregado)
            {
                Console.WriteLine("Carregando weeklies");
                Persistence.Deserialize(FilePath, Carrega);
                Carregado = true;
            }

        }
    }
}
