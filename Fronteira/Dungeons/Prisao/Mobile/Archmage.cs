using System;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles 
{ 
    [CorpseName("an archmage corpse")] 
    public class Archmage : BaseCreature 
    {
        public override double DisturbChance { get { return 0.6; } }
        public override bool IsSmart { get { return true; } }
        [Constructable] 
        public Archmage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = NameList.RandomName("evil mage");
            this.Title = "o Arquimago";
            this.Body = Utility.RandomList(125, 126);

            if(Utility.RandomDouble() < 0.1)
            {
                var robe = new Robe();
                robe.Name = "Sobretudo do Arquimago";
                robe.Hue = Loot.RandomRareDye();
                PackItem(robe);
            } else
            {
                PackItem(new Robe(Utility.RandomMetalHue()));
                PackItem(new Gold(300));
            }

            SetStr(85, 90);
            SetDex(194, 203);
            SetInt(237, 241);

            SetHits(800, 1000);
            SetMana(578, 616);

            SetDamage(3, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 65, 68);
            SetResistance(ResistanceType.Fire, 65, 66);
            SetResistance(ResistanceType.Cold, 62, 69);
            SetResistance(ResistanceType.Poison, 62, 67);
            SetResistance(ResistanceType.Energy, 64, 68);

            SetSkill(SkillName.EvalInt, 120, 120);
            SetSkill(SkillName.Magery, 120, 130);
            SetSkill(SkillName.Meditation, 89.9, 90.7);
            SetSkill(SkillName.MagicResist, 87.2, 88.7);
            SetSkill(SkillName.Tactics, 78.2, 79.9);
            SetSkill(SkillName.Wrestling, 84.8, 92.6);

            Fame = 14500;
            Karma = -14500;

            VirtualArmor = 16;
			switch (Utility.Random(8))
            {
                case 0: PackItem(new BloodOathScroll()); break;
                case 1: PackItem(new CurseWeaponScroll()); break;
                case 2: PackItem(new StrangleScroll()); break;
                case 3: PackItem(new LichFormScroll()); break;
			}
            PackReg(23);
            PackItem(new Sandals());

            if (Utility.RandomDouble() < 0.1)
            {
                PackItem(new SpellbookDyeTub());
            }

            if (Utility.RandomDouble() < 0.3)
            {
                PackItem(new PianoAddonDeed());
            }
 
            if (Utility.RandomDouble() < 0.75)
            {
                PackItem(new SeveredHumanEars());
            } else
            {
                PackItem(DefJewelcrafting.GetRandomReceitaNoob());
            }
        }

        public Archmage(Serial serial)
            : base(serial)
        { 
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int TreasureMapLevel { get { return Core.AOS ? 2 : 0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.MedScrolls, 2);
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
