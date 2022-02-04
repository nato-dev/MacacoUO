//Cows, Goats, and Sheep don't just make milk all the time (unlike chickens and eggs).
//They won't produce milk unless they've given birth.  Once they've started producing
//milk, they will continue to produce milk until the calf is weined.  Farmers have
//capitalized on this fact and continue to milk the cows, goats, sheep, long after the
//offspring is weined thus fooling their bodies into thinking they still need to produce milk.
//As such, the Cows, Sheep, and eventually Goat, have been written to only produce milk
//after they've given birth, and to continue only until their offspring turns to a Young
//animal (between Baby and Adult) AND they haven't been milked for (I think) 7 days straight.
using System;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Prompts;
using Server.Multis;

namespace Server.Items
{
	public enum TypeOfMilk
	{
		None,
		Cow,
		Sheep,
		Goat,
		Mixed
	}
	
	public class MilkingBucket : Item
	{
		private int m_Quantity;
		[CommandProperty( AccessLevel.GameMaster )]
		public int Quantity
		{
			get { return m_Quantity; }
			set {	m_Quantity = value; }
		}
		
		private TypeOfMilk m_MilkType;
		[CommandProperty( AccessLevel.GameMaster )]
		public TypeOfMilk MilkType
		{
			get { return m_MilkType; }
			set {	m_MilkType = value; }
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
			Weight = 2 + m_Quantity;
			if (m_Quantity < 1) m_MilkType = TypeOfMilk.None;
			
			int perc = ( m_Quantity * 100 ) / 10;
			if ( perc <= 0 )
				return "Vazio.";
			else if ( perc <= 33 )
				return "Quase vazio.";
			else if ( perc <= 66 )
				return "Metade cheio.";
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
			if (m_Quantity > 0) Name = "balde leiteiro " + GetAgeDescription() + " [" + m_MilkType + "]";
			else Name = "balde leiteiro";
		}
		
		[Constructable]
		public MilkingBucket() : base( 0x14E0 )
		{
			Name = "balde leiteiro";
			Weight = 2;
			//Hue = 1257;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (this.IsChildOf(from.Backpack))
			{
				from.SendMessage("Pegar leite de que animal ?");
				from.Target = new MilkingTarget(this);
			}
			else from.SendMessage("Mochila por favor...");
		}

		public MilkingBucket( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			
			writer.Write(m_Age);
			
			writer.Write( (int) m_Quantity);
			writer.Write( (int) m_MilkType);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			switch (version)
			{
				case 1:
				{
					m_Age = reader.ReadDateTime();
					goto case 0;
				}
				case 0:
				{
					m_Quantity = reader.ReadInt();
					m_MilkType = (TypeOfMilk) reader.ReadInt();
					break;
				}
			}
		}

		private class MilkingTarget : Target
		{
			private MilkingBucket t_mb;

