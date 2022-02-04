using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Bancos
{
    public class BancoConta
    {
        public static void AbreGump(PlayerMobile from)
        {
            var mobiles = from.Account.GetMobiles().ToArray();
            var chars = mobiles.Where(m => m != null).Select(m => m.Name).ToArray();

            from.SendGump(new GumpOpcoes("Selecione o banco", (opt) =>
            {
                var selecionado = mobiles[opt];

            }, 3708, 0, chars));
        }

    }
}
