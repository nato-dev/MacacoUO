using System;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles
{

    [TypeAlias("Server.Mobiles.HumanBrigand")]
    public class BaseBrigand : BaseCreature
    {
        [Constructable]
        public BaseBrigand(AIType type, FightMode mode, int n1, int n2, double n3, double n4)
            : base(type, mode, n1, n2, n3, n4)
        {
            SetSkill(SkillName.Hiding, 100, 100);
            SetSkill(SkillName.Stealth, 100, 100);
            if (Utility.Random(20) == 1)
            {
                PackItem(Loot.RandomClothing());
            }

            this.PackItem(Loot.RandomFood());
            if(Utility.Random(150)==1)
            {
                PackItem(Decos.RandomDeco());
            }
            if (Utility.Random(10) == 1)
            {
                PackItem(DefCookingExp.GetReceitaMolhoRandom());
            }
        }

        public override bool CanStealth { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();
            if (!this.Hidden && this.Combatant == null)
            {
                this.AllowedStealthSteps = 999;
                this.Hidden = true;
                this.IsStealthing = true;
            }
        }

        public override void OnRevealed()
        {
            base.OnRevealed();
            PublicOverheadMessage(Network.MessageType.Emote, 0, false, "* saiu do esconderijo *");
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();
            this.Hidden = true;
            this.IsStealthing = true;
            this.AllowedStealthSteps = 999;
        }

        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }

        public BaseBrigand(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle
        {
            get
            {
                return false;
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

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.75)
                c.DropItem(new SeveredHumanEars());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [TypeAlias("Server.Mobiles.HumanBrigand")]
    public class Brigand : BaseBrigand
    {

        public Brigand(Serial serial) : base(serial)
        {
            
        }
        [Constructable]
        public Brigand()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();

            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "a Bandoleira";
                AddItem(new Skirt(Utility.RandomNeutralHue()));
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
                Title = "o bandoleiro";
            }

            SetStr(100, 120);
            SetDex(81, 95);
            SetInt(61, 75);

            SetDamage(15, 25);

            SetSkill(SkillName.Hiding, 100, 100);
            SetSkill(SkillName.Stealth, 100, 100);
            SetSkill(SkillName.Fencing, 66.0, 97.5);
            SetSkill(SkillName.Macing, 65.0, 87.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Swords, 65.0, 87.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Wrestling, 15.0, 37.5);

            Fame = 1000;
            Karma = -1000;

            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt());
            AddItem(new Bandana());
        
            this.PackItem(Loot.RandomFood());

            switch (Utility.Random(7))
            {
                case 0:
                    AddItem(new Longsword());
                    break;
                case 1:
                    AddItem(new Cutlass());
                    break;
                case 2:
                    AddItem(new Broadsword());
                    break;
                case 3:
                    AddItem(new Axe());
                    break;
                case 4:
                    AddItem(new DoubleAxe());
                    break;
                case 5:
                    AddItem(new Dagger());
                    break;
                case 6:
                    AddItem(new Spear());
                    break;
            }

            Utility.AssignRandomHair(this);

            var arma = this.Weapon;
            if(arma != null)
            {
                var barma = (BaseWeapon)arma;
                if (Utility.RandomDouble() <= 0.15)
                {
                    barma.Poison = Poison.Lesser;
                    barma.PoisonCharges = 12;
                }
                if (Utility.RandomDouble() <= 0.1)
                {
                    barma.Quality = ItemQuality.Exceptional;
                }
                else if (Utility.RandomDouble() <= 0.1)
                {
                    barma.Quality = ItemQuality.Low;
                }
                if (Utility.RandomDouble() <= 0.1)
                {
                    barma.Resource = CraftResource.Cobre;
                }
                else if (Utility.RandomDouble() <= 0.1)
                {
                    barma.Resource = CraftResource.Bronze;
                }



            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddPackedLoot(LootPack.AverageProvisions, typeof(Backpack));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
