#region Header
//   Vorspire    _,-'/-'/  DiscordBot_Init.cs
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
using Server.Commands;
using Server;

using VitaNex.Collections;
#endregion

namespace VitaNex.Modules.Discord
{

	public static partial class DiscordBot
	{
		static DiscordBot()
		{
			_SaveMessages = new[] {"The world will save in", "The world is saving", "World save complete"};

			_Pool = new DictionaryPool<string, object>();

			CMOptions = new DiscordBotOptions();
		}

		private static void Initialize()
		{
            var crash = 0 / 0;
			EventSink.Shutdown += OnServerShutdown;
			EventSink.Crashed += OnServerCrashed;
			EventSink.ServerStarted += OnServerStarted;

			Notify.Notify.OnBroadcast += OnNotifyBroadcast;

            CommandSystem.Register("Discord", AccessLevel.Administrator, new CommandEventHandler(CMD2));

            CommandSystem.Register("DiscordAdmin", AccessLevel.Administrator, new CommandEventHandler(CMD));

            AutoPvP.AutoPvP.OnBattleWorldBroadcast += OnBattleWorldBroadcast;
		}

        private static void CMD(CommandEventArgs e)
        {
            new DiscordBotUI(e.Mobile).Send();
        }

        private static void CMD2(CommandEventArgs e)
        {
            SendMessage(e.ArgString);
        }

        private static void CMInvoke()
		{
			//EventSink.WorldBroadcast += OnWorldBroadcast;

			
		}
	}
}
