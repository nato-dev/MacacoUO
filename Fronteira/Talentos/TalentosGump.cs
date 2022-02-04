using Server.Fronteira.Talentos;
using Server.Mobiles;
using Server.Network;
/*
namespace Server.Gumps
{

  public class TalentosGump : Server.Gumps.Gump
  {

      public TalentosGump(PlayerMobile from) : base(0, 0)
      {
          this.Closable = true;
          this.Disposable = true;
          this.Dragable = true;
          this.Resizable = false;
          this.AddPage(0);
          this.AddBackground(291, 154, 454, 186, 1579);
          this.AddHtml(308, 279, 83, 25, @"Hipismo", (bool)false, (bool)false);
          this.AddHtml(368, 238, 30, 20, from.Talentos.GetNivel(Talento.Hipismo) + "/3", (bool)false, (bool)false);
          this.AddButton(484, 228, 39860, 39860, (int)Buttons.Hipismo, GumpButtonType.Reply, 0);
          this.AddHtml(533, 240, 30, 20, @"0/3", (bool)false, (bool)false);
          this.AddHtml(465, 276, 83, 25, @"Em Breve", (bool)false, (bool)false);
          this.AddButton(638, 228, 39860, 39860, (int)Buttons.Nada2, GumpButtonType.Reply, 0);
          this.AddHtml(687, 241, 30, 20, @"0/3", (bool)false, (bool)false);
          this.AddHtml(620, 277, 83, 25, @"Em Breve", (bool)false, (bool)false);
          this.AddHtml(462, 176, 97, 21, @"TALENTOS", (bool)false, (bool)false);
          this.AddButton(318, 230, 20745, 20745, (int)Buttons.Nada2, GumpButtonType.Reply, 0);
      }

      public enum Buttons
      {
          Fechou,
          Hipismo,
          Nada1,
          Nada2,
      }

      public override void OnResponse(NetState sender, RelayInfo info)
      {
          switch (info.ButtonID)
          {
              case (int)Buttons.Hipismo:
                  sender.Mobile.SendMessage("Voce deve aprender os talentos de outra forma...");
                  break;
              case (int)Buttons.Nada1:
              case (int)Buttons.Nada2:
                  sender.Mobile.SendMessage("Em breve...");
                  break;
          }
      }
  }
}
*/
