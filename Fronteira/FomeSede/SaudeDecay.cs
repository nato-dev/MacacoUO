using System;
using Server.Fronteira.Animations;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Misc
{
    public class SaudeDecay : Timer
    {
        public SaudeDecay() : base(TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(6))
        {
            this.Priority = TimerPriority.OneSecond;
        }

        public static void Initialize()
        {
            /* Desabilitado e movido check pro FomeSedeDecay */
            //new SaudeDecay().Start();
        }

        protected override void OnTick()
        {
            SaudeDecayTick();
        }

        private static void SaudeDecayTick()
        {
            foreach (NetState state in NetState.Instances)
            {
                if (state.Mobile != null && state.Mobile.IsPlayer() && state.Mobile.Alive)
                {
                    PlayerMobile playerMobile = state.Mobile as PlayerMobile;
                    if (playerMobile.Hunger <= 6 || playerMobile.Thirst <= 6)
                    {
                        CustomAnimations.GritarDeDorAnimation(playerMobile);
                        playerMobile.Damage(1);
                        if (playerMobile.Thirst <= 2)
                        {
                            playerMobile.SendMessage(38, "Voce desmaiou de tanto passar sede!");
                            playerMobile.Kill();
                            playerMobile.Hunger = 6;
                            playerMobile.Thirst = 6;
                        }
                        else if (playerMobile.Hunger <= 2)
                        {
                            playerMobile.SendMessage(38, "Voce desmaiou de tanto passar fome!");
                            playerMobile.Kill();
                            playerMobile.Hunger = 6;
                            playerMobile.Thirst = 6;
                        }
                        else
                        {
                            if (playerMobile.Hunger <= 6)
                            {
                                playerMobile.SendMessage(38, "Voce esta morrendo de fome!");
                            }

                            if (playerMobile.Thirst <= 6)
                            {
                                playerMobile.SendMessage(38, "Voce esta morrendo de sede!");
                            }
                        }
                    }
                }
            }
        }
    }
}
