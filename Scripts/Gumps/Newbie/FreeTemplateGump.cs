
using Server.Network;
using Server.Commands;
using Server.Gumps.Newbie;
using Server.Items;
using Server.Mobiles;
using Server.Scripts.New.Adam.NewGuild;
using Server.Misc.Templates;
using Server.Misc.Custom;
using Server.Items.Functional.Pergaminhos;
using System;
using System.Linq;

namespace Server.Gumps
{
    public class FreeTemplateGump : Gump
    {
        string chosen = null;
        string desc = null;

        private StarterKits.Kit k;
        private bool newCharacter;

        private SkillName[] skills;
        private int v;

        public FreeTemplateGump(SkillName[] template, int valor) : base(0, 0)
        {
            this.skills = template;
            this.v = valor;
            this.Closable = !newCharacter;
            this.Disposable = !newCharacter;
            this.Dragable = false;
            this.Resizable = false;
            this.newCharacter = newCharacter;

            StarterKits.BuildKits();

            AddPage(0);
            AddBackground(79, 69, 493, 464, 9200);
            AddButton(353, 104, 5553, 5554, 3, GumpButtonType.Reply, 0);
            //if (newCharacter)
            //    AddButton(473, 185, 5545, 5546, 8, GumpButtonType.Reply, 0);
            AddButton(107, 103, 5553, 5554, 1, GumpButtonType.Reply, 0);
            AddButton(227, 102, 5553, 5554, 2, GumpButtonType.Reply, 0);
            AddButton(472, 103, 5553, 5554, 4, GumpButtonType.Reply, 0);
            AddButton(107, 181, 5553, 5554, 5, GumpButtonType.Reply, 0);
            AddButton(228, 183, 5553, 5554, 6, GumpButtonType.Reply, 0);
            //AddButton(355, 183, 5571, 5572, 7, GumpButtonType.Reply, 0);
            AddImage(29, 13, 10440);
            AddImage(540, 14, 10441);
            AddImage(234, -167, 1418);
            AddHtml(100, 84, 86, 19, @"War PvP", (bool)false, (bool)false);
            AddHtml(224, 84, 86, 19, @"War PvM", (bool)false, (bool)false);
            AddHtml(359, 83, 86, 19, @"Mago PvP", (bool)false, (bool)false);
            AddHtml(471, 83, 124, 19, @"Mago PvM", (bool)false, (bool)false);
            AddHtml(105, 163, 124, 19, @"Arqueiro PvP", (bool)false, (bool)false);
            AddHtml(235, 164, 124, 19, @"Arqueiro PvM", (bool)false, (bool)false);


            //if (newCharacter)
            //    AddHtml(450, 166, 124, 19, @"Manter Skills", (bool)false, (bool)false);

            var desc = "Selecione uma template de skills iniciais.";

            desc = "Suas skills irao resetar e as skills da classe vao para 90";
            AddHtml(103, 258, 441, 83, desc, (bool)true, (bool)false);
            AddButton(473, 498, 247, 248, 10, GumpButtonType.Reply, 0);

            AddHtml(104, 368, 441, 83, "", (bool)true, (bool)false);
            AddHtml(106, 345, 200, 20, @"Skills", (bool)false, (bool)false);

            var x = 0;
            var y = 0;

            foreach (var skillname in template)
            {
                var value = 90;
                AddHtml(110 + x, 370 + y, 441, 83, skillname + ": " + value, false, false);
                x += 120;
                if (x > 330)
                {
                    x = 0;
                    y += 30;
                }
            }

        }

        private static void EquipItem(Mobile mob, Item item)
        {
            if (mob != null && mob.EquipItem(item))
                return;

            var pack = mob.Backpack;

            if (pack != null)
                pack.DropItem(item);
        }

        private static void PackItem(Mobile mob, Item item)
        {

            var pack = mob.Backpack;

            if (pack != null)
                pack.DropItem(item);
            else
                item.Delete();
        }

        public static int TEMP_ID = 2;

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                from.SendGump(new FreeTemplateGump(TemplateDeed.WarPvP(), v));
            }
            else if (info.ButtonID == 2)
            {
                from.SendGump(new FreeTemplateGump(TemplateDeed.WarPvM(), v));
            }
            else if (info.ButtonID == 3)
            {
                from.SendGump(new FreeTemplateGump(TemplateDeed.MagePvP(), v));
            }
            else if (info.ButtonID == 4)
            {
                from.SendGump(new FreeTemplateGump(TemplateDeed.MagePvM(), v));
            }
            else if (info.ButtonID == 5)
            {
                from.SendGump(new FreeTemplateGump(TemplateDeed.ArqueiroPvP(), v));
            }
            else if (info.ButtonID == 6)
            {
                from.SendGump(new FreeTemplateGump(TemplateDeed.ArqueiroPvM(), v));
            }
            else if (info.ButtonID == 10)
            {
                var pl = from as PlayerMobile;

                var deed = pl.Backpack.FindItemByType(typeof(TemplateDeed));
                if(deed == null)
                {
                    return;
                }
                deed.Consume();
                if (pl.Wisp != null)
                    pl.Wisp.Delete();
                pl.Wisp = null;

                var temp = new Template();
                temp.Name = "Template " + info.ButtonID;
                temp.ToPlayer(pl);
                pl.Templates.Templates.Add(temp);
                pl.CurrentTemplate = temp.Name;

                foreach (var skill in from.Skills)
                {
                    skill.SendPacket = false;
                    skill.Base = 0;
                    skill.SendPacket = true;
                }
                foreach (var skill in skills)
                {
                    from.Skills[skill].Base = v;
                }

                if (pl.RawStr < 60)
                    pl.RawStr = 60;
                if (pl.RawDex < 60)
                    pl.RawDex = 60;
                if (pl.RawInt < 60)
                    pl.RawInt = 60;

                Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
                Effects.PlaySound(from.Location, from.Map, 0x243);

                Effects.SendMovingParticles(new Entity(0, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                Effects.SendMovingParticles(new Entity(0, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                Effects.SendMovingParticles(new Entity(0, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);
                from.SendMessage("Voce sente o poder dentro de voce. Voce nao e mais um novato.");
            }
        }
    }
}
