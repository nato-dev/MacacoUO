using System;

namespace Server.Items
{
    public class RobeofRite : Robe
    {
        private int m_Lifespan;
        private Timer m_Timer;

        [Constructable]
        public RobeofRite() : base(0x1F03)
        {
            this.Name = "Sobretudo do Ritual";
            this.PartyLoot = true;
            this.Weight = 3;
            this.Hue = 2702;
            this.StrRequirement = 10;

            if (this.Lifespan > 0)
            {
                this.m_Lifespan = this.Lifespan;
                this.StartTimer();
            }
        }
        
        public override bool Nontransferable { get { return true; } }

        public RobeofRite(Serial serial) : base(serial)
        {
        }

        public virtual int Lifespan { get { return 604800/7; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TimeLeft
        {
            get { return this.m_Lifespan; }
            set
            {
                this.m_Lifespan = value;
                this.InvalidateProperties();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (this.Lifespan > 0)
            {
                TimeSpan t = TimeSpan.FromSeconds(this.m_Lifespan);

                int weeks = (int)t.Days / 7;
                int days = t.Days;
                int hours = t.Hours;
                int minutes = t.Minutes;

                if (weeks > 1)
                    list.Add(weeks.ToString()+" Semanas"); // Lifespan: ~1_val~ weeks
                else if (days > 1)
                    list.Add(days.ToString()+" Dias"); // Lifespan: ~1_val~ days
                else if (hours > 1)
                    list.Add(hours.ToString()+" Horas"); // Lifespan: ~1_val~ hours
                else if (minutes > 1)
                    list.Add(minutes.ToString()+ " Minutos"); // Lifespan: ~1_val~ minutes
                else
                    list.Add(this.m_Lifespan.ToString()+ " Segundos"); // Lifespan: ~1_val~ seconds
            }
        }

        public virtual void StartTimer()
        {
            if (this.m_Timer != null)
                return;

            this.m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), new TimerCallback(Slice));
            this.m_Timer.Priority = TimerPriority.OneSecond;
        }

        public virtual void StopTimer()
        {
            if (this.m_Timer != null)
                this.m_Timer.Stop();

            this.m_Timer = null;
        }

        public virtual void Slice()
        {
            this.m_Lifespan -= 10;

            this.InvalidateProperties();

            if (this.m_Lifespan <= 0)
                this.Decay();
        }

        public virtual void Decay()
        {
            if (this.RootParent is Mobile)
            {
                Mobile parent = (Mobile)this.RootParent;

                if (this.Name == null)
                    parent.SendLocalizedMessage(1072515, "#" + this.LabelNumber); // The ~1_name~ expired...
                else
                    parent.SendLocalizedMessage(1072515, this.Name); // The ~1_name~ expired...

                Effects.SendLocationParticles(EffectItem.Create(parent.Location, parent.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
                Effects.PlaySound(parent.Location, parent.Map, 0x201);
            }
            else
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
                Effects.PlaySound(this.Location, this.Map, 0x201);
            }

            this.StopTimer();
            this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)this.m_Lifespan);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            this.m_Lifespan = reader.ReadInt();

            this.StartTimer();
        }
    }
}

