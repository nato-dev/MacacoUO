using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Templates
{
    public class TemplateStone : Item
    {
        [Constructable]
        public TemplateStone()
            : base(0xED4)
        {
            this.Movable = false;
            this.Hue = 78;
        }

        public TemplateStone(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Pedra de Templates";
            }
        }
        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new TemplatesGump((PlayerMobile)from));
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
