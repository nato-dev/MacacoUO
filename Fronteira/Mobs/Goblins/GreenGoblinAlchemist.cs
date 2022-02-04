using System;
using Server.Engines.Craft;
using Server.Items;
using Server.Network;
using Server.Services;

namespace Server.Mobiles
{
    [CorpseName("a goblin corpse")]
    public class GreenGoblinAlchemist : BaseCreature
    {
        [Constructable]
        public GreenGoblinAlchemist()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "goblin alquimista";
            Body = 723;
            BaseSoundID = 0x600;

            SetStr(282, 331);
            SetDex(62, 79);
            SetInt(100, 149);

            SetHits(163, 197);
            SetStam(62, 79);
            SetMana(100, 149);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 35, 45);
            SetResistance(ResistanceType.Energy, 11, 20);

            SetSkill(SkillName.MagicResist, 120.3, 129.2);
            SetSkill(SkillName.Tactics, 81.9, 87.1);
            SetSkill(SkillName.Anatomy, 0.0, 0.0);
            SetSkill(SkillName.Wrestling, 94.8, 106.9);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 28;

            // loot 60 gold, magic item, gem, bola ball, liquar,gob blood
            switch ( Utility.Random(20) )
            {
                case 0:
                    PackItem(new Scimitar());
                    break;
                case 1:
                    PackItem(new Katana());
                    break;
                case 2:
                    PackItem(new WarMace());
                    break;
                case 3:
                    PackItem(new WarHammer());
                    break;
                case 4:
                    PackItem(new Kryss());
                    break;
                case 5:
                    PackItem(new Pitchfork());
                    break;
            }

            PackItem(new ThighBoots());

            switch ( Utility.Random(3) )
            {
                case 0:
                    PackItem(new Ribs());
                    break;
                case 1:
                    PackItem(new Shaft());
                    break;
                case 2:
                    PackItem(new Candle());
                    break;
            }

            if (0.2 > Utility.RandomDouble())
                PackItem(new BolaBall());

            PackItem(new LesserExplosionPotion());

            if (0.05 > Utility.RandomDouble())
                PackItem(new ScouringToxin());

            PackItem(new Bottle(5));

            if(0.01 > Utility.RandomDouble())
            {
                PackItem(new BasketWeavingBook());
            }

            if(0.05 > Utility.RandomDouble())
            {
                switch(Utility.Random(4))
                {
                    case 0:
                        PackItem(new RecipeScroll((int)AlchemyRecipes.Conflagracao));
                        break;
                    case 1:
                        PackItem(new RecipeScroll((int)AlchemyRecipes.SuperConflagracao));
                        break;
                    case 2:
                        PackItem(new RecipeScroll((int)AlchemyRecipes.Expl));
                        break;
                    case 3:
                        PackItem(new RecipeScroll((int)AlchemyRecipes.Expl2));
                        break;
                }
            }
        }

        public GreenGoblinAlchemist(Serial serial)
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
                if(m_Defender == null || m_Target==null || !m_Defender.Alive || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                m_Defender.PublicOverheadMessage(MessageType.Regular, 23, true, ""+count);
                count--;
                if(count < 0)
                {
                    Stop();
                    var distancia = m_Defender.GetDistance(m_Target.Location);
                    
                    if(m_Defender.Paralyzed || !m_Defender.InLOS(m_Target) || distancia > 16)
                    {
                        Effects.PlaySound(m_Defender.Location, m_Defender.Map, 0x307);
                        Effects.SendLocationEffect(m_Defender.Location, m_Defender.Map, 0x36B0, 9, 10, 0, 0);
                        var dmg = 20 + Utility.Random(20);
                        m_Defender.Damage(dmg);
                        DamageNumbers.ShowDamage(dmg, m_Defender, m_Defender, 42);
                    } else
                    {
                        Effects.SendMovingEffect(m_Defender, m_Target, 0xF0D, 12, 0, false, false, 0, 0);
                        Timer.DelayCall(TimeSpan.FromMilliseconds(500), () => {
                            var dmg = 25 + Utility.Random(35);
                            m_Target.Damage(dmg);
                            DamageNumbers.ShowDamage(dmg, m_Defender, m_Target, 32);
                            Effects.PlaySound(m_Target.Location, m_Target.Map, 0x307);
                            Effects.SendLocationEffect(m_Target.Location, m_Target.Map, 0x36B0, 9, 10, 0, 0);
                        });
                    }
                   
                }
            }
        }

        public override void OnThink()
        {
            if(this.Combatant != null && this.Combatant is Mobile)
            {
                if(!IsCooldown("pot"))
                {
                    OverheadMessage("* abre uma pot *");
                    SetCooldown("pot", TimeSpan.FromSeconds(15));
                    new PotTimer(this, (Mobile)this.Combatant);
                }
            }
            base.OnThink();
        }

        public override int GetAngerSound() { return 0x600; }
        public override int GetIdleSound() { return 0x600; }
        public override int GetAttackSound() { return 0x5FD; }
        public override int GetHurtSound() { return 0x5FF; }
        public override int GetDeathSound() { return 0x5FE; }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 1; } }
        public override int Meat { get { return 1; } }
        public override TribeType Tribe { get { return TribeType.GreenGoblin; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 3);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.02)
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
