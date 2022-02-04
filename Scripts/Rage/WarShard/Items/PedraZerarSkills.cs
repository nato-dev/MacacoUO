using Server.Gumps;

namespace Server.Items
{
    public class PedraZerarSkills : Item
    {
        [Constructable]
        public PedraZerarSkills()
            : base(0xED4)
        {
            this.Name = "Pedra de Zerar Skills";
            this.Movable = false;
            this.Weight = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this, 8))
            {
                from.SendGump(new PedraZerarSkillsGump(from));
            }
            else
            {
                from.SendMessage(0x00FE, "Você está muito longe.");
            }
        }

        public PedraZerarSkills(Serial serial)
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
