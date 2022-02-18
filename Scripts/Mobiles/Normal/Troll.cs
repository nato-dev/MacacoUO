using Server.Items;
using System;

namespace Server.Mobiles
{
    [CorpseName("a troll corpse")]
    public class Troll : BaseCreature
    {
        [Constructable]
        public Troll()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "troll";
            this.Body = Utility.RandomList(53, 54);
            this.BaseSoundID = 461;

            this.SetStr(176, 205);
            this.SetDex(46, 65);
            this.SetInt(46, 70);

            this.SetHits(106, 123);

            this.SetDamage(8, 14);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 25, 35);
            this.SetResistance(ResistanceType.Cold, 15, 25);
            this.SetResistance(ResistanceType.Poison, 5, 15);
            this.SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Parry, 80);
            this.SetSkill(SkillName.MagicResist, 45.1, 60.0);
            this.SetSkill(SkillName.Tactics, 50.1, 70.0);
            this.SetSkill(SkillName.Wrestling, 50.1, 70.0);

            this.Fame = 3500;
            this.Karma = -3500;

            this.VirtualArmor = 40;

            Imp.Converte(this);

            if (Utility.RandomDouble() < 0.05)
            {
                var livro = new RedBook(1, false);
                livro.Title = "Poema do Troll";
                livro.Pages[0].Lines = new string[] {
                    "carregador",
                    "carre gador",
                    "carrega a dor",
                    "a dor que carrega",
                    "sem receio",
                    "teu brioco na reta",
                    "um piru sem freio"
                };
                PackItem(livro);
            }
         
        }

        public Troll(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            if (this.Combatant != null)
            {
                if (!this.IsCooldown("act"))
                {
                    this.SetCooldown("act", TimeSpan.FromSeconds(10));
                    this.OverheadMessage(falas[Utility.Random(falas.Length)]);
                }
            }
        }

        private static string[] falas = new string[]
        {
            "Puxa meu dedo", "Voce conhece o mario ?",
            "Conhece o nao nem eu ?", "Eu tava cozinhando, e prendi meu dedo na panela...",
            "Setembro Chove?", "Eu jogo RPG. Voce tem dado em casa ?"

        };

        public override bool CanRummageCorpses
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
        public override int Meat
        {
            get
            {
                return 2;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.LV3);
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
