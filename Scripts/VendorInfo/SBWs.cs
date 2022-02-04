
using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SbWs : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SbWs()
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
                Add(new GenericBuyInfo(typeof(Spyglass), 50, 999, 0x14F5, 0));
                Add(new GenericBuyInfo(typeof(RefreshPotion), 50, 999, 0xF0B, 0));
                Add(new GenericBuyInfo(typeof(GreaterAgilityPotion), 50, 999, 0xF08, 0));
                Add(new GenericBuyInfo(typeof(GreaterHealPotion), 50, 999, 0xF0C, 0));
                Add(new GenericBuyInfo(typeof(GreaterStrengthPotion), 50, 999, 0xF09, 0));
                Add(new GenericBuyInfo(typeof(GreaterPoisonPotion), 50, 999, 0xF0A, 0));
                Add(new GenericBuyInfo(typeof(GreaterCurePotion), 50, 999, 0xF07, 0));
                Add(new GenericBuyInfo(typeof(GreaterExplosionPotion), 50, 999, 0xF0D, 0));
               
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(WizardsHat), 15);
                Add(typeof(BlackPearl), 1);
                Add(typeof(Bloodmoss), 1);
                Add(typeof(MandrakeRoot), 1);
                Add(typeof(Garlic), 1);
                Add(typeof(Ginseng), 1);
                Add(typeof(Nightshade), 1);
                Add(typeof(SpidersSilk), 1);
                Add(typeof(SulfurousAsh), 1);

                if (Core.AOS)
                {
                    Add(typeof(BatWing), 1);
                    Add(typeof(DaemonBlood), 3);
                    Add(typeof(PigIron), 2);
                    Add(typeof(NoxCrystal), 3);
                    Add(typeof(GraveDust), 1);
                }

                Add(typeof(RecallRune), 13);
                Add(typeof(Spellbook), 25);

                if (Core.SE)
                {
                    Add(typeof(ExorcismScroll), 3);
                    Add(typeof(AnimateDeadScroll), 8);
                    Add(typeof(BloodOathScroll), 8);
                    Add(typeof(CorpseSkinScroll), 8);
                    Add(typeof(CurseWeaponScroll), 8);
                    Add(typeof(EvilOmenScroll), 8);
                    Add(typeof(PainSpikeScroll), 8);
                    Add(typeof(SummonFamiliarScroll), 8);
                    Add(typeof(HorrificBeastScroll), 8);
                    Add(typeof(MindRotScroll), 10);
                    Add(typeof(PoisonStrikeScroll), 10);
                    Add(typeof(WraithFormScroll), 15);
                    Add(typeof(LichFormScroll), 16);
                    Add(typeof(StrangleScroll), 16);
                    Add(typeof(WitherScroll), 16);
                    Add(typeof(VampiricEmbraceScroll), 20);
                    Add(typeof(VengefulSpiritScroll), 20);
                }
            }
        }
    }
}
