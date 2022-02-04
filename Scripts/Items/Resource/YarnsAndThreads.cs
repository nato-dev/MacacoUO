using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
    public abstract class BaseClothMaterial : Item, IDyable, ICommodity
    {
        public BaseClothMaterial(int itemID)
            : this(itemID, 1)
        {
        }

        public BaseClothMaterial(int itemID, int amount)
            : base(itemID)
        {
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
        }

        public BaseClothMaterial(Serial serial)
            : base(serial)
        {
        }

        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

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

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendMessage("Selecione um loom para isto"); // Select a loom to use that on.
                from.Target = new PickLoomTarget(this);
            }
            else
            {
                from.SendMessage("Precisa estar em sua mochila"); // That must be in your pack for you to use it.
            }
        }

        private static string[] phases = new string[] {
            "Voce comeca a criar tecido",
            "O tecido esta ganhando forma",
            "O tecido esta na metade",
            "O tecido parece estar quase pronto",
            "O tecido esta quase pronto"
        };

        private class PickLoomTarget : Target
        {
            private readonly BaseClothMaterial m_Material;
            public PickLoomTarget(BaseClothMaterial material)
                : base(3, false, TargetFlags.None)
            {
                m_Material = material;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Material.Deleted)
                    return;

                ILoom loom = targeted as ILoom;

                if (loom == null && targeted is AddonComponent)
                    loom = ((AddonComponent)targeted).Addon as ILoom;

                if (loom != null)
                {
                    ((PlayerMobile)from).LastTarget = targeted;
                    SpellHelper.Turn(from, loom);
                    if (!m_Material.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("O item precisa estar em sua mochila"); // That must be in your pack for you to use it.
                        return;
                    }
                    else if (loom.Phase < 4)
                    {
                        from.PlayAttackAnimation();
                        m_Material.Consume();
                        var phase = loom.Phase++;
                        if (targeted is Item)
                            ((Item)targeted).PrivateMessage(phases[phase], from);
                    }
                    else
                    {
                        from.PlayAttackAnimation();
                        Item create = new BoltOfCloth();
                        create.Amount = 1;
                        create.Hue = m_Material.Hue;

                        m_Material.Consume(1);
                        loom.Phase = 0;
                        from.SendMessage("Voce criou tecido e colocou " + create.Amount + " rolos de tecido em sua mochila"); // You create some cloth and put it in your backpack.
                        from.AddToBackpack(create);
                    }

                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        if (m_Material != null && m_Material.Amount > 0)
                        {
                            new PickLoomTarget(m_Material).Invoke(from, ((PlayerMobile)from).LastTarget);
                        }
                    });

                }
                else
                {
                    from.SendLocalizedMessage(500367); // Try using that on a loom.
                }
            }
        }
    }

    public class DarkYarn : BaseClothMaterial
    {
        [Constructable]
        public DarkYarn()
            : this(1)
        {
        }

        [Constructable]
        public DarkYarn(int amount)
            : base(0xE1D, amount)
        {
        }

        public DarkYarn(Serial serial)
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
    }

    public class LightYarn : BaseClothMaterial
    {
        [Constructable]
        public LightYarn()
            : this(1)
        {
        }

        [Constructable]
        public LightYarn(int amount)
            : base(0xE1E, amount)
        {
        }

        public LightYarn(Serial serial)
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
    }

    public class LightYarnUnraveled : BaseClothMaterial
    {
        [Constructable]
        public LightYarnUnraveled()
            : this(1)
        {
        }

        [Constructable]
        public LightYarnUnraveled(int amount)
            : base(0xE1F, amount)
        {
        }

        public LightYarnUnraveled(Serial serial)
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
    }

    public class SpoolOfThread : BaseClothMaterial
    {
        [Constructable]
        public SpoolOfThread()
            : this(1)
        {
        }

        [Constructable]
        public SpoolOfThread(int amount)
            : base(0xFA0, amount)
        {
            this.Name = "Fios de coser";
        }

        public SpoolOfThread(Serial serial)
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
    }
}
