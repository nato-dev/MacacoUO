using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Pvm
{
    public class Lapide : Item
    {
        XmlSpawner spawner;

        public Lapide(BaseCreature creature): base(1)
        {
            if(creature.Spawner is XmlSpawner)
            {
              
                var spawner = (XmlSpawner)creature.Spawner;
                this.spawner = spawner;
                var next = DateTime.UtcNow + spawner.NextSpawn;
                var faltaSegundos = DateTime.Now - next;
            }    
        }

        public Lapide(Serial s) : base(s) { }

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
