using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Mobiles
{
    public delegate void OnSpawn(Mobile m, XmlSpawner spawner, XmlSpawner.SpawnObject ob);
    public delegate void OnKilled(Mobile m, XmlSpawner spawner, XmlSpawner.SpawnObject ob);

    public class SpawnEvents
    {
       
        public static event OnSpawn OnSpawn;

        public static void OnSpawnEvent(Mobile m, XmlSpawner spawner, XmlSpawner.SpawnObject ob)
        {
            OnSpawn?.Invoke(m, spawner, ob);
        }
    }
}
