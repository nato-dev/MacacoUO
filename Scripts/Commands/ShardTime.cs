using System;

namespace Server.Commands
{
	public class ShardTime
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Time", AccessLevel.Player, new CommandEventHandler( Time_OnCommand ) );
		}

		[Usage( "Time" )]
		[Description( "Retorna a hora local do servidor." )]
		private static void Time_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( DateTime.UtcNow.ToString() );
		}
	}
}
