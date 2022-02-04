using System;

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
}
