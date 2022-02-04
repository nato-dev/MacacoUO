using System;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class PedraHall : Item
    {
        [Constructable]
        public PedraHall()
            : base(0xED4)
        {
            this.Name = "Pedra para voltar ao Hall";
            this.Movable = false;
            this.Weight = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this, 8))
            {
                if (SpellHelper.CheckCombatFast(from))
                {
                    from.SendMessage(0x00FE, "Você não pode se teletransportar no calor da batalha.");
                }
                else if (from.BeginAction(typeof(PedraHall)))
                {
                    var tempo = TimeSpan.FromSeconds(4);
                    from.SendMessage(0x00FE, "Você sente uma aura mágica envolvendo seu corpo.");
                    from.Freeze(tempo);
                    from.PlaySound(0x244);
                    from.FixedEffect(0x3740, 5, 100);
                    Timer.DelayCall(tempo, m => Transporta(m), from);
                }
            }
            else
            {
                from.SendMessage(0x00FE, "Você está muito longe.");
            }
        }

        public static void Transporta(Mobile m)
        {

            var hall = new Point3D(989, 519, -50);
            BaseCreature.TeleportPets(m, hall, Map.Malas);
            m.MoveToWorld(hall, Map.Malas);
          
            m.Frozen = false;
            m.SendMessage(0x00FE, "Você foi teletransportado para o Hall.");
            m.EndAction(typeof(PedraHall));
        }

        public PedraHall(Serial serial)
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
