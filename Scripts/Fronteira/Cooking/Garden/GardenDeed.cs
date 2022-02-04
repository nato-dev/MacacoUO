using System; 
using Server.Network; 
using Server.Prompts; 
using Server.Items; 
using Server.Mobiles; 
using Server.Gumps;
using Server.Misc;

namespace Server.Items
{
    public class GardenDeed : Item
    {
        [Constructable]
        public GardenDeed()
            : base(3720) //pitchfork
        {
            Name = "Ferramenta de Jardinagem";
            Hue = 1164;
            Weight = 50.0;
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (GardenCheck(from) == false)
            {
                from.SendMessage("Voce ja tem um jardim.");
            }
            else
            {

                if (IsChildOf(from.Backpack))
                {
                    if (Validate(from) == true)
                    {
                        GardenFence v = new GardenFence();
                        v.Location = from.Location;
                        v.Map = from.Map;

                        GardenGround y = new GardenGround();
                        y.Location = from.Location;
                        y.Map = from.Map;

                        GardenVerifier gardenverifier = new GardenVerifier();
                        from.AddToBackpack(gardenverifier);

                        SecureGarden securegarden = new SecureGarden((PlayerMobile)from);
                        securegarden.Location = new Point3D(from.X - 1, from.Y - 2, from.Z);
                        securegarden.Map = from.Map;

                        GardenDestroyer x = new GardenDestroyer(v, y, (PlayerMobile)from, (SecureGarden)securegarden, (GardenVerifier)gardenverifier);
                        x.Location = new Point3D(from.X + 3, from.Y - 2, from.Z);
                        x.Map = from.Map;

                        from.SendGump(new GardenGump(from));
                        this.Delete();
                    }
                    else
                    {
                        from.SendMessage("Voce nao pode por um jardim aqui.");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
            }
        }

        public bool Validate(Mobile from)
        {
            if (from.Region == null)
            {
                return true;
            }
            else
            {
               return false;
            }
        }

        public bool GardenCheck(Mobile from)
        {
            int count = 0;
            foreach (Item verifier in from.Backpack.Items)
            {
                if (verifier is GardenVerifier)
                {
                    count = count + 1;
                }
                else
                {
                    count = count + 0;
                }
            }
            if (count > 0) //change this if you want players to own more than 1,2,3 etc.
            {
                return false;
            }
            else
            {
                return true;
            }
            //return GardenCheck(from);
        }

        public GardenDeed(Serial serial)
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
