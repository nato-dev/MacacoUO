using System;
using System.Collections.Generic;

using Server;
using Server.Mobiles;

namespace Server.Dueling
{
    public class DuelTimer : Timer
    {
        internal Duel _Duel;
        private int _Seconds;
        private int _Countdown = 10;

        public int SecondsRemaining { get { return _Seconds; } }

        public DuelTimer( Duel duel, int seconds ) : base( TimeSpan.FromSeconds( 0 ), TimeSpan.FromSeconds( 1.0 ) )
        {
            _Seconds = seconds;
            _Duel = duel;
        }

        protected override void OnTick()
        {
            if( _Countdown == 0 )
            {
                if( _Seconds == DuelController.Instance.DuelLengthInSeconds )
                {
                    _Duel.Started = true;
                    _Duel.Broadcast( "The duel has begun" );
                }
                if( _Seconds % 300 == 0 )
                {
                    int min = _Seconds / 60;

                    _Duel.Broadcast( String.Format( "{0} minutes remaining in the duel.", min ) );
                }

                if( _Seconds == 0 )
                {
                    _Duel.EndDuel();
                    Stop();
                }

                _Seconds--;
            }
            else
            {
                _Duel.Broadcast( String.Format( "Duel will begin in {0} seconds", _Countdown ) );
                _Countdown--;

                if (_Countdown == 0)
                {
                    _Duel.Broadcast("Fight!");

                    for (int i = 0; i < _Duel.Attackers.Count; i++)
                    {
                        _Duel.Attackers[i].Delta(MobileDelta.Noto);
                        _Duel.Defenders[i].Delta(MobileDelta.Noto);
                        _Duel.Attackers[i].InvalidateProperties();
                        _Duel.Defenders[i].InvalidateProperties();
                    }
                }
            }
        }
    }
}
