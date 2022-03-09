using System;
using System.Collections.Generic;
using Server.Engines.Points;
using Server.Engines.Quests;
using Server.Items;
using Server.Mobiles;

namespace Server.Fronteira.Weeklies
{
    public class Weekly : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title
        {
            get
            {
                return "Desafio Semanal";
            }
        }
        public override object Description
        {
            get
            {
                return "Desafio Semanal: Mate monstros e colete items para completar a missao e ganhar recompensas ! </br></br>Recompensa em Exp: 2500</br>Ouro: 10000</br>Recompensas: Pergaminho +1 Item na bag, Pergaminho +5 Peso na Bag, Pergaminho +1 Skillcap";
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
                return "Muito bem, receba aqui entao suas recompensas !";
            }
        }

        public CombinacaoKill[] PossiveisMonstros = new CombinacaoKill[] {
            new CombinacaoKill("Aranha do Gelo", typeof(FrostSpider), 300),
            new CombinacaoKill("Lagarto de Fogo", typeof(LavaLizard), 300),
            new CombinacaoKill("Elemental do Fogo", typeof(FireElemental), 300),
            new CombinacaoKill("Elemental da Agua", typeof(WaterElemental), 300),
            new CombinacaoKill("Demonio ", typeof(Daemon), 400),
            new CombinacaoKill("Gargula", typeof(Gargoyle), 300),
            new CombinacaoKill("Lich", typeof(Lich), 150),
            new CombinacaoKill("Dragao", typeof(Dragon), 50),
            new CombinacaoKill("Caum", typeof(HellHound), 300),
            new CombinacaoKill("Caum", typeof(HellHound), 300),
        };

        public class CombinacaoKill
        {
            public CombinacaoKill(String n, Type t, int q)
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

        public Weekly()
            : base()
        {
            var semana = GetSemana();
            if(semana != SaveWeekly.SEMANA_ATUAL)
            {
                var r = new List<CombinacaoKill>(PossiveisMonstros);
                var random = r[Utility.Random(r.Count)];
                r.Remove(random);
                var random2 = r[Utility.Random(r.Count)];
                SaveWeekly.Kills.Add(random);
                SaveWeekly.Kills.Add(random2);
                SaveWeekly.SEMANA_ATUAL = semana;
            }

            foreach(var obj in SaveWeekly.Kills)
            {
                this.AddObjective(obj.GetObj());
            }
        }

        public static int DiasFaltam()
        {
            return (int)(TimeSpan.FromTicks(DateTime.Now.Ticks).TotalDays % 7);
        }

        public static int GetSemana()
        {
            return (int)Math.Ceiling(TimeSpan.FromTicks(DateTime.Now.Ticks).TotalDays / 7);
        }

        public override bool CanOffer()
        {
            return base.CanOffer();
        }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 2500);
            this.Owner.PlaySound(this.CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(SaveWeekly.SEMANA_ATUAL);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            var semana = reader.ReadInt();
            if(semana != GetSemana())
            {
                OnResign(true);
            }
        }
    }

    //// AQUI EH A CLASSE DO NPC Q VAI DAR A QUETS ///   
    public class QuestGiverSemanal : MondainQuester
    {
        /// AQUI REGISTRA QUAL QUEST ELE VAI DAR 
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(Weekly)
        };
            }
        }


        [Constructable]
        public QuestGiverSemanal()
            : base("Semanal", "O Recompensador")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public QuestGiverSemanal(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Fale comigo para fazer a missao semanal !");  // Know yourself, and you will become a true warrior.
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

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
