using Server.Mobiles;
using System;

namespace Server.Ziden.Items
{
    public class LapideBoss : Item
    {

   

        public TimeSpan Tempo;
        public XmlSpawner spawner;
        public string Nome;

        public LapideBoss(BaseCreature morreu) : base(0x1173)
        {
            if (!morreu.IsBoss)
                return;

            Movable = false;
            if (morreu.Spawner is XmlSpawner)
            {
                Shard.Debug("Tem spawner", morreu);
                var s = morreu.Spawner as XmlSpawner;
                Nome = morreu.GetType().Name;
                spawner = s;
            }
            Name = "Lapide de " + morreu.Name;
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
            list.Add("Clique para saber o tempo de respawn do boss");
        }

        public override void OnDoubleClick(Mobile from)
        {
            foreach (var o in spawner.SpawnObjects)
            {
                if (o.TypeName.ToLower() == Nome.ToLower())
                {
                    Tempo = (o.NextSpawn - DateTime.UtcNow + spawner.NextSpawn);
                }
            }

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
            writer.Write(0);
            writer.Write(spawner);
            writer.Write(Nome);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var v = reader.ReadInt();
            spawner = reader.ReadItem() as XmlSpawner;
            Nome = reader.ReadString();
        }

    }
}
