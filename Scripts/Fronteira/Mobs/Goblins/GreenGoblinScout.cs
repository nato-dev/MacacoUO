#region References
using Server.Targeting;
using Server.Items;
#endregion

namespace Server.Mobiles
{
	[CorpseName("a goblin corpse")]
	public class GreenGoblinScout : BaseCreature
	{
		[Constructable]
		public GreenGoblinScout()
			: base(AIType.AI_OrcScout, FightMode.Closest, 10, 7, 0.15, 0.3)
		{
			Name = "goblin ladino";
			Body = 723;
			BaseSoundID = 0x600;

			SetStr(176, 209);
			SetDex(95, 120);
			SetInt(107, 146);

			SetHits(80, 110);
			SetMana(107, 146);
			SetStam(165, 179);

			SetDamage(3, 7);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 41, 49);
			SetResistance(ResistanceType.Fire, 33, 39);
			SetResistance(ResistanceType.Cold, 26, 33);
			SetResistance(ResistanceType.Poison, 14, 20);
			SetResistance(ResistanceType.Energy, 11, 20);

			SetSkill(SkillName.MagicResist, 90.7, 98.8);
			SetSkill(SkillName.Tactics, 80.9, 86.3);
			SetSkill(SkillName.Wrestling, 107.7, 119.5);
			SetSkill(SkillName.Anatomy, 80.3, 88.2);
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 100.1, 120.0);

            Fame = 1500;
			Karma = -1500;

            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
		}

		public GreenGoblinScout(Serial serial)
			: base(serial)
		{ }

        public override bool CanStealth { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();
            if (!this.Hidden && this.Combatant == null)
            {
                this.Hidden = true;
                this.IsStealthing = true;
                this.AllowedStealthSteps = 999;
            }
        }

        public override void OnAfterSpawn()
        {
            base.OnAfterSpawn();
            this.Hidden = true;
            this.IsStealthing = true;
            this.AllowedStealthSteps = 999;
        }

        public override int GetAngerSound() { return 0x600; }
        public override int GetIdleSound() { return 0x600; }
        public override int GetAttackSound() { return 0x5FD; }
        public override int GetHurtSound() { return 0x5FF; }
        public override int GetDeathSound() { return 0x5FE; }
        public override Poison HitPoison
        {
            get
            {
                return Poison.Greater;
            }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 1; } }
        public override int Meat { get { return 1; } }
        public override TribeType Tribe { get { return TribeType.GoblinVerde; } }

		public override void GenerateLoot()
		{
			AddLoot(LootPack.LV3);
		}

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.01)
                c.DropItem(new LuckyCoin());
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		private Mobile FindTarget()
		{
            IPooledEnumerable eable = GetMobilesInRange(10);
			foreach (Mobile m in eable)
			{
				if (m.Player && m.Hidden && m.IsPlayer())
				{
                    eable.Free();
					return m;
				}
			}

            eable.Free();
			return null;
		}

		private void TryToDetectHidden()
		{
			Mobile m = FindTarget();

			if (m != null)
			{
				if (Core.TickCount >= NextSkillTime && UseSkill(SkillName.DetectHidden))
				{
					Target targ = Target;

					if (targ != null)
					{
						targ.Invoke(this, this);
					}

					Effects.PlaySound(Location, Map, 0x340);
				}
			}
		}
	}
}
