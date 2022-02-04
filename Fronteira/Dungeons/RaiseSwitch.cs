using System;
using System.Collections.Generic;
using Server.Network;

namespace Server.Items
{
    public class RaiseSwitch : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public int Precisa { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LinkId { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Ativas
        {
            get
            {
                int ativadas = 0;
                Ativadas.TryGetValue(LinkId, out ativadas);
                if (ativadas < 0 && LinkId != 0)
                {
                    ativadas = 0;
                    Ativadas[LinkId] = ativadas;
                }
                    
                return ativadas;
            }
        }

        public static Dictionary<int, int> Ativadas = new Dictionary<int, int>();



        private RaisableItem m_RaisableItem;
        private ResetTimer m_ResetTimer;
        [Constructable]
        public RaiseSwitch()
            : this(0x1093)
        {

        }

        public RaiseSwitch(Serial serial)
            : base(serial)
        {
        }

        protected RaiseSwitch(int itemID)
            : base(itemID)
        {
            this.Movable = false;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public RaisableItem RaisableItem
        {
            get
            {
                return this.m_RaisableItem;
            }
            set
            {
                this.m_RaisableItem = value;
            }
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (!m.InRange(this, 2))
            {
                m.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Muito longe"); // I can't reach that.
                return;
            }

            if (this.RaisableItem != null && this.RaisableItem.Deleted)
                this.RaisableItem = null;

            var ativou = this.Flip(m);

            if (!ativou)
                return;

            if (this.RaisableItem != null)
            {
                if (this.RaisableItem.IsRaisable)
                {
                    this.RaisableItem.Raise();
                    m.LocalOverheadMessage(MessageType.Regular, 0x5A, true, "Voce escuta um barulho de algo se movimentando.");
                }
                else
                {
                    m.LocalOverheadMessage(MessageType.Regular, 0x5A, true, "Nada acontece.");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)1); // version

            writer.Write((Item)this.m_RaisableItem);
            writer.Write(LinkId);
            writer.Write(Precisa);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            this.m_RaisableItem = (RaisableItem)reader.ReadItem();
            if (version == 1)
            {
                LinkId = reader.ReadInt();
                Precisa = reader.ReadInt();
            }
            this.Reset();
        }

        protected virtual bool Flip(Mobile m)
        {
            var ativou = true;
            if (this.ItemID != 0x1093)
            {
                this.ItemID = 0x1093;

                this.StopResetTimer();
                if (Ativadas.ContainsKey(LinkId))
                    Ativadas[LinkId] = Ativadas[LinkId] - 1;
                ativou = false;
            }
            else
            {
                if (this.RaisableItem != null)
                {
                    if (this.RaisableItem.IsRaisable)
                    {
                        if (Precisa > 0 && LinkId != 0)
                        {
                            if (!Ativadas.ContainsKey(LinkId))
                                Ativadas[LinkId] = 1;
                            else
                                Ativadas[LinkId] = Ativadas[LinkId] + 1;

                            if (Ativas == Precisa)
                            {
                                Ativadas[LinkId] = 1;
                                ativou = true;
                            }
                            else
                            {
                                m.SendMessage("Voce ouve um circuito ativar porem nada acontece");
                                ativou = false;
                            }
                        }
                    } else
                    {
                        m.SendMessage("Parece que ainda existem engrenagens rodando");
                    }
                }

                this.ItemID = 0x1095;

                if (this.RaisableItem != null && this.RaisableItem.CloseDelay >= TimeSpan.Zero)
                    this.StartResetTimer(this.RaisableItem.CloseDelay);
                else
                    this.StartResetTimer(Precisa == 0 ? TimeSpan.FromMinutes(2.0) : TimeSpan.FromSeconds(4.0));
            }

            if (Ativadas.ContainsKey(LinkId))
                Shard.Debug("Ativadas: " + Ativadas[LinkId]);
            Effects.PlaySound(this.Location, this.Map, 0x3E8);
            return ativou;
        }

        protected void StartResetTimer(TimeSpan delay)
        {
            this.StopResetTimer();

            this.m_ResetTimer = new ResetTimer(this, delay);
            this.m_ResetTimer.Start();
        }

        protected void StopResetTimer()
        {
            if (this.m_ResetTimer != null)
            {
                this.m_ResetTimer.Stop();
                this.m_ResetTimer = null;
            }
        }

        protected virtual void Reset()
        {
            if (this.ItemID != 0x1093)
                this.Flip(null);
        }

        private class ResetTimer : Timer
        {
            private readonly RaiseSwitch m_RaiseSwitch;
            public ResetTimer(RaiseSwitch raiseSwitch, TimeSpan delay)
                : base(delay)
            {
                this.m_RaiseSwitch = raiseSwitch;

                this.Priority = ComputePriority(delay);
            }

            protected override void OnTick()
            {
                if (this.m_RaiseSwitch.Deleted)
                    return;

                this.m_RaiseSwitch.m_ResetTimer = null;

                this.m_RaiseSwitch.Reset();
            }
        }
    }

    public class DisappearingRaiseSwitch : RaiseSwitch
    {
        [Constructable]
        public DisappearingRaiseSwitch()
            : base(0x108F)
        {
        }

        public DisappearingRaiseSwitch(Serial serial)
            : base(serial)
        {
        }

        public int CurrentRange
        {
            get
            {
                return this.Visible ? 3 : 2;
            }
        }
        public override bool HandlesOnMovement
        {
            get
            {
                return true;
            }
        }
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (Utility.InRange(m.Location, this.Location, this.CurrentRange) || Utility.InRange(oldLocation, this.Location, this.CurrentRange))
                this.Refresh();
        }

        public override void OnMapChange()
        {
            if (!this.Deleted)
                this.Refresh();
        }

        public override void OnLocationChange(Point3D oldLoc)
        {
            if (!this.Deleted)
                this.Refresh();
        }

        public void Refresh()
        {
            bool found = false;
            IPooledEnumerable eable = GetMobilesInRange(CurrentRange);

            foreach (Mobile mob in eable)
            {
                if (mob.Hidden && mob.IsStaff())
                    continue;

                found = true;
                break;
            }
            eable.Free();
            this.Visible = found;
        }

        public override void Serialize(GenericWriter writer)
        {
            if (this.RaisableItem != null && this.RaisableItem.Deleted)
                this.RaisableItem = null;

            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Refresh));
        }

        protected override bool Flip(Mobile m)
        {
            return base.Flip(m);
        }

        protected override void Reset()
        {
        }
    }
}
