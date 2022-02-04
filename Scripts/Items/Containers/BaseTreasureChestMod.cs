// Treasure Chest Pack - Version 0.99H
// By Nerun

using Server;
using Server.Items;
using Server.Network;
using System;
using System.Collections.Generic;

namespace Server.Items
{
	public abstract class BaseTreasureChestMod : LockableContainer
	{
        public static List<BaseTreasureChestMod> Tesouros = new List<BaseTreasureChestMod>();

        public static Dictionary<Region, List<BaseTreasureChestMod>> ByRegion = new Dictionary<Region, List<BaseTreasureChestMod>>(); 

        private ChestTimer m_DeleteTimer;
		//public override bool Decays { get{ return true; } }
		//public override TimeSpan DecayTime{ get{ return TimeSpan.FromMinutes( Utility.Random( 10, 15 ) ); } }
		public override int DefaultGumpID{ get{ return 0x42; } }
		public override int DefaultDropSound{ get{ return 0x42; } }
		public override Rectangle2D Bounds{ get{ return new Rectangle2D( 20, 105, 150, 180 ); } }
		public override bool IsDecoContainer{get{ return false; }}
        public abstract int GetLevel();

        public override void LockPick(Mobile from)
        {
            base.LockPick(from);
            var xp = 50 + (GetLevel() * 200);
            if(from.RP)
            {
                foreach (var i in this.Items)
                    i.RP = true;
            }
        }

        public BaseTreasureChestMod( int itemID ) : base ( itemID )
		{
            Name = "Tesouro nivel "+GetLevel();
			Locked = true;
			Movable = false;
            Tesouros.Add(this);
			Key key = (Key)FindItemByType( typeof(Key) );

			if( key != null )
				key.Delete();

            if(Core.SA)
                RefinementComponent.Roll(this, 1, 0.08);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () => {
                var r = this.GetRegion();
                if(r != null)
                {
                    if (!ByRegion.ContainsKey(r))
                        ByRegion.Add(r, new List<BaseTreasureChestMod>());
                    ByRegion[r].Add(this);
                }

            });
		}

		public BaseTreasureChestMod( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

        public override void OnDelete()
        {
            var r = this.GetRegion();
            if (r != null)
            {
                if (ByRegion.ContainsKey(r))
                    ByRegion[r].Remove(this);
            }
            Tesouros.Remove(this);
            base.OnDelete();
        }

        public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( !Locked )
				StartDeleteTimer();

            Tesouros.Add(this);
		}

		public override void OnTelekinesis( Mobile from )
		{
			if ( CheckLocked( from ) )
			{
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5022 );
				Effects.PlaySound( Location, Map, 0x1F5 );
				return;
			}

			base.OnTelekinesis( from );
			Name = "um bau de tesouro";
			StartDeleteTimer();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( CheckLocked( from ) )
				return;

			base.OnDoubleClick( from );
			Name = "um bau de tesouro";
			StartDeleteTimer();
		}

        protected void AddLoot(Item item)
        {
            if (item == null)
                return;

            if (Core.SA && RandomItemGenerator.Enabled)
            {
                int min, max;
                TreasureMapChest.GetRandomItemStat(out min, out max);

                RunicReforging.GenerateRandomItem(item, 0, min, max);
            }

            DropItem(item);
        }

		private void StartDeleteTimer()
		{
			if( m_DeleteTimer == null )
				m_DeleteTimer = new ChestTimer( this );
			else
				m_DeleteTimer.Delay = TimeSpan.FromSeconds( Utility.Random( 1, 2 ));
				
			m_DeleteTimer.Start();
		}

		private class ChestTimer : Timer
		{
			private BaseTreasureChestMod m_Chest;
			
			public ChestTimer( BaseTreasureChestMod chest ) : base ( TimeSpan.FromMinutes( Utility.Random( 2, 5 ) ) )
			{
				m_Chest = chest;
				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				m_Chest.Delete();
			}
		}
	}
}
