using System;
using Server.Engines.Craft;
using Server.Items;
using Server.Ziden;

namespace Server.Mobiles
{
    [CorpseName("a savage corpse")]
    public class Savage : BaseCreature
    {
        [Constructable]
        public Savage()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = NameList.RandomName("savage");

            if (this.Female = Utility.RandomBool())
                this.Body = 184;
            else
                this.Body = 183;

            this.SetStr(96, 115);
            this.SetDex(86, 105);
            this.SetInt(51, 65);

            this.SetDamage(23, 27);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetSkill(SkillName.Fencing, 60.0, 82.5);
            this.SetSkill(SkillName.Macing, 60.0, 82.5);
            this.SetSkill(SkillName.Poisoning, 60.0, 82.5);
            this.SetSkill(SkillName.MagicResist, 57.5, 80.0);
            this.SetSkill(SkillName.Swords, 60.0, 82.5);
            this.SetSkill(SkillName.Tactics, 60.0, 82.5);

            this.Fame = 1000;
            this.Karma = -1000;

            this.PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

            if (this.Female && 0.1 > Utility.RandomDouble())
                this.PackItem(new TribalBerry());
            else if (!this.Female && 0.1 > Utility.RandomDouble())
                this.PackItem(new BolaBall());

            var arma = new Spear();
            if (0.1 > Utility.RandomDouble())
                arma.Quality = ItemQuality.Exceptional;
            else if (0.1 > Utility.RandomDouble())
                arma.Quality = ItemQuality.Low;

            if (0.1 > Utility.RandomDouble())
                arma.Resource = CraftResource.Cobre;
            arma.HitPoints = arma.MaxHitPoints;
            this.AddItem(arma);
            this.AddItem(new BoneArms());
            this.AddItem(new BoneLegs());

            if (0.01 > Utility.RandomDouble())
                this.AddItem(new OrcishKinMask());
            else
                this.AddItem(new SavageMask());

            if (Utility.RandomDouble() < 0.2)
                AddItem(new TaxidermyKit());

            if (Utility.Random(5) == 1)
            {
                PackItem(DefCookingExp.GetReceitaIgredienteRandom());
            }

            this.Backpack.DropItem(new Bandage(Utility.Random(10) + 5));
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            var bands = this.Backpack.FindItemByType(typeof(Bandage));
            if (!IsCooldown("bands") && bands != null)
            {
                if (!willKill && this.Hits < this.HitsMax)
                {
                    SetCooldown("bands", TimeSpan.FromSeconds(Server.Mobiles.BaseOrc.BANDS_TIME + 12));
                    PublicOverheadMessage(Network.MessageType.Regular, 0, false, "* aplicando bandagens *");
                    bands.Consume(1);
                    this.Backpack.DropItem(new BandVeia());
                    new Server.Mobiles.BaseOrc.HealTimer(this);
                }
            }
            base.OnDamage(amount, from, willKill);
        }

        public Savage(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }

        public override TribeType Tribe { get { return TribeType.Savage; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.SavagesAndOrcs;
            }
        }

        public override bool OnBeforeDeath()
        {
            if(Utility.RandomDouble() > 0.05)
            {
                var mask = this.FindItemByType<SavageMask>();
                if (mask != null)
                    this.RemoveItem(mask);
            }
            return base.OnBeforeDeath();
        }

        public override void GenerateLoot()
        {
            this.AddPackedLoot(LootPack.MeagerProvisions, typeof(Bag));
            this.AddLoot(LootPack.Average, 1+Utility.Random(0, 1));
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.BodyMod == 183 || m.BodyMod == 184)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.BodyMod == 183 || aggressor.BodyMod == 184)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Sua pele queima junto com a tinta tribal"); // Your skin is scorched as the tribal paint burns away!

                if (aggressor is PlayerMobile)
                    ((PlayerMobile)aggressor).SavagePaintExpiration = TimeSpan.Zero;
            }
        }

        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            if (to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Hiryu || to is LesserHiryu || to is Daemon)
                damage *= 3;
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
