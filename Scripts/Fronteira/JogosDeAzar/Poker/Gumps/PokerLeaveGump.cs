#region References

using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
#endregion

namespace Server.Engines.TexasHoldem
{
	public class PokerLeaveGump : Gump
	{
		private readonly PokerGame _MGame;

		public PokerLeaveGump(PokerGame game)
			: base(50, 50)
		{
			_MGame = game;

            AddBackground(0, 0, 400, 165, 9200);
            AddAlphaRegion(115, 10, 165, 24);
            AddHtml(14, 14, 371, 24, String.Format("<CENTER><BIG><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></BIG></CENTER>", "Sair da mesa ?"), false, false);
            AddImageTiled(20, 34, 347, 2, 9277);
            AddImageTiled(34, 36, 347, 2, 9277);

            AddHtml(20, 41, 365, 130, String.Format("<LEFT><BASEFONT COLOR=#F7D358>{0}</BASEFONT><</LEFT>", "Voce tem certeza que deseja pegar seu dinheiro e sair ? Voce jogara essa ultima mao e ai os seus ganhos serao depositados no seu banco. Voce nao podera entrar em outra mesa de poker por 60 segundos."), false, false);
            AddImageTiled(20, 115, 347, 2, 9277);
            AddImageTiled(34, 117, 347, 2, 9277);

            AddButton(250, 125, 247, 248, (int)Handlers.BtnOkay, GumpButtonType.Reply, 0);
            AddButton(320, 125, 242, 241, (int)Handlers.None, GumpButtonType.Reply, 0);
		}

		public enum Handlers
		{
			None,
			BtnOkay
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			if (from == null)
			{
				return;
			}

            PokerPlayer player;

            _MGame.IsPlayer(from as PlayerMobile, out player);

			if (player != null)
			{
				if (info.ButtonID == 1)
				{
					if (_MGame.State == PokerGameState.Inactive || _MGame.State == PokerGameState.Intermission)
					{
						if (_MGame.Players.Contains(player))
						{
							_MGame.RemovePlayer(player);
						}
						return;
					}

					if (player.RequestLeave)
					{
						from.SendMessage(0x22, "Voce ja pediu para sair.");
					}
					else
					{
						from.SendMessage(0x22, "Voce enviou uma requisicao para sair da mesa.");
						player.RequestLeave = true;
					}
				}
			}
		}
	}
}
