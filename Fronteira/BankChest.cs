using Server.Misc.Custom;

namespace Server.Ziden
{
    public class BankChest : Item
    {

        [Constructable]
        public BankChest() : base(3708) {
            this.Name = "Banco";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Seu Container Privado");
        }

        public BankChest(Serial s) : base(s) { }

        public override void OnDoubleClick(Mobile from)
        {
            BankLevels.OpenBank(from);
        }

    }
}
