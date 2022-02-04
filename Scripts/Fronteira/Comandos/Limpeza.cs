using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;

namespace Server.Commands
{
    public class Limpeza
    {
        public static void Initialize()
        {
            CommandSystem.Register("wipashard", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile;

            pl.SendMessage("Wipando jogadores");
            foreach (var pm in PlayerMobile.Instances)
            {
                pm.Delete();
            }

            pl.SendMessage("Wipando items de felucca");
            List<Item> del = new List<Item>();
            foreach (var item in World.Items.Values)
            {
                if (item.Map == Map.Felucca)
                    del.Add(item);
            }
            foreach (var i in del)
                i.Delete();

            pl.SendMessage("Limpando casas");
            List<BaseHouse> delHouse = new List<BaseHouse>();
            foreach (var h in BaseHouse.AllHouses)
            {
                delHouse.Add(h);
            }
            foreach (var h in delHouse)
                h.Delete();

            pl.SendMessage("Finalizado");
        }
    }
}
