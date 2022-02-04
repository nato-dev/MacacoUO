using Server.Gumps;
using Server.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Fama
{
    [Flipable(0x1E5E, 0x1E5F)]
    public class QuadroRanking : Item
    {
        [Constructable]
        public QuadroRanking()
            : base(0x1E5E)
        {
            Name = "Quadro de Ranking";
        }

        public QuadroRanking(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new Rankings(from, null));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
