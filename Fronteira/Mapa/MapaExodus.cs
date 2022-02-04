
using System;
using Server;
using Server.Items;
using Server.Regions;

namespace Shrink.ShrinkSystem
{


    public class MapaExodus : Item
    {
        public MapaExodus(Serial serial) : base(serial)
        {
        }

        [Constructable]
        public MapaExodus() : base(0x14EB)
        {
            Hue = TintaPreta.COR;
        }

        public override string DefaultName { get { return "Mapa Exodo"; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.QuestArrow != null)
            {
                from.SendMessage("Voce nao esta mais seguindo o mapa");
                from.QuestArrow.Stop();
                from.ClearQuestArrow();
            }
            else
            {
                if(from.Region != null && from.Region is DungeonRegion)
                {
                    from.SendMessage("Voce precisa sair deste local para conseguir ver o mapa direito");
                    return;
                }
                if (from.Map != Map.Felucca)
                {
                    from.SendMessage("Voce nao esta no mapa correto");
                    return;
                }
                from.OverheadMessage("* Olhando um mapa *");
                from.SendMessage("Voce esta seguindo o mapa");
                from.QuestArrow = new QuestArrow(from, new Point3D(799, 1037, 0));
                from.QuestArrow.Name = "mapa";
                from.QuestArrow.Update();
            }
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
