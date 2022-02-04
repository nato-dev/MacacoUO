using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Fronteira.Quests
{
    public class Tamavel : IEquatable<Tamavel>
    {
        private static HashSet<Tamavel> l50 = new HashSet<Tamavel>();
        private static HashSet<Tamavel> l70 = new HashSet<Tamavel>();
        private static HashSet<Tamavel> l80 = new HashSet<Tamavel>();
        private static HashSet<Tamavel> l90 = new HashSet<Tamavel>();
        private static HashSet<Tamavel> l100 = new HashSet<Tamavel>();
        private static HashSet<Tamavel> l120 = new HashSet<Tamavel>();

        private static Tamavel Randomiza(HashSet<Tamavel> lista)
        {
            if (lista.Count == 0)
            {
                if (lista == l70) return Randomiza(l50);
                if (lista == l80) return Randomiza(l70);
                if (lista == l90) return Randomiza(l80);
                if (lista == l100) return Randomiza(l90);
                if (lista == l120) return Randomiza(l100);
            }
            Shard.Debug("Randomizando Tamavel");
            return lista.ElementAt(Utility.Random(lista.Count));
        }

        public static Tuple<Tamavel, int> Sorteia(double skill)
        {
            if (skill > 100)
                if (Utility.RandomDouble() < 0.35)
                    return new Tuple<Tamavel, int>(Randomiza(l80), 6);
                else if (Utility.RandomBool())
                    return new Tuple<Tamavel, int>(Randomiza(l120), 1);
                else
                    return new Tuple<Tamavel, int>(Randomiza(l100), 3);
            else if (skill > 90)
                if (Utility.RandomDouble() < 0.35)
                    return new Tuple<Tamavel, int>(Randomiza(l80), 8);
                else if (Utility.RandomBool())
                    return new Tuple<Tamavel, int>(Randomiza(l100), 3);
                else
                    return new Tuple<Tamavel, int>(Randomiza(l90), 3);
            else if (skill > 80)
                if (Utility.RandomDouble() < 0.35)
                    return new Tuple<Tamavel, int>(Randomiza(l70), 8);
                else if (Utility.RandomBool())
                    return new Tuple<Tamavel, int>(Randomiza(l90), 3);
                else
                    return new Tuple<Tamavel, int>(Randomiza(l80), 4);
            else if (skill > 70)
                if (Utility.RandomDouble() < 0.35)
                    return new Tuple<Tamavel, int>(Randomiza(l50), 15);
                else if (Utility.RandomBool())
                    return new Tuple<Tamavel, int>(Randomiza(l80), 4);
                else
                    return new Tuple<Tamavel, int>(Randomiza(l70), 5);
            else if (Utility.RandomBool())
                return new Tuple<Tamavel, int>(Randomiza(l70), 5);
            else
                return new Tuple<Tamavel, int>(Randomiza(l50), 5);

        }

        // registra tds possiveis bixos q tem no shard
        public static void RegistraBixo(BaseCreature bc)
        {
            if (bc == null || (bc.Map != Map.Felucca && bc.Map != Map.Trammel))
                return;

            if (bc.ControlMaster != null)
                return;

            if (bc.Owners != null && bc.Owners.Count > 0)
                return;

            if (bc != null && bc.Tamable)
            {
                if (bc.MinTameSkill < 50)
                    l50.Add(new Tamavel(bc));
                else if (bc.MinTameSkill < 70)
                    l70.Add(new Tamavel(bc));
                else if (bc.MinTameSkill < 80)
                    l80.Add(new Tamavel(bc));
                else if (bc.MinTameSkill < 90)
                    l90.Add(new Tamavel(bc));
                else if (bc.MinTameSkill < 100)
                    l100.Add(new Tamavel(bc));
                else if (bc.MinTameSkill <= 120)
                    l120.Add(new Tamavel(bc));

                Shard.Debug("Registrei bixo taming", bc);
            }
        }

        public string Name;
        public int Hue;
        public double Skill;
        public int Body;

        public Tamavel(BaseCreature bc)
        {
            Hue = bc.Hue;
            Name = bc.Name;
            Skill = bc.MinTameSkill;
            Body = bc.Body;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + Hue.GetHashCode();
            return hash;
        }

        public bool Equals(Tamavel other)
        {
            return other.Name == this.Name && other.Hue == this.Hue;
        }
    }
}
