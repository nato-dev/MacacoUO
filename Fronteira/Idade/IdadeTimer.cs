using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Fronteira.Idade
{
    public class IdadeTimer : Timer
    {
        public IdadeTimer() : base(TimeSpan.FromHours(24), TimeSpan.FromHours(24))
        {
            this.Priority = TimerPriority.OneMinute;
        }

        public static void Initialize()
        {
            new IdadeTimer().Start();
        }

        protected override void OnTick()
        {
            VerificaIdades();
        }

        private void VerificaIdades()
        {
            var vaiMorrer = Utility.RandomBool();
            if (!vaiMorrer) return;
            foreach (NetState state in NetState.Instances)
            {
                if (state.Mobile != null && state.Mobile.IsPlayer() && state.Mobile.Alive && vaiMorrer &&
                    IdadeUtilidade.IsTempoLimiteVida(state.Mobile as PlayerMobile))
                {
                    PlayerMobile playerMobile = state.Mobile as PlayerMobile;
                    playerMobile.SendMessage(38, "Chegou sua hora...");
                    playerMobile.SendMessage(38, IdadeUtilidade.GeraMotivoMorte());
                    playerMobile.Deaths = PlayerMobile.MAX_MORTES;
                    playerMobile.Kill();
                }
            }
        }
    }
}
