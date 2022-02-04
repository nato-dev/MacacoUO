using System;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles 
{ 
    [CorpseName("an evil mage corpse")] 
    public class EvilMage : BaseCreature 
    {
        public override double DisturbChance { get { return 1; } }

        [Constructable] 
        public EvilMage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        { 
            Name = NameList.RandomName("evil mage");
            Title = "o mago vil";
            Body = 0x191;

            SetStr(81, 105);
            SetDex(91, 115);
            SetInt(96, 120);

            SetHits(70, 90);

            SetDamage(9, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 25.1, 50.0);
            SetSkill(SkillName.Magery, 60.1, 70);
            SetSkill(SkillName.MagicResist, 75.0, 97.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Macing, 50, 80);

            Fame = 2500;
            Karma = -2500;

            AddItem(new BlackStaff());
            var hue = Utility.RandomNeutralHue();
            if (Utility.Random(100) == 1)
                AddItem(new MagicWizardsHat(2));
            else
                AddItem(new WizardsHat(hue));


            VirtualArmor = 25;
            PackReg(25);
            AddItem(new Robe(hue)); // TODO: Proper hue
            AddItem(new Sandals(hue));

            if(Utility.Random(100)==1)
            {
               PackItem(DefAlchemy.GetRandomRecipe());
            }
        }

        public override int GetDeathSound()
        {
            return 0x423;
        }

        public override int GetHurtSound()
        {
            return 0x436;
        }

        public EvilMage(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return Core.AOS ? 1 : 0;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            if(Utility.Random(10)==1)
                AddLoot(LootPack.MedScrolls);
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

    [CorpseName("an evil mage corpse")]
    public class EvilMagePelado : BaseCreature
    {
        public override double DisturbChance { get { return 1; } }

        [Constructable]
        public EvilMagePelado()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("evil mage");
            Title = "o mago vil";
            Body = 0x191;

            SetStr(81, 105);
            SetDex(91, 115);
            SetInt(96, 120);

            SetHits(70, 90);

            SetDamage(9, 20);

            SetDamageType(ResistanceType.Physical, 100);

            Utility.AssignRandomHair(this);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 25;
        }

        public override int GetDeathSound()
        {
            return 0x423;
        }

        public override int GetHurtSound()
        {
            return 0x436;
        }

        public EvilMagePelado(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return Core.AOS ? 1 : 0;
            }
        }
        public override void GenerateLoot()
        {
         
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
