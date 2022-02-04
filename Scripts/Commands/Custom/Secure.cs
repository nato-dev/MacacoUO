using System;
using Server.Mobiles;
using Server.Multis;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Regions;

namespace Server.Commands
{
    public class Secure
    {
        public static void Initialize()
        {
            CommandSystem.Register("Secure", AccessLevel.Player, Trancar_OnCommand);
        }

        public static void Trancar_OnCommand(CommandEventArgs t)
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

            if (isCoOwner)
            {
                from.SendMessage("Selecione um item para transformar em secure"); // Choose the item you wish to secure
                from.Target = new SecureTarget(false, House);
            }
            else
            {
                from.SendLocalizedMessage(502094); // You must be in your house to do this. 
            }
        }
    }
}
