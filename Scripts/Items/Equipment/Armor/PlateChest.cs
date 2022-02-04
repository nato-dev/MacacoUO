using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(GargishPlateChest))]
    [FlipableAttribute(0x1415, 0x1416)]
    public class PlateChest : BaseArmor
    {
        [Constructable]
        public PlateChest()
            : base(0x1415)
        {
            this.Weight = 10.0;
            this.Name = "Peitoral de Metal";
        }

        public PlateChest(Serial serial)
            : base(serial)
        {
        }

        public override int MaxMageryCircle { get { return 3; } }

        public override int BasePhysicalResistance
        {
            get
            {
                return 5;
            }
        }
        public override int OldDexBonus
        {
            get
            {
                return -14;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 2;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 2;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 50;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 65;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 95;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 80;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 60;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Plate;
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

            if (this.Weight == 1.0)
                this.Weight = 10.0;
        }
    }
}
