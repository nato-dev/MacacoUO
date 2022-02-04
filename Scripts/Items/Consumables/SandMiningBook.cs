using System;
using Server.Mobiles;

namespace Server.Items
{
    public class SandMiningBook : Item
    {
        [Constructable]
        public SandMiningBook()
            : base(0xFF4)
        {
            Name = "Manual Minerar Areia";
            Weight = 2.0;
        }

        public SandMiningBook(Serial serial)
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

            if (Weight != 2.0)
            {
                Weight = 2.0;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm == null)
            {
                return;
            }

            if (!IsChildOf(pm.Backpack))
            {
                pm.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }

            else if (pm.Skills[SkillName.Mining].Base < 60)
            {
                pm.SendLocalizedMessage("Voce precisa de 60 mining para aprender isto"); // Only a Grandmaster Miner can learn from this book.
            }
            else if (pm.SandMining)
            {
                pm.SendLocalizedMessage("Voce ja aprendeu isso"); // You have already learned this information.
            }
            else
            {
                pm.SandMining = true;
                pm.SendLocalizedMessage("Voce aprendeu a minerar areia"); // You have learned how to mine fine sand.  Target sand areas when mining to look for fine sand.

                Delete();
            }
        }
    }
}
