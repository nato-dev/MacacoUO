using Server.Items;
using Server.Items.Crops;
using Server.Items.Functional.Pergaminhos;
using System;

namespace Server.Mobiles
{
    [CorpseName("a dragon corpse")]
    public class AncientWyrm : BaseCreature
    {

        public override bool IsBoss => true;

        [Constructable]
        public AncientWyrm()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an ancient wyrm";
            this.Body = 46;
            this.BaseSoundID = 362;

            this.SetStr(1096, 1185);
            this.SetDex(86, 175);
            this.SetInt(686, 775);

            this.SetHits(15000, 15000);

            this.SetDamage(29, 45);

            this.SetDamageType(ResistanceType.Physical, 75);
            this.SetDamageType(ResistanceType.Fire, 25);

            this.SetResistance(ResistanceType.Physical, 65, 75);
            this.SetResistance(ResistanceType.Fire, 80, 90);
            this.SetResistance(ResistanceType.Cold, 70, 80);
            this.SetResistance(ResistanceType.Poison, 60, 70);
            this.SetResistance(ResistanceType.Energy, 60, 70);

            this.SetSkill(SkillName.EvalInt, 80.1, 100.0);
            this.SetSkill(SkillName.Magery, 80.1, 100.0);
            this.SetSkill(SkillName.Meditation, 52.5, 75.0);
            this.SetSkill(SkillName.MagicResist, 100.5, 150.0);
            this.SetSkill(SkillName.Tactics, 97.6, 100.0);
            this.SetSkill(SkillName.Wrestling, 120, 120);
            this.SetSkill(SkillName.Parry, 120, 120);

            this.Fame = 22500;
            this.Karma = -22500;

            this.VirtualArmor = 150;
        }

        public AncientWyrm(Serial serial)
            : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            SorteiaItem(new TemplateDeed());
            SorteiaItem(new DragonWolfCostume());
            SorteiaItem(new SnapdragonSeed(3));
            if (Utility.RandomBool())
            {
                SorteiaItem(new DraconicOrb());
            }
            SorteiaItem(Carnage.GetRandomPS(105));
            var arma = Loot.RandomWeapon();
            arma.WeaponAttributes.HitColdArea = 15;
            if (arma.Name != null)
                arma.Name += " de fogo de dragao";
        }

        public override bool ReacquireOnMovement
        {
            get
            {
                return true;
            }
        }
        public override bool HasBreath
        {
            get
            {
                return true;
            }
        }// fire breath enabled
        public override bool AutoDispel
        {
            get
            {
                return true;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Barbed;
            }
        }
        public override int Hides
        {
            get
            {
                return 40;
            }
        }
        public override int Meat
        {
            get
            {
                return 19;
            }
        }

        public override int Scales
        {
            get
            {
                return 12;
            }
        }

        public override ScaleType ScaleType
        {
            get
            {
                return (ScaleType)Utility.Random(4);
            }
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Regular;
            }
        }
        public override Poison HitPoison
        {
            get
            {
                return Utility.RandomBool() ? Poison.Lesser : Poison.Regular;
            }
        }

        public override double TreasureMapChance => 1;

        public override int TreasureMapLevel
        {
            get
            {
                return 5;
            }
        }
        public override bool CanFly
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV5, 3);
            this.AddLoot(LootPack.Gems, 5);
        }

        public override int GetIdleSound()
        {
            return 0x2D3;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
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
