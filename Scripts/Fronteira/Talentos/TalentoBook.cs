using Server.Gumps;
using Server.Mobiles;
using System;

namespace Server.Fronteira.Talentos
{
    public class TalentoBook : Item
    {
        [Constructable]
        public TalentoBook() : base(0x225A)
        {
            Name = "Catalogo de Talentos";
        }

        public TalentoBook(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Enciclopedia dos Talentos");
        }

        public override void OnDoubleClick(Mobile from)
        {
            var pl = from as PlayerMobile;
            if (pl == null)
                return;

            if (from.HasGump(typeof(GumpLivroTalento)))
                from.CloseGump(typeof(GumpLivroTalento));

            Talento[] Talentos = Enum.GetValues(typeof(Talento)).CastToArray<Talento>();
            from.SendGump(new GumpLivroTalento(pl, Talentos));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

        }
    }
}
