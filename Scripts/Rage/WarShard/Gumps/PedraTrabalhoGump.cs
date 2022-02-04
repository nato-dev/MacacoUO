using Server.Network;
using Server.Items;

namespace Server.Gumps
{
    public class PedraTrabalhoGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraTrabalhoGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
            this.AddBackground(66, 9, 305, 670, 3600);
            this.AddLabel(165, 25, 63, @"Pedra de Trabalho");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
            this.AddLabel(145, 70, 1153, @"Lingotes de Ferro");
            this.AddLabel(145, 100, 1153, @"Lingotes Tier 3");
            this.AddLabel(145, 130, 1153, @"Lingotes Tier 2");
            this.AddLabel(145, 160, 1153, @"Lingotes Tier 1");
            this.AddLabel(145, 190, 1153, @"Madeiras Comuns");
            this.AddLabel(145, 220, 1153, @"Madeiras Tier 3");
            this.AddLabel(145, 250, 1153, @"Madeiras Tier 2");
            this.AddLabel(145, 280, 1153, @"Madeiras Tier 1");
            this.AddLabel(145, 310, 1153, @"Couro Comum");
            this.AddLabel(145, 340, 1153, @"Couro Tier 3");
            this.AddLabel(145, 370, 1153, @"Couro Tier 2");
            this.AddLabel(145, 400, 1153, @"Couro Tier 1");
            this.AddLabel(145, 430, 1153, @"Escamas Tier 3");
            this.AddLabel(145, 460, 1153, @"Escamas Tier 2");
            this.AddLabel(145, 490, 1153, @"Escamas Tier 1");
			this.AddLabel(145, 520, 1153, @"Tecido");
            this.AddLabel(145, 550, 1153, @"Garrafas Vazias");
            this.AddLabel(145, 580, 1153, @"Ferramentas");            
            this.AddCheck(115, 71, 210, 211, false, 1);
			this.AddCheck(115, 101, 210, 211, false, 2);
            this.AddCheck(115, 131, 210, 211, false, 3);
            this.AddCheck(115, 161, 210, 211, false, 4);
            this.AddCheck(115, 191, 210, 211, false, 5);
            this.AddCheck(115, 221, 210, 211, false, 6);
            this.AddCheck(115, 251, 210, 211, false, 7);
            this.AddCheck(115, 281, 210, 211, false, 8);
            this.AddCheck(115, 311, 210, 211, false, 9);
            this.AddCheck(115, 341, 210, 211, false, 10);
            this.AddCheck(115, 371, 210, 211, false, 11);
            this.AddCheck(115, 401, 210, 211, false, 12);
            this.AddCheck(115, 431, 210, 211, false, 13);
            this.AddCheck(115, 461, 210, 211, false, 14);
            this.AddCheck(115, 491, 210, 211, false, 15);
            this.AddCheck(115, 521, 210, 211, false, 16);
            this.AddCheck(115, 551, 210, 211, false, 17);
            this.AddCheck(115, 581, 210, 211, false, 18);
            this.AddButton(185, 630, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 18; i++)
                {
                    if (i == 3 || i == 4 || i == 7 || i == 8) continue;    //ajustar conforme liberar
                    if (i >= 11 && i <= 15) continue;                      //ajustar conforme liberar

                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                            {
                                sender.Mobile.AddToBackpack(new IronIngot(100));
                                criou = true;
                                break;
                            }
                            case 2:
                            {
                                sender.Mobile.AddToBackpack(new CopperIngot(100));
                                sender.Mobile.AddToBackpack(new BronzeIngot(100));
                                sender.Mobile.AddToBackpack(new SilverIngot(100));
                                criou = true;
                                break;
                            }
                            case 3:
                            {
                                sender.Mobile.AddToBackpack(new NiobioIngot(100));
                                sender.Mobile.AddToBackpack(new LazuritaIngot(100));
                                sender.Mobile.AddToBackpack(new QuartzoIngot(100));
                                criou = true;
                                break;
                            }
                            case 4:
                            {
                                sender.Mobile.AddToBackpack(new BeriloIngot(100));
                                sender.Mobile.AddToBackpack(new VibraniumIngot(100));
                                sender.Mobile.AddToBackpack(new AdamantiumIngot(100));
                                criou = true;
                                break;
                            }
                            case 5:
                            {
                                
                                sender.Mobile.AddToBackpack(new Board(100));
                                criou = true;
                                break;
                            }
                            case 6:
                            {
                                
                                sender.Mobile.AddToBackpack(new OakBoard(100));
                                sender.Mobile.AddToBackpack(new AshBoard(100));
                                criou = true;
                                break;
                            }
                            case 7:
                            {
                                
                                sender.Mobile.AddToBackpack(new YewBoard(100));
                                sender.Mobile.AddToBackpack(new HeartwoodBoard(100));
                                criou = true;
                                break;
                            }
                            case 8:
                            {
                                
                                sender.Mobile.AddToBackpack(new BloodwoodBoard(100));
                                sender.Mobile.AddToBackpack(new FrostwoodBoard(100));
                                criou = true;
                                break;
                            }
                            case 9:
                            {
                                sender.Mobile.AddToBackpack(new Leather(100));
                                criou = true;
                                break;
                            }
                            case 10:
                            {
                                sender.Mobile.AddToBackpack(new SpinedLeather(100));
                                criou = true;
                                break;
                            }
                            case 11:
                            {
                                sender.Mobile.AddToBackpack(new HornedLeather(100));
                                criou = true;
                                break;
                            }
                            case 12:
                            {
                                sender.Mobile.AddToBackpack(new BarbedLeather(100));
                                criou = true;
                                break;
                            }
                            case 13:
                            {
                                sender.Mobile.AddToBackpack(new YellowScales(100));
                                sender.Mobile.AddToBackpack(new BlueScales(100));
                                criou = true;
                                break;
                            }
                            case 14:
                            {
                                sender.Mobile.AddToBackpack(new GreenScales(100));
                                sender.Mobile.AddToBackpack(new RedScales(100));
                                criou = true;
                                break;
                            }
                            case 15:
                            {
                                sender.Mobile.AddToBackpack(new WhiteScales(100));
                                sender.Mobile.AddToBackpack(new BlackScales(100));
                                criou = true;
                                break;
                            }
                            case 16:
                            {
                                sender.Mobile.AddToBackpack(new UncutCloth(100));
                                criou = true;
                                break;
                            }
                            case 17:
                            {
                                sender.Mobile.AddToBackpack(new Bottle(50));
                                criou = true;
                                break;
                            }
                            case 18:
                            {
                                sender.Mobile.AddToBackpack(new KitFerramentas(50));
                                criou = true;
                                break;
                            }
                        }
                    }
                }

                if (criou)
                {
                    sender.Mobile.SendMessage(0x00FE, "Os itens selecionados foram criados na sua mochila.");
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
