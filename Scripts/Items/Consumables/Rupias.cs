using System;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Items
{
    public class MoedaMagica : Item
    {
        [Constructable]
        public MoedaMagica()
            : this(1)
        {
            this.Name = "Moedas Magicas";
            this.LootType = LootType.Blessed;
            this.Hue = 200;
        }

        [Constructable]
        public MoedaMagica(int amountFrom, int amountTo)
            : this(Utility.RandomMinMax(amountFrom, amountTo))
        {
            this.Name = "Moedas Magicas";
            this.LootType = LootType.Blessed;
            this.Hue = 200;
        }

        [Constructable]
        public MoedaMagica(int amount)
            : base(0xEED)
        {
            this.Stackable = true;
            this.Amount = amount;
            this.Name = "Moedas Magicas";
            this.LootType = LootType.Blessed;
            this.Hue = 200;

        }

        public MoedaMagica(Serial serial)
            : base(serial)
        {
            this.Name = "Moedas Magicas";
            this.LootType = LootType.Blessed;
            this.Hue = 200;
        }

        public override double DefaultWeight
        {
            get
            {
                return 0.04;
            }
        }
        public override int GetDropSound()
        {
            if (this.Amount <= 1)
                return 0x2E4;
            else if (this.Amount <= 5)
                return 0x2E5;
            else
                return 0x2E6;
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

        protected override void OnAmountChange(int oldValue)
        {
            int newValue = this.Amount;

            this.UpdateTotal(this, TotalType.Gold, newValue - oldValue);
        }
    }
}
