using Server.Custom.RaresCrafting;
using Server.Gumps;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Custom.RaresCrafting
{
    public class RaresCraftingGump : Gump
    {
        private ECraftableRareCategory m_Category;

        public RaresCraftingGump(Mobile from, ECraftableRareCategory category)
            : base(0, 0)
        {
            from.CloseGump(typeof(RaresCraftingGump));

            m_Category = category;

            // MAIN
            AddPage(0);
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(73, 41, 588, 523, 9200);
            this.AddBackground(206, 75, 441, 467, 9200);
            this.AddLabel(80, 258, 2036, "Inscription");
            this.AddLabel(80, 156, 2036, "Blacksmithing");
            this.AddLabel(80, 192, 2036, "Carpentry");
            this.AddLabel(80, 88, 2036, "Alchemy");
            this.AddLabel(80, 292, 2036, "Tailoring");
            this.AddLabel(80, 326, 2036, "Tinkering");
            this.AddLabel(80, 122, 2036, "Bowcrafting");
            this.AddLabel(80, 224, 2036, "Cooking");
            this.AddLabel(80, 356, 78, "Vale Decoracao");
            this.AddButton(172, 88, 4005, 4007, (int)Buttons.btnAlchemy, GumpButtonType.Reply, 0);
            this.AddButton(172, 122, 4005, 4007, (int)Buttons.btnBowcrafting, GumpButtonType.Reply, 0);
            this.AddButton(172, 156, 4005, 4007, (int)Buttons.btnBlacksmithing, GumpButtonType.Reply, 0);
            this.AddButton(172, 190, 4005, 4007, (int)Buttons.btnCarpentry, GumpButtonType.Reply, 0);
            this.AddButton(172, 224, 4005, 4007, (int)Buttons.btnCooking, GumpButtonType.Reply, 0);
            this.AddButton(172, 258, 4005, 4007, (int)Buttons.btnInscription, GumpButtonType.Reply, 0);
            this.AddButton(172, 292, 4005, 4007, (int)Buttons.btnTailoring, GumpButtonType.Reply, 0);
            this.AddButton(172, 326, 4005, 4007, (int)Buttons.btnTinkering, GumpButtonType.Reply, 0);
            this.AddButton(172, 356, 4005, 4007, (int)Buttons.btnRandom, GumpButtonType.Reply, 0);

            if (m_Category == ECraftableRareCategory.None)
            {
                AddLabel(222, 180, 53, "Leia !");
                string s = " -Todos ingredientes precisam estar na sua mochila.\n -Ingredientes precisam ter o mesmo grafico.\n   -Isto inclui .virar mas nao cor do item\n -O resultado sera colocado em sua mochila e nao eh newbie.\n -Compre pozinho no alquimista ou no joalheiro !";
                AddHtml(222, 200, 400, 140, s, true, false);
            }
            List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);
            if (craftables == null)
            {
                return;
            }

            // Craft options
            if (craftables.Count > 0)
            {
                this.AddLabel(250, 85, 2036, craftables[0].GetResult().m_Name);
                this.AddButton(309, 139, 4005, 4007, (int)Buttons.btnCraftItemRangeStart, GumpButtonType.Reply, 0);
                this.AddItem(257 + craftables[0].DispOffsetX, 139 + craftables[0].DispOffsetY, craftables[0].GetResult().m_ItemId);
            }
            if (craftables.Count > 1)
            {
                this.AddLabel(507, 85, 2036, craftables[1].GetResult().m_Name);
                this.AddButton(584, 139, 4005, 4007, (int)Buttons.btnCraftItemRangeStart + 1, GumpButtonType.Reply, 0);
                this.AddItem(509 + craftables[1].DispOffsetX, 139 + craftables[1].DispOffsetY, craftables[1].GetResult().m_ItemId);
            }
            if (craftables.Count > 2)
            {
                this.AddLabel(250, 248, 2036, craftables[2].GetResult().m_Name);
                this.AddButton(309, 299, 4005, 4007, (int)Buttons.btnCraftItemRangeStart + 2, GumpButtonType.Reply, 0);
                this.AddItem(257 + craftables[2].DispOffsetX, 299 + craftables[2].DispOffsetY, craftables[2].GetResult().m_ItemId);
            }
            if (craftables.Count > 3)
            {
                this.AddLabel(507, 248, 2036, craftables[3].GetResult().m_Name);
                this.AddButton(584, 299, 4005, 4007, (int)Buttons.btnCraftItemRangeStart + 3, GumpButtonType.Reply, 0);
                this.AddItem(509 + craftables[3].DispOffsetX, 299 + craftables[3].DispOffsetY, craftables[3].GetResult().m_ItemId);
            }
        }

        public enum Buttons
        {
            btnAlchemy = 1,
            btnBowcrafting,
            btnBlacksmithing,
            btnCarpentry,
            btnCooking,
            btnInscription,
            btnTailoring,
            btnTinkering,
            btnRandom,
            btnCraftItemRangeStart = 1000,
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID >= (int)Buttons.btnCraftItemRangeStart && m_Category != ECraftableRareCategory.None)
            {
                List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);
                int which_item = info.ButtonID - (int)Buttons.btnCraftItemRangeStart;
                if (craftables.Count > which_item)
                    state.Mobile.SendGump(new CraftItemGump(state.Mobile, m_Category, which_item));
            }
            else if (info.ButtonID == (int)Buttons.btnAlchemy)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Alchemy));
            }
            else if (info.ButtonID == (int)Buttons.btnBowcrafting)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Bowcrafting));
            }
            else if (info.ButtonID == (int)Buttons.btnBlacksmithing)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Blacksmithing));
            }
            else if (info.ButtonID == (int)Buttons.btnCarpentry)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Carpentry));
            }
            else if (info.ButtonID == (int)Buttons.btnCooking)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Cooking));
            }
            else if (info.ButtonID == (int)Buttons.btnInscription)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Inscription));
            }
            else if (info.ButtonID == (int)Buttons.btnTailoring)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Tailoring));
            }
            else if (info.ButtonID == (int)Buttons.btnTinkering)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Tinkering));
            }
            else if (info.ButtonID == (int)Buttons.btnRandom)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.Random));
            }
        }
    }
}
