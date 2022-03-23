using System;
using Server.Fronteira.Elementos;
using Server.Items;
using Server.Items.Functional.Pergaminhos;

namespace Server.Mobiles
{
    [CorpseName("a shimmering effusion corpse")]
    public class ShimmeringEffusion : BasePeerless
    {
        [Constructable]
        public ShimmeringEffusion()
            : base(AIType.AI_Spellweaving, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Foco Energetico Supremo";
            Body = 0x105;

            SetStr(1000, 1000);
            SetDex(50, 60);
            SetInt(1500, 1600);

            SetHits(30000);

            SetDamage(37, 45);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Wrestling, 100.0, 105.0);
            SetSkill(SkillName.Tactics, 100.0, 105.0);
            SetSkill(SkillName.MagicResist, 150);
            SetSkill(SkillName.Magery, 150.0);
            SetSkill(SkillName.EvalInt, 150.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Spellweaving, 120.0);

            Fame = 30000;
            Karma = -30000;

            PackResources(8);
            //AddItem(new TemplateDeed());
            //PackTalismans(5);

            //for (int i = 0; i < Utility.RandomMinMax(1, 6); i++)
            // {
            //     PackItem(Loot.RandomScroll(0, Loot.ArcanistScrollTypes.Length, SpellbookType.Arcanist));
            // }
            SetWeaponAbility(WeaponAbility.ArmorIgnore);
            SetSpecialAbility(SpecialAbility.StickySkin);
        }

        public virtual int BonusExp => 800;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LV7, 8);
            AddLoot(LootPack.Parrot, 2);
            this.AddLoot(LootPack.Gems, 40);
            AddLoot(LootPack.HighScrolls, 3);
            AddLoot(LootPack.MedScrolls, 3);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            SorteiaItem(BaseEssencia.RandomEssencia());
            SorteiaItem(new ShimmeringCrystals());
            SorteiaItem(new ElvenForgeDeed());
            for (var x = 0; x < 20; x++)
            {
                SorteiaItem(BaseEssencia.RandomEssencia());
                SorteiaItem(ElementoUtils.GetRandomPedraSuperior());
            }

            switch (Utility.Random(4))
            {
                case 0:
                    SorteiaItem(new ShimmeringEffusionStatuette());
                    break;
                case 1:
                    SorteiaItem(new CorporealBrumeStatuette());
                    break;
                case 2:
                    SorteiaItem(new MantraEffervescenceStatuette());
                    break;
                case 3:
                    SorteiaItem(new FetidEssenceStatuette());
                    break;
            }

            if (Utility.RandomDouble() < 0.05)
                SorteiaItem(new FerretImprisonedInCrystal());

            if (Utility.RandomDouble() < 0.025)
                SorteiaItem(new CrystallineRing());

          
            var livro = new Spellbook();
            livro.Slayer = BaseRunicTool.GetRandomSlayer();
            livro.Hue = 1172;
            livro.Name = "Livro com Foco de Energia Magica";
            livro.EngravedText = "Wololo";
            SorteiaItem(livro);
        }

        public override bool AutoDispel
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
                return 5;
            }
        }
        public override bool HasFireRing
        {
            get
            {
                return true;
            }
        }
        public override double FireRingChance
        {
            get
            {
                return 0.1;
            }
        }

        public override int GetIdleSound()
        {
            return 0x1BF;
        }

        public override int GetAttackSound()
        {
            return 0x1C0;
        }

        public override int GetHurtSound()
        {
            return 0x1C1;
        }

        public override int GetDeathSound()
        {
            return 0x1C2;
        }

        #region Helpers
        public override bool CanSpawnHelpers
        {
            get
            {
                return true;
            }
        }
        public override int MaxHelpersWaves
        {
            get
            {
                return 4;
            }
        }
        public override double SpawnHelpersChance
        {
            get
            {
                return 0.1;
            }
        }

        public override void SpawnHelpers()
        {
            int amount = 1;

            if (Altar != null)
                amount = Altar.Fighters.Count;

            if (amount > 5)
                amount = 5;

            for (int i = 0; i < amount; i++)
            {
                switch (Utility.Random(3))
                {
                    case 0:
                        SpawnHelper(new MantraEffervescence(), 2);
                        break;
                    case 1:
                        SpawnHelper(new CorporealBrume(), 2);
                        break;
                    case 2:
                        SpawnHelper(new FetidEssence(), 2);
                        break;
                }
            }
        }

        #endregion

        public ShimmeringEffusion(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
