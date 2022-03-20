#region Header
//   Vorspire    _,-'/-'/  PvPProfilesSortUI.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server;
using Server.Gumps;

using VitaNex.SuperGumps.UI;
#endregion

namespace VitaNex.Modules.AutoPvP
{
	public class PvPProfilesSortUI : MenuGump
	{
		public PvPProfilesUI ListGump { get; set; }

		public PvPProfilesSortUI(Mobile user, PvPProfilesUI listGump, Gump parent = null, GumpButton clicked = null)
			: base(user, parent, clicked: clicked)
		{
			ListGump = listGump;
		}

		protected override void CompileOptions(MenuGumpOptions list)
		{
			if (ListGump != null)
			{
				if (ListGump.SortOrder != PvPProfileRankOrder.None)
				{
					list.AppendEntry(
						"Sem Ordem",
						button =>
						{
							ListGump.SortOrder = PvPProfileRankOrder.None;
							ListGump.Refresh(true);
						});
				}

				if (ListGump.SortOrder != PvPProfileRankOrder.Kills)
				{
					list.AppendEntry(
						"Kills",
						button =>
						{
							ListGump.SortOrder = PvPProfileRankOrder.Kills;
							ListGump.Refresh(true);
						});
				}

				if (ListGump.SortOrder != PvPProfileRankOrder.Pontos)
				{
					list.AppendEntry(
						"Pontos",
						button =>
						{
							ListGump.SortOrder = PvPProfileRankOrder.Pontos;
							ListGump.Refresh(true);
						});
				}

				if (ListGump.SortOrder != PvPProfileRankOrder.Vitorias)
				{
					list.AppendEntry(
						"Vitorias",
						button =>
						{
							ListGump.SortOrder = PvPProfileRankOrder.Vitorias;
							ListGump.Refresh(true);
						});
				}
			}

			base.CompileOptions(list);
		}
	}
}
