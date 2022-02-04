using Server.Network;
using Server.Items;
using Server.Spells.First;

namespace Server.Gumps
{
    public class PedraDiversosGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraDiversosGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
            this.AddBackground(66, 9, 305, 370, 3600);
            this.AddLabel(150, 25, 253, @"Pedra de Itens Diversos");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
            this.AddLabel(180, 70, 1153, @"Flechas");
			this.AddLabel(180, 100, 1153, @"Dardos");
            this.AddLabel(180, 130, 1153, @"Bandagens");
            this.AddLabel(180, 160, 1153, @"Comida");
            this.AddLabel(180, 190, 1153, @"Poções");
            this.AddLabel(180, 220, 1153, @"Instrumentos Musicais");
            this.AddLabel(180, 250, 1153, @"Carnes");
            this.AddLabel(180, 280, 1153, @"Vegetais");
            this.AddCheck(150, 71, 210, 211, false, 1);
			this.AddCheck(150, 101, 210, 211, false, 2);
            this.AddCheck(150, 131, 210, 211, false, 3);
            this.AddCheck(150, 161, 210, 211, false, 4);
            this.AddCheck(150, 191, 210, 211, false, 5);
            this.AddCheck(150, 221, 210, 211, false, 6);
            this.AddCheck(150, 251, 210, 211, false, 7);
            this.AddCheck(150, 281, 210, 211, false, 8);
            this.AddButton(185, 330, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 8; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                            {
                                sender.Mobile.AddToBackpack(new Arrow(100));
                                criou = true;
                                break;
                            }
                            case 2:
                            {
                                sender.Mobile.AddToBackpack(new Bolt(100));
                                criou = true;
                                break;
                            }
                            case 3:
                            {
                                sender.Mobile.AddToBackpack(new Bandage(100));
                                criou = true;
                                break;
                            }
                            case 4:
                            {
                                sender.Mobile.AddToBackpack(new EarOfCorn(50));
                                criou = true;
                                break;
                            }
                            case 5:
                            {
                                for (int j = 1; j <= 10; j++)
                                {
                                    sender.Mobile.AddToBackpack(new GreaterHealPotion());
									sender.Mobile.AddToBackpack(new ManaPotion());
                                    if(Shard.POL_STYLE)
                                        sender.Mobile.AddToBackpack(new GreaterManaPotion());
                                    sender.Mobile.AddToBackpack(new TotalRefreshPotion());
                                    sender.Mobile.AddToBackpack(new GreaterAgilityPotion());
                                    sender.Mobile.AddToBackpack(new InvisibilityPotion());
                                    sender.Mobile.AddToBackpack(new GreaterCurePotion());
                                    sender.Mobile.AddToBackpack(new GreaterStrengthPotion());
                                    sender.Mobile.AddToBackpack(new GreaterIntelligencePotion());
                                    sender.Mobile.AddToBackpack(new GreaterExplosionPotion());
                                }
                                criou = true;
                                break;
                            }
                            case 6:
                            {
                                BaseInstrument instrumento = null;
                                switch (Utility.Random(8))
                                {
                                    case 1: instrumento = new AudChar(); break;
                                    case 2: instrumento = new BambooFlute(); break;
                                    case 3: instrumento = new Drums(); break;
                                    case 4: instrumento = new Harp(); break;
                                    case 5: instrumento = new LapHarp(); break;
                                    case 6: instrumento = new Lute(); break;
                                    case 7: instrumento = new Tambourine(); break;
                                    case 8: instrumento = new TambourineTassel(); break;
                                }
                                if (instrumento != null)
                                {
                                    sender.Mobile.AddToBackpack(instrumento);
                                    criou = true;
                                }
                                break;
                            }
                            case 7:
                            {
                                Item carne = null;
                                switch (Utility.Random(8))
                                {
                                    case 1: carne = new SlabOfBacon(10); break;
                                    case 2: carne = new CookedBird(10); break;
                                    case 3: carne = new RoastPig(10); break;
                                    case 4: carne = new Sausage(10); break;
                                    case 5: carne = new Ham(10); break;
                                    case 6: carne = new Ribs(10); break;
                                    case 7: carne = new LambLeg(10); break;
                                    case 8: carne = new ChickenLeg(10); break;
                                }
                                if (carne != null)
                                {
                                    sender.Mobile.AddToBackpack(carne);
                                    criou = true;
                                }
                                break;
                            }
                            case 8:
                            {
                                Item fruta = null;
                                switch (Utility.Random(11))
                                {
                                    case 1: fruta = new Banana(10); break;
                                    case 2: fruta = new Lemon(10); break;
                                    case 3: fruta = new Lime(10); break;
                                    case 4: fruta = new Dates(10); break;
                                    case 5: fruta = new Grapes(10); break;
                                    case 6: fruta = new Peach(10); break;
                                    case 7: fruta = new Pear(10); break;
                                    case 8: fruta = new Apple(10); break;
                                    case 9: fruta = new Watermelon(10); break;
                                    case 10: fruta = new Squash(10); break;
                                    case 11: fruta = new Plum(10); break;
                                }
                                if (fruta != null)
                                {
                                    sender.Mobile.AddToBackpack(fruta);
                                    criou = true;
                                }
                                break;
                            }
                        }
                    }
                }

                if (criou)
                {
                    sender.Mobile.SendMessage(0x00FE, "Os itens selecionados foram criados.");
                }
                else
                {
                    sender.Mobile.SendMessage(0x00FE, "Você não selecionou nenhum item.");
                }
            }
            else
            {
                sender.Mobile.SendMessage(0x00FE, "Cancelado.");
            }
        }
    }
}
