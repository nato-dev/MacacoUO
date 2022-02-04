using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
    public class SBMuambeiro : SBInfo 
    { 
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBMuambeiro() 
        { 
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return this.m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return this.m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo> 
        { 
            public InternalBuyInfo() 
            {
                this.Add(new GenericBuyInfo(typeof(SkillBook), 50000, 50, 0xEFA, 0));
                this.Add(new GenericBuyInfo(typeof(CombatSkillBook), 5000, 200, 0xEFA, 0));

                this.Add(new GenericBuyInfo(typeof(EnhancedBandage), 50, 2000, 0xE21, 0x8A5));

                this.Add(new GenericBuyInfo(typeof(GreaterHealPotion), 330, 1000, 0xF0C, 0));
                this.Add(new GenericBuyInfo(typeof(GreaterCurePotion), 330, 1000, 0xF07, 0));
                this.Add(new GenericBuyInfo(typeof(GreaterManaPotion), 330, 1000, 0x0EFB, 0));

               // this.Add(new GenericBuyInfo(typeof(IronIngot), 25, 10000, 0x1BF2, 0));
               // this.Add(new GenericBuyInfo(typeof(CopperIngot), 50, 10000, 0x1BF2, CraftResources.GetHue(CraftResource.Cobre)));
               // this.Add(new GenericBuyInfo(typeof(BronzeIngot), 80, 10000, 0x1BF2, CraftResources.GetHue(CraftResource.Bronze)));

                this.Add(new GenericBuyInfo(typeof(EtherealHorse), 600000, 2, 0x20DD, 0));

               // this.Add(new GenericBuyInfo(typeof(Board), 25, 10000, 0x1BD7, 0));
               // this.Add(new GenericBuyInfo(typeof(OakBoard), 50, 10000, 0x1BD7, CraftResources.GetHue(CraftResource.Carvalho)));
               // this.Add(new GenericBuyInfo(typeof(YewBoard), 80, 10000, 0x1BD7, CraftResources.GetHue(CraftResource.Pinho)));

                foreach(var i in this)
                {
                    i.StockMult = 1;
                }
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
