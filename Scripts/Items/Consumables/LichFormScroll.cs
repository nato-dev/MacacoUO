using System;

namespace Server.Items
{
    public class LichFormScroll : SpellScroll
    {
        [Constructable]
        public LichFormScroll()
            : this(1)
        {
            if (!Shard.NECRO)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(10), () =>
                {
                    this.Delete();
                });
            }
        }

        [Constructable]
        public LichFormScroll(int amount)
            : base(106, 0x2266, amount)
        {
        }

        public LichFormScroll(Serial serial)
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
