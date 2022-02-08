using Server.Fronteira.Elementos;

namespace Server.Items
{
    public class ColarElemental : BaseNecklace
    {
        private int _nivel;

        public static int GetNivel(Mobile from, ElementoPvM elemento)
        {
            var colar = from.NeckArmor as ColarElemental;
            if(colar != null && colar.Elemento == elemento && from.Elemento == elemento)
            {
                return colar.Nivel;
            }
            return 0;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Nivel { get { return _nivel; } set { _nivel = value; InvalidateProperties(); } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Encantado com " + Elemento.ToString());
            list.Add("Nivel: "+Nivel+"/50");
            foreach (var e in EfeitosElementos.GetEfeitosColar(Elemento))
                list.Add(e);
        }

        [Constructable]
        public ColarElemental(ElementoPvM elemento)
            : base(0x3BB5)
        {
            this.Elemento = elemento;
            Name = "Colar de "+Elemento.ToString();
             
            Hue = BaseArmor.HueElemento(Elemento);
            Nivel = 1;
        }

        public ColarElemental(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(Nivel);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Nivel = reader.ReadInt();
        }
    }
}
