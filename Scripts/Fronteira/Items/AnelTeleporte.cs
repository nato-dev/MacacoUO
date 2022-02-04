using Server.Items;
using Server.Misc.Custom;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using System;

namespace Server.Ziden
{
    public class AnelTeleporte : BaseRing
    {

        [Constructable]
        public AnelTeleporte() : base(0x108a)
        {
            this.Name = "Anel de Teleporte";
            Usos = 1;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Usos { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Local { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map Mapa { get; set; }

        public AnelTeleporte(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Usos: " + Usos);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if(SpellHelper.CheckCombatFast(from))
            {
                from.SendMessage("Voce nao consegue usar este item no calor da batalha");
                return;
            }

            Usos--;
            InvalidateProperties();
            if(Usos<=0)
            {
                Consume();
            }

            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
            Effects.PlaySound(from.Location, from.Map, 0x243);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 78, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 78, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 78, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

            Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 78, 9502, (EffectLayer)255, 0x100);
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if(from.Alive)
                {
                    BaseCreature.TeleportPets(from, Local, Mapa);
                    from.MoveToWorld(Local, Mapa);
                    from.OverheadMessage("* poof *");
                    from.SendMessage("Voce sente seu corpo sendo esvanecido");
                }
            });

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
            writer.Write(Local);
            writer.Write(Mapa);
            writer.Write(Usos);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var v = reader.ReadInt();
            Local = reader.ReadLocation();
            Mapa = reader.ReadMap();
            Usos = reader.ReadInt();
        }
    }
}
