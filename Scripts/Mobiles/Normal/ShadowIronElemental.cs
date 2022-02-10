using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ore elemental corpse")]
    public class ShadowIronElemental : BaseCreature
    {
        [Constructable]
        public ShadowIronElemental()
            : this(25)
        {
        }

        [Constructable]
        public ShadowIronElemental(int oreAmount)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental de vibranium";
            this.Body = 111;
            this.BaseSoundID = 268;

            this.SetStr(226, 255);
            this.SetDex(126, 145);
            this.SetInt(71, 92);

            this.SetHits(336, 453);

            this.SetDamage(9, 16);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 50.1, 95.0);
            this.SetSkill(SkillName.Tactics, 60.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 100.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 23;

            Item ore = new VibraniumOre(oreAmount);
            ore.ItemID = 0x19B9;
            Hue = ore.Hue;
            this.PackItem(ore);
        }

        public ShadowIronElemental(Serial serial)
            : base(serial)
        {
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
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override bool BreathImmune
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV3);
            this.AddLoot(LootPack.Gems, 2);
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

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)from;

                if (bc.Controlled || bc.BardTarget == this)
                    damage = 0; // Immune to pets and provoked creatures
            }
        }

        public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
            if (caster is BaseCreature && ((BaseCreature)caster).GetMaster() is PlayerMobile)
            {
                scalar = 0.0; // Immune to magic
            }
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage, ElementoPvM e)
        {
            if (from is BaseCreature && ((BaseCreature)from).GetMaster() is PlayerMobile)
            {
                damage = 0;
            }
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

    [CorpseName("an ore elemental corpse")]
    public class AdamantiumElemental : BaseCreature
    {
        [Constructable]
        public AdamantiumElemental()
            : this(25)
        {
        }

        [Constructable]
        public AdamantiumElemental(int oreAmount)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental de adamantium";
            this.Body = 111;
            this.BaseSoundID = 268;

            this.SetStr(226, 255);
            this.SetDex(126, 145);
            this.SetInt(71, 92);

            this.SetHits(136, 153);

            this.SetDamage(9, 16);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 50.1, 95.0);
            this.SetSkill(SkillName.Tactics, 60.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 100.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 23;

            Item ore = new AdamantiumOre(oreAmount);
            ore.ItemID = 0x19B9;
            Hue = ore.Hue;
            this.PackItem(ore);
        }

        public AdamantiumElemental(Serial serial)
            : base(serial)
        {
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
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override bool BreathImmune
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV3);
            this.AddLoot(LootPack.Gems, 2);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)from;

                if (bc.Controlled || bc.BardTarget == this)
                    damage = 0; // Immune to pets and provoked creatures
            }
        }

        public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
            if (caster is BaseCreature && ((BaseCreature)caster).GetMaster() is PlayerMobile)
            {
                scalar = 0.0; // Immune to magic
            }
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage, ElementoPvM e)
        {
            if (from is BaseCreature && ((BaseCreature)from).GetMaster() is PlayerMobile)
            {
                damage = 0;
            }
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
