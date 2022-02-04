using Server.Commands;
using Server.Engines.Points;
using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden
{
    public class RankFama
    {
        public static void Initialize()
        {
            CommandSystem.Register("calcularank", AccessLevel.Administrator, new CommandEventHandler(CMD));
            CommandSystem.Register("rank", AccessLevel.Administrator, new CommandEventHandler(CMDRank));
            Rank = PlayerMobile.Instances.OrderByDescending(e => e.Fame).Select((e, index) => {
                e.RankingFama = index;
                return new PointsEntry(e, e.Fame); }
            ).ToList();
            Console.WriteLine("Rankings de Fama Calculados");
        }

        public static List<PointsEntry> Rank = new List<PointsEntry>();

        [Usage("calcula")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            if (pl == null)
                return;

            Rank = PlayerMobile.Instances.OrderByDescending(e => e.Fame).Select((e, index) => { e.RankingFama = index; return new PointsEntry(e, e.Fame); }).ToList();
            pl.SendMessage("Rankings Calculados");
            Console.WriteLine("Rankings de Fama Calculados");
        }

        [Usage("calcula")]
        [Description("Camping Menu.")]
        public static void CMDRank(CommandEventArgs arg)
        {
            arg.Mobile.SendGump(new FamaGump(arg.Mobile));
        }
    }
}
