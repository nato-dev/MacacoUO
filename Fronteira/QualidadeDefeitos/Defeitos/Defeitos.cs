using System.Collections.Generic;

namespace Server.Fronteira.QualidadeDefeitos
{
    public class Defeitos
    {
        private HashSet<Defeito> _Defeitos = new HashSet<Defeito>();

        public bool TemDefeito(Defeito defeito)
        {
            return _Defeitos.Contains(defeito);
        }

        public void Limpa()
        {
            _Defeitos.Clear();
        }

        public void Adquire(Defeito defeito)
        {
            _Defeitos.Add(defeito);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(_Defeitos.Count);
            foreach (var defeito in _Defeitos)
            {
                writer.Write((int) defeito);
            }
        }

        public void Deserialize(GenericReader reader)
        {
            var ct = reader.ReadInt();
            for (var x = 0; x < ct; x++)
            {
                _Defeitos.Add((Defeito) reader.ReadInt());
            }
        }
    }
}
