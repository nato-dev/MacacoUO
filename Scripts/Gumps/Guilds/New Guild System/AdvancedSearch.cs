using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Guilds
{
    public delegate void SearchSelectionCallback(GuildDisplayType display);

    public class GuildAdvancedSearchGump : BaseGuildGump
    {
        private readonly GuildDisplayType m_Display;
        private readonly SearchSelectionCallback m_Callback;
        public GuildAdvancedSearchGump(PlayerMobile pm, Guild g, GuildDisplayType display, SearchSelectionCallback callback)
            : base(pm, g)
        {
            this.m_Callback = callback;
            this.m_Display = display;
            this.PopulateGump();
        }

        public override void PopulateGump()
        {
            base.PopulateGump();

            this.AddHtml(431, 43, 110, 26, "Diplomacia", 0xF, false, false); // Diplomacy

            this.AddHtml(65, 80, 480, 26, "Pesquisa", 0xF, true, false); // <i>Advanced Search Options</i>

            this.AddHtml(65, 110, 480, 26, "Mostrando Tudo" + (int)this.m_Display, 0xF, false, false); // Showing All Guilds/w/Relation/Waiting Relation

            this.AddGroup(1);
            this.AddRadio(75, 140, 0xD2, 0xD3, false, 2);
            this.AddHtml(105, 140, 200, 26, "Relacoes", 0x0, false, false); // Show Guilds with Relationship
            this.AddRadio(75, 170, 0xD2, 0xD3, false, 1);
            this.AddHtml(105, 170, 200, 26, "Aguardando Acoes", 0x0, false, false); // Show Guilds Awaiting Action
            this.AddRadio(75, 200, 0xD2, 0xD3, false, 0);
            this.AddHtml(105, 200, 200, 26, "Todas Guildas", 0x0, false, false); // Show All Guilds

            this.AddBackground(450, 370, 100, 26, 0x2486);
            this.AddButton(455, 375, 0x845, 0x846, 5, GumpButtonType.Reply, 0);
            this.AddHtml(480, 373, 60, 26, "OK", 0x0, false, false); // OK
            this.AddBackground(340, 370, 100, 26, 0x2486);
            this.AddButton(345, 375, 0x845, 0x846, 0, GumpButtonType.Reply, 0);
            this.AddHtml(370, 373, 60, 26, "Cancelar", 0x0, false, false); // Cancel
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);

            PlayerMobile pm = sender.Mobile as PlayerMobile;

            if (pm == null || !IsMember(pm, this.guild))
                return;

            GuildDisplayType display = this.m_Display;

            if (info.ButtonID == 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (info.IsSwitched(i))
                    {
                        display = (GuildDisplayType)i;
                        this.m_Callback(display);
                        break;
                    }
                }
            }
        }
    }
}
