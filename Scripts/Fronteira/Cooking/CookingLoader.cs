using Server.Items.Crops;
using System;
using System.Collections.Generic;

namespace Server.Fronteira.Cooking
{
    public class CookingLoader
    {
        private static List<Type> Sementes = new List<Type>();
        private static List<Type> Plantas = new List<Type>();

        public static Item GetPlantaRandom()
        {
            var t = Plantas[Utility.Random(Plantas.Count)];
            return (Item)Activator.CreateInstance(t);
        }

        public static Item GetSementeRandom()
        {
            var t = Sementes[Utility.Random(Sementes.Count)];
            return (Item)Activator.CreateInstance(t);
        }

        public static void Initialize()
        {
            foreach (var s in ScriptCompiler.GetAllItemsOfBase(typeof(HerdingBaseCrop)))
            {
                if (s.Name.Contains("Crop"))
                {
                    Shard.Debug("Registrada planta " + s.Name);
                    Plantas.Add(s);
                }
                else if (s.Name.Contains("Seed"))
                {
                    Shard.Debug("Registrada semente " + s.Name);
                    Sementes.Add(s);
                }
            }
        }
    }
}
