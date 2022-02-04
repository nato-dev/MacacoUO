using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class QuestInicial1 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        //public override QuestChain ChainID => QuestChain.Tutorial;

        //public override Type NextQuest =>  typeof(CloseEnoughQuest)

        public override object Title
        {
            get
            {
                return "O Iniciante";
            }
        }
        public override object Description
        {
            get
            {
                return "Ola viajante. Veja, a <b>oeste da cidade</b> varios <b>esqueletos</b> estao causando baderna. Ali e perto da casa de minha avo, coitada dela, poderia me ajudar?";
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
                return "Vejo que retornou, mas ainda acho que tem mais esqueletos a serem mortos !";
            }
        }
        public override object Complete
        {
            get
            {
                return "Ah muito obrigado aventureiro, agora poderei montar minha rota de comercio em paz";
            }
        }

        public QuestInicial1()
            : base()
        {
            this.AddObjective(new SlayObjective(typeof(Skeleton), "esqueletos", 3));
            this.AddReward(new BaseReward(typeof(Gold), 1000, "Moedas de Ouro"));
            this.AddReward(new BaseReward(typeof(IronIngot), 100, "Lingotes de Ferro"));
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

    public class NPC1 : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(QuestInicial1)
        };
            }
        }


        [Constructable]
        public NPC1()
            : base("Maria Joana", "A Guerreira")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public NPC1(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Por favor, me ajudem ! Esqueletos estao por toda parte !");  // Know yourself, and you will become a true warrior.
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
