
using Server.Services;
using System;

namespace Server.Ziden.Items
{
    public class CaliceRusso : Item
    {
        [Constructable]
        public CaliceRusso() : base(2483)
        {
            Name = "Calice Russo";
            Hue = 32;
        }

        public CaliceRusso(Serial s) : base(s) { }

        private bool bebendo = false;

        public void Anim(Mobile m)
        {
            m.PlaySound(0x031);

            if (m.Body.IsHuman && !m.Mounted)
            {

                m.Animate(AnimationType.Eat, 0);
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("20% Chance de Morrer");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (bebendo)
                return;

            from.OverheadMessage("* bebendo *");
            Anim(from);

            Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
            {
                Anim(from);
            });

            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
            {
                var rnd = Utility.Random(5);
                if (Utility.Random(5) == 1)
                {
                    from.SendMessage("O calice expele fiapos de metal que penetram sua garganta");
                    from.OverheadMessage("* bateu as botas *");
                    DamageNumbers.ShowDamage(150, from, from, 32);
                    from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    from.PlaySound(0x207);
                    from.Damage(150);
                }
                else
                {
                    Anim(from);
                    from.OverheadMessage("* burp *");
                    from.SendMessage("Voce degusta o conteudo do calice.");
                    from.SendMessage("Porem nao havia nada no calice, voce esta ficando louco.");
                }
                base.OnDoubleClick(from);
            });
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
