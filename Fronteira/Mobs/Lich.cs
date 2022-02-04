using System;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a liche's corpse")]
    public class Lich : BaseCreature
    {
        public override double DisturbChance { get { return 0; } }

        [Constructable]
        public Lich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "lich";
            Body = 24;
            BaseSoundID = 0x3E9;

            SetStr(171, 200);
            SetDex(126, 145);
            SetInt(276, 305);

            SetHits(203, 260);

            SetDamage(18, 33);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Necromancy, 89, 99.1);
            SetSkill(SkillName.SpiritSpeak, 90.0, 99.0);
            SetSkill(SkillName.Poisoning, 90.0, 99.0);
            SetSkill(SkillName.Inscribe, 100.0);
            SetSkill(SkillName.EvalInt, 50.0);
            SetSkill(SkillName.Magery, 70.1, 80.0);
            SetSkill(SkillName.Meditation, 85.1, 95.0);
            SetSkill(SkillName.MagicResist, 100, 200);
            SetSkill(SkillName.Tactics, 70.1, 90.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;

            if (Utility.Random(20) == 1)
            {
                PackItem(Loot.RandomClothing());
            }

            /*
            if(Utility.RandomDouble() < 0.05)
            {
                PackItem(new NecromancerSpellbook());
                PackItem(new AnimateDeadScroll());
            }
            */

            var ns = NecroScroll(25, true);
            if(ns != null)
            {
                PackItem(ns);
            }

            PackItem(new GnarledStaff());
            PackNecroReg(17, 24);
        }

        public override bool UseSmartAI
        {
            get { return !this.Summoned; }
        }

        public override bool IsSmart
        {
            get { return !this.Summoned; }
        }


        public static Item NecroScroll(int rnd, bool newbs)
        {
            var r = Utility.Random(rnd);
            if(newbs && rnd > 9)
            {
                return null;
            }
            switch (r)
            {
                case 0: return new AnimateDeadScroll();
                case 1: return new BloodOathScroll();
                case 2: return new CorpseSkinScroll();
                case 3: return new CurseWeaponScroll();
                case 4: return new EvilOmenScroll(); 
                case 5: return new HorrificBeastScroll();
                case 6: return new MindRotScroll();
                case 7: return new PainSpikeScroll(); 
                case 8: return new WraithFormScroll(); 
                case 9: return new PoisonStrikeScroll();
                case 10: return new WitherScroll();
                case 11: return new LichFormScroll();
                case 12: return new StrangleScroll();
                case 13: return new SummonFamiliarScroll();
                case 14: return new VampiricEmbraceScroll();
                case 15: return new VengefulSpiritScroll();
            }
            return null;
        }

        public Lich(Serial serial)
            : base(serial)
        {
        }

        public class SkelTimer : Timer
        {
            private Lich mob;
            private int ct = 0;
            private Mobile from;

            int dMin = 0;
            int dMax = 0;
            int hue = 0;

            public SkelTimer(Lich defender, Mobile from)
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

            if(from is BaseSummoned)
            {
                PublicOverheadMessage(Network.MessageType.Emote, 0, false, "An Kal Ort");
                ((BaseSummoned)from).Dispel(this);
            } else if(from is BaseCreature)
            {
                var creature = (BaseCreature)from;
                if(creature.Controlled)
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
                if(!IsCooldown("skeleton") && rnd == 5)
                {
                    SetCooldown("skeleton", TimeSpan.FromSeconds(40));
                    PublicOverheadMessage(Network.MessageType.Emote, 0, false, "Kal Vas Corp");
                    new SkelTimer(this, (Mobile)Combatant).Start();
                }
            }
        }

        public override TribeType Tribe { get { return TribeType.Undead; } }

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
        public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
            AddPackedLoot(LootPack.MeagerProvisions, typeof(Bag));
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
