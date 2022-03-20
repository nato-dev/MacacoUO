#region Header
//   Vorspire    _,-'/-'/  PvPProfilesUI.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

using Server;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

using VitaNex.SuperGumps;
using VitaNex.SuperGumps.UI;
#endregion

namespace VitaNex.Modules.AutoPvP
{
	public class PvPProfilesUI : ListGump<PvPProfile>
	{
		public PvPSeason Season { get; set; }

		public PvPProfileRankOrder SortOrder { get; set; }

		public bool UseConfirmDialog { get; set; }

		public PvPProfilesUI(
			Mobile user,
			PvPSeason season,
			Gump parent = null,
			bool useConfirm = true,
			PvPProfileRankOrder sortOrder = PvPProfileRankOrder.None)
			: base(user, parent, emptyText: "There are no profiles to display.", title: "Perfil Arena PvP")
		{
			Season = season ?? AutoPvP.CurrentSeason;

			SortOrder = sortOrder != PvPProfileRankOrder.None ? sortOrder : AutoPvP.CMOptions.Advanced.Profiles.RankingOrder;

			UseConfirmDialog = useConfirm;

			CanSearch = User.AccessLevel > AccessLevel.VIP || AutoPvP.CMOptions.Advanced.Profiles.AllowPlayerSearch;

			ForceRecompile = true;
		}

		protected override void Compile()
		{
			if (AutoPvP.SeasonSchedule.Enabled)
			{
				Title = "Perfis PvP (Temporada " + (Season != null ? Season.Number : AutoPvP.CurrentSeason.Number) + ")";
			}

			base.Compile();
		}

		protected override void CompileMenuOptions(MenuGumpOptions list)
		{
			list.Clear();

			if (User.AccessLevel >= AutoPvP.Access)
			{
				list.AppendEntry(
					new ListGumpEntry(
						"Delete All",
						button => new ConfirmDialogGump(User, this)
						{
							Title = "Delete All Profiles?",
							Html = "All profiles in the database will be deleted, erasing all data associated with them.\n" +
								   "This action can not be reversed.\n\nDo you want to continue?",
							AcceptHandler = subButton =>
							{
								var profiles = new List<PvPProfile>(AutoPvP.Profiles.Values);

								foreach (var p in profiles.Where(p => p != null && !p.Deleted))
								{
									p.Delete();
								}

								Refresh(true);
							},
							CancelHandler = Refresh
						}.Send(),
						HighlightHue));
			}

			list.AppendEntry(new ListGumpEntry("My Profile", OnMyProfile));

			list.AppendEntry(
				new ListGumpEntry("Ordenar Por (" + SortOrder + ")", b => new PvPProfilesSortUI(User, this, this, b).Send()));

			if (Season != null)
			{
				list.AppendEntry(
					new ListGumpEntry(
						"Ranks",
						b =>
						{
							Season = null;
							Refresh(true);
						}));
			}

			var season = AutoPvP.CurrentSeason;

			if (Season != season)
			{
				list.AppendEntry(
					new ListGumpEntry(
						String.Format("Ranking Temporada {0:#,0}", season.Number),
						b =>
						{
							Season = season;
							Refresh(true);
						}));
			}

			if (season.Number > 1)
			{
				list.AppendEntry(
					new ListGumpEntry(
						"Selecionar Temporada",
						b => new InputDialogGump(User, this)
						{
							Title = "Selecione a temporada",
							Html = "Escolha entre temporada 1 ate " + season.Number,
							InputText = Season == null ? "" : Season.Number.ToString(CultureInfo.InvariantCulture),
							Callback = (ib, text) =>
							{
								int num;

								if (Int32.TryParse(text, out num))
								{
									PvPSeason s;

									if (!AutoPvP.Seasons.TryGetValue(num, out s) || s == null)
									{
										User.SendMessage(ErrorHue, "Temporada invalida.");
									}
									else
									{
										Season = s;
									}
								}

								Refresh(true);
							},
							CancelHandler = Refresh
						}.Send()));
			}

			base.CompileMenuOptions(list);
		}

		protected virtual void OnMyProfile(GumpButton button)
		{
			new PvPProfileUI(User, AutoPvP.EnsureProfile(User as PlayerMobile), Hide(true)).Send();
		}

