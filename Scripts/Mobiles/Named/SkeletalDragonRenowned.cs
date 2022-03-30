using System;
using Server.Fronteira.Elementos;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Skeletal Dragon [Renowned] corpse")]  
    public class SkeletalDragonRenowned : BaseRenowned
    {

       // public override bool IsBoss => true;

        [Constructable]
        public SkeletalDragonRenowned()
            : base(AIType.AI_NecroMage)
        {
            this.Name = "Dragao Esqueleto";
            this.Body = 104;
            this.BaseSoundID = 0x488;

            this.Hue = 906;

            this.SetStr(898, 1030);
            this.SetDex(100, 200);
            this.SetInt(488, 620);

            this.SetHits(10558, 10599);

            this.SetDamage(29, 35);

            this.SetDamageType(ResistanceType.Physical, 75);
            this.SetDamageType(ResistanceType.Fire, 25);

            this.SetResistance(ResistanceType.Physical, 75, 80);
            this.SetResistance(ResistanceType.Fire, 40, 60);
            this.SetResistance(ResistanceType.Cold, 40, 60);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 60);

            this.SetSkill(SkillName.EvalInt, 80.1, 100.0);
            this.SetSkill(SkillName.Magery, 80.1, 100.0);
            this.SetSkill(SkillName.Necromancy, 80.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 100.3, 130.0);
            this.SetSkill(SkillName.Tactics, 97.6, 100.0);
            this.SetSkill(SkillName.Wrestling, 97.6, 100.0);

            this.Fame = 22500;
            this.Karma = -22500;

            this.VirtualArmor = 80;

         
        }

        public SkeletalDragonRenowned(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement
        {
            get
            {
                return true;
            }
        }

        public override bool HasBreath
        {
            get
            {
                return true;
            }
        }

        // fire breath enabled
        public override int BreathFireDamage
        {
            get
            {
                return 0;
            }
        }
        public override int BreathColdDamage
        {
            get
            {
                return 90;
            }
        }
        public override int BreathEffectHue
        {
            get
            {
                return 0x480;
            }
        }
        public override double BonusPetDamageScalar
        {
            get
            {
                return (Core.SE) ? 3.0 : 1.0;
            }
        }
        // TODO: Undead summoning?
        public override bool AutoDispel
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
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override int Meat
        {
            get
            {
                return 19;
            }
        }// where's it hiding these? :)
        public override int Hides
        {
            get
            {
                return 20;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Barbed;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            var hs = new SkeletalMount();
            hs.MoveToWorld(c.Location, c.Map);
            hs.MinTameSkill = 99;
            var q = new Aljava();
            q.Name = "Aljava de Ossos";
            q.Hue = 1154;
            SorteiaItem(q);
            SorteiaItem(new FragmentosAntigos());
            SorteiaItem(new FragmentosAntigos());
            SorteiaItem(Loot.JoiaRaraRandom(this.Map));
            SorteiaItem(Loot.JoiaRaraRandom(this.Map));
            SorteiaItem(Loot.JoiaRaraRandom(this.Map));
            SorteiaItem(Loot.JoiaRaraRandom(this.Map));
            for (var x = 0; x < 5; x++)
            {
                SorteiaItem(ElementoUtils.GetRandomPedraSuperior());
            }
            var r = Utility.Random(5);
            switch (r)
            {
                case 0: SorteiaItem(new BoneThroneDeed()); break;
                case 1: SorteiaItem(new BoneCouchDeed()); break;
                case 2: SorteiaItem(new BoneTableDeed()); break;
                case 3: SorteiaItem(new CreepyPortraitDeed()); break;
                case 4: SorteiaItem(new DisturbingPortraitDeed()); break;
            }
        }

        public override Type[] UniqueSAList { get { return new Type[] { }; } }

        public override Type[] SharedSAList { get { return new Type[] { }; } }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV4, 3);
            this.AddLoot(LootPack.Gems, 30);
        }

        public virtual int BonusExp => 500;

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