			public MilkingTarget(MilkingBucket mb) : base( 1, false, TargetFlags.None )
			{
				t_mb = mb;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if (targ is FarmCow)
				{
					FarmCow c = (FarmCow) targ;
					if (c.Owner == from || c.Owner == null)
					{
						bool CanUse = from.CheckSkillMult( SkillName.AnimalLore, 20, 50 );
						if (CanUse)
						{
							if (c.Milk > 0)
							{
								if (t_mb.Quantity < 10)
								{
									c.PlaySound(c.GetIdleSound());
									from.SendMessage("Voce tira leite da vaca.");
									if (t_mb.Quantity < 1) t_mb.Age = DateTime.UtcNow;
									int tofill = 10 - t_mb.Quantity;
									if (c.Milk > tofill)
									{
										t_mb.Quantity = 10;
										c.Milk -= tofill;
										if (t_mb.MilkType != TypeOfMilk.Cow && t_mb.MilkType != TypeOfMilk.None) t_mb.MilkType = TypeOfMilk.Mixed;
										else t_mb.MilkType = TypeOfMilk.Cow;
									}
									else 
									{
										t_mb.Quantity += c.Milk;
										c.Milk = 0;
										if (t_mb.MilkType != TypeOfMilk.Cow && t_mb.MilkType != TypeOfMilk.None) t_mb.MilkType = TypeOfMilk.Mixed;
										else t_mb.MilkType = TypeOfMilk.Cow;
									}
									t_mb.InvalidateProperties();
									t_mb.UpdateName();
								}
								else from.SendMessage("Seu balde esta cheio!");
							}
							else from.SendMessage("O animal nao tem mais leite.");
						}
						else
						{
							c.PlaySound(c.GetIdleSound());
							from.SendMessage("Voce falhou em tirar leite da vaca!");
						}
					}
					else from.SendMessage("Voce nao eh o dono do animal.");
				}
				else if (targ is FarmSheep)
				{
					FarmSheep c = (FarmSheep) targ;
					if (c.Owner == from || c.Owner == null)
					{
						bool CanUse = from.CheckSkillMult( SkillName.AnimalLore, 20, 50 );
						if (CanUse)
						{
							if (c.Milk > 0)
							{
								if (t_mb.Quantity < 10)
								{
									c.PlaySound(c.GetIdleSound());
									from.SendMessage("Voce tirou leite da ovelha.");
									if (t_mb.Quantity < 1) t_mb.Age = DateTime.UtcNow;
									int tofill = 10 - t_mb.Quantity;
									if (c.Milk > tofill)
									{
										t_mb.Quantity = 10;
										c.Milk -= tofill;
										if (t_mb.MilkType != TypeOfMilk.Sheep && t_mb.MilkType != TypeOfMilk.None) t_mb.MilkType = TypeOfMilk.Mixed;
										else t_mb.MilkType = TypeOfMilk.Sheep;
									}
									else
									{
										t_mb.Quantity += c.Milk;
										c.Milk = 0;
										if (t_mb.MilkType != TypeOfMilk.Sheep && t_mb.MilkType != TypeOfMilk.None) t_mb.MilkType = TypeOfMilk.Mixed;
										else t_mb.MilkType = TypeOfMilk.Sheep;
									}
									t_mb.InvalidateProperties();
									t_mb.UpdateName();
								}
								else from.SendMessage("Seu balde de leite esta cheio!");
							}
							else from.SendMessage("O animal esta sem leite.");
						}
						else
						{
							c.PlaySound(c.GetIdleSound());
							from.SendMessage("Voce falhou em tirar leite do animal!");
						}
					}
					else from.SendMessage("O animal nao eh seu.");
				}
				else if (targ is FarmGoat)
				{
					FarmGoat c = (FarmGoat) targ;
					if (c.Owner == from || c.Owner == null)
					{
						bool CanUse = from.CheckSkillMult( SkillName.AnimalLore, 20, 50 );
						if (CanUse)
						{
							if (c.Milk > 0)
							{
								if (t_mb.Quantity < 10)
								{
									c.PlaySound(c.GetIdleSound());
									from.SendMessage("Voce tirou leite do animal.");
									if (t_mb.Quantity < 1) t_mb.Age = DateTime.UtcNow;
									int tofill = 10 - t_mb.Quantity;
									if (c.Milk > tofill)
									{
										t_mb.Quantity = 10;
										c.Milk -= tofill;
										if (t_mb.MilkType != TypeOfMilk.Goat && t_mb.MilkType != TypeOfMilk.None) t_mb.MilkType = TypeOfMilk.Mixed;
										else t_mb.MilkType = TypeOfMilk.Goat;
									}
									else
									{
										t_mb.Quantity += c.Milk;
										c.Milk = 0;
										if (t_mb.MilkType != TypeOfMilk.Goat && t_mb.MilkType != TypeOfMilk.None) t_mb.MilkType = TypeOfMilk.Mixed;
										else t_mb.MilkType = TypeOfMilk.Goat;
									}
									t_mb.InvalidateProperties();
									t_mb.UpdateName();
								}
								else from.SendMessage("Balde cheio!");
							}
							else from.SendMessage("O animal nao tem mais leite.");
						}
						else
						{
							c.PlaySound(c.GetIdleSound());
							from.SendMessage("Voce falho ao tirar leite do animal !");
						}
					}
					else from.SendMessage("Voce nao eh dono do animal.");
				}
				else if (targ is Pitcher)
				{
					Pitcher bev = (Pitcher) targ;
					if (bev.Content == BeverageType.Milk || bev.Quantity < 1)
					{
						bev.Content = BeverageType.Milk;
						if ( t_mb.Quantity > (bev.MaxQuantity - bev.Quantity))
						{
							t_mb.Quantity -= (bev.MaxQuantity - bev.Quantity);
							bev.Quantity = bev.MaxQuantity;
						}
						else
						{
							bev.Quantity += t_mb.Quantity;
							t_mb.Quantity = 0;
						}
						t_mb.InvalidateProperties();
						t_mb.UpdateName();
						from.PlaySound( 0x4E );
					}
					else from.SendMessage("Voce nao deveria misturar as coisas.");
				}
				else if (targ is MilkKeg)
				{
					MilkKeg bev = (MilkKeg) targ;
					if(bev.Quantity < 100)
					{
						if (bev.MilkType == t_mb.MilkType || bev.Quantity < 1)
						{
							bev.MilkType = t_mb.MilkType;
							if (bev.Quantity < 1) bev.Age = t_mb.Age;
							if (bev.Age > t_mb.Age) bev.Age = t_mb.Age;
							if ( t_mb.Quantity > (100 - bev.Quantity))
							{
								t_mb.Quantity -= (100 - bev.Quantity);
								bev.Quantity = 100;
							}
							else
							{
								bev.Quantity += t_mb.Quantity;
								t_mb.Quantity = 0;
							}
							t_mb.InvalidateProperties();
							t_mb.UpdateName();
							bev.InvalidateProperties();
							bev.UpdateName();
							from.PlaySound( 0x4E );
							from.SendMessage("Voce colocou leite no barril.");
						}
						else
						{
							bev.MilkType = TypeOfMilk.Mixed;
							if (bev.Quantity < 1) bev.Age = t_mb.Age;
							if (bev.Age > t_mb.Age) bev.Age = t_mb.Age;
							if ( t_mb.Quantity > (100 - bev.Quantity))
							{
								t_mb.Quantity -= (100 - bev.Quantity);
								bev.Quantity = 100;
							}
							else
							{
								bev.Quantity += t_mb.Quantity;
								t_mb.Quantity = 0;
							}
							t_mb.InvalidateProperties();
							t_mb.UpdateName();
							bev.InvalidateProperties();
							bev.UpdateName();
							from.PlaySound( 0x4E );
							from.SendMessage("Voce colocou leite no barril.");
						}
					}
					else from.SendMessage("Cheio!");
				}
				else if (targ is MeasuringCup)
				{
					if (t_mb.Quantity > 0)
					{
						from.AddToBackpack( new Milk());
						from.PlaySound( 0x240 );
						from.SendMessage("Voce mediu um copo de leite.");
						t_mb.Quantity -= 1;
						t_mb.InvalidateProperties();
						t_mb.UpdateName();
					}
					else from.SendMessage("O balde esta vaziu!");
				}
				else
				{
					from.SendMessage ("Voce nao pode fazer isto.");
				}
			}
		}
	}
}
