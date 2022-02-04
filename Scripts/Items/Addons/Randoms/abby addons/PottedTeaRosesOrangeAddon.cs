
////////////////////////////////////////
//                                    //
//   Generated by CEO's YAAAG - V1.2  //
// (Yet Another Arya Addon Generator) //
//                                    //
////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PottedTeaRosesOrangeAddon : BaseAddon
	{
         
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new PottedTeaRosesOrangeAddonDeed();
			}
		}

		[ Constructable ]
		public PottedTeaRosesOrangeAddon()
		{



			AddComplexComponent( (BaseAddon) this, 3348, 0, 0, 6, 1161, -1, "tea roses", 1);// 1
			AddComplexComponent( (BaseAddon) this, 3332, 0, 0, 3, 0, -1, "leaves", 1);// 2
			AddComplexComponent( (BaseAddon) this, 3345, 0, 0, 4, 1161, -1, "tea roses", 1);// 3
			AddComplexComponent( (BaseAddon) this, 4551, 0, 0, 0, 956, -1, "", 1);// 4

		}

		public PottedTeaRosesOrangeAddon( Serial serial ) : base( serial )
		{
		}

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType) lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class PottedTeaRosesOrangeAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new PottedTeaRosesOrangeAddon();
			}
		}

		[Constructable]
		public PottedTeaRosesOrangeAddonDeed()
		{
			Name = "PottedTeaRosesOrange";
		}

		public PottedTeaRosesOrangeAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}