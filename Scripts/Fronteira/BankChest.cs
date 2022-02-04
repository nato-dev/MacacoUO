using Server.Items;
using Server.Misc.Custom;
using Server.Mobiles;

namespace Server.Ziden
{
    public class BankChest : Item
    {

        [Constructable]
        public BankChest() : base(3708) {
            this.Name = "Banco";
            this.Movable = false;
        }

        public int Custo { get { if (Movable) return 5000; else return 500; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Banco");
            list.Add(Custo+" Moedas para usar");
        }

        public BankChest(Serial s) : base(s) { }

        public override void OnDoubleClick(Mobile from)
        {
            if(Banker.Withdraw(from, Custo, true))
                BankLevels.OpenBank(from);
            else
            {
                if (!from.Backpack.HasItem(typeof(Gold), Custo, true))
                {
                    from.SendMessage("Voce nao tem gold suficiente no banco para isto");
                    return;
                }
                from.Backpack.ConsumeTotal(new System.Type[] { typeof(Gold) }, new int[] { 500 });
                BankLevels.OpenBank(from);
                return;
            }
        }

    }
}
