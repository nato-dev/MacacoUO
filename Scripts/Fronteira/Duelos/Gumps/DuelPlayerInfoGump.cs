using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Gumps;

namespace Server.Dueling
{
	public class DuelPlayerInfoGump : Gump
	{
		public DuelPlayerInfoGump( DuelPoints duelPoints )
			: base( 200, 00 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(36, 25, 402, 347, 3500);
            this.AddButton(401, 46, 3, 4, (int)Buttons.closeBtn, GumpButtonType.Reply, 0);
            this.AddLabel( 105, 43, 36, @"Onsite Duel System Player Duel Information" );
            this.AddLabel( 105, 42, 36, @"Onsite Duel System Player Duel Information" );
            this.AddLabel( 168, 91, 36, @"Wins:" );
            this.AddLabel( 168, 90, 36, @"Wins:" );
            this.AddLabel( 122, 69, 36, @"Player Name: " + duelPoints.Mobile.Name );
            this.AddLabel( 122, 68, 36, @"Player Name: " + duelPoints.Mobile.Name );
            this.AddLabel( 163, 189, 36, @"Loses:" );
            this.AddLabel( 163, 188, 36, @"Loses:" );
            this.AddLabel( 81, 289, 36, @"Quickest Duel Time:" );
            this.AddLabel( 81, 288, 36, @"Quickest Duel Time:" );
            this.AddLabel( 87, 383, 36, @"Longest Duel Time:" );
            this.AddLabel( 87, 382, 36, @"Longest Duel Time:" );
            this.AddHtml(206, 91, 215, 93, GetWins( duelPoints ), (bool)true, (bool)true);
            this.AddHtml(206, 189, 215, 93, GetLoses( duelPoints ), (bool)true, (bool)true);
            this.AddHtml(206, 287, 215, 93, GetQuickestTimes(duelPoints), (bool)true, (bool)true);
            this.AddHtml(206, 385, 215, 93, GetLongestTimes(duelPoints), (bool)true, (bool)true);
        }

        public static string GetLongestTimes(DuelPoints duelPoints)
        {
            if (duelPoints == null || (duelPoints.LongestLoses.Count == 0 && duelPoints.LongestWins.Count == 0))
                return "No records";

            StringBuilder sb = new StringBuilder();

            if (duelPoints.LongestWins.Count > 0)
            {
                sb.Append("Longest Wins" + br);
                IDictionaryEnumerator myEnum = duelPoints.LongestWins.GetEnumerator();

                while (myEnum.MoveNext())
                {
                    int players = (int)myEnum.Key;
                    DuelInfo dInfo = (DuelInfo)myEnum.Value;

                    sb.Append(String.Format("{0}vs{0} - {1} minutes {2} seconds{3}", players, dInfo.DuelTime.Minutes, dInfo.DuelTime.Seconds, br));
                }
            }

            if (duelPoints.LongestLoses.Count > 0)
            {
                sb.Append(br);
                sb.Append("Longest Loses" + br);
                IDictionaryEnumerator myEnum = duelPoints.LongestLoses.GetEnumerator();

                while (myEnum.MoveNext())
                {
                    int players = (int)myEnum.Key;
                    DuelInfo dInfo = (DuelInfo)myEnum.Value;

                    sb.Append(String.Format("{0}vs{0} - {1} minutes {2} seconds{3}", players, dInfo.DuelTime.Minutes, dInfo.DuelTime.Seconds, br));
                }
            }

            return sb.ToString();
        }

        public static string br { get { return "<br>"; } }

        public static string GetQuickestTimes(DuelPoints duelPoints)
        {
            if (duelPoints == null || (duelPoints.FastestWins.Count == 0 && duelPoints.FastestLoses.Count == 0))
                return "No records";

            StringBuilder sb = new StringBuilder();

            if (duelPoints.FastestWins.Count > 0)
            {
                sb.Append("Fastest Wins" + br);
                IDictionaryEnumerator myEnum = duelPoints.FastestWins.GetEnumerator();

                while (myEnum.MoveNext())
                {
                    int players = (int)myEnum.Key;
                    DuelInfo dInfo = (DuelInfo)myEnum.Value;

                    sb.Append(String.Format("{0}vs{0} - {1} minutes {2} seconds{3}", players, dInfo.DuelTime.Minutes, dInfo.DuelTime.Seconds, br));
                }
            }

            if (duelPoints.FastestLoses.Count > 0)
            {
                sb.Append(br);
                sb.Append("Fastest Loses" + br);
                IDictionaryEnumerator myEnum = duelPoints.FastestLoses.GetEnumerator();

                while (myEnum.MoveNext())
                {
                    int players = (int)myEnum.Key;
                    DuelInfo dInfo = (DuelInfo)myEnum.Value;

                    sb.Append(String.Format("{0}vs{0} - {1} minutes {2} seconds{3}", players, dInfo.DuelTime.Minutes, dInfo.DuelTime.Seconds, br));
                }
            }

            return sb.ToString();
        }

        public static string GetLoses(DuelPoints duelPoints)
        {
            if (duelPoints == null || duelPoints.Loses.Count == 0 )
                return "No records";

            StringBuilder sb = new StringBuilder();

            IDictionaryEnumerator myEnum = duelPoints.Loses.GetEnumerator();

            while (myEnum.MoveNext())
            {
                int players = (int)myEnum.Key;
                int loses = (int)myEnum.Value;

                sb.Append(String.Format("{0}vs{0} - {1} loses{2}", players, loses, br));
            }

            return sb.ToString();
        }

        public static string GetWins(DuelPoints duelPoints)
        {
            if (duelPoints == null || duelPoints.Wins.Count == 0)
                return "No records";

            StringBuilder sb = new StringBuilder();

            IDictionaryEnumerator myEnum = duelPoints.Wins.GetEnumerator();

            while (myEnum.MoveNext())
            {
                int players = (int)myEnum.Key;
                int loses = (int)myEnum.Value;

                sb.Append(String.Format("{0}vs{0} - {1} wins{2}", players, loses, br));
            }

            return sb.ToString();
        }
		
		public enum Buttons
		{
			closeBtn = 0,
		}

	}
}