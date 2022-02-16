using Server.Items;
using Server.Misc.Custom;
using Server.Targeting;
using Server.Mobiles;
using System;

namespace Server.Ziden
{

    public class SpecialLockPick : Item
    {
        [Constructable]
        public SpecialLockPick() : base(0x14fc)
        {
            this.Name = "Lock Pick Especial";
            this.Stackable = true;
            this.Weight = 1.0;
            this.Hue = (0xAA7);
        }

        public SpecialLockPick(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Você pode destrancar baús sem precisar da skill lockpicking");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("O que você quer destrancar?");
            from.Target = new InternalTarget(this);

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("Precisa estar em sua mochila");
            }
        }


        protected virtual void BeginLockpick(Mobile from, ILockpickable item)
        {
            if (item.Locked)
            {
                from.PlaySound(0x1F5);

                Timer.DelayCall(TimeSpan.FromMilliseconds(200.0), EndLockpick, new object[] { item, from });
                this.Consume();
                
            }
            else
            {
                from.SendMessage("Não está trancado");
            }
        }

        protected virtual void EndLockpick(object state)
        {
            object[] objs = (object[])state;
            ILockpickable lockpickable = objs[0] as ILockpickable;
            Mobile from = objs[1] as Mobile;

            Item item = (Item)lockpickable;

            if (!from.InRange(item.GetWorldLocation(), 1))

            
            from.Animate(AnimationType.Attack, 4);
            from.OverheadMessage("*Essa Ferramenta e boa mesmo*");
            from.MovingEffect(item, 0x374A, 2, 0, false, false);
            from.SendMessage("Você conseguiu destrancar o item");
            from.PlaySound(0x200);
            lockpickable.LockPick(from);

        }


        private class InternalTarget : Target
        {
            private SpecialLockPick m_Item;

            public InternalTarget(SpecialLockPick item) : base(2, false, TargetFlags.None)
            {
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Item.Deleted)
                    return;

                if (!this.m_Item.IsChildOf(from.Backpack))
                {
                    from.OverheadMessage("*A Ferramenta deve estar na sua mochila*");
                }

                else if (!(targeted is ILockpickable))
                {
                    from.SendLocalizedMessage("Voce nao pode destrancar isto");
                }

                else
                {
                    m_Item.BeginLockpick(from, (ILockpickable)targeted);
                    
                }
                                
            }
        }
    }
}
