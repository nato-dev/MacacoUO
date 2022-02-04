using System;
using System.Xml;
using Server.Spells;

namespace Server.Regions
{
    public class NoHousingRegion : BaseRegion
    {
        /*  - False: this uses 'stupid OSI' house placement checking: part of the house may be placed here provided that the center is not in the region
        *  -  True: this uses 'smart RunUO' house placement checking: no part of the house may be in the region
        */
        private readonly bool m_SmartChecking;

        public NoHousingRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
            ReadBoolean(xml["smartNoHousing"], "active", ref this.m_SmartChecking, false);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool SmartChecking
        {
            get
            {
                return this.m_SmartChecking;
            }
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return this.m_SmartChecking;
        }
    }

    public class NoGuardCity : BaseRegion
    {
        /*  - False: this uses 'stupid OSI' house placement checking: part of the house may be placed here provided that the center is not in the region
        *  -  True: this uses 'smart RunUO' house placement checking: no part of the house may be in the region
        */
        private readonly bool m_SmartChecking;

        public NoGuardCity(XmlElement xml, Map map, Region parent)

            : base(xml, map, parent)
        {
            ReadBoolean(xml["smartNoHousing"], "active", ref this.m_SmartChecking, false);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool SmartChecking
        {
            get
            {
                return this.m_SmartChecking;
            }
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return this.m_SmartChecking;
        }

        public override void OnEnter(Mobile m)
        {
            m.SendMessage(38, "CUIDADO: Voce entrou em uma cidade sem protecao de guardas");
            base.OnEnter(m);
        }
    }

    public class NoRecallRegion : BaseRegion
    {

        public NoRecallRegion(XmlElement xml, Map map, Region parent)

            : base(xml, map, parent)
        {
           
        }

        public override bool CheckTravel(Mobile traveller, Point3D p, TravelCheckType type)
        {
            return type == TravelCheckType.RecallFrom;
        }
    }
}

