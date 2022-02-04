using System;

namespace Server.Items
{
    public class GreaterCurePotion : BaseCurePotion
    {
        public override string DefaultName
        {
            get { return "Poção de Cura Maior"; }
        }

        private static CureLevelInfo[] m_OldLevelInfo = null;

        [Constructable]
        public GreaterCurePotion()
            : base(PotionEffect.CuraMaior)
        {
        }

        public GreaterCurePotion(Serial serial)
            : base(serial)
        {
        }

        public override CureLevelInfo[] LevelInfo
        {
            get
            {
                if (m_OldLevelInfo == null)
                {
                    m_OldLevelInfo = new CureLevelInfo[]
       {
            new CureLevelInfo(Poison.Lesser, 1.00), // 100% chance to cure lesser poison
            new CureLevelInfo(Poison.Regular, 1.00), // 100% chance to cure regular poison
            new CureLevelInfo(Poison.Greater, 1.00), // 100% chance to cure greater poison
            new CureLevelInfo(Poison.Deadly, 0.9), //  75% chance to cure deadly poison
            new CureLevelInfo(Poison.Lethal, 0.85)//  25% chance to cure lethal poison
       };
                }
                return m_OldLevelInfo;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
