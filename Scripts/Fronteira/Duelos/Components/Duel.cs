using System;
using System.Collections.Generic;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Dueling
{
    public class Duel
    {
        private bool _Started;
        public bool Started { get { return _Started; } set { _Started = value; } }

        private int _SpotsRemaining = 0;
        public int SpotsRemaing { get { return _SpotsRemaining; } set { _SpotsRemaining = value; } }

        public static int MaxDistance { get { return DuelController.Instance.MaxDistance; } }

        private Mobile _Creator;
        public Mobile Creator { get { return _Creator; } set { _Creator = value; } }

        private List<Mobile> _Contestants;
        private List<Mobile> _Attackers;
        private List<Mobile> _Defenders;
        public List<Mobile> Attackers { get { return _Attackers; } }
        public List<Mobile> Defenders { get { return _Defenders; } }
        public List<Mobile> Contestants { get { return _Contestants; } }

        private bool _Statemate;

        private DuelTimer _DuelTimer;
        public DuelTimer DuelTimer { get { return _DuelTimer; } }

        public Duel() { }

        public Duel( Mobile creator )
        {
            _Creator = creator;
        }

        public Duel( Mobile creator, int playerCountPerTeam )
        {
            Configure( playerCountPerTeam );
            _Contestants.Add( creator );
            _Creator = creator;
        }

        public void Configure( int playerCountPerTeam )
        {
            _Attackers = new List<Mobile>( playerCountPerTeam );
            _Defenders = new List<Mobile>( playerCountPerTeam );
            _Contestants = new List<Mobile>( playerCountPerTeam * 2 );
        }

        internal void HandleDeath( Mobile m )
        {
            InternalHandleDeath( m ); 
        }

        private void InternalHandleDeath( Mobile m )
        {              
            if( CheckIfComplete() )
            {
                EndDuel();
            }
        }

        private bool CheckIfComplete()
        {
            return ( ( DeathCheck( _Attackers ) == _Attackers.Capacity ) || 
                ( DeathCheck( _Defenders ) == _Defenders.Capacity ) );
        }

        private int DeathCheck( List<Mobile> mobiles )
        {
            int deadCount = 0;

            for( int i = 0; i < mobiles.Count; i++ )
                if( !mobiles[i].Alive )
                    deadCount++;

            return deadCount;
        }

        private void AttackersWin()
        {
            if( !_Statemate )
            {
                List<Mobile> winners = _Attackers;
                List<Mobile> losers = _Defenders;

                for( int i = 0; i < winners.Count; i++ )
                {
                    HandlePoints( winners[i], true );
                    HandlePoints( losers[i], false );
                }
            }
        }

        private void CompleteDuel()
        {
            List<Mobile> contestants = new List<Mobile>();

            contestants.AddRange( _Attackers );
            contestants.AddRange( _Defenders );

            for( int i = 0; i < contestants.Count; i++ )
                FixMobile( contestants[i] );

            DuelController.DestroyDuel( this );
        }

        private void HandlePoints( Mobile m, bool winner )
        {
            if( winner )
            {
                DuelController.AddWin( this, m );
            }
            else
            {
                DuelController.AddLoss( this, m );
            }
        }

        private void DefendersWin()
        {
            if( !_Statemate )
            {
                List<Mobile> winners = _Defenders;
                List<Mobile> losers = _Attackers;
                
                for( int i = 0; i < winners.Count; i++ )
                {
                    HandlePoints( winners[i], true );
                    HandlePoints( losers[i], false );
                }                    
            }
        }

        private void FixMobile( Mobile m )
        {
            if( !m.Alive )
                m.Resurrect();

            HandleCorpse( m );

            m.Aggressed.Clear();
            m.Aggressors.Clear();
            m.Hits = m.HitsMax;
            m.Stam = m.StamMax;
            m.Mana = m.ManaMax;
            m.DamageEntries.Clear();
            m.Combatant = null;
            m.InvalidateProperties();

            m.SendMessage( "The duel has ended." );
        }

        internal void Begin()
        {
            InternalBegin();

            _DuelTimer = new DuelTimer(this, DuelController.Instance.DuelLengthInSeconds );
            _DuelTimer.Start();
        }

        private void InternalBegin()
        {
            if( DuelController.DuelStartTimeoutTable.ContainsKey( Creator.Serial ) )
            {
                DuelStartTimer timer = DuelController.DuelStartTimeoutTable[Creator.Serial];
                timer.Stop();

                DuelController.DuelStartTimeoutTable.Remove( Creator.Serial );
            }

            for( int i = 0; i < _Attackers.Count; i++ )
            {
                _Attackers[i].Delta( MobileDelta.Noto );
                _Defenders[i].Delta( MobileDelta.Noto );
                _Attackers[i].InvalidateProperties();
                _Defenders[i].InvalidateProperties();
            }
        }

        internal void Stalemate()
        {
            _Statemate = true;

            CompleteDuel();
        }

        internal bool CheckDistance()
        {
            bool toFar = false;

            for( int i = 0; i < _Attackers.Count; i++ )
            {
                for( int j = 0; j < _Defenders.Count; j++ )
                    if( GetDistance( _Attackers[i], _Defenders[i] ) > MaxDistance )
                    {
                        toFar = true;
                        break;
                    }

                if( toFar )
                    break;
            }

            return toFar;
        }

        public void HandleCorpse( Mobile from )
        {
            if( from.Corpse != null )
            {
                Corpse c = ( Corpse )from.Corpse;
                Container b = from.Backpack;
                List<Item> toAdd = new List<Item>();
                

                foreach( Item i in c.Items )
                {
                    if( i.Map != Map.Internal )
                        toAdd.Add(i);
                }

                foreach (Item i in toAdd)
                {
                    b.AddItem(i);
                }

                toAdd = null;

                c.Delete();

                from.SendMessage( 1161, "The contents of your corpse have been safely placed into your backpack" );
            }
        }

        private int GetDistance( Mobile to, Mobile from )
        {
            int minX = Math.Min( to.Location.X, from.Location.X );
            int minY = Math.Min( to.Location.Y, from.Location.Y );
            int maxX = Math.Max( to.Location.X, from.Location.X );
            int maxY = Math.Max( to.Location.Y, from.Location.Y );
            
            return Math.Max( maxX - minX, maxY - minY );
        }

        internal void EndDuel()
        {
            _Started = false;
            _DuelTimer.Stop();

            if( DeathCheck( _Defenders ) > DeathCheck( _Attackers ) )
                DefendersWin();
            else if( DeathCheck( _Attackers ) > DeathCheck( _Defenders ) )
                AttackersWin();
            else
            {
                int attackersHealth = 0;
                int defendersHealth = 0;

                for( int i = 0; i < _Attackers.Count; i++ )
                    attackersHealth += _Attackers[i].Hits;

                for( int i = 0; i < _Defenders.Count; i++ )
                    defendersHealth += _Defenders[i].Hits;

                if( attackersHealth > defendersHealth )
                    AttackersWin();
                else if( defendersHealth > attackersHealth )
                    DefendersWin();
                else
                {
                    _Statemate = true;

                    Broadcast( "It's a draw!!!" );
                }
            }

            CompleteDuel();
        }

        internal void CheckBegin()
        {
            if( _Contestants.Count == _Contestants.Capacity )
            {
                Broadcast( "Please wait while the duel creator sets the teams." );
                _Creator.SendGump( new DuelTeamSelectionGump( this ) );
            }

            if( _Defenders.Count == _Defenders.Capacity && _Attackers.Count == _Attackers.Capacity )
                Begin();
        }

        internal void Broadcast( string msg )
        {
            DuelController.Broadcast( msg, Attackers );
            DuelController.Broadcast( msg, Defenders );
        }
    }
}
