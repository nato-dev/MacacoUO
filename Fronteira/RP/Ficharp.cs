
//////////////////////////////////////////////////////////////////////
// Automatically generated by Bradley's GumpStudio and roadmaster's 
// exporter.dll,  Special thanks goes to Daegon whose work the exporter
// was based off of, and Shadow wolf for his Template Idea.
//////////////////////////////////////////////////////////////////////
#define RunUo2_0

using Server.Commands;
using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Gumps
{
    public static class RP
    {
        public static void Initialize()
        {
            CommandSystem.Register("ficharpteste", AccessLevel.Player, new CommandEventHandler(CMD));
        }

        [Usage("")]
        [Description("Makes a call to your custom gump.")]
        public static void CMD(CommandEventArgs e)
        {
            var caller = e.Mobile as PlayerMobile;
            if (caller == null)
                return;
            if (caller.HasGump(typeof(FichaRP)))
                caller.CloseGump(typeof(FichaRP));
            caller.SendGump(new FichaRP(caller));

        }
    }

    public class FichaRP : Server.Gumps.Gump
    {
        public PlayerMobile m;

        public FichaRP(PlayerMobile m) : base(0, 0)
        {
            this.m = m;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddImage(678, 158, 1596);
            this.AddImage(678, 300, 1597);
            this.AddImage(678, 438, 1599);
            this.AddButton(853, 301, 5575, 5576, (int)Buttons.BotaoPerfil, GumpButtonType.Reply, 0);
            this.AddButton(724, 406, 5587, 5588, (int)Buttons.BG, GumpButtonType.Reply, 0);
            this.AddButton(855, 405, 5569, 5570, (int)Buttons.NotasStaff, GumpButtonType.Reply, 0);
            this.AddButton(980, 403, 5581, 5582, (int)Buttons.Objetivos, GumpButtonType.Reply, 0);
            this.AddImage(820, 365, 1589);
            this.AddImage(943, 469, 1589);
            this.AddImage(818, 470, 1589);
            this.AddImage(694, 470, 1589);
            this.AddBackground(843, 236, 104, 24, 9300);
            this.AddHtml(856, 238, 125, 26, @"Ficha RP", (bool)false, (bool)false);
            this.AddHtml(861, 368, 151, 26, @"Perfil", (bool)false, (bool)false);
            this.AddHtml(713, 473, 151, 26, @"Background", (bool)false, (bool)false);
            this.AddHtml(837, 475, 151, 26, @"Notas Staff", (bool)false, (bool)false);
            this.AddHtml(968, 473, 151, 26, @"Objetivos", (bool)false, (bool)false);
        }

        public enum Buttons
        {
            Nada,
            BotaoPerfil,
            BG,
            NotasStaff,
            Objetivos,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (Shard.DebugEnabled)
                Shard.Debug("" + info.ButtonID);

            var from = sender.Mobile;
            if (info.ButtonID == (int)Buttons.BotaoPerfil)
            {
                from.SendGump(new Perfil(this.m));
            }
            else if (info.ButtonID == (int)Buttons.Objetivos)
            {
                from.SendGump(new Objetivos(this.m));
            }
            else if (info.ButtonID == (int)Buttons.NotasStaff)
            {
                from.SendGump(new NotasStaff(sender.Mobile as PlayerMobile, this.m));
            }
            else if (info.ButtonID == (int)Buttons.BG)
            {
                sender.Mobile.Send(new DisplayProfile(sender.Mobile != this.m || !this.m.ProfileLocked, this.m, "Background de "+this.m.Name, this.m.Profile, ""));
                //EventSink.InvokeProfileRequest(new ProfileRequestEventArgs(sender.Mobile, this.m, null));
            }
        }
    }

    public class Perfil : Server.Gumps.Gump
    {
        public PlayerMobile m;
        public Perfil(PlayerMobile from) : base(0, 0)
        {
            this.m = from;
            var ficha = this.m.FichaRP;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddImage(353, 179, 1596);
            this.AddImage(353, 321, 1597);
            this.AddImage(353, 603, 1599);
            this.AddImage(513, 193, 1589);
            this.AddHtml(551, 197, 151, 26, @"Perfil", (bool)false, (bool)false);
            this.AddBackground(390, 227, 344, 26, 9300);
            this.AddBackground(390, 259, 344, 26, 9300);
            this.AddImage(353, 463, 1598);
            this.AddBackground(391, 294, 344, 406, 9300);
            this.AddTextEntry(397, 300, 330, 395, 0, (int)Buttons2.Aparencia, ficha.Aparencia == null ? @"Descreva a aparencia do personagem. Isto sera o que outros irao ver quando clicarem no profile do seu personagem." : ficha.Aparencia);
            this.AddTextEntry(394, 230, 331, 20, 0, (int)Buttons2.Nome, ficha.Nome == null ? @"Digite o nome do personagem" : ficha.Nome);
            this.AddTextEntry(397, 263, 331, 20, 0, (int)Buttons2.Idade, ficha.Idade == 0 ? @"Digite a idade do personagem" : ficha.Idade.ToString());
        }

        public enum Buttons2
        {
            Aparencia,
            Nome,
            Idade,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var nome = info.GetTextEntry((int)Buttons2.Nome);
            var idade = info.GetTextEntry((int)Buttons2.Idade);
            var aparencia = info.GetTextEntry((int)Buttons2.Aparencia);
            var ficha = this.m.FichaRP;
            ficha.Nome = nome.Text;
            ficha.Aparencia = aparencia.Text;
            try
            {
                ficha.Idade = Int32.Parse(idade.Text);
            }
            catch (Exception e)
            {
                sender.Mobile.SendMessage("A idade precisa apenas aceita numeros");
            }
            sender.Mobile.SendGump(new FichaRP(this.m));
        }
    }

    public class NotasStaff : Server.Gumps.Gump
    {
        public PlayerMobile m;
        public NotasStaff(PlayerMobile viewer, PlayerMobile m) : base(0, 0)
        {
            this.m = m;
            var ficha = this.m.FichaRP;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddImage(353, 179, 1596);
            this.AddImage(353, 321, 1597);
            this.AddImage(353, 603, 1599);
            this.AddImage(513, 193, 1589);
            this.AddHtml(540, 197, 151, 26, @"Notas Staff", (bool)false, (bool)false);
            this.AddImage(353, 463, 1598);
            this.AddBackground(394, 230, 344, 464, 9300);
            if (viewer.AccessLevel >= AccessLevel.Counselor)
                this.AddTextEntry(400, 237, 330, 420, 0, 0, ficha.NotaStaff == null ? @"Descrevaos objetivos do personagem" : ficha.NotaStaff);
            else
                this.AddHtml(400, 237, 830, 120, ficha.NotaStaff == null ? @"Nenhuma nota de staffs" : ficha.NotaStaff, false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var ficha = this.m.FichaRP;
            if (sender.Mobile.AccessLevel >= AccessLevel.Counselor)
            {
                ficha.NotaStaff = info.GetTextEntry(0).Text;
                this.m.SendMessage(78, "Suas notas de staff foram alteradas !");
            }
            sender.Mobile.SendGump(new FichaRP(this.m));
        }
    }

    public class Objetivos : Server.Gumps.Gump
    {
        public PlayerMobile m;
        public Objetivos(PlayerMobile m) : base(0, 0)
        {
            this.m = m;
            var ficha = this.m.FichaRP;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddImage(353, 179, 1596);
            this.AddImage(353, 321, 1597);
            this.AddImage(353, 603, 1599);
            this.AddImage(513, 193, 1589);
            this.AddHtml(540, 197, 151, 26, @"Objetivos", (bool)false, (bool)false);
            this.AddImage(353, 463, 1598);
            this.AddBackground(393, 230, 344, 433, 9300);
            this.AddTextEntry(400, 237, 330, 420, 0, 0, ficha.Objetivos == null ? @"Descreva os objetivos do personagem" : ficha.Objetivos);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var ficha = this.m.FichaRP;
            ficha.Objetivos = info.GetTextEntry(0).Text;
            sender.Mobile.SendGump(new FichaRP(this.m));
        }
    }
}
