using System.Collections.Generic;

namespace Server.Fronteira.QualidadeDefeitos
{
    public class Qualidades
    {
        private HashSet<Qualidade> _Qualidades = new HashSet<Qualidade>();

        public bool TemQualidade(Qualidade qualidade)
        {
            return _Qualidades.Contains(qualidade);
        }

        public void Limpa()
        {
            _Qualidades.Clear();
        }

        public void Adquire(Qualidade qualidade)
        {
            _Qualidades.Add(qualidade);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(_Qualidades.Count);
            foreach (var qualidade in _Qualidades)
            {
                writer.Write((int) qualidade);
            }
        }

        public void Deserialize(GenericReader reader)
        {
            var ct = reader.ReadInt();
            for (var x = 0; x < ct; x++)
            {
                _Qualidades.Add((Qualidade) reader.ReadInt());
            }
        }
    }
}
