#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Poker.Gumps;

#endregion

namespace Server.Engines.TexasHoldem
{
    public class PokerPlayer
    {
        public int Currency { get; set; }
        public int AmountWon { get; set; }
        public int AmountToReturn { get; set; }
        public int StartCurrency { get; set; }

        public int PendingCredit { get; set; }
        public int TotalBetInRound { get; set; }

        public bool RequestLeave { get; set; }
        public bool HasActed { get; set; }
        public bool HasFolded { get; set; }

        public PlayerMobile Owner { get; set; }
        public Point3D Seat { get; set; }
        public TimeSpan TurnEnd { get; set; }

        public List<Card> HoleCards { get; private set; }

        public uint HandRank { get; set; }

        public PokerPlayer(PlayerMobile owner, int buyin, Point3D seat, PokerGame game)
        {
            Owner = owner;
            Owner.PokerGame = game;
            Seat = seat;
            Currency = buyin;
            HoleCards = new List<Card>();
            TeleportToSeat();
        }

        public void Payout(Type coinType)
        {
            //bool isDonationCurrency = coinType == typeof(DonationCoin);
            bool isDonationCurrency = false;

            if (Currency > 0)
            {
                int amt = 0;
                do
                {
                    if (Currency >= 1000000)
                    {
                        amt += 1000000;
                        Currency -= amt;

                    }
                    else
                    {
                        amt += Currency;
                        Currency = 0;
                    }

                    Owner.BankBox.DropItem(new BankCheck(amt));
                }
                while (Currency > 0);

                StringBuilder sb = new StringBuilder();
                sb.Append(String.Format("Um cheque de {0:n0} {1} foi colocado no seu banco.", amt,
                    isDonationCurrency ? "donation" : "moedas de ouro"));
            
                Owner.SendMessage(38, sb.ToString());
            }
        }

        public void LeaveGame(PokerGame game)
        {
            Payout(game.TypeOfCurrency);
            Owner.SendMessage(0x22, "Voce saiu da mesa.");
            Owner.Blessed = false;
            if (Owner.AccessLevel < AccessLevel.GameMaster)
                Owner.PokerJoinTimer = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            Owner.PokerGame = null;
            CloseAllGumps();
        }

        public void ClearGame()
        {
            StartCurrency = Currency;
            AmountWon = 0;
            AmountToReturn = 0;
            HasFolded = false;
            HoleCards.Clear();
            CloseAllGumps();
            ClearRound();
        }

        public void ClearRound()
        {
            TotalBetInRound = 0;
            HasActed = false;
        }

        public void AwardCredit(int amount)
        {
            AmountWon += amount;
        }

        public void ReturnCredit(int amount)
        {
            AmountToReturn += amount;
        }

        public void MakeBet(int amount)
        {
            Currency -= amount;
            TotalBetInRound += amount;
        }

        public void DistributeCredit(PokerGame game)
        {
            string currencyType = game.TypeOfCurrency == typeof(Gold) ? "moedas de ouro" : "donation coins";
            var publicmessage = string.Empty;
            if (AmountWon > 0)
            {
                publicmessage = string.Format("Ganhou {0:n0} {1}.", AmountWon, currencyType);
                Currency += AmountWon;
            }
            else if (AmountWon < 0)
            {
                publicmessage = string.Format("Perdeu {0:n0} {1}.", Math.Abs(AmountWon), currencyType);
            }

            if (!string.IsNullOrEmpty(publicmessage))
                Owner.PublicOverheadMessage(MessageType.Spell, 36, true, publicmessage);

            if (AmountToReturn > 0)
            {
                Owner.SendMessage(38, string.Format("{0} {1} retornou a voce.", AmountToReturn, currencyType));
                Currency += AmountToReturn;
            }
        }

        public void ProcessCredit(int minbuyin, int maxbuyin, Type currencyType)
        {
            if (PendingCredit > 0)
            {
                if ((PendingCredit + Currency) > minbuyin &&
                    (PendingCredit + Currency) < maxbuyin &&
                    Banker.Withdraw(Owner, PendingCredit))
                {
                    Currency += PendingCredit;
                    Owner.SendMessage(61,
                        string.Format("Voce sacou {0:n0} {1}.", PendingCredit,
                            currencyType == typeof(Gold) ? "moedas" : "donation"));
                }
                else if ((PendingCredit + Currency) > maxbuyin)
                {
                    int diff = maxbuyin - Currency;

                    if (diff > 0)
                    {
                        if (Banker.Withdraw(Owner, diff))
                        {
                            Currency += diff;
                            Owner.SendMessage(61,
                                string.Format("Voce sacou {0:n0} {1}.", diff,
                                    currencyType == typeof(Gold) ? "moedas" : "donation"));
                        }
                        else
                        {
                            Owner.SendMessage(61,
                                "Voce nao tem dinheiro suficiente no banco.");
                        }
                    }
                    else
                    {
                        Owner.SendMessage(61,
                            "Voce ja esta no buy-in maximo.");
                    }
                }
                else if ((PendingCredit + Currency) < minbuyin)
                {
                    Owner.SendMessage(61,
                        "Seu rebuy nao chega a ser suficiente para o buy-in.");
                }
                PendingCredit = 0;
            }
        }

        public void AddCard(Card card)
        {
            HoleCards.Add(card);
        }

        public void RankHand(List<Card> cards)
        {
            var sb = new StringBuilder();

            var allcards = new List<Card>(HoleCards);

            if (cards != null && cards.Count > 0)
                allcards.AddRange(cards);

            foreach (var card in allcards)
            {
                sb.Append(string.Format("{0}{1}", card.GetRankLetter(), card.GetSuitLetter()));
            }
            HandRank = Hand.Evaluate(sb.ToString());
        }

        public string GetHandString()
        {
            var hand = Hand.DescriptionFromHandValueInternal(HandRank);

            return string.Format("{0}", hand);
        }

        public void CloseAllGumps()
        {
            CloseGump(typeof(PokerLeaveGump));
            CloseGump(typeof(PokerJoinGump));
            CloseGump(typeof(PokerBetGump));
        }

        public void CloseGump(Type type)
        {
            if (Owner != null)
            {
                Owner.CloseGump(type);
            }
        }

        public void SendGump(Gump toSend)
        {
            if (Owner != null)
            {
                Owner.SendGump(toSend);
            }
        }

        public void PublicMessage(string message)
        {
            SendMessage(message);
        }

        public void SendMessage(string message)
        {
            if (Owner != null)
            {
                Owner.SendMessage(message);
            }
        }

        public void SendMessage(int hue, string message)
        {
            if (Owner != null)
            {
                Owner.SendMessage(hue, message);
            }
        }

        public void TeleportToSeat()
        {
            if (Owner != null && Seat != Point3D.Zero)
            {
                Owner.Blessed = true;
                Owner.Location = Seat;
                SendMessage(0x22, "Voce se sentou na mesa");
            }
        }

        public bool IsOnline()
        {
            return Owner != null && Owner.NetState != null && Owner.NetState.Socket != null &&
                   Owner.NetState.Socket.Connected;
        }
    }
}
