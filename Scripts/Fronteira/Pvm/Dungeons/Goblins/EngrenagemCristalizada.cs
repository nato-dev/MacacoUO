using Server.Mobiles;
using Server.Targeting;
using System;

namespace Server.Items
{
    public class EngrenagemCristalizada : BaseDecayingItem
    {
        [Constructable]
        public EngrenagemCristalizada()
            : base(0x1053)
        {
            Name = "Engrenagem Cristalizada";
            this.LootType = LootType.Regular;
            this.Hue = 0x480;
            this.Weight = 2;
        }

        public override int Lifespan { get { return 21600/6; } }
        public override bool UseSeconds { get { return false; } }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Selecione aonde deseja colocar isto");
            from.Target = new InternalTarget(this);
        }

        public class InternalTarget : Target
        {
            Item i;

            public InternalTarget(Item i) : base(2, true, TargetFlags.None)
            {
                this.i = i;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is MaquinaEstranha)
                {
                    from.SendMessage("Voce colocou a engrenagem na maquina estranha");
                    i.Consume();
                    var maq = (MaquinaEstranha)targeted;

                    maq.PublicOverheadMessage(Network.MessageType.Regular, 32, true, "ALERTA ALERTA ALERTA");
                    maq.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* fazendo barulhos estranhos *");
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => {
                        maq.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* girando engrenagens *");
                    });
                    Timer.DelayCall(TimeSpan.FromSeconds(4), () => {
                        maq.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* revirando parafusos *");
                    });
                    Timer.DelayCall(TimeSpan.FromSeconds(6), () => {
                        maq.PublicOverheadMessage(Network.MessageType.Regular, 32, true, "ALERTA ALERTA ALERTA");
                        maq.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* subindo plataforma *");
                    });
                    Timer.DelayCall(TimeSpan.FromSeconds(8), () => {
                        maq.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* abrindo plataforma *");
                    });
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                        maq.PublicOverheadMessage(Network.MessageType.Regular, 32, true, "ALERTA ALERTA ALERTA");
                    });
                    Timer.DelayCall(TimeSpan.FromSeconds(12), () => {
                        var mob = new GolemMecanico();
                        mob.MoveToWorld(maq.Location, maq.Map);
                        mob.OverheadMessage("* vapor, engrenagens e medo *");
                    });
                }
            }
        }


        public EngrenagemCristalizada(Serial serial)
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
