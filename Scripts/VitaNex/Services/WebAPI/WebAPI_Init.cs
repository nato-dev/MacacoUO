#region Header
//   Vorspire    _,-'/-'/  WebAPI_Init.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

using Server;
using Server.Misc;
using VitaNex.Modules.WebStats;
#endregion

namespace VitaNex.Web
{
	[CoreService("Web API", "3.0.0.1")]
	public static partial class WebAPI
	{
		static WebAPI()
		{
            return;

			CSOptions = new WebAPIOptions();

			Clients = new List<WebAPIClient>();

			Handlers = new Dictionary<string, WebAPIHandler>();

			_ActivityTimer = PollTimer.FromSeconds(
				60.0,
				() =>
				{
					if (!_Listening || Listener == null || Listener.Server == null || !Listener.Server.IsBound)
					{
						_Listening = false;
						ListenerUtility.ListenAsync();
					}

					Clients.RemoveAll(c => !c.Connected);
				},
				() => Clients.Count > 0,
				false);

			ServicePointManager.SecurityProtocol = (SecurityProtocolType)0xFF0; // Ssl3, Tls, Tls11, Tls12
		}

		private static void CSConfig()
		{
            return;
			if (_ServerStarted)
			{
				ListenerUtility.ListenAsync();
				return;
			}

			EventSink.ServerStarted += () =>
			{
				_ServerStarted = true;

				ListenerUtility.ListenAsync();
			};

			var t = typeof(StatusPage);
			var f = t.GetField("Enabled", BindingFlags.Public | BindingFlags.Static);

			if (f != null)
			{
				f.SetValue(null, false);
			}
		}

		private static void CSInvoke()
        {
            return;
            _ActivityTimer.Start();

			//Register("/", HandleRoot);
            //Register("/registrarmanolo", WebStats.HandleRegistro);
            //Register("/online", WebStats.HandleOnline);
        }

		private static void CSDisposed()
        {
            return;
            Clients.ForEachReverse(c => c.Close());

			_ActivityTimer.Stop();
			_ActivityTimer = null;

			if (Listener == null)
			{
				return;
			}

			Listener.Stop();
			Listener = null;
		}
	}
}
