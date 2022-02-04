using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpo de uma formiga fantasma")]
    public class SolenFantasma : BaseCreature, IBlackSolen
    {
        private bool m_BurstSac;
        private Timer m_SoundTimer;

        public virtual void SendTrackingSound()
        {
            if (Hidden)
            {
                Effects.PlaySound(Location, Map, 0x2C8);
                Combatant = null;
            }
            else
            {
                Frozen = false;

                if (m_SoundTimer != null)
                    m_SoundTimer.Stop();

                m_SoundTimer = null;
            }
        }

        [Constructable]
        public SolenFantasma()
            : base(AIType.AI_Ninja, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "formiga fantasma";
            this.Body = 806;
            this.BaseSoundID = 959;
            this.Hue = 0x497;

            this.SetStr(196, 220);
            this.SetDex(151, 175);
            this.SetInt(70, 100);

            this.SetHits(200, 400);

            this.SetDamage(5, 15);

            this.SetDamageType(ResistanceType.Physical, 80);
            this.SetDamageType(ResistanceType.Poison, 20);

            this.SetResistance(ResistanceType.Physical, 20, 35);
            this.SetResistance(ResistanceType.Fire, 20, 35);
            this.SetResistance(ResistanceType.Cold, 10, 25);
            this.SetResistance(ResistanceType.Poison, 20, 35);
            this.SetResistance(ResistanceType.Energy, 10, 25);

            this.SetSkill(SkillName.Tactics, 80.0);
            this.SetSkill(SkillName.Hiding, 100.0);
            this.SetSkill(SkillName.Stealth, 100.0);
            this.SetSkill(SkillName.Ninjitsu, 90.0);
            this.SetSkill(SkillName.MagicResist, 85.0);

            this.Fame = 3500;
            this.Karma = -3500;

            this.VirtualArmor = 35;

            SetWeaponAbility(Habilidade.ShadowStrike);

            SolenHelper.PackPicnicBasket(this);

            this.PackItem(new ZoogiFungus((0.05 > Utility.RandomDouble()) ? 13 : 3));

            if (Utility.RandomDouble() < 0.05)
                this.PackItem(new BraceletOfBinding());
        }

        public SolenFantasma(Serial serial)
            : base(serial)
        {
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (attacker.Weapon is BaseRanged)
                BeginAcidBreath();

            base.OnGotMeleeAttack(attacker);
        }

        public override void OnDamagedBySpell(Mobile attacker)
        {
            base.OnDamagedBySpell(attacker);

            BeginAcidBreath();
        }

        #region Acid Breath
        private DateTime m_NextAcidBreath;

        public void BeginAcidBreath()
        {
            PlayerMobile m = Combatant as PlayerMobile;
            // Mobile m = Combatant;

            if (m == null || m.Deleted || !m.Alive || !Alive || m_NextAcidBreath > DateTime.Now || !CanBeHarmful(m))
                return;

            PlaySound(0x118);
            MovingEffect(m, 0x36D4, 1, 0, false, false, 0x3F, 0);

            TimeSpan delay = TimeSpan.FromSeconds(GetDistanceToSqrt(m) / 5.0);
            Timer.DelayCall<Mobile>(delay, new TimerStateCallback<Mobile>(EndAcidBreath), m);

            m_NextAcidBreath = DateTime.Now + TimeSpan.FromSeconds(5);
        }

        public void EndAcidBreath(Mobile m)
        {
            if (m == null || m.Deleted || !m.Alive || !Alive)
                return;

            if (0.2 >= Utility.RandomDouble())
                m.ApplyPoison(this, Poison.Regular);

            AOS.Damage(m, Utility.RandomMinMax(30, 45), 0, 0, 0, 100, 0);
        }
        #endregion

        public bool BurstSac
        {
            get
            {
                return this.m_BurstSac;
            }
        }
        public override int GetAngerSound()
        {
            return 0xB5;
        }

        public override int GetIdleSound()
        {
            return 0xB5;
        }

        public override int GetAttackSound()
        {
            return 0x289;
        }

        public override int GetHurtSound()
        {
            return 0xBC;
        }

        public override int GetDeathSound()
        {
            return 0xE4;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
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

            if (!willKill)
            {
                if (!this.BurstSac)
                {
                    if (this.Hits < 50)
                    {
                        this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A bolsa de acido da formiga se rompeu *");
                        this.m_BurstSac = true;
                    }
                }
                else if (from != null && from != this && this.InRange(from, 1))
                {
                    this.SpillAcid(from, 1);
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public override bool OnBeforeDeath()
        {
            this.SpillAcid(4);

            return base.OnBeforeDeath();
        }

        private bool m_HasTeleportedAway;

        public override void OnThink()
        {
            if (!m_HasTeleportedAway && Hits < (HitsMax / 2))
            {
                Map map = Map;

                if (map != null)
                {
                    // try 10 times to find a teleport spot
                    for (int i = 0; i < 10; ++i)
                    {
                        int x = X + (Utility.RandomMinMax(5, 10) * (Utility.RandomBool() ? 1 : -1));
                        int y = Y + (Utility.RandomMinMax(5, 10) * (Utility.RandomBool() ? 1 : -1));
                        int z = Z;

                        if (!map.CanFit(x, y, z, 16, false, false))
                            continue;

                        Point3D from = Location;
                        Point3D to = new Point3D(x, y, z);

                        if (!InLOS(to))
                            continue;

                        Location = to;
                        ProcessDelta();
                        Hidden = true;
                        Combatant = null;

                        Effects.SendLocationParticles(EffectItem.Create(from, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                        Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                        Effects.PlaySound(to, map, 0x1FE);

                        m_HasTeleportedAway = true;
                        m_SoundTimer = Timer.DelayCall(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(2.5), new TimerCallback(SendTrackingSound));

                        Frozen = true;

                        break;
                    }
                }
            }

            base.OnThink();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(this.m_BurstSac);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        this.m_BurstSac = reader.ReadBool();
                        break;
                    }
            }
        }
    }
}
