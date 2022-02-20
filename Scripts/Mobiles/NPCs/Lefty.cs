using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{ 
    public class WondersOfTheNaturalWorldQuest : BaseQuest
    { 
        public WondersOfTheNaturalWorldQuest()
            : base()
        { 
            this.AddObjective(new ObtainObjective(typeof(Gold), "gold coins", 10000, 0xEED));
			
            this.AddReward(new BaseReward(typeof(PrismOfLightAdmissionTicket), 1074340)); // Prism of Light Admission Ticket
        }

        /* Wonders of the Natural World */
        public override object Title
        {
            get
            {
                return 1074444;
            }
        }
        /* Step right up!  Step right up!  Lords and Ladies, this is your opportunity to view the find of a lifetime!  
        What magical energy caused the fascinating play of light and darkness within these subterranean passageways?  
        What mysterious forces are at work deep within the Prism of Light?  Admission tickets are good for a full day 
        of adventure and excitement and well worth the price at 10,000 gold. Whadda ya say? */
        public override object Description
        {
            get
            {
                return 1074445;
            }
        }
        /* C'mon now Lords and Ladies -- you're passing up the opportunity of a lifetime.  Is 10,000 gold too much 
        to pay for your enlightenment? */
        public override object Refuse
        {
            get
            {
                return 1074446;
            }
        }
        /* Dig into those pockets Lords and Ladies!  Just ten-thousand-shiny-gold-coins and you'll be walking in the 
        bootsteps of the famous Lord Denthe himself! */
        public override object Uncomplete
        {
            get
            {
                return 1074447;
            }
        }
        /* Step right up!  Thank you, enjoy the amazing caverns. */
        public override object Complete
        {
            get
            {
                return 1074448;
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

    public class SBLefty : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBLefty()
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
                //this.Add(new GenericBuyInfo(typeof(IronIngot), 5, 16, 0x1BF2, 0, true));
                this.Add(new GenericBuyInfo(typeof(PrismOfLightAdmissionTicket), 10000, 0x14EF, 0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                
            }
        }
    }

    public class LeftyOld : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get
            {
                return m_SBInfos;
            }
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.BlacksmithsGuild;
            }
        }

        [Constructable]
        public LeftyOld()
            : base("Lefty")
        {
            Title = "o vendedor de tickets";
            SetSkill(SkillName.ArmsLore, 36.0, 68.0);
            SetSkill(SkillName.Blacksmith, 65.0, 88.0);
            SetSkill(SkillName.Fencing, 60.0, 83.0);
            SetSkill(SkillName.Macing, 61.0, 93.0);
            SetSkill(SkillName.Swords, 60.0, 83.0);
            SetSkill(SkillName.Tactics, 60.0, 83.0);
            SetSkill(SkillName.Parry, 61.0, 93.0);
        }

        public override void InitSBInfo()
        {
            /*m_SBInfos.Add( new SBSmithTools() );
            m_SBInfos.Add( new SBMetalShields() );
            m_SBInfos.Add( new SBWoodenShields() );
            m_SBInfos.Add( new SBPlateArmor() );
            m_SBInfos.Add( new SBHelmetArmor() );
            m_SBInfos.Add( new SBChainmailArmor() );
            m_SBInfos.Add( new SBRingmailArmor() );
            m_SBInfos.Add( new SBAxeWeapon() );
            m_SBInfos.Add( new SBPoleArmWeapon() );
            m_SBInfos.Add( new SBRangedWeapon() );
            m_SBInfos.Add( new SBKnifeWeapon() );
            m_SBInfos.Add( new SBMaceWeapon() );
            m_SBInfos.Add( new SBSpearForkWeapon() );
            m_SBInfos.Add( new SBSwordWeapon() );*/

      
                m_SBInfos.Add(new SBLefty());
            

        }



        public override VendorShoeType ShoeType
        {
            get
            {
                return VendorShoeType.None;
            }
        }

        public override void InitOutfit()
        {
            Item item = (Utility.RandomBool() ? null : new Server.Items.RingmailChest());

            if (item != null && !EquipItem(item))
            {
                item.Delete();
                item = null;
            }

            if (item == null)
                AddItem(new Server.Items.FullApron());

            AddItem(new Server.Items.Bascinet());
            AddItem(new Server.Items.SmithHammer());

            base.InitOutfit();
        }

        public LeftyOld(Serial serial)
            : base(serial)
        {
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

    public class Lefty : MondainQuester
    { 
        [Constructable]
        public Lefty()
            : base("Lefty", "the ticket seller")
        { 
        }

        public Lefty(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests
        { 
            get
            {
                return new Type[] 
                {
                    typeof(WondersOfTheNaturalWorldQuest)
                };
            }
        }
        public override void InitBody()
        {
            this.InitStats(100, 100, 25);
			
            this.Female = false;
            this.CantWalk = true;
            this.Race = Race.Human;
			
            this.Hue = 0x83F4;
            this.HairItemID = 0x203B;
            this.HairHue = 0x470;
        }

        public override void InitOutfit()
        {
            this.AddItem(new ThighBoots(0x901));
            this.AddItem(new LongPants(0x70D));
            this.AddItem(new Tunic(0x30));
            this.AddItem(new Cloak(0x30));
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
