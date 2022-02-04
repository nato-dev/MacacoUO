using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a druid's corpse")]
    public class BearformDruid : BaseCreature
    {
        [Constructable]
        public BearformDruid()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "druidurso";
            Body = 212;
            BaseSoundID = 0xA3;

            SetStr(700, 750);
            SetDex(140, 145);
            SetInt(1800, 2000);

            SetHits(200, 300);
            SetMana(1000, 1200);

            SetDamage(15, 35);

            VirtualArmor = 200;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 90.0, 95.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 95.0);

            SetSkill(SkillName.MagicResist, 0, 0);

            SetSkill(SkillName.Wrestling, 90, 90);

            Fame = 24000;
            Karma = -24000;

            if (Utility.Random(5) == 1)
                this.AddLoot(LootPack.MedScrolls);
            else if (Utility.Random(5) == 1)
                this.AddLoot(LootPack.HighScrolls);
            else
                this.AddLoot(LootPack.LowScrolls);

            if (0.05 > Utility.RandomDouble())
            {
                var mask = new MagicalBearMask();
                mask.Hue = TintaPreta.COR;
                PackItem(mask);
            }

        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Meager);
            this.AddLoot(LootPack.Rich);
          
        }

        public override bool CanRummageCorpses { get { return true; } }

        public override int Meat { get { return 1; } }

        public BearformDruid(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
