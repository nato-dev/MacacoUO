using System;
using Server.Items;
using Server.Spells;
using Server.Ziden;

namespace Server.Mobiles
{
    [CorpseName("a reapers corpse")]
    public class Reaper : BaseCreature
    {
        [Constructable]
        public Reaper()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "ente";
            this.Body = 47;
            this.BaseSoundID = 442;

            this.SetStr(66, 215);
            this.SetDex(66, 75);
            this.SetInt(101, 250);

            this.SetHits(40, 129);
            this.SetStam(0);

            this.SetDamage(9, 15);

            this.SetDamageType(ResistanceType.Physical, 80);
            this.SetDamageType(ResistanceType.Poison, 20);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 100.1, 125.0);
            this.SetSkill(SkillName.Tactics, 45.1, 60.0);
            this.SetSkill(SkillName.Wrestling, 50.1, 60.0);

            this.Fame = 3500;
            this.Karma = -3500;

            this.VirtualArmor = 40;

            var r = Utility.Random(20);
            if(r==1)
            {
                Name = "ente gelido";
                Hue = 1154;
                this.SetDamage(20, 40);
                this.PackItem(new FrostwoodLog(5));
            }
          
            this.PackItem(new Log(10));
            this.PackItem(new MandrakeRoot(5));

            SetWeaponAbility(Habilidade.MortalStrike);
        }

        public Reaper(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            if (this.Combatant != null && this.Combatant.InRange2D(this.Location, 14))
            {
                if (this.Combatant.GetDistance(this.Location) <= 2)
                    return;

                if (!this.IsCooldown("omnoma"))
                {
                    this.SetCooldown("omnoma", TimeSpan.FromSeconds(1.5));
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
                SpellHelper.Turn(this, defender);
                var locPlayerGo = Corpser.GetPoint(defender, this.Direction);
                if (defender.Map.CanFit(locPlayerGo, locPlayerGo.Z))
                {
                    this.PlayAttackAnimation();
                    this.MovingParticles(defender, 0x0D3B, 15, 0, false, false, 9502, 4019, 0x160);
                    Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                    {
                        defender.Freeze(TimeSpan.FromMilliseconds(600));
                        Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                        {
                            defender.MovingParticles(this, 0x0D3B, 15, 0, false, false, 9502, 4019, 0x160);
                            defender.SendMessage("A arvore joga uma vinha");
                            defender.MoveToWorld(locPlayerGo, defender.Map);
                            if (!this.IsCooldown("omnom"))
                            {
                                this.SetCooldown("omnom", TimeSpan.FromSeconds(10));
                                this.OverheadMessage("* nhac nhac *");
                            }
                        });
                    });

                }
            }
            base.OnThink();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if(!BleedAttack.IsBleeding(defender))
            {
                BleedAttack.BeginBleed(defender, this, true);
            }
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Greater;
            }
        }

        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Arachnid;
            }
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }

        public override bool DisallowAllMoves
        {
            get
            {
                return true;
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Average);
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

    [CorpseName("a reapers corpse")]
    public class ArvoreDeSeiva : BaseCreature
    {
        [Constructable]
        public ArvoreDeSeiva()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "seringueira";
            this.Body = 47;
            this.BaseSoundID = 442;

            this.Hue = Loot.RandomRareDye();

            this.SetStr(66, 215);
            this.SetDex(66, 75);
            this.SetInt(101, 250);

            this.SetHits(600, 700);
            this.SetStam(0);

            this.SetDamage(9, 15);

            this.SetDamageType(ResistanceType.Physical, 80);
            this.SetDamageType(ResistanceType.Poison, 20);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 100.1, 125.0);
            this.SetSkill(SkillName.Tactics, 45.1, 60.0);
            this.SetSkill(SkillName.Wrestling, 50.1, 60.0);

            this.Fame = 3500;
            this.Karma = -3500;

            this.VirtualArmor = 40;

            this.PackItem(new Log(10));
            this.PackItem(new MandrakeRoot(5));

            SetWeaponAbility(Habilidade.MortalStrike);
        }

        public ArvoreDeSeiva(Serial serial)
            : base(serial)
        {
        }

        public override void OnBeforeDamage(Mobile from, ref int totalDamage, DamageType type)
        {
            if(from.Weapon is BaseAxe)
            {
                totalDamage *= 4;
            } else
            {
                if(!from.IsCooldown("dicaaxe"))
                {
                    from.SetCooldown("dicaaxe");
                    from.SendMessage(78, "Talvez um machado fosse mais eficiente contra isto");
                }
            }
            base.OnBeforeDamage(from, ref totalDamage, type);
        }

        public override void OnThink()
        {
            if (this.Combatant != null && this.Combatant.InRange2D(this.Location, 14))
            {
                if (this.Combatant.GetDistance(this.Location) <= 2)
                    return;

                if (!this.IsCooldown("omnoma"))
                {
                    this.SetCooldown("omnoma", TimeSpan.FromSeconds(1.5));
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
                SpellHelper.Turn(this, defender);
                var locPlayerGo = Corpser.GetPoint(defender, this.Direction);
                if (defender.Map.CanFit(locPlayerGo, locPlayerGo.Z))
                {
                    this.PlayAttackAnimation();
                    this.MovingParticles(defender, 0x0D3B, 15, 0, false, false, 9502, 4019, 0x160);
                    Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                    {
                        defender.Freeze(TimeSpan.FromMilliseconds(600));
                        Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                        {
                            defender.MovingParticles(this, 0x0D3B, 15, 0, false, false, 9502, 4019, 0x160);
                            defender.SendMessage("A arvore joga uma vinha");
                            defender.MoveToWorld(locPlayerGo, defender.Map);
                            if (!this.IsCooldown("omnom"))
                            {
                                this.SetCooldown("omnom", TimeSpan.FromSeconds(10));
                                this.OverheadMessage("* nhac nhac *");
                            }
                        });
                    });

                }
            }
            base.OnThink();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if (!BleedAttack.IsBleeding(defender))
            {
                BleedAttack.BeginBleed(defender, this, true);
            }

        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Greater;
            }
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }

        public override bool DisallowAllMoves
        {
            get
            {
                return true;
            }
        }

        public override void OnDeath(Container c)
        {
          
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Average);
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

        public override void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            from.OverheadMessage("* extraiu seiva *");
            var dt = new Seiva();
            //dt.charges = 2;
            dt.Hue = this.Hue;
            //dt.Name = "Tubo de Seiva Colorida";
            corpse.AddItem(dt);
            from.SendMessage("Voce extraiu seiva da arvore");
            corpse.Carved = true;
            PlaySound(0x57);
        }
    }
}
