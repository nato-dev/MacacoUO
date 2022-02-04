using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Misc.Custom
{
    public class BankLevel
    {
        public int Stones;
        public int Items;
        public int Preco;

        public BankLevel(int s, int i, int p)
        {
            Stones = s;
            Items = i;
            Preco = p;
        }
    }

    public class BankLevels
    {
        private static List<BankLevel> _Nivels = new List<BankLevel>();

        public static List<BankLevel> Niveis { get
            {
                if (_Nivels.Count == 0)
                    Initialize();
                return _Nivels;
            }
        }

        public static void Initialize()
        {
            Console.WriteLine("[Banco Doido] Inicializando niveis do banco");

            // NOOBS
            _Nivels.Add(new BankLevel(500, 20, 0));
            _Nivels.Add(new BankLevel(750, 40, 100));
            _Nivels.Add(new BankLevel(1000, 60, 200));
            _Nivels.Add(new BankLevel(1500, 80, 500));
            _Nivels.Add(new BankLevel(2000, 100, 1000));
            _Nivels.Add(new BankLevel(5000, 120, 5000));

            // CAROS FODAS
            _Nivels.Add(new BankLevel(15000, 200, 10000));
            _Nivels.Add(new BankLevel(25000, 250, 20000));
            _Nivels.Add(new BankLevel(45000, 350, 50000));
            _Nivels.Add(new BankLevel(150000, 1000, 100000));
        }

        public static void OpenBank(Mobile m)
        {

            var p = m as PlayerMobile;
            if (p == null)
                return;

            if(p.Wisp != null)
            {
                p.Wisp.AbreBanco();
            }
            var nivel = ((PlayerMobile)m).NivelBanco;

            var infoNivel = BankLevels.Niveis[nivel];

            m.BankBox.MaxItems = infoNivel.Items;
            m.BankBox.PesoMax = infoNivel.Stones;

            var items = m.BankBox.TotalItems + "/" + infoNivel.Items;
            var peso = m.BankBox.TotalWeight + "/" + infoNivel.Stones;

            m.PrivateOverheadMessage("Seu banco tem " + items + " items e pesa " + peso + " stones");

            if (m.BankBox.TotalItems > infoNivel.Items / 1.2 || m.BankBox.TotalWeight > infoNivel.Stones / 1.2)
            {
                m.PrivateOverheadMessage("Seu banco esta quase cheio, para aprimorar diga que quer 'aprimorar' para o banqueiro");
            }

            m.SendGump(new BankInfoGump(m));
            m.BankBox.Open();
        }
    }
}
