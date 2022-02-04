using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class FomeSede : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("fome", AccessLevel.Player, new CommandEventHandler(FomeSede_OnCommand));
        }

        [Usage("fome")]
        [Description("Mostra a fome/sede do personagem.")]
        public static void FomeSede_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(FomeSede)))
                e.Mobile.CloseGump(typeof(FomeSede));
            e.Mobile.SendGump(new FomeSede(e.Mobile));
        }

        public FomeSede(Mobile caller) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(-7, 87, 145, 54, 9200);
            AddItem(-15, 94, 2429);
            AddItem(-17, 114, 2544);
            AddImageTiled(20, 98, 110, 12, 2053);
            AddImageTiled(20, 98, (int)(caller.Hunger * 5.5), 12, 2054);

            AddImageTiled(20, 118, 110, 12, 2053);
            AddImageTiled(20, 118, (int)(caller.Thirst * 5.5), 12, 2054);
            caller.SendMessage("Digite .fome para ver sua fome ou sede");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {

                        break;
                    }

            }
        }
    }
}
