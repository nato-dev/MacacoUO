using Server.Commands;
using Server.Mobiles;
using Server.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Comandos
{
    class FixSpawners
    {
        public static void Initialize()
        {
            CommandSystem.Register("fixspawnersdg", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        public static void CMD(CommandEventArgs arg)
        {
            foreach (var item in World.Items.Values)
            {
                if(item.Map == Map.Trammel && item is XmlSpawner)
                {
                    var spawner = (XmlSpawner)item;
                    var region = spawner.GetRegion();
                    if(region is DungeonRegion)
                    {
                        if (spawner.MaxDelay <= TimeSpan.FromMinutes(50))
                            spawner.MaxDelay = TimeSpan.FromMinutes(50);
                    }
                }
            }
            arg.Mobile.SendMessage("Spawners arrumados");
          
        }

    }
}
