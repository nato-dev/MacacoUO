using System;
using Server.Mobiles;
using Server.Multis;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Regions;

namespace Server.Commands
{
    public class Trancar
    {
        public static void Initialize()
        {
            CommandSystem.Register("Trancar", AccessLevel.Player, OnCommand);
        }

        public static void OnCommand(CommandEventArgs t)
        {
            Mobile from = t.Mobile;
            HouseRegion region = Region.Find(from.Location, from.Map) as HouseRegion;

            if (region == null)
            {
                from.SendMessage(0x00FE, "Você precisa estar em sua casa para usar este comando.");
                return;
            }

            BaseHouse House = region.House;

            if (House == null)
            {
                from.SendMessage(0x00FE, "Você não possui uma casa colocada.");
                return;
            }

            bool isOwner = House.IsOwner(from);
            bool isCoOwner = isOwner || House.IsCoOwner(from);
            bool isFriend = isCoOwner || House.IsFriend(from);

            if (isOwner || isCoOwner || isFriend)
            {
                from.SendMessage("O que deseja trancar?"); // Lock what down?
                from.Target = new LockdownTarget(false, House);

            }
            else
            {
                from.SendMessage("Você não tem permissão para usar este comando.");
            }
        }
    }
}
