using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Mobs
{
    [CorpseName("a dragon corpse")]
    public class FlyingDragon : BaseCreature
    {
        public override bool ReduceSpeedWithDamage { get { return false; } }

        public override double DisturbChance { get { return 0.1; } }

        [Constructable]
        public FlyingDragon() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.05, 0.1)
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


        }

        public override void GenerateLoot()
        {
            //if(Utility.RandomDouble() < 0.05)
            //    PackItem(new EscamaMagica());
            AddLoot(LootPack.LV5);
            AddLoot(LootPack.Gems, 2);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);
            Shard.Debug("M = " + m.Name);
            this.MoveToWorld(new Point3D(Location.X, Location.Y, Location.Z + 10), this.Map);
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

        public FlyingDragon(Serial serial) : base(serial)
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

}
