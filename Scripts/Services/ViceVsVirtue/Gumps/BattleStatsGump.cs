using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Guilds;
using Server.Network;
using System.Collections.Generic;
using System.Globalization;

namespace Server.Engines.VvV
{
    public class BattleStatsGump : Gump
    {
        public VvVBattle Battle { get; set; }

        public static readonly int Color16 = Engines.Quests.BaseQuestGump.C32216(0xB22222);

        public BattleStatsGump(PlayerMobile pm, VvVBattle battle)
            : base(50, 50)
        {
            Battle = battle;
            BattleTeam leader = battle.GetLeader();
            Guild myGuild = pm.Guild as Guild;

            if (leader == null || leader.Guild == null || myGuild == null)
                return;

            AddBackground(0, 0, 500, 500, 9380);

            AddHtml(0, 40, 500, 20, "<CENTER>Fim da Guerra Infinita</CENTER>", Color16, false, false); // The Battle between Vice and Virtue has ended!
            AddHtml(40, 65, 420, 20, String.Format("<basefont color=#B22222>{0} [{1}] Ganhou a batalha!", leader.Guild.Name, leader.Guild.Abbreviation), false, false);

            int y = 90;

            if (leader.Guild.Alliance != null)
            {
                AddHtml(40, y, 420, 20, String.Format("<basefont color=#B22222>A Alianca {0} ganhou a batalha!", leader.Guild.Alliance.Name), false, false);
                y += 25;
            }

            BattleTeam team = Battle.GetTeam(myGuild);

            //TODO: Are totals the PLAYERS OVERALL totals, or the guild/alliance totals for that battle???  Or that players totals for that battle
            /*silver += (int)ViceVsVirtueSystem.Instance.GetPoints(pm);

            VvVPlayerEntry entry = ViceVsVirtueSystem.Instance.GetPlayerEntry<VvVPlayerEntry>(pm);

            if (entry != null)
            {
                score = entry.Score;
            }*/

            AddHtml(40, y, 420, 20, "Pratinhas Totais: "+ team.Silver.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false); // Total Silver Points: ~1_val~
            y += 25;

            AddHtml(40, y, 420, 20, "Pontos Totais: "+ team.Score.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false); // Total Score: ~1_val~
            y += 25;

            AddHtml(40, y, 420, 20, "Kills: "+ team.Kills.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;

            AddHtml(40, y, 420, 20, "Assists: "+ team.Assists.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;

            AddHtml(40, y, 420, 20, "Mortes: "+ team.Deaths.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;

            AddHtml(40, y, 420, 20, "Roubados: "+ team.Stolen.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;

            AddHtml(40, y, 420, 20, "Retornados: "+ team.ReturnedSigils.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;

            AddHtml(40, y, 420, 20, "Caos Retornados: "+ team.ViceReturned.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;

            AddHtml(40, y, 420, 20, "Ordem Retornados: "+ team.VirtueReturned.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;

            AddHtml(40, y, 420, 20, "Desarmados: "+ team.Disarmed.ToString("N0", CultureInfo.GetCultureInfo("en-US")), Color16, false, false);
            y += 25;
        }
    }
}
