using System;

namespace Server.Items
{
    public class Engenhoca : Item
    {
        [Constructable]
        public Engenhoca()
            : base(0x9A9)
        {
            Name = "Caixa Estranha";
            this.Hue = 2500;
            this.Weight = 1;
            LootType = LootType.Blessed;
        }


        public Engenhoca(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Voce nao consegue abrir a caixa. Talvez o engenheiro que o sargento mencionou saiba algo sobre isto");
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
