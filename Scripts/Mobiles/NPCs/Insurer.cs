using System;
using System.Collections.Generic;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
    public class Insurer : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get
            {
                return m_SBInfos;
            }
        }


        [Constructable]
        public Insurer()
            : base("o vendedor de seguros")
        {
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

      
        }

        public override VendorShoeType ShoeType
        {
            get
            {
                return VendorShoeType.None;
            }
        }

        public override void VendorBuy(Mobile from)
        {
            var pl = from as PlayerMobile;
            if (pl != null)
            {
                pl.OpenItemInsuranceMenu();
                if (!pl.IsCooldown("dicaseguro"))
                {
                    pl.SetCooldown("dicaseguro", TimeSpan.FromSeconds(60));
                    pl.SendMessage(78, "Voce pode dar insure em roupas, joias e talismans. Voce paga um preco para nao perder estes items quando morrer.");
                    pl.SendMessage(78, "Voce pode usar o comando .insure");
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            VendorBuy(from);
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

      

        public Insurer(Serial serial)
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
