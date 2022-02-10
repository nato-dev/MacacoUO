using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ore elemental corpse")]
    public class VeriteElemental : BaseCreature
    {
        [Constructable]
        public VeriteElemental()
            : this(2)
        {
        }

        [Constructable]
        public VeriteElemental(int oreAmount)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "elemental de lazurita";
            Body = 113;
            BaseSoundID = 268;

            SetStr(226, 255);
            SetDex(126, 145);
            SetInt(71, 92);

            SetHits(336, 453);

            SetDamage(9, 16);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 50.1, 95.0);
            SetSkill(SkillName.Tactics, 60.1, 100.0);
            SetSkill(SkillName.Wrestling, 60.1, 100.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 35;

            Item ore = new LazuritaOre(oreAmount);
            ore.ItemID = 0x19B9;
            Hue = ore.Hue;
            PackItem(ore);
        }

        public VeriteElemental(Serial serial)
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

        public static void OnHit(Mobile defender, Item item, int damage)
        {
            IWearableDurability dur = item as IWearableDurability;

            if (dur == null || dur.MaxHitPoints == 0 || item.LootType == LootType.Blessed || item.Insured)
            {
                return;
            }

            if (damage < 10)
                damage = 10;

            if (dur.HitPoints > 0)
            {
                dur.HitPoints = Math.Max(0, dur.HitPoints - damage);
            }
            else
            {
                defender.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.
                dur.MaxHitPoints = Math.Max(0, dur.MaxHitPoints - damage);

                if (!item.Deleted && dur.MaxHitPoints == 0)
                {
                    item.Delete();
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV4);
            AddLoot(LootPack.Gems, 2);
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
