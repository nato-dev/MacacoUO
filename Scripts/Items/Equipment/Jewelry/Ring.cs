using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public abstract class BaseRing : BaseJewel
    {
        public BaseRing(int itemID)
            : base(itemID, Layer.Ring)
        {
        }

        public BaseRing(Serial serial)
            : base(serial)
        {
        }

        public override int BaseGemTypeNumber
        {
            get
            {
                return 1044176;
            }
        }// star sapphire ring
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 1)
            {
                if (Weight == .1)
                {
                    Weight = -1;
                }
            }
        }
    }

    public class GoldRing : BaseRing
    {
        [Constructable]
        public GoldRing()
            : base(0x108a)
        {
            //Weight = 0.1;
        }

        public GoldRing(Serial serial)
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

    public class GoldRingMagico : BaseRing
    {
        [Constructable]
        public GoldRingMagico()
            : base(0x108a)
        {
            Name = "Anel de Ouro Magico";
            this.SkillBonuses.Skill_1_Name = Utility.RandomSkill();
            this.SkillBonuses.Skill_1_Value = 1;
            //Weight = 0.1;
        }

        public GoldRingMagico(Serial serial)
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

    public class GoldRingElegante : BaseRing
    {
        [Constructable]
        public GoldRingElegante()
            : base(0x108a)
        {
            Name = "Anel de Ouro Elegante";
            switch (Utility.Random(3))
            {
                case 0: this.Attributes.BonusStr = 1; break;
                case 1: this.Attributes.BonusDex = 1; break;
                case 2: this.Attributes.BonusInt = 1; break;
            }
            //Weight = 0.1;
        }

        public GoldRingElegante(Serial serial)
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

    public class SilverRing : BaseRing, IRepairable
    {
        public CraftSystem RepairSystem { get { return DefTinkering.CraftSystem; } }

        [Constructable]
        public SilverRing()
            : base(0x1F09)
        {
            //Weight = 0.1;
        }

        public SilverRing(Serial serial)
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
