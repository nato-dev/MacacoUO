using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpo de formiga-aranha")]
    public class SolenLouca : BaseCreature, IBlackSolen
    {
        [Constructable]
        public SolenLouca()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "formiga-aranha";
            this.Body = 805;
            this.BaseSoundID = 959;
            this.Hue = 0x47D;

            this.SetStr(96, 120);
            this.SetDex(170, 190);
            this.SetInt(36, 60);

            this.SetHits(100, 100);

            this.SetDamage(5, 7);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 30);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.MagicResist, 60.0);
            this.SetSkill(SkillName.Tactics, 65.0);
            this.SetSkill(SkillName.Wrestling, 60.0);

            this.Fame = 1500;
            this.Karma = -1500;

            this.VirtualArmor = 28;

            SetWeaponAbility(WeaponAbility.MortalStrike);

            this.PackGold(Utility.Random(100, 180));

            SolenHelper.PackPicnicBasket(this);

            this.PackItem(new ZoogiFungus());
        }

        public SolenLouca(Serial serial)
            : base(serial)
        {
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
            if (this.Combatant != null && this.Combatant.InRange2D(this.Location, 9))
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

        public override int GetAngerSound()
        {
            return 0x269;
        }

        public override int GetIdleSound()
        {
            return 0x269;
        }

        public override int GetAttackSound()
        {
            return 0x186;
        }

        public override int GetHurtSound()
        {
            return 0x1BE;
        }

        public override int GetDeathSound()
        {
            return 0x8E;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 2));
        }

        public override bool IsEnemy(Mobile m)
        {
            if (SolenHelper.CheckBlackFriendship(m))
                return false;
            else
                return base.IsEnemy(m);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            SolenHelper.OnBlackDamage(from);

            base.OnDamage(amount, from, willKill);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            corpse.Carved = true;
            from.PrivateOverheadMessage("* Coletou teias *");
            from.AddToBackpack(new SpidersSilk(7 + Utility.Random(10)));
            PlaySound(0x57);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
