using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBMoedas : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBMoedas()
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
                // Add(new GenericBuyInfo(typeof(BreadLoaf), 6, 20, 0x103B, 0)); 
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(BreadLoaf), 3);
                Add(typeof(FrenchBread), 1);
                Add(typeof(Cake), 5);
                Add(typeof(Cookies), 3);
                Add(typeof(Muffins), 2);
                Add(typeof(CheesePizza), 4);
                Add(typeof(ApplePie), 5);
                Add(typeof(PeachCobbler), 5);
                Add(typeof(Quiche), 6);
                Add(typeof(Dough), 4);
                Add(typeof(JarHoney), 1);
                Add(typeof(Pitcher), 5);
                Add(typeof(SackFlour), 1);
                Add(typeof(Eggs), 1);
            }
        }
    }
}
