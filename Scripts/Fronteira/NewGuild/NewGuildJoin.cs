#region References

using System.Linq;
using System.Net;
using Server.Accounting;
using Server.Network;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using Server.Ziden.Tutorial;

#endregion

namespace Server.Scripts.New.Adam.NewGuild
{
    public abstract class NewPlayerGuildAutoJoin
    {
        public static string Abrev = "NOOB";

        public static HashSet<Serial> asked = new HashSet<Serial>();

        public static void SendStarterGuild(PlayerMobile pm)
        {
            if (pm == null || pm.Guild != null)
            {
                return;
            }

            if (asked.Contains(pm.Serial))
                return;

            if (pm.SkillsTotal >= 7000)
            {
                return;
            }

            if (NewGuildPersistence.Instance == null)
            {

                var NGP = new NewGuildPersistence();
            }

            foreach (var guilda in BaseGuild.List.Values)
            {
                Shard.Debug("Guilda Existe: " + guilda.Abbreviation);
            }

            var g = (Guild)BaseGuild.FindByAbbrev(Abrev);

            if (g != null)
            {
                asked.Add(pm.Serial);
                pm.SendGump(new NewPlayerGuildJoinGump(g, pm));

            }
            else
            {
                TutorialNoob.InicializaWisp(pm);
                Shard.Debug("Nao achei guilda noob com tag " + Abrev);
            }
        }
    }
}
