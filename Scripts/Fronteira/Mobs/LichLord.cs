using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a liche's corpse")]
    public class LichLord : BaseCreature
    {
        public override double DisturbChance { get { return 0; } }


        public override bool UseSmartAI
        {
            get { return !this.Summoned; }
        }

        public override bool IsSmart
        {
            get { return !this.Summoned; }
        }

        [Constructable]
        public LichLord()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "lich ancestral";
            this.Body = 79;
            this.BaseSoundID = 412;

            this.SetStr(416, 505);
            this.SetDex(146, 165);
            this.SetInt(566, 655);

            this.SetHits(350, 473);

            this.SetDamage(19, 33);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Cold, 60);
            this.SetDamageType(ResistanceType.Energy, 40);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 50, 60);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.Necromancy, 90, 110.0);
            this.SetSkill(SkillName.SpiritSpeak, 90.0, 110.0);
            SetSkill(SkillName.Poisoning, 90.0, 99.0);
            this.SetSkill(SkillName.Inscribe, 90.1, 100.0);
            this.SetSkill(SkillName.EvalInt, 99.1, 100.0);
            this.SetSkill(SkillName.Magery, 99.9, 100.0);
            this.SetSkill(SkillName.MagicResist, 150.5, 200.0);
             this.SetSkill(SkillName.DetectHidden, 150.5, 200.0);
            this.SetSkill(SkillName.Tactics, 50.1, 70.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 80.0);

            this.Fame = 18000;
            this.Karma = -18000;

            this.VirtualArmor = 70;
            this.PackItem(new GnarledStaff());
            this.PackNecroReg(40, 60);

            //PackItem(new NecromancerSpellbook());
            //PackItem(new AnimateDeadScroll());


            switch (Utility.Random(5))
            {
                case 0: PackItem(new LichFormScroll()); break;
                case 1: PackItem(new PoisonStrikeScroll()); break;
                case 2: PackItem(new StrangleScroll()); break;
                case 3: PackItem(new VengefulSpiritScroll()); break;
				case 4: PackItem(new WitherScroll()); break;
			}
        }

        public LichLord(Serial serial)
            : base(serial)
        {
        }

        public override TribeType Tribe { get { return TribeType.MortoVivo; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }

        public class SkelTimer : Timer
        {
            private Mobile mob;
            private int ct = 0;
            private Mobile from;

            int dMin = 0;
            int dMax = 0;
            int hue = 0;

            public SkelTimer(Mobile defender, Mobile from)
                : base(TimeSpan.FromSeconds(2))
            {
                mob = defender;
                this.from = from;
            }

            protected override void OnTick()
            {
                var skeleton = new Skeleton();
                skeleton.MoveToWorld(mob.Location, mob.Map);
                var skeleton2 = new Skeleton();
                skeleton2.MoveToWorld(mob.Location, mob.Map);
                skeleton.PublicOverheadMessage(Network.MessageType.Emote, 0, false, "* levanta de baixo da terra *");
                skeleton.Combatant = from;
                skeleton.FocusMob = from;
                skeleton2.Combatant = from;
                skeleton2.FocusMob = from;
                mob.MovingParticles(skeleton, 0x376A, 9, 0, false, false, 9502, 0x376A, 0x204);
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (this.Summoned)
                return;

            if (from is BaseSummoned)
            {
                PublicOverheadMessage(Network.MessageType.Emote, 0, false, "An Kal Ort");
                ((BaseSummoned)from).Dispel(this);
            }
            else if (from is BaseCreature)
            {
                var creature = (BaseCreature)from;
                if (creature.Controlled)
                {
                    PublicOverheadMessage(Network.MessageType.Emote, 0, false, "An Vas Ex Por");
                    this.MovingParticles(creature, 0x376A, 7, 0, false, false, 9502, 0x376A, 0x204);
                    creature.FocusMob = null;
                    creature.Warmode = false;
                    creature.Combatant = null;
                    creature.Freeze(TimeSpan.FromSeconds(10));
                }
            }
        }

        public override void OnThink()
        {
            if (this.Summoned)
                return;

            base.OnThink();
            var rnd = Utility.Random(10);
            if (Combatant != null)
            {
                if (!IsCooldown("skeleton") && rnd == 5)
                {
                    SetCooldown("skeleton", TimeSpan.FromSeconds(40));
                    PublicOverheadMessage(Network.MessageType.Emote, 0, false, "Kal Vas Corp");
                    new SkelTimer(this, (Mobile)Combatant).Start();
                }
            }
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 4;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV5);
            this.AddLoot(LootPack.MedScrolls, 2);
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
