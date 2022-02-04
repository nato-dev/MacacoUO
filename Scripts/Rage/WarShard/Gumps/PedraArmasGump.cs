using Server.Network;
using Server.Items;

namespace Server.Gumps
{
    public class PedraArmasGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraArmasGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
            this.AddBackground(66, 9, 305, 327, 3600);
            this.AddLabel(167, 27, 253, @"Pedra de Armas");
			this.AddLabel(82, 47, 1153, @"---------------------------------------------");
            this.AddLabel(190, 76, 1153, @"Lâminas");
			this.AddLabel(190, 106, 1153, @"Perfuração");
            this.AddLabel(190, 136, 1153, @"Contusão");
            this.AddLabel(190, 166, 1153, @"Machados");
            this.AddLabel(190, 196, 1153, @"Arcos"); 
            this.AddLabel(190, 196+30, 1153, @"Cajados");
            this.AddCheck(160, 77, 210, 211, false, 1);
			this.AddCheck(160, 107, 210, 211, false, 2);
            this.AddCheck(160, 137, 210, 211, false, 3);
            this.AddCheck(160, 167, 210, 211, false, 4);
            this.AddCheck(160, 197, 210, 211, false, 5);
            this.AddCheck(160, 197+30, 210, 211, false, 6);
            this.AddButton(185, 277, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 6; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                            {
                                sender.Mobile.SendGump(new Laminas(sender.Mobile));
                                criou = true;
                                break;
                            }
                            case 2:
                            {
                                sender.Mobile.SendGump(new Perfuracao(sender.Mobile));
                                criou = true;
                                break;
                            }
                            case 3:
                            {
                                sender.Mobile.SendGump(new Contusao(sender.Mobile));
                                criou = true;
                                break;
                            }
                            case 4:
                            {
                                sender.Mobile.SendGump(new Machados(sender.Mobile));
                                criou = true;
                                break;
                            }
                            case 5:
                            {
                                sender.Mobile.SendGump(new Arcos(sender.Mobile));
                                criou = true;
                                break;
                            }
                            case 6:
                            {
                                sender.Mobile.SendGump(new Cajados(sender.Mobile));
                                criou = true;
                                break;
                            }
                        }
                    }
                }

                if (!criou)
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

    public class Laminas : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public Laminas(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(66, 9, 305, 297, 3600);
            this.AddLabel(197, 27, 253, @"Lâminas");
            this.AddLabel(82, 47, 1153, @"---------------------------------------------");
            this.AddLabel(190, 76, 1153, @"Katana");
            this.AddLabel(190, 106, 1153, @"Alabarda");
            this.AddLabel(190, 136, 1153, @"Bardiche");
            this.AddLabel(190, 166, 1153, @"Espada Viking");
            this.AddLabel(190, 196, 1153, @"Espada Curta");
            this.AddCheck(160, 77, 210, 211, false, 1);
            this.AddCheck(160, 107, 210, 211, false, 2);
            this.AddCheck(160, 137, 210, 211, false, 3);
            this.AddCheck(160, 167, 210, 211, false, 4);
            this.AddCheck(160, 197, 210, 211, false, 5);
            this.AddButton(186, 247, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public static void StripWeapons(Mobile m)
        {
            var arma = m.FindItemOnLayer(Layer.OneHanded);
            if(arma != null)
            {
                m.RemoveItem(arma);
                m.Backpack.AddItem(arma);
            }
            arma = m.FindItemOnLayer(Layer.TwoHanded);
            if (arma != null)
            {
                m.RemoveItem(arma);
                m.Backpack.AddItem(arma);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 5; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Katana());
                                    criou = true;
                                    break;
                                }
                            case 2:
                                {
                                    StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Halberd());
                                    criou = true;
                                    break;
                                }
                            case 3:
                                {
                                    StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Bardiche());
                                    criou = true;
                                    break;
                                }
                            case 4:
                                {
                                    StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new VikingSword());
                                    criou = true;
                                    break;
                                }
                            case 5:
                                {
                                    StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Longsword());
                                    criou = true;
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

    public class Machados : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public Machados(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(66, 9, 305, 297, 3600);
            this.AddLabel(190, 27, 253, @"Machados");
            this.AddLabel(82, 47, 1153, @"---------------------------------------------");
            this.AddLabel(170, 76, 1153, @"Machado Duplo");
            this.AddLabel(170, 106, 1153, @"Machado do Carrasco");
            this.AddLabel(170, 136, 1153, @"Machado de Batalha");
            this.AddLabel(170, 166, 1153, @"Machado de Batalha Pesado");
            this.AddLabel(170, 196, 1153, @"Machado");
            this.AddCheck(140, 77, 210, 211, false, 1);
            this.AddCheck(140, 107, 210, 211, false, 2);
            this.AddCheck(140, 137, 210, 211, false, 3);
            this.AddCheck(140, 167, 210, 211, false, 4);
            this.AddCheck(140, 197, 210, 211, false, 5);
            this.AddButton(186, 247, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 5; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new DoubleAxe());
                                    criou = true;
                                    break;
                                }
                            case 2:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new ExecutionersAxe());
                                    criou = true;
                                    break;
                                }
                            case 3:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new BattleAxe());
                                    criou = true;
                                    break;
                                }
                            case 4:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new LargeBattleAxe());
                                    criou = true;
                                    break;
                                }
                            case 5:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Axe());
                                    criou = true;
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


    public class Perfuracao : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public Perfuracao(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(66, 9, 305, 297, 3600);
            this.AddLabel(190, 27, 253, @"Perfuração");
            this.AddLabel(82, 47, 1153, @"---------------------------------------------");
            this.AddLabel(190, 76, 1153, @"Kopesh");
            this.AddLabel(190, 106, 1153, @"Lança");
            this.AddLabel(190, 136, 1153, @"Lança Curta");
            this.AddLabel(190, 166, 1153, @"Garfo de Guerra");
            this.AddLabel(190, 196, 1153, @"Adaga");
            this.AddCheck(160, 77, 210, 211, false, 1);
            this.AddCheck(160, 107, 210, 211, false, 2);
            this.AddCheck(160, 137, 210, 211, false, 3);
            this.AddCheck(160, 167, 210, 211, false, 4);
            this.AddCheck(160, 197, 210, 211, false, 5);
            this.AddButton(186, 247, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 5; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Kryss());
                                    criou = true;
                                    break;
                                }
                            case 2:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Spear());
                                    criou = true;
                                    break;
                                }
                            case 3:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new ShortSpear());
                                    criou = true;
                                    break;
                                }
                            case 4:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new WarFork());
                                    criou = true;
                                    break;
                                }
                            case 5:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Dagger());
                                    criou = true;
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

    public class Contusao : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public Contusao(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(66, 9, 305, 297, 3600);
            this.AddLabel(195, 27, 253, @"Contusão");
            this.AddLabel(82, 47, 1153, @"---------------------------------------------");
            this.AddLabel(190, 76, 1153, @"Marreta");
            this.AddLabel(190, 106, 1153, @"Maça");
            this.AddLabel(190, 136, 1153, @"Maça Longa");
            this.AddLabel(190, 166, 1153, @"Maça de Guerra");
            this.AddLabel(190, 196, 1153, @"Martelo de Guerra");

            this.AddCheck(160, 77, 210, 211, false, 1);
            this.AddCheck(160, 107, 210, 211, false, 2);
            this.AddCheck(160, 137, 210, 211, false, 3);
            this.AddCheck(160, 167, 210, 211, false, 4);
            this.AddCheck(160, 197, 210, 211, false, 5);
            this.AddButton(186, 247, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 5; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new HammerPick());
                                    criou = true;
                                    break;
                                }
                            case 2:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Mace());
                                    criou = true;
                                    break;
                                }
                            case 3:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Maul());
                                    criou = true;
                                    break;
                                }
                            case 4:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new WarMace());
                                    criou = true;
                                    break;
                                }
                            case 5:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new WarHammer());
                                    criou = true;
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

    public class Cajados : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public Cajados(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(66, 9, 305, 297, 3600);
            this.AddLabel(197, 27, 253, @"Cajados");
            this.AddLabel(82, 47, 1153, @"---------------------------------------------");
            this.AddLabel(170, 76, 1153, @"Cajado Torcido");
            this.AddLabel(170, 106, 1153, @"Cajado Negro");
            this.AddLabel(170, 136, 1153, @"Cajado Liso");
            this.AddLabel(170, 136+30, 1153, @"Cajado Elfico (Em Breve)");
            //this.AddLabel(170, 136, 1153, @"Cajado da Serpente (Em Breve)");
            this.AddCheck(140, 77, 210, 211, false, 1);
            this.AddCheck(140, 107, 210, 211, false, 2);
            this.AddCheck(140, 137, 210, 211, false, 3);
            this.AddButton(186, 247, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 5; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new GnarledStaff());
                                    criou = true;
                                    break;
                                }
                            case 2:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new BlackStaff());
                                    criou = true;
                                    break;
                                }
                            case 3:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new QuarterStaff());
                                    criou = true;
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

    public class Arcos : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public Arcos(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(66, 9, 305, 237, 3600);
            this.AddLabel(197, 27, 253, @"Arcos");
            this.AddLabel(82, 47, 1153, @"---------------------------------------------");
            this.AddLabel(190, 76, 1153, @"Arco");
            this.AddLabel(190, 106, 1153, @"Besta");
            this.AddLabel(190, 136, 1153, @"Besta Pesada");
            this.AddCheck(160, 77, 210, 211, false, 1);
            this.AddCheck(160, 107, 210, 211, false, 2);
            this.AddCheck(160, 137, 210, 211, false, 3);
            this.AddButton(186, 187, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 5; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Bow());
                                    sender.Mobile.Backpack.AddItem(new Arrow(100));
                                    criou = true;
                                    break;
                                }
                            case 2:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new Crossbow());
                                    sender.Mobile.Backpack.AddItem(new CrossbowBolts(100));
                                    criou = true;
                                    break;
                                } 
                            case 3:
                                {
                                    Laminas.StripWeapons(sender.Mobile);
                                    sender.Mobile.EquipItem(new HeavyCrossbow());
                                    sender.Mobile.Backpack.AddItem(new CrossbowBolts(100));
                                    criou = true;
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
