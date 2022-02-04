using Server.Targeting;

namespace Server.Commands
{
    public class Refresh
    {

        public static void Initialize()
        {
            CommandSystem.Register("Refresh", AccessLevel.GameMaster, Refresh_OnCommand);
        }

        public static void Refresh_OnCommand(CommandEventArgs t)
        {
            t.Mobile.Target = new RefreshTarget();
        }

        public class RefreshTarget : Target
        {
            public RefreshTarget()
                : base(-1, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    var alvo = (Mobile)targeted;

                    if (alvo.Player)
                    {
                        if (!alvo.Alive)
                        {
                            alvo.Resurrect();
                            alvo.Hits = alvo.HitsMax;
                            alvo.Mana = alvo.ManaMax;
                            alvo.Stam = alvo.StamMax;
                            from.SendMessage(0x00FE, "Alvo ressuscitado e curado.");
                        }
                        else
                        {
                            alvo.Hits = alvo.HitsMax;
                            alvo.Mana = alvo.ManaMax;
                            alvo.Stam = alvo.StamMax;
                            from.SendMessage(0x00FE, "Alvo curado.");
                        }
                    }
                    else
                    {
                        from.SendMessage(0x00FE, "Você precisa selecionar um jogador.");
                    }
                }
                else
                {
                    from.SendMessage(0x00FE, "Alvo inválido.");
                }
            }
        }
    }
}
