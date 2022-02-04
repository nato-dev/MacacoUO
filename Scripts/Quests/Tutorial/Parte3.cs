using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class QuestInicial3 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        //public override QuestChain ChainID => QuestChain.Tutorial;

        //public override Type NextQuest =>  typeof(CloseEnoughQuest)

        public override object Title
        {
            get
            {
                return "Falando com os Mortos";
            }
        }
        public override object Description
        {
            get
            {
                return "Voce sabia que voce pode usar a skill SpiritSpeak para recuperar vida usando mana ou corpos ? <br> Alem disso, a skill fortalece as criaturas que voce pode conjurar com magias.<br> Upe a skill ate 60 que lhe darei um premio.";
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
                return "Vejo que retornou, mas ainda acho que nao treinou sua skill em falar com mortos suficiente (60) !";
            }
        }
        public override object Complete
        {
            get
            {
                return "Ah voce conseguiu muito bem !";
            }
        }

        public QuestInicial3()
            : base()
        {
            this.AddObjective(new ApprenticeObjective(SkillName.SpiritSpeak, 60));
            this.AddReward(new BaseReward(typeof(Gold), 1000, "Moedas de Ouro"));
            this.AddReward(new BaseReward(typeof(BladeSpiritsScroll), 1, null));
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

    public class NPC3 : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(QuestInicial3)
        };
            }
        }


        [Constructable]
        public NPC3()
            : base("Oshuai o Medium", "Grande Mestre Necromante")
        {
            this.SetSkill(SkillName.SpiritSpeak, 120.0, 120.0);
            this.SetSkill(SkillName.Magery, 120.0, 120.0);
        }

        public NPC3(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Voce sabia que falar com os mortos pode ser mais util do que parece ?");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
 
            this.CantWalk = true;
            this.Race = Race.Human;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            this.AddItem(new Backpack());
            this.AddItem(new Boots());
            var robe = new Robe();
            robe.Hue = 1175;
            this.AddItem(robe);
            this.AddItem(new BlackStaff());
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
