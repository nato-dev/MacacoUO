using System;
using System.Collections.Generic;
using Server.Fronteira.Quests;
using Server.Items;
using Server.Mobiles;
using Server.Ziden.Traducao;

namespace Server.Engines.BulkOrders
{
    public class BodTamer : BaseDecayingItem
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public string Nome { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Cor { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quantidade { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int QuantidadeAtual { get; set; }

        public int GraficoBody { get; set; }

        public int Pontos { get; set; }

        public override int Lifespan { get { return 60 * 60 * 24 * 5; } }
        public override bool UseSeconds { get { return false; } }

        public BodTamer(Tamavel tamavel, int quantidade)
            : base(0x2258)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
            this.GraficoBody = tamavel.Body;
            Quantidade = quantidade;
            Cor = tamavel.Hue;
            Nome = tamavel.Name;
        }

        public BodTamer(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(string.Format("Domar {0} {1}", Quantidade, Nome));
            list.Add(string.Format("Completo: {0}/{1}", QuantidadeAtual, Quantidade));
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || InSecureTrade || RootParent is PlayerVendor)
            {
                from.SendGump(new TameBodGump(from, this));
            }
            else
            {
                from.SendMessage("Precisa estar na mochila"); // You must have the deed in your backpack to use it.
            }
        }

        public override void OnDoubleClickNotAccessible(Mobile from)
        {
            OnDoubleClick(from);
        }

        public override void OnDoubleClickSecureTrade(Mobile from)
        {
            OnDoubleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version
            writer.Write(Nome);
            writer.Write(Cor);
            writer.Write(Quantidade);
            writer.Write(QuantidadeAtual);
            writer.Write(GraficoBody);
            writer.Write(Pontos);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Nome = reader.ReadString();
            Cor = reader.ReadInt();
            Quantidade = reader.ReadInt();
            QuantidadeAtual = reader.ReadInt();
            GraficoBody = reader.ReadInt();
            Pontos = reader.ReadInt();
        }

        private static Item CriaBod(PlayerMobile tamer)
        {
            var skill = tamer.Skills.AnimalTaming.Value;
            var tamavel = Tamavel.Sorteia(skill);
            var bod = new BodTamer(tamavel.Item1, tamavel.Item2);
            bod.BoundTo = tamer.Name;
            return bod;
        }

        public static void EntregaBodTamer(PlayerMobile from, BaseVendor vendor)
        {
            if (!(vendor is AnimalTrainer))
                return;

            if(from.Skills[SkillName.AnimalTaming].Value < 40)
            {
                vendor.SayTo(from, true, "Voce nao eh um domador bom suficiente para trabalhar para mim...");
                return;
            }

            if (BulkOrderSystem.CanGetBulkOrder(from, BODType.Taming))
            {
                Shard.Debug("Criando Bod Tamer");
                var bod = CriaBod(from);
                if(bod == null)
                {
                    vendor.SayTo(from, true, "Achei que teria trabalho mas me enganei, me desculpe");
                    return;
                }
                from.PlaceInBackpack(bod);
                vendor.SayTo(from, true, "Complete esta ordem de trabalho domando animais que lhe ensinarei mais sobre a arte de domar animais.");
            }
            else
            {
                TimeSpan ts = BulkOrderSystem.GetNextBulkOrder(BODType.Taming, (PlayerMobile)from);

                int totalSeconds = (int)ts.TotalSeconds;
                int totalHours = (totalSeconds + 3599) / 3600;
                int totalMinutes = (totalSeconds + 59) / 60;

                vendor.SayTo(from, "Terei trabalho em " + totalMinutes.ToString() + " minutos"); // An offer may be available in about ~1_minutes~ minutes.
            }
        }
    }
}
