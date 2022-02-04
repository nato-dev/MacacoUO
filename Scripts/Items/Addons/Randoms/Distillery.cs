using Server;
using System;
using Server.Multis;
using Server.Engines.Distillation;

namespace Server.Items
{
    public class DistilleryEastAddon2 : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new DistilleryEastAddonDeed2(); } }

        [Constructable]
        public DistilleryEastAddon2()
        {
            AddComponent(new LocalizedAddonComponent(15802, 1150640), 0, 0, 0);
            AddComponent(new LocalizedAddonComponent(15803, 1150640), 0, 1, 0);
        }

        public override void OnComponentUsed(AddonComponent component, Mobile from)
        {
            BaseHouse house = BaseHouse.FindHouseAt(component);

            if (from.InRange(component.Location, 2) && (house == null || house.IsCoOwner(from)))
                from.SendGump(new DistillationGump(from));
        }

        public DistilleryEastAddon2(Serial serial)
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

    public class DistilleryEastAddonDeed2 : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new DistilleryEastAddon2(); } }
        public override int LabelNumber { get { return 1150664; } } // distillery (east)

        [Constructable]
        public DistilleryEastAddonDeed2()
        {
        }

        public DistilleryEastAddonDeed2(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    public class DistillerySouthAddon2 : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new DistillerySouthAddonDeed2(); } }

        [Constructable]
        public DistillerySouthAddon2()
        {
            AddComponent(new LocalizedAddonComponent(15800, 1150640), 0, 0, 0);
            AddComponent(new LocalizedAddonComponent(15801, 1150640), -1, 0, 0);
        }

        public override void OnComponentUsed(AddonComponent component, Mobile from)
        {
            BaseHouse house = BaseHouse.FindHouseAt(component);

            if (house == null || house.IsCoOwner(from))
                from.SendGump(new DistillationGump(from));
        }

        public DistillerySouthAddon2(Serial serial)
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

    public class DistillerySouthAddonDeed2 : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new DistillerySouthAddon2(); } }
        public override int LabelNumber { get { return 1150663; } } // distillery (south)

        [Constructable]
        public DistillerySouthAddonDeed2()
        {
        }

        public DistillerySouthAddonDeed2(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
