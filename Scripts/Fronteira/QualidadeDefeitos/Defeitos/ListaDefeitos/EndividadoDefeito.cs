using System;
using Server.Fronteira.Idade;
using Server.Mobiles;
using Server.Network;

namespace Server.Fronteira.QualidadeDefeitos.ListaDefeitos
{
    public class EndividadoDefeito : Timer
    {
        public EndividadoDefeito() : base( TimeSpan.FromHours( 6 ), TimeSpan.FromHours( 6 ) )
        {
            Priority = TimerPriority.OneMinute;
        }

        public static void Initialize()
        {
            new AmaldicoadoTimer().Start();
        }

        protected override void OnTick()
        {
            foreach (NetState state in NetState.Instances)
            {
                //Procuro todos os mobiles do tipo PlayerMobile
                if (state != null && state.Mobile != null && state.Mobile is PlayerMobile)
                {
                    PlayerMobile playerMobile = state.Mobile as PlayerMobile;

                    //Procura todos os players com tempo de jogo com números divisiveis por 7
                    if (IdadeUtilidade.IdadeEmDias(playerMobile.CreationTime) % 7 == 0)
                    {
                        if (!Banker.Withdraw(playerMobile, 200))
                        {
                            /*
                            if (playerMobile.GetPropertyValue($"{playerMobile.Account.Username} endividado", out int value))
                            {
                                int contatorEndividado = ++value;
                                if (contatorEndividado == 2)
                                {
                                    //TODO Remover todos os alugueis do player
                                    //TODO Fazer a logica de divida
                                } else if (contatorEndividado >= 4)
                                {
                                    //Tag de criminal permanente, até pagar
                                    playerMobile.Criminal = true;
                                }
                            }
                            else
                            {
                                playerMobile.SetPropertyValue($"{playerMobile.Account.Username} endividado", 1);
                            }
                            */
                        }
                    }
                }
            }
        }
    }
}
