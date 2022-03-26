using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an earth elemental corpse")]
    public class EarthElemental : BaseCreature
    {
        [Constructable]
        public EarthElemental()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "elemental da terra";
            this.Body = 14;
            this.BaseSoundID = 268;

            this.SetStr(126, 155);
            this.SetDex(66, 85);
            this.SetInt(71, 92);

            this.SetHits(76, 93);

            this.SetDamage(9, 12);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 30, 35);
            this.SetResistance(ResistanceType.Fire, 10, 20);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 15, 25);
            this.SetResistance(ResistanceType.Energy, 15, 25);

            this.SetSkill(SkillName.MagicResist, 50.1, 95.0);
            this.SetSkill(SkillName.Tactics, 60.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 100.0);
            this.SetSkill(SkillName.Parry, 10.1, 11);

            this.Fame = 3500;
            this.Karma = -3500;

            this.VirtualArmor = 80;
            this.ControlSlots = 2;

            this.PackItem(new FertileDirt(Utility.RandomMinMax(1, 4)));
            this.PackItem(new MandrakeRoot());

            Item ore = new IronOre(5);
            ore.ItemID = 0x19B7;
            this.PackItem(ore);
        }


        public EarthElemental(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            if(this.Combatant is Mobile && !IsCooldown("pula"))
            {
                this.PlayAngerSound();
                OverheadMessage("* enterrando *");
                SetCooldown("pula", TimeSpan.FromSeconds(30));
                new TerraTimer(this, this.Combatant as Mobile, 0.09).Start();
            }
        }

        public override double DispelDifficulty
        {
            get
            {
                return 117.5;
            }
        }

        public override double DispelFocus
        {
            get
            {
                return 45.0;
            }
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
                return 1;
            }
        }

        public class TerraTimer : Timer {

            BaseCreature bc;
            bool desce;
            Mobile alvo;
            int ct = 0;
            Rectangle2D rect;

            public TerraTimer(BaseCreature c, Mobile alvo, double tempo = 0.06) : base(TimeSpan.FromSeconds(tempo), TimeSpan.FromSeconds(tempo), 20)
            {
                this.bc = c;
                this.alvo = alvo;
                rect = new Rectangle2D(c.X - 1, c.Y - 1, 3, 3);
               
            }

            protected override void OnTick()
            {
                if (bc == null)
                    return;

                if (!bc.Alive && bc.Corpse != null && ct != 666)
                {
                    if (ct > 10)
                        ct /= 2;
                    bc.Corpse.MoveToWorld(new Point3D(bc.Location.X, bc.Location.Y, bc.Location.Z + 2 * ct), bc.Map);
                    ct = 666;
                    return;
                }

                if (bc.Deleted || ct == 666)
                    return;

                ct++;
                if (ct < 10)
                {
                    bc.MoveToWorld(new Point3D(bc.Location.X, bc.Location.Y, bc.Location.Z-2), bc.Map);
                } else if (ct == 10) {
                    if(alvo.Alive)
                    {
                        bc.MoveToWorld(new Point3D(alvo.X, alvo.Location.Y, alvo.Location.Z - 20), alvo.Map);
                        rect = new Rectangle2D(bc.X - 1, bc.Y - 1, 3, 3);
                    }
                } else if (ct < 20)
                {
                    bc.MoveToWorld(new Point3D(bc.Location.X, bc.Location.Y, bc.Location.Z+2), bc.Map);
                }
                else if (ct == 20)
                {
                    bc.Combatant = alvo;
                    bc.PlayAngerSound();
                }
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV1);
            this.AddLoot(LootPack.LV2);
            if(Utility.RandomBool())
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
