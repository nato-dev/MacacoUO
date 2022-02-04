using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class TithingGump : Gump
    {
        private readonly Mobile m_From;
        private int m_Offer;
        public TithingGump(Mobile from, int offer)
            : base(160, 40)
        {
            int totalGold = Banker.GetBalance(from); //from.TotalGold;

            if(!from.IsCooldown("cruz"))
            {
                from.SetCooldown("cruz");
                from.SendMessage(78, "Voce pode doar dinheiro em ankhs para usar magias de Chivalry");
            }

            if (offer > totalGold)
                offer = totalGold;
            else if (offer < 0)
                offer = 0;

            m_From = from;
            m_Offer = offer;

            AddPage(0);

            AddImage(30, 30, 102);

            string total = totalGold - offer > 100000 ? "100000+" : (totalGold - offer).ToString();

            AddLabel(57, 274, 0, "Saldo:");
            AddLabel(87, 274, 53, total);

            AddLabel(137, 274, 0, "Doar:");
            //AddLabel(172, 274, 53, offer.ToString());
            AddTextEntry(172, 274, 100, 20, 53, 0, offer.ToString());

            AddButton(105, 230, 5220, 5220, 2, GumpButtonType.Reply, 0);
            AddButton(113, 230, 5222, 5222, 2, GumpButtonType.Reply, 0);
            AddLabel(108, 228, 0, "<");
            AddLabel(112, 228, 0, "<");

            AddButton(127, 230, 5223, 5223, 1, GumpButtonType.Reply, 0);
            AddLabel(131, 228, 0, "<");

            AddButton(147, 230, 5224, 5224, 3, GumpButtonType.Reply, 0);
            AddLabel(153, 228, 0, ">");

            AddButton(168, 230, 5220, 5220, 4, GumpButtonType.Reply, 0);
            AddButton(176, 230, 5222, 5222, 4, GumpButtonType.Reply, 0);
            AddLabel(172, 228, 0, ">");
            AddLabel(176, 228, 0, ">");

            AddHtml(95, 100, 120, 100, "Bom Karma Boa Ventura", 1, false, false); // May your wealth bring blessings to those in need, if tithed upon this most sacred site.

            AddButton(217, 272, 4023, 4024, 5, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int offer = 0;
            var txt = info.GetTextEntry(0).Text;
            if (Shard.DebugEnabled)
                Shard.Debug("QTD Doar: " + txt);
            if (txt != null && int.TryParse(txt, out offer))
            {
                Shard.Debug("Doando oq foi digitado");
                m_Offer = offer;
            }

            switch ( info.ButtonID )
            {
                case 0:
                    {
                        // You have decided to tithe no gold to the shrine.
                        m_From.SendMessage("Voce decidiu nao doar nada");
                        break;
                    }
                case 1:
                case 2:
                case 3:
                case 4:
                    {
                        switch ( info.ButtonID )
                        {
                            case 1:
                                offer = m_Offer - 100;
                                break;
                            case 2:
                                offer = 0;
                                break;
                            case 3:
                                offer = m_Offer + 100;
                                break;
                            case 4:
                                offer = Math.Min(100000, Banker.GetBalance(m_From));
                                break;
                        }

                        m_From.SendGump(new TithingGump(m_From, offer));
                        break;
                    }
                case 5:
                    {
                        int totalGold = Banker.GetBalance(m_From);  // m_From.TotalGold;

                        if (m_Offer > totalGold)
                            m_Offer = totalGold;
                        else if (m_Offer < 0)
                            m_Offer = 0;

                        if ((m_From.TithingPoints + m_Offer) > 100000) // TODO: What's the maximum?
                            m_Offer = (100000 - m_From.TithingPoints);

                        if (m_Offer <= 0)
                        {
                            // You have decided to tithe no gold to the shrine.
                            m_From.SendMessage("Voce preferiu nao doar nada");
                            break;
                        }

                        Container pack = m_From.Backpack;

                        if (Banker.Withdraw(m_From, m_Offer, true))
                        {
                            // You tithe gold to the shrine as a sign of devotion.
                            m_From.SendMessage("Voce doou ouro como simbolo de sua devocao");
                            m_From.TithingPoints += m_Offer;

                            m_From.PlaySound(0x243);
                            m_From.PlaySound(0x2E6);
                        }
                        else
                        {
                            // You do not have enough gold to tithe that amount!
                            //m_From.LocalOverheadMessage(MessageType.Regular, 0x7B2, 1060194);
                            m_From.SendMessage("Sua intencao eh nobre porem voce precisa de moedas de ouro para doar");
                        }

                        break;
                    }
            }
        }
    }
}
