using Server.Guilds;
using Server.Ziden.Traducao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Traducoes
{
    public class SaveTrads
    {
        private static string FilePath = Path.Combine("Saves/Traducoes", "Traducoes.bin");

        public static bool Carregado = false;

        public static void Configure()
        {
            Console.WriteLine("Inicializando traducoes");
            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        private static void Salva(GenericWriter writer)
        {
            Console.WriteLine("Salvando traducoes");
            writer.Write((int)1);

            writer.Write(Trads.Items.Count);
            writer.Write(Trads.Mobiles.Count);
            writer.Write(Trads.Clilocs.Count);
            writer.Write(Trads.Tradutores.Count);

            foreach(var key in Trads.Items.Keys)
            {
                writer.Write(key);
                writer.Write(Trads.Items[key]);
            }

            foreach (var key in Trads.Mobiles.Keys)
            {
                writer.Write(key);
                writer.Write(Trads.Mobiles[key]);
            }

            foreach (var key in Trads.Clilocs.Keys)
            {
                writer.Write(key);
                writer.Write(Trads.Clilocs[key]);
            }

            /*
            foreach (var key in Trads.Tradutores.Keys)
            {
                writer.Write(key);
                writer.Write(Trads.Tradutores[key]);
            }
            */
            Console.WriteLine("Traducoes salvas");
        }

        private static void Carrega(GenericReader reader)
        {
            var ver = reader.ReadInt();

            var items = reader.ReadInt();
            var mobiles = reader.ReadInt();
            var clilocs = reader.ReadInt();
            var tradutores = reader.ReadInt();

            Console.WriteLine(items + " items traduzidos");
            Console.WriteLine(mobiles + " mobiles traduzidos");
            Console.WriteLine(clilocs + " clilocs traduzidos");
            Console.WriteLine(tradutores + " tradutores participantes");

            Console.WriteLine("Lendo items");
            for (var x = 0; x < items; x++)
            {
                var key = reader.ReadString();
                var value = reader.ReadString();
                Trads.Items.Add(key, value);
            }

            Console.WriteLine("Lendo mobiles");
            for (var x = 0; x < mobiles; x++)
            {
                var key = reader.ReadString();
                var value = reader.ReadString();
                Trads.Mobiles.Add(key, value);
            }

            Console.WriteLine("Lendo Clilocs");
            for (var x = 0; x < clilocs; x++)
            {
                var key = reader.ReadInt();
                Console.WriteLine(key);
                var value = reader.ReadString();
                Console.WriteLine(value);
                Trads.Clilocs.Add(key, value);
            }

            Console.WriteLine("Lendo Tradutores");
            /*
            for (var x = 0; x < items; x++)
            {
                var key = reader.ReadInt();
                var value = reader.ReadInt();
                Trads.Tradutores.Add(key, value);
            }
            */

            Console.WriteLine("Traducao ingame carregada !!!");
          

        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Console.WriteLine("Salvando Traducoes");
            Persistence.Serialize(FilePath, Salva);
        }

        public static void OnLoad()
        {
            if(!Carregado)
            {
                Console.WriteLine("Carregando Traducoes");
                Persistence.Deserialize(FilePath, Carrega);
                Carregado = true;
            }
          
        }

    }
}
