using System;
using Server.Items;
using Server.Ziden;

namespace Server.Mobiles
{
    [CorpseName("a snow elemental corpse")]
    public class SnowElemental : BaseCreature
    {
        [Constructable]
        public SnowElemental()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental da neve";
            this.Body = 163;
            this.BaseSoundID = 263;

            this.SetStr(326, 355);
            this.SetDex(166, 185);
            this.SetInt(71, 95);

            this.SetHits(196, 213);

            this.SetDamage(11, 17);

            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Cold, 80);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 10, 15);
            this.SetResistance(ResistanceType.Cold, 60, 70);
            this.SetResistance(ResistanceType.Poison, 25, 35);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            this.SetSkill(SkillName.MagicResist, 120, 120);
            this.SetSkill(SkillName.Tactics, 80.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 100, 100.0);

            this.Fame = 5000;
            this.Karma = -5000;

            this.VirtualArmor = 50;

            this.PackItem(new BlackPearl(3));
            Item ore = new IronOre(3);
            ore.ItemID = 0x19B8;
            this.PackItem(ore);
            this.PackItem(new BolaDeNeve());
            this.PackItem(new BolaDeNeve());
            Imp.Converte(this, 1154);
        }

        public override void OnThink()
        {
            base.OnThink();
            if (this.Combatant != null)
            {
                if (!IsCooldown("bonethrow"))
                {
                    if (this.Combatant is PlayerMobile)
                    {
                        var player = (PlayerMobile)this.Combatant;
                        var dist = player.GetDistanceToSqrt(this.Location);
                        if (dist <= 3 || dist >= 9 || !this.InLOS(player))
                        {
                            return;
                        }
     
                        SetCooldown("bonethrow", TimeSpan.FromSeconds(10));
                        this.MovingParticles(player, 0x3729, 9, 0, false, false, 9502, 4019, 0x160);
                        AOS.Damage(player, 5 + Utility.Random(5), 0, 0, 0, 0, 0);
                        PublicOverheadMessage(Network.MessageType.Regular, 0, false, "* joga gelo *");
                        player.Freeze(TimeSpan.FromSeconds(0.5));
                    }


                }
            }
        }

        public SnowElemental(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return Utility.RandomList(2, 3);
            }
        }

        public override bool HasAura { get { return true; } }
        public override int AuraBaseDamage { get { return 15; } }
        public override int AuraRange { get { return 2; } }
        public override int AuraFireDamage { get { return 0; } }
        public override int AuraColdDamage { get { return 100; } }

        public override void AuraEffect(Mobile m)
        {
            m.SendLocalizedMessage(1008111, false, this.Name); //  : The intense cold is damaging you!
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV4);
            this.AddLoot(LootPack.Gems);
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
