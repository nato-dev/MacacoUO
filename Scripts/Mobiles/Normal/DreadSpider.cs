using System;
using Server.Items;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.DreadSpiderWeak")]
    [CorpseName("a dread spider corpse")]
    public class DreadSpider : BaseCreature
    {
        [Constructable]
        public DreadSpider()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "aranha assassina";
            Body = 11;
            BaseSoundID = 1170;

            SetStr(196, 220);
            SetDex(126, 145);
            SetInt(286, 310);

            SetHits(118, 132);

            SetDamage(5, 17);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 65.1, 80.0);
            SetSkill(SkillName.Magery, 65.1, 80.0);
            SetSkill(SkillName.MagicResist, 45.1, 60.0);
            SetSkill(SkillName.Tactics, 55.1, 70.0);
            SetSkill(SkillName.Wrestling, 60.1, 75.0);
            SetSkill(SkillName.Poisoning, 80.0);
            SetSkill(SkillName.DetectHidden, 50.0, 60.0);
            SetSkill(SkillName.Necromancy, 20.0);
            SetSkill(SkillName.SpiritSpeak, 20.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 36;

            PackItem(new SpidersSilk(8));

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 96.0;
        }

              public override void OnThink()
        {
            if (!this.IsCooldown("teia"))
            {
                this.SetCooldown("teia", TimeSpan.FromSeconds(3));
            }
            else
            {
                return;
            }
            if (this.Combatant != null && this.Combatant.InRange2D(this.Location, 7))
            {  
                if (!this.IsCooldown("teiab"))
                {
                    this.SetCooldown("teiab", TimeSpan.FromSeconds(30));
                }
                else
                {
                    return;
                }

                if (!this.InLOS(this.Combatant))
                {
                    return;
                }
                this.PlayAngerSound();
                this.MovingParticles(this.Combatant, 0x10D3, 15, 0, false, false, 9502, 4019, 0x160);
                var m = this.Combatant as Mobile;
                Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                {
                    m.SendMessage("Voce foi preso por uma teia e nao consegue se soltar");
                    m.OverheadMessage("* Preso em uma teia *");
                    var teia = new Teia(m);
                    teia.MoveToWorld(m.Location, m.Map);
                    m.Freeze(TimeSpan.FromSeconds(6));
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        teia.Delete();
                        m.Frozen = false;
                    });
                });
            }
        }

        public DreadSpider(Serial serial)
            : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 3; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV5);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version == 0 && (AbilityProfile == null || AbilityProfile.MagicalAbility == MagicalAbility.None))
            {
                SetMagicalAbility(MagicalAbility.Poisoning);
            }
        }
    }
}
