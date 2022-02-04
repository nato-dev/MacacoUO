using Server.Network;
using Server.Items;

namespace Server.Gumps
{
    public class PedraArmaduraGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraArmaduraGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(66, 9, 305, 250, 3600);
            this.AddLabel(165, 25, 63, @"Pedra de Armaduras");
            this.AddLabel(82, 40, 1153, @"---------------------------------------------");
            this.AddLabel(210, 70, 1153, @"Couro");
            this.AddLabel(210, 100, 1153, @"Loriga");
            this.AddLabel(210, 130, 1153, @"Malha");
            this.AddLabel(210, 160, 1153, @"Metal");
            this.AddCheck(180, 71, 210, 211, false, 1);
            this.AddCheck(180, 101, 210, 211, false, 2);
            this.AddCheck(180, 131, 210, 211, false, 3);
            this.AddCheck(180, 161, 210, 211, false, 4);
            this.AddButton(185, 210, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        private void StripArmor(Mobile m)
        {
            var armor = m.FindItemOnLayer(Layer.Arms);
            if(armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.Gloves);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.InnerTorso);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.InnerTorso);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.InnerLegs);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.Pants);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.OuterLegs);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.Helm);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
            armor = m.FindItemOnLayer(Layer.Neck);
            if (armor != null)
            {
                m.RemoveItem(armor);
                m.Backpack.AddItem(armor);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                var criou = false;

                for (int i = 1; i <= 4; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        switch (i)
                        {
                            case 1:
                                {
                                    StripArmor(sender.Mobile);
                                    sender.Mobile.EquipItem(new LeatherChest());
                                    sender.Mobile.EquipItem(new LeatherGloves());
                                    sender.Mobile.EquipItem(new LeatherCap());
                                    sender.Mobile.EquipItem(new LeatherLegs());
                                    sender.Mobile.EquipItem(new LeatherArms());
                                    sender.Mobile.EquipItem(new LeatherGorget());
                                    criou = true;
                                    break;
                                }
                            case 2:
                                {
                                    StripArmor(sender.Mobile);
                                    sender.Mobile.EquipItem(new RingmailCoif());
                                    sender.Mobile.EquipItem(new RingmailChest());
                                    sender.Mobile.EquipItem(new RingmailGloves());
                                    sender.Mobile.EquipItem(new RingmailArms());
                                    sender.Mobile.EquipItem(new RingmailLegs());
                                    criou = true;
                                    break;
                                }
                            case 3:
                                {
                                    StripArmor(sender.Mobile);
                                    sender.Mobile.EquipItem(new ChainCoif());
                                    sender.Mobile.EquipItem(new ChainChest());
                                    sender.Mobile.EquipItem(new ChainGloves());
                                    sender.Mobile.EquipItem(new ChainLegs());
                                    criou = true;
                                    break;
                                }
                            case 4:
                                {
                                    StripArmor(sender.Mobile);
                                    sender.Mobile.EquipItem(new PlateHelm());
                                    sender.Mobile.EquipItem(new PlateChest());
                                    sender.Mobile.EquipItem(new PlateGloves());
                                    sender.Mobile.EquipItem(new PlateArms());
                                    sender.Mobile.EquipItem(new PlateLegs());
                                    sender.Mobile.EquipItem(new PlateGorget());
                                    criou = true;
                                    break;
                                }
                        }
                    }
                }

                if (criou)
                {
                    sender.Mobile.SendMessage(0x00FE, "Os itens selecionados foram equipados.");
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
