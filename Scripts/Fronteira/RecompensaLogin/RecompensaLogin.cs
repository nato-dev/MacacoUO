using Server.Accounting;
using Server.Commands;
using Server.Engines.Points;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Ziden.RecompensaLogin;
using System;
using System.Collections.Generic;

namespace Server.Ziden.Kills
{

    public class PontosLoginGuilda : PointsSystem
    {
        public override TextDefinition Name { get { return "Pontos de Guilda"; } }
        public override PointsType Loyalty { get { return PointsType.PontosGuilda; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosLogin : PointsSystem
    {
        public override TextDefinition Name { get { return "Pontos de Login"; } }
        public override PointsType Loyalty { get { return PointsType.Login; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;


        public static void Initialize()
        {
            new LoginTimer().Start();
            CommandSystem.Register("login", AccessLevel.Player, new CommandEventHandler(Ranking_OnCommand));
            CommandSystem.Register("ticklogin", AccessLevel.Administrator, new CommandEventHandler(DeathTick));
        }



        [Usage("login")]
        [Description("ve as recompensas de login.")]
        public static void DeathTick(CommandEventArgs e)
        {
            Ticka("Voce ganhou 1 ponto de login forcado");
        }

        public static bool LOGIN_GUILDA = false;

        public static void Ticka(string msg)
        {
            var ips = new HashSet<string>();
            var guildas = new Dictionary<Guild, int>();
            var membros = new HashSet<Mobile>();
           
            foreach (var ns in NetState.Instances)
            {
                if (ns != null && ns.Mobile != null)
                {
                    ns.Mobile.SendMessage(78, msg);
                    PointsSystem.PontosLogin.AwardPoints(ns.Mobile, 1);

                    if (LOGIN_GUILDA && ns.Mobile.Guild is Guild)
                    {
                        if (!ips.Contains(ns.Mobile.NetState.Address.ToString()))
                        {
                            ips.Add(ns.Mobile.NetState.Address.ToString());
                            var ct = 0;
                            var g = ns.Mobile.Guild as Guild;
                            guildas.TryGetValue(g, out ct);
                            ct++;
                            guildas[g] = ct;
                            membros.Add(ns.Mobile);
                        }
                    }
                    else
                    {
                        if(LOGIN_GUILDA)
                            ns.Mobile.SendMessage(38, "Se voce estivesse em uma guilda, teria ganho pontos de login de guilda. Procure ou crie uma guilda para aproveitar ao maximo !");
                    }

                    if (ns.Mobile.RP && ns.Mobile.Deaths > 0 && ns.Mobile.Alive)
                    {
                        ns.Mobile.Deaths--;
                        ns.Mobile.SendMessage(string.Format("Regenerou uma Morte: {0}/5 ", ns.Mobile.Deaths));
                    }
                }
            }

            if(LOGIN_GUILDA)
            {
                foreach (var guilda in guildas.Keys)
                {
                    foreach (var membro in membros)
                    {
                        var valor = guildas[membro.Guild as Guild];
                        membro.SendMessage("Voce ganhou pontos de login de guilda. Ganhe mais pontos quanto mais jogadores estiver online em sua guilda. Digite .login para ver as recompensas.");
                        PointsSystem.LoginGuilda.AwardPoints(membro, valor);
                    }
                }
            }
        }

        [Usage("login")]
        [Description("ve as recompensas de login.")]
        public static void Ranking_OnCommand(CommandEventArgs e)
        {
            if(LOGIN_GUILDA)
            {
                var from = e.Mobile;
                from.SendGump(new GumpOpcoes("Escolha", (opt) =>
                {
                    if (opt == 0)
                    {
                        e.Mobile.SendGump(new LoginRewardsGump(e.Mobile, e.Mobile as PlayerMobile));
                    }
                    else if (opt == 1)
                    {
                        if (from.Guild == null)
                        {
                            from.SendMessage("Voce precisa estar em uma guilda para ganhar estas recompensas. Quando mais pessoas online na guilda, mais recompensas !");
                            return;
                        }
                        e.Mobile.SendGump(new LoginRewardsGump(e.Mobile, e.Mobile as PlayerMobile));
                    }
                }, 0x1BEB, 0, new string[] { "Solo", "Guilda" }));
            } else
            {
                e.Mobile.SendGump(new LoginRewardsGump(e.Mobile, e.Mobile as PlayerMobile));
            }
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
                Ticka(msg);
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
