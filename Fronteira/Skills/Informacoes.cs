using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Skills
{
    public class Informacoes
    {
        private static Dictionary<SkillName, string> _infos = new Dictionary<SkillName, string>();
        private static Dictionary<SkillName, int> _items = new Dictionary<SkillName, int>();

        private static void AddInfo(SkillName skill, int itemID, params string[] linhas)
        {    
            _infos[skill] = string.Join("<br>", linhas);
            _items[skill] = itemID;
        }

        static Informacoes()
        {
            AddInfo(
                SkillName.Alchemy,
                0xEED,
                "um paragrafo", "outro paragrafo", "infinitos paragrafos"
            );
        }
    }
}
