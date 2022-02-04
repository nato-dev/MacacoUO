using Server.Misc.Custom;

namespace Server.Ziden
{
    public class FreeBankChest : Item
    {

        [Constructable]
        public FreeBankChest() : base(3708) {
            this.Name = "Banco";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Seu Container Privado");
        }

        public FreeBankChest(Serial s) : base(s) { }

        public override void OnDoubleClick(Mobile from)
        {
            BankLevels.OpenBank(from);
        }

    }
}
