#region References

using System;
using System.Globalization;
using Server.Items;
using Server.Network;

#endregion

namespace Server.Gumps
{
    //offers a new game to a player
    public class OfferNewGameGump : BoardGameGump
    {
        protected bool _ControlNumberOfPlayers;

        public override int Height { get { return 500; } }
        public override int Width { get { return 400; } }

        public OfferNewGameGump(Mobile owner, BoardGameControlItem controlitem, bool controlnumberofplayers)
            : base(owner, controlitem)
        {
            _ControlNumberOfPlayers = controlnumberofplayers;

            AddLabel(40, 20, 1152, "Jogo:");

            AddLabel(140, 20, 1172, _ControlItem.GameName);

            AddLabel(40, 50, 1152, "Descricao:");

            AddHtml(40, 70, 300, 100, _ControlItem.GameDescription, true, true);

            AddLabel(40, 180, 1152, "Regras:");

            AddHtml(40, 200, 300, 150, _ControlItem.GameRules, true, true);

            if (_ControlItem.CostToPlay > 0)
            {
                AddLabel(40, 370, 1152, "Custo:");
                AddLabel(240, 370, 1172, _ControlItem.CostToPlay + " moedas");
            }

            if (_ControlItem.MaxPlayers != _ControlItem.MinPlayers)
            {
                AddLabel(40, 430, 1152,
                    "# de jogadores (" + _ControlItem.MinPlayers + "-" + _ControlItem.MaxPlayers +
                    "):");

                if (_ControlNumberOfPlayers)
                {
                    AddLabel(60, 410, 1172, "Escolha o numero de jogadores");
                    AddTextField(240, 430, 30, 20, 0, _ControlItem.CurrentMaxPlayers.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    AddLabel(240, 430, 1152, _ControlItem.CurrentMaxPlayers.ToString(CultureInfo.InvariantCulture));
                }
            }

            AddLabel(40, 470, 1152, "Jogar?");

            AddButton(200, 460, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);
            AddButton(300, 460, 0xF1, 0xF2, 0, GumpButtonType.Reply, 0);
        }

        protected override void DeterminePageLayout()
        {}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int buttonid = info.ButtonID;

            //cancel or right click
            if (buttonid == 0)
            {
                _ControlItem.RemovePlayer(_Owner);

                _Owner.SendMessage("Voce preferiu nao jogar");
            }
            else
            {
                if (_ControlItem.MaxPlayers != _ControlItem.MinPlayers && _ControlNumberOfPlayers)
                {
                    try
                    {
                        _ControlItem.CurrentMaxPlayers = Int32.Parse(GetTextField(info, 0));

                        if (_ControlItem.CurrentMaxPlayers > _ControlItem.MaxPlayers ||
                            _ControlItem.CurrentMaxPlayers < _ControlItem.MinPlayers)
                        {
                            throw (new Exception());
                        }
                    }
                    catch
                    {
                        _Owner.SendMessage("Numero invalido.");
                        _Owner.SendGump(new OfferNewGameGump(_Owner, _ControlItem, _ControlNumberOfPlayers));
                        return;
                    }
                }
                _Owner.SendMessage("Voce entrou no jogo.");

                _ControlItem.AddPlayer(_Owner);
            }
        }
    }
}
