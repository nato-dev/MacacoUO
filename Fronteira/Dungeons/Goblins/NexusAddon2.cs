using System;
using Server;

namespace Server.Items
{
    public class NexusComponent2 : AddonComponent
    {
        public override int LabelNumber { get { return 1152442; } } // Nexus

        public NexusComponent2(int itemID)
            : base(itemID)
        {
        }

        public NexusComponent2(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Parece que isto pode ser ativado de alguma forma...");
            base.OnDoubleClick(from);
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

    public class NexusAddon2 : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new NexusAddonDeed(); } }

        [Constructable]
        public NexusAddon2()
        {
            this.AddComponent(new NexusComponent2(19324), -1, 1, 0);
            this.AddComponent(new NexusComponent2(19326), 0, 0, 0);
            this.AddComponent(new NexusComponent2(19319), 1, 2, 0);
            this.AddComponent(new NexusComponent2(19321), 2, 1, 0);
            this.AddComponent(new NexusComponent2(19319), 1, 2, 0);
            this.AddComponent(new NexusComponent2(19323), 0, 1, 0);
            this.AddComponent(new NexusComponent2(19327), 1, -1, 0);
            this.AddComponent(new NexusComponent2(19316), 1, 1, 0);
            this.AddComponent(new NexusComponent2(19322), 0, -1, 0);
            this.AddComponent(new NexusComponent2(19320), 2, 2, 0);
            this.AddComponent(new NexusComponent2(19325), 1, 0, 0);
            this.AddComponent(new NexusComponent2(19318), 0, 2, 0);
            this.AddComponent(new NexusComponent2(19317), -1, 2, 0);
        }

        public NexusAddon2(Serial serial) : base(serial)
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
