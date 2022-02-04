using System;
using Server.Items;
using Server.Mobiles;
using Server.Ziden;
using Server.Ziden.Dungeons.Goblins.Quest;
using Server.Ziden.Items;
using Shrink.ShrinkSystem;

namespace Server.Engines.Quests
{
    public class QuestExodo : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        //public override QuestChain ChainID => QuestChain.Tutorial;

        //public override Type NextQuest =>  typeof(CloseEnoughQuest)

        public override object Title
        {
            get
            {
                return "Exodo";
            }
        }
        public override object Description
        {
            get
            {
                return @"Meu jovem, teria um tempo para uma historia ? <br><br>
A muito tempo, muito mesmo, um mago chamado Ziro se aprofundou em magias de invocacao. Seu conhecimento era impar, esse mago ganhou renome 
em toda ilha por suas habilidades. Porem, com grande poder, sempre vem grande responsabilidade - e tal mago comecou a perder sua cabeca e a se
comunicar diretamente com o submundo. O perigo que esta pessoa colocava em todos era muito, portanto os 7 Sabios decidiram que tal mago deveria ser exilado, e assim foi.
 Seu exilio fora no outro mundo o qual ele tanto se dedicou , e um altar, a mao do exodo, foi colocado para proteger deste voltar.<br><br>
Problema que acontece eh que tal selo apenas dura um certo tempo - precisa ser renovado. Os 7 sabios se foram, misteriosamente... Alguem ...poderia certificar que Ziro ainda esta selado.
<br><br> E uma tarefa muito simples, ir la, ver o selo, voltar e me contar. Voce <b> so vai precisar fazer um ritualzinho bobo </b> para poder ver o selo. Vai precisar de um grupo para fazer o ritual. O que acha ?";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Irei procurar outras pessoas para ver tal selo entao...";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return @"E entao ? Noticias do selo ? Nao conseguiu ver ainda ? Basta ir na caverna onde Ziro foi
selado, o local que o mapa que lhe dei indica, e fazer o ritual. Como fazer o ritual voce pergunta ? Eu nao sei, mas tenho certeza que
 na propria caverna deve ter informacoes sobre isto basta procurar. Lembro que para entrar no local onde Ziro foi selado voce precisa encontrar um Goblin no inicio da caverna.";
            }
        }
        public override object Complete
        {
            get
            {
                return "O QUE ? Ziro se tornou um monstro e foi selado novamente ? Voces sao muito mais fortes do que imaginei. Muito bem, aqui estao sua recompensa.";
            }
        }

        public QuestExodo()
            : base()
        {
            this.AddObjective(new SlayObjective(typeof(ClockworkExodus), "???", 1));
            this.AddReward(new BaseReward(typeof(Gold), 10000, "10.000 Moedas de Ouro"));
            this.AddReward(new BaseReward(typeof(DispelScroll), 1, "1 Pergaminho"));
            this.AddReward(new BaseReward(typeof(CaixaDeGold), 1, "1 Caixa cheia de ouro"));
            this.AddReward(new BaseReward(typeof(LivroAntigo), 1, "1 Livro Antigo"));
        }

        public override void OnAccept()
        {
            base.OnAccept();
            this.Owner.SendMessage("Voce recebeu um mapa muito velho mas facil de entender");
            this.Owner.Backpack.DropItem(new MapaExodus());
            if(this.Owner.Wisp != null)
            {
                this.Owner.Wisp.FalaJill();
            }
        }

        public override void OnCompleted()
        {
            Points.PointsSystem.Exp.AwardPoints(this.Owner, 600);
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

    public class VelhoExodo : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(QuestExodo)
        };
            }
        }


        [Constructable]
        public VelhoExodo()
            : base("Jill", "a velha")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public VelhoExodo(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Humm... voce...quer uma....aventura ?");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
            this.Female = true;
            this.CantWalk = true;
            this.Race = Race.Human;
            this.HairItemID = 0x203D;
            this.HairHue = 0;
            base.InitBody();
        }

        public override void InitOutfit()
        {
            this.AddItem(new Backpack());
            this.AddItem(new Robe(788));
            this.AddItem(new ThighBoots());

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
