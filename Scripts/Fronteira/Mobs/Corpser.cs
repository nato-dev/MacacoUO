using System;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a corpser corpse")]
    public class Corpser : BaseCreature
    {
        [Constructable]
        public Corpser()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 4, 0.2, 0.4)
        {
            this.Name = "planta carnivora";
            this.Body = 8;
            this.BaseSoundID = 684;

            this.SetStr(156, 180);
            this.SetDex(120, 180);
            this.SetInt(26, 40);

            this.SetHits(64, 70);
            this.SetMana(0);

            this.SetDamage(3, 10);
            this.SetDamageType(ResistanceType.Physical, 60);
            this.SetDamageType(ResistanceType.Poison, 40);

            this.SetResistance(ResistanceType.Physical, 15, 20);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 20, 30);

            this.SetSkill(SkillName.MagicResist, 15.1, 20.0);
            this.SetSkill(SkillName.Tactics, 45.1, 60.0);
            this.SetSkill(SkillName.Wrestling, 55.1, 65.0);

            this.Fame = 1000;
            this.Karma = -1000;

            this.VirtualArmor = 8;

            if (0.25 > Utility.RandomDouble())
                this.PackItem(new Board(10));
            else
                this.PackItem(new Log(5));

            this.PackItem(new MandrakeRoot(5));
            this.PackItem(new Ginseng(5));
        }



        public Corpser(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lesser;
            }
        }
        public override bool DisallowAllMoves
        {
            get
            {
                return true;
            }
        }

        public override void OnThink()
        {
            if (this.Combatant != null && this.Combatant.InRange2D(this.Location, 9))
            {
                if (this.Combatant.GetDistance(this.Location) <= 2)
                    return;

                if (!this.IsCooldown("omnoma"))
                {
                    this.SetCooldown("omnoma", TimeSpan.FromSeconds(2));
                }
                else
                {
                    return;
                }

                if (!this.InLOS(this.Combatant))
                {
                    return;
                }

                var defender = (Mobile)this.Combatant;
                if (defender == null || defender.Map == null || !defender.Alive)
                    return;

                if (defender.IsCooldown("omnom"))
                    return;

                SpellHelper.Turn(this, defender);
                var locPlayerGo = GetPoint(defender, this.Direction);
                if (defender.Map.CanFit(locPlayerGo, locPlayerGo.Z))
                {
                    this.PlayAttackAnimation();
                    this.MovingParticles(defender, 0x0D3B, 11, 0, false, false, 9502, 4019, 0x160);
                    Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                    {
                        defender.Freeze(TimeSpan.FromMilliseconds(600));
                        Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                        {
                            defender.MovingParticles(this, 0x0D3B, 15, 0, false, false, 9502, 4019, 0x160);
                            defender.SendMessage("A planta carnivora te puxa");
                            defender.MoveToWorld(locPlayerGo, defender.Map);
                            defender.SetCooldown("onnom", TimeSpan.FromSeconds(2));
                            if (!this.IsCooldown("omnom"))
                            {
                                this.SetCooldown("omnom", TimeSpan.FromSeconds(10));
                                this.OverheadMessage("* Nhom nom nom *");
                            }
                        });
                    });

                }
            }
            base.OnThink();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {

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
            loc.X -= x * 2;
            loc.Y -= y * 2;
            return loc;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV2, 2);
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

            if (this.BaseSoundID == 352)
                this.BaseSoundID = 684;
        }
    }

    public class CorpserAntimago : BaseCreature
    {
        [Constructable]
        public CorpserAntimago()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 4, 0.2, 0.4)
        {
            this.Name = "planta carnivora fertilizada";
            this.Body = 8;
            this.BaseSoundID = 684;
            this.Hue = 1154;

            this.SetStr(156, 180);
            this.SetDex(120, 180);
            this.SetInt(26, 40);

            this.SetHits(700, 900);
            this.SetMana(0);

            this.SetDamage(25, 35);
            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 100, 100);

            this.SetSkill(SkillName.MagicResist, 300, 400);
            this.SetSkill(SkillName.Tactics, 45.1, 60.0);
            this.SetSkill(SkillName.Wrestling, 75.1, 90.0);

            this.Fame = 3000;
            this.Karma = -3000;

            this.VirtualArmor = 0;

            if (0.25 > Utility.RandomDouble())
                this.PackItem(new Board(30));
            else
                this.PackItem(new Log(30));

            this.PackItem(new MandrakeRoot(15));
            this.PackItem(new Ginseng(15));
            this.PackItem(new Emerald());
        }

        public CorpserAntimago(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lesser;
            }
        }
        public override bool DisallowAllMoves
        {
            get
            {
                return true;
            }
        }

        public override void OnThink()
        {
            if (this.Combatant != null && this.Combatant.InRange2D(this.Location, 14))
            {
                if (this.Combatant.GetDistance(this.Location) <= 2)
                    return;

                if (!this.IsCooldown("omnoma"))
                {
                    this.SetCooldown("omnoma", TimeSpan.FromSeconds(2));
                }
                else
                {
                    return;
                }

                if (!this.InLOS(this.Combatant))
                {
                    return;
                }

                var defender = (Mobile)this.Combatant;
                if (defender == null || defender.Map == null || !defender.Alive)
                    return;

                if (defender.IsCooldown("omnom"))
                    return;

                SpellHelper.Turn(this, defender);
                var locPlayerGo = GetPoint(defender, this.Direction);
                if (defender.Map.CanFit(locPlayerGo, locPlayerGo.Z))
                {
                    this.PlayAttackAnimation();
                    this.MovingParticles(defender, 0x0D3B, 11, 0, false, false, 9502, 4019, 0x160);
                    Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                    {
                        defender.Freeze(TimeSpan.FromMilliseconds(600));
                        Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                        {
                            defender.MovingParticles(this, 0x0D3B, 15, 0, false, false, 9502, 4019, 0x160);
                            defender.SendMessage("A planta carnivora te puxa");
                            defender.MoveToWorld(locPlayerGo, defender.Map);
                            defender.SetCooldown("onnom", TimeSpan.FromSeconds(2));
                            if (!this.IsCooldown("omnom"))
                            {
                                this.SetCooldown("omnom", TimeSpan.FromSeconds(10));
                                this.OverheadMessage("* Nhom nom nom *");
                            }
                        });
                    });

                }
            }
            base.OnThink();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {

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
            loc.X -= x * 2;
            loc.Y -= y * 2;
            return loc;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV2, 2);
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

            if (this.BaseSoundID == 352)
                this.BaseSoundID = 684;
        }
    }
}
