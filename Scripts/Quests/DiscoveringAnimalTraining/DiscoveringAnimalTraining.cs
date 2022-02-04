using System;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Engines.Quests
{
    public class TamingPetQuest : BaseQuest
    {
        public TamingPetQuest()
            : base()
        {
            AddObjective(new InternalObjective());
            
            AddReward(new BaseReward("Um passo a frente para ser um domador de animais")); // A step closer to mastering Animal Training.
        }

        public override QuestChain ChainID{get {return QuestChain.AnimalTraining; } }
        public override Type NextQuest { get { return typeof(UsingAnimalLoreQuest); } }

        /* Discovering Animal Training */
        public override object Title { get { return "Descobrindo animais"; } }

        /*Years of patience and meticulous study have paid off!  New Animal Training techniques have been discovered!  
         * Animal tamers may now train their pets, teaching them new ways to fight and survive!  The first step is to 
         * tame a creature.  Find a creature in the wild, and using your animal taming skill - tame it!*/
        public override object Description { get { return @"Anos de paciência e estudo meticuloso valeram a pena! Novas técnicas de treinamento de animais foram descobertas!
 Domadores de animais agora podem treinar seus animais de estimação, ensinando-lhes novas maneiras de lutar e sobreviver! O primeiro passo é
 domar uma criatura. Encontre uma criatura em estado selvagem e, usando sua habilidade de domar animais - domestique-a!
"; } }

        /* The life of an animal trainer is not for everyone, return to an Animal Trainer if you wish to try again. */
        public override object Refuse { get { return "Tudo bem"; } }

        /* Find a creature in the wild, and using the animal taming skill - tame it! */
        public override object Uncomplete { get { return "Encontre uma criatura e use a skill Animal Taming nela !"; } }

        /* Well done!  Now that you have your pet it is time to start training! */
        public override object Complete { get { return "Muito bom ! Agora eh a hora de comecar o treinamento !"; } }

        public override int AcceptSound { get { return 0x2E8; } }

        public static void CheckTame(PlayerMobile pm)
        {
            if (pm == null)
                return;

            var quest = QuestHelper.GetQuest(pm, typeof(TamingPetQuest));

            if (quest != null && !quest.Completed)
            {
                quest.Objectives[0].CurProgress++;
                quest.OnCompleted();
            }
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

        public class InternalObjective : BaseObjective
        {
            public override object ObjectiveDescription { get { return "Dome uma criatura"; } } // Tame a Creature

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

    public class UsingAnimalLoreQuest : BaseQuest
    {
        public UsingAnimalLoreQuest()
            : base()
        {
            AddObjective(new InternalObjective());

            AddReward(new BaseReward("Um passo mais perto de ser um mestre domador")); // A step closer to mastering Animal Training.
        }

        public override QuestChain ChainID { get { return QuestChain.AnimalTraining; } }
        public override Type NextQuest { get { return typeof(LeadingIntoBattleQuest); } }
        //public override bool DoneOnce { get { return true; } }

        /* Discovering Animal Training */
        public override object Title { get { return 1157527; } }

        /*Now that your pet is tame, you must begin the training process.  Pets will train while they are engaged in 
         * combat, and will progress as they battle other creatures.  Pets train best against wild creatures, and will
         * learn the most from the fiercest creatures in the realm!  There is a limit to how much a pet can learn from 
         * a single foe, so make sure your pet has fresh adversaries!<br><br>When you are ready to begin the training 
         * process, use the Animal Lore skill on your pet and select "Begin Animal Training."  When your pet has 
         * completed the training process you can teach them new ways to fight and survive!*/
        public override object Description { get { return @"Agora que seu animal de estimação é doméstico, você deve iniciar o processo de treinamento. Os animais de estimação treinam enquanto estão envolvidos em
 combate, e progredirá enquanto eles batalham com outras criaturas. Os animais de estimação treinam melhor contra criaturas selvagens e
 Aprenda ao máximo as criaturas mais ferozes do reino! Há um limite para o quanto um animal de estimação pode aprender
 um único inimigo, então verifique se seu animal de estimação tem novos adversários! <br> <br> Quando você estiver pronto para começar o treinamento
 processo, use a habilidade Animal Lore em seu animal de estimação e selecione 'Iniciar Treinamento'. Quando seu animal de estimação tem
 completou o processo de treinamento, você pode ensinar-lhes novas maneiras de lutar e sobreviver!
"; } }

        /* The life of an animal trainer is not for everyone, return to an Animal Trainer if you wish to try again. */
        public override object Refuse { get { return "Uma pena, mas tudo bem"; } }

        /* When you are ready to begin the training process, use the Animal Lore skill on your pet and select "Begin Animal Training." */
        public override object Uncomplete { get { return "Use a skill animal lore no seu animal e selecione 'Iniciar Treinamento'"; } }

        /* Well done!  Now that your pet has begun the Animal Training process return to the Animal Trainer to learn more about the next steps. */
        public override object Complete { get { return "Muito bom !! Retorne ao domador !"; } }

        public override void OnCompleted()
        {
            Owner.SendLocalizedMessage(1157536, null, 0x23); // Well done!  Now that your pet has begun the Animal Training process return to the Animal Trainer to learn more about the next steps.					
            Owner.PlaySound(CompleteSound);
        }

        public static void CheckComplete(PlayerMobile pm)
        {
            if (pm == null)
                return;

            var quest = QuestHelper.GetQuest(pm, typeof(UsingAnimalLoreQuest));

            if (quest != null && !quest.Completed)
            {
                quest.Objectives[0].CurProgress++;
                quest.OnCompleted();
            }
        }

        public class InternalObjective : BaseObjective
        {
            public override object ObjectiveDescription { get { return "Use Animal Lore em seu animal e selecione 'Iniciar Treinamento'"; } }
            /*Use the Animal Lore Skill on your pet and select "Begin Animal Training."<br><br> */

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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LeadingIntoBattleQuest : BaseQuest
    {
        public LeadingIntoBattleQuest()
            : base()
        {
            AddObjective(new InternalObjective());

            AddReward(new BaseReward("Um passo a ser um mestre domador")); // A step closer to mastering Animal Training.
        }

        public override QuestChain ChainID { get { return QuestChain.AnimalTraining; } }
        public override Type NextQuest { get { return typeof(TeachingSomethingNewQuest); } }
        //public override bool DoneOnce { get { return true; } }

        /* Discovering Animal Training */
        public override object Title { get { return 1157527; } }

        /*Now that you have started the training process it is time to lead your pet into battle!  Pets will train while 
         * they are engaged in combat, and will progress as they battle other creatures.  Pets train best against wild 
         * creatures, and will learn the most from the fiercest creatures in the realm!  There is a limit to how much a 
         * pet can learn from a single foe, so make sure your pet has fresh adversaries!  When the "Pet Training Progress"
         * bar is full, your pet is ready to learn new ways to fight and survive.  <br><br>Now you must lead your pet into 
         * the wild and battle it against other creatures!*/
        public override object Description { get { return @"Agora que você iniciou o processo de treinamento, é hora de levar seu animal de estimação para a batalha! Animais de estimação treinam enquanto
 eles estão envolvidos em combate e progredirão enquanto combatem outras criaturas. Animais de estimação treinam melhor contra animais selvagens
 criaturas, e aprenderá mais com as criaturas mais ferozes do reino! Existe um limite para quanto
 o animal de estimação pode aprender com um único inimigo, portanto, verifique se ele tem novos adversários! Quando o 'Progresso do treinamento para animais de estimação'
 a barra está cheia, seu animal de estimação está pronto para aprender novas maneiras de lutar e sobreviver. < br > < br > Agora você deve levar seu animal de estimação para
 selvagem e batalhe contra outras criaturas!
"; } }

        /* The life of an animal trainer is not for everyone, return to an Animal Trainer if you wish to try again. */
        public override object Refuse { get { return "Nao tem problema"; } }

        /* Lead your pet into the wild and battle it against other creatures until the "Pet Training Progress" bar is full. 
         * Remember Pets train best against wild creatures, and will learn the most from the fiercest creatures in the realm!  
         * There is a limit to how much a pet can learn from a single foe, so make sure your pet has fresh adversaries! */
        public override object Uncomplete { get { return @"Leve seu animal de estimação à natureza e lute contra outras criaturas até que a barra 'Progresso no treinamento do animal de estimação' esteja cheia.
 Lembre - se de que os animais de estimação treinam melhor contra criaturas selvagens e aprenderão o máximo das criaturas mais ferozes do reino!
 Há um limite para o quanto um animal de estimação pode aprender com um único inimigo, portanto, verifique se ele tem novos adversários!"; } }

        /* Well done!  Now that your pet has begun the Animal Training process return to the Animal Trainer to learn more about the next steps. */
        public override object Complete { get { return "Muito bom !!"; } }

        public override void OnCompleted()
        {
            Owner.SendMessage("Parabens, volte ao treinador para aprender os proximos passos !"); // Well done!  Now that your pet has begun the Animal Training process return to the Animal Trainer to learn more about the next steps.	
            Owner.PlaySound(CompleteSound);
        }

        public static void CheckComplete(PlayerMobile pm)
        {
            if (pm == null)
                return;

            var quest = QuestHelper.GetQuest(pm, typeof(LeadingIntoBattleQuest));

            if (quest != null && !quest.Completed)
            {
                quest.Objectives[0].CurProgress++;
                quest.OnCompleted();
            }
        }

        public class InternalObjective : BaseObjective
        {
            public override object ObjectiveDescription { get { return "Leve seu animal de estimação à natureza e lute contra outras criaturas até que a barra 'Progresso no treinamento do animal de estimação' esteja cheia."; } }
            /*Lead your pet into the wild and battle it against other creatures until the "Pet Training Progress" bar is full.*/

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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TeachingSomethingNewQuest : BaseQuest
    {
        public TeachingSomethingNewQuest()
            : base()
        {
            AddObjective(new InternalObjective());

            AddReward(new BaseReward("Um passo para maestria de treinamento de animais")); // A step closer to mastering Animal Training.
        }

        public override QuestChain ChainID { get { return QuestChain.AnimalTraining; } }
        public override Type NextQuest { get { return null; } }
        public override bool DoneOnce { get { return true; } }

        /* Discovering Animal Training */
        public override object Title { get { return "Descobrindo o Treinamento de Animais"; } }

        /*Now that your pet has fully trained, it is time to teach it something new!  Use the Animal Lore skill on your pet and select 
         * "Pet Training Options."  The Animal Training menu lists all of the available training properties you can apply to the pet.
         * <br><br>The Categories pane shows the category of available properties:<br><br>Stats and Resists allow you to increase
         * individual Stat and Resist properties for the pet.<br><br>Increase Magic Skill Caps and Increase Combat Skill Caps allow
         * you to increase skill caps for various magical and combat related skills.  This process requires the use of Power Scrolls
         * and only increases the skill cap, you will still need to train the pet in the specific skill through traditional pet skill 
         * training. <br><br>Magical Abilities allow you to give the pet magical abilities in one of several spell schools<br><br>Special
         * Abilities allow you to give the pet special abilities, different than those traditionally found as weapon special moves.
         * <br><br>Special Moves allow you to give the pet special moves, similar to those traditionally found as weapon special moves. 
         * <br><br>Area Effect Abilities allow you to give the pet an area attack, targeting multiple adversaries within an area.<br><br>
         * When you train your pet, the number of control slots the pet requires will increase.  The maximum number of control slots any 
         * pet can have is 5, however individual pets have maximum control slots they can be trained to.<br><br>A pet can only learn so
         * much during each training level.  As you mix and match properties from the Animal Training menu, the amount of available training
         * points will decrease based on your selections.  Different property selections have different training point costs.  When you are
         * ready to apply a new property to your pet, select "Train Pet" and confirm you are ready to do so!*/
        public override object Description { get { return @"Agora que seu animal de estimação foi treinado completamente, é hora de ensinar algo novo! Use a habilidade Animal Lore em seu animal de estimação e selecione
 'Opções de treinamento para animais de estimação'. O menu Treinamento de animais lista todas as propriedades de treinamento disponíveis que você pode aplicar ao animal.
 < br > < br > O painel Categorias mostra a categoria de propriedades disponíveis: < br > < br > Estatísticas e resistências permitem aumentar
propriedades Stat e Resist individuais do animal de estimação. < br > < br > Aumentar os limites de habilidades mágicas e aumentar os limites de habilidades de combate permitem
você aumenta o limite de habilidades para várias habilidades mágicas e relacionadas ao combate.Esse processo requer o uso de Power Scrolls
e apenas aumenta o limite de habilidades, você ainda precisará treinar o animal na habilidade específica através da habilidade tradicional
Treinamento. < br > < br > Habilidades mágicas permitem que você ofereça habilidades mágicas ao animal de estimação em uma das várias escolas de feitiços<br> < br > Especial
 As habilidades permitem que você dê ao animal de estimação habilidades especiais, diferentes daquelas tradicionalmente encontradas como movimentos especiais de armas.
  
 < br > < br > Movimentos especiais permitem que você faça movimentos especiais ao animal de estimação, semelhantes aos tradicionalmente encontrados como movimentos especiais de armas.
      
 < br > < br > As habilidades de efeito de área permitem que você ataque o animal de estimação, mirando em vários adversários dentro de uma área. < br > < br >
Quando você treina seu animal de estimação, o número de slots de controle requeridos aumentará.O número máximo de slots de controle que qualquer
 o animal de estimação pode ter 5, no entanto, animais de estimação individuais têm slots de controle máximos para os quais podem ser treinados. < br > < br > Um animal de estimação só pode aprender isso.
            
 muito durante cada nível de treinamento.À medida que você mistura e combina propriedades no menu Animal Training, a quantidade de treinamento disponível
os pontos diminuirão com base nas suas seleções.Diferentes seleções de propriedades têm custos de pontos de treinamento diferentes.Quando você é
 pronto para aplicar uma nova propriedade ao seu animal de estimação, selecione 'Treinar Animal' e confirme que você está pronto para fazê-lo!
"; } }

        /* The life of an animal trainer is not for everyone, return to an Animal Trainer if you wish to try again. */
        public override object Refuse { get { return "Sem problemas"; } }

        /*Use the Animal Lore skill on your pet and select "Pet Training Options" to mix and match which properties 
             * you will train your pet.  When you are satisfied with the property you have chosen select "Train Pet" 
             * and confirm the training!.*/
        public override object Uncomplete { get { return "Use Animal Lore no seu pet, selecione 'Opcoes de treinamento', faca suas escolhas e clique em 'Treinar Pet'"; } }

        /* You have Discovered Animal Training!  Train new pets and mix and match properties to create unique variations of pets to take into 
         * battle!  Good Luck, Animal Trainer! */
        public override object Complete { get { return "Muito bom !!!"; } }

        public override void OnCompleted()
        {
            Owner.SendLocalizedMessage("Muito bom ! Voce completou a quest ! Volte ao treinador de animais para mais !"); // You've completed an Animal Training quest! Visit an Animal Trainer to continue!						
            Owner.PlaySound(CompleteSound);
        }

        public static void CheckComplete(PlayerMobile pm)
        {
            if (pm == null)
                return;

            var quest = QuestHelper.GetQuest(pm, typeof(TeachingSomethingNewQuest));

            if (quest != null && !quest.Completed)
            {
                quest.Objectives[0].CurProgress++;
                quest.OnCompleted();
            }
        }

        public override void GiveRewards()
        {
            base.GiveRewards();

            Owner.AddToBackpack(new EthologistTitleDeed());
        }

        public class InternalObjective : BaseObjective
        {
            public override object ObjectiveDescription { get { return @" Use a habilidade Animal Lore em seu animal de estimação e selecione 'Opções de treinamento para animais de estimação' para misturar e combinar quais propriedades
 você treinará seu animal de estimação. Quando estiver satisfeito com a propriedade que você escolheu, selecione 'Treinar Pet'
 e confirme o treinamento!"; } }


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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
