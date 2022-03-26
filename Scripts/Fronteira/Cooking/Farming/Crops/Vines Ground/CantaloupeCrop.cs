using System;
using System.Collections;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
namespace Server.Items.Crops
{
	public class CantaloupeSeed : HerdingBaseCrop
	{
		public override bool CanGrowGarden{ get{ return true; } }

		[Constructable]
		public CantaloupeSeed() : this( 1 ) { }

		[Constructable]
		public CantaloupeSeed( int amount ) : base( 0xF27 )
		{
			Stackable = true;
			Weight = .1;
			Hue = 0x5E2;
			Movable = true;
			Amount = amount;
			Name = "Cantaloupe Seed";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.Mounted && !CropHelper.CanWorkMounted ) { from.SendMessage( "Voce nao pode plantar estando montado." ); return; }
			Point3D m_pnt = from.Location;
			Map m_map = from.Map;
			if ( !IsChildOf( from.Backpack ) ) { from.SendLocalizedMessage( 1042010 ); return; }
			else if ( !CropHelper.CheckCanGrow( this, m_map, m_pnt.X, m_pnt.Y ) ) { from.SendMessage( "Esta semente nao vai crescer aqui. Talvez esta semente precise ser plantada em um jardim em casa, rancho ou fazenda.." ); return; }
			ArrayList cropshere = CropHelper.CheckCrop( m_pnt, m_map, 0 );
			if ( cropshere.Count > 0 ) { from.SendMessage( "Ja existe uma plantacao aqui." ); return; }
			ArrayList cropsnear = CropHelper.CheckCrop( m_pnt, m_map, 1 );
			if ( ( cropsnear.Count > 1 ) ) { from.SendMessage( "Muitas plantacoes perto." ); return; }
			if ( this.BumpZ ) ++m_pnt.Z;
			if ( !from.Mounted ) from.Animate( 32, 5, 1, true, false, 0 );
			from.SendMessage("Voce plantou a semente.");
			this.Consume();
			Item item = new CantaloupeSeedling( from );
			item.Location = m_pnt;
			item.Map = m_map;
		}

		public CantaloupeSeed( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}

	public class CantaloupeSeedling : HerdingBaseCrop
	{
		private static Mobile m_sower;
		public Timer thisTimer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }

