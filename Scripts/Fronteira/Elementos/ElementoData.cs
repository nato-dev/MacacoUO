using Server.Items;
using System.Collections.Generic;
using System.Linq;

namespace Server.Fronteira.Elementos
{
    [PropertyObject]
    public class ElementoData
    {
        private Dictionary<ElementoPvM, ushort> _niveis = new Dictionary<ElementoPvM, ushort>();

        private Dictionary<ElementoPvM, int> _exps = new Dictionary<ElementoPvM, int>();

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Fogo { get { return GetNivel(ElementoPvM.Fogo); } set { SetNivel(ElementoPvM.Fogo, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Gelo { get { return GetNivel(ElementoPvM.Gelo); } set { SetNivel(ElementoPvM.Gelo, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Vento { get { return GetNivel(ElementoPvM.Vento); } set { SetNivel(ElementoPvM.Vento, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Agua { get { return GetNivel(ElementoPvM.Agua); } set { SetNivel(ElementoPvM.Agua, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Terra { get { return GetNivel(ElementoPvM.Terra); } set { SetNivel(ElementoPvM.Terra, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Raio { get { return GetNivel(ElementoPvM.Raio); } set { SetNivel(ElementoPvM.Raio, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Luz { get { return GetNivel(ElementoPvM.Luz); } set { SetNivel(ElementoPvM.Luz, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ushort Escuridao { get { return GetNivel(ElementoPvM.Escuridao); } set { SetNivel(ElementoPvM.Escuridao, value); } }

        public ElementoData()
        {
            _niveis[ElementoPvM.Fogo] = 0;
            _niveis[ElementoPvM.Raio] = 0;
            _niveis[ElementoPvM.Agua] = 0;
            _niveis[ElementoPvM.Gelo] = 0;
            _niveis[ElementoPvM.Vento] = 0;
            _niveis[ElementoPvM.Terra] = 0;
            _niveis[ElementoPvM.Luz] = 0;
            _niveis[ElementoPvM.Escuridao] = 0;

            _exps[ElementoPvM.Fogo] = 0;
            _exps[ElementoPvM.Raio] = 0;
            _exps[ElementoPvM.Agua] = 0;
            _exps[ElementoPvM.Gelo] = 0;
            _exps[ElementoPvM.Vento] = 0;
            _exps[ElementoPvM.Terra] = 0;
            _exps[ElementoPvM.Luz] = 0;
            _exps[ElementoPvM.Escuridao] = 0;
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(_niveis[ElementoPvM.Fogo]);
            writer.Write(_niveis[ElementoPvM.Agua]);
            writer.Write(_niveis[ElementoPvM.Raio]);
            writer.Write(_niveis[ElementoPvM.Gelo]);
            writer.Write(_niveis[ElementoPvM.Vento]);
            writer.Write(_niveis[ElementoPvM.Terra]);
            writer.Write(_niveis[ElementoPvM.Luz]);
            writer.Write(_niveis[ElementoPvM.Escuridao]);

            writer.Write(_exps[ElementoPvM.Fogo]);
            writer.Write(_exps[ElementoPvM.Agua]);
            writer.Write(_exps[ElementoPvM.Raio]);
            writer.Write(_exps[ElementoPvM.Gelo]);
            writer.Write(_exps[ElementoPvM.Vento]);
            writer.Write(_exps[ElementoPvM.Terra]);
            writer.Write(_exps[ElementoPvM.Luz]);
            writer.Write(_exps[ElementoPvM.Escuridao]);
        }

        public void Deserialize(GenericReader reader)
        {
            _niveis[ElementoPvM.Fogo] = reader.ReadUShort();
            _niveis[ElementoPvM.Agua] = reader.ReadUShort();
            _niveis[ElementoPvM.Raio] = reader.ReadUShort();
            _niveis[ElementoPvM.Gelo] = reader.ReadUShort();
            _niveis[ElementoPvM.Vento] = reader.ReadUShort();
            _niveis[ElementoPvM.Terra] = reader.ReadUShort();
            _niveis[ElementoPvM.Luz] = reader.ReadUShort();
            _niveis[ElementoPvM.Escuridao] = reader.ReadUShort();

            _exps[ElementoPvM.Fogo] = reader.ReadInt();
            _exps[ElementoPvM.Agua] = reader.ReadInt();
            _exps[ElementoPvM.Raio] = reader.ReadInt();
            _exps[ElementoPvM.Gelo] = reader.ReadInt();
            _exps[ElementoPvM.Vento] = reader.ReadInt();
            _exps[ElementoPvM.Terra] = reader.ReadInt();
            _exps[ElementoPvM.Luz] = reader.ReadInt();
            _exps[ElementoPvM.Escuridao] = reader.ReadInt();
        }

        public void SetNivel(ElementoPvM elemento, ushort nivel)
        {
            _niveis[elemento] = nivel;
        }

        public ushort GetNivel(ElementoPvM elemento)
        {
            if (elemento == ElementoPvM.None)
                return 0;
            return _niveis[elemento];
        }

        public List<ushort> GetNiveis()
        {
            return _niveis.Values.ToList();
        }

        public void SetExp(ElementoPvM elemento, int nivel)
        {
            _exps[elemento] = nivel;
        }

        public int GetExp(ElementoPvM elemento)
        {
            if (elemento == ElementoPvM.None)
                return 0;
            return _exps[elemento];
        }

        public double GetBonus(ElementoPvM elemento)
        {
            return BonusPorNivel(_niveis[elemento]);
        }

        // Formula magica q permite noiz escalar ao infinito
        // a ideia eh da formula de armor do dota2 escalada pra cima
        public double BonusPorNivel(double nivel)
        {
            if (nivel == 0)
                return 0;
            return (0.05 * nivel) / (0.9 + 0.015 * nivel);
        }

    }
}
