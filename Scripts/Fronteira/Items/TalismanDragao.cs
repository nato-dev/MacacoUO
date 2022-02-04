using System;

namespace Server.Items
{
    public class TalismanDragao : BaseTalisman
    {
        [Constructable]
        public TalismanDragao()
            : base(0x2F58)
        {
            this.Name = "Talisman do Dragao de Quartzo";
            Hue = 778;
        }

        public TalismanDragao(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Reduz em 35% Dano de Bafo de Dragao");
          
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
