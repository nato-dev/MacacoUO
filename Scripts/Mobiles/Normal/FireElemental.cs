using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;

namespace Server.Mobiles
{
    [CorpseName("a fire elemental corpse")]
    public class FireElemental : BaseCreature
    {
        public override Spell ChooseSpell()
        {
            if ((ControlMaster == null && Utility.RandomBool()) || (Utility.RandomDouble() < 0.35))
            {
                return new FlameStrikeSpell(this, null);
            }
            return null;
        }

        [Constructable]
        public FireElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental do fogo";
            this.Body = 15;
            this.BaseSoundID = 838;

            this.SetStr(126, 155);
            this.SetDex(166, 185);
            this.SetInt(101, 125);

            this.SetHits(76, 93);

            this.SetDamage(7, 9);

            this.SetDamageType(ResistanceType.Fire, 100);
            this.SetResistance(ResistanceType.Fire, 100, 100);

            this.SetSkill(SkillName.EvalInt, 60.1, 75.0);
            this.SetSkill(SkillName.Magery, 60.1, 75.0);
            this.SetSkill(SkillName.MagicResist, 75.2, 105.0);
            this.SetSkill(SkillName.Tactics, 80.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 70.1, 100.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 40;
            this.ControlSlots = 4;

            this.PackItem(new SulfurousAsh(10));

            this.AddItem(new LightSource());
        }

        public FireElemental(Serial serial)
            : base(serial)
        {
        }

        public override double DispelDifficulty
        {
            get
            {
                return 117.5;
            }
        }
        public override double DispelFocus
        {
            get
            {
                return 45.0;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV3);
            this.AddLoot(LootPack.LV2);
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

            if (this.BaseSoundID == 274)
                this.BaseSoundID = 838;
        }
    }
}
