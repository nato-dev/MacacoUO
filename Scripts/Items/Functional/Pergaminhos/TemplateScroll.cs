using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Items.Functional.Pergaminhos
{
    public class TemplateDeed : Item
    {
        public static SkillName [] WarPvM()
        {
            return new SkillName[]
            {
                SkillName.Macing, SkillName.Parry, SkillName.Anatomy,
                SkillName.Healing, SkillName.Tactics, SkillName.MagicResist,
                SkillName.Magery
            };
        }

        public static SkillName[] WarPvP()
        {
            return new SkillName[]
            {
                SkillName.Fencing, SkillName.Focus, SkillName.Anatomy,
                SkillName.Healing, SkillName.Tactics, SkillName.MagicResist,
                SkillName.Magery
            };
        }

        public static SkillName[] MagePvM()
        {
            return new SkillName[]
            {
                SkillName.Wrestling, SkillName.Magery, SkillName.EvalInt,
                SkillName.SpiritSpeak, SkillName.Inscribe, SkillName.MagicResist,
                SkillName.Meditation
            };
        }

        public static SkillName[] MagePvP()
        {
            return new SkillName[]
            {
                SkillName.Macing, SkillName.Magery, SkillName.EvalInt,
                SkillName.Tactics, SkillName.Inscribe, SkillName.MagicResist,
                SkillName.Meditation
            };
        }

        public static SkillName[] TankPvM()
        {
            return new SkillName[]
            {
                SkillName.Macing, SkillName.Magery, SkillName.EvalInt,
                SkillName.Tactics, SkillName.Anatomy, SkillName.MagicResist,
                SkillName.Healing
            };
        }

        public static SkillName[] TankPvP()
        {
            return new SkillName[]
            {
                SkillName.Fencing, SkillName.Magery, SkillName.EvalInt,
                SkillName.Tactics, SkillName.Anatomy, SkillName.MagicResist,
                SkillName.Healing
            };
        }

        public static SkillName[] ArqueiroPvP()
        {
            return new SkillName[]
            {
                SkillName.Archery, SkillName.Magery, SkillName.EvalInt,
                SkillName.Tactics, SkillName.Anatomy, SkillName.MagicResist,
                SkillName.Healing
            };
        }

        public static SkillName[] ArqueiroPvM()
        {
            return new SkillName[]
            {
                SkillName.Archery, SkillName.Musicianship, SkillName.Peacemaking,
                SkillName.Tactics, SkillName.Anatomy, SkillName.Provocation,
                SkillName.Healing
            };
        }

        [Constructable]
        public TemplateDeed()
            : base(0x14F0)
        {
            this.Hue = 54;
            this.Name = "Pergaminho de Template de Novatos";
        }

        public TemplateDeed(Serial serial)
            : base(serial)
        {
          
        }

        public override void OnDoubleClick(Mobile from)
        {
            var pl = from as PlayerMobile;
            if (pl == null)
                return;

            if(!pl.Young)
            {
                pl.SendMessage("Voce precisa ser um novato para usar isto");
                return;
            }

            if(pl.Templates.Templates.Count >= TemplatesGump.max_templates)
            {
                pl.SendMessage("Voce ja e muito experiente para isto");
                return;
            }

            if(pl.Profession==0)
            {
                pl.SendMessage("Escolha sua template inicial na quest inicial da fada antes");
                return;
            }

            pl.SendGump(new FreeTemplateGump(WarPvM(), 90));
        }


        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Novatos podem usar isto");
            list.Add("Template free com skills em 90");
            list.Add("De a algum amigo iniciante");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
