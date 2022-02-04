using Server.Fronteira.Pvm;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Gumps
{
    public class DungeonOption
    {
        public CustomDungeons Name;
        public Point3D Location;
        public Map Map;
    }

    public class DungeonSelect : Gump
    {
        List<DungeonOption> dgs;

        public DungeonSelect(List<DungeonOption> dgs, Mobile m)
            : base(0, 0)
        {
            this.dgs = dgs;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            var sizeY = 336;
            m.SendMessage(78, "Complete dungeons para liberar novas dungeons. Cada dungeon e completada de um modo, descubra !");

            this.AddBackground(114, 104, 499, sizeY, 9200);

            this.AddLabel(320, 110, 1153, @"Dungeons");

            var i = 0;
            var y = 0;
            foreach (var dg in dgs)
            {
                this.AddHtml(172, 130 + y, 1153, 23, @"", (bool)false, (bool)false);
                i += 1;
                this.AddHtml(172, 162 + y, 300, 23, "(Lvl "+i+")" + dg.Name, (bool)true, (bool)false);
                this.AddButton(144, 159 + y, 2472, 2472, i, GumpButtonType.Reply, 0);
                y += 30;
            }

            this.AddImage(141, 162, 7814);
            this.AddImage(139, 248, 7814);
            this.AddImage(137, 343, 7814);


            this.AddImage(64, 80, 10440);
            this.AddImage(581, 66, 10441);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);

            var player = (PlayerMobile)sender.Mobile;
            var from = player;

            if (info.ButtonID > 0)
            {
                var dg = dgs[info.ButtonID - 1];

                BaseCreature.TeleportPets(from, dg.Location, dg.Map);
                from.MoveToWorld(dg.Location, dg.Map);
                from.SendMessage("Voce chegou em " + dg.Name);
            }
        }
    }
}
