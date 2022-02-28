using Server.Items;
using System;

namespace Server.Mobiles
{
    [CorpseName("a titans corpse")]
    public class Titan : BaseCreature
    {
        [Constructable]
        public Titan()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "titan";
            this.Body = 76;
            this.BaseSoundID = 609;

            this.SetStr(536, 585);
            this.SetDex(126, 145);
            this.SetInt(281, 305);

            this.SetHits(1522, 1851);

            this.SetDamage(23, 36);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.EvalInt, 85.1, 100.0);
            this.SetSkill(SkillName.Magery, 85.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 120, 120);
            this.SetSkill(SkillName.Tactics, 120, 120);
            this.SetSkill(SkillName.Wrestling, 120, 120);
            this.SetSkill(SkillName.Parry, 80, 80);

            this.Fame = 31500;
            this.Karma = -31500;

            this.VirtualArmor = 40;

            if (Utility.RandomDouble() < .33)
                this.PackItem(Engines.Plants.Seed.RandomPeculiarSeed(1));

            if (0.1 > Utility.RandomDouble())
                this.PackItem(new Server.Items.RoastPig());

            if (Utility.RandomDouble() < 0.35)
                AddItem(new T2ARecallRune());
        }

        public Titan(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 4;
            }
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Regular;
            }
        }

        public void JogaPedra(Mobile target, bool cd)
        {
            if (!IsCooldown("pedra"))
            {
                SetCooldown("pedra", TimeSpan.FromSeconds(5));
                new Ettin.PedraTimer(this, target).Start();
                PublicOverheadMessage(Network.MessageType.Emote, 0, false, "* nervoso com a magia *");
            }
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);

            JogaPedra(from, true);
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 5;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV6);
            this.AddLoot(LootPack.LV5);
            this.AddLoot(LootPack.MedScrolls);
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
