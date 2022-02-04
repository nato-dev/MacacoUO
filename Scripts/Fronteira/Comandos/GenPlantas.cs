using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Commands
{
    public class GenPlantas
    {
        public static void Initialize()
        {
            CommandSystem.Register("genplantas", AccessLevel.Owner, new CommandEventHandler(CMD));
        }

        [Description("Gera spawners de recursos pelo mapa.")]
        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            pl.SendMessage("Gerando spawners de plantas em Trammel");
            var Spawners = new List<XmlSpawner>();
            foreach (var item in World.Items.Values)
            {
                if (item.Map == Map.Trammel && item is XmlSpawner)
                {
                    var spawner = (XmlSpawner)item;
                    var region = spawner.GetRegion();
                    if (region == null || region.Name == null)
                    {
                        if(region == null || region.Parent == null || region.Parent.Name == null)
                            Spawners.Add(spawner);
                    }
                }
            }

            foreach(var spawner in Spawners)
            {
                if(spawner.Name == "Spawner de Recursos")
                {
                    continue;
                }
                var novoSpawner = new XmlSpawner();
                var loc = spawner.Location;
                loc.Z += 5;
                novoSpawner.MoveToWorld(loc, spawner.Map);
                novoSpawner.Name = "Spawner de Plantas";
                novoSpawner.SpawnRange = spawner.SpawnRange * 5;
                novoSpawner.MaxCount = spawner.MaxCount * 5;
                if (novoSpawner.MaxCount < 1)
                    novoSpawner.MaxCount = 1;
                novoSpawner.DespawnTime = TimeSpan.FromHours(4);
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableCabbage", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableCarrot", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableCotton", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableFlax", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableLettuce", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableGarlic", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableGinseng", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableMandrakeroot", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableNightShade", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableOnion", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmablePumpkin", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableTurnip", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("FarmableWheat", novoSpawner.MaxCount));
                novoSpawner.Respawn();
            }
        }
    }
}
