using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class MechanicalLifeManual : Item
    {

        [Constructable]
        public MechanicalLifeManual()
            : base(0xFF4)
        {
            Name = "Manual de Mecanica";
            Weight = 2.0;
        }

        public MechanicalLifeManual(Serial serial)
            : base(serial)
        {
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

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (pm == null || from.Skills[SkillName.Tinkering].Base < 80)
            {
                from.SendMessage("Voce precisa de pelo menos 80 tinkering para isto"); // Only a Grandmaster Tinker can learn from this book.
            }
            else if (pm.MechanicalLife)
            {
                pm.SendMessage("Voce ja sabe isto"); // You have already learned this information.
            }
            else
            {
                pm.MechanicalLife = true;
                pm.SendMessage("Voce aprendeu como construir engenhocas mecanicas"); // You have learned how to build mechanical companions.

                Delete();
            }
        }
    }
}
