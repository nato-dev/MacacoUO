
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class GumpLore : Gump
    {

        public GumpLore(Mobile caller) : base(0, 0)
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = false;
            this.Resizable = false;

            AddPage(0);
            AddBackground(471, 230, 368, 442, 9380);
            AddHtml(497, 270, 311, 22, @"Seja Bem Vindo Aspirante!", (bool)false, (bool)false);
            AddHtml(498, 300, 312, 78, @"Você acaba de se alistar no Forte Fofnolsaern, nossa base de operações militares, lutamos contra um antigo mal que há muito tempo estava adormecido. ", (bool)false, (bool)false);
            AddHtml(497, 382, 311, 114, @"Em nossa base devemos trabalhar juntos para fortalecer nossas defesas, melhorar nossas habilidades em grupo e explorar o ninho do inimigo em busca de novas pistas que nos ajudem a erradicar este mal.", (bool)false, (bool)false);
            AddHtml(498, 499, 310, 48, @"Pouco se sabe de nossos inimigos a não ser por relatos e lendas antigas.", (bool)false, (bool)false);
            AddHtml(498, 550, 309, 44, @"Nós somos a esperança de nossa terra, nós somos a Última fronteira.", (bool)false, (bool)false);
            AddHtml(635, 612, 175, 18, @"Capitão Erich Larssem", (bool)false, (bool)false);
            AddButton(627, 649, 247, 248, (int)Buttons.Button1, GumpButtonType.Reply, 0);
            AddItem(607, 610, 4031);
        }

        public enum Buttons
        {
            Button1,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.Button1:
                    {
                        from.SendGump(new NonRPClassGump());
                        break;
                    }
            }
        }
    }
}
