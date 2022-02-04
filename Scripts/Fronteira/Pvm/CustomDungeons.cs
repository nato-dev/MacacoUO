using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Pvm
{
    public enum CustomDungeons
    {
        FARAOH,
        PIRATAS,
    }

    public class DungeonsCompletasSerializer
    {
        public static List<CustomDungeons> Deserialize(GenericReader reader)
        {
            List<CustomDungeons> lista = new List<CustomDungeons>();
            var n = reader.ReadByte();
            for(var x = 0; x < n; x++)
            {
                var dg = (CustomDungeons)reader.ReadByte();
                lista.Add(dg);
            }
            return lista;
        }

        public static void Serialize(PlayerMobile p, GenericWriter writer)
        {
            if(p.DungeonsCompletas == null)
            {
                p.DungeonsCompletas = new List<CustomDungeons>();
            }
            writer.Write((byte)p.DungeonsCompletas.Count);
            foreach (var dg in p.DungeonsCompletas)
                writer.Write((byte)dg);
        }
    }
}
