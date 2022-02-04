using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Fronteira.Talentos
{
    public class Talentos
    {
        public static void Initialize()
        {
            EventSink.SkillCapChange += CapChange;
        }

        public static void CapChange(SkillCapChangeEventArgs e)
        {
            if(e.Mobile is PlayerMobile)
            {
                if (e.NewCap > 0 && e.Skill.Base < e.NewCap / 4)
                {
                    e.Skill.Base = e.NewCap / 2;
                    e.Skill.Base = Math.Floor(e.Skill.Base / 10) * 10;
                }
            }
        }

        private HashSet<Talento> _talentos = new HashSet<Talento>();

        public Talento [] ToArray()
        {
            return _talentos.ToArray();
        }

        public List<Talento> Habilidades()
        {
            var habs = new List<Talento>();
            foreach(var talento in _talentos)
            {
                if (IsHabilidade(talento))
                    habs.Add(talento);
            }
            return habs;
        }

        public static bool IsHabilidade(Talento t)
        {
            return t.ToString().StartsWith("Hab_");
        }

        public int Quantidade()
        {
            return _talentos.Count;
        }

        public bool Tem(Talento t)
        {
            return _talentos.Contains(t);
        }

        public void Wipa()
        {
            _talentos.Clear();
        }

        public void Desaprende(Talento t)
        {
            _talentos.Remove(t);
        }

        public void Aprende(Talento t)
        {
            _talentos.Add(t);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(_talentos.Count);
            foreach(var talento in _talentos)
            {
                writer.Write((int)talento);
            }
        }

        public void Deserialize(GenericReader reader)
        {
            var ct = reader.ReadInt();
            for (var x = 0; x < ct; x++)
            {
                _talentos.Add((Talento)reader.ReadInt());
            }
        }

        public void DeserialzieOld(GenericReader reader)
        {
            var ct = reader.ReadInt();
            for(var x = 0; x < ct; x++)
            {
               reader.ReadInt();
               reader.ReadInt();
            }
        }
    }
}
