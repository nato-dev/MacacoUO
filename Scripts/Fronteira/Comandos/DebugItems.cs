using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Scripts.Custom.Items;
using Server.Targeting;

namespace Server.Commands
{
    public class DebugItems
    {
        public static void Initialize()
        {
            CommandSystem.Register("contaitems", AccessLevel.Administrator, new CommandEventHandler(CONTA));
            CommandSystem.Register("limpaitems", AccessLevel.Administrator, new CommandEventHandler(LIMPA));
        }

        [Usage("contaitems")]
        [Description("conta os items do shard.")]
        public static void CONTA(CommandEventArgs arg)
        {
            var map = new Dictionary<Type, int>();
            arg.Mobile.SendMessage("DEBUGANDO");
            foreach(var item in World.Items.Values)
            {
                var curr = 0;
                map.TryGetValue(item.GetType(), out curr);
                curr++;
                map[item.GetType()] = curr;
            }
            foreach(var type in map.Keys)
            {
                Console.WriteLine(map[type] +" | "+type.Name);
            }
            arg.Mobile.SendMessage("DEBUGADO");
        }

        [Usage("limpaitems")]
        [Description("Camping Menu.")]
        public static void LIMPA(CommandEventArgs arg)
        {
            var map = new Dictionary<Type, int>();
            arg.Mobile.SendMessage("DEBUGANDO");
            int l = 0;
            var deletar = new List<Item>();
            foreach (var item in World.Items.Values)
            {
                if (item is DecoRelPor || item is BaseDecorationArtifact)
                {
                    if (item.Location == Point3D.Zero)
                    {
                        l++;
                        deletar.Add(item);
                    }
                    Console.WriteLine(item.ItemID + " - LOC " + item.Location + " - PAR " + (item.RootParent != null ? item.RootParent.GetType().Name : "null"));
                }
            }

            foreach (var i in deletar)
            {
                i.Delete();
            }

            arg.Mobile.SendMessage("LIMPEI " + l + " ITEMS");
        }
    }
}


