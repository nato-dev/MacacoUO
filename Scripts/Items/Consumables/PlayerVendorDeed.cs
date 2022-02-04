using System;
using Server.Mobiles;
using Server.Multis;

namespace Server.Items
{
    public class ContractOfEmployment : Item
    {
        [Constructable]
        public ContractOfEmployment()
            : base(0x14F0)
        {
            this.Weight = 1.0;
            //LootType = LootType.Blessed;
            Name = "Contrato de Vendedor";
        }

        public ContractOfEmployment(Serial serial)
            : base(serial)
        {
             Name = "Contrato de Vendedor";
        }

    
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                from.SendMessage("Vc eh GM e pode botar isso onde c quiser meu rei"); // Your godly powers allow you to place this vendor whereever you wish.

                Mobile v = new PlayerVendor(from, BaseHouse.FindHouseAt(from));

                v.Direction = from.Direction & Direction.Mask;
                v.MoveToWorld(from.Location, from.Map);

                v.SayTo(from, 503246); // Ah! it feels good to be working again.

                this.Delete();
            }
            else
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);

                if (house == null)
                {
                    from.SendMessage("Voce so pode colocar isto em sua casa"); // Vendors can only be placed in houses.	
                }
                else if (!BaseHouse.NewVendorSystem && !house.IsFriend(from))
                {
                    from.SendMessage("Voce precisa ser amigo da casa para fazer isto"); // You must ask the owner of this building to name you a friend of the household in order to place a vendor here.
                }
                else if (BaseHouse.NewVendorSystem && !house.IsOwner(from))
                {
                    from.SendMessage("Apenas o dono da casa pode fazer isto"); // Only the house owner can directly place vendors.  Please ask the house owner to offer you a vendor contract so that you may place a vendor in this house.
                }
                else if (!house.Public || !house.CanPlaceNewVendor()) 
                {
                    from.SendMessage("Voce nao pode por vendedores ou taverneiros aqui"); // You cannot place this vendor or barkeep.  Make sure the house is public and has sufficient storage available.
                }
                else
                {
                    bool vendor, contract;
                    BaseHouse.IsThereVendor(from.Location, from.Map, out vendor, out contract);

                    if (vendor)
                    {
                        from.SendMessage("Voce nao pode por um vendedor ou taverneiro aqui"); // You cannot place a vendor or barkeep at this location.
                    }
                    else if (contract)
                    {
                        from.SendMessage("Voce nao pode colocar um vendedor ou taverneiro aqui"); // You cannot place a vendor or barkeep on top of a rental contract!
                    }
                    else
                    {
                        Mobile v = new PlayerVendor(from, house);

                        v.Direction = from.Direction & Direction.Mask;
                        v.MoveToWorld(from.Location, from.Map);

                        v.SayTo(from, 503246); // Ah! it feels good to be working again.

                        this.Delete();
                    }
                }
            }
        }
    }
}
