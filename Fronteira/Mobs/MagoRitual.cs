using System;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an evil mage corpse")]
    public class RitualMage : BaseCreature
    {
        [Constructable]
        public RitualMage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Zirokden";
            Title = "o mago ritualistico";
            Body = 0x191;

            SetStr(81, 105);
            SetDex(91, 115);
            SetInt(96, 120);

            SetHits(300, 400);

            SetDamage(9, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 0, 1);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 97.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Macing, 70.2, 80);

            Fame = 4500;
            Karma = -4500;
            var staff = new QuarterStaff();

            staff.Quality = ItemQuality.Exceptional;
            staff.Resource = CraftResource.Carvalho;
            AddItem(staff);
            var hue = Utility.RandomNeutralHue();
            AddItem(new WizardsHat(hue));


            VirtualArmor = 25;
            PackReg(25);
            AddItem(new Robe(hue)); // TODO: Proper hue
            AddItem(new Sandals(hue));

            if (Utility.Random(30) == 1)
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

        public RitualMage(Serial serial)
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

        public override void GenerateLoot(bool spawning)
        {
            if(!spawning)
            {
                var r = new RobeofRite();
                r.PartyLoot = false;

                var cont = Backpack;
                if (!cont.TryDropItem(this, r, false))
                {
                    cont.DropItem(r);
                }
                Backpack.AddItem(r);
                r.PartyLoot = true;
            }

            base.GenerateLoot(spawning);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            if (Utility.Random(10) == 1)
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
}
