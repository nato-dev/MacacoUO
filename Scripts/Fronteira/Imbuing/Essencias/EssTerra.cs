using System;

namespace Server.Items
{
    public class EssenciaTerra : BaseEssencia, ICommodity
    {
        public override ElementoPvM Elemento { get { return ElementoPvM.Terra; } }

        [Constructable]
        public EssenciaTerra()
            : this(1)
        {
        }

        [Constructable]
        public EssenciaTerra(int amount)
            : base(0x571C)
        {
         
            Stackable = true;
            Amount = amount;
           
        }

        public EssenciaTerra(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1113325;
            }
        }// essence of achievement
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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
