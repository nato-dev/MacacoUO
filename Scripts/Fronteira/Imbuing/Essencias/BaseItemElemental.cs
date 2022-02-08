using Server.Items;
using System;

namespace Server.Items
{
    public class BaseItemElemental : Item, ICommodity
    {
        public virtual ElementoPvM Elemento { get { return ElementoPvM.None; } }

        private static Type[] Elementos = new Type[] {
            typeof(EssenciaFogo), typeof(EssenciaAgua), typeof(EssenciaTerra), typeof(EssenciaRaio), typeof(EssenciaLuz),
             typeof(EssenciaEscuridao),  typeof(EssenciaVento),  typeof(EssenciaGelo)
        };

        public static Item RandomEssencia()
        {
            var tipoRandom = Elementos[Utility.Random(Elementos.Length)];
            return (Item)Activator.CreateInstance(tipoRandom);
        }

        [Constructable]
        public BaseItemElemental()
            : this(1)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Contem magia de " + Elemento.ToString());
            list.Add("Usado para criar joias e talismans elementais");
        }

        [Constructable]
        public BaseItemElemental(int amount)
            : base(0x571C)
        {
            Name = "Essencia de " + Elemento.ToString();
            Stackable = true;
            Amount = amount;
            Hue = BaseArmor.HueElemento(Elemento);
        }

        public BaseItemElemental(Serial serial)
            : base(serial)
        {
        }

      
        TextDefinition ICommodity.Description
        {
            get
            {
                return "Essencia";
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
