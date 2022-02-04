using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System.Linq;

namespace Server.Fronteira.Elementos
{

    public class EstatuaElemental : Item
    {
        [Constructable]
        public EstatuaElemental()
            : base(0xA725)
        {
            Name = "Estatua Elemental";
            Weight = 200;
            Movable = false;
        }

        public EstatuaElemental(Serial serial)
            : base(serial)
        {
        }

        public static void Envia(Mobile from)
        {
            var pl = from as PlayerMobile;
            if (pl == null)
                return;

            from.SendMessage("Voce sente uma energia muito forte emandando desta estatua...");
            /*
            if (from.Skills.Total < 4000)
            {
                from.SendMessage("Voce ainda tem a aprender para poder compreender esta estatua. [Skills " + from.Skills.Total/10 + "/400]");
                return;
            }
            */
            if (pl.Nivel <= 1)
            {
                pl.SendGump(new SemElementoGump());
            }
            else
            {
                if(!pl.IsCooldown("ele"))
                {
                    pl.SetCooldown("ele");
                    if (!pl.Elementos.GetNiveis().Any(n => n > 0)) {
                        pl.SendMessage(78, "Equipe armaduras de algum elemento e mate monstros para ganhar exp neste elemento.");
                        pl.SendMessage(78, "Use pedras preciosas com a skill Imbuing em armaduras para adicionar elementos.");
                    }
                }
                pl.SendGump(new ElementosGump(pl));
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            Envia(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

