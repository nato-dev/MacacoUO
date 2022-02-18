using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a gargoyle corpse")]
    public class StoneGargoyle : BaseCreature
    {
        [Constructable]
        public StoneGargoyle()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "gargula de pedra";
            this.Body = 67;
            this.BaseSoundID = 0x174;

            this.SetStr(246, 275);
            this.SetDex(76, 95);
            this.SetInt(81, 105);

            this.SetHits(2148, 2165);

            this.SetDamage(21, 37);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 85.1, 100.0);
            this.SetSkill(SkillName.Tactics, 80.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 100.0, 100.0);

            this.Fame = 14000;
            this.Karma = -14000;

            this.VirtualArmor = 50;

            this.PackItem(new IronIngot(12));

            if (0.05 > Utility.RandomDouble())
                this.PackItem(new GargoylesPickaxe());
        }

        public StoneGargoyle(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV4, 2);
            this.AddLoot(LootPack.Gems, 1);
            this.AddLoot(LootPack.Potions);
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
