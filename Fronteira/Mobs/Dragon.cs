#region References

using Server.Engines.Craft;
using Server.Items;
using Server.Ziden;

#endregion

namespace Server.Mobiles
{
    [CorpseName("a dragon corpse")]
    public class Dragon : BaseCreature
    {
        public override bool ReduceSpeedWithDamage { get { return false; } }

        public override double DisturbChance { get { return 0.1; } }

        [Constructable]
        public Dragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.05, 0.1)
        {
            Body = Utility.RandomList(12, 59);
            Name = "Dragao";
            if (Body == 12)
            {
                int random = Utility.Random(100);
                if (random == 0)
                {
                    Hue = 1153;
                    Name = "Dragao Branco";
                    AddItem(new Gold(20000));
                    AddItem(Carnage.GetRandomPS(110));
                    AddItem(Carnage.GetRandomPS(105));

                }
                else if (random <= 4)
                {
                    SetHits(1500, 2000);
                    Hue = Utility.RandomList(1445, 1436, 2006, 2001);
                }
            }

            BaseSoundID = 362;

            SetStr(796, 825);
            SetDex(86, 105);
            SetInt(436, 475);

            SetHits(1500, 2000);

            SetDamage(16, 27);

            SetSkill(SkillName.EvalInt, 150, 200);
            SetSkill(SkillName.Magery, 80, 80);
            SetSkill(SkillName.MagicResist, 99.1, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 93.2);
            SetSkill(SkillName.Parry, 20, 30);
            SetSpecialAbility(SpecialAbility.DragonBreath);
            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 1000;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 100;

            if (Utility.RandomDouble() < 0.2)
            {
                AddItem(new PetBrandingIron());
            }

            if (Hue == 1153 || Utility.RandomDouble() < 0.2)
            {
                AddItem(DefJewelcrafting.GetReceitaPower());
            }
            else
            {
                AddItem(DefJewelcrafting.GetRandomReceitaNoob());
            }
            if (Utility.Random(10) == 1)
            {
                AddItem(new DecoWyrmsHeart());
            }
            if (Utility.Random(6) == 1)
            {
                var cloth = new UncutCloth(Utility.Random(10, 10));
                cloth.Hue = Loot.RandomRareDye();
                cloth.Name = "Tecido Raro";
                AddItem(cloth);
            }
        }

        public override void GenerateLoot()
        {
            if(Utility.RandomDouble() < 0.05)
                PackItem(new EscamaMagica());
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 20);

            if (0.001 > Utility.RandomDouble()) // 2 percent - multipy number x 100 to get percent
            {
                switch (Utility.Random(2))
                {
                    // rolls and number it gives it a case. if the number is , say , 3 it will pack that item
                    case 0:
                        PackItem(new LeatherDyeTub());
                        break;
                    case 1:
                        PackItem(new DragonHead());
                        break;
                }
            }
        }

        public override bool OnBeforeDeath()
        {
            return base.OnBeforeDeath();
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override int BreathFireDamage { get { return 60; } }
        public override int BreathColdDamage { get { return 60; } }
        public override bool AutoDispel { get { return !Controlled; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override int Meat { get { return Body != 104 ? 19 : 0; } }
        public override int Hides { get { return Body != 104 ? 25 : 0; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override int Scales { get { return Body != 104 ? 7 : 0; } }

        public override ScaleType ScaleType
        {
            get { return Hue == 0 ? (Body == 12 ? ScaleType.Yellow : ScaleType.Red) : ScaleType.Green; }
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }

        public override int BreathEffectHue { get { return Body != 104 ? base.BreathEffectHue : 0x480; } }
        public override int DefaultBloodHue { get { return -2; } } //Template

        public Dragon(Serial serial) : base(serial)
        { }

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

        public override void OnAfterTame(Mobile tamer)
        {
            base.OnAfterTame(tamer);

            if (PetTrainingHelper.Enabled)
            {
                SetHits(500);
                SetInt(100);
                SetStr(250);
            }
        }
    }

    [CorpseName("a dragon corpse")]
    public class YoungDragon : BaseCreature
    {
        [Constructable]
        public YoungDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = Utility.RandomList(60, 61);
            Name = "Dragao Jovem";
         
            BaseSoundID = 362;

            SetStr(500, 500);
            SetDex(86, 105);
            SetInt(436, 475);

            SetHits(350, 450);

            SetDamage(10, 15);

            SetSkill(SkillName.EvalInt, 30.1, 40.0);
            SetSkill(SkillName.Magery, 30.1, 40.0);
            SetSkill(SkillName.MagicResist, 99.1, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 93.2);
            SetSpecialAbility(SpecialAbility.DragonBreath);
            Fame = 15000 / 2;
            Karma = -15000 / 2;

            VirtualArmor = 40;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 90;

            if (Utility.Random(50) == 1)
            {
                AddItem(new DecoWyrmsHeart());
            }

            if (Utility.RandomDouble() < 0.3)
            {
                AddItem(DefJewelcrafting.GetRandomReceitaNoob());
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            if (Utility.RandomDouble() < 0.05)
                PackItem(new EscamaMagica());
        }

        public override bool OnBeforeDeath()
        {
            return base.OnBeforeDeath();
        }

        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override int BreathFireDamage { get { return 40; } }
        public override int BreathColdDamage { get { return 40; } }
        public override bool AutoDispel { get { return !Controlled; } }
        public override int Meat { get { return Body != 104 ? 19 : 0; } }
        public override int Hides { get { return Body != 104 ? 25 : 0; } }
        public override HideType HideType { get { return HideType.Horned; } }
        public override int Scales { get { return Body != 104 ? 7 : 0; } }

        public override ScaleType ScaleType
        {
            get { return Hue == 0 ? (Body == 12 ? ScaleType.Yellow : ScaleType.Red) : ScaleType.Green; }
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }

        public override int BreathEffectHue { get { return Body != 104 ? base.BreathEffectHue : 0x480; } }
        public override int DefaultBloodHue { get { return -2; } } //Template

        public YoungDragon(Serial serial) : base(serial)
        { }

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
