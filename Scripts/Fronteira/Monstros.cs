//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira
{
    public class Configuracao
    {
        public static string FilePathMonstros = Path.Combine("Saves/Help", "monstros.json");

        public static void Configure()
        {
            EventSink.WorldLoad += Carrega;
        }

        public static void Carrega()
        {
            Console.WriteLine("Carregando dados do shard");
            try
            {
              // Monstros.Configs = JsonConvert.DeserializeObject<Dictionary<string, Monstros>>(File.ReadAllText(FilePathMonstros));
            } catch(Exception e)
            {

            }
            Console.WriteLine("Carregado " + Monstros.Configs.Count + " monstros");
        }
    }

    [Serializable]
    public class ItemLoot
    {
        public int Grafico;
        public int Cor;
        public int Qtd;
        public string Nome;
        public string Type;
        public bool Sorteado;

        public ItemLoot(Item i, bool sorteio = false)
        {
            Grafico = i.ItemID;
            Cor = Gump.ToRgb(Gump.HueToColor((short)i.Hue));
            Qtd = i.Amount;
            Nome = i.Name;
            Type = i.GetType().Name;
            Sorteado = sorteio;
        }
    }

   

    [Serializable]
    public class Monstros
    {
        public int Grafico;
        public string Type;
        public string Name;
        public int Hits;
        public int Mana;
        public int Stam;
        public int DanoMax;
        public int DanoMin;
        public int Str;
        public int Dex;
        public int Int;

        public double TameSkill;
        public List<ItemLoot> Loots;

        public Monstros(BaseCreature bc)
        {
            this.Grafico = bc.Body;
            this.Type = bc.GetType().Name;
            this.Name = bc.Name;
            Hits = bc.HitsMax;
            Mana = bc.ManaMax;
            Stam = bc.StamMax;
            DanoMax = bc.DamageMax;
            DanoMin = bc.DamageMin;
            Str = bc.Str;
            Dex = bc.Dex;
            Int = bc.Int;
            if (bc.Tamable)
                TameSkill = bc.MinTameSkill;

        }

        public static Dictionary<string, Monstros> Configs = new Dictionary<string, Monstros>();


        public static void GeraDocMonstros()
        {
            List<string> erros = new List<string>();
            using (StreamWriter outputFile = new StreamWriter(Configuracao.FilePathMonstros))
            {
                LootPack.OldMagicItems.Clear();
                LootPack.GemItems.Clear();

                BaseCreature.BypassTimerInicial = true;
                var docs = new Dictionary<string, HashSet<string>>();
                foreach (Assembly a in ScriptCompiler.Assemblies)
                {
                    var delay = 0;
                    foreach (var tipo in a.GetTypes())
                    {
                        delay += 1;
                        // Timer.DelayCall(TimeSpan.FromMilliseconds(delay), () =>
                        // {
                        if (tipo.IsSubclassOf(typeof(BaseCreature)))
                        {
                            Console.WriteLine($"Gerando {tipo.Name}");

                            try
                            {
                                Utility.FIX = 1;

                                Console.WriteLine($"Spawn 1");
                                var bc1 = (BaseCreature)Activator.CreateInstance(tipo);
                                Console.WriteLine($"Kill 1");
                                bc1.Kill();
                                Utility.FIX = 0;
                                Console.WriteLine($"Spawn 2");
                                var bc2 = (BaseCreature)Activator.CreateInstance(tipo);
                                Console.WriteLine($"Kill 2");
                                bc2.Kill();

                                Console.WriteLine($"Spawnados");
                                var c1 = bc1.Corpse;
                                var c2 = bc2.Corpse;

                                var itemLoots = new List<ItemLoot>();
                                var loots = new HashSet<string>();
                                if (c1 != null)
                                    foreach (var i in c1.Items)
                                    {
                                        Console.WriteLine($"Loot {i.GetType()}");
                                        if (loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}"))
                                            itemLoots.Add(new ItemLoot(i));
                                    }

                                if (c2 != null)
                                    foreach (var i in c2.Items)
                                    {
                                        if(loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}"))
                                            itemLoots.Add(new ItemLoot(i));
                                        Console.WriteLine($"Loot {i.GetType()}");
                                    }


                                foreach (var i in bc1.Sorteado)
                                {
                                    if(loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}"))
                                        itemLoots.Add(new ItemLoot(i, true));
                                }

                                foreach (var i in bc2.Sorteado)
                                {
                                    if(loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}"))
                                        itemLoots.Add(new ItemLoot(i, true));
                                }

                               
                                c1.Delete();
                                c2.Delete();
                                Console.WriteLine("Terminando");
                                if(loots.Count > 0)
                                {
                                    var s = string.Join("", loots.Select(l => $"<li>{l}</li>"));
                                    Monstros.Configs[tipo.Name] = new Monstros(bc1);
                                    //outputFile.WriteLine($"<div class='mob'><div class='stats'></div><span>{nome}</span><div class='loots'><ul>{s}</ul></div></div>");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                //Console.WriteLine(e.StackTrace);
                                erros.Add(tipo.Name+"</br>"+e.Message+"</br>"+e.StackTrace+"</br></br>");
                            }
                            Console.WriteLine($"Gerado {tipo.Name}");

                        }
                        //});

                    }
                    Utility.FIX = -1;
                }

                //outputFile.WriteLine(JsonConvert.SerializeObject(Monstros.Configs));
               

                Console.WriteLine("----- FIM -----");
            }

            using (StreamWriter outputFile = new StreamWriter("C:/monstros/erros.html"))
            {
                foreach(var erro in erros)
                    outputFile.WriteLine(erro);
            }
        }
    }
}
