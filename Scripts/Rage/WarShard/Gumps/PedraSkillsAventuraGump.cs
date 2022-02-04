using Server.Network;

namespace Server.Gumps
{
    public class PedraSkillsAventuraGump : Gump
    {
        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraSkillsAventuraGump(Mobile from)
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
   			this.AddLabel(189, 55, 4, @"Aventura");
			this.AddLabel(125, 90, 1153, @"Animal Lore");
			this.AddLabel(125, 110, 1153, @"Animal Taming");
			this.AddLabel(125, 130, 1153, @"Begging");
			this.AddLabel(125, 150, 1153, @"Cartography");
			this.AddLabel(125, 170, 1153, @"Discordance");
			this.AddLabel(125, 190, 1153, @"Lockpicking");
			this.AddLabel(125, 210, 1153, @"Musicianship");
			this.AddLabel(125, 230, 1153, @"Peacemaking");
			this.AddLabel(125, 250, 1153, @"Provocation");
			this.AddLabel(125, 270, 1153, @"Remove Trap");
			this.AddLabel(125, 290, 1153, @"Camping");
            this.AddLabel(125, 310, 1153, @"Tracking");
            this.AddLabel(125, 330, 1153, @"Veterinary");
			this.AddLabel(125, 350, 1153, @"Item Identification");
			this.AddTextEntry(285, 91, 50, 20, 1153, 2, @from.Skills[2].Base.ToString(), 3);
			this.AddTextEntry(285, 111, 50, 20, 1153, 35, @from.Skills[35].Base.ToString(), 3);
			this.AddTextEntry(285, 131, 50, 20, 1153, 6, @from.Skills[6].Base.ToString(), 3);
			this.AddTextEntry(285, 151, 50, 20, 1153, 12, @from.Skills[12].Base.ToString(), 3);
			this.AddTextEntry(285, 171, 50, 20, 1153, 15, @from.Skills[15].Base.ToString(), 3);
			this.AddTextEntry(285, 191, 50, 20, 1153, 24, @from.Skills[24].Base.ToString(), 3);
			this.AddTextEntry(285, 211, 50, 20, 1153, 29, @from.Skills[29].Base.ToString(), 3);
			this.AddTextEntry(285, 231, 50, 20, 1153, 9, @from.Skills[9].Base.ToString(), 3);
			this.AddTextEntry(285, 251, 50, 20, 1153, 22, @from.Skills[22].Base.ToString(), 3);
			this.AddTextEntry(285, 271, 50, 20, 1153, 48, @from.Skills[48].Base.ToString(), 3);
			this.AddTextEntry(285, 291, 50, 20, 1153, 10, @from.Skills[10].Base.ToString(), 3);
            this.AddTextEntry(285, 311, 50, 20, 1153, 38, @from.Skills[38].Base.ToString(), 3);
			this.AddTextEntry(285, 331, 50, 20, 1153, 39, @from.Skills[39].Base.ToString(), 3);
			this.AddTextEntry(285, 351, 50, 20, 1153, 3, @from.Skills[3].Base.ToString(), 3);
			this.AddButton(185, 400, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 10)
            {
				string[] skills = new string[14] {"2", "35", "6", "12", "15", "24", "29", "9", "22", "48", "10", "38", "39", "3"};
				int j = 0, skillCapAtual = 0, skillCapNew = 0;

				for (int i = 0; i < skills.Length; i++)
				{
					TextRelay skillVal = info.GetTextEntry(System.Int32.Parse(skills[i]));
					if (int.TryParse(skillVal.Text, out j))
					{
						if (System.Int32.Parse(skillVal.Text) > 100)
						{
							sender.Mobile.SendMessage(0x00FE, "Valor individual das skills não pode ultrapassar 100.");
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
					sender.Mobile.SendMessage(0x00FE, "SkillCap não pode ultrapassar 750.");
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
