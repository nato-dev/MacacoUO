using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute( 0x18E7, 0x18E8 )]
	public class GinsengUprooted : Item, ICarvable
	{
        public virtual Item GetCarved { get { return new Ginseng(); } }
        public virtual int GetCarvedAmount { get { return Utility.Random(3); ; } }

        public bool Carve(Mobile from, Item item)
        {
            if (Parent is ShippingCrate && !((ShippingCrate)Parent).CheckCarve(this))
            {
                return false;
            }

            Item newItem = GetCarved;

            if (newItem == null)
            {
                newItem = new Ginseng();
            }

            if (newItem != null && HasSocket<Caddellite>())
            {
                newItem.AttachSocket(new Caddellite());
            }

            int count = Utility.Random(3);
            if (newItem != null)
            {
                if (count == 0)
                {
                    from.SendMessage("Voce nao encontrou nada usavel.");
                    this.Consume();
                }
                else
                {
                    base.ScissorHelper(from, newItem, GetCarvedAmount);
                    from.SendMessage("Voce cortou {0} root{1}.", count, (count == 1 ? "" : "s"));
                }
            }

            return true;
        }

		[Constructable]
		public GinsengUprooted() : this( 1 )
		{
		}

		[Constructable]
		public GinsengUprooted( int amount ) : base( Utility.RandomList( 0x18EB, 0x18EC ) )
		{
			Stackable = false;
			Weight = 1.0;
			
			Movable = true; 
			Amount = amount;

			Name = "Uprooted Ginseng Plant";
		}


		public GinsengUprooted( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
