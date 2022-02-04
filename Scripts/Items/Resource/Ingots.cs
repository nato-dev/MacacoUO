using System;

namespace Server.Items
{
    public abstract class BaseIngot : Item, ICommodity
    {
        protected virtual CraftResource DefaultResource { get { return CraftResource.Ferro; } }

        private CraftResource m_Resource;
        public BaseIngot(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseIngot(CraftResource resource, int amount)
            : base(0x1BF2)
        {
            this.Stackable = true;
            this.Amount = amount;
            this.Hue = CraftResources.GetHue(resource);

            this.m_Resource = resource;
        }

        public BaseIngot(Serial serial)
            : base(serial)
        {
        }

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
        public override int LabelNumber
        {
            get
            {
                if (this.m_Resource >= CraftResource.Berilo && this.m_Resource <= CraftResource.Quartzo)
                    return 1042684 + (int)(this.m_Resource - CraftResource.Berilo);

                return 1042692;
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

            writer.Write((int)1); // version

            writer.Write((int)this.m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 2: // Reset from Resource System
                    this.m_Resource = this.DefaultResource;
                    reader.ReadString();
                    break;
                case 1:
                    {
                        this.m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        OreInfo info;

                        switch ( reader.ReadInt() )
                        {
                            case 0:
                                info = OreInfo.Iron;
                                break;
                            case 1:
                                info = OreInfo.DullCopper;
                                break;
                            case 2:
                                info = OreInfo.ShadowIron;
                                break;
                            case 3:
                                info = OreInfo.Copper;
                                break;
                            case 4:
                                info = OreInfo.Bronze;
                                break;
                            case 5:
                                info = OreInfo.Gold;
                                break;
                            case 6:
                                info = OreInfo.Agapite;
                                break;
                            case 7:
                                info = OreInfo.Verite;
                                break;
                            case 8:
                                info = OreInfo.Valorite;
                                break;
                            default:
                                info = null;
                                break;
                        }

                        this.m_Resource = CraftResources.GetFromOreInfo(info);
                        break;
                    }
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if(this.Amount == 1)
                list.Add("Lingote de "+m_Resource);
            else
                list.Add(this.Amount+" Lingotes de " + m_Resource);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
        }
    }

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class IronIngot : BaseIngot
    {
        [Constructable]
        public IronIngot()
            : this(1)
        {
        }

        [Constructable]
        public IronIngot(int amount)
            : base(CraftResource.Ferro, amount)
        {
        }

        public IronIngot(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Bom e velho ferro. Recurso primordial para ferreiros e inventores.");
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class BeriloIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Berilo; } }

        [Constructable]
        public BeriloIngot()
            : this(1)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Minerio com a cor de sangue. Normalmente usado para armas.");
        }

        [Constructable]
        public BeriloIngot(int amount)
            : base(CraftResource.Berilo, amount)
        {
        }

        public BeriloIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class AdamantiumIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Adamantium; } }

        [Constructable]
        public AdamantiumIngot()
            : this(1)
        {
        }

        [Constructable]
        public AdamantiumIngot(int amount)
            : base(CraftResource.Adamantium, amount)
        {
        }

        public AdamantiumIngot(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Minerio claro brilhante, muito raro. Otimo para armas.");
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }



    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class VibraniumIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Vibranium; } }

        [Constructable]
        public VibraniumIngot()
            : this(1)
        {
        }

        [Constructable]
        public VibraniumIngot(int amount)
            : base(CraftResource.Vibranium, amount)
        {
        }

        public VibraniumIngot(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Minerio de cor unica. Muito bom para armaduras.");
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class CopperIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Cobre; } }

        [Constructable]
        public CopperIngot()
            : this(1)
        {
        }

        [Constructable]
        public CopperIngot(int amount)
            : base(CraftResource.Cobre, amount)
        {
        }

        public CopperIngot(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Minerio conhecido de tempos antigos. Muito usado para criar barcos.");
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class BronzeIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Bronze; } }

        [Constructable]
        public BronzeIngot()
            : this(1)
        {
        }

        [Constructable]
        public BronzeIngot(int amount)
            : base(CraftResource.Bronze, amount)
        {
        }

        public BronzeIngot(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Minerio conhecido de tempos antigos. Decente para equipamentos.");
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class SilverIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Dourado; } }

        [Constructable]
        public SilverIngot()
            : this(1)
        {
        }

        [Constructable]
        public SilverIngot(int amount)
            : base(CraftResource.Dourado, amount)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Minerio com alto valor devido a diversas utilidades. Muito bom para armaduras.");
        }

        public SilverIngot(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            if (version == 0 && Hue != 0x8A5)
            {
                Hue = 0x8A5;
                this.InvalidateProperties();
            }   
        }
    }

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class NiobioIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Niobio; } }

        [Constructable]
        public NiobioIngot()
            : this(1)
        {
        }

        [Constructable]
        public NiobioIngot(int amount)
            : base(CraftResource.Niobio, amount)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Minerio negro, nao eh muito bom para armas.");
        }

        public NiobioIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class LazuritaIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Lazurita; } }

        [Constructable]
        public LazuritaIngot()
            : this(1)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Um elegante minerio azul. Util para tudo.");
        }

        [Constructable]
        public LazuritaIngot(int amount)
            : base(CraftResource.Lazurita, amount)
        {
        }

        public LazuritaIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class QuartzoIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Quartzo; } }

        [Constructable]
        public QuartzoIngot()
            : this(1)
        {
        }

        [Constructable]
        public QuartzoIngot(int amount)
            : base(CraftResource.Quartzo, amount)
        {
        }

        public QuartzoIngot(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Este bonito minerio diz ser muito eficiente para criar ferramentas");
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
