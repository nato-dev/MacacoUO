using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Regions;
using Server.Mobiles;
using Server.Commands;

namespace Server.Dueling
{
    public class DuelController
    {
        private int _MaxDistance = 30;
        private int _DuelLengthInSeconds = 1800;

        private static bool _Enabled = false;

        [CommandProperty(AccessLevel.Administrator)]
        public static bool Enabled { get { return _Enabled; } set { _Enabled = value; } }

        [CommandProperty( AccessLevel.Administrator )]
        public int DuelLengthInSeconds { get { return _DuelLengthInSeconds; } set { _DuelLengthInSeconds = value; } }

        [CommandProperty( AccessLevel.Administrator )]
        public int MaxDistance { get { return _MaxDistance; } set { _MaxDistance = value; } }

        private static List<Mobile> _DeclineDuelList;
        public static List<Mobile> DeclineDuelList { get { return _DeclineDuelList; } }

        private static DuelController _Instance;
        public static DuelController Instance { get { return _Instance; } }

        private static Dictionary<Serial, LogoutTimeoutTimer> _TimeoutTable;
        private static Dictionary<Serial, DuelStartTimer> _DuelStartTimeoutTable;
        private static Dictionary<Serial, Duel> _DuelTable;
        private static Dictionary<Serial, DuelPoints> _PointsTable;

        public static Dictionary<Serial, Duel> DuelTable { get { return _DuelTable; } set { _DuelTable = value; } }
        public static Dictionary<Serial, DuelStartTimer> DuelStartTimeoutTable { get { return _DuelStartTimeoutTable; } set { _DuelStartTimeoutTable = value; } }

        public static void Initialize()
        {
            EventSink.PlayerDeath += new PlayerDeathEventHandler( EventSink_PlayerDeath );
            EventSink.Logout += new LogoutEventHandler( EventSink_Logout );
            EventSink.Login += new LoginEventHandler( EventSink_Login );
            EventSink.Movement += new MovementEventHandler( EventSink_Movement );
            EventSink.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );

            //PlayerMobile.AllowBeneficialHandler = new AllowBeneficialHandler( PlayerMobile_AllowBenificial );
            //PlayerMobile.AllowHarmfulHandler = new AllowHarmfulHandler( PlayerMobile_AllowHarmful );
            //Notoriety.Handler = new NotorietyHandler( Notoriety_HandleNotoriety );
            
            CommandSystem.Register( "OnsiteConfig", AccessLevel.Administrator, new CommandEventHandler( OnCommand_OnsiteConfig ) );
            CommandSystem.Register( "Duel", AccessLevel.Player, new CommandEventHandler( OnCommand_Duel ) );

            _Instance = new DuelController();
            _TimeoutTable = new Dictionary<Serial, LogoutTimeoutTimer>();
            _DuelTable = new Dictionary<Serial, Duel>();
            _PointsTable = new Dictionary<Serial, DuelPoints>();
            _DeclineDuelList = new List<Mobile>();
            _DuelStartTimeoutTable = new Dictionary<Serial, DuelStartTimer>();

            LoadSaves();  
        }

        private static void OnCommand_Duel( CommandEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;


            if (!_Enabled)
            {
                m.SendMessage("Sorry the duel system is currently offline. Please try again later.");
                return;
            }

            if( !CanDuel( m ) )
                return;

            Duel duel = new Duel( m );
                        
            if( !_DuelTable.ContainsKey( m.Serial ) )
                _DuelTable.Add(m.Serial, duel);

            m.CloseGump( typeof( DuelConfigGump ) );
            m.SendGump( new DuelConfigGump( duel ) );
        }

        public static bool CanDuel( Mobile m )
        {
            Duel duel;

            if( m.Criminal )
            {
                m.SendMessage( "You may not start a duel while flagged criminal." );
                return false;
            }
            /*else if( _DeclineDuelList.Contains( m ) )
            {
                m.SendMessage( "You have elected to not duel, use \"[AllowDuel true\" to be able to duel." );
                return false;
            }*/
            else if( Spells.SpellHelper.CheckCombat( m ) )
            {
                m.SendMessage( "You cannot start a duel while in combat." );
                return false;
            }           
            else if( IsInDuel( m, out duel) )
            {
                m.SendMessage( "You are currently in a duel." );
                return false;
            }
            else if( m.Hits != m.HitsMax )
            {
                m.SendMessage( "Try again when you have full health." );
                return false;
            }
            else if( Factions.Sigil.ExistsOn( m ) )
            {
                m.SendMessage( "You may not challenge someone while you have a faction sigil." );
                return false;
            }

            return true;
        }

        private static void OnCommand_OnsiteConfig( CommandEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            m.CloseGump( typeof( PropertiesGump ) );
            m.SendGump( new PropertiesGump( m, _Instance ) );
        }

