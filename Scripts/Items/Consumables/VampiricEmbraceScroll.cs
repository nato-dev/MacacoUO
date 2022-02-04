using System;

namespace Server.Items
{
    public class VampiricEmbraceScroll : SpellScroll
    {
        [Constructable]
        public VampiricEmbraceScroll()
            : this(1)
        {
        }

        [Constructable]
        public VampiricEmbraceScroll(int amount)
            : base(112, 0x226C, amount)
        {
            if (!Shard.NECRO)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(10), () =>
                {
                    this.Delete();
                });
            }
        }

        public VampiricEmbraceScroll(Serial serial)
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
