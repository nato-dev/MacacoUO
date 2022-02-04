using Server;
using System;
using Server.Items;
using Server.Mobiles;
using Server.Services.TownCryer;
using Server.Engines.Points;

namespace Server.Engines.Quests
{

    public class SacolaJoiaRara : Bag
    {
        [Constructable]
        public SacolaJoiaRara()
        {
            var item = Loot.RandomJewelry(false);
            RunicReforging.GenerateRandomItem(item, 0, 0, 0, this.Map);
            this.AddItem(item);
            Hue = 55;
            Name = "Sacola";
        }

        public SacolaJoiaRara(Serial s) : base(s) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    public class RightingWrongQuest : BaseQuest
    {

        public override object Title { get { return "(Repetivel) A Prisao"; } }

        public override object Description { get { return @"A situacao das prisoes de rhodes estao indo mal. Tentamos isolar ela da cidade mas agora ela foi tomada por monstros. Talvez voce queira ajudar ? O Reino de Rhodes agradece !
  Para chegar na prisao, siga a estrada  e vire a segunda a esquerda, tem uma caverna em uma montanha la que antigamente eram mantidos os prisioneiros."; } }

 
        public override object Refuse { get { return "Okay"; } }

        public override object Uncomplete { get { return "Encontre o guarda Arnoldo na porta da Prisao. Para chegar na prisao, siga a estrada a leste de Rhodes e vire a segunda a esquerda, tem uma caverna em uma montanha la"; } }

        public override object Complete { get { return "Voce trouxe justica a prisao ! Muito obrigado !"; } }

        public override int AcceptSound { get { return 0x2E8; } }
        public override bool DoneOnce { get { return true; } }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 200);
            base.OnCompleted();
        }

        public RightingWrongQuest()
        {
            AddObjective(new InternalObjective());

            AddReward(new BaseReward(1158153)); // A unique opportunity with the Royal Britannian Guard
        }

        public override void GiveRewards()
        {
            base.GiveRewards();

            QuestHelper.DoneQuest(Owner, typeof(RightingWrongQuest), RestartDelay);
        }

        private class InternalObjective : BaseObjective
        {
            public override object ObjectiveDescription { get { return Quest.Uncomplete; } }

            public InternalObjective()
                : base(1)
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

    public class RightingWrongQuest2 : BaseQuest
    {
        public override QuestChain ChainID { get { return QuestChain.RightingWrong; } }
        public override Type NextQuest { get { return typeof(RightingWrongQuest3); } }
        
        public override object Title { get { return "(Repetivel) A Prisao"; } }

        public override object Description { get { return "Outro guarda ? Bah. Voce deveria saber que aqui tem muitos horrores.. Os homens lagarto tomaram conta da prisao. Mate as criaturas !"; } }

       
        public override object Refuse { get { return "Certo...medroso...cagao..."; } }

        public override object Uncomplete { get { return "Mate as criaturas da prisao"; } }

        public override object Complete { get { return "Voce nao eh tao inutil quanto parece..."; } }

        public override int AcceptSound { get { return 0x2E8; } }
        public override bool DoneOnce { get { return true; } }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 200);
            base.OnCompleted();
        }


        public RightingWrongQuest2()
        {
            AddObjective(new SlayObjective(typeof(LizardmanDefender), "homens lagarto defensores", 5));
            AddObjective(new SlayObjective(typeof(LizardmanSquatter), "homens lagartos agressivos", 5));
            AddObjective(new SlayObjective(typeof(CaveTrollWrong), "trolls da caverna", 5));
            AddObjective(new SlayObjective(typeof(HungryOgre), "ogros faminto", 5));
        }
    }

    public class RightingWrongQuest3 : BaseQuest
    {
        public override QuestChain ChainID { get { return QuestChain.RightingWrong; } }
        public override Type NextQuest { get { return typeof(RightingWrongQuest4); } }

