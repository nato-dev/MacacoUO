using System;
using Server.Mobiles;

namespace Server.Items
{
    public class BODRewardTitleDeed : BaseRewardTitleDeed
    {
        public override string DefaultName { get { return "Titulo de BOD"; } } // A Deed for a Reward Title
        public override TextDefinition Title { get { return _Title; } }

        private TextDefinition _Title;

        [Constructable]
        public BODRewardTitleDeed(string title)
        {
            _Title = title;
        }

        [Constructable]
        public BODRewardTitleDeed(int title)
        {
            if (title == 0)
                _Title = new TextDefinition("o Armeiro");
            else if (title == 1)
                _Title = new TextDefinition("o Ferreiro");
            else if(title == 2)
            {
                _Title = new TextDefinition("o Costureiro");
            }
            else if (title == 3)
            {
                _Title = new TextDefinition("o Designer de Roupas");
            }
            else if (title == 4)
            {
                _Title = new TextDefinition("o Grande Alfaiate");
            }
            else if (title == 10)
            {
                _Title = new TextDefinition("o Madeireiro");
            }
            else if (title == 11)
            {
                _Title = new TextDefinition("o Carpinteiro");
            }
            else if (title == 12)
            {
                _Title = new TextDefinition("o Mestre da Madeirada");
            }
            else if (title == 13)
            {
                _Title = new TextDefinition("o Escritor");
            }
            else if (title == 14)
            {
                _Title = new TextDefinition("o Escriba");
            }
            else if (title == 15)
            {
                _Title = new TextDefinition("o Poeta");
            }
            else if (title == 16)
            {
                _Title = new TextDefinition("o Padeiro");
            }
            else if (title == 17)
            {
                _Title = new TextDefinition("o Cozinheiro");
            }
            else if (title == 18)
            {
                _Title = new TextDefinition("o Masterchef Gordao");
            }
            else if (title == 19)
            {
                _Title = new TextDefinition("o Criador de Flechas");
            }
            else if (title == 20)
            {
                _Title = new TextDefinition("o Criador de Pocoes");
            }
            else if (title == 21)
            {
                _Title = new TextDefinition("o Alquimista");
            }
            else if (title == 22)
            {
                _Title = new TextDefinition("o Fabricante de Dorgas");
            }

            else
                _Title = new TextDefinition(1157181 + title);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1114057, Title.ToString()); // ~1_NOTHING~
        }

        public BODRewardTitleDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            TextDefinition.Serialize(writer, _Title);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();

            _Title = TextDefinition.Deserialize(reader);
        }
    }
}
