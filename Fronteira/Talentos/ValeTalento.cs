using Server.Items;
using Server.Mobiles;

namespace Server.Fronteira.Talentos
{
    public class ValeTalento : Item
    {
        public ValeTalento(Serial s) : base(s) { }
        /*
        [CommandProperty(AccessLevel.GameMaster)]
        public Talento Talento { get; set; }

        [Constructable]
        public ValeTalento(Talento t): base(0x14F0)
        {
            Talento = t;
            Name = "Diploma de Talento";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Use para ganhar +1 talento em " + Talento.ToString());
        }

        public ValeTalento(Serial s) : base(s) { }

        public override void OnDoubleClick(Mobile from)
        {
            var pl = from as PlayerMobile;
            if (pl == null)
                return;

            var nivel = pl.Talentos.GetNivel(Talento);

            if(nivel >= 3)
            {
                pl.SendMessage("Talento ja esta no nivel maximo");
                return;
            }

            nivel++;
            pl.Talentos.SetNivel(Talento, nivel);
            pl.SendMessage("Seu nivel no talento " + Talento.ToString() + " aumentou em 1");
            pl.SendMessage("Nivel em " + Talento.ToString() + ": " + nivel + "/3");
            pl.SendMessage("Use .talentos para ver seus talentos");
            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 78, 0, 5060, 0);
            Effects.PlaySound(from.Location, from.Map, 0x243);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 78, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 78, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 78, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

            Effects.SendTargetParticles(from, 0x375A, 35, 90, 78, 0x00, 9502, (EffectLayer)255, 0x100);

        }
        */

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            //writer.Write((int)Talento);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var v = reader.ReadInt();
            reader.ReadInt();
        }
    }
}
