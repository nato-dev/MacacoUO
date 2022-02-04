using System;

namespace Server.Items
{
    public class WitherScroll : SpellScroll
    {
        [Constructable]
        public WitherScroll()
            : this(1)
        {
        }

        [Constructable]
        public WitherScroll(int amount)
            : base(114, 0x226E, amount)
        {
            if (!Shard.NECRO)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(10), () =>
                {
                    this.Delete();
                });
            }
        }

        public WitherScroll(Serial serial)
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
