using System;

namespace Server.Items
{
    public abstract class BaseScales : Item, ICommodity
    {
        protected virtual CraftResource DefaultResource { get { return CraftResource.EscamasVermelhas; } }

        private CraftResource m_Resource;
        public BaseScales(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseScales(CraftResource resource, int amount)
            : base(0x26B4)
        {
            this.Stackable = true;
            this.Amount = amount;
            this.Hue = CraftResources.GetHue(resource);

            this.m_Resource = resource;
        }

        public BaseScales(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1053139;
            }
        }// dragon scales
        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get
            {
                return this.m_Resource;
            }
            set
            {
                this.m_Resource = value;
                this.InvalidateProperties();
            }
        }
        public override double DefaultWeight
        {
            get
            {
                return 0.1;
            }
        }
        TextDefinition ICommodity.Description
        {
            get
            {
                return this.LabelNumber;
            }
        }
        bool ICommodity.IsDeedable
        {
            get
            {
                return true;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)this.m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1: // Reset from Resource System
                    this.m_Resource = this.DefaultResource;
                    reader.ReadString();
                    break;
                case 0:
                    {
                        this.m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
            }
        }
    }

    public class RedScales : BaseScales
    {
        [Constructable]
        public RedScales()
            : this(1)
        {
        }

        [Constructable]
        public RedScales(int amount)
            : base(CraftResource.EscamasVermelhas, amount)
        {
        }

        public RedScales(Serial serial)
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

    public class YellowScales : BaseScales
    {
        protected override CraftResource DefaultResource { get { return CraftResource.EscamasAmarelas; } }

        [Constructable]
        public YellowScales()
            : this(1)
        {
        }

        [Constructable]
        public YellowScales(int amount)
            : base(CraftResource.EscamasAmarelas, amount)
        {
        }

        public YellowScales(Serial serial)
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

    public class BlackScales : BaseScales
    {
        protected override CraftResource DefaultResource { get { return CraftResource.EscamasNegras; } }

        [Constructable]
        public BlackScales()
            : this(1)
        {
        }

        [Constructable]
        public BlackScales(int amount)
            : base(CraftResource.EscamasNegras, amount)
        {
        }

        public BlackScales(Serial serial)
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

    public class GreenScales : BaseScales
    {
        protected override CraftResource DefaultResource { get { return CraftResource.EscamasVerdes; } }

        [Constructable]
        public GreenScales()
            : this(1)
        {
        }

        [Constructable]
        public GreenScales(int amount)
            : base(CraftResource.EscamasVerdes, amount)
        {
        }

        public GreenScales(Serial serial)
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

    public class WhiteScales : BaseScales
    {
        protected override CraftResource DefaultResource { get { return CraftResource.EscamasBrancas; } }

        [Constructable]
        public WhiteScales()
            : this(1)
        {
        }

        [Constructable]
        public WhiteScales(int amount)
            : base(CraftResource.EscamasBrancas, amount)
        {
        }

        public WhiteScales(Serial serial)
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

    public class BlueScales : BaseScales
    {
        protected override CraftResource DefaultResource { get { return CraftResource.EscamasAzuis; } }

        [Constructable]
        public BlueScales()
            : this(1)
        {
        }

        [Constructable]
        public BlueScales(int amount)
            : base(CraftResource.EscamasAzuis, amount)
        {
        }

        public BlueScales(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1053140;
            }
        }// sea serpent scales
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
