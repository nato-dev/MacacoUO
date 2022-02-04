using Server.Items;
using Server.Misc.Custom;
using Server.Targeting;

namespace Server.Ziden
{
    public class PergaminhoCarregamento : Item
    {

        [Constructable]
        public PergaminhoCarregamento() : base(0x1F35)
        {
            this.Name = "Pergaminho do Carregamento";
            this.Stackable = true;
        }

        public PergaminhoCarregamento(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Usar aumenta a quantidade de items carregados");
            list.Add("+1 Item carregado na mochila");
        }

        public override void OnDoubleClick(Mobile from)
        {
            var bp = from.Backpack;
            if (bp == null)
                return;

            if(bp.MaxItems >= 200)
            {
                from.SendMessage("Sua mochila ja aguenta o maximo de items");
                return;
            }

            bp.MaxItems++;
            Consume(1);

            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
            Effects.PlaySound(from.Location, from.Map, 0x243);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

            Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);
            from.SendMessage("Sua mochila agora carrega um pouco mais de peso");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
