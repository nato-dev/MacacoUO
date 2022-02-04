using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Dueling
{
    public class DuelInfo
    {
        private TimeSpan _DuelTime;
        private List<Mobile> _AgainstList;

        public TimeSpan DuelTime { get { return _DuelTime; } }
        public List<Mobile> AgainstList { get { return _AgainstList; } }        

        public DuelInfo( List<Mobile> team, TimeSpan duelTime )
        {
            _AgainstList = team;
            _DuelTime = duelTime;
        }

        public DuelInfo( GenericReader reader )
        {
            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        _DuelTime = reader.ReadTimeSpan();
                        _AgainstList = reader.ReadStrongMobileList();
                        break;
                    }
            }
        }

        internal void Serialize( GenericWriter writer )
        {
            writer.Write( ( int )0 );

            writer.Write( ( TimeSpan )_DuelTime );
            writer.WriteMobileList<Mobile>( _AgainstList );
        }
    }
}
