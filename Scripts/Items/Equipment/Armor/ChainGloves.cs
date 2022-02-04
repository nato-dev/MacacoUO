using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(GargishPlateKilt))]
    public class ChainGloves : BaseArmor
    {
        [Constructable]
        public ChainGloves()
            : base(0x13EB)
        {
            this.Weight = 2.0;
            this.Hue = 980;
            this.Name = "Luvas de Malha";
        }

        public ChainGloves(Serial serial)
            : base(serial)
        {
        }


        public override int OldDexBonus
        {
            get
            {
                return -1;
            }
        }


        public override int MaxMageryCircle { get { return 5; } }

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
                return 45;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 55;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 50;
            }
        }

        public override int ArmorBase
        {
            get
            {
                return 25;
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

            if (this.Weight == 1.0)
                this.Weight = 2.0;
        }
    }
}
