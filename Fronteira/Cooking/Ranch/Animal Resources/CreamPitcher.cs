using System;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class CreamPitcher : Item
	{
		private int m_Quantity;
		[CommandProperty( AccessLevel.GameMaster )]
		public int Quantity
		{
			get { return m_Quantity; }
			set {	m_Quantity = value; }
		}
		
		private DateTime m_Age = DateTime.UtcNow;
		[ CommandProperty( AccessLevel.GameMaster ) ]
		public DateTime Age
		{
			get { return m_Age; }
			set { m_Age = value; }
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add(GetQuantityDescription());
		}

		public virtual string GetQuantityDescription()
		{
			Weight = 1 + m_Quantity;
			
			int perc = ( m_Quantity * 100 ) / 5;
			if ( perc <= 0 )
				return "Vazio.";
			else if ( perc <= 33 )
				return "Quase vazio.";
			else if ( perc <= 66 )
				return "Quase cheio.";
			else
				return "Cheio.";
		}
		
		public virtual string GetAgeDescription()
		{
			TimeSpan CheckDifference = DateTime.UtcNow - m_Age;
			int agecheck = (int)((CheckDifference.TotalMinutes / 60 / 24 )*12);
			if (agecheck < 1) return "novo ";
			if (agecheck < 2) return "fresco ";
			//if (agecheck < 4) return "";
			if (agecheck > 14) return "azedo ";
			if (agecheck > 10) return "velho ";
			return "";
		}
		
		public virtual void UpdateName()
		{
			if (m_Quantity > 0) Name = "frasco de " + GetAgeDescription() + "creme";
			else Name = "a cream pitcher";
		}
		
		[Constructable]
		public CreamPitcher() : base( 0x9D6 )
		{
			Name = "frasco de creme";
			Weight = 1;
			Hue = 1191;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (this.IsChildOf(from.Backpack))
			{
				
				from.SendMessage("Usar aonde ?");
				from.Target = new CreamTarget(this);
			}
			else from.SendMessage("Tem que estar na mochila pra usar.");
		}

		public CreamPitcher( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write(m_Age);
			writer.Write( (int) m_Quantity);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch (version)
			{
				case 1:
				{
					goto case 0;
				}
				case 0:
				{
					m_Age = reader.ReadDateTime();
					m_Quantity = reader.ReadInt();
					break;
				}
			}
		}

		private class CreamTarget : Target
		{
			private CreamPitcher t_cp;

			public CreamTarget(CreamPitcher cp) : base( 1, false, TargetFlags.None )
			{
				t_cp = cp;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if (targ is MilkKeg)
				{
					if (t_cp.Quantity < 5)
					{
						bool CanUse = from.CheckSkillMult( SkillName.Cooking, 15, 30 );
						if (CanUse)
						{
							MilkKeg bev = (MilkKeg) targ;
							if(bev.Quantity > 10)
							{
								if (t_cp.Quantity < 1) t_cp.Age = bev.Age;
								if (t_cp.Age > bev.Age) t_cp.Age = bev.Age;
								bev.Quantity -= 1;
								t_cp.Quantity += 1;
								t_cp.InvalidateProperties();
								t_cp.UpdateName();
								bev.InvalidateProperties();
								bev.UpdateName();
								from.PlaySound( 0x4E );
								from.SendMessage("Voce tira um pouco de creme.");
							}
							else from.SendMessage("Acabou o creme!");
						}
						else 
						{
							from.SendMessage("Voce galhou em tirar o creme!");
							from.PlaySound( 0x4E );
						}
					}
					else from.SendMessage("Ta cheio !");
				}
				else if (targ is ButterChurn)
				{
					ButterChurn bev = (ButterChurn) targ;
					if(bev.Quantity < 10)
					{
						if (t_cp.Quantity < 10 - bev.Quantity)
						{
							bev.Quantity += t_cp.Quantity;
							t_cp.Quantity = 0;
						}
						else
						{
							t_cp.Quantity -= (10 - bev.Quantity);
							bev.Quantity = 10;
						}	
						t_cp.InvalidateProperties();
						t_cp.UpdateName();
						bev.InvalidateProperties();
						bev.UpdateName();
						from.PlaySound( 0x4E );
						from.SendMessage("Voce colocou o creme na batedeira.");
					}
					else from.SendMessage("Ta cheia !");
				}
				else if (targ is MeasuringCup)
				{
					if (t_cp.Quantity > 0)
					{
						from.AddToBackpack( new Cream());
						from.PlaySound( 0x240 );
						from.SendMessage("Voce mediu um copo de creme.");
						t_cp.Quantity -= 1;
						t_cp.InvalidateProperties();
						t_cp.UpdateName();
					}
					else from.SendMessage("Ta vazio !");
				}
				else
				{
					from.SendMessage ("Voce nao pode fazer isto.");
				}
			}
		}
	}
}
