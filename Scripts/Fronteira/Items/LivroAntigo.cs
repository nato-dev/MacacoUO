namespace Server.Ziden.Items
{
    public class LivroAntigo : Item
    {
        [Constructable]
        public LivroAntigo(): base(0xFF1)
        {
            Name = "Livro Antigo";
        }

        public LivroAntigo(Serial s): base(s)
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            from.AddToBackpack(LootPackItem.RandomScroll(5, 8));
            if(Shard.NECRO && Utility.RandomBool())
                from.AddToBackpack(LootPackItem.RandomNecroScroll(4, 8));
            else
                from.AddToBackpack(LootPackItem.RandomScroll(5, 7));
            Consume();
            from.SendMessage("O livro tinha apenas 2 paginas. As paginas na verdade eram pergaminhos !");
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
