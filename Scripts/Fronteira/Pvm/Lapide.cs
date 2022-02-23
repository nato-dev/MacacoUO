using Server.Mobiles;
using System;

namespace Server.Ziden.Items
{
    public class LapideBoss : Item
    {
        public TimeSpan Tempo;

        public LapideBoss(BaseCreature morreu) : base(0x1173)
        {
            if (!morreu.IsBoss)
                return;
            Movable = false;
            if(morreu.Spawner is XmlSpawner)
            {
                var s = morreu.Spawner as XmlSpawner;
                s.Grave = this;
                foreach(var o in s.SpawnObjects)
                {
                    if(o.TypeName == morreu.GetType().Name)
                    {
                        Timer.DelayCall(TimeSpan.FromMilliseconds(10), () =>
                        {
                            Tempo = (o.NextSpawn - DateTime.UtcNow);
                        });
                       
                    }
                }
            }
            Name = "Lapide de "+morreu.Name;
            Hue = 1161;
        }

        public LapideBoss(Serial s) : base(s)
        {

        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            var t = Tempo;
            int weeks = (int)t.Days / 7;
            int days = t.Days;
            int hours = t.Hours;
            int minutes = t.Minutes;
            list.Add("Lapide de BOSS");
            list.Add("Tempo de Respawn");

            if (weeks > 1)
                list.AddTwoValues("Semanas", (t.Days / 7).ToString()); // Lifespan: ~1_val~ weeks
            else if (days > 1)
                list.AddTwoValues("Dias", t.Days.ToString()); // Lifespan: ~1_val~ days
            else if (hours > 1)
                list.AddTwoValues("Horas", t.Hours.ToString()); // Lifespan: ~1_val~ hours
            else if (minutes > 1)
                list.AddTwoValues("Minutos", t.Minutes.ToString()); // Lifespan: ~1_val~ minutes
            else
                list.AddTwoValues("Segundos", t.Seconds.ToString()); // Lifespan: ~1_val~ seconds
        }


        public override void OnDoubleClick(Mobile from)
        {
            var t = Tempo;
            int weeks = (int)t.Days / 7;
            int days = t.Days;
            int hours = t.Hours;
            int minutes = t.Minutes;
            if (hours > 1)
                from.SendMessage("Respawn do boss - Horas: " + t.Hours.ToString()); // Lifespan: ~1_val~ hours
            else if (minutes > 1)
                from.SendMessage("Respawn do boss - Minutos: " + t.Minutes.ToString());
            else
                from.SendMessage("Respawn do boss - Segundos: " + t.Seconds.ToString());
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
