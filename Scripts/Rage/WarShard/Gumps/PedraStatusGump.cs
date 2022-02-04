using Server.Network;

namespace Server.Gumps
{
	public class PedraStatusGump : Gump
	{     
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
		public PedraStatusGump(Mobile from)
			: base(GumpOffsetX, GumpOffsetY)
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);			
            this.AddBackground(66, 9, 305, 220, 3600);
            this.AddLabel(200, 25, 253, @"Status");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
			this.AddLabel(145, 70, 1153, @"Força:");
            this.AddLabel(145, 100, 1153, @"Destreza:");
			this.AddLabel(145, 130, 1153, @"Inteligência:");
            this.AddTextEntry(255, 71, 50, 20, 1153, 20, @from.Str.ToString(), 3);
			this.AddTextEntry(255, 101, 50, 20, 1153, 30, @from.Dex.ToString(), 3);
			this.AddTextEntry(255, 131, 50, 20, 1153, 40, @from.Int.ToString(), 3);
            this.AddButton(185, 180, 247, 248, 10, GumpButtonType.Reply, 0);
		}
        
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                TextRelay strVal = info.GetTextEntry(20);
                TextRelay dexVal = info.GetTextEntry(30);
                TextRelay intVal = info.GetTextEntry(40);
                int j = 0;

                if (int.TryParse(strVal.Text, out j) && int.TryParse(dexVal.Text, out j) && int.TryParse(intVal.Text, out j))
                {
                    if (System.Int32.Parse(strVal.Text) + System.Int32.Parse(dexVal.Text) + System.Int32.Parse(intVal.Text) > 225)
                    {
                        sender.Mobile.SendMessage(0x00FE, "StatCap não pode ultrapassar 225.");
                    }
                    else if (System.Int32.Parse(strVal.Text) > 100 || System.Int32.Parse(dexVal.Text) > 100 || System.Int32.Parse(intVal.Text) > 100)
                    {
                        sender.Mobile.SendMessage(0x00FE, "Status individual não pode ultrapassar 100.");
                    }
                    else
                    {
                        sender.Mobile.Str = System.Int32.Parse(strVal.Text);
                        sender.Mobile.Dex = System.Int32.Parse(dexVal.Text);
                        sender.Mobile.Int = System.Int32.Parse(intVal.Text);
                        sender.Mobile.SendMessage(0x00FE, "Status atualizados com sucesso.");
                    }
                }
                else
                {
                    sender.Mobile.SendMessage(0x00FE, "Os valores informados são inválidos.");
                }
            }
            else
            {
                sender.Mobile.SendMessage(0x00FE, "Cancelado.");
            }
        }
	}
}
