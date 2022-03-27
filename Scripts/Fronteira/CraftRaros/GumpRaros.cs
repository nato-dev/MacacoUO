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
            this.AddBackground(87, 75, 558, 488, 9200);
            this.AddLabel(317, 52, 0, "CRAFT ITENS RAROS");

            if (m_Category == ECraftableRareCategory.None)
            {
                this.AddLabel(172, 88, 2036, "Alchemy");
                this.AddLabel(340, 88, 2036, "Bowcrafting");
                this.AddLabel(500, 88, 2036, "Blacksmithing");
                this.AddLabel(172, 210, 2036, "Carpentry");
                this.AddLabel(340, 210, 2036, "Cooking");
                this.AddLabel(500, 210, 2036, "Inscription");            
                this.AddLabel(172, 330, 2036, "Tailoring");
                this.AddLabel(340, 330, 2036, "Tinkering");         
                //this.AddLabel(80, 356, 78, "Vale Decoracao");

                this.AddItem(152, 118, 0x9D83); // Alchemy
                this.AddItem(320, 118, 0x9C35); // Bowcrafting
                this.AddItem(320, 159, 0x1022); // Bowcrafting
                this.AddItem(480, 113, 0x9A86); // Blacksmithing
                this.AddItem(152, 240, 0x9C2D); // Carpentry
                this.AddItem(320, 240, 0x9D9F); // Cooking
                this.AddItem(480, 240, 0x0A9B); // Inscription
                this.AddItem(152, 360, 0x101E); // Tayloring
                this.AddItem(320, 360, 0x4CE7); // Tinkering

                this.AddButton(212, 138, 2151, 2153, (int)Buttons.btnAlchemy, GumpButtonType.Reply, 0);            
                this.AddButton(380, 138, 2151, 2153, (int)Buttons.btnBowcrafting, GumpButtonType.Reply, 0);
                this.AddButton(545, 138, 2151, 2153, (int)Buttons.btnBlacksmithing, GumpButtonType.Reply, 0);
                this.AddButton(212, 260, 2151, 2153, (int)Buttons.btnCarpentry, GumpButtonType.Reply, 0);
                this.AddButton(380, 260, 2151, 2153, (int)Buttons.btnCooking, GumpButtonType.Reply, 0);
                this.AddButton(545, 260, 2151, 2153, (int)Buttons.btnInscription, GumpButtonType.Reply, 0);
                this.AddButton(212, 380, 2151, 2153, (int)Buttons.btnTailoring, GumpButtonType.Reply, 0);
                this.AddButton(380, 380, 2151, 2153, (int)Buttons.btnTinkering, GumpButtonType.Reply, 0);
                //this.AddButton(172, 356, 4005, 4007, (int)Buttons.btnRandom, GumpButtonType.Reply, 0);

            
                AddLabel(168, 438, 53, "Leia !");
                string s = " -Todos ingredientes precisam estar na sua mochila.\n -Ingredientes precisam ter o mesmo grafico.\n -Isto inclui .virar mas nao cor do item\n -O resultado sera colocado em sua mochila e nao eh newbie.\n -Compre pozinho no alquimista ou no joalheiro !";
                AddHtml(168, 458, 400, 100, s, true, false);
            }

            List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);
            if (craftables == null)
            {
                return;
            }

            var iFrom = page * 9;
            var iTo = page * 9 + 8;
            if (iTo >= craftables.Count)
                iTo = craftables.Count - 1;

            if(iTo < craftables.Count - 1)
            {
                this.AddLabel(566, 542, 0, "Proximo");
                this.AddButton(612, 540, 4005, 4007, (int)Buttons.Prox, GumpButtonType.Reply, 0);
            }

            if(iFrom > 0)
            {
                this.AddLabel(121, 541, 0, "Anterior");
                this.AddButton(91, 540, 4014, 4014, (int)Buttons.Anterior, GumpButtonType.Reply, 0);
            }

            if(iFrom == 0)
            {
                this.AddLabel(121, 541, 0, "Inicio");
                this.AddButton(91, 540, 4014, 4014, (int)Buttons.Inicio, GumpButtonType.Reply, 0);
            }

            for (var x = 0d; x < 9; x++)
            {
                int index = iFrom + (int)x;
                if (index < craftables.Count)
                {
                    var coluna = x % 3; // 0 ou 1
                    var linha = Math.Ceiling((x + 1) / 3) - 1;

                    var modX = (int)(coluna * 160) + 16;
                    var modY = (int)(linha * 160);

                    this.AddBackground(132 + modX, 102 + modY, 100, 100, 3500);
                    //this.AddBackground(209 + modX, 85 + modY, 108, 23, 3000);
                    this.AddLabel(134 + modX, 85 + modY, 2036, craftables[index].GetResult().m_Name);                    
                    this.AddButton(228 + modX, 140 + modY, 2151, 2153, index + (int)Buttons.btnCraftItemRangeStart, GumpButtonType.Reply, 0);
                    NewAuctionGump.AddItemCentered(134 + modX, 102 + modY, 100, 100, craftables[index].GetResult().m_ItemId, 0, this);
                }                         
            }
        }

        public enum Buttons
        {
            None,
            Prox, Anterior, Inicio, Page, 
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

            if (info.ButtonID == (int)Buttons.Inicio)
            {
                state.Mobile.SendGump(new RaresCraftingGump(state.Mobile, ECraftableRareCategory.None));
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
