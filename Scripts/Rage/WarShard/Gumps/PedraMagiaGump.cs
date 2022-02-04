using Server.Network;
using Server.Items;

namespace Server.Gumps
{
    public class PedraMagiaGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraMagiaGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
            this.AddBackground(66, 9, 305, 430, 3600);
            this.AddLabel(170, 25, 4, @"Pedra de Magia");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
            this.AddLabel(130, 70, 1153, @"Reagentes Mago");
			this.AddLabel(130, 100, 1153, @"Reagentes Alquimia");
            this.AddLabel(130, 130, 1153, @"Livro de Magias");
            this.AddLabel(130, 160, 1153, @"Runa");
            this.AddLabel(130, 190, 1153, @"Reagentes Mysticism (Em breve)");
            this.AddLabel(130, 220, 1153, @"Livro de Bushido (Em breve)");
            this.AddLabel(130, 250, 1153, @"Livro de Chivalry (Em breve)");
            this.AddLabel(130, 280, 1153, @"Livro de Ninjitsu (Em breve)");
            this.AddLabel(130, 310, 1153, @"Livro de Mysticism (Em breve)");
            this.AddLabel(130, 340, 1153, @"Livro de Necromancer (Em breve)");
            this.AddCheck(100, 71, 210, 211, false, 1);
			this.AddCheck(100, 101, 210, 211, false, 2);
            this.AddCheck(100, 131, 210, 211, false, 3);
            this.AddCheck(100, 161, 210, 211, false, 4);
            //this.AddCheck(100, 191, 210, 211, false, 5);
            //this.AddCheck(100, 221, 210, 211, false, 6);
            //this.AddCheck(100, 251, 210, 211, false, 7);
            //this.AddCheck(100, 281, 210, 211, false, 8);
            //this.AddCheck(100, 311, 210, 211, false, 9);
            //this.AddCheck(100, 341, 210, 211, false, 10);
            this.AddButton(185, 390, 247, 248, 20, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 20)
            {
                var criou = false;

                //for (int i = 1; i <= 10; i++)
                for (int i = 1; i <= 4; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                            {
                                var bagReags = new BagOfReagents(100);
                                bagReags.Hue = 33;
                                sender.Mobile.AddToBackpack(bagReags);
                                criou = true;
                                break;
                            }
                            case 2:
                            {
                                var bagNecro = new BagOfNecroReagents(100);
                                bagNecro.Hue = 70;
                                sender.Mobile.AddToBackpack(bagNecro);
                                criou = true;
                                break;
                            }
                            case 3:
                            {
                                var livroMago = new Spellbook();
                                if (livroMago.BookCount == 64)
                                {
                                    livroMago.Content = ulong.MaxValue;
                                }
                                else
                                {
                                    livroMago.Content = (1ul << livroMago.BookCount) - 1;
                                }
                                sender.Mobile.AddToBackpack(livroMago);
                                criou = true;
                                break;
                            }
                            case 4:
                            {
                                sender.Mobile.AddToBackpack(new RecallRune());
                                criou = true;
                                break;
                            }
                            case 5:
                            {
                                var bagMystic = new BagOfMysticismReagents(100);
                                bagMystic.Hue = 55;
                                sender.Mobile.AddToBackpack(bagMystic);
                                criou = true;
                                break;
                            }
                            case 6:
                            {
                                var livroBush = new BookOfBushido();
                                if (livroBush.BookCount == 64)
                                {
                                    livroBush.Content = ulong.MaxValue;
                                }
                                else
                                {
                                    livroBush.Content = (1ul << livroBush.BookCount) - 1;
                                }
                                sender.Mobile.AddToBackpack(livroBush);
                                criou = true;
                                break;
                            }
                            case 7:
                            {
                                var livroChiva = new BookOfChivalry();
                                if (livroChiva.BookCount == 64)
                                {
                                    livroChiva.Content = ulong.MaxValue;
                                }
                                else
                                {
                                    livroChiva.Content = (1ul << livroChiva.BookCount) - 1;
                                }
                                sender.Mobile.AddToBackpack(livroChiva);
                                criou = true;
                                break;
                            }
                            case 8:
                            {
                                var livroNinja = new BookOfNinjitsu();
                                if (livroNinja.BookCount == 64)
                                {
                                    livroNinja.Content = ulong.MaxValue;
                                }
                                else
                                {
                                    livroNinja.Content = (1ul << livroNinja.BookCount) - 1;
                                }
                                sender.Mobile.AddToBackpack(livroNinja);
                                criou = true;
                                break;
                            }
                            case 9:
                            {
                                var livroMyst = new MysticBook();
                                if (livroMyst.BookCount == 64)
                                {
                                    livroMyst.Content = ulong.MaxValue;
                                }
                                else
                                {
                                    livroMyst.Content = (1ul << livroMyst.BookCount) - 1;
                                }
                                sender.Mobile.AddToBackpack(livroMyst);
                                criou = true;
                                break;
                            }
                            case 10:
                            {
                                var livroNecro = new NecromancerSpellbook();
                                if (livroNecro.BookCount == 64)
                                {
                                    livroNecro.Content = ulong.MaxValue;
                                }
                                else
                                {
                                    livroNecro.Content = (1ul << livroNecro.BookCount) - 1;
                                }
                                sender.Mobile.AddToBackpack(livroNecro);
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
