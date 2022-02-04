using System;
using Server.Gumps;
using Server.Mobiles;
using System.Linq;
using Server.Network;
using Server.Engines.CityLoyalty;

namespace Server.Engines.Points
{
    public class LoyaltyRatingGump : Gump
    {
        public LoyaltyRatingGump(PlayerMobile pm)
            : base(120, 120)
        {
            AddBackground(0, 0, 400, 800, 3500);

            AddHtml(0, 8, 345, 20, "<center>Pontos</center>", false, false); // <center>Loyalty Ratings</center>

            int y = 40;

            foreach (var sys in PointsSystem.Systems.Where(sys => sys.ShowOnLoyaltyGump))
            {
                if (sys.Name.Number > 0)
                    AddHtmlLocalized(50, y, 150, 20, sys.Name.Number, false, false);
                else if (sys.Name.String != null)
                    AddHtml(50, y, 150, 20, sys.Name.String, false, false);

                TextDefinition title = sys.GetTitle(pm);

                AddHtmlLocalized(175, y, 100, 20, 1095171, ((int)sys.GetPoints(pm)).ToString(), 0, false, false); // (~1_AMT~ points)

                y += 25;
            }

            AddHtmlLocalized(175, y, 150, 20, 1115129, pm.Fame.ToString(), 0, false, false); // Fame: ~1_AMT~
            y += 25;
            AddHtmlLocalized(175, y, 150, 20, 1115130, pm.Karma.ToString(), 0, false, false); // Karma: ~1_AMT~}

            if (CityLoyaltySystem.Enabled && CityLoyaltySystem.Cities != null)
            {
                AddHtml(60, 395, 150, 20, "Lealdade a Cidade:", false, false);  // City Loyalty
                AddButton(40, 400, 2103, 2104, 1, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile pm = state.Mobile as PlayerMobile;

            if (CityLoyaltySystem.Enabled && CityLoyaltySystem.Cities != null && pm != null && info.ButtonID == 1)
                BaseGump.SendGump(new CityLoyaltyGump(pm));
        }
    }
}
