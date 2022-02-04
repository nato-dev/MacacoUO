using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Network;
using Server.Scripts.Custom.RelporMap;
using Server.Ziden;

namespace Server.Gumps
{

    public class Cmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("mapa", AccessLevel.Player, new CommandEventHandler(CmdMapa));
        }

        public static void CmdMapa(CommandEventArgs e)
        {
            e.Mobile.SendGump(new ZMapGump(e.Mobile, RelPorMapList.WorldMapCutout, new List<Point2D>()));
        }
    }


    public class ZMapGump : Gump
    {

        Mobile caller;
        MapCutout mapCutout;

        public ZMapGump(Mobile from, MapCutout cutout, List<Point2D> pinLocations)
            : this()
        {
            caller = from;
            mapCutout = cutout;
            AddPage(0);
            AddBackground(0, 0, 250, 260, 9380);

            if (mapCutout != null)
                AddImage(25, 30, mapCutout.GumpID);
            else
                from.CloseGump(typeof(ZMapGump));

            foreach (Point2D pin in pinLocations)
            {
                AddImage(pin.X + 25, pin.Y + 13, 0x2331);
            }

        }

        public ZMapGump()
            : base(100, 100)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
        }



        public override void OnResponse(NetState sender, RelayInfo info)
        {
        }
    }
}
