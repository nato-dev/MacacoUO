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
    public class CraftItemGump : Gump
    {
        private ECraftableRareCategory m_Category;
        private int m_ItemIndex;
        Mobile m_From;
        public CraftItemGump(Mobile from, ECraftableRareCategory category, int itemidx) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            from.CloseAllGumps();

            m_Category = category;
            List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);

            m_ItemIndex = Math.Min(craftables.Count - 1, Math.Max(itemidx, 0));
            m_From = from;

            ICraftableRare the_rare = craftables[m_ItemIndex];

            this.AddPage(0);
            this.AddBackground(5, 5, 441, 361, 9200);
            this.AddBackground(13, 41, 139, 314, 9200);

            // result
            this.AddLabel(14, 16, 53, "Craftavel: ");
            this.AddItem(66 + the_rare.DispOffsetX, 135 + the_rare.DispOffsetY, the_rare.GetResult().m_ItemId);

            // ingredients
            Ingr[] ingredients = the_rare.GetIngredients();
            this.AddLabel(158, 16, 53, "Ingredientes:");

            this.AddItem(190, 81, ingredients[0].m_ItemId);
            this.AddLabel(249, 81, 2036, String.Format("{0} : {1}", ingredients[0].m_Name, ingredients[0].m_AmountRequired));

            if (ingredients.Length > 1)
            {
                this.AddItem(190, 135, ingredients[1].m_ItemId);
                this.AddLabel(249, 135, 2036, String.Format("{0} : {1}", ingredients[1].m_Name, ingredients[1].m_AmountRequired));
            }
            if (ingredients.Length > 2)
            {
                this.AddItem(190, 189, ingredients[2].m_ItemId);
                this.AddLabel(249, 189, 2036, String.Format("{0} : {1}", ingredients[2].m_Name, ingredients[2].m_AmountRequired));
            }
            if (ingredients.Length > 3) // TRANSFORMATION DUST!
            {
                this.AddItem(190, 275, ingredients[3].m_ItemId);
                this.AddLabel(249, 275, 2036, String.Format("{0} : {1}", ingredients[3].m_Name, ingredients[3].m_AmountRequired));
            }

            // transform
            this.AddLabel(345, 318, 2036, "Criar");
            this.AddButton(404, 317, 4005, 4007, (int)Buttons.ButtonCraft, GumpButtonType.Reply, 0);


            // required skills 
            this.AddLabel(158, 295, 53, "Skill Necessaria:");
            if (the_rare.m_FirstRequiredSkill.Length > 0)
            {
                int hue = the_rare.MeetsRequiredSkillLevel_1(m_From) ? 0x44 : 1643; // green or red
                this.AddLabel(180, 318, 2036, the_rare.m_FirstRequiredSkill);
                this.AddLabel(268, 318, hue, "100.0");
            }
            if (the_rare.m_SecondRequiredSkill.Length > 0)
            {
                int hue = the_rare.MeetsRequiredSkillLevel_2(m_From) ? 0x44 : 1643; // green or red
                this.AddLabel(180, 336, 2036, the_rare.m_SecondRequiredSkill);
                this.AddLabel(268, 336, hue, "100.0");
            }
        }

        public enum Buttons
        {
            ButtonCraft = 1,
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.ButtonCraft)
            {
                List<ICraftableRare> craftables = RaresCraftingSystem.GetCraftables(m_Category);
                ICraftableRare the_rare = craftables[m_ItemIndex];
                if (!the_rare.MeetsRequiredSkillLevel_1(state.Mobile) && the_rare.MeetsRequiredSkillLevel_2(state.Mobile))
                {
                    state.Mobile.SendMessage("Voce nao tem habilidade suficiente");
                    return;
                }

                RaresCraftingSystem.TryCreateItem(state.Mobile, the_rare);
            }
            state.Mobile.SendGump(new RaresCraftingGump(m_From, m_Category));
        }

    }
}
