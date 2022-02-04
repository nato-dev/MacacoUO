using System;

namespace Server.Items
{
    public class GrayBrickFireplaceEastAddonExp : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new GrayBrickFireplaceEastDeedExp();
            }
        }

        [Constructable]
        public GrayBrickFireplaceEastAddonExp()
        {
            AddonComponent ac = null;
            ac = new

            AddonComponent(0x93D);
            ac.Name = "Gray Brick Fireplace";
            AddComponent(ac, 0, 0, 0);

            ac = new AddonComponent(0x8CF);
            ac.Name = "Gray Brick Fireplace";
            AddComponent(ac, 0, 1, 0);
             
        }

        public GrayBrickFireplaceEastAddonExp(Serial serial)
            : base(serial)
        {
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
                from.SendMessage("Muito longe!");
            else
            {
                if (ac.ItemID == 0x937)
                {
                    ac.ItemID = 0x8CF;
                    Effects.PlaySound(from.Location, from.Map, 0x4B9);
                    from.SendMessage("Voce apagou o fogo!");
                }
                else if (ac.ItemID == 0x8CF)
                {
                    Container pack = from.Backpack;

                    if (pack == null)
                        return;

                    int res = pack.ConsumeTotal(new Type[]{typeof( Log )}, new int[]{ 3 });

                    switch (res)
                    {
                        case 0:
                            {
                                from.SendMessage("Voce precisa ter 3 toras para colocar no fogo");
                                break;
                            }
                        default:
                            {
                                Effects.PlaySound(from.Location, from.Map, 0x137);
                                from.SendMessage("Voce colocou as toras na fogueira!");
                                ac.ItemID = 0x935;
                                break;
                            }
                    }
                }
                else if (ac.ItemID == 0x935)
                {
                    Item matchlight = from.Backpack.FindItemByType(typeof(MatchLight));

                    if (matchlight != null)
                    {
                        matchlight.Delete();
                        ac.ItemID = 0x937;
                        ac.Light = LightType.Circle225;
                        Effects.PlaySound(from.Location, from.Map, 0x4BA);
                        from.SendMessage("Voce ascendeu o fogo!");
                    }
                    else
                    {
                        if (matchlight == null)
                        {
                            from.SendMessage("Voce precisa ter um fosforo para ascender a fogueira");
                        }
                    }
                }
                else
                    return;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

    }

    public class GrayBrickFireplaceEastDeedExp : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new GrayBrickFireplaceEastAddonExp();
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1061846;
            }
        }// grey brick fireplace (east)

        [Constructable]
        public GrayBrickFireplaceEastDeedExp()
        {
        }

        public GrayBrickFireplaceEastDeedExp(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
