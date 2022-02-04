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

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            if (e.Arguments.Count() != 1)
            {
                e.Mobile.SendMessage("NOP");
                return;
            }
            var conta = e.GetString(0);
            var acc = Accounts.GetAccount(conta) as Account;
            if(acc==null)
            {
                e.Mobile.SendMessage("Nao achei a conta");
                return;
            }
            acc.DepositarMoedasMagicas(3000);
            var from = acc.GetOnlineMobile();
            Consome(from);
        }

        private static string FilePath = Path.Combine("Saves/Loja", "Codigos.bin");

        public static void Consome(Mobile from)
        {
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

        private static void Salva(GenericWriter writer)
        {
            writer.Write((int)1);
            writer.Write(Cods.Count);
            foreach(var key in Cods.Keys)
            {
                writer.Write(key);
                writer.Write(Cods[key]);
            }
        }

        private static void Carrega(GenericReader reader)
        {
            var version = reader.ReadInt();
            var count = reader.ReadInt();
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Console.WriteLine("Salvando Loja Donates");
            Persistence.Serialize(FilePath, Salva);
        }

        public static void OnLoad()
        {
            Console.WriteLine("Carregando Loja Donates");
            Persistence.Deserialize(FilePath, Carrega);
        }
    }
}
