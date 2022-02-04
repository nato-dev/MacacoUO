using System;
using System.Collections.Generic;

using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Dueling
{
    public class DuelTarget : Target
    {
        private Mobile _Mobile;
        private Duel _Duel;

        public DuelTarget( Mobile mobile, Duel duel )
            : base( 15, false, TargetFlags.None )
        {
            _Mobile = mobile;
            _Duel = duel;
        }

        protected override void OnTargetOutOfLOS( Mobile from, object targeted )
        {
            from.SendMessage( "You cannot see that." );
            from.SendMessage( "Select a new target." );
            from.Target = new DuelTarget( _Mobile, _Duel );
            return;
        }

        protected override void OnTargetNotAccessible( Mobile from, object targeted )
        {
            from.SendMessage( "You cannot see that." );
            from.SendMessage( "Select a new target." );
            from.Target = new DuelTarget( _Mobile, _Duel );
            return;
        }

        protected override void OnTargetUntargetable( Mobile from, object targeted )
        {
            from.SendMessage( "You cannot see that." );
            from.SendMessage( "Select a new target." );
            from.Target = new DuelTarget( _Mobile, _Duel );
            return;
        }

        protected override void OnTargetDeleted( Mobile from, object targeted )
        {
            from.SendMessage( "You cannot see that." );
            from.SendMessage( "Select a new target." );
            from.Target = new DuelTarget( _Mobile, _Duel );
            return;
        }

        protected override void OnCantSeeTarget( Mobile from, object targeted )
        {
            from.SendMessage( "You cannot see that." );
            from.SendMessage( "Select a new target." ); 
            from.Target = new DuelTarget( _Mobile, _Duel );
            return;
        }

        protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
        {
            base.OnTargetCancel( from, cancelType ); 

            _Duel.Broadcast( "The duel was canceled." );            
            DuelController.DestroyDuel( _Duel );
            
            from.SendMessage( "You have cancelled the duel request." );
        }

        protected override void OnTarget( Mobile from, object target )
        {
            if( target is PlayerMobile )
            {
                Mobile m = ( Mobile )target;

                if( m.NetState == null )
                {
                    from.SendMessage( "That player is not online." );
                    from.SendMessage( "Select a new target." );
                    from.Target = new DuelTarget( _Mobile, _Duel );
                    return;
                }
                else if( DuelController.DeclineDuelList.Contains( m ) )
                {
                    from.SendMessage( "This person has elected to decline all duels." );
                    from.SendMessage( "Select a new target." );
                    from.Target = new DuelTarget( _Mobile, _Duel );
                    return;
                }
                else if( m == from )
                {
                    from.SendMessage( "You slap yourself across the face and declare yourself the winner." );
                    from.SendMessage( "Select a new target." );
                    from.Target = new DuelTarget( _Mobile, _Duel );
                    return;
                }
                if( m.Criminal )
                {
                    m.SendMessage( "You may not start a duel with someone who is flagged criminal." );
                    from.SendMessage( "Select a new target." );
                    from.Target = new DuelTarget( _Mobile, _Duel );
                    return;
                }
                else if( Spells.SpellHelper.CheckCombat( m ) )
                {
                    from.SendMessage( "That person is currently in combat. You must wait to duel them." );
                    from.SendMessage( "Select a new target." );
                    from.Target = new DuelTarget( _Mobile, _Duel );
                    return;
                }
                else if( DuelController.IsInDuel( m ) )
                {
                    from.SendMessage( "That person is currently in a duel. You must wait to duel them." );
                    from.SendMessage( "Select a new target." );
                    from.Target = new DuelTarget( _Mobile, _Duel );
                    return;
                }
                else
                {
                    if( _Duel != null )
                    {
                        _Duel.SpotsRemaing--;
                        m.CloseGump( typeof( DuelAcceptGump ) );
                        m.SendGump( new DuelAcceptGump( m, _Duel ) );

                        if( _Duel.SpotsRemaing != 0 )
                        {
                            _Duel.Creator.SendMessage( "Select another player to join the duel." );
                            _Duel.Creator.Target = new DuelTarget( _Duel.Creator, _Duel );
                        }
                    }
                }
            }
            else
            {
                from.SendMessage( "That is not a player." );
                from.SendMessage( "Select a new target." );
                from.Target = new DuelTarget( _Mobile, _Duel );
                return;
            }
        }
    }
}
