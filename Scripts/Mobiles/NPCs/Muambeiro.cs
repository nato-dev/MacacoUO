using System;
using System.Collections.Generic;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
    public class Muambeiros : BaseVendor
    {
        public override TimeSpan RestockDelay { get { return TimeSpan.FromDays(1); } }

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
        public Muambeiros()
            : base("o muambeiro")
        {
            SetSkill(SkillName.ArmsLore, 36.0, 68.0);
            SetSkill(SkillName.Blacksmith, 65.0, 88.0);
            SetSkill(SkillName.Fencing, 60.0, 83.0);
            SetSkill(SkillName.Macing, 61.0, 93.0);
            SetSkill(SkillName.Swords, 60.0, 83.0);
            SetSkill(SkillName.Tactics, 60.0, 83.0);
            SetSkill(SkillName.Parry, 61.0, 93.0);
        }

        public override void VendorBuy(Mobile from)
        {
            base.VendorBuy(from);
            SayTo(from, "Eu restoco todo o dia ...");
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMuambeiro());
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

        public Muambeiros(Serial serial)
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
}
