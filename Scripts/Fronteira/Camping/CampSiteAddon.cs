
using System;
using Server;
using Server.Items;
using Server.Multis;

namespace Server.Items
{
	public class CampSiteAddon : BaseAddon {

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Com a skill camping talvez voce possa acampar aqui e salvar este ponto de acampamento");
        }

        public string Nome { get; set; }
		public override BaseAddonDeed Deed{get{return new CampSiteAddonDeed();}}
		[ Constructable ]
		public CampSiteAddon(string nome)
		{


			AddComponent(new CampComponent(13001), 3, -1, 0 );
			AddComponent(new CampComponent(3315), 3, -1, 0 );

			AddComponent(new CampComponent(3319), 3, -2, 0 );
			AddComponent(new CampComponent(13001), 3, -2, 0 );
			AddComponent(new CampComponent(13001), 2, -1, 0 );
			AddComponent(new CampComponent(13001), 2, -2, 0 );

			AddComponent(new CampComponent(3318), 2, -2, 0 );

			AddComponent(new CampComponent(13001), 1, -1, 0 );


			AddComponent(new CampComponent(13001), 1, -2, 0 );

			AddComponent(new CampComponent(13001), 0, -1, 0 );

			AddComponent(new CampComponent(3319), 0, -2, 0 );


			AddComponent(new CampComponent(13001), 0, -2, 0 );

			AddComponent(new CampComponent(13001), -1, -1, 0 );

			AddComponent(new CampComponent(3318), -1, -2, 0 );

			AddComponent(new CampComponent(13001), -1, -2, 0 );


			AddComponent(new CampComponent(3316), -2, -1, 0 );

			AddComponent(new CampComponent(13001), -2, -1, 0 );

			AddComponent(new CampComponent(3315), -2, -2, 0 );

			AddComponent(new CampComponent(13001), -2, -2, 0 );

			AddComponent(new CampComponent(3316), 3, 2, 0 );

			AddComponent(new CampComponent(13001), 3, 2, 0 );

			AddComponent(new CampComponent(13001), 3, 1, 0 );

			AddComponent(new CampComponent(3315), 3, 1, 0 );

			AddComponent(new CampComponent(3316), 3, 0, 0 );

			AddComponent(new CampComponent(13001), 3, 0, 0 );

			AddComponent(new CampComponent(13001), 2, 2, 0 );

			AddComponent(new CampComponent(3319), 2, 2, 0 );

			AddComponent(new CampComponent(13001), 2, 1, 0 );

			AddComponent(new CampComponent(13001), 2, 0, 0 );

			AddComponent(new CampComponent(13001), 1, 2, 0 );

			AddComponent(new CampComponent(3318), 1, 2, 0 );

			AddComponent(new CampComponent(13001), 1, 1, 0 );

			AddComponent(new CampComponent(13001), 1, 0, 0 );

			//ac = new AddonComponent( 10749 );
			//ac.Light = LightType.ArchedWindowEast;
			//AddComponent( ac, 1, 0, 0 );


			AddComponent(new CampComponent(3319), 0, 2, 0 );


			AddComponent(new CampComponent(13001), 0, 2, 0 );

	
			AddComponent(new CampComponent(13001), 0, 1, 0 );


			AddComponent(new CampComponent(13001), 0, 0, 0 );

			AddComponent(new CampComponent(3318), -1, 2, 0 );

			AddComponent(new CampComponent(13001), -1, 2, 0 );


			AddComponent(new CampComponent(13001), -1, 1, 0 );

	
			AddComponent(new CampComponent(13001), -1, 0, 0 );

	
			AddComponent(new CampComponent(13001), -2, 2, 0 );

			AddComponent(new CampComponent(3316), -2, 1, 0 );


			AddComponent(new CampComponent(13001), -2, 1, 0 );

			AddComponent(new CampComponent(3315), -2, 0, 0 );

			AddComponent(new CampComponent(13001), -2, 0, 0 );

            Acampamento.Points[nome] = this;
            this.Nome = nome;
		}
		public CampSiteAddon( Serial serial ) : base( serial ){}
		public override void Serialize( GenericWriter writer ){
            base.Serialize( writer );
            writer.Write( 0 );
            writer.Write(Nome);
        }
		public override void Deserialize( GenericReader reader ){
            base.Deserialize( reader );
            int v = reader.ReadInt();
            Nome = reader.ReadString();
            Acampamento.Points[Nome] = this;
        }

        public override void OnAfterDelete()
        {
            Shard.Debug("Camp Deletado");
            Acampamento.Points.Remove(Nome);
            base.OnAfterDelete();
        }
    }

	public class CampSiteAddonDeed : BaseAddonDeed {
		public override BaseAddon Addon{get{return new CampSiteAddon("Sem Nome");}}
		[Constructable]
		public CampSiteAddonDeed(){Name = "CampSite";}
		public CampSiteAddonDeed( Serial serial ) : base( serial ){}
		public override void Serialize( GenericWriter writer ){	base.Serialize( writer );writer.Write( 0 );}
		public override void	Deserialize( GenericReader reader )	{base.Deserialize( reader );reader.ReadInt();}
	}

    public class CampComponent : AddonComponent
    {


        public CampComponent(int itemid) : base(itemid)
        {
          
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Com a skill camping talvez voce possa acampar aqui e salvar este ponto de acampamento");
        }


        public CampComponent(Serial serial) : base(serial)
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
