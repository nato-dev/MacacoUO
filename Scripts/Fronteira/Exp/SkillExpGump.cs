using Server.Network;
using Server.Commands;
using Server.Mobiles;

namespace Server.Gumps
{
    public class SkillExperienceGump : Gump
    {
        public SkillExperienceGump(PlayerMobile caller, SkillName skill, ushort expGanha) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
 
            AddPage(0);
            AddBackground(10, 40, 220, 54, 9200);
            var pct = caller.Skills[skill].GetExp();
           
            AddHtml(20, 45, 200, 25, skill.GetName() +" "+pct+"%", true, false);  
            AddImageTiled(20, 72, 110, 12, 2053);
            AddImageTiled(20, 72, (int)pct, 12, 2054);
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
