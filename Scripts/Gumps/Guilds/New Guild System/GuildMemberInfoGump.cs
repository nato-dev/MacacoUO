using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Guilds
{
    public class GuildMemberInfoGump : BaseGuildGump
    {
        readonly PlayerMobile m_Member;
        readonly bool m_ToLeader;
        readonly bool m_toKick;
        public GuildMemberInfoGump(PlayerMobile pm, Guild g, PlayerMobile member, bool toKick, bool toPromoteToLeader)
            : base(pm, g, 10, 40)
        {
            this.m_ToLeader = toPromoteToLeader;
            this.m_toKick = toKick;
            this.m_Member = member;
            this.PopulateGump();
        }

        public override void PopulateGump()
        {
            this.AddPage(0);

            this.AddBackground(0, 0, 350, 255, 0x242C);
            this.AddHtml(20, 15, 310, 26, "<div align=center><i>Informacoes</i></div>",  false, false); // <div align=center><i>Guild Member Information</i></div>
            this.AddImageTiled(20, 40, 310, 2, 0x2711);
			
            this.AddHtml(20, 50, 150, 26, "Nome",  true, false); // <i>Name</i>
            this.AddHtml(180, 53, 150, 26, this.m_Member.Name, false, false);
			
            this.AddHtml(20, 80, 150, 26, "Rank",  true, false); // <i>Rank</i>
            this.AddHtml(180, 83, 150, 26, this.m_Member.GuildRank.Name,  false, false);
			
            this.AddHtml(20, 110, 150, 26, "Titulo",  true, false); // <i>Guild Title</i>
            this.AddHtml(180, 113, 150, 26, this.m_Member.GuildTitle, false, false);
            this.AddImageTiled(20, 142, 310, 2, 0x2711);

            this.AddBackground(20, 150, 310, 26, 0x2486);
            this.AddButton(25, 155, 0x845, 0x846, 4, GumpButtonType.Reply, 0);
            this.AddHtml(50, 153, 270, 26, (this.m_Member == this.player.GuildFealty && this.guild.Leader != this.m_Member) ? "Limpo" : "Votado",  false, false); // Clear/Cast Vote For This Member
			
            this.AddBackground(20, 180, 150, 26, 0x2486);
            this.AddButton(25, 185, 0x845, 0x846, 1, GumpButtonType.Reply, 0);
            this.AddHtml(50, 183, 110, 26, "Promover", false, false); // Promote
			
            this.AddBackground(180, 180, 150, 26, 0x2486);
            this.AddButton(185, 185, 0x845, 0x846, 3, GumpButtonType.Reply, 0);
            this.AddHtml(210, 183, 110, 26, "Setar Titulo",  false, false); // Set Guild Title
			
            this.AddBackground(20, 210, 150, 26, 0x2486);
            this.AddButton(25, 215, 0x845, 0x846, 2, GumpButtonType.Reply, 0);
            this.AddHtml(50, 213, 110, 26, "Rebaixar",  false, false); // Demote
			
            this.AddBackground(180, 210, 150, 26, 0x2486);
            this.AddButton(185, 215, 0x845, 0x846, 5, GumpButtonType.Reply, 0);
            this.AddHtml(210, 213, 110, 26, "Kikar", false, false); // Kick
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile pm = sender.Mobile as PlayerMobile;

            if (pm == null || !IsMember(pm, this.guild) || !IsMember(this.m_Member, this.guild))
                return;

            RankDefinition playerRank = pm.GuildRank;
            RankDefinition targetRank = this.m_Member.GuildRank;

            switch( info.ButtonID )
            {
                case 1:	//Promote
                    {
                        if (playerRank.GetFlag(RankFlags.CanPromoteDemote) && ((playerRank.Rank - 1) > targetRank.Rank || (playerRank == RankDefinition.Leader && playerRank.Rank > targetRank.Rank)))
                        {
                            targetRank = RankDefinition.Ranks[targetRank.Rank + 1];

                            if (targetRank == RankDefinition.Leader)
                            {
                                if (this.m_ToLeader)
                                {
                                    this.m_Member.GuildRank = targetRank;
                                    pm.SendMessage("Atualizado"); // The guild information for ~1_val~ has been updated.
                                    this.guild.Leader = this.m_Member;
                                }
                                else
                                {
                                    pm.SendMessage("Tem certeza?"); // Are you sure you wish to make this member the new guild leader?
                                    pm.SendGump(new GuildMemberInfoGump(this.player, this.guild, this.m_Member, false, true));
                                }
                            }
                            else
                            {
                                this.m_Member.GuildRank = targetRank;
                                pm.SendMessage("Atualizado"); // The guild information for ~1_val~ has been updated.
                            }
                        }
                        else
                            pm.SendMessage("Sem permissao"); // You don't have permission to promote this member.

                        break;
                    }
                case 2:	//Demote
                    {
                        if (playerRank.GetFlag(RankFlags.CanPromoteDemote) && playerRank.Rank > targetRank.Rank)
                        {
                            if (targetRank == RankDefinition.Lowest)
                            {
                                if (RankDefinition.Lowest.Name.Number == 1062963)
                                    pm.SendMessage("Nao pode fazer isto"); // You can't demote a ronin.
                                else
                                    pm.SendMessage("Nao pode rebaixar mais q isso.");
                            }
                            else
                            {
                                this.m_Member.GuildRank = RankDefinition.Ranks[targetRank.Rank - 1];
                                pm.SendMessage("Atualizado"); // The guild information for ~1_val~ has been updated.
                            }
                        }
                        else
                            pm.SendMessage("Sem Permissao"); // You don't have permission to demote this member.
						
                        break;
                    }
                case 3:	//Set Guild title
                    {
                        if (playerRank.GetFlag(RankFlags.CanSetGuildTitle) && (playerRank.Rank > targetRank.Rank || this.m_Member == this.player))
                        {
                            pm.SendMessage("Digite o novo titulo ou none para remover"); // Enter the new title for this guild member or 'none' to remove a title:

                            pm.BeginPrompt(new PromptCallback(SetTitle_Callback));
                        }
                        else if (this.m_Member.GuildTitle == null || this.m_Member.GuildTitle.Length <= 0)
                        {
                            pm.SendMessage("Sem permissao"); // You don't have the permission to set that member's guild title.
                        }
                        else
                        {
                            pm.SendMessage("Sem permissao"); // You don't have permission to change this member's guild title.
                        }

                        break;
                    }
                case 4:	//Vote
                    {
                        if (this.m_Member == pm.GuildFealty && this.guild.Leader != this.m_Member)
                            pm.SendMessage("Limpou seus votos"); // You have cleared your vote for guild leader.
                        else if (this.guild.CanVote(this.m_Member))//( playerRank.GetFlag( RankFlags.CanVote ) )
                        {
                            if (this.m_Member == this.guild.Leader)
                                pm.SendMessage("Nao pode votar no lider"); // You can't vote for the current guild leader.
                            else if (!this.guild.CanBeVotedFor(this.m_Member))
                                pm.SendMessage("Nao pode votar em inativos"); // You can't vote for an inactive guild member.
                            else
                            {
                                pm.GuildFealty = this.m_Member;
                                pm.SendMessage("Voce votou em "+this.m_Member.Name); // You cast your vote for ~1_val~ for guild leader.
                            }
                        }
                        else
                            pm.SendMessage("Sem permissao para votar"); // You don't have permission to vote.

                        break;
                    }
                case 5:	//Kick
                    {
                        if ((playerRank.GetFlag(RankFlags.RemovePlayers) && playerRank.Rank > targetRank.Rank) || (playerRank.GetFlag(RankFlags.RemoveLowestRank) && targetRank == RankDefinition.Lowest))
                        {
                            if (this.m_toKick)
                            {
                                this.guild.RemoveMember(this.m_Member);
                                pm.SendMessage("Membro removido"); // The member has been removed from your guild.
                            }
                            else
                            {
                                pm.SendMessage("Tem certeza?"); // Are you sure you wish to kick this member from the guild?
                                pm.SendGump(new GuildMemberInfoGump(this.player, this.guild, this.m_Member, true, false));
                            }
                        }
                        else
                            pm.SendMessage("Sem Permissao"); // You don't have permission to remove this member.

                        break;
                    }
            }
        }

        public void SetTitle_Callback(Mobile from, string text)
        {
            PlayerMobile pm = from as PlayerMobile;
            PlayerMobile targ = this.m_Member;

            if (pm == null || targ == null)
                return;

            Guild g = targ.Guild as Guild;

            if (g == null || !IsMember(pm, g) || !(pm.GuildRank.GetFlag(RankFlags.CanSetGuildTitle) && (pm.GuildRank.Rank > targ.GuildRank.Rank || pm == targ)))
            {
                if (this.m_Member.GuildTitle == null || this.m_Member.GuildTitle.Length <= 0)
                    pm.SendMessage("Sem permissao"); // You don't have the permission to set that member's guild title.
                else
                    pm.SendMessage("Sem permissao"); // You don't have permission to change this member's guild title.

                return;
            }

            string title = Utility.FixHtml(text.Trim());

            if (title.Length > 20)
                from.SendMessage("Titulo maior que 20 caracteres"); // That title is too long.
            else if (!BaseGuildGump.CheckProfanity(title))
                from.SendMessage("Titulo negado"); // That title is disallowed.
            else
            {
                if (Insensitive.Equals(title, "none"))
                    targ.GuildTitle = null;
                else
                    targ.GuildTitle = title;

                pm.SendMessage("Atualizado"); // The guild information for ~1_val~ has been updated.
            }
        }
    }
}
