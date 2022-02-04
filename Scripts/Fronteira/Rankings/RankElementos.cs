using Server.Commands;
using Server.Engines.Points;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden
{
    public class RankElementos
    {
        public static void Initialize()
        {
            CommandSystem.Register("calcularankelementos", AccessLevel.Administrator, new CommandEventHandler(CMD));
            Calcula(ElementoPvM.Agua);
            Calcula(ElementoPvM.Fogo);
            Calcula(ElementoPvM.Gelo);
            Calcula(ElementoPvM.Vento);
            Calcula(ElementoPvM.Terra);
            Calcula(ElementoPvM.Raio);
            Calcula(ElementoPvM.Luz);
            Calcula(ElementoPvM.Escuridao);
            Console.WriteLine("Rankings Elemental Calculado");
        }

        public static void Calcula(ElementoPvM el)
        {
            RankPorElemento[el] = PlayerMobile.Instances.OrderByDescending(e => e.Elementos.GetNivel(el)).Select((e, index) =>
            {
                e.RankingFama = index;
                return new PointsEntry(e, e.Elementos.GetNivel(el));
            }).ToList();
        }

        public static Dictionary<ElementoPvM, List<PointsEntry>> RankPorElemento = new Dictionary<ElementoPvM, List<PointsEntry>>();

        [Usage("calcula")]
        [Description("Camping Menu.")]
        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile as PlayerMobile;
            if (pl == null)
                return;

            //RankPorElemento = PlayerMobile.Instances.OrderByDescending(e => e.Fame).Select((e, index) => { e.RankingFama = index; return new PointsEntry(e, e.Fame); }).ToList();
            pl.SendMessage("Rankings Calculados");
            Console.WriteLine("Rankings Calculados");
        }
    }
}
