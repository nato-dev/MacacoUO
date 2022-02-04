using System;

namespace Server.Items
{
    public class ExecutionersCap : Item
    {
        [Constructable]
        public ExecutionersCap()
            : base(0xF83)
        {
            Name = "Cogumelo do Encolhimento";
            this.Weight = 1.0;
            Stackable = true;
        }

        public ExecutionersCap(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Talvez um alquimista possa usar isto para algo...");
            base.OnDoubleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
