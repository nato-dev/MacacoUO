using System;
using Server;
using Server.Gumps;

namespace Server.Dueling
{
	public class DuelAcceptGump : Gump
	{
        private Mobile _Mobile;
        private Duel _Duel;
        private TimeoutTimer _Timer;

		public DuelAcceptGump( Mobile m, Duel duel )
			: base( 200, 200 )
		{
            _Mobile = m;
            _Duel = duel;

            _Timer = new TimeoutTimer( duel, m );
            _Timer.Start();

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(40, 25, 324, 167, 3500);
            this.AddLabel( 130, 40, 36, @"Onsite Duel System 2.0" ); 
            this.AddLabel( 130, 39, 36, @"Onsite Duel System 2.0" );
			this.AddLabel( 60, 73, 36, m.Name + " has invited you to their duel" );
            this.AddLabel( 60, 72, 36, m.Name + " has invited you to their duel" );
            this.AddLabel( 60, 96, 36, @"Do you wish to join?" );
            this.AddLabel( 60, 95, 36, @"Do you wish to join?" );
            this.AddLabel( 162, 146, 36, @"Yes" );
            this.AddLabel( 162, 145, 36, @"Yes" );
            this.AddLabel( 222, 146, 36, @"No" );
            this.AddLabel( 222, 145, 36, @"No" ); 
            this.AddButton( 327, 44, 3, 4, ( int )Buttons.closeBtn, GumpButtonType.Reply, 0 );
			this.AddButton(144, 149, 4034, 4034, (int)Buttons.yesBtn, GumpButtonType.Reply, 0);
			this.AddButton(204, 149, 4034, 4034, (int)Buttons.noBtn, GumpButtonType.Reply, 0);

		}
		
		public enum Buttons
		{
			closeBtn,
			yesBtn,
			noBtn,
		}

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            _Timer.Stop();

            Mobile m = sender.Mobile;

            if( m == null || _Duel == null )
                return;

            switch( ( Buttons )info.ButtonID )
            {
                case Buttons.closeBtn:
                    {
                        _Duel.SpotsRemaing++;
                        _Duel.Broadcast( m.Name + " declined to join the duel" );
                        CheckTarget();
                        break;
                    }
                case Buttons.noBtn:
                    {
                        _Duel.SpotsRemaing++;
                        _Duel.Broadcast( m.Name + " declined to join the duel" );
                        CheckTarget();
                        break;
                    }
                case Buttons.yesBtn:
                    {
                        _Duel.Contestants.Add( m );
                        DuelController.DuelTable.Add( m.Serial, _Duel );
                        _Duel.Broadcast( m.Name + " has joined the duel." );
                        _Duel.CheckBegin();
                        break;
                    }
            }
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