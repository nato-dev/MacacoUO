
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Utilidades
{
    public class TesteDB
    {
        static HttpClient client = new HttpClient();

        public static void Run()
        {
            Console.WriteLine("Ini");
            var login = "admin";
            var pass = "TUUetdyg6";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            Console.Write("Requestando");
            var stringTask = client.GetStringAsync("http://ultimafronteirashard.com.br/wp-json/LudosParadise/apiconta/" + login + "/" + pass).Result;
            Console.Write(stringTask);
            Console.ReadLine();
        }
    }
}
