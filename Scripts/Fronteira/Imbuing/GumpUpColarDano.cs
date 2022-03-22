using Server.Gumps;
using Server.Items;
using Server.Leilaum;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Imbuing
{
    public class ColarDanoGump : Gump
    {
        private AnelDano e;


        public ColarDanoGump(PlayerMobile pl, AnelDano colar) : base(0, 0)
        {
            e = colar;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(673 - 130, 334 - 20, 400, 150, 9200);
            AddHtml(673 - 125, 334 - 20, 100, 20, "Aprimorar Anel ?", false, false);
            var nivel = colar.Nivel;

            AddBackground(673 - 110, 334, 111, 101, 3500);
            AddHtml(711 - 110, 350, 183, 22, 5.ToString(), (bool)false, (bool)false);
            AddHtml(678 - 110, 406, 100, 22, "Frg. Reliquia", (bool)true, (bool)false);
            //AddItem(703, 374, custos.Item);
            NewAuctionGump.AddItemCentered(673 - 100, 334, 111, 101, 0x2DB3, Paragon.Hue, this);

            AddBackground(673, 334, 111, 101, 3500);
            AddHtml(711, 350, 183, 22, 20.ToString(), (bool)false, (bool)false);
            AddHtml(678, 406, 120, 22, "Crtl. Elemental", (bool)true, (bool)false);
            //AddItem(703, 374, custos.Item);
            NewAuctionGump.AddItemCentered(673, 334, 111, 101, 16395, 2611, this);

            AddBackground(784, 335, 111, 101, 3500);
            AddHtml(827, 350, 83, 22, 10.ToString(), (bool)false, (bool)false);
            AddHtml(793, 405, 100, 22, "Frg. Antigo", (bool)true, (bool)false);
            //AddItem(811, 367, 576);
            NewAuctionGump.AddItemCentered(784, 335, 111, 101, 0x1053, 1152, this);

            AddButton(804, 435, 247, 248, (int)ElementoButtons.Upar, GumpButtonType.Reply, 0);
        }

        public enum ElementoButtons
        {
            Nads,
            Upar,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var from = sender.Mobile as PlayerMobile;

            if (info.ButtonID != 1)
            {
                return;
            }

            if (!from.Backpack.HasItems(new Type[] { typeof(RelicFragment) }, new int[] { 5 }))
            {
                from.SendMessage("Falta fragmentos de reliquia");
                return;
            }
            var cristal = from.Backpack.FindItemByType<CristalElemental>();
            if (cristal == null || cristal.Amount < 20)
            {
                from.SendMessage("Faltam cristais elementais");
                return;
            }

            var frag = from.Backpack.FindItemByType<FragmentosAntigos>();
            if (frag == null || frag.Amount < 10)
            {
                from.SendMessage("Faltam fragmentos antigos");
                return;
            }

            frag.Consume(10);
            cristal.Consume(20);
            from.Backpack.ConsumeTotal(new Type[] { typeof(RelicFragment) }, new int[] { 5 });

            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
            Effects.PlaySound(from.Location, from.Map, 0x243);

            Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

            Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

            e.Nivel += 1;
            from.PlayAttackAnimation();
            from.OverheadMessage("* encantou *");
            from.SendMessage("Seu anel absorveu a energia");
            from.CloseAllGumps();
        }
    }
}
