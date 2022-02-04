#region References
using System;
using System.Linq;

using Server.Items;
using Server.Regions;
using Server.Targeting;
#endregion

namespace Server.Commands
{
    public class Guards
    {
        public static void Initialize()
        {
            CommandSystem.Register("g", AccessLevel.Player, Guard_Command);
        }

        private static Random rnd = new Random();
        public static string[] msg = new string[] {
            "Ai meu deus ai meu deus, GUARDAS me ajudem !",
            "Jesus me ajuda, GUAAAARDAAAAS",
            "GUARDAS socorro !!!! SOCORRO !!",
            "Guaaaaardaaas livrem-se dessa criatura do mal !"
        };

        [Usage("g")]
        [Description("Chama os homi.")]
        private static void Guard_Command(CommandEventArgs e)
        {
            var m = msg[rnd.Next(msg.Length)];
            e.Mobile.Yell(m);
            if(e.Mobile.Region != null && e.Mobile.Region is GuardedRegion)
            {
                var region = (GuardedRegion)e.Mobile.Region;
                region.CallGuards(e.Mobile.Location);
            } else
            {
                e.Mobile.SendMessage("Nenhum guarda por perto, ninguem escuta seu pedido de socorro");
            }
        }
    }
}
