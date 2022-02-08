using Server.Accounting;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.UltimaStore
{
    public class Doou
    {
        private static Dictionary<string, int> Cods = new Dictionary<string, int>();

        public static void Initialize()
        {
            CommandSystem.Register("doou", AccessLevel.Administrator, OnAction);
        }

        [Description("Da moedas magicas a alguem que doou")]
        private static void OnAction(CommandEventArgs e)
        {
            try
            {
                if (e.Arguments.Count() != 2)
                {
                    e.Mobile.SendMessage("Use .doou <login> <reais>");
                    return;
                }
                var conta = e.GetString(0);
                var reais = e.GetInt32(1);
                var valor = reais * 100;
                var acc = Accounts.GetAccount(conta) as Account;
                if (acc == null)
                {
                    e.Mobile.SendMessage("Nao achei a conta " + conta);
                    return;
                }
                acc.DepositarMoedasMagicas(valor);
                var from = acc.GetOnlineMobile();
                Consome(from);
                Log(conta, valor.ToString());
                e.Mobile.SendMessage("Despositada moedas magicas na conta " + conta + " com sucesso !");
            } catch(Exception ex)
            {
                e.Mobile.SendMessage("Algum erro aconteceu. Contate os devs e mande isso pra eles:");
                e.Mobile.SendMessage(ex.StackTrace);
            }
        }

        private static string FilePath = Path.Combine("Saves/Loja", "Doadas.bin");

        public static void Consome(Mobile from)
        {
            if (from == null || from.Deleted || from.NetState == null)
                return;

            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
            Effects.PlaySound(from.Location, from.Map, 0x243);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

            Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

            from.SendMessage("Voce doou para o servidor e recebeu suas moedas magicas !");
            /*
            foreach(var pl in PlayerMobile.Instances)
            {
                if(pl != null && pl.NetState != null && pl != from)
                {
                    pl.SendMessage(78, from.Name + " contribuiu com o shard e recebeu moedas magicas !");
                }
            }
            */
        }


        public static void Log(string conta, string valor)
        {
            using (StreamWriter w = File.AppendText("doacoes.csv"))
            {
                w.WriteLine(conta + ";" + valor + ";" + DateTime.Now.ToShortDateString());
            }
        }

    }
}
