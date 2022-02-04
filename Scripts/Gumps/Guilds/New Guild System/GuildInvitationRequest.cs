using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Guilds
{
    public class GuildInvitationRequest : BaseGuildGump
    {
        readonly PlayerMobile m_Inviter;
        public GuildInvitationRequest(PlayerMobile pm, Guild g, PlayerMobile inviter)
            : base(pm, g)
        {
            this.m_Inviter = inviter;

            this.PopulateGump();
        }

        public override void PopulateGump()
        {
            this.AddPage(0);

            this.AddBackground(0, 0, 350, 170, 0x2422);
            this.AddHtml(25, 20, 300, 45, "Voce foi convidado a se juntar a uma guilda!",  true, false); // <center>You have been invited to join a guild! (Warning: Accepting will make you attackable!)</center>
            this.AddHtml(25, 75, 300, 25, String.Format("<center>{0}</center>", this.guild.Name), true, false);
            this.AddButton(265, 130, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);
            this.AddButton(195, 130, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0);
            this.AddButton(20, 130, 0xD2, 0xD3, 2, GumpButtonType.Reply, 0);
            this.AddHtml(45, 130, 150, 30, "Ignorar Convites",  false, false); // <i>Ignore Guild Invites</i>
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (this.guild.Disbanded || this.player.Guild != null)
                return;

            switch( info.ButtonID )
            {
                case 0:
                    {
                        this.m_Inviter.SendMessage("Convite recusado"); // ~1_val~ has declined your invitation to join ~2_val~.
                        break;
                    }
                case 1:
                    {
                        this.guild.AddMember(this.player);
                        this.player.SendMessage("Voce se uniu a guilda "+ this.guild.Name); // You have joined ~1_val~.
                        this.m_Inviter.SendMessage("Convite aceito. O jogador agora pertence a sua guilda"); // ~1_val~ has accepted your invitation to join ~2_val~.

                        break;
                    }
                case 2: 
                    {
                        this.player.AcceptGuildInvites = false;
                        this.player.SendMessage("Ignorando convites"); // You are now ignoring guild invitations.

                        break;
                    }
            }
        }
    }
}
