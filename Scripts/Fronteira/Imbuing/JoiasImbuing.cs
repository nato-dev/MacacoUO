using Server.Fronteira.Elementos;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    public class TalismanElemental : BaseTalisman
    {
        [Constructable]
        public TalismanElemental()
            : base(0x2F58)
        {
            this.Name = "Talisman Elemental";
            Hue = 784;
        }

        public static bool Tem(Mobile m)
        {
            return m.Talisman is TalismanElemental;
        }

        public TalismanElemental(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("Voce recebera menos dano de alguns elementos e mais dano de outros dependendo de seu elemento. Voce precisa de um elemento para isto acontecer.");
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Faz seu elemento ficar mais forte e/ou mais fraco versus outros elementos");

        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    public class ColarElemental : BaseNecklace
    {
        private int _nivel;

        public static int GetNivel(Mobile from, ElementoPvM elemento)
        {
            if (from == null || from.Elemento != elemento)
                return 0;

            var colar = from.NeckArmor as ColarElemental;
            if(colar != null && colar.Elemento == elemento && from.Elemento == elemento)
            {
                return colar.Nivel;
            }
            return 0;
        }

        public override bool CanEquip(Mobile from)
        {
            var pl = from as PlayerMobile;
            if(pl != null && pl.Elementos.GetNivel(Elemento) < 20)
            {
                pl.SendMessage("Voce precisa estar pelo menos " + Elemento.ToString() + " lvl 20 para equipar isto");
                return false;
            }
            return base.CanEquip(from);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Nivel { get { return _nivel; } set { _nivel = value; InvalidateProperties(); } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Colar de " + Elemento.ToString());
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

        public override void OnDoubleClick(Mobile from)
        {
            if (Nivel >= 50)
                return;

            if(!ColarElementalGump.CheckForja(from, 4))
            {
                from.SendMessage("Voce precisa de uma forja elfica para aprimorar seu colar elemental. Dizem que forjas elficas foram abandonadas na dungeon Caverna de Cristal, em Nujelm.");
                return;
            }
            from.SendGump(new ColarElementalGump(from as PlayerMobile, this));
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
