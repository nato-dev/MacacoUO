using System;

namespace Server.Items
{
    [FlipableAttribute(0x13BB, 0x13C0)]
    public class RingmailCoif : BaseArmor
    {
        [Constructable]
        public RingmailCoif()
            : base(0x13BB)
        {
            this.Weight = 1.0;
            this.Name = "Capuz de Loriga";
            this.Hue = 642;
        }

        public RingmailCoif(Serial serial)
            : base(serial)
        {
        }

        public override int MaxMageryCircle { get { return 6; } }

        public override int BasePhysicalResistance
        {
            get
            {
                return 3;
            }
        }
    
        public override int InitMinHits
        {
            get
            {
                return 35;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 60;
            }
        }

        public override int OldStrReq
        {
            get
            {
                return 20;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 18;
            }
        }
        public override ArmorMeditationAllowance DefMedAllowance
        {
            get
            {
                return ArmorMeditationAllowance.Half;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Ringmail;
            }
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
