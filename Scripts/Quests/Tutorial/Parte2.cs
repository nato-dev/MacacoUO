using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class QuestInicial2 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        //public override QuestChain ChainID => QuestChain.Tutorial;

        //public override Type NextQuest =>  typeof(CloseEnoughQuest)

        public override object Title
        {
            get
            {
                return "A importancia de acampamentos";
            }
        }
        public override object Description
        {
            get
            {
                return "Ola viajante. Se seu desejo e explorar o mundo, lhe recomendo treinar habilidades em acampamento pois sao muito uteis, voce <b><i>pode ir rapidamente de um local a outro</i></b> Treine um pouco suas skills de acampamento que lhe darei um premio.";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Ah uma pena";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Vejo que retornou, mas ainda acho que nao treinou sua skill em acampamentos suficiente (50) !";
            }
        }
        public override object Complete
        {
            get
            {
                return "Ah voce conseguiu muito bem ! Voce pode explorar o mundo e salvar pontos para acampar. Procure acampamentos de curandeiros e faca uma fogueira proxima.";
            }
        }

        public QuestInicial2()
            : base()
        {
            this.AddObjective(new ApprenticeObjective(SkillName.Camping, 60));
            this.AddReward(new BaseReward(typeof(Gold), 1000, "Moedas de Ouro"));
            this.AddReward(new BaseReward(typeof(Kindling), 100, null));
        }

        public override void OnCompleted()
        {
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

    public class NPC2 : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(QuestInicial2)
        };
            }
        }


        [Constructable]
        public NPC2()
            : base("Nilton o Campista", "Grande Mestre Campista")
        {
            this.SetSkill(SkillName.Tracking, 120.0, 120.0);
            this.SetSkill(SkillName.Camping, 120.0, 120.0);
        }

        public NPC2(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Voce sabia que skill de acampar eh otima para explorar ? Quem acampa tambem pode carregar mais peso !");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
            this.Female = true;
            this.CantWalk = true;
            this.Race = Race.Human;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            this.AddItem(new Backpack());
            this.AddItem(new LeatherLegs());
            this.AddItem(new ThighBoots());
            this.AddItem(new FemaleLeatherChest());
            this.AddItem(new StuddedGloves());
            this.AddItem(new LeatherNinjaBelt());
            this.AddItem(new StuddedGorget());
            this.AddItem(new LightPlateJingasa());
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
