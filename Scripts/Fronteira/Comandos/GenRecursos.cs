using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Commands
{
    public class GenRecursos
    {
        public static void Initialize()
        {
            CommandSystem.Register("genrecursos", AccessLevel.Owner, new CommandEventHandler(CMD));
        }

        [Description("Gera spawners de recursos pelo mapa.")]
        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            pl.SendMessage("Gerando spawners de recursos em Trammel");
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
                if (spawner.Name == "Spawner de Plantas")
                {
                    continue;
                }
                var novoSpawner = new XmlSpawner();
                var loc = spawner.Location;
                loc.Z += 5;
                novoSpawner.MoveToWorld(loc, spawner.Map);
                novoSpawner.Name = "Spawner de Recursos";
                novoSpawner.SpawnRange = spawner.SpawnRange;
                novoSpawner.MaxCount = spawner.MaxCount * 2;
                if (novoSpawner.MaxCount < 1)
                    novoSpawner.MaxCount = 1;
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("Recurso", novoSpawner.MaxCount));
                novoSpawner.m_SpawnObjects.Add(new XmlSpawner.SpawnObject("Recurso,WOOD", novoSpawner.MaxCount));
                novoSpawner.Respawn();
            }
        }
    }
}
