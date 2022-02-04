using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Guilds
{
    public class CreateGuildGump : Gump
    {
        public CreateGuildGump(PlayerMobile pm)
            : this(pm, "Guild Name", "")
        {
        }

        public CreateGuildGump(PlayerMobile pm, string guildName, string guildAbbrev)
            : base(10, 10)
        {
            pm.CloseGump(typeof(CreateGuildGump));
            pm.CloseGump(typeof(BaseGuildGump));

            this.AddPage(0);

            this.AddBackground(0, 0, 500, 300, 0x2422);
            this.AddHtml(25, 20, 450, 25, "Menu de Guildas", true, false); // <center>GUILD MENU</center>
            this.AddHtml(25, 60, 450, 60, "Como voce nao tem guilda voce pode criar uma", true, false); // As you are not a member of any guild, you can create your own by providing a unique guild name and paying the standard guild registration fee.
            this.AddHtml(25, 135, 120, 25, "Taxa de criacao:", false, false); // Registration Fee:
            this.AddLabel(155, 135, 0x481, Guild.RegistrationFee.ToString());
            this.AddHtml(25, 165, 120, 25, "Nome da guilda:", false, false); // Enter Guild Name: 
            this.AddBackground(155, 160, 320, 26, 0xBB8);
            this.AddTextEntry(160, 163, 315, 21, 0x481, 5, guildName);
            this.AddHtml(25, 191, 120, 26, "Tag:", false, false); // Abbreviation:
            this.AddBackground(155, 186, 320, 26, 0xBB8);
            this.AddTextEntry(160, 189, 315, 21, 0x481, 6, guildAbbrev);
            this.AddButton(415, 217, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);
            this.AddButton(345, 217, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0);

            if (pm.AcceptGuildInvites)
                this.AddButton(20, 260, 0xD2, 0xD3, 2, GumpButtonType.Reply, 0);
            else
                this.AddButton(20, 260, 0xD3, 0xD2, 2, GumpButtonType.Reply, 0);

            this.AddHtml(45, 260, 200, 30, "Ignorar Convites", false, false); // <i>Ignore Guild Invites</i>
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile pm = sender.Mobile as PlayerMobile;

            if (pm == null || pm.Guild != null)
                return;		//Sanity

            switch( info.ButtonID )
            {
                case 1:
                    {
                        TextRelay tName = info.GetTextEntry(5);
                        TextRelay tAbbrev = info.GetTextEntry(6);

                        string guildName = (tName == null) ? "" : tName.Text;
                        string guildAbbrev = (tAbbrev == null) ? "" : tAbbrev.Text;

                        guildName = Utility.FixHtml(guildName.Trim());
                        guildAbbrev = Utility.FixHtml(guildAbbrev.Trim());

                        if (guildName.Length <= 0)
                            pm.SendMessage("Nome vazio"); // Guild name cannot be blank.
                        else if (guildAbbrev.Length <= 0)
                            pm.SendMessage("Tag vazia"); // You must provide a guild abbreviation.
                        else if (guildName.Length > Guild.NameLimit)
                            pm.SendMessage("Nome muito grande"); // A guild name cannot be more than ~1_val~ characters in length.
                        else if (guildAbbrev.Length > Guild.AbbrevLimit)
                            pm.SendMessage("Tag muito grande"); // An abbreviation cannot exceed ~1_val~ characters in length.
                        else if (Guild.FindByAbbrev(guildAbbrev) != null || !BaseGuildGump.CheckProfanity(guildAbbrev))
                            pm.SendMessage("Tag nao disponivel"); // That abbreviation is not available.
                        else if (Guild.FindByName(guildName) != null || !BaseGuildGump.CheckProfanity(guildName))
                            pm.SendMessage("Nome nao disponivel"); // That guild name is not available.
                        else if (!Banker.Withdraw(pm, Guild.RegistrationFee))
                            pm.SendMessage("Custa "+Guild.RegistrationFee.ToString()+" moedas para isto e voce nao tem isso"); // You do not possess the ~1_val~ gold piece fee required to create a guild.
                        else
                        {
                            pm.SendMessage("Voce fundou uma nova guilda"); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                            pm.Guild = new Guild(pm, guildName, guildAbbrev);
                        }
                        break;
                    }
                case 2:
                    {
                        pm.AcceptGuildInvites = !pm.AcceptGuildInvites;

                        if (pm.AcceptGuildInvites)
                            pm.SendMessage("Aceitando convites"); // You are now accepting guild invitations.
                        else
                            pm.SendMessage("Negando convites"); // You are now ignoring guild invitations.

                        break;
                    }
            }
        }
    }
}
