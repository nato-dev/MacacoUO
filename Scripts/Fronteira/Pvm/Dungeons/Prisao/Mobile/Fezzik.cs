using System;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ogre corpse")]
    public class Fezzik : BaseOgre
    {
        private DateTime m_StinkingCauldronTime;

        public static bool DROP_RECEITA_POWER = false;

        [Constructable]
        public Fezzik()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Fezzik";
            this.Title = "o Ogro Cozinheiro";
            this.Body = 1;
            this.BaseSoundID = 427;

            this.SetHits(2000, 2000);
            this.SetStr(1142, 1381);
            this.SetDex(73, 90);
            this.SetInt(52, 84);

            this.SetMana(0);

            this.SetDamage(25, 30);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 75, 80);
            this.SetResistance(ResistanceType.Fire, 70, 75);
            this.SetResistance(ResistanceType.Cold, 65, 75);
            this.SetResistance(ResistanceType.Poison, 55, 65);
            this.SetResistance(ResistanceType.Energy, 65, 75);

            this.SetSkill(SkillName.MagicResist, 133.3, 151.9);
            this.SetSkill(SkillName.Tactics, 120.3, 130.0);
            this.SetSkill(SkillName.Wrestling, 122.2, 128.9);
            this.SetSkill(SkillName.Anatomy, 10.0, 15.0);
            this.SetSkill(SkillName.DetectHidden, 90.0);
            this.SetSkill(SkillName.Parry, 95.0, 100.0);

            this.Fame = 3000;
            this.Karma = -3000;

            this.VirtualArmor = 52;

            // this.PackItem(DefCookingExp.GetReceitaRandom());

            this.PackItem(DefCookingExp.GetReceitaIgredienteRandom());

          

            this.PackItem(new Club());
        }

        public Fezzik(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 2; } }

        public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
            if (0.5 >= Utility.RandomDouble())
                this.SpawnGreenGoo();
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (0.5 >= Utility.RandomDouble())
                this.SpawnGreenGoo();
        }

        public void SpawnGreenGoo()
        {
            if (this.m_StinkingCauldronTime <= DateTime.UtcNow)
            {
                new StinkingCauldron().MoveToWorld(this.Location, this.Map);

                this.m_StinkingCauldronTime = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            SorteiaItem(DefCookingExp.GetReceitaRandom());
            SorteiaItem(DefCookingExp.GetReceitaRandom());
            var receita = DefCookingExp.Basicas[Utility.Random(DefCookingExp.Basicas.Length)];
            SorteiaItem(new RecipeScroll((int)receita));
            var receita2 = DefCookingExp.Basicas[Utility.Random(DefCookingExp.Basicas.Length)];
            SorteiaItem(new RecipeScroll((int)receita2));
            DistribuiItem(AnimatedSeed.GetRandomSeed());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV4, 2);
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
