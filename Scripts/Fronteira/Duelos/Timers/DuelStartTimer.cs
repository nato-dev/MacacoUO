using System;
using System.Collections.Generic;

using Server;
using Server.Mobiles;

namespace Server.Dueling
{
    public class DuelStartTimer : Timer
    {
        internal Duel _Duel;

        public DuelStartTimer( Duel duel ) 
            : base( TimeSpan.FromSeconds( 120.0 ) )
        {
            _Duel = duel;
        }

        protected override void OnTick()
        {
            _Duel.Broadcast( "O duelo demorou demais..." );
            
            if( _Duel != null )
                DuelController.DestroyDuel( _Duel );
        }
    }
}
