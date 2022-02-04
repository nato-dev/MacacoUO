#region References

using Server.Items;
using Server.Network;

#endregion

namespace Server.Gumps
{
    //offers a new game to a player
    public class AwaitRecruitmentGump : BoardGameGump
    {
        public override int Height { get { return 200; } }
        public override int Width { get { return 400; } }

        public AwaitRecruitmentGump(Mobile owner, BoardGameControlItem controlitem) : base(owner, controlitem)
        {
            //force it so players can't close this gump
            Closable = false;

            AddLabel(40, 20, 1152, "Game:");

            AddLabel(140, 20, 1172, _ControlItem.GameName);

            AddHtml(40, 50, Width - 80, 80,
                "Voce esta aguardando mais jogadores entrarem.  Quando mais jogadores vierem, o jogo ira comecar sozinho.  Se voce deseja cancelar, clique no botao..",
                true, false);

            AddButton(160, 160, 0xF1, 0xF2, 1, GumpButtonType.Reply, 0);
        }

        protected override void DeterminePageLayout()
        {}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int buttonid = info.ButtonID;

            //cancel button
            if (buttonid == 1)
            {
                _Owner.CloseGump(typeof(SelectStyleGump));
                _ControlItem.RemovePlayer(_Owner);

                _Owner.SendMessage("Voce nao esta mais esperando.");
            }
        }
    }
}
