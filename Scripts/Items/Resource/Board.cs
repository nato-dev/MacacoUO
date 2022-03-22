using System;

namespace Server.Items
{
    [FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class BaseWoodBoard : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set { m_Resource = value; InvalidateProperties(); }
		}

		TextDefinition ICommodity.Description 
		{ 
			get
			{
				if ( m_Resource >= CraftResource.Carvalho && m_Resource <= CraftResource.Mogno )
					return 1075052 + ( (int)m_Resource - (int)CraftResource.Carvalho );

				switch ( m_Resource )
				{
					case CraftResource.Carmesim: return 1075055;
					case CraftResource.Gelo: return 1075056;
					case CraftResource.Eucalipto: return 1075062;	//WHY Osi.  Why?
				}

				return LabelNumber;
			} 
		}

		bool ICommodity.IsDeedable { get { return true; } }

		[Constructable]
		public BaseWoodBoard()
			: this( 1 )
		{
		}

		[Constructable]
		public BaseWoodBoard( int amount )
			: this( CraftResource.Cedro, amount )
		{
		}

		public BaseWoodBoard( Serial serial )
			: base( serial )
		{
		}

		[Constructable]
		public BaseWoodBoard( CraftResource resource ) : this( resource, 1 )
		{
		}

		[Constructable]
		public BaseWoodBoard( CraftResource resource, int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Amount = amount;
            Weight = 0.05;
			m_Resource = resource;
			Hue = CraftResources.GetHue( resource );
		}

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (this.Amount == 1)
                list.Add("Tabua de " + m_Resource);
            else
                list.Add(this.Amount+" Tabuas de " + m_Resource);
        }



        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 4 );

			writer.Write( (int)m_Resource );
		}

		public static bool UpdatingBaseClass;
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			if ( version == 3 )
				UpdatingBaseClass = true;
			switch ( version )
			{
				case 4:
				case 3:
				case 2:
					{
						m_Resource = (CraftResource)reader.ReadInt();
						break;
					}
			}

            Weight = 0.1;

			if ( version <= 1 )
				m_Resource = CraftResource.Cedro;
		}
	}
	
	public class Board : BaseWoodBoard
	{
		[Constructable]
		public Board()
			: this(1)
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Baixa qualidade");
        }

        [Constructable]
		public Board(int amount)
			: base(CraftResource.Cedro, amount)
		{
		}

		public Board(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			if (BaseWoodBoard.UpdatingBaseClass)
				return;
			int version = reader.ReadInt();
		}
	}

    public class HeartwoodBoard : BaseWoodBoard
    {
        [Constructable]
        public HeartwoodBoard()
            : this(1)
        {
        }

        [Constructable]
        public HeartwoodBoard(int amount)
            : base(CraftResource.Eucalipto, amount)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Madeira muito boa para construir barcos e baus");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Boa para barcos e baus");
        }

        public HeartwoodBoard(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BloodwoodBoard : BaseWoodBoard
    {
        [Constructable]
        public BloodwoodBoard()
            : this(1)
        {
        }

        [Constructable]
        public BloodwoodBoard(int amount)
            : base(CraftResource.Carmesim, amount)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Madeira muito boa para instrumentos e arcos");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Boa para instrumentos e arcos");
        }

        public BloodwoodBoard(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class FrostwoodBoard : BaseWoodBoard
    {
        [Constructable]
        public FrostwoodBoard()
            : this(1)
        {
        }

        [Constructable]
        public FrostwoodBoard(int amount)
            : base(CraftResource.Gelo, amount)
        {
        }

        public FrostwoodBoard(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Madeira clara, conhecida por fabricar cajados de qualidade magnifica.");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Boa para cajados");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class OakBoard : BaseWoodBoard
    {
        [Constructable]
        public OakBoard()
            : this(1)
        {
        }

        [Constructable]
        public OakBoard(int amount)
            : base(CraftResource.Carvalho, amount)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Madeira decente pra tudo.");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Boa pra tudo.");
        }

        public OakBoard(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class AshBoard : BaseWoodBoard
    {
        [Constructable]
        public AshBoard()
            : this(1)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Madeira decente pra tudo.");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Boa pra tudo.");
        }

        [Constructable]
        public AshBoard(int amount)
            : base(CraftResource.Pinho, amount)
        {
        }

        public AshBoard(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class YewBoard : BaseWoodBoard
    {
        [Constructable]
        public YewBoard()
            : this(1)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Madeira muito boa para instrumentos e moveis");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Boa para instrumentos e moveis");
        }

        [Constructable]
        public YewBoard(int amount)
            : base(CraftResource.Mogno, amount)
        {
        }

        public YewBoard(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
