using Server.Items;
using System;

namespace Server.Ziden.Dungeons.Goblins.Quest
{
    public class CaixaDeGold : Item
    {
        public int Apertos = 0;

        [Constructable]
        public CaixaDeGold(): base(0xE7E)
        {
            Name = "Caixa Definitivamente Nada Convencional";
        }

        public CaixaDeGold(Serial s):base(s)
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            if(Apertos == 0)
            {
                from.OverheadMessage("* futucando em uma caixa *");
                this.PrivateOverheadMessage(Network.MessageType.Regular, 0, true, "* cleck. *", from.NetState);
                from.SendMessage("Voce comeceu a apertar a caixa de todas as formas, a caixa fez barulhos estranhos. Talvez tentar apertar ela mais vezes ?");
            } else if(Apertos == 1)
            {
                this.PrivateOverheadMessage(Network.MessageType.Regular, 0, true, "* clock! *", from.NetState);
                from.SendMessage("Voce aperta os lados, tenta contorcer a caixa, ela faz outro barulho.");
            }
            else if (Apertos == 2)
            {
                this.PrivateOverheadMessage(Network.MessageType.Regular, 0, true, "* cluck.. *", from.NetState);
                from.SendMessage("Voce aperta os lados, tenta contorcer a caixa, ela faz outro barulho.");
            } else if(Apertos == 3)
            {
                from.SendMessage("A caixa explode em moedas de ouro. A explosao foi forte a ponto de algumas moedas cairem fora de sua mochila");
                this.Consume();
                from.AddToBackpack(new Gold(10000));
                Map map = from.Map;

                double delay = 0;
                if (map != null)
                {
                    for (int x = -2; x <= 2; ++x)
                    {
                        for (int y = -2; y <= 2; ++y)
                        {
                            double dist = Math.Sqrt(x * x + y * y);
                            Shard.Debug("Dist " + dist);
                            if (dist <= 8 && Utility.RandomBool())
                            {
                                new GoldTimer(from, map, from.X + x, from.Y + y, Location.Z, delay).Start();
                                delay += 0.15;
                            }
                        }
                    }
                }
            }
            Effects.PlaySound(this.Location, this.Map, 0x3E8);
            Apertos++;
            base.OnDoubleClick(from);
        }

        public class GoldTimer : Timer
        {
            private Map m_Map;
            private int m_X, m_Y;
            private int z;
            private Mobile m;

            public GoldTimer(Mobile from, Map map, int x, int y, int z, double delay) : base(TimeSpan.FromSeconds(delay))
            {
                m = from;
                m_Map = map;
                m_X = x;
                m_Y = y;
                this.z = z;
            }

            protected override void OnTick()
            {
                //int z = m_Map.GetAverageZ(m_X, m_Y);
                bool canFit = m_Map.CanFit(m_X, m_Y, z, z, false, false);

                for (int i = -3; !canFit && i <= 3; ++i)
                {
                    canFit = m_Map.CanFit(m_X, m_Y, z + i, z, false, false);

                    if (canFit)
                        z += i;
                }

                Shard.Debug("Fitting : " + canFit);

                if (!canFit)
                    return;

                Gold g = new Gold(100, 150);
                g.MoveToWorld(new Point3D(m_X, m_Y, z), m_Map);
                Effects.SendMovingEffect(m, g, 0xEEF, 25, 2, true, false);

                if (0.3 >= Utility.RandomDouble())
                {
                    //Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                    Effects.PlaySound(g, g.Map, 0x2E6);
                }
            }
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
