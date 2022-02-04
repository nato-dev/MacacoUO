using System;

namespace Server.Items
{
    public class DecoGoldIngots2 : Item
    {
        [Constructable]
        public DecoGoldIngots2()
            : base(0x1BEB)
        {
            this.Movable = true;
            this.Stackable = false;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Decoracao");
        }

        public DecoGoldIngots2(Serial serial)
            : base(serial)
        {
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
