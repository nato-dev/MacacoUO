using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Leilaum;
using Server.Mobiles;
using Server.Network;
using VitaNex;

namespace Server.Gumps
{
    public class LootsGump : Gump
    {
        public List<KeyValuePair<Item, Mobile>> loots;
        public LootsGump(List<KeyValuePair<Item, Mobile>> loots) : base(0, 0)
        {
            this.loots = loots;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(33, 21, 201, 53, 9200);
            this.AddHtml(76, 38, 97, 16, @"Ver Loots ", (bool)false, (bool)false);
            this.AddButton(41, 32, 2151, 2151, (int)1, GumpButtonType.Reply, 0);
            this.AddItem(183, 25, 2475);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if(info.ButtonID == 1)
            {
                sender.Mobile.SendGump(new VerLootsGump(sender.Mobile, loots, 0));
            }
        }
    }

    public class VerLootsGump : Gump
    {
        public List<KeyValuePair<Item, Mobile>> loots;
        public int page;
        public Mobile sender;

        public VerLootsGump(Mobile viewer, List<KeyValuePair<Item, Mobile>> loots, int page = 0) : base(0, 0)
        {
            sender = viewer;
            this.page = page;
            this.loots = loots.ToList();
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
         

            this.AddBackground(43, 61, 255, 577, 9200);

            this.AddBackground(30, 34, 282, 28, 9200);
            this.AddHtml(39, 37, 260, 20, "<CENTER>LOOTS</CENTER>" , 55, (bool)false, (bool)false);

            var pag = 1;
            var n = 0;

            var from = page * 8;
            var to = page * 8 + 7;

            if (to >= loots.Count)
                to = loots.Count - 1;

            for(var i = from; i <= to; i++)
            {
                if (i < 0 || i > this.loots.Count)
                    continue;

                var item = this.loots[i].Key;
                var player = this.loots[i].Value;
                this.AddBackground(51, 66 + (n * 70), 50, 50, 3000);
                var hue = item.Hue;
                if (item.HueRaridade != 0)
                    hue = item.HueRaridade;
                NewAuctionGump.AddItemCentered(51, 66 + (n * 70), 50, 50, item.ItemID, hue, this);
                AddItemProperty(item.Serial);
                this.AddHtml(103, 67 + (n * 70), 190, 20, item.Amount + " " + item.Name ?? Clilocs.GetString(ClilocLNG.ENU, item.GetType()), 78, (bool)false, (bool)false);
                this.AddHtml(104, 95 + (n * 70), 182, 20, player == null ? viewer.Name : player.Name , 200, (bool)false, (bool)false);
                n++;
            }

            if (to < loots.Count - 1)
                this.AddButton(263, 610, 4007, 4007, (int)2, GumpButtonType.Reply, 0);

            if(from > 0)
                this.AddButton(48, 612, 4014, 4014, (int)1, GumpButtonType.Reply, 0);

            this.AddItem(283, 19, 2475);
            this.AddItem(245, 18, 2476);
            this.AddItem(262, 33, 2477);
            this.AddItem(273, 44, 2472);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 2)
            {
                sender.Mobile.SendGump(new VerLootsGump(sender.Mobile, loots, page + 1));
            }
            else if (info.ButtonID == 1)
            {
                sender.Mobile.SendGump(new VerLootsGump(sender.Mobile, loots, page - 1));
            }
        }
    }
}
