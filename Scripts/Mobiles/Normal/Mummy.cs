using System;
using Server.Items;
using Server.Services;

namespace Server.Mobiles
{
    [CorpseName("a mummy corpse")]
    public class Mummy : BaseCreature
    {
        public double BandHealPct = 0.15;

        [Constructable]
        public Mummy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            this.Name = "mumia";
            Shard.Debug("m1", this);
            this.Body = 154;
            this.BaseSoundID = 471;
          
            this.SetStr(30, 60);
            this.SetDex(15, 30);
            this.SetInt(26, 40);

            this.SetHits(40, 45);

            this.SetDamage(2, 4);

            this.SetDamageType(ResistanceType.Physical, 40);
            this.SetDamageType(ResistanceType.Cold, 60);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 10, 20);
            this.SetResistance(ResistanceType.Cold, 50, 60);
            this.SetResistance(ResistanceType.Poison, 20, 30);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.MagicResist, 120.1, 180.0);
            this.SetSkill(SkillName.Tactics, 35.1, 50.0);
            this.SetSkill(SkillName.Wrestling, 35.1, 37.0);

            this.Fame = 2000;
            this.Karma = -2000;

            this.VirtualArmor = 16;

            if (Utility.Chance(1))
                this.PackItem(Engines.Plants.Seed.RandomPeculiarSeed());

            if (Utility.Chance(10))
                this.PackItem(new CottonSeeds());

            if (Utility.RandomBool())
                this.PackItem(new Garlic(5));
            else
                this.PackItem(new Ginseng(5));
            this.PackItem(new Bandage(6));
        }

        public Mummy(Serial serial)
            : base(serial)
        {
        }

        public override int Hides
        {
            get
            {
                return Utility.Random(1, 2);
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Regular;
            }
        }

        public override bool BardImmune
        {
            get
            {
                return true;
            }
        }
        public override bool AllureImmune
        {
            get
            {
                return true;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return true;
            }
        }
        public override bool Uncalmable
        {
            get
            {
                return true;
            }
        }

        public override int BloodHue { get { return 862; } set { } }

        public override void OnThink()
        {
            base.OnThink();
            if (IsCooldown("pegabands"))
                return;

            SetCooldown("pegabands", TimeSpan.FromSeconds(4));
            var items = this.Map.GetItemsInRange(this.Location, 2);
            var anexou = false;
            foreach (var item in items)
            {
                if (item.Movable && item is Bandage)
                {
                    anexou = true;
                    item.Consume();
                }
            }
            items.Free();
            if (anexou)
            {
                OverheadMessage("* anexou bandagem *");

                this.Hits += 20;
                DamageNumbers.ShowDamage(-20, this, this, 0);
                this.PlaySound(0x57);
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (amount > 5 && Utility.RandomBool())
            {
                if(!IsCooldown("pegabands"))
                    SetCooldown("pegabands", TimeSpan.FromSeconds(4));
                var bandage = new Bandage();
                var loc = this.Location;
                var map = this.Map;
                Effects.ItemToFloor(this, bandage, Effects.TryGetNearRandomLoc(this), this.Map);
                bandage.PublicOverheadMessage("* caiu *");

            }
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lesser;
            }
        }

        public override TribeType Tribe { get { return TribeType.MortoVivo; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV1);
            if (Utility.RandomDouble() < 0.02)
                this.AddLoot(LootPack.Gems);
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
