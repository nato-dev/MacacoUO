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
    public class Monstros
    {
        public static void GeraDocMonstros()
        {
            List<string> erros = new List<string>();
            using (StreamWriter outputFile = new StreamWriter("C:/monstros/monstros.html"))
            {
           

                LootPack.OldMagicItems.Clear();
                LootPack.GemItems.Clear();

                outputFile.WriteLine("<html><body>");
                BaseCreature.BypassTimerInicial = true;
                var docs = new Dictionary<string, string>();
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

                                var loots = new HashSet<string>();
                                if (c1 != null)
                                    foreach (var i in c1.Items)
                                    {
                                        Console.WriteLine($"Loot {i.GetType()}");
                                        loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}");
                                    }

                                if (c2 != null)
                                    foreach (var i in c2.Items)
                                    {
                                        loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}");
                                        Console.WriteLine($"Loot {i.GetType()}");
                                    }


                                foreach (var i in bc1.Sorteado)
                                {
                                    Console.WriteLine($"Sorteio {i.GetType()}");
                                    loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}");
                                }

                                foreach (var i in bc2.Sorteado)
                                {
                                    Console.WriteLine($"Sorteio {i.GetType()}");
                                    loots.Add($"{i.Amount}x {i.Name ?? i.GetType().Name}");
                                }

                                var nome = bc1.GetType().Name;
                                if (bc1.Name != null)
                                {
                                    nome = $"{bc1.Name} ({nome})";
                                }
                                c1.Delete();
                                c2.Delete();
                                Console.WriteLine("Terminando");
                                if(loots.Count > 0)
                                {
                                    docs[nome] = string.Join("", loots.Select(l => $"<li>{l}</li>"));
                                    outputFile.WriteLine($"<div class='mob'><span>{nome}</span><div class='loots'><ul>{docs[nome]}</ul></div></div>");
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
                Console.WriteLine("----- FIM -----");
                outputFile.WriteLine("</body></html>");
            }

            using (StreamWriter outputFile = new StreamWriter("C:/monstros/erros.html"))
            {
                foreach(var erro in erros)
                    outputFile.WriteLine(erro);
            }
        }
    }
}
