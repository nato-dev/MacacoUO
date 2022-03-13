using Server.Commands;
using Server.Misc;
using System;

namespace Server.Items.Functional.Pergaminhos
{
    public class DoubleGoldDeed : Item
    {
        [Constructable]
        public DoubleGoldDeed()
            : this(0x14F0)
        {
            this.Hue = 54;
            this.Name = "Pergaminho de Double Gold";
        }

        public DoubleGoldDeed(int itemID)
           : base(itemID)
        {
            this.Hue = 54;
            this.Name = "Pergaminho de Double Gold";
            this.LootType = LootType.Newbied;
        }

        public DoubleGoldDeed(Serial serial)
            : base(serial)
        {
            this.Hue = 356;
            this.Name = "Pergaminho de Double Gold";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (GoldHour.GOLD_MULT != 0)
            {
                from.SendMessage("Ja esta ativo");
                return;
            }
            GoldHour.GOLD_MULT = 1.5;
            Anuncio.Anuncia(from.Name + " ativou um GoldHour 2x Gold para todos");
            Consume();
            Timer.DelayCall(TimeSpan.FromHours(1), () => {
                GoldHour.GOLD_MULT = 0;
                Anuncio.Anuncia("O PowerHour de Gold Terminou !");
            });
        }


        public override void AddNameProperties(ObjectPropertyList list)
        {
            list.Add("Ativa Double Gold por 1h");
            list.Add("Para o shard inteiro");
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

    public class DoubleExpDeed : Item
    {

        [Constructable]
        public DoubleExpDeed()
            : this(0x14F0)
        {
            
            this.Hue = 356;
            this.Name = "Pergaminho de Double Exp";
          
        }

        public DoubleExpDeed(int itemID)
           : base(itemID)
        {
            this.LootType = LootType.Newbied;
            this.Hue = 356;
            this.Name = "Pergaminho de Double Exp";
        }

        public DoubleExpDeed(Serial serial)
            : base(serial)
        {
            this.Hue = 356;
            this.Name = "Pergaminho de Double Exp";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if(SkillCheck.BONUS_GERAL != 0)
            {
                from.SendMessage("Double Exp ja esta ativo");
                return;
            }
            SkillCheck.BONUS_GERAL = 1.5;
            Anuncio.Anuncia(from.Name+" ativou um PowerHour 2x Exp para todos");
            Consume();
            Timer.DelayCall(TimeSpan.FromHours(1), () => {
                SkillCheck.BONUS_GERAL = 0;
                Anuncio.Anuncia("O PowerHour de XP Terminou !");
            });
        }


        public override void AddNameProperties(ObjectPropertyList list)
        {
            list.Add("Ativa Double Exp por 1h");
            list.Add("Para o shard inteiro");
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
