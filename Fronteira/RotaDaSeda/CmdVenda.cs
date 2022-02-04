using Server.Commands;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.RotaDaSeda
{
    class CmdVenda
    {
        public static void FazVenda(PlayerMobile from)
        {
            Mobile v = new PlayerVendor(from, null);

            from.Frozen = true;
            from.Squelched = true;

            v.Direction = from.Direction & Direction.Mask;
            v.MoveToWorld(from.Location, from.Map);
            var toRemove = new List<Item>();
            foreach (var tr in v.Items)
                toRemove.Add(tr);
            foreach (var tr in toRemove)
                v.RemoveItem(tr);

            foreach (var i in from.FindEquippedItems<Item>())
            {
                var dupe = Dupe.DupeItem(i);
                v.EquipItem(dupe);
                v.Name = from.Name;
                v.Title = from.Title;
            }
        }
    }
}
