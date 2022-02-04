
using Server.Network;
using Server.Mobiles;
using Server.Misc.Custom;

namespace Server.Gumps
{
    public class BankInfoGump : Gump
    {
        public BankInfoGump(Mobile m) : base(30, 30)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            var nivel = ((PlayerMobile)m).NivelBanco;

            var infoNivel = BankLevels.Niveis[nivel];

            var items = m.BankBox.TotalItems + " / " + infoNivel.Items;
            var peso = m.BankBox.TotalWeight + " / " + infoNivel.Stones;

            AddPage(0);
            AddBackground(14, 16, 315, 217, 9200);
            AddHtml(73, 33, 227, 20, @"Informacao do Banco", (bool)false, (bool)false);
            AddBackground(23, 72, 292, 119, 3500);
            AddItem(25, 19, 2475);
            AddItem(39, 83, 7150);
            AddHtml(82, 90, 209, 19, @"Items: "+items, (bool)false, (bool)false);
            AddItem(36, 115, 6225);
            AddHtml(83, 121, 209, 19, @"Peso: "+peso, (bool)false, (bool)false);
            AddItem(38, 148, 3823);
            AddHtml(84, 153, 209, 19, @"Saldo: "+m.BankBox.TotalGold, (bool)false, (bool)false);
            AddHtml(45, 199, 267, 28, @"Aprimorar", (bool)true, (bool)false);
            AddButton(27, 198, 250, 250, (int)1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)1:
                    {
                        var nivelMax = BankLevels.Niveis.Count - 1;
                        var nivelAtual = ((PlayerMobile)from).NivelBanco;

                        if (nivelAtual >= nivelMax)
                        {
                            from.SendMessage("Voce ja eh nosso maior cliente VIP. Para aprimorar mais ainda seu banco aguarde construirmos cofres maiores.");
                            break;
                        }
                        from.CloseAllGumps();
                        from.SendGump(new UpgradeBankGump(from));
                        break;
                    }
            }
        }
    }
}
