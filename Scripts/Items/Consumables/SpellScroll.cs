using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
    public class SpellScroll : Item, ICommodity
    {
        private int m_SpellID;
        public SpellScroll(Serial serial)
            : base(serial)
        {
        }

        [Constructable]
        public SpellScroll(int spellID, int itemID)
            : this(spellID, itemID, 1)
        {
        }

        [Constructable]
        public SpellScroll(int spellID, int itemID, int amount)
            : base(itemID)
        {
            this.Stackable = true;
            this.Weight = 1.0;
            this.Amount = amount;

            this.m_SpellID = spellID;
            Timer.DelayCall(TimeSpan.FromMilliseconds(100), () =>
            {
                var spell = SpellRegistry.NewSpell(this.m_SpellID, null, this) as MagerySpell;
                if(spell != null)
                {
                    switch((int)spell.Circle)
                    {
                        case 0: Hue = 0; break;
                        case 1: Hue = 300; break;
                        case 2: Hue = 310; break;
                        case 3: Hue = 320; break;
                        case 4: Hue = 330; break;
                        case 5: Hue = 340; break;
                        case 6: Hue = TintaPreta.COR; break;
                        case 7: Hue = TintaBranca.COR; break;
                    }
                }
            });
        }

        public int SpellID
        {
            get
            {
                return this.m_SpellID;
            }
        }
        TextDefinition ICommodity.Description
        {
            get
            {
                return this.LabelNumber;
            }
        }
        bool ICommodity.IsDeedable
        {
            get
            {
                return (Core.ML);
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)this.m_SpellID);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 0:
                    {
                        this.m_SpellID = reader.ReadInt();

                        break;
                    }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive && this.Movable)
                list.Add(new ContextMenus.AddToSpellbookEntry());
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Multis.DesignContext.Check(from))
                return; // They are customizing

            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            #region SA
            else if (from.Flying && from is PlayerMobile && BaseMount.OnFlightPath(from))
            {
                from.SendLocalizedMessage(1113749); // You may not use that while flying over such precarious terrain.
                return;
            }
            #endregion

            Spell spell = SpellRegistry.NewSpell(this.m_SpellID, from, this);

            if (spell != null)
                spell.Cast();
            else
                from.SendLocalizedMessage(502345); // This spell has been temporarily disabled.
        }
    }
}
