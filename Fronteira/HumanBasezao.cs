using System;
using Server.Items;

namespace Server.Mobiles
{
    public class HumanBase : BaseCreature
    {
        [Constructable]
        public HumanBase()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.SpeechHue = Utility.RandomDyedHue();

            this.Hue = Utility.RandomSkinHue();

            this.Body = 0x190;
            this.Name = NameList.RandomName("male");
            this.AddItem(new ShortPants(Utility.RandomRedHue()));

            this.SetStr(386, 400);
            this.SetDex(151, 165);
            this.SetInt(161, 175);

            this.SetDamage(8, 10);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 20, 25);
            this.SetResistance(ResistanceType.Fire, 20, 22);
            this.SetResistance(ResistanceType.Cold, 20, 22);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 10, 20);

            this.SetSkill(SkillName.Parry, 25);
            this.SetSkill(SkillName.Anatomy, 30.0);
            this.SetSkill(SkillName.Fencing, 50.0, 60.5);
            this.SetSkill(SkillName.Macing, 35.0, 57.5);
            this.SetSkill(SkillName.Poisoning, 20.0, 32.5);
            this.SetSkill(SkillName.MagicResist, 83.5, 92.5);
            this.SetSkill(SkillName.Swords, 50.0);
            this.SetSkill(SkillName.Tactics, 55.0);

            this.Fame = 1000;
            this.Karma = -1000;

            this.VirtualArmor = 40;

            Utility.AssignRandomHair(this);

            if (Utility.RandomDouble() < 0.05)
                this.AddItem(Decos.RandomDeco());

            this.SetWeaponAbility(Habilidade.BleedAttack);
            this.SetWeaponAbility(Habilidade.ParalyzingBlow);
        }

        public HumanBase(Serial serial)
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
        public override void GenerateLoot()
        {
            this.AddPackedLoot(LootPack.MeagerProvisions, typeof(Bag));
            this.AddLoot(LootPack.Meager);
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
