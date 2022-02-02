using Server.Gumps;
using Server.Items;
using Server.Multis;
using Server.Targeting;
using System.Collections.Generic;
using System.Linq;

namespace Server.Fronteira.Armory
{
    public class ArmarioItem
    {
        public ArmarioItem()
        {
        }

        public ArmarioItem(Item i)
        {
            nomeType = i.GetType().Name;
            nome = i.Name;
            qtd = i.Amount;
        }

        public string nomeType;
        public string nome;
        public int qtd;
    }

    public class BaseArmario : FurnitureContainer
    {
        private List<ArmarioItem> _set = new List<ArmarioItem>();

        public BaseArmario()
            : base(0xA4F)
        {
            this.Weight = 1.0;
        }

        public BaseArmario(int itemID)
         : base(itemID)
        {
            this.Weight = 1.0;
        }

        public BaseArmario(Serial serial)
            : base(serial)
        {
        }

        public override void DisplayTo(Mobile m)
        {
            if (DynamicFurniture.Open(this, m))
                base.DisplayTo(m);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if(!Acesso(this, from))
            {
                from.SendMessage("Este armario nao e seu");
                return;
            } 
            AbreArmario(this, from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(_set.Count);
            foreach(var i in _set)
            {
                writer.Write(i.nome);
                writer.Write(i.nomeType);
                writer.Write(i.qtd);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            var qtd = reader.ReadInt();
            for(int x = 0; x < qtd; x++)
            {
                var i = new ArmarioItem();
                i.nome = reader.ReadString();
                i.nomeType = reader.ReadString();
                i.qtd = reader.ReadInt();
                _set.Add(i);
            }
            DynamicFurniture.Close(this);
        }

        public static bool Acesso(FurnitureContainer armario, Mobile from)
        {
            var casa = BaseHouse.FindHouseAt(armario);
            if (casa != null && !casa.HasAccess(from))
            {
                return false;
            }
            return true;
        }

        public void AbreArmario(FurnitureContainer armario, Mobile from)
        {
            var casa = BaseHouse.FindHouseAt(armario);
            if (casa != null && !casa.HasAccess(from))
            {
                from.SendMessage("Este armario nao e seu");
                return;
            }

            from.SendGump(new GumpOpcoes("Selecione", (opt) =>
            {
                if (opt == 0)
                {
                    Ver(armario, from);
                }
                else if (opt == 1)
                {
                    Depositar(armario, from);
                }
                else if (opt == 2)
                {
                    Pegar(armario, from);
                }
                else if (opt == 3)
                {
                    Pegar(armario, from);
                }

            }, armario.ItemID, 0, "Abrir", "Setar Set", "Retirar Set"));
        }

        private void Ver(FurnitureContainer armario, Mobile from)
        {
            armario.Open(from);
        }

        private void Depositar(FurnitureContainer armario, Mobile from)
        {
            from.SendMessage("Selecione uma mochila com os items do set.");
            from.BeginTarget(5, false, TargetFlags.None, new TargetCallback((Mobile m, object t) =>
            {
                var mochila = t as Container;
                if (mochila == null)
                {
                    from.SendMessage("Voce apenas pode fazer isto com mochilas");
                    return;
                }
                if (mochila.Parent is Mobile)
                {
                    from.SendMessage("Nao pode fazer isto com mochilas equipadas");
                    return;
                }
                if (mochila.RootParent != from)
                {
                    from.SendMessage("Voce apenas pode selecionar mochilas que estao dentro de sua mochila");
                    return;
                }

                _set.Clear();
                m.SendMessage("Voce colocou o set no armario. Agora voce apenas pode colocar mochilas identicas a esta no armario.");
                var adds = "";
                foreach(var item in mochila.Items)
                {
                    adds += (item.Amount > 1 ? item.Amount+"x " : "") + item.Name + ", ";
                    _set.Add(new ArmarioItem(item));
                    armario.DropItem(item);
                }
                m.SendMessage("Voce setou o set desse armario com os items:");
                m.SendMessage(adds);
            }));
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Shard.Debug("Dropei " + dropped.GetType().Name);
            return base.OnDragDrop(from, dropped);
        }

        private static void Pegar(FurnitureContainer armario, Mobile from)
        {

        }

        private static void Equipar(FurnitureContainer armario, Mobile from)
        {

        }

        private static List<Backpack> GetSets(FurnitureContainer armario)
        {
            return armario.Items.Select(i => i as Backpack).Where(i => i != null).ToList();
        }

        public override void OnCra
    }


}
