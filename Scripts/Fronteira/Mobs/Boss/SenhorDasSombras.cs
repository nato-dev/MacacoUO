using Server.Items;
using Server.Ziden;
using Server.Ziden.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using Server.Ziden.Achievements;
using Server.Items.Functional.Pergaminhos;
using Server.Fronteira.Elementos;
using Server.SkillHandlers;

namespace Server.Mobiles
{
    [CorpseName("a senhor das sombras corpse")]
    public class SenhorDasSombras : BaseCreature
    {
        public override bool IsBoss => true;
        public override int BonusExp => 1000;

        [Constructable]
        public SenhorDasSombras()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 2, 0.6, 0.8)
        {
            Name = "Senhor Das Sombras";
            Body = 311;
            Hue = 2071;
            BaseSoundID = 0x47E;

            SetStr(300, 310);
            SetDex(81, 90);
            SetInt(200, 210);

            SetHits(36000, 36000);

            SetDamage(15, 30);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 20);
            SetDamageType(ResistanceType.Poison, 0);

            SetResistance(ResistanceType.Physical, 0, 0);
            SetResistance(ResistanceType.Fire, 0, 0);
            SetResistance(ResistanceType.Cold, 0, 0);
            SetResistance(ResistanceType.Poison, 0, 0);
            SetResistance(ResistanceType.Energy, 0, 0);

            SetSkill(SkillName.EvalInt, 80);
            SetSkill(SkillName.Magery, 90, 100);
            SetSkill(SkillName.SpiritSpeak, 200);
            SetSkill(SkillName.Necromancy, 50, 60);
            SetSkill(SkillName.Poisoning, 0);
            SetSkill(SkillName.Meditation, 100);
            SetSkill(SkillName.MagicResist, 0, 0);
            SetSkill(SkillName.Tactics, 100);
            SetSkill(SkillName.Wrestling, 82, 88);
            SetSkill(SkillName.Hiding, 120, 140);
            SetSkill(SkillName.Stealth, 150, 180);

            Fame = 26000;
            Karma = -26000;

            VirtualArmor = 0;

            Tamable = false;

            SetWeaponAbility(WeaponAbility.MortalStrike);
            SetWeaponAbility(WeaponAbility.WhirlwindAttack);
            SetWeaponAbility(WeaponAbility.ShadowStrike);
        }

        public SenhorDasSombras(Serial serial)
            : base(serial)
        {
        }               

        public override bool IgnoreYoungProtection
        {
            get
            {
                return Core.ML;
            }
        }
        public override bool BardImmune
        {
            get
            {
                return !Core.SE;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return Core.SE;
            }
        }
        public override bool AreaPeaceImmune
        {
            get
            {
                return Core.SE;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lesser;
            }
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            base.AlterMeleeDamageFrom(from, ref damage);
            if (from is BaseCreature)
                damage /= 3;
        }

        public override int GetIdleSound() { return 0x47F; }
        public override int GetAngerSound() { return 0x482; }
        public override int GetDeathSound() { return 0x167; }

        

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV6, 3);
            AddLoot(LootPack.Gems, 30);
        }

        public override void OnDeath(Container c)
        {
            var tinta = new DyeTub();
            tinta.DyedHue = 1910;
            tinta.Name = "Tinta do Senhor das Sombras";

            base.OnDeath(c);
            c.DropItem(new Gold(2000));
            DistribuiPs(105);
            if(Utility.RandomDouble() < 0.1)
                SorteiaItem(Carnage.GetRandomPS(110));
            SorteiaItem(tinta);
            SorteiaItem(new PergaminhoCarregamento());            
            SorteiaItem(new SkillBook());
            SorteiaItem(new CristalDoPoder() { Amount = 20 });
            //SorteiaItem(new TemplateDeed());
            SorteiaItem(new LivroAntigo());
            SorteiaItem(Decos.RandomDeco(this));
            for(var x = 0; x < 3; x++)
                SorteiaItem(ElementoUtils.GetRandomPedraSuperior(3));
            GolemMecanico.JorraOuro(c.Location, c.Map, 550);
        }        

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);
            if (from != this)
            {
                var rnd = Utility.RandomDouble();
                if (from != this && rnd < 0.4)
                {
                    this.Combatant = from;
                    this.OverheadMessage("* Suas almas serao minhas *");
                    this.PlaySound(0x1AD);
                }

                if (rnd < 0.5)
                {
                    var sombra = new Wraith();
                    sombra.MoveToWorld(from.Location, from.Map);
                    if (from != this)
                    sombra.Combatant = from;
                    sombra.OverheadMessage("* mwahahaha *");
                    from.PlaySound(0x487);
                    from.SendMessage("Um ser sai das sombras te atacando");
                }                
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