        private static void LoadSaves()
        {
            if( !File.Exists( Path.Combine( Core.BaseDirectory, "\\Saves\\OnsitePoints\\Points.sav" ) ) )
                return;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write( "Onsite: Loading Points " );

            BinaryFileReader read = new BinaryFileReader(
                new BinaryReader( new FileStream( Path.Combine( Core.BaseDirectory, "\\Saves\\OnsitePoints\\Points.sav" ),
                FileMode.Open ) )
                );

            GenericReader reader = read;

            InternalLoad( reader );
            read.Close();
            DrawPercent(100);
            Console.WriteLine( "" );
            Console.WriteLine( "Onsite: Load Complete. " );

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void InternalLoad( GenericReader reader )
        {
            int version = reader.ReadInt();

            switch( version )
            {
                case 1:
                    {
                        _Enabled = reader.ReadBool();
                        _Instance._DuelLengthInSeconds = reader.ReadInt();
                        _Instance._MaxDistance = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        int count = reader.ReadInt();

                        for( int i = 0; i < count; i++ )
                        {
                            Serial serial = ( Serial )reader.ReadInt();
                            DuelPoints points = new DuelPoints( reader );
                            _PointsTable.Add( serial, points );
                            DrawPercent( GetPercent( i, count ) );
                        }

                        _DeclineDuelList = reader.ReadStrongMobileList();

                        break;
                    }
            }
        }

        public static bool IsInDuel( Mobile m )
        {
            Duel d;
            return IsInDuel( m, out d );
        }

        public static bool IsInDuel( Mobile m, out Duel duel )
        {
            duel = null;

            if( _DuelTable.ContainsKey( m.Serial ) )
            {
                duel = _DuelTable[m.Serial];
                return true;
            }

            return false;
        }

        private static void EventSink_WorldSave( WorldSaveEventArgs e )
        {
            int oldX, oldY;

            oldX = Console.CursorLeft;
            oldY = Console.CursorTop;

            Console.CursorVisible = false;

            if( !Directory.Exists( Path.Combine( Core.BaseDirectory, "\\Saves\\OnsitePoints" ) ) )
                Directory.CreateDirectory( Path.Combine( Core.BaseDirectory, "\\Saves\\OnsitePoints" ) );

            GenericWriter writer = new BinaryFileWriter( new FileStream( Path.Combine( Core.BaseDirectory, "\\Saves\\OnsitePoints\\Points.sav" ), FileMode.OpenOrCreate ), true );

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write( "Onsite: Saving Points " );
            InternalSave( writer );
            writer.Close();
            DrawPercent(100);
            Console.WriteLine("");
            Console.WriteLine("Onsite: Complete. ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write( "World: " );
        }

        private static void InternalSave( GenericWriter writer )
        {
            writer.Write( ( int )1 );

            writer.Write( ( bool )_Enabled );
            writer.Write( ( int )_Instance._DuelLengthInSeconds );
            writer.Write( ( int )_Instance._MaxDistance );

            int count = _PointsTable.Count;

            writer.Write( ( int )count );
            IDictionaryEnumerator myEnum = _PointsTable.GetEnumerator();

            int i = 0;
            while( myEnum.MoveNext() )
            {
                Serial serial = ( Serial )myEnum.Key;
                DuelPoints duelPoints = ( DuelPoints )myEnum.Value;

                writer.Write( ( int )serial );
                duelPoints.Serialize( writer );

                DrawPercent( GetPercent( i, count ) );
                i++;
            }

            writer.WriteMobileList<Mobile>( _DeclineDuelList );
        }

        public static int GetPercent( long current, long total )
        {
            return ( int )( ( current * 100 ) / total );
        }

        private static void DrawPercent( int percent )
        {
            int oldX = Console.CursorLeft;
            int oldY = Console.CursorTop;
            int totalblocks = 20;
            int blocks = percent / 5;
            int remainingblocks = totalblocks - blocks;
            Console.Write( "[" );

            for( int i = 0; i < blocks; i++ )
                DrawBlock( percent );

            for( int i = 0; i < remainingblocks; i++ )
                Console.Write( "=" );

            Console.Write( "]{0}%", percent );
            Console.SetCursorPosition( oldX, oldY );
        }

        private static void DrawBlock( int percent )
        {
            ConsoleColor oldColor = Console.BackgroundColor;
            Console.BackgroundColor = GetColor( percent );
            Console.Write( " " );
            Console.BackgroundColor = oldColor;
        }

        private static ConsoleColor GetColor( int percent )
        {
            if( percent < 20 )
                return ConsoleColor.DarkRed;
            else if( percent < 40 )
                return ConsoleColor.Red;
            else if( percent < 60 )
                return ConsoleColor.Yellow;
            else if( percent < 80 )
                return ConsoleColor.Green;
            else
                return ConsoleColor.Blue;
        }

        private static int Notoriety_HandleNotoriety( Mobile from, Mobile target )
        {
            if( from == null || target == null )
                return NotorietyHandlers.MobileNotoriety( from, target );

            Duel fromDuel, targetDuel;
            bool fromInDuel = IsInDuel( from, out fromDuel );
            bool targetInDuel = IsInDuel( target, out targetDuel );

            if( fromInDuel && targetInDuel )
            {
                if( fromDuel == null || targetDuel == null )
                    return NotorietyHandlers.MobileNotoriety( from, target );
        
                if( fromDuel == targetDuel )
                {
                    if (fromDuel.Started)
                    {
                        if ((fromDuel.Attackers.Contains(from) && fromDuel.Attackers.Contains(target)) || (fromDuel.Defenders.Contains(from) && fromDuel.Defenders.Contains(target)))
                            return Notoriety.Ally;
                        else
                            return Notoriety.Enemy;
                    }
                    else
                        return NotorietyHandlers.MobileNotoriety(from, target);                    
                }
                else
                    return Notoriety.Invulnerable;
            }
            else if( ( fromInDuel && !targetInDuel ) || ( !fromInDuel && targetInDuel ) )
            {
                if( !target.Player || !from.Player )
                    return NotorietyHandlers.MobileNotoriety( from, target );
                else if( !( target.Region is GuardedRegion ) )
                    return NotorietyHandlers.MobileNotoriety( from, target );
                else
                    if( ( fromInDuel && fromDuel.Started ) || ( targetInDuel && targetDuel.Started ) )
                        return Notoriety.Invulnerable;
                    else
                        return NotorietyHandlers.MobileNotoriety( from, target );
            }
            else
                return NotorietyHandlers.MobileNotoriety( from, target );
        }

        private static bool PlayerMobile_AllowHarmful( Mobile from, Mobile target )
        {
            if( from == null || target == null )
               return NotorietyHandlers.Mobile_AllowHarmful( from, target ); ;

            Duel fromDuel, targetDuel;
            bool fromInDuel = IsInDuel( from, out fromDuel );
            bool targetInDuel = IsInDuel( target, out targetDuel );

            if( fromInDuel && targetInDuel )
            {
                if( fromDuel == null || targetDuel == null )
                    return NotorietyHandlers.Mobile_AllowHarmful( from, target );

                return ( fromDuel == targetDuel );
            }
            else if( ( fromInDuel && !targetInDuel ) || ( targetInDuel && !fromInDuel ) )
                if( from.Player && target.Player )
                    return false;
                
            return NotorietyHandlers.Mobile_AllowHarmful( from, target );
        }

        private static bool PlayerMobile_AllowBenificial( Mobile from, Mobile target )
        {
            if( from == null || target == null )
                return NotorietyHandlers.Mobile_AllowBeneficial( from, target ); ;

            Duel fromDuel, targetDuel;
            bool fromInDuel = IsInDuel( from, out fromDuel );
            bool targetInDuel = IsInDuel( target, out targetDuel );

            if( fromInDuel && targetInDuel )
            {
                if( fromDuel == null || targetDuel == null )
                    return NotorietyHandlers.Mobile_AllowBeneficial( from, target );

                return ( fromDuel == targetDuel );
            }
            else if( ( fromInDuel && !targetInDuel ) || ( targetInDuel && !fromInDuel ) )
                if( from.Player && target.Player )
                    return false;
                
            return NotorietyHandlers.Mobile_AllowHarmful( from, target );
        }

        private static void EventSink_Movement( MovementEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            if( _DuelTable.ContainsKey( m.Serial ) )
            {
                Duel duel = _DuelTable[m.Serial];

                if( duel.Started && duel.CheckDistance() )
                    duel.EndDuel();
            }
        }

        private static void EventSink_Login( LoginEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            if (_TimeoutTable.ContainsKey(m.Serial))
            {
                LogoutTimeoutTimer timer = _TimeoutTable[m.Serial];
                timer.Stop();

                _TimeoutTable.Remove(m.Serial);
            }
        }

        private static void EventSink_Logout( LogoutEventArgs e )
        {            
            Mobile m = e.Mobile;

            if( m == null )
                return;

            Duel duel;

            if (IsInDuel(m, out duel) && !_TimeoutTable.ContainsKey(m.Serial))
            {
                if( duel != null && duel.Started )
                {
                    LogoutTimeoutTimer timer = new LogoutTimeoutTimer(m, duel);
                    timer.Start();

                    _TimeoutTable.Add(m.Serial, timer);
                }
            }
        }

        private static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            if( _DuelTable.ContainsKey( m.Serial ) )
            {
                Duel duel = _DuelTable[m.Serial];
                if( duel.Started )
                    duel.HandleDeath( m );
            }
        }

        internal static void WritePointsDictionary( Dictionary<int, DuelInfo> dictionary, GenericWriter writer )
        {
            writer.Write( ( int )0 );

            int count = dictionary.Count;
            writer.Write( ( int )count );

            IDictionaryEnumerator myEnum = dictionary.GetEnumerator();

            while( myEnum.MoveNext() )
            {
                int key = ( int )myEnum.Key;
                DuelInfo info = ( DuelInfo )myEnum.Value;

                writer.Write( ( int )key );
                info.Serialize( writer );
            }
        }

        internal static void WriteIntDictionary( Dictionary<int, int> dictionary, GenericWriter writer )
        {
            writer.Write( ( int )0 );

            int count = dictionary.Count;
            writer.Write( ( int )count );

            IDictionaryEnumerator myEnum = dictionary.GetEnumerator();

            while( myEnum.MoveNext() )
            {
                int key = ( int )myEnum.Key;
                int info = ( int )myEnum.Value;

                writer.Write( ( int )key );
                writer.Write( ( int )info );
            }
        }

        internal static Dictionary<int, DuelInfo> ReadPointsDictionary( GenericReader reader )
        {
            Dictionary<int, DuelInfo> dictionary = new Dictionary<int, DuelInfo>();

            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        int count = reader.ReadInt();

                        for( int i = 0; i < count; i++ )
                        {
                            int key = reader.ReadInt();
                            DuelInfo dInfo = new DuelInfo( reader );
                            dictionary.Add( key, dInfo );
                        }

                        break;
                    }
            }

