using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class Cotton : Item, IDyable, ICommodity
    {
        [Constructable]
        public Cotton()
            : this(1)
        {
        }

        [Constructable]
        public Cotton(int amount)
            : base(0xDF9)
        {
            this.Stackable = true;
            this.Weight = 4.0;
            this.Amount = amount;
            this.Name = "Algodao";
        }

        public Cotton(Serial serial)
            : base(serial)
        {
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (this.Deleted)
                return false;

            this.Hue = sender.DyedHue;

            return true;
        }


        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }


        public static void OnSpun(ISpinningWheel wheel, Mobile from, int hue)
        {
            Item item = new SpoolOfThread(3);
            item.Hue = hue;

            from.AddToBackpack(item);
            from.SendMessage("Voce colocou fios de coser em sua mochila"); // You put the spools of thread in your backpack.

            var player = (PlayerMobile)from;
            if (player.LastItem != null && player.LastTarget != null)
            {
                new PickWheelTarget((Cotton)player.LastItem).Invoke(from, player.LastTarget);
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
            if (this.IsChildOf(from.Backpack))
            {
                from.SendMessage("Selecione uma roda de tecer"); // What spinning wheel do you wish to spin this on?
                from.Target = new PickWheelTarget(this);
            }
            else
            {
                from.SendMessage("O item precisa estar em sua mochila"); // That must be in your pack for you to use it.
            }
        }

        private class PickWheelTarget : Target
        {
            private readonly Cotton m_Flax;
            public PickWheelTarget(Cotton flax)
                : base(3, false, TargetFlags.None)
            {
                this.m_Flax = flax;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (this.m_Flax.Deleted)
                    return;

                ISpinningWheel wheel = targeted as ISpinningWheel;

                if (wheel == null && targeted is AddonComponent)
                    wheel = ((AddonComponent)targeted).Addon as ISpinningWheel;

                if (from is PlayerMobile)
                {
                    var player = (PlayerMobile)from;
                    player.LastTarget = targeted;
                    player.LastItem = m_Flax;
                }

                if (wheel is Item)
                {
                    Item item = (Item)wheel;

                    if (!this.m_Flax.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("Precisa estar em sua mochila"); // That must be in your pack for you to use it.
                    }
                    else if (wheel.Spinning)
                    {
                        from.SendMessage("Esta roda ja esta sendo usada"); // That spinning wheel is being used.
                    }
                    else
                    {
                        this.m_Flax.Consume();
                        from.PlayAttackAnimation();
                        wheel.BeginSpin(new SpinCallback(Cotton.OnSpun), from, this.m_Flax.Hue);
                    }
                }
                else
                {
                    from.SendMessage("Use isto em uma roda de tecer"); // Use that on a spinning wheel.
                }
            }
        }
    }
}

