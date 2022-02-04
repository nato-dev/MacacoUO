using System;
using Server.Mobiles;

namespace Server.Items
{
    public class GlassblowingBook : Item
    {
        [Constructable]
        public GlassblowingBook()
            : base(0xFF4)
        {
            Weight = 5.0;
            Name = "Manual Trabalhar com Vidro";
        }

        public GlassblowingBook(Serial serial)
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

            if (Weight != 5.0)
            {
                Weight = 5.0;
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
            else if (pm.Skills[SkillName.Alchemy].Base < 60)
            {
                pm.SendLocalizedMessage("Precisa ter 60 alchemy para isto"); // Only a Grandmaster Alchemist can learn from this book.
            }
            else if (pm.Glassblowing)
            {
                pm.SendLocalizedMessage("Voce ja aprendeu isto"); // You have already learned this information.
            }
            else
            {
                pm.Glassblowing = true;
                pm.SendLocalizedMessage("Voce aprendeu a trabalhar com vidros"); // You have learned to make items from glass.  You will need to find miners to mine fine sand for you to make these items.

                Delete();
            }
        }
    }
}
