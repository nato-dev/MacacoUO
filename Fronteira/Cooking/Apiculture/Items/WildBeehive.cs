using System;
using Server;
using System.Collections;
using System.Text;
using Server.Gumps;
using Server.Network;
using Server.Spells;
using Server.Mobiles;


namespace Server.Items
{
    public class WildBeehive : Item
    {
        private int m_UsesRemaining = Utility.RandomMinMax(1, 3);

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }


        [Constructable]
        public WildBeehive() : base(0x91A)
        {
            Name = "colmeia de abelhas";
            Movable = false;
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 1))
            {
                if (m_UsesRemaining == 1)
                {
                    from.AddToBackpack(new HoneyComb());
                    InvalidateProperties();
                    from.SendMessage("Voce pegou o ultimo favo de mel da colmeia");
                    this.Delete();
                }
                else
                {
                    from.AddToBackpack(new HoneyComb());
                    m_UsesRemaining -= 1;
                    InvalidateProperties();
                    from.SendMessage("Voce pegou um favo de mel da colmeia.");

                }
            }
            else
            {
                from.SendMessage("Voce esta muito longe.");
                return;
            }
        }
        public override bool HandlesOnMovement
        {
            get { return true; }
        }

        public WildBeehive(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.Write((int)m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_UsesRemaining = (int)reader.ReadInt();

        }
    }
}
