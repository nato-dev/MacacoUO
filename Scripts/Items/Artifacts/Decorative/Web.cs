using Server.Mobiles;
using System;

namespace Server.Items
{
    public class Web : Item
    {
		public override bool IsArtifact { get { return true; } }
        private static readonly int[] m_itemids = new int[]
        {
            0x10d7, 0x10d8, 0x10dd, 0x0EE3, 0x0EE4, 0x0EE5, 0x0EE6, 0x10D2, 0x10D3, 0x10D4
        };
        [Constructable]
        public Web()
            : base(m_itemids[Utility.Random(m_itemids.Length)])
        {
            Movable = false;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if(m is PlayerMobile)
            {
                if(!m.IsCooldown("pteia"))
                {
                    m.SetCooldown("pteia", TimeSpan.FromSeconds(3));
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendMessage("Voce se enrosca na teia de aranha");
                    m.OverheadMessage("* preso na teia *");
                } 
            }
            return true;
        }

        [Constructable]
        public Web(int itemid)
            : base(itemid)
        {
            Movable = false;
        }

        public Web(Serial serial)
            : base(serial)
        {
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
