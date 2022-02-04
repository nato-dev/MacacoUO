using System;
using System.Collections.Generic;

using Server;
using Server.Mobiles;

namespace Server.Dueling
{
    public class LogoutTimeoutTimer : Timer
    {
        internal Mobile _Mobile;
        internal Duel _Duel;

        public LogoutTimeoutTimer( Mobile m, Duel duel )
            : base(TimeSpan.FromSeconds(30.0))
        {
            _Mobile = m;
            _Duel = duel;
        }

        protected override void OnTick()
        {
            if (_Duel != null && _Duel.Started && !_Mobile.Alive)
                _Mobile.Kill();

            if (DuelController.DuelStartTimeoutTable.ContainsKey(_Mobile.Serial))
                DuelController.DuelStartTimeoutTable.Remove(_Mobile.Serial);

            Stop();
        }
    }
}
