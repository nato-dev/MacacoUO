using Server.Items;
using System;

namespace Server.Mobiles
{
    [CorpseName("a cyclopean corpse")]
    public class Cyclops : BaseCreature
    {
        [Constructable]
        public Cyclops()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "ciclope";
            this.Body = 75;
            this.BaseSoundID = 604;

            this.SetStr(336, 385);
            this.SetDex(96, 115);
            this.SetInt(31, 55);

            this.SetHits(1202, 1231);
            this.SetMana(0);

            this.SetDamage(27, 33);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 45, 50);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 60.3, 105.0);
            this.SetSkill(SkillName.Tactics, 80.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 80.1, 90.0);
            this.SetSkill(SkillName.Parry, 50);

            this.Fame = 34500;
            this.Karma = -34500;

            this.VirtualArmor = 48;
            SetWeaponAbility(WeaponAbility.CrushingBlow);

            if (Utility.RandomDouble() < 0.35)
                AddItem(new T2ARecallRune());
        }

        public Cyclops(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 4;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 3;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV5);
            this.AddLoot(LootPack.LV4);
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

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            Orc.TentaAtacarMaster(this, defender);
            if (Utility.Random(0, 3) == 1)
            {
                if (!IsCooldown("chute"))
                {
                    this.PlayAttackAnimation();
                    this.PlaySound(0x13C);
                    SetCooldown("chute", TimeSpan.FromSeconds(5));
                    this.OverheadMessage("* Marretada Epica *");
                    new ChuteTimer(this, defender).Start();
                }
            }
        }

        public class ChuteTimer : Timer
        {
            private BaseCreature m_Defender;
            private Mobile player;
            private Direction dir;
            private int tick;

            public ChuteTimer(BaseCreature defender, Mobile player)
                : base(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100), 7)
            {
                this.dir = defender.Direction;
                this.m_Defender = defender;
                this.player = player;
            }

            public static Point3D GetPoint(Mobile m, Direction d)
            {
                var loc = m.Location.Clone3D();
                var x = 0;
                var y = 0;
                switch (d & Direction.Mask)
                {
                    case Direction.North:
                        --y;
                        break;
                    case Direction.Right:
                        ++x;
                        --y;
                        break;
                    case Direction.East:
                        ++x;
                        break;
                    case Direction.Down:
                        ++x;
                        ++y;
                        break;
                    case Direction.South:
                        ++y;
                        break;
                    case Direction.Left:
                        --x;
                        ++y;
                        break;
                    case Direction.West:
                        --x;
                        break;
                    case Direction.Up:
                        --x;
                        --y;
                        break;
                }
                loc.X += x;
                loc.Y += y;
                return loc;
            }

            protected override void OnTick()
            {
                if (this.player == null || this.m_Defender == null)
                    return;

                if (this.m_Defender.Map == null || !this.m_Defender.Alive)
                    return;

                if (this.player.Map == null || !this.player.Alive)
                    return;

                int z = 0;
                if (this.player.CheckMovement(this.dir, out z))
                {
                    this.player.MoveToWorld(GetPoint(this.player, this.dir), this.player.Map);
                }
                else
                {
                    this.player.PlaySound(0x13C);
                    this.player.OverheadMessage("* Ouch! *");
                    var b = new Rectangle2D(this.player.X - 1, this.player.Y - 1, 3, 3);
                    for (var i = 0; i < 3; i++)
                        BaseWeapon.AddBlood(this.player, this.player.Map.GetRandomSpawnPoint(b), this.player.Map);
                    this.player.FixedParticles(6008, 9, 1, 1, 0, 1, EffectLayer.Head, 0);
                    AOS.Damage(this.player, Utility.Random(30, 50));
                    this.player.SendMessage("Voce bateu seu corpo contra a parede");
                    this.player.OverheadMessage("* tonto *");
                    this.player.Freeze(TimeSpan.FromSeconds(3));
                    this.Stop();
                }
                this.player.Move(this.dir, true);
            }
        }
    }
}
