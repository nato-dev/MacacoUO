using Server.Network;

namespace Server.Gumps
{
    public class PedraSkillsWorkerGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraSkillsWorkerGump(Mobile from)
            : base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
            this.AddBackground(66, 9, 305, 440, 3600);
			this.AddLabel(197, 25, 253, @"Skills");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
			this.AddLabel(190, 55, 63, @"Trabalho");
			this.AddLabel(125, 90, 1153, @"Alchemy");
			this.AddLabel(125, 110, 1153, @"Arms Lore");
			this.AddLabel(125, 130, 1153, @"Blacksmithy");
			this.AddLabel(125, 150, 1153, @"Bowcraft");
			this.AddLabel(125, 170, 1153, @"Carpentry");
			this.AddLabel(125, 190, 1153, @"Cooking");
			this.AddLabel(125, 210, 1153, @"Fishing");
			this.AddLabel(125, 230, 1153, @"Forensics");
			this.AddLabel(125, 250, 1153, @"Herding");
			this.AddLabel(125, 270, 1153, @"Mining");
            this.AddLabel(125, 290, 1153, @"Poisoning");
            this.AddLabel(125, 310, 1153, @"Imbuing");
			this.AddLabel(125, 330, 1153, @"Tailoring");
            this.AddLabel(125, 350, 1153, @"Tinkering");
            this.AddLabel(125, 370, 1153, @"Lumber");
            this.AddTextEntry(285, 91, 50, 20, 1153, 0, @from.Skills[0].Base.ToString(), 3);
			this.AddTextEntry(285, 111, 50, 20, 1153, 4, @from.Skills[4].Base.ToString(), 3);
			this.AddTextEntry(285, 131, 50, 20, 1153, 7, @from.Skills[7].Base.ToString(), 3);
			this.AddTextEntry(285, 151, 50, 20, 1153, 8, @from.Skills[8].Base.ToString(), 3);
			this.AddTextEntry(285, 171, 50, 20, 1153, 11, @from.Skills[11].Base.ToString(), 3);
			this.AddTextEntry(285, 191, 50, 20, 1153, 13, @from.Skills[13].Base.ToString(), 3);
			this.AddTextEntry(285, 211, 50, 20, 1153, 18, @from.Skills[18].Base.ToString(), 3);
			this.AddTextEntry(285, 231, 50, 20, 1153, 19, @from.Skills[19].Base.ToString(), 3);
			this.AddTextEntry(285, 251, 50, 20, 1153, 20, @from.Skills[20].Base.ToString(), 3);
			this.AddTextEntry(285, 271, 50, 20, 1153, 45, @from.Skills[45].Base.ToString(), 3);
			this.AddTextEntry(285, 291, 50, 20, 1153, 30, @from.Skills[30].Base.ToString(), 3);
			this.AddTextEntry(285, 311, 50, 20, 1153, 56, @from.Skills[56].Base.ToString(), 3);
			this.AddTextEntry(285, 331, 50, 20, 1153, 34, @from.Skills[34].Base.ToString(), 3);
			this.AddTextEntry(285, 351, 50, 20, 1153, 37, @from.Skills[37].Base.ToString(), 3);
            this.AddTextEntry(285, 371, 50, 20, 1153, 44, @from.Skills[44].Base.ToString(), 3);
            this.AddButton(185, 400, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
                string[] skills = new string[15] {"0", "4", "7", "8", "11", "13", "18", "19", "20", "45", "30", "56", "34", "37", "44"};
				int j = 0, skillCapAtual = 0, skillCapNew = 0;

				for (int i = 0; i < skills.Length; i++)
				{
					TextRelay skillVal = info.GetTextEntry(System.Int32.Parse(skills[i]));
					if (int.TryParse(skillVal.Text, out j))
					{
						if (System.Int32.Parse(skillVal.Text) > 120)
						{
							sender.Mobile.SendMessage(0x00FE, "Valor individual das skills não pode ultrapassar 120.");
							return;
						}

						if (System.Int32.Parse(skillVal.Text) > sender.Mobile.Skills[System.Int32.Parse(skills[i])].Base)
						{
							skillCapNew += System.Int32.Parse(skillVal.Text);
						}
					}
					else if (skillVal.Text != "")
					{
						sender.Mobile.SendMessage(0x00FE, "Os valores informados são inválidos.");
						return;
					}
				}

				for (int i = 0; i < 58; i++)
				{
					skillCapAtual += System.Convert.ToInt32(sender.Mobile.Skills[i].Base);
				}

				if ((skillCapAtual + skillCapNew) > 750)
				{
					sender.Mobile.SendMessage(0x00FE, "SkillCap não pode ultrapassar 750. Resete suas skills primeiro na pedra de reset.");
					return;
				}

				for (int i = 0; i < skills.Length; i++)
				{
					TextRelay skillVal = info.GetTextEntry(System.Int32.Parse(skills[i]));
					if (skillVal.Text != "")
					{
						if (System.Int32.Parse(skillVal.Text) != sender.Mobile.Skills[System.Int32.Parse(skills[i])].Base)
						{
							sender.Mobile.Skills[System.Int32.Parse(skills[i])].Base = System.Int32.Parse(skillVal.Text);
						}
					}
				}
				sender.Mobile.SendMessage(0x00FE, "Skills atualizadas com sucesso.");
			}
			else
			{
				sender.Mobile.SendMessage(0x00FE, "Cancelado.");
			}
        }
    }
}
