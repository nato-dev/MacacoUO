using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class CoresRaras
    {
        public static void Initialize()
        {
            CommandSystem.Register("coresraras", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            if (pl == null)
                return;

            var bag = new MetalChest();
            bag.Name = "Bau de Cores Raras";
            foreach(var cor in Loot.RareHues)
            {
                var robe = new Robe();
                robe.Name = "Cor " + cor;
                robe.Hue = cor;
                bag.AddItem(robe);
            }
            pl.AddToBackpack(bag);
            pl.SendMessage("Addei");
        }
    }
}
