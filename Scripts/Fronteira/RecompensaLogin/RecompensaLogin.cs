using Server.Commands;
using Server.Engines.Points;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Ziden.RecompensaLogin;
using System;

namespace Server.Ziden.Kills
{
    public class PontosLogin : PointsSystem
    {
        public override TextDefinition Name { get { return "Login Points"; } }
        public override PointsType Loyalty { get { return PointsType.Login; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;


        public static void Initialize()
        {
            new LoginTimer().Start();
            CommandSystem.Register("login", AccessLevel.Player, new CommandEventHandler(Ranking_OnCommand));
            CommandSystem.Register("deathtick", AccessLevel.Administrator, new CommandEventHandler(DeathTick));
        }

        [Usage("login")]
        [Description("ve as recompensas de login.")]
        public static void DeathTick(CommandEventArgs e)
        {
            foreach (var ns in NetState.Instances)
            {
                if (ns != null && ns.Mobile != null)
                {
                    Shard.Debug("Ponto login");
                    PointsSystem.PontosLogin.AwardPoints(ns.Mobile, 1);
                    if (ns.Mobile.Alive && ns.Mobile.RP && ns.Mobile.Deaths > 0)
                    {
                        ns.Mobile.Deaths--;
                        ns.Mobile.SendMessage(string.Format("Regenerou uma Morte: {0}/5 ", ns.Mobile.Deaths));
                    }

                }
            }
        }

        [Usage("login")]
        [Description("ve as recompensas de login.")]
        public static void Ranking_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new LoginRewardsGump(e.Mobile, e.Mobile as PlayerMobile));
        }

        private class LoginTimer : Timer
        {
            public static string msg = "Voce ganhou 1 ponto de atividade que pode trocado usando o comando '.login'";
            public LoginTimer()
                : base(TimeSpan.FromHours(1), TimeSpan.FromHours(1))
            {
                this.Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                foreach(var ns in NetState.Instances)
                {
                    if(ns != null && ns.Mobile != null)
                    {
                        ns.Mobile.SendMessage(78, msg);
                        PointsSystem.PontosLogin.AwardPoints(ns.Mobile, 1);
                        if(ns.Mobile.RP && ns.Mobile.Deaths > 0 && ns.Mobile.Alive)
                        {
                            ns.Mobile.Deaths--;
                            ns.Mobile.SendMessage(string.Format("Regenerou uma Morte: {0}/5 ", ns.Mobile.Deaths));
                        }

                    }
                }
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
