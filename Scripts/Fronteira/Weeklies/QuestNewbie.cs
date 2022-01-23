using System;
using System.Collections.Generic;
using Server.Engines.Points;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class QuestNewbie : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title
        {
            get
            {
                return "[Aleatoria]Pergaminho da Bencao";
            }
        }
        public override object Description
        {
            get
            {
                return "Ola viajante. Ja pensou em poder ter roupas e runebooks que nao se perdem ao morrer? Posso cria-los ! <br> Basta cumprir uma missao longa e desafiradora.<br>A missao eh aleatoria!";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Entendo";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Termine a longa e desafiante missao para obter seu pergaminho !";
            }
        }
        public override object Complete
        {
            get
            {
                return "Muito bem, receba aqui entao seu pergaminho !";
            }
        }

        public Combinacao[] Possiveis = new Combinacao[] {
            new Combinacao("Aranha do Gelo", typeof(FrostSpider), 300),
            new Combinacao("Lagarto de Fogo", typeof(LavaLizard), 300),
            new Combinacao("Elemental do Fogo", typeof(FireElemental), 300),
            new Combinacao("Elemental da Agua", typeof(WaterElemental), 300),
            new Combinacao("Elemental do Fogo", typeof(Daemon), 200),
            new Combinacao("Elemental do Fogo", typeof(Gargoyle), 200),
            new Combinacao("Elemental do Fogo", typeof(Lich), 150),
            new Combinacao("Elemental do Fogo", typeof(Dragon), 50),
        };

        public class Combinacao
        {
            public Combinacao(String n, Type t, int q)
            {
                Monstro = t;
                qtd = q;
                this.n = n;
            }
            public Type Monstro;
            public int qtd;
            public String n;

            public BaseObjective GetObj()
            {
                return new SlayObjective(Monstro, n, qtd);
            }
        }

        public int DiaAtual;
        public List<BaseObjective> Objs = new List<BaseObjective>();

        public QuestNewbie()
            : base()
        {

            var dia = (int)Math.Round(TimeSpan.FromTicks(DateTime.Now.Ticks).TotalDays);
            if(dia != DiaAtual)
            {
                DiaAtual = dia;
                Objs.Clear();
                var r = new List<Combinacao>(Possiveis);
                var random = r[Utility.Random(r.Count)];
                r.Remove(random);
                //Objs.Add(random);

                random = r[Utility.Random(r.Count)];
                r.Remove(random);
                //Objs.Add(random);

            }

            this.AddObjective(new SlayObjective(typeof(Orc), "Orcs", 50));

            
            this.AddReward(new BaseReward(typeof(OrcishKinMask), 1, "Mascara"));
            this.AddReward(new BaseReward(typeof(Gold), 500, "500 Moedas"));
            this.AddReward(new BaseReward(typeof(CottonSeeds), 10, "10 Sementes de Algodao"));
        }

        public override void OnCompleted()
        {
            // AQUI VC BOTA QUANTO DE EXP VAI DAR A QUEST
            PointsSystem.Exp.AwardPoints(this.Owner, 300);
            this.Owner.PlaySound(this.CompleteSound);
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

    //// AQUI EH A CLASSE DO NPC Q VAI DAR A QUETS ///   
    public class QuestGiverNoob : MondainQuester
    {
        /// AQUI REGISTRA QUAL QUEST ELE VAI DAR 
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(MatarOrcs)
        };
            }
        }


        [Constructable]
        public QuestGiverNoob()
            : base("Helton", "O Fazendeiro Hermitao")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public QuestGiverNoob(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Por favor, me ajudem ! Orcs estao acabando com minha fazenda !");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
            this.Female = false;
            this.CantWalk = true;
            this.Race = Race.Human;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            switch (Utility.Random(3))
            {
                case 0:
                    SetWearable(new FancyShirt(GetRandomHue()));
                    break;
                case 1:
                    SetWearable(new Doublet(GetRandomHue()));
                    break;
                case 2:
                    SetWearable(new Shirt(GetRandomHue()));
                    break;
            }

            SetWearable(new Shoes(GetShoeHue()));
            int hairHue = GetHairHue();

            Utility.AssignRandomHair(this, hairHue);
            Utility.AssignRandomFacialHair(this, hairHue);

            if (Body == 0x191)
            {
                FacialHairItemID = 0;
            }

            if (Body == 0x191)
            {
                switch (Utility.Random(6))
                {
                    case 0:
                        SetWearable(new ShortPants(GetRandomHue()));
                        break;
                    case 1:
                    case 2:
                        SetWearable(new Kilt(GetRandomHue()));
                        break;
                    case 3:
                    case 4:
                    case 5:
                        SetWearable(new Skirt(GetRandomHue()));
                        break;
                }
            }
            else
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        SetWearable(new LongPants(GetRandomHue()));
                        break;
                    case 1:
                        SetWearable(new ShortPants(GetRandomHue()));
                        break;
                }
            }

            if (!Siege.SiegeShard)
                PackGold(100, 200);
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