            return dictionary;
        }

        internal static Dictionary<int, int> ReadIntDictionary( GenericReader reader )
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        int count = reader.ReadInt();

                        for( int i = 0; i < count; i++ )
                        {
                            int key = reader.ReadInt();
                            int value = reader.ReadInt();
                            dictionary.Add( key, value );
                        }

                        break;
                    }
            }

            return dictionary;
        }

        internal static void DestroyDuel( Duel duel )
        {
            List<Mobile> mobs = new List<Mobile>();

            if( _DuelTable.ContainsKey( duel.Creator.Serial ) )
                _DuelTable.Remove( duel.Creator.Serial );

            if( duel.Attackers != null )
                mobs.AddRange( duel.Attackers );
            if( duel.Defenders != null )
                mobs.AddRange( duel.Defenders );
            if( duel.Contestants != null )
                mobs.AddRange( duel.Contestants );

            for( int i = 0; i < mobs.Count; i++ )
            {
                if( _DuelTable.ContainsKey( mobs[i].Serial ) )
                    _DuelTable.Remove( mobs[i].Serial );
            }

            duel = null;
        }

        internal static void Broadcast( string msg, List<Mobile> broadcastTo )
        {
            if( broadcastTo == null )
                return;

            for( int i = 0; i < broadcastTo.Count; i++ )
                broadcastTo[i].SendMessage( msg );
        }        

        internal static void AddWin( Duel duel, Mobile m )
        {
            if( _PointsTable.ContainsKey( m.Serial ) )
            {
                DuelPoints dPoints = _PointsTable[m.Serial];
                DuelInfo dInfo = new DuelInfo( duel.Attackers.Contains( m ) ? duel.Attackers : duel.Defenders, TimeSpan.FromSeconds( duel.DuelTimer.SecondsRemaining ) );
                dPoints.AddWin( duel.Attackers.Count, dInfo );
                _PointsTable[m.Serial] = dPoints;
            }
            else
            {
                DuelPoints dPoints = new DuelPoints( m );                
                DuelInfo dInfo = new DuelInfo( duel.Attackers.Contains( m ) ? duel.Attackers : duel.Defenders, TimeSpan.FromSeconds( duel.DuelTimer.SecondsRemaining ) );
                dPoints.AddWin( duel.Attackers.Count, dInfo );
                _PointsTable.Add( m.Serial, dPoints );
            }
        }

        internal static void AddLoss( Duel duel, Mobile m )
        {
            if( _PointsTable.ContainsKey( m.Serial ) )
            {
                DuelPoints dPoints = _PointsTable[m.Serial];
                DuelInfo dInfo = new DuelInfo( duel.Attackers.Contains( m ) ? duel.Attackers : duel.Defenders, TimeSpan.FromSeconds( duel.DuelTimer.SecondsRemaining ) );
                dPoints.AddLoss( duel.Attackers.Count, dInfo );
            }
            else
            {
                DuelPoints dPoints = new DuelPoints( m );
                DuelInfo dInfo = new DuelInfo( duel.Attackers.Contains( m ) ? duel.Attackers : duel.Defenders, TimeSpan.FromSeconds(  duel.DuelTimer.SecondsRemaining ) );
                dPoints.AddLoss( duel.Attackers.Count, dInfo );
            }
        }
    }
}
