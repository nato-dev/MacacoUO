using Server.Network;
using Server.Mobiles;
using Server.Fronteira.Tutorial.WispGuia;

namespace Server.Gumps
{
    public class GumpFada : Gump
    {
        PlayerMobile caller;

        [Usage("")]
        public GumpFada(PlayerMobile from) : this()
        {
            caller = from;
        }

        public GumpFada() : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(127, 124, 351, 203, 9200);
            AddHtml(201, 174, 200, 23, @"Remover sua Fada Guia", (bool)false, (bool)false);
            AddHtml(201, 202, 198, 70, @"Voce nao ira ganhar alguns items de iniciante. (Nao Recomendado)", (bool)false, (bool)false);
            AddItem(415, 211, 8448);
            AddHtml(139, 134, 327, 23, @"<CENTER>Fada Guia Dragonic Age</CENTER>", (bool)true, (bool)false);
            AddHtml(203, 291, 200, 23, @"Conversar", (bool)false, (bool)false);
            AddHtml(419, 192, 41, 24, @"Oi oi", (bool)true, (bool)false);
            AddButton(175, 176, 1896, 1896, (int)Buttons.Remover, GumpButtonType.Reply, 0);
            AddButton(177, 291, 1896, 1896, (int)Buttons.Conversar, GumpButtonType.Reply, 0);
        }

        public enum Buttons
        {
            Nada,
            Remover,
            Conversar,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var from = sender.Mobile as PlayerMobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.Remover:
                    {
                        from.Wisp.Delete();
                        from.Wisp = null;
                        from.SendMessage("Sua Fada foi embora resmungando...");
                        from.PassoWispGuia = (int)PassoTutorial.FIM;
                        if(from.Profession==0)
                        {
                            from.SendGump(new NonRPClassGump());
                        }
                        break;
                    }
                case (int)Buttons.Conversar:
                    {
                        from.Wisp.ResetaCds();
                        break;
                    }

            }
        }
    }
}
