using System;
using System.Linq;
using System.Web;
using Server;
using Server.Misc;
using Server.Commands;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace VitaNex.Modules.Discord
{
    public class DiscordBot
    {
        public static DiscordBotOptions CMOptions { get; private set; }

        private static void OnServerShutdown(ShutdownEventArgs e)
        {
            if (CMOptions.HandleStatus)
            {
                SendMessage("TestShard: Offline - Manutencao");
            }
        }

        private static void OnServerCrashed(CrashedEventArgs e)
        {
            if (CMOptions.HandleStatus)
            {
                SendMessage(":cross: TestShard: Offline (Crash - Reiniciando !)");
            }
        }

        private static void OnServerStarted()
        {
            if (CMOptions.HandleStatus)
            {
                SendMessage(":white_check_mark: TestShard Online !");
            }
        }

        private static void OnNotifyBroadcast(string message)
        {
            if (CMOptions.HandleNotify)
            {
                SendMessage(message);
            }
        }

        /*
		private static void OnBattleWorldBroadcast(PvPBattle b, string text)
		{
			if (CMOptions.HandleBattles)
			{
				SendMessage(text);
			}
		}
        */

        public static void SendMessage(string message)
        {
            SendMessage(message, true);
        }

        public static void SendMessage(string message, bool filtered)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }

            message = message.StripHtmlBreaks(true).StripHtml(false);

            Requesta(message);
        }

        private static async void Requesta(string message)
        {
            var uri = GetWebhookUri();

            if (Shard.DebugEnabled)
                Shard.Debug("URL Discord " + uri);

            if (uri.Contains("NULL"))
            {
                return;
            }

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            Shard.Debug("Escrevendo Stream");
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"content\":\"" + message + "\"," +
                 "\"username\":\"Dragonic\"}";
                stream.Write(json);
            }
            request.GetResponseAsync();
        }

        public static string GetWebhookUri()
        {
            return GetWebhookUri(true);
        }

        public static string GetWebhookUri(bool debug)
        {
            return GetWebhook(debug).ToString();
        }

        public static Uri GetWebhook(bool debug)
        {
            var id = CMOptions.WebhookID;
            var key = CMOptions.WebhookKey;

            if (String.IsNullOrWhiteSpace(id))
            {
                id = "NULL";
            }

            if (String.IsNullOrWhiteSpace(key))
            {
                key = "NULL";
            }
            return new Uri("https://discord.com/api/webhooks/" + id + "/" + key);
        }

        static DiscordBot()
        {
            CMOptions = new DiscordBotOptions();
            CMOptions.SetDefaults();
        }

        public static void Initialize()
        {
            EventSink.Shutdown += OnServerShutdown;
            EventSink.Crashed += OnServerCrashed;
            EventSink.ServerStarted += OnServerStarted;
            Notify.Notify.OnBroadcast += OnNotifyBroadcast;
            CommandSystem.Register("Discord", AccessLevel.Administrator, new CommandEventHandler(CMD2));
            //AutoPvP.AutoPvP.OnBattleWorldBroadcast += OnBattleWorldBroadcast;
        }

        private static void CMD2(CommandEventArgs e)
        {
            SendMessage(e.ArgString);
        }

    }

    public class DiscordBotOptions
    {

        public string WebhookID { get; set; }


        public string WebhookKey { get; set; }


        public string WebhookDebugID { get; set; }


        public string WebhookDebugKey { get; set; }


        public bool FilterSaves { get; set; }


        public bool FilterRepeat { get; set; }


        public bool HandleBroadcast { get; set; }


        public bool HandleNotify { get; set; }


        public bool HandleBattles { get; set; }


        public bool HandleStatus { get; set; }

        public void SetDefaults()
        {

            WebhookID = "942521189895647246";
            WebhookKey = "ft-5_yeqhzZGLcgROXD_Igdz9TZKFyZuKvB3mi3hYlKmdfqaYx1VJ80yEXX1WbZ_hFPX";

            WebhookDebugID = String.Empty;
            WebhookDebugKey = String.Empty;

            FilterSaves = true;
            FilterRepeat = true;

            HandleBroadcast = true;
            HandleNotify = true;
            HandleBattles = true;
            HandleStatus = true;
        }


    }

}
