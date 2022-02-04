using System;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Mobiles 
{ 
    [CorpseName("an elf corpse")]
    public class ElfBrigand : BaseBrigand
    {
        [Constructable]
        public ElfBrigand()
            : base(AIType.AI_Spellweaving, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Race = Race.Elf;

            if (Female = Utility.RandomBool())
            {
                Body = 606;
                Name = NameList.RandomName("Elf female")+" o elfo anarquista";
            }
            else
            {
                Body = 605;
                Name = NameList.RandomName("Elf male")+" a elfa anarquista";
            }

            Hue = Race.RandomSkinHue();

            SetStr(150, 200);
            SetDex(81, 95);
            SetInt(61, 75);

            SetHits(150, 250);
            SetDamage(10, 23);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Wrestling, 80, 90);
            SetSkill(SkillName.Swords, 80, 90);
            SetSkill(SkillName.Fencing, 80, 90);
            SetSkill(SkillName.Macing, 80, 90);
            SetSkill(SkillName.Spellweaving, 50.0, 75.0);
            SetSkill(SkillName.Magery, 50, 75);
            SetSkill(SkillName.Focus, 50.0, 75.0);

            Fame = 1000;
            Karma = -1000;

            // outfit
            AddItem(new Shirt(Utility.RandomNeutralHue()));

            if (Utility.RandomDouble() < 0.02)
                AddItem(new VillageCauldron());

            switch (Utility.Random(4))
            {
                case 0:
                    AddItem(new Sandals());
                    break;
                case 1:
                    AddItem(new Shoes());
                    break;
                case 2:
                    AddItem(new Boots());
                    break;
                case 3:
                    AddItem(new ThighBoots());
                    break;
            }

            if (Female)
            {
                if (Utility.RandomBool())
                    AddItem(new Skirt(Utility.RandomNeutralHue()));
                else
                    AddItem(new Kilt(Utility.RandomNeutralHue()));
            }
            else
                AddItem(new ShortPants(Utility.RandomNeutralHue()));

            // hair, facial hair			
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // weapon, shield
            Item weapon = Loot.RandomWeapon();

            AddItem(weapon);

            if (weapon.Layer == Layer.OneHanded && Utility.RandomBool())
                AddItem(Loot.RandomShield());

            if(Utility.Random(20)==1)
            {
                PackItem(Decos.RandomDeco());
            }
            if (Utility.Random(3) == 1)
            {
                PackItem(Loot.RandomGem());
            }

            PackGold(50, 150);
        }

        public ElfBrigand(Serial serial)
            : base(serial)
        {
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
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.75)
                c.DropItem(new SeveredElfEars());
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
