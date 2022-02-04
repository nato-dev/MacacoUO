using System;
using Server.Mobiles;
using Server.Multis;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Regions;
using Server.Spells;

namespace Server.Commands
{
    public class Expulsar
    {
        public static void Initialize()
        {
            CommandSystem.Register("Expulsar", AccessLevel.Player, Expulsar_OnCommand);
        }

        public static void Expulsar_OnCommand(CommandEventArgs t)
        {

            Mobile from = t.Mobile;
            HouseRegion region = Region.Find(from.Location, from.Map) as HouseRegion;

            if(from.IsCooldown("expulsar"))
            {
                from.SendMessage("Aguarde para poder expulsar alguem novamente, voce esta em combate");
                return;
            }

            if(SpellHelper.CheckCombat(from)) {
                from.SetCooldown("expulsar", TimeSpan.FromSeconds(10));
            }

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
                from.SendMessage("Selecione um alvo"); // Target the individual to eject from this house.
                from.Target = new HouseKickTarget(House);

            }
            else
            {
                from.SendMessage(0x00FE, "Você não tem permissão para usar este comando.");
            }
        }
    }
}
