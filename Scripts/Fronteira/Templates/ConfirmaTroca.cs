using Server.Gumps;
using Server.Misc.Templates;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Templates
{
    public class ConfirmaTroca : BaseConfirmGump
    {
        private Template t;
        private PlayerMobile f;

        public override string LabelString { get { return "Voce ira perder seus seguidores !"; } }
        public override string TitleString
        {
            get
            {
                return "Deseja trocar de template mesmo assim ?";
            }
        }

        public ConfirmaTroca(PlayerMobile from, Template t)
        {
            this.t = t;
            this.f = from;
        }

        public override void Confirm(Mobile from)
        {
            var player = from as PlayerMobile;
            t.CopySkillsFromPlayer(player);
            player.CurrentTemplate = t.Name;
            player.CloseAllGumps();
            player.SendGump(new TemplatesGump(player));
        }

        public override void Refuse(Mobile from)
        {
            from.CloseGump(typeof(TemplatesGump));
            base.Refuse(from);
        }
    }
}
