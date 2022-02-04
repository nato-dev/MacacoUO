using System;
using Server.Mobiles;
using Server.Multis;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Regions;

namespace Server.Commands
{
    public class Sair
    {
        public static void Initialize()
        {
            CommandSystem.Register("Sair", AccessLevel.Player, TartarugaCommand);
        }

        public static void TartarugaCommand(CommandEventArgs t)
        {
            Mobile from = t.Mobile;
            HouseRegion region = Region.Find(from.Location, from.Map) as HouseRegion;

            if (region == null)
            {
                from.SendMessage(0x00FE, "Você precisa estar em uma casa para usar este comando.");
                return;
            }

            BaseHouse House = region.House;

            if (House == null)
            {
                from.SendMessage(0x00FE, "Voce precisa estar em uma casa.");
                return;
            }

            from.SendMessage("Aguarde 5 segundos para sair da casa");
            var duracao = TimeSpan.FromSeconds(5);
            var duracaoMais = duracao + TimeSpan.FromSeconds(2);
            from.Freeze(duracaoMais);
            Timer.DelayCall(duracao, m => Vaza(m, House), from);
        }

        public static void Vaza(Mobile m, BaseHouse house)
        {
            m.Frozen = false;
            BaseCreature.TeleportPets(m, house.BanLocation, m.Map);
            m.MoveToWorld(house.BanLocation, m.Map);
            m.SendMessage("Voce saiu da casa");
        }

    }
}
