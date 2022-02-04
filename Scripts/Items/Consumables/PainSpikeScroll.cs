using System;

namespace Server.Items
{
    public class PainSpikeScroll : SpellScroll
    {
        [Constructable]
        public PainSpikeScroll()
            : this(1)
        {
        }

        [Constructable]
        public PainSpikeScroll(int amount)
            : base(108, 0x2268, amount)
        {
            if (!Shard.NECRO)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(10), () =>
                {
                    this.Delete();
                });
            }
        }

        public PainSpikeScroll(Serial serial)
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
