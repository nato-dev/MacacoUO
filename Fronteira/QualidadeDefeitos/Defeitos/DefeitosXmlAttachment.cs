using System;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Fronteira.QualidadeDefeitos
{
    public class DefeitosXmlAttachment : XmlAttachment
    {
        private Mobile _From = null;
        private Defeitos _Defeitos = null;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile From
        {
            get { return _From; }
            set { _From = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Defeitos Defeitos
        {
            get { return _Defeitos; }
            set { _Defeitos = value; }
        }

        public DefeitosXmlAttachment(ASerial serial, Mobile from, Defeitos defeitos)
            : base(serial)
        {
            From = from;
            Defeitos = defeitos;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // version 0
            writer.Write((int) 0);
            writer.Write((Mobile) From);

            Defeitos.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // version 0
            int version = reader.ReadInt();
            From = reader.ReadMobile();

            Defeitos.Deserialize(reader);
        }
    }
}
