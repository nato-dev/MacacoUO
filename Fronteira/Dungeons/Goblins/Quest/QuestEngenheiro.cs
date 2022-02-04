using Server.Engines.Points;
using Server.Engines.Quests;
using Server.Items;
using Server.Items.Functional.Pergaminhos;
using Server.Mobiles;
using Server.Ziden.Dungeons.Goblins.Quest;
using Server.Ziden.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Quests.Engenheiro

{
    public class Engenheiro2 : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override QuestChain ChainID { get { return QuestChain.Engenheiro; } }

        public override object Title
        {
            get
            {
                return "Engenheiro Suspeito Parte 2";
            }
        }

        public override object Description
        {
            get
            {
                return @"“O engenheiro, muda o semblante amistoso para um olhar bem mais sóbrio.
- Bem, isso de fato é algo inusitado. Goblínico com certeza! Mas temos um problema... Sem minhas ferramentas é impossível acessar o núcleo dessa coisa. Poderia fazer novas, mas isso levaria dias! Um grupo de bandoleiros e seu lider, afanou minha caixa de ferramentas enquanto eu trabalhava em um serviço externo. Se você puder me ajudar, poderei retribuir o favor.<br>Os bandoleiros vivem em um forte abandonado a sudeste de Rhodes. Procure o lider deles.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Uma pena.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Alguma noticia das minhas ferramentas ?";
            }
        }

        public override object Complete
        {
            get
            {
                return @"O engenheiro olha para você com um imenso sorriso no rosto.
- Ah! As minhas ferramentas da sorte! Mamãe me deu esse conjunto quando eu ainda era um rapazote – ele lança um olhar saudoso para o vazio seguido de um suspiro.
- Saudades, mamãe...- ele olha em volta, como quem cai em si, notando seu olhar impassível.
Visivelmente desconcertado ele enrubesce, gagueja um pouco ao tentar retomar o assunto com você e então segue.
- Bom, vejamos o que essa coisinha irá nos revelar. 
Ele apanha uma série de ferramentas estranhas e se põe a trabalhar. – Rá! – berra o engenheiro de sua bancada – Está para nascer o goblin gosmento que irá superar minhas habilidades! Aqui está, parece ser uma mecanismo de reparo... 
Ele lhe estende a estranha chave enquanto coça o queixo com as mãos sujas de óleo. - Talvez precise de um funileiro para dar uma polida final.";
            }
        }

        public Engenheiro2()
            : base()
        {
            this.AddObjective(new ObtainObjective(typeof(FerramentasDaMamae), "Ferramentas da Mamae", 1));
            this.AddReward(new BaseReward(typeof(MechanicalComponent), "Componente Mecanico"));
        }

        public override void OnCompleted()
        {
            this.Owner.PlaySound(this.CompleteSound);
            PointsSystem.Exp.AwardPoints(this.Owner, 80);
        }

        public override void OnAccept()
        {
            base.OnAccept();
            this.Owner.QuestArrow = new QuestArrow(this.Owner, new Point3D(1481, 1432, 0));
            this.Owner.QuestArrow.Update();
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

    public class Engenheiro1 : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override QuestChain ChainID { get { return QuestChain.Engenheiro; } }

        public override Type NextQuest { get { return typeof(Engenheiro2); } }

        public override object Title
        {
            get
            {
                return "Engenheiro Suspeito";
            }
        }
        public override object Description
        {
            get
            {
                return @"“Uma pequena multidão se aglomera para observar a cena encontrada pelos guardas. Um cadáver recém encontrado caído diante de uma das entradas da cidade. É um homem jovem, com as roupas ainda úmidas de suor, com sangue ainda fresco.
- Provavelmente morreu fugindo de alguma coisa, pobre miserável – comenta um dos guardas.
Em suas costas, estão cravadas algumas pequenas facas, incomuns para o que a maior parte dos presentes costuma ver fabricado pelos artesãos locais. 
- Foram goblins! – comentou o guarda mais experiente – Provavelmente meteu-se em algum covil, achando que se daria bem saqueando goblins. Deixe-os sabendo que ainda quer mexer no ouro deles, e você ainda terá mais problema! Ele não fugiu... Foi espetado e trazido aqui como aviso.
<br><br>
O guarda revista o cadáver, encontrando em um alforje uma estranha caixa metálica, com catracas e dobradiças, exalando um forte cheiro de óleo queimado.
<br><br>
- Isso certamente nos dará mais pistas. Mas diabos, não posso sair daqui até que o inspetor chegue com o papa-defuntos... Além do que esse recruta não é dos mais espertos. Estou certo de que, caso queira ajudar, a prefeitura da cidade lhe proporcionará uma razoável recompensa. Meu palpite, é que nosso engenheiro pode ajudar com informações sobre essa traquitana. Ele fica <b>a sudoeste</b> daqui, em sua casa.
O guarda estende o braço para lhe entregar a estranha caixa.”";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Certo";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Alguma novidade de nosso engenheiro ?";
            }
        }
        public override object Complete
        {
            get
            {
                return @"“O engenheiro olha para você por cima de um par de lentes de aumento, enquanto suspira com um aparente desapontamento.<br>
- Acho que essa solda nunca vai funcionar sem minhas ferramentas, mas que bom que você está aqui, uma boa prosa certamente vai acabar com todo esse desânimo! Em que posso ajudar?”<br><br>
- entrega o item –";
            }
        }

        public Engenheiro1()
            : base()
        {
            this.AddObjective(new DeliverObjective(typeof(Engenhoca), "Engenhoca", 1, typeof(EngenheiroMilitar), "Diolo o Engenheiro Militar"));
        }

        public override void OnCompleted()
        {
            this.Owner.PlaySound(this.CompleteSound);
            PointsSystem.Exp.AwardPoints(this.Owner, 100);
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

    public class MatarBoss : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        //public override QuestChain ChainID => QuestChain.Tutorial;

        //public override Type NextQuest =>  typeof(CloseEnoughQuest)

        public override object Title
        {
            get
            {
                return "A Arma Secreta";
            }
        }
        public override object Description
        {
            get
            {
                return @"O goblin parece afoito, um tanto nervoso. <br> - Humano...
<br> - Nao sei mais a quem recorrer... Voce tem de me ouvir.
<br * O goblin se ajoelha *>
<br> - Voce precisa parar Kalzu. Quem e Kalzu voce pergunta ? Humanos... nao sabem de nada o que acontece que nao seja com seu proprio umbigo.
<br> - Nosso novo 'Lider' * falando com uma voz abobalhada *
<br> - Kalzu sabe isso, Kalzu sabe aquilo, Kalzu tem todo conhecimento... MALDITO !
<br> - Mas agora... pirou de vez... ele completou... ele construiu... uma arma. Voces tem que impedi-lo ! Voces tem que destruir-la !
* o goblin mostra uma cara mais seria *
<br> - Voces tem que encontrar o que Kalzu construiu, e destrui-la ! Uma arma muito poderosa.
<br> - Se o fizer, estara salvando a todos, Goblins, Humanos, Orcs, Selvagens, Trolls, Ogros - TODOS !";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Humanos, so pensam em seu proprio umbigo.";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return @"Voce precisa destruir a arma que Kalzu, o 'novo' lider dos goblins construiu.";
            }
        }
        public override object Complete
        {
            get
            {
                return "Nao tenho palavras para expressar o que sinto - voce salvou a todos... TODOS.... Voce merece, fique com isto.";
            }
        }

        public MatarBoss()
            : base()
        {
            this.AddObjective(new SlayObjective(typeof(GolemMecanico), "A Arma Secreta", 1));
            this.AddReward(new BaseReward(typeof(CaixaDeGold), 1, "Agradecimento de um goblin"));
            //this.AddReward(new BaseReward(typeof(PergaminhoSagrado), 1, "Pergaminho Sagrado para Roupas"));
            this.AddReward(new BaseReward(typeof(LivroAntigo), 1, "Livro Antigo"));
            this.AddReward(new BaseReward(typeof(Gold), 3000, "3.000 Moedas"));
        }

        public override void OnAccept()
        {
            base.OnAccept();
        }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 500);
            this.Owner.PlaySound(this.CompleteSound);
            //this.Owner.Backpack.AddItem(new Gold(60000));
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
