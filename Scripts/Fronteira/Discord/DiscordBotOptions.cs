#region Header
//   Vorspire    _,-'/-'/  DiscordBotOptions.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;

using Server;
#endregion

namespace VitaNex.Modules.Discord
{
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
