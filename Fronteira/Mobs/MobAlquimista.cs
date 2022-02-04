using System;
using Server.Engines.Craft;
using Server.Items;
using Server.Network;
using Server.Services;

namespace Server.Mobiles
{
    [CorpseName("a goblin corpse")]
    public class MobAlquimista : BaseCreature
    {
        [Constructable]
        public MobAlquimista()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "alquimista doido";
            Body = 0x191;

            SetStr(282, 331);
            SetDex(62, 79);
            SetInt(100, 149);

            SetHits(163, 197);
            SetStam(62, 79);
            SetMana(100, 149);

            SetDamage(5, 7);

            SetSkill(SkillName.Wrestling, 94.8, 106.9);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 28;
        }

        public MobAlquimista(Serial serial)
            : base(serial)
        {
        }

        public class PotTimer : Timer
        {
            private BaseCreature m_Defender;
            private Mobile m_Target;
            private int count = 3;

            public PotTimer(BaseCreature defender, Mobile target)
                : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 4)
            {
                m_Defender = defender;
                m_Target = target;
                Start();
            }

            protected override void OnTick()
            {
                if (m_Defender == null || m_Target == null || !m_Defender.Alive || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                m_Defender.PublicOverheadMessage(MessageType.Regular, 23, true, "" + count);
                count--;
                if (count < 0)
                {
                    Stop();
                    var explo = Utility.RandomBool();
                    var distancia = m_Defender.GetDistance(m_Target.Location);
                    if (m_Defender.Paralyzed || !m_Defender.InLOS(m_Target) || distancia > 16)
                    {
                        Effects.PlaySound(m_Defender.Location, m_Defender.Map, 0x307);
                        Effects.SendLocationEffect(m_Defender.Location, m_Defender.Map, 0x36B0, 9, 10, 0, 0);
                        var dmg = explo ? 20 : 5 + Utility.Random(explo ? 20 : 5);
                        m_Defender.Damage(dmg);
                        DamageNumbers.ShowDamage(dmg, m_Defender, m_Defender, explo ? 42 : 68);
                    }
                    else
                    {
                        Effects.SendMovingEffect(m_Defender, m_Target, explo ? 0xF0D : 0xF0A, 12, 0, false, false, 0, 0);
                        Timer.DelayCall(TimeSpan.FromMilliseconds(500), () =>
                        {
                            var dmg = explo ? 35 : 5 + Utility.Random(explo ? 35 : 10);
                            m_Target.Damage(dmg);
                            DamageNumbers.ShowDamage(dmg, m_Defender, m_Target, explo ? 32 : 68);
                            Effects.PlaySound(m_Target.Location, m_Target.Map, 0x307);
                            Effects.SendLocationEffect(m_Target.Location, m_Target.Map, 0x36B0, 9, 10, explo ? 0 : 68, 0);
                            if (!explo)
                                m_Target.Poison = Poison.Greater;
                        });
                    }

                }
            }
        }

        public override void OnThink()
        {
            if (this.Combatant != null && this.Combatant is Mobile)
            {
                if (!IsCooldown("pot"))
                {
                    var rnd = Utility.Random(3);
                    OverheadMessage("* abre uma pot *");
                    SetCooldown("pot", TimeSpan.FromSeconds(10));
                    new PotTimer(this, (Mobile)this.Combatant);
                }
            }
            base.OnThink();
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 1; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
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
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
