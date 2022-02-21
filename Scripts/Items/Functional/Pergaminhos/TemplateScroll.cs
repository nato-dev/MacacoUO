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
                SkillName.Swords, SkillName.Lumberjacking, SkillName.Anatomy,
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
                SkillName.Wrestling, SkillName.Magery, SkillName.EvalInt,
                SkillName.Focus, SkillName.Inscribe, SkillName.MagicResist,
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
            : this(0x14F0)
        {
            this.Hue = 54;
            this.Name = "Pergaminho de Template";
        }

        public TemplateDeed(int itemID)
           : base(itemID)
        {
            this.Hue = 54;
            this.Name = "Pergaminho de Double Gold";
        }

        public TemplateDeed(Serial serial)
            : base(serial)
        {
            this.Hue = 356;
            this.Name = "Pergaminho de Double Gold";
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Value { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public SkillName [] Skills { get; set; }

        public override void OnDoubleClick(Mobile from)
        {
            
        }


        public override void AddNameProperties(ObjectPropertyList list)
        {
            //list.Add("Ativa Double Gold por 1h");
            //list.Add("Para o shard inteiro");
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
