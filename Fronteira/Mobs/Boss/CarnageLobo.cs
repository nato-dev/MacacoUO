using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Regions;
using System;

namespace Server.Mobiles
{
    [CorpseName("a dragon wolf corpse")]
    public class CarnageLobo : BaseCreature
    {
        [Constructable]
        public CarnageLobo()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.05, 0.5)
        {
            Name = "Lobo Sinistro";
            Body = 0xE1;
            BaseSoundID = 0xE5;

            SetStr(200, 200);
            SetDex(400, 400);
            SetStam(400, 400);

            SetInt(50, 55);

            SetHits(5000, 5000);

            SetDamage(3, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 60.0, 70.0);
            SetSkill(SkillName.MagicResist, 125.0, 140.0);
            SetSkill(SkillName.Tactics, 95.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 105.0);
            SetSkill(SkillName.DetectHidden, 60.0);

            Fame = 8500;
            Karma = -8500;

            Tamable = false;
            ControlSlots = 4;
            MinTameSkill = 102.0;

            SetWeaponAbility(Habilidade.BleedAttack);

            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                if (!this.Alive || this.Deleted)
                    return;

                var msg = "Carnage renasce em " + this.Location.X + " - " + this.Location.Y;
                foreach (var mobile in PlayerMobile.Instances)
                {
                    if (mobile != null && mobile.NetState != null)
                    {
                        mobile.SendGump(new AnuncioGump(mobile as PlayerMobile, msg));
                        mobile.SendMessage(78, "Um lobo sinistro nasceu em  " + this.Location.X + " - " + this.Location.Y+" ! Lute para mata-lo e ganhe recompensas !");
                        if(mobile.QuestArrow == null && mobile.Map == Map.Felucca && !(mobile.Region is DungeonRegion))
                        {
                            mobile.QuestArrow = new QuestArrow(mobile, this.Location);
                            mobile.QuestArrow.Update();
                        }
                    }
                }
            });

            Timer.DelayCall(TimeSpan.FromHours(2), () =>
            {
                ChecaTreta();
            });
        }

        public void ChecaTreta()
        {
            if(Combatant == null)
            {
                if(Hits == HitsMax)
                {
                    Delete();
                    return;
                }
            }

            Timer.DelayCall(TimeSpan.FromHours(1), () =>
            {
                ChecaTreta();
            });
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                if (c == null)
                    return;
                c.PublicOverheadMessage(Network.MessageType.Regular, 32, true, "* se meche *");
            });

            Timer.DelayCall(TimeSpan.FromSeconds(20), () => {
                if (c == null)
                    return;
                c.PublicOverheadMessage(Network.MessageType.Regular, 32, true, "* se meche mais *");
            });

            Timer.DelayCall(TimeSpan.FromSeconds(30), () => {
                if (c == null)
                    return;
                c.PublicOverheadMessage(Network.MessageType.Regular, 32, true, "* sinistro *");
            });

            Timer.DelayCall(TimeSpan.FromSeconds(31), () => {
                if (c == null)
                    return;

                c.PublicOverheadMessage(Network.MessageType.Regular, 32, true, "* se transforma *");
                Carnage rm = new Carnage();
                rm.Team = this.Team;
                rm.Combatant = this.Combatant;
                

                rm.MoveToWorld(c.Location, c.Map);
                Effects.PlaySound(c, c.Map, 0x208);
                Effects.SendLocationEffect(c.Location, c.Map, 0x3709, 30, 10, 542, 0);

                var from = rm;
                Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
                Effects.PlaySound(from.Location, from.Map, 0x243);

                Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 542, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 542, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 542, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                Effects.SendTargetParticles(from, 0x375A, 35, 90, 542, 0x00, 9502, (EffectLayer)255, 0x100);
                c.Delete();
            });
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);
            if (from != this)
            {
                var rnd = Utility.RandomDouble();
                if (from != this && rnd < 0.8)
                {
                    this.Combatant = from;
                    this.OverheadMessage("* awwrrrr *");
                }
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            var rnd = Utility.RandomDouble();

            if (from != this && rnd < 0.8)
            {
                this.Combatant = from;
                this.OverheadMessage("* awwrrrr *");
            }

            if (rnd < 0.03)
            {
                var lobim = new SavagePackWolfy();
                lobim.MoveToWorld(from.Location, from.Map);
                if (from != this)
                    lobim.Combatant = from;
                lobim.OverheadMessage("* grrr *");
                from.PlaySound(lobim.GetAngerSound());
                from.SendMessage("Um lobo sai da tocaia lhe atacando");
            }
        }

        public override bool OnBeforeDeath()
        {
            return base.OnBeforeDeath();
        }

        public CarnageLobo(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 5; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }
        public override HideType HideType { get { return HideType.Horned; } }


        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Rich);
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
