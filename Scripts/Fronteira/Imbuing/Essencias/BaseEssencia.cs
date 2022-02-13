using Server.Engines.Craft;
using System;

namespace Server.Items
{
    public class BaseEssencia : Item, ICommodity
    {
        public virtual ElementoPvM Elemento { get { return ElementoPvM.None; } }

        private static readonly Type[] Elementos = new Type[] {
            typeof(EssenciaFogo), typeof(EssenciaAgua), typeof(EssenciaTerra), typeof(EssenciaRaio), typeof(EssenciaLuz),
             typeof(EssenciaEscuridao),  typeof(EssenciaVento),  typeof(EssenciaGelo)
        };

        public static Type GetEssencia(ElementoPvM e)
        {
            switch(e)
            {
                case ElementoPvM.Agua: return typeof(EssenciaAgua);
                case ElementoPvM.Fogo: return typeof(EssenciaFogo);
                case ElementoPvM.Terra: return typeof(EssenciaTerra);
                case ElementoPvM.Raio: return typeof(EssenciaRaio);
                case ElementoPvM.Luz: return typeof(EssenciaLuz);
                case ElementoPvM.Vento: return typeof(EssenciaVento);
                case ElementoPvM.Escuridao: return typeof(EssenciaEscuridao);
                case ElementoPvM.Gelo: return typeof(EssenciaGelo);
            }
            return null;
        }

        public static Item RandomEssencia()
        {
            var tipoRandom = Elementos[Utility.Random(Elementos.Length)];
            return (Item)Activator.CreateInstance(tipoRandom);
        }

        [Constructable]
        public BaseEssencia()
            : this(1)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Contem magia de " + Elemento.ToString());
            list.Add("Usado para criar joias elementais");
            list.Add("Imbuing: 70");
            list.Add("Tinkering: 90");
        }

        public static readonly int QTD = 30;

        public override void OnDoubleClick(Mobile from)
        {
            if(this.Amount < QTD)
            {
                from.SendMessage("Junte "+QTD+" essencias do mesmo elemento para craftar joias elementais");
                return;
            }
            var ferramentas = from.FindItemsByType(typeof(TinkerTools));
            TinkerTools tem = null;
            foreach(var ferramenta in ferramentas)
            {
                if(((IResource)ferramenta).Resource == CraftResource.Quartzo)
                {
                    tem = ferramenta as TinkerTools;
                    break;
                }
            }
            if(tem == null)
            {
                from.SendMessage("Voce precisa de ferramentas do funileiro de quartzo para fazer isto");
                return;
            }
            if(from.Skills.Imbuing.Value < 70 || from.Skills.Tinkering.Value < 90)
            {
                from.SendMessage("Voce precisa de 70 imbuing e 90 tinkering para usar isto");
                return;
            }

            bool anvil, forge;
            DefBlacksmithy.CheckAnvilAndForge(from, 2, out anvil, out forge);
            if(!anvil || !forge)
            {
                from.SendMessage("Voce precisa de uma forja e uma bigorna para isto");
                return;
            }
            tem.UsesRemaining -= 30;
            if(tem.UsesRemaining <= 0)
            {
                tem.Delete();
                from.SendMessage("Sua ferramenta quebrou");
            }
            this.Consume(QTD);
            var colar = new ColarElemental(this.Elemento);
            colar.OwnerName = from.Name;
            from.PlaceInBackpack(colar);
            from.SendMessage("Voce criou um colar elemental elemental");
            from.Animate(AnimationType.Attack, 3);
            from.PlaySound(0x2A);

        }

        [Constructable]
        public BaseEssencia(int amount)
            : base(0x571C)
        {
            Name = "Essencia de " + Elemento.ToString();
            Stackable = true;
            Amount = amount;
            Hue = BaseArmor.HueElemento(Elemento);
        }

        public BaseEssencia(Serial serial)
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
