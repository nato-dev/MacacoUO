using System;
using Server.Spells;

namespace Server.Items
{
    public class PedraRefresh : Item
    {
        [Constructable]
        public PedraRefresh()
            : base(0xED4)
        {
            this.Name = "Pedra de Refresh";
            this.Movable = false;
            this.Weight = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this, 8))
            {
                if (from.Hits == from.HitsMax && from.Mana == from.ManaMax && from.Stam == from.StamMax)
                {
                    from.SendMessage(0x00FE, "Você já está plenamente recuperado.");
                }
                else if (SpellHelper.CheckCombatFast(from))
                {
                    from.SendMessage(0x00FE, "Você não pode se recuperar no calor da batalha.");
                }
                else if (from.BeginAction(typeof(PedraRefresh)))
                {
                    var tempo = TimeSpan.FromSeconds(5);
                    from.SendMessage(0x00FE, "Você permanece imóvel e sente sua energia revigorando aos poucos.");
                    from.Freeze(tempo);
                    Timer.DelayCall(tempo, m => Refresh(m), from);
                }
            }
            else
            {
                from.SendMessage(0x00FE, "Você está muito longe.");
            }
        }

        public static void Refresh(Mobile m)
        {
            m.Hits = m.HitsMax;
            m.Mana = m.ManaMax;
            m.Stam = m.StamMax;
            m.Frozen = false;
            m.SendMessage(0x00FE, "Você se sente totalmente recuperado.");
            m.EndAction(typeof(PedraRefresh));
        }

        public PedraRefresh(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
