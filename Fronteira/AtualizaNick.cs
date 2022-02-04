using Server.Gumps;
using Server.Misc;
using Server.Network;

namespace Server.Ziden
{
    public class AtualizaNick : Gump
    {
        public AtualizaNick()
           : base(0, 0)
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(141, 110, 378, 280, 9200);
            this.AddHtml(196, 123, 248, 20, @"Seu Nome nao eh Valido", (bool)false, (bool)false);
            this.AddHtml(196, 154, 249, 60, @"Digite um novo nome para seu personagem.", (bool)false, (bool)false);

            this.AddHtml(196, 223, 248, 25, @"", (bool)true, (bool)false);
            this.AddTextEntry(196, 223, 248, 25, 0, 0, "");
            this.AddButton(159, 219, 9721, 9721, 1, GumpButtonType.Reply, 0);
            this.AddItem(159, 219, 0xFBF);
            /*
            this.AddButton(159, 219, 9721, 9721, (int)Buttons.Teleportar, GumpButtonType.Reply, 0);
            if (!Shard.WARSHARD)
                this.AddHtml(196, 223, 248, 25, @"Teleportar a um curandeiro", (bool)true, (bool)false);
            else
                this.AddHtml(196, 223, 248, 25, @"Teleportar para o Hall", (bool)true, (bool)false);
            this.AddHtml(195, 296, 248, 25, @"Continuar como alma", (bool)true, (bool)false);
            this.AddButton(159, 292, 9721, 9721, (int)Buttons.Continuar, GumpButtonType.Reply, 0);
            if (Shard.WARSHARD)
                this.AddHtml(197, 246, 309, 35, @"Voce será ressuscitado e enviado ao Hall", (bool)false, (bool)false);
            else
                this.AddHtml(197, 246, 309, 35, @"Voce será ressuscitado pelo curandeiro mais proximo", (bool)false, (bool)false);
            this.AddHtml(196, 321, 307, 59, @"Continue vagando como alma até encontrar um curandeiro ou alguém que lhe retorne à vida", (bool)false, (bool)false);
            this.AddItem(187, 153, 3808);
            this.AddItem(163, 133, 3799);
            this.AddItem(148, 162, 3897);
            this.AddItem(195, 166, 3794);
            this.AddItem(193, 173, 587);
            */
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var m = sender.Mobile;

            if(info.ButtonID == 1)
            {
                var name = info.GetTextEntry(0).Text;
                if (!NameVerification.Validate(name, 2, 16, true, false, true, 4, NameVerification.SpaceDashPeriodQuote))
                    m.SendMessage("Este nome nao e valido.");
                else
                {
                    m.Name = name;
                    m.SendMessage("Voce mudou seu nome para " + name);
                    foreach(var item in m.Backpack.Items)
                    {
                        if (item.BoundTo != null)
                            item.BoundTo = name;
                    }
                }
            }
            base.OnResponse(sender, info);
        }

    }
}
