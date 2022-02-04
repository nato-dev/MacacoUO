using Server.Commands;
using Server.Fronteira.Talentos;
using Server.Mobiles;
using System;

namespace Server.Gumps
{
    public class GumpLivroTalento : Server.Gumps.Gump
    {

        public static void Initialize()
        {
            CommandSystem.Register("vertalentos", AccessLevel.Administrator, new CommandEventHandler(_OnCommand));
        }

        [Usage("")]
        [Description("Makes a call to your custom gump.")]
        public static void _OnCommand(CommandEventArgs e)
        {
            var caller = e.Mobile;
            if (caller.HasGump(typeof(GumpLivroTalento)))
                caller.CloseGump(typeof(GumpLivroTalento));
            Talento[] Talentos = Enum.GetValues(typeof(Talento)).CastToArray<Talento>();
            caller.SendGump(new GumpLivroTalento(caller as PlayerMobile, Talentos));
        }

        public GumpLivroTalento(PlayerMobile p, Talento [] Talentos) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            var page = 1;

            this.AddPage(0);
            this.AddImage(241, 335, 11058);

            for (var x = 0; x < Talentos.Length; x += 2)
            {
                this.AddPage(page);
                this.AddButton(563, 343, 2206, 2206, (int)1, GumpButtonType.Page, page + 1);
                if (page > 0)
                    this.AddButton(284, 344, 2205, 2205, (int)2, GumpButtonType.Page, page - 1);

                var t1 = (Talento)Talentos[x];
                var def = DefTalentos.GetDef(t1);

                this.AddButton(336, 388, def.Icone, def.Icone, (int)t1 + 1, GumpButtonType.Reply, 0);
                this.AddHtml(291, 433, 146, 112, def.Desc1, (bool)false, (bool)false);
                this.AddHtml(291, 367, 151, 20, def.Nome, (bool)false, (bool)false);

                if (Talentos.Length - 1 >= x + 1)
                {
                    var t2 = (Talento)Talentos[x + 1];

                    var def2 = DefTalentos.GetDef(t2);

                    this.AddButton(503, 390, def2.Icone, def2.Icone, (int)t2 + 1, GumpButtonType.Reply, 0);
                    this.AddHtml(451, 433, 146, 112, def2.Desc1, (bool)false, (bool)false);
                    this.AddHtml(451, 368, 145, 20, def2.Nome, (bool)false, (bool)false);
                }
                page = page + 1;
            }
        }
    }
}

