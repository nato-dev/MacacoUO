using System;
using System.Collections.Generic;

using Server;
using Server.Gumps;

namespace Server.Dueling
{
	public class DuelTeamSelectionGump : Gump
	{
        private List<MobileEntry> _MobileEntries;
        private Duel _Duel;

		public DuelTeamSelectionGump( Duel duel )
			: base( 200, 200 )
		{
            _Duel = duel;
            _MobileEntries = new List<MobileEntry>();

			this.Closable=false;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(36, 25, 402, 375, 3500);
            this.AddLabel( 166, 40, 36, @"Onsite Duel System 2.0" );
            this.AddLabel( 166, 39, 36, @"Onsite Duel System 2.0" );
			this.AddButton(401, 46, 3, 4, (int)Buttons.closeBtn, GumpButtonType.Reply, 0);
            this.AddLabel( 129, 56, 36, @"Please select the team for each player" );
            this.AddLabel( 129, 55, 36, @"Please select the team for each player" );
            this.AddLabel( 100, 80, 36, @"Players" );
            this.AddLabel( 100, 79, 36, @"Players" );
            this.AddLabel( 263, 80, 36, @"Team 1" );
            this.AddLabel( 263, 79, 36, @"Team 1" );
            this.AddLabel( 327, 80, 36, @"Team 2" );
            this.AddLabel( 327, 79, 36, @"Team 2" );
            this.AddLabel( 121, 360, 36, @"Start" );
            this.AddLabel( 121, 359, 36, @"Start" );
            this.AddButton( 103, 363, 4034, 4034, ( int )Buttons.startBtn, GumpButtonType.Reply, 0 );

            for( int i = 0; i < duel.Contestants.Count; i++ )
                AddEntry( duel.Contestants[i], i );         
		}

        private void AddEntry( Mobile mobile, int i )
        {
            int y = ( i * 25 ) + 108;
            int x = 100;

            int buttonOne = mobile.Serial.Value;
            int buttonTwo = mobile.Serial.Value + 1;

            MobileEntry mEntry = new MobileEntry( mobile, buttonOne, buttonTwo );
            _MobileEntries.Add( mEntry );

            AddGroup( i );
            AddLabel( x, y, 36, mobile.Name );     
            
            y = ( i * 25 ) + 104;
            x = 272;

            AddRadio( x, y, 9792, 9793, ( i < 5 ), buttonOne );  
               
            x = 338;
            AddRadio( x, y, 9792, 9793, ( i > 4 ), buttonTwo );
       }
		
		public enum Buttons
		{
			closeBtn,
            startBtn
		}

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            Mobile m = sender.Mobile;

            if( m == null || _Duel == null )
                return;

            int[] sw = info.Switches;

            if( sw.Length != ( _Duel.Attackers.Capacity + _Duel.Defenders.Capacity ) )
            {
                _Duel.Creator.SendMessage( "Some players were not assigned to a team. Please try again." );
                _Duel.Creator.CloseGump( typeof( DuelTeamSelectionGump ) );
                _Duel.Creator.SendGump( new DuelTeamSelectionGump( _Duel ) );
                return;
            }

            List<int> switches = new List<int>(sw);

            switch( (Buttons)info.ButtonID )
            {
                case Buttons.closeBtn:
                    {
                        _Duel.Broadcast( "The duel was canceled" );
                        DuelController.DestroyDuel( _Duel );
                        break;
                    }
                case Buttons.startBtn:
                    {
                        int teamCheckOne = 0;
                        int teamCheckTwo = 0;

                        for( int i = 0; i < _MobileEntries.Count; i++ )
                            if( switches.Contains( _MobileEntries[i].TeamOne ) )
                                teamCheckOne++;
                            else
                                teamCheckTwo++;                     

                        if( teamCheckOne != teamCheckTwo )
                        {
                            _Duel.Creator.SendMessage( "The two teams were not even, please try again." );
                            _Duel.Creator.CloseGump( typeof( DuelTeamSelectionGump ) );
                            _Duel.Creator.SendGump( new DuelTeamSelectionGump( _Duel ) );
                            return;
                        }
                        else
                        {
                            for( int i = 0; i < _MobileEntries.Count; i++ )
                                if( switches.Contains( _MobileEntries[i].TeamOne ) )
                                    _Duel.Attackers.Add( _MobileEntries[i].Mobile );
                                else
                                    _Duel.Defenders.Add( _MobileEntries[i].Mobile );

                            _Duel.Contestants.Clear();

                            _Duel.CheckBegin();
                        }

                        break;
                    }
            }
        }
	}

    public class MobileEntry
    {
        private int _TeamOne;
        private int _TeamTwo;
        private Mobile _Mobile;

        public int TeamOne { get { return _TeamOne; } }
        public int TeamTwo { get { return _TeamTwo; } }
        public Mobile Mobile { get { return _Mobile; } }

        public MobileEntry( Mobile mobile, int teamOne, int teamTwo )
        {
            _Mobile = mobile;
            _TeamOne = teamOne;
            _TeamTwo = teamTwo;
        }
    }
}