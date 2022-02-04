#region References
using System;
using System.Globalization;
using System.Linq;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
#endregion

namespace Server.Engines.TexasHoldem
{
	public class PokerRebuy : Gump
	{
		private readonly PokerGame m_Game;

        public PokerRebuy(Mobile from, PokerGame game)
			: base(50, 50)
		{
			m_Game = game;
            PokerPlayer pokerplayer;

            game.IsPlayer(from as PlayerMobile, out pokerplayer);
            AddBackground(0, 0, 400, 300, 9200);
            AddAlphaRegion(92, 10, 215, 24);
            AddHtml(14, 14, 371, 24, String.Format("<CENTER><BIG><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></BIG></CENTER>", "Comprar o Rebuy ?"), false, false);
            AddImageTiled(20, 34, 347, 2, 9277);
            AddImageTiled(34, 36, 347, 2, 9277);

            AddHtml(20, 41, 365, 130, String.Format("<LEFT><BASEFONT COLOR=#F7D358>{0}</BASEFONT><</LEFT>", "Tem certeza que deseja comprar o Rebuy ? Seu novo buy-in + suas fichas atuais devem ser maior ou igual ao buy-in minimo e nao podem passar o buy-in maximo.  Note que esta e uma maneira facil de perder dinheiro.  Seu dinheiro nao sera retornado se perde-lo."), false, false);
            AddImageTiled(20, 150, 347, 2, 9277);
            AddImageTiled(34, 152, 347, 2, 9277);

            AddHtml(53, 160, 100, 25, String.Format("<LEFT><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></LEFT>", "Min Buy-In:"), false, false);
            AddLabel(125, 160, 1258, m_Game.Dealer.MinBuyIn.ToString("#,0") + " " + (m_Game.Dealer.IsDonation ? "donation coins" : "moedas de ouro"));
            AddHtml(50, 180, 100, 25, String.Format("<LEFT><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></LEFT>", "Max Buy-In:"), false, false);
            AddLabel(125, 180, 1258, m_Game.Dealer.MaxBuyIn.ToString("#,0") + " " + (m_Game.Dealer.IsDonation ? "donation coins" : "moedas de ouro"));
            AddHtml(40, 200, 100, 25, String.Format("<LEFT><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></LEFT>", "Saldo:"), false, false);

            int balance = Banker.GetBalance(from);

            AddLabel(125, 200, balance + pokerplayer.Currency >= m_Game.Dealer.MinBuyIn ? 63 : 137, balance.ToString("#,0") + " " + (m_Game.Dealer.IsDonation ? "donation coins" : "moedas de ouro"));

            AddHtml(34, 220, 100, 25, String.Format("<LEFT><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></LEFT>", "Fichas:"), false, false);
            AddLabel(125, 220, 1258, pokerplayer.Currency.ToString("#,0") + " " + (m_Game.Dealer.IsDonation ? "donation coins" : "moedas de ouro"));

            AddHtml(32, 240, 100, 25, String.Format("<LEFT><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></LEFT>", "Credito Pendente:"), false, false);
            AddLabel(125, 240, 1258, pokerplayer.PendingCredit.ToString("#,0") + " " + (m_Game.Dealer.IsDonation ? "donation coins" : "moedas de ouro"));

            AddHtml(31, 260, 100, 25, String.Format("<LEFT><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></LEFT>", "Quantidade de Buy-In:"), false, false);

            AddImageTiled(125, 260, 80, 19, 0xBBC);
            AddAlphaRegion(125, 260, 80, 19);
            if (pokerplayer.Currency + pokerplayer.PendingCredit > m_Game.Dealer.MaxBuyIn)
                AddTextEntry(128, 260, 77, 19, 99, (int)Handlers.txtBuyInAmount, 0.ToString());
            else
            {
                AddTextEntry(128, 260, 77, 19, 99, (int)Handlers.txtBuyInAmount, (m_Game.Dealer.MaxBuyIn - pokerplayer.Currency - pokerplayer.PendingCredit).ToString());                
            }

            AddButton(250, 260, 247, 248, (int)Handlers.btnOkay, GumpButtonType.Reply, 0);
            AddButton(320, 260, 242, 241, (int)Handlers.btnCancel, GumpButtonType.Reply, 0);
		}

		public enum Handlers
		{
			btnOkay = 1,
			btnCancel,
			txtBuyInAmount
		}

	    public override void OnResponse(NetState state, RelayInfo info)
	    {
	        if (state == null)
	            return;

	        Mobile from = state.Mobile;
	        int buyInAmount;

	        if (info.ButtonID != 1)
	        {
	            return;
	        }

	        if (m_Game == null || from == null || m_Game.Dealer == null)
	            return;

            PokerPlayer pokerplayer;

            m_Game.IsPlayer(from as PlayerMobile, out pokerplayer);

	        if (pokerplayer != null)
	        {
	            int balance = Banker.GetBalance(from);

	            int currency = pokerplayer.Currency;

	            if (balance + currency < m_Game.Dealer.MinBuyIn)
	            {
	                from.SendMessage(
	                    0x22,
	                    "Voce nao tem {0} suficientes para entrar novamente no jogo. Buy-in minimo: {1:#,0}",
                        (m_Game.Dealer.IsDonation ? "donation coins" : "moedas de ouro"),
	                    m_Game.Dealer.MinBuyIn);

	                return;
	            }


	            var t = info.GetTextEntry(3);

	            if (t == null)
	                return;

	            if (!Int32.TryParse(t.Text, out buyInAmount))
	            {
	                from.SendMessage(0x22, "Use numeros sem virgulas (ex 25000)");
	                return;
	            }

                if (buyInAmount > balance)
                {
                    from.SendMessage(
                        0x22,
                        "Voce nao tem {0} suficientes.",
                        (m_Game.Dealer.IsDonation ? "donation coins" : "moedas de ouro"));

                    return;
                }

                if (buyInAmount + currency < m_Game.Dealer.MinBuyIn)
                {
                    from.SendMessage(
                        0x22,
                        "Seu valor precisa ser pelo menos igual ao buy-in minimo. Buy-in minimo: {0:#,0}",
                        m_Game.Dealer.MinBuyIn);

                    return;
                }

                if (buyInAmount + currency > m_Game.Dealer.MaxBuyIn)
                {
                    from.SendMessage(
                        0x22,
                        "O valor especificado de buy-in + as fichas que voce ja tem iriam alem do buy-in maximo. Buy-in maximo: {0:#,0}",
                        m_Game.Dealer.MaxBuyIn);

                    return;
                }

                pokerplayer.PendingCredit = buyInAmount;
	        }
}
	}
}
