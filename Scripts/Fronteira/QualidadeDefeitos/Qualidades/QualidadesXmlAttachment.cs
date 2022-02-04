using Server.Engines.XmlSpawner2;

namespace Server.Fronteira.QualidadeDefeitos
{
    public class QualidadesXmlAttachment : XmlAttachment
    {
        private Mobile _From = null;
        private Qualidades _Qualidades = null;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile From
        {
            get { return _From; }
            set { _From = value;} 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Qualidades Qualidades
        {
            get { return _Qualidades; }
            set { _Qualidades = value; }
        }

        public QualidadesXmlAttachment(ASerial serial, Mobile from, Qualidades qualidade)
            : base(serial)
        {
            From = from;
            Qualidades = qualidade;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // version 0
            writer.Write((int) 0);
            writer.Write((Mobile) From);

            Qualidades.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // version 0
            int version = reader.ReadInt();
            From = reader.ReadMobile();

            Qualidades.Deserialize(reader);
        }
    }
}
