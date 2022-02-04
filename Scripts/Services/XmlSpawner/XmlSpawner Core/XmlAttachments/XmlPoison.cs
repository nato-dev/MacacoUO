using System;

namespace Server.Engines.XmlSpawner2
{
    public class XmlPoison : XmlAttachment
    {
        private int p_level = 0;
        public bool HitaPoison = true;
        // a serial constructor is REQUIRED
        public XmlPoison(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlPoison(int level, bool hitPoison = true)
        {
            this.p_level = level;
            this.HitaPoison = hitPoison;
        }

        public Poison PoisonImmune
        {
            get
            {
                if (this.p_level < 1)
                {
                    return Poison.Lesser;
                }
                else if (this.p_level == 1)
                {
                    return Poison.Regular;
                }
                else if (this.p_level == 2)
                {
                    return Poison.Greater;
                }
                else if (this.p_level == 3)
                {
                    return Poison.Deadly;
                }
                else if (this.p_level > 3)
                {
                    return Poison.Lethal;
                }
                else
                {
                    return Poison.Regular;
                }
            }
        }
        public Poison HitPoison
        {
            get
            {
                if (this.p_level < 1)
                {
                    return Poison.Lesser;
                }
                else if (this.p_level == 1)
                {
                    return Poison.Regular;
                }
                else if (this.p_level == 2)
                {
                    return Poison.Greater;
                }
                else if (this.p_level == 3)
                {
                    return Poison.Deadly;
                }
                else if (this.p_level > 3)
                {
                    return Poison.Lethal;
                }
                else
                {
                    return Poison.Regular;
                }
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
            // version 0
            writer.Write(this.p_level);
            writer.Write(HitaPoison);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            this.p_level = reader.ReadInt();
            this.HitaPoison = reader.ReadBool();
        }
    }
}