        public override object Title { get { return "(Repetivel) A Prisao"; } }

   
        public override object Description { get { return @"Voce eh forte hein ? Mas ei, acredita que um ogro cozinheiro tomou conta da nossa prisao? Quando fomos tentar remove-lo le arremecou um pecado enorme de bacon nos guardas...
Podia dar um fim a este ogro cozinheiro e ter certeza que ele coma sua ultima refeicao ?"; } }

        public override object Refuse { get { return "Ta com medo de um cozinheiro cara ?"; } }

        public override object Uncomplete { get { return "Entre na prisao e mate o ogro cozinheiro"; } }

      
        public override object Complete { get { return @"Parece que o ensopado de ogro nunca mais sera servidor - hah.
Seus servicos sao muito bons. Se ainda quer continuar, um de nossos camaradas foi pego pelo carcereiro maldito. Ele fica la dentro da prisao !"; } }

        public override int AcceptSound { get { return 0x2E8; } }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 500);
            base.OnCompleted();
        }


        public RightingWrongQuest3()
        {
            AddObjective(new SlayObjective(typeof(Fezzik), "fezzik o ogro cozinheiro", 1));
        }
    }

    public class RightingWrongQuest4 : BaseQuest
    {
        public override QuestChain ChainID { get { return QuestChain.RightingWrong; } }

        private RoyalBritannianGuardOrders Orders { get; set; }

  
        public override object Title { get { return "(Repetivel) A Prisao"; } }

        public override object Description { get { return @"Parece que o ensopado de ogro nunca mais sera servidor - hah.
Seus servicos sao muito bons.Se ainda quer continuar, um de nossos camaradas foi pego pelo carcereiro maldito. Ele fica la dentro da prisao !"; } }

        public override object Refuse { get { return "Ta com medinho ?"; } }


        public override object Uncomplete { get { return "Va a Prisao e mate o Carcereiro Maldito"; } }

        public override object Complete { get { return "Voce fez justica ! Muito bem !"; } }

        public override int AcceptSound { get { return 0x2E8; } }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 800);
            base.OnCompleted();
        }


        public RightingWrongQuest4()
        {
            AddObjective(new InternalObjective());

            AddReward(new BaseReward(typeof(Gold), 3000, "3000 Moedas de Ouro"));
            AddReward(new BaseReward(typeof(SacolaJoiaRara), 1, "Joia Rara"));
            AddReward(new BaseReward(typeof(RightingWrongRewardTitleDeed), "Honraria a Guarda de Rhodes")); // A Unique Honor from the Royal Britannian Guard
        }

        public override void OnAccept()
        {
            base.OnAccept();

            
            Orders = new RoyalBritannianGuardOrders();
            Owner.Backpack.DropItem(Orders);

            Owner.SendLocalizedMessage("Vore recebeu um item de missao"); // You received a Quest Item!
            
        }

        public void CompleteQuest()
        {
            //TownCryerSystem.CompleteQuest(Owner, new RightingWrongQuest());

            OnCompleted();
            GiveRewards();

            QuestHelper.DoneQuest(Owner, typeof(RightingWrongQuest2), RestartDelay);
        }

        public override void RemoveQuest(bool removeChain)
        {
            base.RemoveQuest(removeChain);

            if (Orders != null && !Orders.Deleted)
            {
                Orders.Delete();
            }
        }

        private class InternalObjective : BaseObjective
        {
            public override object ObjectiveDescription { get { return 1158164; } } // Find the fallen Guard's corpse and escape the prison.

            public InternalObjective()
                : base(1)
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

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(Orders);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Orders = reader.ReadItem() as RoyalBritannianGuardOrders;
        }
    }

    public class Arnold : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RightingWrongQuest2) }; } }

        public static void Initialize()
        {
          
        }

        [Constructable]
        public Arnold()
            : base("Arnoldo", "o Guarda da Prisao")
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            CantWalk = true;

            Body = 0x190;
            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C;
            FacialHairItemID = 0x204C;

            HairHue = 0x8A8;
            FacialHairHue = 0x8A8;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());

            SetWearable(new ChainChest());
            SetWearable(new ThighBoots());
            SetWearable(new BodySash(), 1157);
            SetWearable(new Epaulette(), 1157);
            SetWearable(new ChaosShield());
            SetWearable(new Broadsword());
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (m is PlayerMobile && m.InRange(Location, 5))
            {
                RightingWrongQuest4 quest = QuestHelper.GetQuest<RightingWrongQuest4>((PlayerMobile)m);

                if (quest != null && quest.Completed)
                {
                    quest.CompleteQuest();
                }
                else
                {
                    base.OnDoubleClick(m);
                }
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m is PlayerMobile && InRange(m.Location, 5) && !InRange(oldLocation, 5))
            {
                RightingWrongQuest quest = QuestHelper.GetQuest<RightingWrongQuest>((PlayerMobile)m);

                if (quest != null)
                {
                    quest.OnCompleted();
                    quest.GiveRewards();
                }
            }
        }

        public Arnold(Serial serial)
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

    public class Guarda1 : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RightingWrongQuest) }; } }

        public static void Initialize()
        {
        }

        [Constructable]
        public Guarda1()
            : base("Guarda", "")
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            CantWalk = true;

            Body = 0x190;
            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C;
            FacialHairItemID = 0x204C;

            HairHue = 0x8A8;
            FacialHairHue = 0x8A8;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());

            SetWearable(new ChainChest());
            SetWearable(new ThighBoots());
            SetWearable(new BodySash(), 1157);
            SetWearable(new Epaulette(), 1157);
            SetWearable(new ChaosShield());
            SetWearable(new Broadsword());
        }

        public Guarda1(Serial serial)
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
