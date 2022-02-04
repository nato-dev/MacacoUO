namespace Server.Fronteira.RP
{
    public class ObjetoFicha
    {
        public string Nome;
        public int Idade;
        public string Aparencia;
        public string Objetivos;
        public string NotaStaff;
        public PatenteRP Patente;

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);
            writer.Write(Nome);
            writer.Write(Idade);
            writer.Write(Aparencia);
            writer.Write(Objetivos);
            writer.Write(NotaStaff);
            writer.Write((int)Patente);
        }

        public void Deserialize(GenericReader reader)
        {
            var v = reader.ReadInt();
            Nome = reader.ReadString();
            Idade = reader.ReadInt();
            Aparencia = reader.ReadString();
            Objetivos = reader.ReadString();
            NotaStaff = reader.ReadString();
            Patente = (PatenteRP)reader.ReadInt();
        }
    }
}
