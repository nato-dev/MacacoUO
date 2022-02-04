using System;
using Server.Mobiles;

namespace Server.Items
{
    public class BasketWeavingBook : Item
    {
        [Constructable]
        public BasketWeavingBook()
            : base(0xFBE)
        {
            this.Weight = 1.0;
        }

        public BasketWeavingBook(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Fazendo Valiosos Cestos";
            }
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

            if (pm == null)
            {
                return;
            }

            if (!IsChildOf(pm.Backpack))
            {
                pm.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (pm.Skills[SkillName.Tinkering].Base < 80)
            {
                pm.SendMessage("Voce precisa de pelo menos 80 tinker para aprender isto.");
            }
            else if (pm.BasketWeaving)
            {
                pm.SendMessage("Voce ja aprendeu isto.");
            }
            else
            {
                pm.BasketWeaving = true;
                pm.SendMessage("Voce aprendeu a fazer cestos. Voce vai precisar de jardineiros para cultivar plantas.");
                
                Delete();
            }
        }
    }
}
