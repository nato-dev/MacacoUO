using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Collections;

namespace Server.Items
{
	[Flipable( 0x14F0, 0x14EF )]
	public class RanchExtensionDeed : Item
	{
		[Constructable]
		public RanchExtensionDeed() : base( 0x14F0)
		{
			Name = "escritura para extensao de rancho";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (this.IsChildOf( from.Backpack ))
			{
				from.SendMessage("Selecione uma pedra de rancho.");
				from.Target = new RanchTarget(this);
			}
			else from.SendMessage("Na mochila por favor");
		}

		private class RanchTarget : Target
		{
			private RanchExtensionDeed t_re;

			public RanchTarget(RanchExtensionDeed d) : base( 10, false, TargetFlags.None )
			{
				t_re = d;
			}
	
			protected override void OnTarget( Mobile from, object targ )
			{
				if (targ is RanchStone)
				{
					RanchStone rs = (RanchStone) targ;
					if ( from.InRange( rs.GetWorldLocation(), 2 ) )
					{
						if (rs.Owner == from)
						{
							if ((rs.Size + 5) < 50) 
							{
								t_re.Delete();
								rs.Size += 5;
								rs.InvalidateProperties();
								from.SendMessage("Seu rancho aumentou.");
							}
							else from.SendMessage("Seu rancho ja ta no tamanho maximo!");
						}
						else from.SendMessage("Voce nao eh dono desse rancho !");
					}
					else
						from.SendMessage("Muito longe!");
				}
			}
		}

		public RanchExtensionDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
