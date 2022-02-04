using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpo de formiga-do-fogo")] // TODO: Corpse name?
    public class SolenFogo : BaseCreature, IBlackSolen
    {
        [Constructable]
        public SolenFogo()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "formiga-do-fogo";
            this.Body = 807;
            this.BaseSoundID = 959;
            this.Hue = 0xAA1;

            this.SetStr(326, 350);
            this.SetDex(141, 165);
            this.SetInt(400, 500);

            this.SetHits(1510, 3000);

            this.SetDamage(10, 15);

            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Poison, 30);

            this.SetResistance(ResistanceType.Physical, 40, 45);
            this.SetResistance(ResistanceType.Fire, 30, 35);
            this.SetResistance(ResistanceType.Cold, 40, 45);
            this.SetResistance(ResistanceType.Poison, 40, 45);
            this.SetResistance(ResistanceType.Energy, 25, 30);

            this.SetSkill(SkillName.MagicResist, 100.0);
            this.SetSkill(SkillName.Tactics, 90.0);
            this.SetSkill(SkillName.Wrestling, 90.0);
            this.SetSkill(SkillName.Magery, 120.0);
            this.SetSkill(SkillName.EvalInt, 120.0);
            this.SetSkill(SkillName.Meditation, 120.0);

            this.Fame = 10500;
            this.Karma = -9500;

            this.VirtualArmor = 50;

            SetWeaponAbility(Habilidade.Disarm);

            SolenHelper.PackPicnicBasket(this);

            this.PackItem(new ZoogiFungus((0.05 > Utility.RandomDouble()) ? 16 : 4));
        }

        public override bool OnBeforeDeath()
        {
            return base.OnBeforeDeath();
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override int BreathFireDamage { get { return 35; } }
        public override int BreathColdDamage { get { return 35; } }
        public override bool AutoDispel { get { return !Controlled; } }

        public SolenFogo(Serial serial)
                : base(serial)
        {
        }

        public override int GetAngerSound()
        {
            return 0x259;
        }

        public override int GetIdleSound()
        {
            return 0x259;
        }

        public override int GetAttackSound()
        {
            return 0x195;
        }

        public override int GetHurtSound()
        {
            return 0x250;
        }

        public override int GetDeathSound()
        {
            return 0x25B;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
        }

        public override bool BardImmune
        {
            get
            {
                return !Core.SE;
            }
        }

        public override bool Unprovokable
        {
            get
            {
                return Core.SE;
            }
        }

        public override bool AreaPeaceImmune
        {
            get
            {
                return Core.SE;
            }
        }

        public override bool IsEnemy(Mobile m)
        {
            if (SolenHelper.CheckBlackFriendship(m))
                return false;
            else
                return base.IsEnemy(m);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            SolenHelper.OnBlackDamage(from);

            base.OnDamage(amount, from, willKill);
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
