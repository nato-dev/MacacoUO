using Server.Commands;
using Server.Engines.Points;
using Server.Gumps;
using Server.Mobiles;
using System;

namespace Server.Ziden.Kills
{
    public class PontosKills : PointsSystem
    {
        public override TextDefinition Name { get { return "Kills"; } }
        public override PointsType Loyalty { get { return PointsType.Kills; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return false; } }
        public static bool Enabled = true;

        public static void Initialize()
        {
            if (Shard.WARSHARD)
            {
                CommandSystem.Register("rankpvp", AccessLevel.Player, Cmd);
                EventSink.PlayerDeath += new PlayerDeathEventHandler(PlayerDeath);
            }
        }

        [Usage("Kills")]
        private static void Cmd(CommandEventArgs e)
        {
            e.Mobile.SendGump(new KillsGump(e.Mobile));
        }

        public static void PlayerDeath(PlayerDeathEventArgs ev)
        {
            if (ev.Mobile.IsPlayer() && ev.Killer != null && ev.Killer is PlayerMobile)
            {

                PointsSystem.PontosKills.AwardPoints(ev.Killer, 1, false, false);
                ev.Killer.SendMessage("Você ganhou 1 ponto de .pvp pela morte do inimigo");
                PointsSystem.PontosKills.DeductPoints(ev.Mobile, 1, false);
                ev.Mobile.SendMessage("Você perdeu 1 ponto de .pvp ao morrer");
                ev.Killer.SetCooldown("kill" + ev.Mobile.Name, TimeSpan.FromHours(4));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            Shard.Debug("Salvando pontos de kills");
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            Shard.Debug("Carregando pontos de kills");
            base.Deserialize(reader);
            this.GetOrCalculateRank();
        }
    }
}
