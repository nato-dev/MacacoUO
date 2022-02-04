using CustomsFramework;
using Server.Fronteira.Tutorial.WispGuia;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Scripts.New.Adam.NewGuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira
{
    public class SequenciaLogin
    {
        public static void Initialize()
        {
            Console.WriteLine("Carregando sequencia de login");
            EventSink.Login += OnLogin;
        }

        private static void OnLogin(LoginEventArgs e)
        {
            var pm = e.Mobile as PlayerMobile;

            if (pm == null)
                return;

            if (Shard.WARSHARD)
            {
                return;
            }

            if (Shard.WHITELIST)
            {
                if (pm.IsStaff() || pm.Name.StartsWith("Tester"))
                    return;

                pm.SendGump(new GumpWhitelist());
                pm.Frozen = true;
            }
            else if (pm.Frozen)
                pm.Frozen = false;

            if (CharacterCreation.Novos.Contains(e.Mobile))
            {
                CharacterCreation.Novos.Remove(e.Mobile);
                if(pm.RP && pm.Profession == 0)
                {
                    GumpClasse.Mostra(pm);
                }
                else if (pm.Profession == 0)
                {
                    pm.SendGump(new GumpLore(pm));
                }
            }
        }
    }
}
