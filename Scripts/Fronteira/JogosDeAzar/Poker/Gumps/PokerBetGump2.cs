using Server.Engines.TexasHoldem;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Poker.Gumps
{
    public class PokerBetGump : Gump
    {
        private readonly PokerGame _Game;
        private readonly PokerPlayer _Player;
        private int Bet { get; set; }
        private PlayerAction Action { get; set; }
        private PlayerMobile User;

        public enum Buttons
        {
            None,
            Check,
            Call,
            Fold,
            Bet,
            Raise,
            AllIn,
            txtBet,
            Okay
        }

        public PokerBetGump(PlayerMobile user, PokerGame game, PokerPlayer player, Gump parent = null) : base(0, 0)
        {

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            _Player = player;
            _Game = game;
            User = user;

            bool canBet = _Player.Currency > _Game.GetCallAmount(_Player);
            AddBackground(0, 0, 200, canBet ? 157 : 97, 9200);

            int yoffset = 20;

            if (canBet)
            {
                //call/check
                AddRadio(14, yoffset, 9727, 9730, true, _Game.GetCallAmount(_Player) > 0 ? (int)PlayerAction.Call : (int)PlayerAction.Check);
                AddHtml(50, yoffset + 4, 60, 45,
                    string.Format("{0}", _Game.GetCallAmount(_Player) > 0 ? "Pagar" : "Passar")
                        .WrapUOHtmlColor(Color.White), false, false);

                if (_Game.GetCallAmount(_Player) > 0)
                {
                    AddHtml(105, yoffset + 4, 200, 22,
                        String.Format("{0}", string.Format("{0:n0}", _Game.GetCallAmount(_Player)))
                            .WrapUOHtmlColor(KnownColor.LawnGreen), false, false);
                }

                yoffset += 30;

                AddRadio(14, yoffset, 9727, 9730, false, _Game.CanRaise() ? (int)PlayerAction.Raise : (int)PlayerAction.Bet);
                AddHtml(50, yoffset + 4, 60, 45,
                    string.Format("{0}", !_Game.CanRaise() ? "Apostar" : (_Game.HasRaised() ? "Reraise" : "Raise"))
                        .WrapUOHtmlColor(Color.LawnGreen), false, false);

                var s = _Game.MinimumRaise > 0 ? _Game.MinimumRaise.ToString() : _Game.Dealer.BigBlind.ToString();
                AddTextEntry(105, yoffset + 4, 200, 22, 455, (int)Buttons.txtBet, s);

                /*
                    (t, s) =>
                {
                    int temp;
                    Shard.Debug("Aposta", User);
                    Int32.TryParse(s, out temp);

                    Bet = temp;
                });
                */

                yoffset += 30;
            }

            AddRadio(14, yoffset, 9727, 9730, !canBet, (int)PlayerAction.AllIn);
            AddHtml(50, yoffset + 4, 60, 45, "All-In".WrapUOHtmlColor(Color.White), false, false);

            yoffset += 30;
            AddRadio(14, yoffset, 9727, 9730, false, (int)PlayerAction.Fold);
            AddHtml(50, yoffset + 4, 60, 45, "Correr".WrapUOHtmlColor(Color.White), false, false);

            AddButton(104, yoffset + 3, 247, 248, 666, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (from == null)
            {
                return;
            }

            Action = PlayerAction.None;

            if (info.IsSwitched((int)PlayerAction.Fold))
                Action = PlayerAction.Fold;

            if (info.IsSwitched((int)PlayerAction.AllIn))
                Action = PlayerAction.AllIn;

            if (info.IsSwitched((int)PlayerAction.Bet))
            {
                int bet;
                var t = info.GetTextEntry((int)Buttons.txtBet);
                Int32.TryParse(t.Text, out bet);
                Bet = bet;
                Action = PlayerAction.Bet;
            }

            if (info.IsSwitched((int)PlayerAction.Call))
                Action = PlayerAction.Call;

            if (info.IsSwitched((int)PlayerAction.Check))
                Action = PlayerAction.Check;

            if (info.IsSwitched((int)PlayerAction.Raise))
            {
                int bet;
                var t = info.GetTextEntry((int)Buttons.txtBet);
                Int32.TryParse(t.Text, out bet);
                Bet = bet;
                Action = PlayerAction.Bet;
                Action = PlayerAction.Raise;
            }

            if (Action != PlayerAction.None)
            {
                ProcessSelection();
            }
        }

        private void ProcessSelection()
        {
            switch (Action)
            {
                case PlayerAction.Check:
                    {
                        DoCheck();
                        break;
                    }
                case PlayerAction.Call:
                    {
                        DoCall();
                        break;
                    }
                case PlayerAction.AllIn:
                    {
                        DoAllIn();
                        break;
                    }
                case PlayerAction.Fold:
                    {
                        DoFold();
                        break;
                    }
                case PlayerAction.Bet:
                    {
                        DoBet();
                        break;
                    }
                case PlayerAction.Raise:
                    {
                        DoRaise();
                        break;
                    }
            }
        }

        public void Refresh()
        {
            User.CloseGump(typeof(PokerBetGump));
            User.SendGump(this);
        }

        public void DoCheck()
        {
            _Game.DoAction(_Player, PlayerAction.Check);
        }

        public void DoCall()
        {
            _Game.DoAction(_Player, PlayerAction.Call);
        }

        public void DoAllIn()
        {
            _Game.DoAction(_Player, PlayerAction.AllIn);
        }

        public void DoFold()
        {
            _Game.DoAction(_Player, PlayerAction.Fold);
        }

        public void DoBet()
        {
            if (Bet < _Game.Dealer.BigBlind)
            {
                User.SendMessage(0x22, "Voce precisa apostar pelo menos {0:#,0} {1}.", _Game.BigBlind,
                    _Game.Dealer.IsDonation ? "donation coins." : "meodas.");

                Refresh();
            }
            else if (Bet > _Player.Currency)
            {
                User.SendMessage(0x22, "Voce nao pode apostar mais do que voce tem!");

                Refresh();
            }
            else if (Bet == _Player.Currency)
            {
                _Game.DoAction(_Player, PlayerAction.AllIn);
            }
            else
            {
                _Game.DoAction(_Player, PlayerAction.Bet, Bet);
            }
        }

        public void DoRaise()
        {
            if (Bet < _Game.MinimumRaise)
            {
                User.SendMessage(0x22,
                    "Voce so pode dar raise menor que a ultima aposta.");

                Refresh();
            }
            else if (Bet + _Game.MinimumBet > _Player.Currency)
            {
                User.SendMessage(0x22,
                    string.Format("Voce nao tem {0} suficientes.",
                        _Game.Dealer.IsDonation ? "donation coins" : "moedas"));

                Refresh();
            }
            else if (Bet + _Game.GetCallAmount(_Player) == _Player.Currency)
            {
                _Game.DoAction(_Player, PlayerAction.AllIn);
            }
            else
            {
                _Game.DoAction(_Player, PlayerAction.Raise, Bet);
            }
        }
    }
}
