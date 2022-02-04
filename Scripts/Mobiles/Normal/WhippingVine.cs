using System;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a whipping vine corpse")]
    public class WhippingVine : BaseCreature
    {
        [Constructable]
        public WhippingVine()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "vinhas assassinas";
            this.Body = 8;
            this.Hue = 0x851;
            this.BaseSoundID = 352;

            this.SetStr(251, 300);
            this.SetDex(76, 100);
            this.SetInt(26, 40);
            this.SetHits(300);
            this.SetMana(0);

            this.SetDamage(10, 25);

            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Poison, 30);

            this.SetResistance(ResistanceType.Physical, 75, 85);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 15, 25);
            this.SetResistance(ResistanceType.Poison, 75, 85);
            this.SetResistance(ResistanceType.Energy, 35, 45);

            this.SetSkill(SkillName.MagicResist, 70.0);
            this.SetSkill(SkillName.Tactics, 70.0);
            this.SetSkill(SkillName.Wrestling, 70.0);

            this.Fame = 1000;
            this.Karma = -1000;

            this.VirtualArmor = 45;

            this.PackReg(3);
            this.PackItem(new FertileDirt(Utility.RandomMinMax(1, 10)));

            if (0.5 >= Utility.RandomDouble())
                this.PackItem(new ExecutionersCap());

            PackItem(new Vines());  //this is correct
            PackItem(new FertileDirt(Utility.RandomMinMax(1, 10)));

            if (Utility.RandomDouble() < 0.10)
            {
              
                PackItem(new DecorativeVines());
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
                SpellHelper.Turn(this, defender);
                var locPlayerGo = Corpser.GetPoint(defender, this.Direction);
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

        public WhippingVine(Serial serial)
            : base(serial)
        {
        }

        public override bool BardImmune
        {
            get
            {
                return !Core.AOS;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
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
