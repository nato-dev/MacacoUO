using System;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a skeletal corpse")]
    public class SkeletalMage : BaseCreature
    {
        [Constructable]
        public SkeletalMage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "bruxo esqueleto";
            this.Body = Utility.RandomList(50, 56);
            this.Hue = 111;
            BaseSoundID = 451;

            SetStr(76, 100);
            SetDex(56, 75);
            SetInt(186, 210);

            SetHits(20, 40);

            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.EvalInt, 60.1, 70.0);
            SetSkill(SkillName.Magery, 60, 90);
            SetSkill(SkillName.MagicResist, 55.1, 70.0);
            SetSkill(SkillName.Tactics, 45.1, 60.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 6;
         
            PackItem(new BlankScroll(Utility.Random(3, 8)));
            PackItem(new Bone());
            PackItem(new MandrakeRoot(Utility.Random(5, 10)));
            PackItem(new SulfurousAsh(Utility.Random(5, 10)));
        }

        public SkeletalMage(Serial serial)
            : base(serial)
        {
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if(IsCooldown("suga"))
            {
                return;
            }
            SetCooldown("suga", TimeSpan.FromSeconds(6));
            this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            this.PlaySound(0x202);

            defender.MovingEffect(this, 0x374A, 18, 1, false, false);
            defender.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
            defender.PlaySound(0x1F8);
            var suga = Utility.Random(8, 18);
            AOS.Damage(defender, suga, DamageType.Spell, this);
            defender.SendMessage("O esqueleto suga suas forcas vitais");
            this.Heal(suga);
        }

        public override bool BleedImmune { get { return true; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override TribeType Tribe { get { return TribeType.MortoVivo; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV3);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
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
