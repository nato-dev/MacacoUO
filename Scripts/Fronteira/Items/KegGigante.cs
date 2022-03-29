using Server.Engines.Craft;
using System;

namespace Server.Items
{
    public class KegGigante : PotionKeg, ICraftable
    {

        [Constructable]
        public KegGigante()
            : base()
        {
            UpdateWeight();
            Name = "Barril de Pocoes Gigante";
        }

        public override int LabelNumber
        {
            get => 0;
        }

        public KegGigante(Serial serial)
            : base(serial)
        {
        }

        public override void UpdateWeight()
        {
            int held = Math.Max(0, Math.Min(m_Held, 1000));

            Weight = 10 + ((held * 40) / 1000);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Type);
            writer.Write((int)m_Held);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                case 0:
                    {
                        m_Type = (PotionEffect)reader.ReadInt();
                        m_Held = reader.ReadInt();

                        break;
                    }
            }

            if (version < 1)
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(UpdateWeight));
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            string number;

            if (m_Held > 0)
            {
                list.Add(m_Held / 10 + "% Cheio");
                list.Add(m_Type.ToString());
            }
            else
            {
                list.Add("Barril vazio");
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            int number;

            if (m_Held <= 0)
                number = 502246; // The keg is empty.
            else if (m_Held < 50)
                number = 502248; // The keg is nearly empty.
            else if (m_Held < 200)
                number = 502249; // The keg is not very full.
            else if (m_Held < 300)
                number = 502250; // The keg is about one quarter full.
            else if (m_Held < 400)
                number = 502251; // The keg is about one third full.
            else if (m_Held < 470)
                number = 502252; // The keg is almost half full.
            else if (m_Held < 540)
                number = 502254; // The keg is approximately half full.
            else if (m_Held < 700)
                number = 502253; // The keg is more than half full.
            else if (m_Held < 800)
                number = 502255; // The keg is about three quarters full.
            else if (m_Held < 960)
                number = 502256; // The keg is very full.
            else if (m_Held < 1000)
                number = 502257; // The liquid is almost to the top of the keg.
            else
                number = 502258; // The keg is completely full.

            LabelTo(from, number);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 2))
            {
                if (m_Held > 0)
                {
                    Container pack = from.Backpack;

                    if (pack != null && pack.ConsumeTotal(typeof(Bottle), 1))
                    {
                        from.SendLocalizedMessage(502242); // You pour some of the keg's contents into an empty bottle...

                        BasePotion pot = FillBottle();

                        if (pack.TryDropItem(from, pot, false))
                        {
                            //from.SendLocalizedMessage(502243); // ...and place it into your backpack.
                            from.PlaySound(0x240);

                            if (--Held == 0)
                                from.SendLocalizedMessage(502245); // The keg is now empty.
                        }
                        else
                        {
                            from.SendLocalizedMessage(502244); // ...but there is no room for the bottle in your backpack.
                            pot.Delete();
                        }
                    }
                    else
                    {
                        // TODO: Target a bottle
                    }
                }
                else
                {
                    from.SendLocalizedMessage(502246); // The keg is empty.
                }
            }
            else
            {
                from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (item is BasePotion)
            {
                BasePotion pot = (BasePotion)item;
                int toHold = Math.Min(1000 - m_Held, pot.Amount);

                if (pot.PotionEffect == PotionEffect.Darkglow || pot.PotionEffect == PotionEffect.Parasitic)
                {
                    from.SendLocalizedMessage(502232); // The keg is not designed to hold that type of object.
                    return false;
                }
                else if (toHold <= 0)
                {
                    from.SendLocalizedMessage(502233); // The keg will not hold any more!
                    return false;
                }
                else if (m_Held == 0)
                {
                    if (GiveBottle(from, toHold))
                    {
                        m_Type = pot.PotionEffect;
                        Held = toHold;

                        from.PlaySound(0x240);

                        from.SendLocalizedMessage(502237); // You place the empty bottle in your backpack.

                        item.Consume(toHold);

                        if (!item.Deleted)
                            item.Bounce(from);

                        return true;
                    }
                    else
                    {
                        from.SendLocalizedMessage(502238); // You don't have room for the empty bottle in your backpack.
                        return false;
                    }
                }
                else if (pot.PotionEffect != m_Type)
                {
                    from.SendLocalizedMessage(502236); // You decide that it would be a bad idea to mix different types of potions.
                    return false;
                }
                else
                {
                    if (GiveBottle(from, toHold))
                    {
                        Held += toHold;

                        from.PlaySound(0x240);

                        from.SendLocalizedMessage(502237); // You place the empty bottle in your backpack.

                        item.Consume(toHold);

                        if (!item.Deleted)
                            item.Bounce(from);

                        return true;
                    }
                    else
                    {
                        from.SendLocalizedMessage(502238); // You don't have room for the empty bottle in your backpack.
                        return false;
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(502232); // The keg is not designed to hold that type of object.
                return false;
            }
        }

    }
}
