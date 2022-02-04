using System;
using System.Collections.Generic;

using Server;
using Server.Mobiles;

namespace Server.Dueling
{
    public class TimeoutTimer : Timer
    {
        internal Mobile _Mobile;
        internal Duel _Duel;

        public TimeoutTimer( Duel duel, Mobile m ) : base( TimeSpan.FromSeconds( 15.0 ) )
        {
            _Mobile = m;
            _Duel = duel;
        }

        protected override void OnTick()
        {
            _Mobile.CloseGump( typeof( DuelAcceptGump ) );
            _Mobile.SendMessage( "Duel accept gump timeout" );
            _Duel.SpotsRemaing++;
            _Duel.Broadcast( _Mobile.Name + " declined to join the duel" );
            CheckTarget();
            Stop();
        }

        private void CheckTarget()
        {
            if( !( _Duel.Creator.Target is DuelTarget ) )
            {
                _Duel.Creator.SendMessage( "Please select another player to join the duel." );
                _Duel.Creator.Target = new DuelTarget( _Duel.Creator, _Duel );
            }
        }


    }
}
