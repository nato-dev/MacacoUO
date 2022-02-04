using Server;
using Server.Gumps;
using Server.Misc.Templates;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfirmDelete : Gump
{
    private Template template;

    public ConfirmDelete(Mobile from, Template template)
        : base(50, 50)
    {
        from.CloseGump(typeof(ConfirmEntranceGump));

        this.template = template;

        Closable = true;
        Disposable = true;
        Dragable = true;
        Resizable = false;

        AddPage(0);
        AddBackground(0, 0, 245, 145, 9250);
        AddHtml(21, 20, 203, 70, "<CENTER>Voce ira deletar a template "+template.Name+" para sempre<br> Tem certeza ???</CENTER>", false, false); // <CENTER>Your party is teleporting to an unknown area.<BR>Do you wish to go?</CENTER>
        AddButton(157, 101, 247, 248, 1, GumpButtonType.Reply, 0);
        AddButton(81, 100, 241, 248, 0, GumpButtonType.Reply, 0);
    }

    public override void OnResponse(NetState sender, RelayInfo info)
    {
        PlayerMobile from = (PlayerMobile)sender.Mobile;

        int button = info.ButtonID;

        switch (button)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    from.Templates.Templates.Remove(this.template);
                    var t = from.Templates.Templates[0];
                    from.CurrentTemplate = t.Name;
                    t.ToPlayer(from);
                    from.SendMessage("Template deletada");
                    break;
                }
        }
    }
}
