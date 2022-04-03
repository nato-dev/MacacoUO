using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Ziden.Items
{
    public class LapideBoss : Item
    {
        private static HashSet<LapideBoss> lapides = new HashSet<LapideBoss>();

        [CommandProperty(AccessLevel.Administrator)]
        public string Killer { get; set; }

        [CommandProperty(AccessLevel.Administrator)]
        public XmlSpawner spawner { get; set; }

        [CommandProperty(AccessLevel.Administrator)]
        public string Nome { get; set; }

        public static void BixoNasce(BaseCreature bc)
        {
            foreach (var lapide in new List<LapideBoss>(lapides))
            {
                if (lapide.Nome.ToLower() == bc.GetType().Name.ToLower())
                {
                    lapide.Delete();
                }
            }
        }

        public LapideBoss(BaseCreature morreu) : base(0x1173)
        {
            if (!morreu.IsBoss)
                return;

            Movable = false;
            if (morreu.Spawner is XmlSpawner)
            {
                Shard.Debug("Tem spawner", morreu);
                var s = morreu.Spawner as XmlSpawner;
                Nome = morreu.GetType().Name.ToLower();
                spawner = s;
                Killer = morreu.LastKiller?.Name;
            }
            Name = "Lapide de " + morreu.Name;
            Hue = 1161;
            lapides.Add(this);
        }

        public LapideBoss(Serial s) : base(s)
        {

        }

        public override void OnAfterDelete()
        {
            lapides.Remove(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (Killer != null)
            {
                list.Add("Morto por " + Killer);
            }
            list.Add("Clique para saber o tempo de respawn do boss");
        }

        public override void OnDoubleClick(Mobile from)
        {
            foreach (var spawnObject in spawner.SpawnObjects)
            {
                if (spawnObject.TypeName.ToLower() == Nome.ToLower())
                {
                    var t = (spawnObject.NextSpawn - DateTime.UtcNow + spawner.NextSpawn);
                    if (t.TotalMinutes <= 0)
                        t = (DateTime.UtcNow - spawnObject.NextSpawn + spawner.NextSpawn);
                    from.SendMessage($"Respawn do boss aproximado - {t.TotalMinutes} minutos");
                    return;
                }
            }
            from.SendMessage("Algo errado nesta lapide, ela parece estar velha...");
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
            lapides.Add(this);
        }

    }
}
