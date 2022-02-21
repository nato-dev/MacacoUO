using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an unfrozen mummy corpse")]
    public class UnfrozenMummy : BaseCreature
    {
        [Constructable]
        public UnfrozenMummy()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            this.Name = "mumia congelada";
            this.Body = 0x9B;
            this.Hue = 0x480;
            this.BaseSoundID = 0x1D7;

            this.SetStr(450, 500);
            this.SetDex(20, 30);
            this.SetInt(800, 850);

            this.SetHits(1500);

            this.SetDamage(35, 50);

            this.SetDamageType(ResistanceType.Cold, 100);

            this.SetResistance(ResistanceType.Physical, 35, 40);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 60, 80);
            this.SetResistance(ResistanceType.Poison, 20, 30);

            this.SetSkill(SkillName.Wrestling, 120, 120);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.MagicResist, 0);
            this.SetSkill(SkillName.Magery, 50.0, 60.0);
            this.SetSkill(SkillName.EvalInt, 50.0, 60.0);
            this.SetSkill(SkillName.Meditation, 80.0);

            this.Fame = 55000;
            this.Karma = -55000;

            AddItem(new EnhancedBandage(2));
            SetWeaponAbility(WeaponAbility.ArmorIgnore);
          
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            if(defender is BaseCreature && Utility.RandomDouble() < 0.1)
            {
                var bc = defender as BaseCreature;
                bc.Paralyze(TimeSpan.FromSeconds(10));
                bc.PlaySound(0x204);
                bc.FixedEffect(0x376A, 6, 1);
                bc.OverheadMessage("* paralizado *");
                if (bc.ControlMaster != null)
                    this.Combatant = bc.ControlMaster;
            }
        }

        public override void OnDeath( Container c )
        {
            base.OnDeath( c );

            if ( Utility.RandomDouble() < 0.6 )
            c.DropItem( new BrokenCrystals() );

            if ( Utility.RandomDouble() < 0.1 )
            c.DropItem( new ParrotItem() );
        }

        public UnfrozenMummy(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems, 5);
            this.AddLoot(LootPack.LV6, 1);
            AddLoot( LootPack.Parrot );
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.MedScrolls);
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
