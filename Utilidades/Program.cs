using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilidades;

namespace TestesConsole
{
    public class FormataCliloc
    {
        static string arquivo = "C:\\Users\\gabri\\Desktop\\ServUO\\GIT\\uofiddler\\trad2.csv";
        static string arquivoOUT = "C:\\Users\\gabri\\Desktop\\ServUO\\GIT\\uofiddler\\trad2_final.csv";

        static string arquivoItem = "C:\\Users\\gabri\\Desktop\\ServUO\\Fiddler\\itemdatacsv.csv";
        static string arquivoItemOUT = "C:\\Users\\gabri\\Desktop\\ServUO\\Fiddler\\itemdatafinal.csv";

        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        public static string Formata(string s, bool acentos = true)
        {
            if (acentos)
            {
                s = removerAcentos(s);
            }
            if (s.Contains("~"))
            {
                // 0        1           2            3        4
                // mais ~ name_1 ~ coisa e outra ~ name_2 ~ coisa
                var res2 = "";
                var pula = false;
                var fim = false;
                foreach (var letra in s)
                {
                    if (pula)
                    {
                        pula = false;
                        if (letra == ' ')
                            continue;
                    }

                    if (letra == '~' && !pula)
                    {
                        if (!fim)
                        {
                            pula = true;
                            fim = true;
                        }
                        else
                        {
                            fim = false;
                            var ultimoChar = res2.Substring(res2.Length - 1);
                            if (ultimoChar == " ")
                                res2 = res2.Substring(0, res2.Length - 1);
                        }

                    }

                    res2 += letra;
                }
                s = res2;
            }
            return s;
        }

        public static void ParseClilocData()
        {
            var txt = System.IO.File.ReadAllText(arquivo, Encoding.GetEncoding("iso-8859-1"));
            var csv = new StringBuilder();
            var n = 0;
            var split = txt.Split(';');
            for (var i = 0; i < split.Length; i++)
            {
                var text = split[i];
                if (Int32.TryParse(text, out n))
                {
                    var linha = split[i] + ";" + split[i + 1] + ";" + split[i + 2];
                    Console.WriteLine(linha);
                    csv.AppendLine(linha);
                    i += 2;
                }
            }
            Console.WriteLine("Escrevendo Arquivo");
            File.WriteAllText(arquivoOUT, csv.ToString());
        }

        public static void ParseItemData()
        {
            Console.WriteLine("Lendo Cliloc");
            string[] lines = System.IO.File.ReadAllLines(arquivo, Encoding.UTF8);

            Console.WriteLine("Parseando");
            var csv = new StringBuilder();
            var max = lines.Length;
            var ct = 0;

            foreach (var text in lines)
            {

                csv.AppendLine(Formata(text));
                ct++;
                Console.WriteLine(ct + "/" + max);
            }


            Console.WriteLine("Escrevendo Arquivo");
            File.WriteAllText(arquivoOUT, csv.ToString(), Encoding.GetEncoding("iso-8859-1"));
            Console.WriteLine("Escrito");
            Console.ReadLine();
        }

        public static void ParseFixItems()
        {
            Console.WriteLine("Lendo Cliloc");
            string[] lines = System.IO.File.ReadAllLines(arquivoItem, Encoding.UTF8);

            Console.WriteLine("Parseando");
            var csv = new StringBuilder();
            var max = lines.Length;
            var ct = 0;

            foreach (var text in lines)
            {

                var str = "";
                var split = text.Split(';');

                var count = split.Length;

                split[1] = Formata(split[1]);
                split[2] = Formata(split[2]);

                split = new string[3] { split[0], split[1], split[2] };

                var frase = string.Join(";", split);
                Console.WriteLine(frase);
                csv.AppendLine(frase);
                ct++;
                Console.WriteLine(ct + " / " + max);
            }


            Console.WriteLine("Escrevendo Arquivo");
            File.WriteAllText(arquivoOUT, csv.ToString(), Encoding.GetEncoding("iso-8859-1"));
            Console.ReadLine();
        }

        public static void ParseLinhas()
        {
            Console.WriteLine("Lendo Cliloc");
            string[] lines = System.IO.File.ReadAllLines(arquivo, Encoding.GetEncoding("iso-8859-1"));

            Console.WriteLine("Parseando");
            var csv = new StringBuilder();
            var max = lines.Length;
            var ct = 0;

            var linha = "";
            for (var x = 0; x < lines.Length; x++)
            {
                var text = lines[x];
                var str = "";
                var split = text.Split(';');

                var count = split.Length;

                var id = 0;
                if (Int32.TryParse(split[0], out id))
                {
                    if (split.Length >= 3)
                    {
                        linha = text;
                    }
                    else

                    {
                        var proxima = lines[x + 1];
                        var ex = proxima.Split(';')[1];
                        linha = text + ex.Split(' ')[1];
                        Console.WriteLine(linha);
                    }
                }


                csv.AppendLine(linha);
                ct++;
                Console.WriteLine(ct + " / " + max);
            }


            Console.WriteLine("Escrevendo Arquivo");
            File.WriteAllText(arquivoOUT, csv.ToString());
            Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            //ParseFixItems();
            //ParseLinhas();
            //ParseClilocData();


            //ParseItemData();
            TesteDB.Run();
        }


    }


}
