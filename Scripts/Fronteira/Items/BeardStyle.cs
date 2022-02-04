/*This was made by TheArt from ServUO.  This was made to include horns in the hair restyling deed
Do not sell this and if you remake this please give me credit.  Please and thank you.
*/
using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{

    public class InternalHumanBaseGump : Gump
    {
        private readonly Mobile m_From;
        private readonly AdvancedBeardRestylingDeed m_Deed;
        /* 
        gump data: bgX, bgY, htmlX, htmlY, imgX, imgY, butX, butY 
        */
        readonly int[][] LayoutArray =
        {
                new int[] { 0 },
                /* padding: its more efficient than code to ++ the index/buttonid */ new int[] { 425, 280, 342, 295, 000, 000, 310, 292 },
                new int[] { 235, 060, 150, 075, 168, 020, 118, 073 },
                new int[] { 235, 115, 150, 130, 168, 070, 118, 128 },
                new int[] { 235, 170, 150, 185, 168, 130, 118, 183 },
                new int[] { 235, 225, 150, 240, 168, 185, 118, 238 },
                new int[] { 425, 060, 342, 075, 358, 018, 310, 073 },
                new int[] { 425, 115, 342, 130, 358, 075, 310, 128 },
                new int[] { 425, 170, 342, 185, 358, 125, 310, 183 },
                new int[] { 425, 225, 342, 240, 358, 185, 310, 238 },
                new int[] { 235, 280, 150, 295, 168, 245, 118, 292 }// slot 10, Curly - N/A for elfs.
            };
        /*
        racial arrays are: cliloc_F, cliloc_M, ItemID_F, ItemID_M, gump_img_F, gump_img_M
        */
        readonly int[][] HumanArray =
        {
                new int[] { 0 },
                new int[] { 3000340, 3000340, 0, 0, 0, 0 }, // no beard 
                new int[] { 3000352, 3000352, 0x203E, 0x203E, 0xC671, 0xC671 }, // Long Beard
                new int[] { 3000353, 3000353, 0x203F, 0x203F, 0xC672, 0xC672 }, // Short Beard
                new int[] { 3000356, 3000356, 0x204C, 0x204C, 0xC675, 0xC675 }, // Long Full Beard
                new int[] { 3000355, 3000355, 0x204B, 0x204B, 0xC676, 0xC676 }, // Short Full Beard
                new int[] { 3000351, 3000351, 0x2040, 0x2040, 0xC670, 0xC670 }, // Goatee
                new int[] { 3000357, 3000357, 0x204D, 0x204D, 0xC677, 0xC677 }, // vandyke
                new int[] { 3000354, 3000354, 0x2041, 0x2041, 0xC678, 0xC678 } // mustache
            };

        public InternalHumanBaseGump(Mobile from, AdvancedBeardRestylingDeed deed)
            : base(50, 50)
        {
            this.m_From = from;
            this.m_Deed = deed;

            from.CloseGump(typeof(InternalHumanBaseGump));

            this.AddBackground(100, 10, 400, 385, 0xA28);

            this.AddHtmlLocalized(100, 25, 400, 35, 1013008, false, false);
            this.AddButton(175, 340, 0xFA5, 0xFA7, 0x0, GumpButtonType.Reply, 0); // CANCEL

            this.AddHtmlLocalized(210, 342, 90, 35, 1011012, false, false);// <CENTER>HAIRSTYLE SELECTION MENU</center>

            int[][] RacialData = (from.Race == Race.Human) ? this.HumanArray : this.HumanArray;

            for (int i = 1; i < RacialData.Length; i++)
            {
                this.AddHtmlLocalized(this.LayoutArray[i][2], this.LayoutArray[i][3], (i == 1) ? 125 : 80, (i == 1) ? 70 : 35, (this.m_From.Female) ? RacialData[i][0] : RacialData[i][1], false, false);
                if (this.LayoutArray[i][4] != 0)
                {
                    this.AddBackground(this.LayoutArray[i][0], this.LayoutArray[i][1], 50, 50, 0xA3C);
                    this.AddImage(this.LayoutArray[i][4], this.LayoutArray[i][5], (this.m_From.Female) ? RacialData[i][4] : RacialData[i][5]);
                }
                this.AddButton(this.LayoutArray[i][6], this.LayoutArray[i][7], 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (this.m_From == null || !this.m_From.Alive)
                return;

            if (this.m_Deed.Deleted)
                return;

            if (info.ButtonID < 1 || info.ButtonID > 10)
                return;

            int[][] RacialData = (this.m_From.Race == Race.Human) ? this.HumanArray : this.HumanArray;

            if (this.m_From is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)this.m_From;

                pm.SetHairMods(-2, -2); // clear any hairmods (disguise kit, incognito)
                this.m_From.FacialHairItemID = (this.m_From.Female) ? RacialData[info.ButtonID][2] : RacialData[info.ButtonID][3];
                this.m_Deed.Delete();
            }
        }
    }
    public class InternalElvenBaseGump : Gump
    {
        private readonly Mobile m_From;
        private readonly AdvancedBeardRestylingElfDeed m_Deed;
        /* 
        gump data: bgX, bgY, htmlX, htmlY, imgX, imgY, butX, butY 
        */
        readonly int[][] LayoutArray =
        {
                new int[] { 0 },
                /* padding: its more efficient than code to ++ the index/buttonid */ new int[] { 425, 280, 342, 295, 000, 000, 310, 292 },
                new int[] { 235, 060, 150, 075, 168, 020, 118, 073 },
                new int[] { 235, 115, 150, 130, 168, 070, 118, 128 },
                new int[] { 235, 170, 150, 185, 168, 130, 118, 183 },
                new int[] { 235, 225, 150, 240, 168, 185, 118, 238 },
                new int[] { 425, 060, 342, 075, 358, 018, 310, 073 },
                new int[] { 425, 115, 342, 130, 358, 075, 310, 128 },
                new int[] { 425, 170, 342, 185, 358, 125, 310, 183 },
                new int[] { 425, 225, 342, 240, 358, 185, 310, 238 },
                new int[] { 235, 280, 150, 295, 168, 245, 118, 292 }// slot 10, Curly - N/A for elfs.
            };
        /*
        racial arrays are: cliloc_F, cliloc_M, ItemID_F, ItemID_M, gump_img_F, gump_img_M
        */
        readonly int[][] ElvenArray =
        {
                new int[] { 0 },
                new int[] { 3000340, 3000340, 0, 0, 0, 0 }, // no beard 
                new int[] { 3000352, 3000352, 0x203E, 0x203E, 0xC671, 0xC671 }, // Long Beard
                new int[] { 3000353, 3000353, 0x203F, 0x203F, 0xC672, 0xC672 }, // Short Beard
                new int[] { 3000356, 3000356, 0x204C, 0x204C, 0xC675, 0xC675 }, // Long Full Beard
                new int[] { 3000355, 3000355, 0x204B, 0x204B, 0xC676, 0xC676 }, // Short Full Beard
                new int[] { 3000351, 3000351, 0x2040, 0x2040, 0xC670, 0xC670 }, // Goatee
                new int[] { 3000357, 3000357, 0x204D, 0x204D, 0xC677, 0xC677 }, // vandyke
                new int[] { 3000354, 3000354, 0x2041, 0x2041, 0xC678, 0xC678 } // mustache
            };

        public InternalElvenBaseGump(Mobile from, AdvancedBeardRestylingElfDeed deed)
            : base(50, 50)
        {
            this.m_From = from;
            this.m_Deed = deed;

            from.CloseGump(typeof(InternalElvenBaseGump));

            this.AddBackground(100, 10, 400, 385, 0xA28);

            this.AddHtmlLocalized(100, 25, 400, 35, 1013008, false, false);
            this.AddButton(175, 340, 0xFA5, 0xFA7, 0x0, GumpButtonType.Reply, 0); // CANCEL

            this.AddHtmlLocalized(210, 342, 90, 35, 1011012, false, false);// <CENTER>HAIRSTYLE SELECTION MENU</center>

            int[][] RacialData = (from.Race == Race.Elf) ? this.ElvenArray : this.ElvenArray;

            for (int i = 1; i < RacialData.Length; i++)
            {
                this.AddHtmlLocalized(this.LayoutArray[i][2], this.LayoutArray[i][3], (i == 1) ? 125 : 80, (i == 1) ? 70 : 35, (this.m_From.Female) ? RacialData[i][0] : RacialData[i][1], false, false);
                if (this.LayoutArray[i][4] != 0)
                {
                    this.AddBackground(this.LayoutArray[i][0], this.LayoutArray[i][1], 50, 50, 0xA3C);
                    this.AddImage(this.LayoutArray[i][4], this.LayoutArray[i][5], (this.m_From.Female) ? RacialData[i][4] : RacialData[i][5]);
                }
                this.AddButton(this.LayoutArray[i][6], this.LayoutArray[i][7], 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (this.m_From == null || !this.m_From.Alive)
                return;

            if (this.m_Deed.Deleted)
                return;

            if (info.ButtonID < 1 || info.ButtonID > 10)
                return;

            int[][] RacialData = (this.m_From.Race == Race.Elf) ? this.ElvenArray : this.ElvenArray;

            if (this.m_From is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)this.m_From;

                pm.SetHairMods(-2, -2); // clear any hairmods (disguise kit, incognito)
                this.m_From.FacialHairItemID = (this.m_From.Female) ? RacialData[info.ButtonID][2] : RacialData[info.ButtonID][3];
                this.m_Deed.Delete();
            }
        }
    }
    public class InternalGargoyleBaseGump : Gump
    {
        private readonly Mobile m_From;
        private readonly AdvancedBeardRestylingDeed m_Deed;
        /* 
        gump data: bgX, bgY, htmlX, htmlY, imgX, imgY, butX, butY 
        */
        readonly int[][] LayoutArray =
        {
                new int[] { 0 },
                /* padding: its more efficient than code to ++ the index/buttonid */ new int[] { 425, 280, 342, 295, 000, 000, 310, 292 },
                new int[] { 235, 060, 150, 075, 168, 020, 118, 073 },
                new int[] { 235, 115, 150, 130, 168, 070, 118, 128 },
                new int[] { 235, 170, 150, 185, 168, 130, 118, 183 },
                new int[] { 235, 225, 150, 240, 168, 185, 118, 238 },
                new int[] { 425, 060, 342, 075, 358, 018, 310, 073 },
                new int[] { 425, 115, 342, 130, 358, 075, 310, 128 },
                new int[] { 425, 170, 342, 185, 358, 125, 310, 183 },
                new int[] { 425, 225, 342, 240, 358, 185, 310, 238 },
                new int[] { 235, 280, 150, 295, 168, 245, 118, 292 }// slot 10, Curly - N/A for elfs.
            };
        /*
        racial arrays are: cliloc_F, cliloc_M, ItemID_F, ItemID_M, gump_img_F, gump_img_M
        */

        readonly int[][] GargoyleArray =
        {
                new int[] { 0 },
                new int[] { 3000340, 3000340, 0, 0, 0, 0 }, // no FacialHorn 
                new int[] { 1112736, 1112736, 0x42AD, 0x42AD, 0xC5E9, 0xC5E9 }, // Horns
                new int[] { 1112736, 1112736, 0x42AE, 0x42AE, 0xC5DC, 0xC5DC }, // Horns
            };
        public InternalGargoyleBaseGump(Mobile from, AdvancedBeardRestylingDeed deed)
            : base(50, 50)
        {
            this.m_From = from;
            this.m_Deed = deed;

            from.CloseGump(typeof(InternalGargoyleBaseGump));

            this.AddBackground(100, 10, 400, 385, 0xA28);

            this.AddHtmlLocalized(100, 25, 400, 35, 1013008, false, false);
            this.AddButton(175, 340, 0xFA5, 0xFA7, 0x0, GumpButtonType.Reply, 0); // CANCEL

            this.AddHtmlLocalized(210, 342, 90, 35, 1011012, false, false);// <CENTER>HAIRSTYLE SELECTION MENU</center>

            int[][] RacialData = (from.Race == Race.Human) ? this.GargoyleArray : this.GargoyleArray;

            for (int i = 1; i < RacialData.Length; i++)
            {
                this.AddHtmlLocalized(this.LayoutArray[i][2], this.LayoutArray[i][3], (i == 1) ? 125 : 80, (i == 1) ? 70 : 35, (this.m_From.Female) ? RacialData[i][0] : RacialData[i][1], false, false);
                if (this.LayoutArray[i][4] != 0)
                {
                    this.AddBackground(this.LayoutArray[i][0], this.LayoutArray[i][1], 50, 50, 0xA3C);
                    this.AddImage(this.LayoutArray[i][4], this.LayoutArray[i][5], (this.m_From.Female) ? RacialData[i][4] : RacialData[i][5]);
                }
                this.AddButton(this.LayoutArray[i][6], this.LayoutArray[i][7], 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (this.m_From == null || !this.m_From.Alive)
                return;

            if (this.m_Deed.Deleted)
                return;

            if (info.ButtonID < 1 || info.ButtonID > 10)
                return;

            int[][] RacialData = (this.m_From.Race == Race.Human) ? this.GargoyleArray : this.GargoyleArray;

            if (this.m_From is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)this.m_From;

                pm.SetHairMods(-2, -2); // clear any hairmods (disguise kit, incognito)
                this.m_From.FacialHairItemID = (this.m_From.Female) ? RacialData[info.ButtonID][2] : RacialData[info.ButtonID][3];
                this.m_Deed.Delete();
            }
        }
    }

    public class AdvancedBeardRestylingDeed : Item
    {
        [Constructable]
        public AdvancedBeardRestylingDeed()
            : base(0x14F0)
        {
            this.Weight = 1.0;
            this.LootType = LootType.Blessed;
            this.Name = "Estilos de Barba";
        }

        public AdvancedBeardRestylingDeed(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack...
            }
            //If you want to allow females to use this, please remove from here down until the next green, then remove /* */
            else if (from.Female)
                from.SendMessage("Only males can use the deed.");

            else
            {

                from.SendGump(new InternalHumanBaseGump(from, this));

            }
            /*
            if (from.Race == Race.Human)
            from.SendGump(new InternalHumanBaseGump(from, this));
            if (from.Race == Race.Elf)
            from.SendGump(new InternalElvenBaseGump(from, this));
            if (from.Race == Race.Gargoyle)
            from.SendGump(new InternalGargoyleBaseGump(from, this));
            */

        }


    }

    public class AdvancedBeardRestylingElfDeed : Item
    {
        [Constructable]
        public AdvancedBeardRestylingElfDeed()
            : base(0x14F0)
        {
            this.Weight = 1.0;
            this.LootType = LootType.Blessed;
            this.Name = "Estilos de Barba Elficos";
        }

        public AdvancedBeardRestylingElfDeed(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack...
            }
            //If you want to allow females to use this, please remove from here down until the next green, then remove /* */
            else if (from.Female)
                from.SendMessage("Only males can use the deed.");

            else
            {
                from.SendGump(new InternalElvenBaseGump(from, this));
            }

        }


    }
}
