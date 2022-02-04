using Server.Network;

namespace Server.Gumps
{
    public class PedraZerarSkillsGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraZerarSkillsGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
            this.AddPage(0);
            this.AddBackground(20, 32, 250, 120, 9200);
            this.AddLabel(47, 45, 1153, @"Tem certeza que deseja zerar");
            this.AddLabel(47, 65, 1153, @"todas as suas skills? Esta ação");
            this.AddLabel(47, 85, 1153, @"NÃO pode ser revertida.");
            this.AddButton(50, 120, 247, 248, 1, GumpButtonType.Reply, 0);
            this.AddButton(175, 120, 242, 242, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                for (int i = 0; i < 58; i++)
				{
                    if (sender.Mobile.Skills[i].Base > 0)
                    {
                        sender.Mobile.Skills[i].Base = 0;
                    }
				}
                sender.Mobile.SendMessage("Suas skills foram resetadas com sucesso!");
            }
            else
            {
                sender.Mobile.SendMessage("Cancelado.");
            }
        }
    }
}
