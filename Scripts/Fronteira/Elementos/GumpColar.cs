using Server.Network;
using Server.Commands;
using Server.Mobiles;
using Server.Items;
using Server.Fronteira.Elementos;
using Server.Leilaum;

namespace Server.Gumps
{
    public class ColarElementalGump : Gump
    {


        private ElementoPvM e;

        public ColarElementalGump(PlayerMobile pl, ColarElemental colar) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(473, 134, 400, 140, 9200);

            var nivel = colar.Nivel;

            AddBackground(673-100, 334, 111, 101, 3500);
            AddHtml(711 - 100, 350, 183, 22, 20.ToString(), (bool)false, (bool)false);
            AddHtml(678 - 100, 406, 100, 22, "Jarro de Po Magico", (bool)true, (bool)false);
            //AddItem(703, 374, custos.Item);
            NewAuctionGump.AddItemCentered(673 - 100, 334, 111, 101, 3823, 0, this);

            AddBackground(673, 334, 111, 101, 3500);
            AddHtml(711, 350, 183, 22, 100.ToString(), (bool)false, (bool)false);
            AddHtml(678, 406, 100, 22, "Cristal Therathan", (bool)true, (bool)false);
            //AddItem(703, 374, custos.Item);
            NewAuctionGump.AddItemCentered(673, 334, 111, 101, 3823, 0, this);

            AddBackground(784, 335, 111, 101, 3500);
            AddHtml(827, 350, 83, 22, 50.ToString(), (bool)false, (bool)false);
            AddHtml(793, 405, 100, 22, "Essencias de " + colar.Elemento.ToString(), (bool)true, (bool)false);
            //AddItem(811, 367, 576);
            NewAuctionGump.AddItemCentered(784, 335, 111, 101, 0x571C, colar.Hue, this);

            AddButton(804, 435, 247, 248, (int)ElementoButtons.Upar, GumpButtonType.Reply, 0);
        }

        public enum ElementoButtons
        {
            Nads,
            Upar,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var from = sender.Mobile as PlayerMobile;

            switch (info.ButtonID)
            {
                case (int)ElementoPvM.Luz:
                    {
                        from.SendGump(new ElementosGump(from, ElementoPvM.Luz));
                        break;
                    }
                case (int)ElementoPvM.Raio:
                    {

                        from.SendGump(new ElementosGump(from, ElementoPvM.Raio));
                        break;
                    }
                case (int)ElementoPvM.Gelo:
                    {
                        from.SendGump(new ElementosGump(from, ElementoPvM.Gelo));
                        break;
                    }
                case (int)ElementoPvM.Agua:
                    {
                        from.SendGump(new ElementosGump(from, ElementoPvM.Agua));
                        break;
                    }
                case (int)ElementoPvM.Terra:
                    {
                        from.SendGump(new ElementosGump(from, ElementoPvM.Terra));
                        break;
                    }
                case (int)ElementoPvM.Vento:
                    {
                        from.SendGump(new ElementosGump(from, ElementoPvM.Vento));
                        break;
                    }
                case (int)ElementoPvM.Fogo:
                    {
                        from.SendGump(new ElementosGump(from, ElementoPvM.Fogo));
                        break;
                    }
                case (int)ElementoPvM.Escuridao:
                    {
                        from.SendGump(new ElementosGump(from, ElementoPvM.Escuridao));
                        break;
                    }
                case (int)1:
                    {
                        var nivel = from.Elementos.GetNivel(e);
                        var exp = from.Elementos.GetExp(e);
                        var expPrecisa = CustosUPElementos.CustoUpExp(nivel);
                        if (exp < expPrecisa)
                        {
                            from.SendMessage("Voce tem " + exp + " exp neste elemento. Para subir o nivel precisa de um total de " + expPrecisa + " exp");
                            return;
                        }
                        var itemPrecisa = CustosUPElementos.GetCustos(e)[0];
                        var qtdPrecisa = CustosUPElementos.QuantidadeItems(nivel);

                        if (!sender.Mobile.Backpack.HasItem(itemPrecisa.type, qtdPrecisa, true))
                        {
                            from.SendMessage("Voce precisa de " + qtdPrecisa + "x " + itemPrecisa.name + " para isto");
                            return;
                        }
                        else if (!Banker.Withdraw(sender.Mobile, qtdPrecisa * 1000))
                        {
                            from.SendMessage("Voce precisa de " + qtdPrecisa * 1000 + " moedas de ouro para isto");
                            return;
                        }
                        else
                        {
                            from.Backpack.ConsumeTotal(new System.Type[] { itemPrecisa.type }, new int[] { qtdPrecisa });
                        }


                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
                        Effects.PlaySound(from.Location, from.Map, 0x243);

                        Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                        Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                        Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                        Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

                        from.Elementos.SetExp(e, 0);
                        from.Elementos.SetNivel(e, (ushort)(nivel + 1));
                        from.InvalidateProperties();
                        from.SendMessage("Voce sente mais poder em seu corpo");
                        from.CloseAllGumps();
                        from.SendGump(new ElementosGump(from, e));
                        break;
                    }

            }
        }
    }
}
