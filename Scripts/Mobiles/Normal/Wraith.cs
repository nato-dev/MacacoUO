using System;
using Server.Items;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.Second;

namespace Server.Mobiles
{
    [CorpseName("a ghostly corpse")]
    public class Wraith : BaseCreature
    {
        [Constructable]
        public Wraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "spectro";
            this.Body = 26;
            this.Hue = 0x4001;
            this.BaseSoundID = 0x482;

            this.SetStr(76, 100);
            this.SetDex(76, 95);
            this.SetInt(36, 60);

            this.SetHits(56, 80);

            this.SetDamage(7, 11);

            this.SetDamageType(ResistanceType.Physical, 45);
            this.SetDamageType(ResistanceType.Cold, 55);

            this.SetResistance(ResistanceType.Physical, 25, 30);
            this.SetResistance(ResistanceType.Cold, 15, 25);
            this.SetResistance(ResistanceType.Poison, 10, 20);

            this.SetSkill(SkillName.EvalInt, 55.1, 70.0);
            this.SetSkill(SkillName.Magery, 55.1, 70.0);
            this.SetSkill(SkillName.MagicResist, 10, 20);
            this.SetSkill(SkillName.Tactics, 45.1, 60.0);
            this.SetSkill(SkillName.Wrestling, 45.1, 55.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 65;

            this.PackReg(10);
        }

        public override Spell ChooseSpell()
        {
            return new HarmSpell(this, null);
        }

        public Wraith(Serial serial)
            : base(serial)
        {
        }

        /*
        public override void OnTarget(Mobile m)
        {
            base.OnTarget(m);
            if(m != FocusMob && m != null && m is PlayerMobile)
            {
                var player = (PlayerMobile)m;
                if(!player.IsCooldown("specfreeze"))
                {
                    player.SetCooldown("specfreeze", TimeSpan.FromSeconds(40));
                    PublicOverheadMessage(Network.MessageType.Emote, 0, false, "An Ex Por");
                    new FreezeTimer(this, m).Start();
                }
            }
        }
        */

        public class FreezeTimer : Timer
        {
            private BaseCreature mob;
            private int ct = 0;
            private Mobile from;

            int dMin = 0;
            int dMax = 0;
            int hue = 0;

            public FreezeTimer(BaseCreature defender, Mobile from)
                : base(TimeSpan.FromSeconds(1))
            {
                mob = defender;
                this.from = from;
            }

            protected override void OnTick()
            {
                from.PlaySound(0x204);
                from.Freeze(TimeSpan.FromSeconds(6));
                mob.MovingParticles(from, 0x376A, 9, 0, false, false, 9502, 0x376A, 0x204);
                mob.SendMessage("O monstro lancou um olhar petrificante");
            }
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }

        public override TribeType Tribe { get { return TribeType.MortoVivo; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV2);
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
