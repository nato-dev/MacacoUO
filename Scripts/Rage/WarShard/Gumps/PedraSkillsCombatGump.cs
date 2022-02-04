using Server.Network;

namespace Server.Gumps
{
    public class PedraSkillsCombatGump : Gump
    {
		public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;
        public PedraSkillsCombatGump(Mobile from)
			: base(GumpOffsetX, GumpOffsetY)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			this.AddBackground(66, 9, 305, 770, 3600);
			this.AddLabel(197, 25, 253, @"Skills");
			this.AddLabel(82, 40, 1153, @"---------------------------------------------");
			this.AddLabel(191, 55, 36, @"Combate");
			this.AddLabel(110, 90, 1153, @"Anatomy");
			this.AddLabel(110, 110, 1153, @"Tactics");
			this.AddLabel(110, 130, 1153, @"Fencing");
			this.AddLabel(110, 150, 1153, @"Swordsmanship");
			this.AddLabel(110, 170, 1153, @"Lumberjacking");
			this.AddLabel(110, 190, 1153, @"Mace Fighting");
			this.AddLabel(110, 210, 1153, @"Wrestling");
			this.AddLabel(110, 230, 1153, @"Parrying");
			this.AddLabel(110, 250, 1153, @"Archery");
            this.AddLabel(110, 270, 1153, @"Hiding");
            this.AddLabel(110, 290, 1153, @"Detect Hidden");
            this.AddLabel(110, 310, 1153, @"Stealth");
			this.AddLabel(110, 330, 1153, @"Focus");
            this.AddLabel(110, 350, 1153, @"Bushido");
			this.AddLabel(110, 370, 1153, @"Chivalry");
			this.AddLabel(110, 390, 1153, @"Ninjitsu");
			this.AddLabel(110, 410, 1153, @"Mysticism");
			this.AddLabel(110, 430, 1153, @"Necromancer");
			this.AddLabel(82, 455, 1153, @"---------------------------------------------");
			this.AddLabel(195, 470, 4, @"Magia");
			this.AddLabel(110, 505, 1153, @"Magery");
			this.AddLabel(110, 525, 1153, @"Inscription");
			this.AddLabel(110, 545, 1153, @"Evaluating Intelligence");
			this.AddLabel(110, 565, 1153, @"Meditation");
			this.AddLabel(110, 585, 1153, @"Resisting Spells");
			this.AddLabel(110, 605, 1153, @"Spirit Speak");
			this.AddLabel(82, 630, 1153, @"---------------------------------------------");
			this.AddLabel(199, 645, 63, @"Cura");
			this.AddLabel(110, 680, 1153, @"Healing");
			this.AddTextEntry(270, 91, 50, 20, 1153, 1, @from.Skills[1].Base.ToString(), 3);
			this.AddTextEntry(270, 111, 50, 20, 1153, 27, @from.Skills[27].Base.ToString(), 3);
			this.AddTextEntry(270, 131, 50, 20, 1153, 42, @from.Skills[42].Base.ToString(), 3);
			this.AddTextEntry(270, 151, 50, 20, 1153, 40, @from.Skills[40].Base.ToString(), 3);
			this.AddTextEntry(270, 171, 50, 20, 1153, 44, @from.Skills[44].Base.ToString(), 3);
			this.AddTextEntry(270, 191, 50, 20, 1153, 41, @from.Skills[41].Base.ToString(), 3);
			this.AddTextEntry(270, 211, 50, 20, 1153, 43, @from.Skills[43].Base.ToString(), 3);
			this.AddTextEntry(270, 231, 50, 20, 1153, 5, @from.Skills[5].Base.ToString(), 3);
            this.AddTextEntry(270, 251, 50, 20, 1153, 31, @from.Skills[31].Base.ToString(), 3);
            this.AddTextEntry(270, 271, 50, 20, 1153, 21, @from.Skills[21].Base.ToString(), 3);
            this.AddTextEntry(270, 291, 50, 20, 1153, 14, @from.Skills[14].Base.ToString(), 3);
            this.AddTextEntry(270, 311, 50, 20, 1153, 47, @from.Skills[47].Base.ToString(), 3);
			this.AddTextEntry(270, 331, 50, 20, 1153, 50, @from.Skills[50].Base.ToString(), 3);
            //this.AddTextEntry(270, 351, 50, 20, 1153, 52, @from.Skills[52].Base.ToString(), 3);
            //this.AddTextEntry(270, 371, 50, 20, 1153, 51, @from.Skills[51].Base.ToString(), 3);
            this.AddTextEntry(270, 391, 50, 20, 1153, 53, @from.Skills[53].Base.ToString(), 3);
            //this.AddTextEntry(270, 411, 50, 20, 1153, 55, @from.Skills[55].Base.ToString(), 3);
            //this.AddTextEntry(270, 431, 50, 20, 1153, 49, @from.Skills[49].Base.ToString(), 3);
            this.AddLabel(270, 351, 1153, @"Em breve");
			this.AddLabel(270, 371, 1153, @"Em breve");
			// this.AddLabel(270, 391, 1153, @"Em breve");
			this.AddLabel(270, 411, 1153, @"Em breve");
			this.AddLabel(270, 431, 1153, @"Em breve");
            this.AddTextEntry(270, 506, 50, 20, 1153, 25, @from.Skills[25].Base.ToString(), 3);
			this.AddTextEntry(270, 526, 50, 20, 1153, 23, @from.Skills[23].Base.ToString(), 3);
			this.AddTextEntry(270, 546, 50, 20, 1153, 16, @from.Skills[16].Base.ToString(), 3);
			this.AddTextEntry(270, 566, 50, 20, 1153, 46, @from.Skills[46].Base.ToString(), 3);
			this.AddTextEntry(270, 586, 50, 20, 1153, 26, @from.Skills[26].Base.ToString(), 3);
			this.AddTextEntry(270, 606, 50, 20, 1153, 32, @from.Skills[32].Base.ToString(), 3);
			this.AddTextEntry(270, 681, 50, 20, 1153, 17, @from.Skills[17].Base.ToString(), 3);
			this.AddButton(185, 730, 247, 248, 10, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
			if (info.ButtonID == 10)
			{
				//string[] skills = new string[25] {"1", "27", "42", "40", "44", "41", "43", "5", "31", "21", "14", "47", "50", "52", "51", "53", "55", "49", "25", "23", "16", "46", "26", "32", "17"};
				string[] skills = new string[21] {"1", "27", "42", "40", "44", "41", "43", "5", "31", "21", "14", "47", "50", "25", "23", "16", "46", "26", "32", "17", "53"};
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
