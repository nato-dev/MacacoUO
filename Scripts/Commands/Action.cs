#region References
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Server.Commands.Generic;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.Network;
#endregion

namespace Server.Commands
{
    public class ActionCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("Action", AccessLevel.Administrator, OnAction);
            CommandSystem.Register("staticaqui", AccessLevel.Administrator, OnStatics);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            var action = e.GetInt32(0);
            e.Mobile.Animate(AnimationType.Attack, action);
        }

        [Usage("Staticsaqui")]
        private static void OnStatics(CommandEventArgs e)
        {
            var m = e.Mobile;
            var statics = m.Map.Tiles.GetStaticTiles(m.X, m.Y, true);
            foreach(var t in statics)
            {
                var id = TileData.ItemTable[t.ID & TileData.MaxItemValue];
                var flags = id.Flags;
                var height = id.CalcHeight;

                m.SendMessage($"ID:{id} | Flags: {flags} | Altura:{height}");
            }
        }
    }
}