		[Constructable]
		public CantaloupeSeedling( Mobile sower ) : base( 0xC61 )
		{
			Movable = false;
			Name = "Cantaloupe Seedling";
			m_sower = sower;
			init( this );
		}
		public static void init( CantaloupeSeedling plant )
		{
			plant.thisTimer = new CropHelper.GrowTimer( plant, typeof(CantaloupeCrop), plant.Sower );
			plant.thisTimer.Start();
		}
		public override void OnDoubleClick( Mobile from )
		{
			if ( from.Mounted && !CropHelper.CanWorkMounted ) { from.SendMessage( "Esta planta eh muito pequena para ser colhida montado." ); return; }
			else from.SendMessage( "Esta plantacao ainda e muito jovem." );
		}
		public CantaloupeSeedling( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); writer.Write( m_sower ); }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_sower = reader.ReadMobile();
			init( this );
		}
	}

	public class CantaloupeCrop : HerdingBaseCrop
	{
		private const int max = 2;
		private int fullGraphic;
		private int pickedGraphic;
		private DateTime lastpicked;
		private Mobile m_sower;
		private int m_yield;
		public Timer regrowTimer;
		private DateTime m_lastvisit;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastSowerVisit{ get{ return m_lastvisit; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Growing{ get{ return regrowTimer.Running; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Yield{ get{ return m_yield; } set{ m_yield = value; } }

		public int Capacity{ get{ return max; } }
		public int FullGraphic{ get{ return fullGraphic; } set{ fullGraphic = value; } }
		public int PickGraphic{ get{ return pickedGraphic; } set{ pickedGraphic = value; } }
		public DateTime LastPick{ get{ return lastpicked; } set{ lastpicked = value; } }

		[Constructable]
		public CantaloupeCrop() : this(null) { }

		[Constructable]
		public CantaloupeCrop( Mobile sower ) : base( 15374 )
		{
			Movable = false;
			Name = "Cantaloupe Plant";
			Hue = 2983;
			m_sower = sower;
			m_lastvisit = DateTime.UtcNow;
			init( this, false );
		}

		public static void init ( CantaloupeCrop plant, bool full )
		{
			plant.PickGraphic = ( 15374 );
			plant.FullGraphic = ( 15374 );
			plant.LastPick = DateTime.UtcNow;
			plant.regrowTimer = new CropTimer( plant );
			if ( full ) { plant.Yield = plant.Capacity; ((Item)plant).ItemID = plant.FullGraphic; }
			else { plant.Yield = 0; ((Item)plant).ItemID = plant.PickGraphic; plant.regrowTimer.Start(); }
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_sower == null || m_sower.Deleted ) m_sower = from;
			if ( from != m_sower ) { from.SendMessage( "Esta planta nao e sua !!!" ); return; }

			if ( from.Mounted && !CropHelper.CanWorkMounted ) { from.SendMessage( "You cannot harvest a crop while mounted." ); return; }
			if ( DateTime.UtcNow > lastpicked.AddSeconds(3) )
			{
				lastpicked = DateTime.UtcNow;
				int cookValue = (int)from.Skills[SkillName.Herding].Value / 20;
				if ( cookValue == 0 ) { from.SendMessage( "Voce nao tem skill suficiente para colher esta planta." ); return; }
				if ( from.InRange( this.GetWorldLocation(), 1 ) )
				{
					if ( m_yield < 1 ) { from.SendMessage( "Nao tem nada aqui para ser colhido." ); }
					else
					{
						from.Direction = from.GetDirectionTo( this );
						from.Animate( from.Mounted ? 29:32, 5, 1, true, false, 0 );
						m_lastvisit = DateTime.UtcNow;
						if ( cookValue > m_yield ) cookValue = m_yield + 1;
						int pick = Utility.RandomMinMax( cookValue - 4, cookValue );
						if (pick < 0 ) pick = 0;
						if ( pick == 0 ) { from.SendMessage( "Voce nao conseguiu colher nada." ); return; }
						m_yield -= pick;
						from.SendMessage( "Voce colheu {0} item{1}!", pick, ( pick == 1 ? "" : "s" ) );
						if (m_yield < 1) ((Item)this).ItemID = pickedGraphic;
						Cantaloupe crop = new Cantaloupe( pick );
						from.AddToBackpack( crop );
						if ( !regrowTimer.Running ) { regrowTimer.Start(); }
					}
				}
				else { from.SendMessage( "Voce esta muito longe para colher." ); }
			}
		}

		private class CropTimer : Timer
		{
			private CantaloupeCrop i_plant;
			public CropTimer( CantaloupeCrop plant ) : base( TimeSpan.FromSeconds( 450 ), TimeSpan.FromSeconds( 15 ) )
			{
				Priority = TimerPriority.OneSecond;
				i_plant = plant;
			}
			protected override void OnTick()
			{
				if ( Utility.RandomBool() )
				{
					if ( ( i_plant != null ) && ( !i_plant.Deleted ) )
					{
						int current = i_plant.Yield;
						if ( ++current >= i_plant.Capacity )
						{
							current = i_plant.Capacity;
							((Item)i_plant).ItemID = i_plant.FullGraphic;
							Stop();
						}
						else if ( current <= 0 ) current = 1;
						i_plant.Yield = current;
					}
					else Stop();
				}
			}
		}

		public override void OnChop( Mobile from )
		{
			if ( !this.Deleted ) Chop( from );
		}

		public void Chop( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				if ( from == m_sower )
				{
					from.Direction = from.GetDirectionTo( this );
					double lumberValue = from.Skills[SkillName.Lumberjacking].Value / 100;
					if ( ( lumberValue > .5 ) && ( Utility.RandomDouble() <= lumberValue ) )
					{
						Cantaloupe fruit = new Cantaloupe( Utility.Random( m_yield +2 ) );
						from.AddToBackpack( fruit );
						if (Utility.RandomDouble() <= .1 ) { Vines logs = new Vines(); from.AddToBackpack( logs ); }
					}
						this.Delete();
						from.SendMessage( "Voce cortou a planta" );
				}
				else from.SendMessage( "Esta planta nao e sua !!!" );
			}
			else from.SendLocalizedMessage( 500446 );
		}

		public CantaloupeCrop( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( m_lastvisit );
			writer.Write( m_sower );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_lastvisit = reader.ReadDateTime();
			m_sower = reader.ReadMobile();
			init( this, true );
		}
	}
}
