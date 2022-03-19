using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBJewel : SBInfo
    {


        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBJewel()
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
                Add(new GenericBuyInfo(typeof(FerramentasJoalheiro), 50, 20, 0x0FB7, 0));

                Add(new GenericBuyInfo("1060740", typeof(BroadcastCrystal), 68, 20, 0x1ED0, 0, new object[] { 500 })); // 500 charges
                Add(new GenericBuyInfo("1060740", typeof(BroadcastCrystal), 131, 20, 0x1ED0, 0, new object[] { 1000 })); // 1000 charges
                Add(new GenericBuyInfo("1060740", typeof(BroadcastCrystal), 256, 20, 0x1ED0, 0, new object[] { 2000 })); // 2000 charges

                Add(new GenericBuyInfo("1060740", typeof(ReceiverCrystal), 6, 20, 0x1ED0, 0));
               
                Add(new GenericBuyInfo(typeof(StarSapphire), 5000, 5, 0x0F0F, 0, true));
                Add(new GenericBuyInfo(typeof(Emerald), 5000, 5, 0xF10, 0, true));
                Add(new GenericBuyInfo(typeof(Sapphire), 5000, 5, 0xF19, 0, true));
                Add(new GenericBuyInfo(typeof(Ruby), 5000, 5, 0xF13, 0, true));
                Add(new GenericBuyInfo(typeof(Citrine), 5000, 5, 0xF15, 0, true));
                Add(new GenericBuyInfo(typeof(Amethyst), 5000, 5, 0xF16, 0, true));
                Add(new GenericBuyInfo(typeof(Tourmaline), 5000, 5, 0x0F18, 0, true));
                Add(new GenericBuyInfo(typeof(Amber), 5000, 5, 0xF25, 0, true));
                Add(new GenericBuyInfo(typeof(Diamond), 5000, 5, 0xF26, 0, true));

                Add(new GenericBuyInfo(typeof(TransformationDust), 5000, 100, 0x5745, 0, true));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            
            }
        }
    }
}
