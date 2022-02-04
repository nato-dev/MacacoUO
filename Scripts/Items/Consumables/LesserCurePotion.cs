using System;

namespace Server.Items
{
    public class LesserCurePotion : BaseCurePotion
    {
        public override string DefaultName
        {
            get { return "Poção de Cura Menor"; }
        }

        private static CureLevelInfo[] m_OldLevelInfo = null;

        private static readonly CureLevelInfo[] m_AosLevelInfo = new CureLevelInfo[]
        {
            new CureLevelInfo(Poison.Lesser, 1.00),
            new CureLevelInfo(Poison.Regular, 0.35),
            new CureLevelInfo(Poison.Greater, 0.15),
            new CureLevelInfo(Poison.Deadly, 0.10),
            new CureLevelInfo(Poison.Lethal, 0.05)
        };
        [Constructable]
        public LesserCurePotion()
            : base(PotionEffect.CuraMenor)
        {
        }

        public LesserCurePotion(Serial serial)
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
            new CureLevelInfo(Poison.Lesser, 0.75), // 75% chance to cure lesser poison
            new CureLevelInfo(Poison.Regular, 0.50), // 50% chance to cure regular poison
            new CureLevelInfo(Poison.Greater, 0.15)// 15% chance to cure greater poison
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
