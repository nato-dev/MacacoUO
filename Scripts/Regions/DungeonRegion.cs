using Server.Items;
using Server.Mobiles;
using Server.SkillHandlers;
using Server.Spells;
using System;
using System.Xml;

namespace Server.Regions
{
    public class DungeonRegion : BaseRegion
    {

        public override void OnEnter(Mobile m)
        {
            //if (m.Player)
            //    m.SendMessage("Voce entrou em uma regiao perigosa, por ficar atento suas skills upam mais rapido aqui");

            if(m.QuestArrow != null && m.QuestArrow.Name == "mapa" && !(m.QuestArrow is TrackArrow))
            {
                m.SendMessage("Voce parou de seguir o mapa ao entrar na dungeon");
                m.QuestArrow.Stop();
                m.QuestArrow = null;
            }
            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
        }

        private Point3D m_EntranceLocation;
        private Map m_EntranceMap;

        public DungeonRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
            XmlElement entrEl = xml["entrance"];

            Map entrMap = map;
            ReadMap(entrEl, "map", ref entrMap, false);

            if (ReadPoint3D(entrEl, entrMap, ref this.m_EntranceLocation, false))
                this.m_EntranceMap = entrMap;

            ChecaMobs();
        }

        public DungeonRegion(string name, XmlElement xml, Map map, Region parent, int priority, params Rectangle3D[] area)
            : base(name, map, priority, area)
        {
            XmlElement entrEl = xml["entrance"];

            Map entrMap = map;
            ReadMap(entrEl, "map", ref entrMap, false);

            if (ReadPoint3D(entrEl, entrMap, ref this.m_EntranceLocation, false))
                this.m_EntranceMap = entrMap;

            ChecaMobs();
        }

        public void ChecaMobs()
        {
            var lista = this.GetMobiles();
            if(lista != null)
            {
                foreach (var mobile in lista)
                {
                    var tiles = mobile.GetLandTilesInRange(mobile.Map, 0);
                    if (tiles.Count == 0)
                        continue;
                    if (!(mobile is PlayerMobile))
                        continue;
                    var tile = tiles[0];
                    if (tile.Z != mobile.Z)
                        continue;
                    var bota = mobile.FindItemOnLayer(Layer.Shoes);
                    var protege = bota != null && bota is ElvenBoots;
                    if(!protege)
                    {
                        if (Mobile.IsDeepLandSwamp(tiles[0].ID))
                        {
                            if(!mobile.Poisoned)
                            {
                                mobile.PrivateOverheadMessage("* pisou em algo estranho *");
                                mobile.SendMessage("Voce se envenena ao pisar sem uma bota elfica no pantano");
                            }
                            mobile.Poison = Poison.Greater;
                        }
                        if (Mobile.IsLightLandSwamp(tiles[0].ID))
                        {
                            if (!mobile.Poisoned)
                            {
                                mobile.PrivateOverheadMessage("* pisou em algo estranho *");
                                mobile.SendMessage("Voce se envenena ao pisar sem uma bota elfica no pantano");
                            }
                            mobile.Poison = Poison.Regular;
                        }
                    }
                   
                }
            }
            Timer.DelayCall(TimeSpan.FromSeconds(15), ChecaMobs);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool YoungProtected
        {
            get
            {
                return true;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D EntranceLocation
        {
            get
            {
                return this.m_EntranceLocation;
            }
            set
            {
                this.m_EntranceLocation = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map EntranceMap
        {
            get
            {
                return this.m_EntranceMap;
            }
            set
            {
                this.m_EntranceMap = value;
            }
        }

        public override bool CheckTravel(Mobile traveller, Point3D p, TravelCheckType type)
        {
            if(Shard.DebugEnabled)
            {
                Shard.Debug("Travel type: " + type.ToString());
            }
            if (type == TravelCheckType.GateTo || type == TravelCheckType.RecallTo || type==TravelCheckType.Mark)
                return false;
            return true;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.DungeonLevel;
        }
    }
}
