using Server.Items;
using Server.Items.Crops;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Items.Crops
{
    public abstract class BaseSeedling : HerdingBaseCrop
    {
        private Mobile m_sower;
        public Timer thisTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Sower { get { return m_sower; } set { m_sower = value; } }

        [Constructable]
        public BaseSeedling(Mobile sower, int itemId) : base(itemId)
        {
            Movable = false;
            Name = "Plantinha de Amora";
            m_sower = sower;
            StartTimer(this);
        }

        public abstract Type GetPlantType();

        public void StartTimer(BaseSeedling plant)
        {
            plant.thisTimer = new CropHelper.GrowTimer(plant, GetPlantType(), plant.Sower);
            plant.thisTimer.Start();
        }
        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Esta plantacao ainda e muito jovem.");
        }

        public BaseSeedling(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); writer.Write(m_sower); }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_sower = reader.ReadMobile();
            //init(this);
        }
    }
}
