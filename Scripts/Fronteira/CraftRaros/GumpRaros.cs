using Server.Custom.RaresCrafting;
using Server.Gumps;
using Server.Leilaum;
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
        private int page = 0;

        public RaresCraftingGump(Mobile from, ECraftableRareCategory category, int page = 0)
            : base(0, 0)
        {
            from.CloseGump(typeof(RaresCraftingGump));

            m_Category = category;
            this.page = page;
            // MAIN
            AddPage(0);
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(73, 41, 588, 523, 9200);
            this.AddBackground(206, 75, 441, 488, 9200);
            this.AddLabel(80, 258, 2036, "Inscription");
            this.AddLabel(80, 156, 2036, "Blacksmithing");
            this.AddLabel(80, 192, 2036, "Carpentry");
            this.AddLabel(80, 88, 2036, "Alchemy");
            this.AddLabel(80, 292, 2036, "Tailoring");
            this.AddLabel(80, 326, 2036, "Tinkering");
            this.AddLabel(80, 122, 2036, "Bowcrafting");
            this.AddLabel(80, 224, 2036, "Cooking");
            //this.AddLabel(80, 356, 78, "Vale Decoracao");
            this.AddButton(172, 88, 4005, 4007, (int)Buttons.btnAlchemy, GumpButtonType.Reply, 0);
            this.AddButton(172, 122, 4005, 4007, (int)Buttons.btnBowcrafting, GumpButtonType.Reply, 0);
            this.AddButton(172, 156, 4005, 4007, (int)Buttons.btnBlacksmithing, GumpButtonType.Reply, 0);
            this.AddButton(172, 190, 4005, 4007, (int)Buttons.btnCarpentry, GumpButtonType.Reply, 0);
            this.AddButton(172, 224, 4005, 4007, (int)Buttons.btnCooking, GumpButtonType.Reply, 0);
            this.AddButton(172, 258, 4005, 4007, (int)Buttons.btnInscription, GumpButtonType.Reply, 0);
            this.AddButton(172, 292, 4005, 4007, (int)Buttons.btnTailoring, GumpButtonType.Reply, 0);
            this.AddButton(172, 326, 4005, 4007, (int)Buttons.btnTinkering, GumpButtonType.Reply, 0);
            //this.AddButton(172, 356, 4005, 4007, (int)Buttons.btnRandom, GumpButtonType.Reply, 0);

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

            var iFrom = page * 6;
            var iTo = page * 6 + 5;
            if (iTo >= craftables.Count)
                iTo = craftables.Count - 1;

            if(iTo < craftables.Count - 1)
            {
                this.AddLabel(548, 542, 0, "Proximo");
                this.AddButton(612, 540, 4005, 4007, (int)Buttons.Prox, GumpButtonType.Reply, 0);
            }

            if(iFrom > 0)
            {
                this.AddLabel(241, 541, 0, "Anterior");
                this.AddButton(210, 540, 4014, 4014, (int)Buttons.Anterior, GumpButtonType.Reply, 0);
            }

            for (var x = 0d; x < 6; x++)
            {
                int index = iFrom + (int)x;
                if (index < craftables.Count)
                {
                    var coluna = x % 2; // 0 ou 1
                    var linha = Math.Ceiling((x + 1) / 2) - 1;

                    var modX = (int)(coluna * 250) + 30;
                    var modY = (int)(linha * 160);

                    this.AddBackground(212 + modX, 102 + modY, 100, 100, 3500);
                    //this.AddBackground(209 + modX, 85 + modY, 108, 23, 3000);
                    this.AddLabel(214 + modX, 85 + modY, 2036, craftables[index].GetResult().m_Name);
                    this.AddButton(309 + modX, 139 + modY, 4005, 4007, index + (int)Buttons.btnCraftItemRangeStart, GumpButtonType.Reply, 0);
                    NewAuctionGump.AddItemCentered(212 + modX, 102 + modY, 100, 100, craftables[index].GetResult().m_ItemId, 0, this);
                }
                   

            
            }
        }

        public enum Buttons
        {
            None,
            Prox, Anterior, Page,
            btnAlchemy,
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
            if (info.ButtonID == (int)Buttons.Prox)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, m_Category, page + 1));
            }

            if (info.ButtonID == (int)Buttons.Anterior)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, m_Category, page - 1));
            }

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
