using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class FerramentasDaMamae : Item
    {
        [Constructable]
        public FerramentasDaMamae() 
            : base(0x1EB9)
        {
            Name = "Ferramentas da Mamãe";
            this.Hue = 78;
            this.Weight = 1;
        }

        public FerramentasDaMamae(Serial serial)
            : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            if(parent is Backpack && ((Backpack)parent).Parent is PlayerMobile)
            {
                var p = ((Backpack)parent).Parent as PlayerMobile;
                if(p.QuestArrow != null)
                {
                    p.QuestArrow.Stop();
                    p.QuestArrow = null;
                }
                
            }
            base.OnAdded(parent);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Um kit de ferramentas excepcionalmente polido - parece ser usado por um expert.");
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