		protected override void CompileList(List<PvPProfile> list)
		{
			list.Clear();
			list.AddRange(AutoPvP.GetSortedProfiles(SortOrder, Season));

			base.CompileList(list);
		}

		public override string GetSearchKeyFor(PvPProfile key)
		{
			if (key != null && !key.Deleted && key.Owner != null)
			{
				return key.Owner.RawName;
			}

			return base.GetSearchKeyFor(key);
		}

		protected override void CompileEntryLayout(SuperGumpLayout layout, Dictionary<int, PvPProfile> range)
		{
			var sup = SupportsUltimaStore;
			var bgID = sup ? 40000 : 3500;
			var fillID = sup ? 40004 : 3004;

			var yOff = range.Count * 30;

			layout.Add(
				"background/footer/help",
				() =>
				{
					AddBackground(0, 75 + yOff, Width + 20, 130, bgID);
					AddImageTiled(10, 85 + yOff, Width, 110, fillID);
				});

			layout.Add("html/footer/help", () => AddHtml(20, 85 + yOff, Width - 10, 110, GetHelpText(), false, false));

			base.CompileEntryLayout(layout, range);
		}

		protected override void CompileEntryLayout(
			SuperGumpLayout layout,
			int length,
			int index,
			int pIndex,
			int yOffset,
			PvPProfile entry)
		{
			base.CompileEntryLayout(layout, length, index, pIndex, yOffset, entry);

			var wOffset = (Width - 10) / Columns;
			var xOffset = (pIndex % Columns) * wOffset;

			layout.Replace(
				"label/list/entry/" + index,
				() =>
				{
					var ww = (wOffset - 40) / 2;

					var hue = GetLabelHue(index, pIndex, entry);
					var text = GetLabelText(index, pIndex, entry);

					AddLabelCropped(60 + xOffset, 2 + yOffset, ww, 20, hue, text);

					hue = GetSortLabelHue(index, pIndex, entry);
					text = GetSortLabelText(index, pIndex, entry);

					AddLabelCropped(60 + xOffset + ww, 2 + yOffset, ww, 20, hue, text);
				});
		}

		protected virtual string GetHelpText()
		{
			return String.Format(
							 "Seu perfil PvP eh salvo toda temporada PvP.\n" +
							 "Ele sera classificado conforme seus pontos, vitorias e derrotas.\n" +
							 "Acha que tem o que precisa pra ser top 10 ?",
							 ServerList.ServerName)
						 .WrapUOHtmlColor(Color.Black, false);
		}

		protected override int GetLabelHue(int index, int pageIndex, PvPProfile entry)
		{
			if (index < 3)
			{
				return HighlightHue;
			}

			if (entry != null)
			{
				return Notoriety.GetHue(Notoriety.Compute(User, entry.Owner));
			}

			return base.GetLabelHue(index, pageIndex, null);
		}

		protected override string GetLabelText(int index, int pageIndex, PvPProfile entry)
		{
			if (entry != null && entry.Owner != null)
			{
				return String.Format("{0}: {1}", index + 1, entry.Owner.RawName);
			}

			return base.GetLabelText(index, pageIndex, entry);
		}

		protected virtual string GetSortLabelText(int index, int pageIndex, PvPProfile entry)
		{
			if (entry != null)
			{
				var key = "Rank";
				long val = index;

				if (SortOrder != PvPProfileRankOrder.None)
				{
					key = SortOrder.ToString().SpaceWords().ToUpperWords();
					val = AutoPvP.GetSortedValue(SortOrder, entry, Season);
				}

				return String.Format("{0}: {1:#,0}", key, val);
			}

			return String.Empty;
		}

		protected virtual int GetSortLabelHue(int index, int pageIndex, PvPProfile entry)
		{
			if (entry != null)
			{
				if (SortOrder == PvPProfileRankOrder.None)
				{
					return index <= AutoPvP.CMOptions.Advanced.Seasons.TopListCount ? HighlightHue : TextHue;
				}

				return AutoPvP.GetSortedValue(SortOrder, entry, Season) <= 0 ? ErrorHue : TextHue;
			}

			return ErrorHue;
		}

		protected override void SelectEntry(GumpButton button, PvPProfile entry)
		{
			base.SelectEntry(button, entry);

			if (button != null && entry != null && !entry.Deleted)
			{
				new PvPProfileUI(User, entry, Hide(true), UseConfirmDialog).Send();
			}
		}
	}
}
