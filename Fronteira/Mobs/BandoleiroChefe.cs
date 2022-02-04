using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Mobs.Goblins
{
    [TypeAlias("Server.Mobiles.HumanBrigand")]
    public class BrigandLeader : BaseBrigand
    {

        public BrigandLeader(Serial serial) : base(serial)
        {

        }
        [Constructable]
        public BrigandLeader()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();
            Female = false;
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female") + " a bandoleira lider";
                AddItem(new Skirt(Utility.RandomNeutralHue()));
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male") + "o bandoleiro lider";
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
                Title = "o bandoleiro lider";
            }

            SetStr(200, 220);
            SetDex(81, 95);
            SetInt(61, 75);
            SetHits(500);
            SetDamage(15, 35);

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
                    AddItem(new VikingSword());
                    break;
                case 3:
                    AddItem(new Axe());
                    break;
                case 4:
                    AddItem(new DoubleAxe());
                    break;
                case 5:
                    AddItem(new ShortSpear());
                    break;
                case 6:
                    AddItem(new Spear());
                    break;
            }

            Utility.AssignRandomHair(this);

            var arma = this.Weapon;
            if (arma != null)
            {
                var barma = (BaseWeapon)arma;
                barma.Poison = Poison.Greater;
                barma.PoisonCharges = 15;
                barma.Quality = ItemQuality.Exceptional;
                barma.Resource = CraftResource.Cobre;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddPackedLoot(LootPack.AverageProvisions, typeof(Backpack));
        }

        public override void GenerateLoot(bool spawning)
        {
            if (!spawning)
            {
                var r = new FerramentasDaMamae();
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
