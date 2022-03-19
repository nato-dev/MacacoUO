using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public abstract class BaseBracelet : BaseJewel
    {
        public BaseBracelet(int itemID)
            : base(itemID, Layer.Bracelet)
        {
        }

        public BaseBracelet(Serial serial)
            : base(serial)
        {
        }

        public override int BaseGemTypeNumber
        {
            get
            {
                return 1044221;
            }
        }// star sapphire bracelet
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

    public class GoldBracelet : BaseBracelet
    {
        [Constructable]
        public GoldBracelet()
            : base(0x1086)
        {
            //Weight = 0.1;
        }

        public GoldBracelet(Serial serial)
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

    public enum TipoBracelete
    {
        Arma, Arco, Magia
    }

    public class BraceleteDoPoder : BaseBracelet
    {
        [CommandProperty(AccessLevel.Administrator)]
        public int Bonus { get; set; }

        [CommandProperty(AccessLevel.Administrator)]
        public TipoBracelete Tipo { get; set; }

        [Constructable]
        public BraceleteDoPoder()
            : base(0x1086)
        {
            //Weight = 0.1;
            Name = "Bracelete do Poder";
            Bonus = 10;
            Hue = 1151;
            Tipo = TipoBracelete.Arma;
        }

        public BraceleteDoPoder(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Este bracelete ajuda com bonus nas habilidades das armas em PvM.");
            from.SendMessage("Para aprender sobre as habilidades das armas, veja nossa wiki.");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if(Tipo==TipoBracelete.Arma)
                list.Add($"Bonus Dano Hab. Armas Fisicas PvM");
            else if (Tipo == TipoBracelete.Arco)
                list.Add($"Bonus Dano Hab. Arcos PvM");
            else if (Tipo == TipoBracelete.Magia)
                list.Add($"Bonus Dano Hab. Magias PvM");
            list.Add($"+{Bonus}%");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(Bonus);
            writer.Write((int)Tipo);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Bonus = reader.ReadInt();
            Tipo = (TipoBracelete)reader.ReadInt();
        }
    }

    public class GoldBraceletMagico : BaseBracelet
    {
        [Constructable]
        public GoldBraceletMagico()
            : base(0x1086)
        {
            Name = "Bracelete Magico";
            this.SkillBonuses.Skill_1_Name = Utility.RandomSkill();
            this.SkillBonuses.Skill_1_Value = 1;
        }

        public GoldBraceletMagico(Serial serial)
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

    public class GoldBraceletBonito : BaseBracelet
    {
        [Constructable]
        public GoldBraceletBonito()
            : base(0x1086)
        {
            Name = "Bracelete Elegante";
            switch(Utility.Random(3))
            {
                case 0: this.Attributes.BonusStr = 1; break;
                case 1: this.Attributes.BonusDex = 1; break;
                case 2: this.Attributes.BonusInt = 1; break;
            }
        }

        public GoldBraceletBonito(Serial serial)
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

    public class SilverBracelet : BaseBracelet, IRepairable
    {
        public CraftSystem RepairSystem { get { return DefTinkering.CraftSystem; } }

        [Constructable]
        public SilverBracelet()
            : base(0x1F06)
        {
            //Weight = 0.1;
        }

        public SilverBracelet(Serial serial)
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
