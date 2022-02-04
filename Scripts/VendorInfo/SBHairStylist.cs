using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBHairStylist : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBHairStylist()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo("tinta especial de barba", typeof(SpecialBeardDye), 500000, 20, 0xE26, 0));
                Add(new GenericBuyInfo("tinta especial de cabelo", typeof(SpecialHairDye), 500000, 20, 0xE26, 0));
                Add(new GenericBuyInfo("tinta de cabelo", typeof(HairDye), 60, 20, 0xEFF, 0));
                Add(new GenericBuyInfo("penteados", typeof(AdvancedHairRestylingDeed), 500, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("penteados elficos", typeof(AdvancedHairRestylingElfDeed), 50000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("barbas", typeof(AdvancedBeardRestylingDeed), 500, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("barbas elficas", typeof(AdvancedBeardRestylingElfDeed), 50000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(HairDye), 30);
                Add(typeof(SpecialBeardDye), 250000);
                Add(typeof(SpecialHairDye), 250000);
            }
        }
    }
}
