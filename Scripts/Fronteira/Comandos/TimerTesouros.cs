using Server.Items;
using Server.Mobiles;
using Server.Regions;
using System.Collections.Generic;

namespace Server.Commands
{
    public class TimerTesouros
    {
        public static void Initialize()
        {
            CommandSystem.Register("timertesouros", AccessLevel.Owner, new CommandEventHandler(CMD));
        }

        [Description("Altera timers de todos tesouros do shard.")]
        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            pl.SendMessage("Alterando tesouros...");
            var Spawners = new List<XmlSpawner>();
            foreach (var item in World.Items.Values)
            {
                if (item.Map == Map.Trammel && item is XmlSpawner)
                {
                    var spawner = (XmlSpawner)item;
                    var region = spawner.GetRegion();
                    if (region is DungeonRegion)
                    {
                        foreach (var obj in spawner.m_SpawnObjects)
                        {
                            if (obj.TypeName.Contains("Treasure"))
                            {
                                spawner.MinDelay = System.TimeSpan.FromMinutes(30);
                                spawner.MaxDelay = System.TimeSpan.FromHours(2);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
