using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ore elemental corpse")]
    public class GoldenElemental : BaseCreature
    {
        [Constructable]
        public GoldenElemental()
            : this(2)
        {
        }

        [Constructable]
        public GoldenElemental(int oreAmount)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental dourado";
            this.Body = 166;
            this.BaseSoundID = 268;

            this.SetStr(226, 255);
            this.SetDex(126, 145);
            this.SetInt(71, 92);

            this.SetHits(336, 353);

            this.SetDamage(9, 16);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 60, 75);
            this.SetResistance(ResistanceType.Fire, 10, 20);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 50.1, 95.0);
            this.SetSkill(SkillName.Tactics, 60.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 100.0);

            this.Fame = 3500;
            this.Karma = -3500;

            this.VirtualArmor = 60;

            Item ore = new SilverOre(oreAmount);
            ore.ItemID = 0x19B9;
            Hue = ore.Hue;
            this.PackItem(ore);
        }

        public GoldenElemental(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            if (this.Combatant is Mobile && !IsCooldown("pula"))
            {
                this.PlayAngerSound();
                OverheadMessage("* enterrando *");
                SetCooldown("pula", TimeSpan.FromSeconds(30));
                new EarthElemental.TerraTimer(this, this.Combatant as Mobile, 0.09).Start();
            }
        }

        public override bool AutoDispel
        {
            get
            {
                return true;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 1;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV3);
            this.AddLoot(LootPack.Gems, 2);
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
