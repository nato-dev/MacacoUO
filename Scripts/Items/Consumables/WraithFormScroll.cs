using System;

namespace Server.Items
{
    public class WraithFormScroll : SpellScroll
    {
        [Constructable]
        public WraithFormScroll()
            : this(1)
        {
        }

        [Constructable]
        public WraithFormScroll(int amount)
            : base(115, 0x226F, amount)
        {
            if (!Shard.NECRO)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(10), () =>
                {
                    this.Delete();
                });
            }
        }

        public WraithFormScroll(Serial serial)
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
