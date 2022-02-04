using Server.Fronteira.Talentos;
using System.Collections.Generic;
using System.Linq;

namespace Server.Fronteira.Classes
{
    public static class ClassDef
    {
        private static Dictionary<int, ClassePersonagem> _classes = new Dictionary<int, ClassePersonagem>();

        public static void _add(int ID, ClassePersonagem classe)
        {
            _classes[ID] = classe;
        }

        public static List<ClassePersonagem> GetClasses()
        {
            return _classes.Values.ToList();
        }

        public static ClassePersonagem GetClasse(int id)
        {
            if (!_classes.ContainsKey(id))
                return null;
            return _classes[id];
        }

        private static void AddClass(ClassePersonagem classe, params OpcaoTalentos[] talentos)
        {
            var id = _classes.Count + 1;
            classe.ID = id;
            _classes[id] = classe;
            classe.Talentos = talentos;
        }

        // TODO: Botar items de classes necessarios (livros etc)
        // TODO: Fazer os powerscolls de 90 a 100 nas skills
        static ClassDef()
        {
            if (_classes.Count != 0)
                return;

            // TODO - mudar o nome das coisa pruns nome doido q ceis curte
            AddClass(new ClassePersonagem("Guerreiro", 40324,
               "Paladino, Cavaleiro Sombrio, Comandante",
               new SkillClasse[] {
                    new SkillClasse(SkillName.Wrestling, 90), new SkillClasse(SkillName.Swords, 90),  new SkillClasse(SkillName.Fencing, 90),
                    new SkillClasse(SkillName.Macing, 90),  new SkillClasse(SkillName.Tactics, 90),
                    new SkillClasse(SkillName.Healing, 80), new SkillClasse(SkillName.Anatomy, 80),
                    new SkillClasse(SkillName.MagicResist, 70),

                    new SkillClasse(SkillName.Lumberjacking, 60),  new SkillClasse(SkillName.Blacksmith, 60), new SkillClasse(SkillName.Mining, 60), new SkillClasse(SkillName.Parry, 60),
                    new SkillClasse(SkillName.Fishing, 60),  new SkillClasse(SkillName.Focus, 50),  new SkillClasse(SkillName.Cooking, 60)
           }),
              new OpcaoTalentos(Talento.Experiente, Talento.Esquiva, Talento.Precisao),
              new OpcaoTalentos(Talento.Hab_Block, Talento.Hab_CrushingBlow, Talento.Hab_Wirlwind),
              new OpcaoTalentos(Talento.Espadas, Talento.Lancas, Talento.Porretes),
              new OpcaoTalentos(Talento.Curandeiro, Talento.Ladrao, Talento.Finta),
              new OpcaoTalentos(Talento.ProtecaoPesada, Talento.PeleArcana, Talento.Perseveranca),
              new OpcaoTalentos(Talento.Paladino, Talento.Darknight, Talento.Comandante),
              new OpcaoTalentos(Talento.Hipismo, Talento.ArmaduraPesada, Talento.Potencia),
              new OpcaoTalentos(Talento.Bloqueador, Talento.ResistSpell, Talento.Brutalidade),
              new OpcaoTalentos(Talento.Defensor, Talento.Rastreador, Talento.Envenenador),
              new OpcaoTalentos(Talento.FisicoPerfeito, Talento.Machados, Talento.Hastes),
              new OpcaoTalentos(Talento.Hab_BleedAttack, Talento.Hab_AtaqueMortal, Talento.Hab_Bladeweave),
              new OpcaoTalentos(Talento.ArmaduraMagica, Talento.Sabedoria, Talento.Regeneracao)
           );

            AddClass(new ClassePersonagem("Ladino", 40324,
             "Assassino, Ranger, Cacador de Tesouro",
             new SkillClasse[] {
                    new SkillClasse(SkillName.Archery, 90), new SkillClasse(SkillName.Hiding, 90),  new SkillClasse(SkillName.Anatomy, 90),
                    new SkillClasse(SkillName.Tactics, 70),  new SkillClasse(SkillName.Lockpicking, 70),
                    new SkillClasse(SkillName.Healing, 70), new SkillClasse(SkillName.Fencing, 70), new SkillClasse(SkillName.MagicResist, 60),
                    new SkillClasse(SkillName.Poisoning, 50),  new SkillClasse(SkillName.Begging, 60), new SkillClasse(SkillName.Focus, 60),

                    new SkillClasse(SkillName.Camping, 60), new SkillClasse(SkillName.Fletching, 40), new SkillClasse(SkillName.RemoveTrap, 60),
                    new SkillClasse(SkillName.DetectHidden, 60), new SkillClasse(SkillName.Tracking, 40)
            }),
                new OpcaoTalentos(Talento.Ladrao, Talento.Esquiva, Talento.Precisao),
                new OpcaoTalentos(Talento.Hab_Doubleshot, Talento.Hab_DoubleStrike, Talento.Hab_ParalizeBlow),
                new OpcaoTalentos(Talento.Adagas, Talento.Espadas, Talento.Lancas),
                new OpcaoTalentos(Talento.Silencioso, Talento.Perseveranca, Talento.Potencia),
                new OpcaoTalentos(Talento.Gatuno, Talento.Hipismo, Talento.Rastreador),
                new OpcaoTalentos(Talento.Assassino, Talento.CacadorDeTesouros, Talento.Ranger),
                new OpcaoTalentos(Talento.Curandeiro, Talento.Regeneracao, Talento.Hab_Infectar),
                new OpcaoTalentos(Talento.Hab_SerpentArrow, Talento.Hab_AtaqueMortal, Talento.Hab_TalonStrike),
                new OpcaoTalentos(Talento.Hab_Shadowstrike, Talento.Hab_Infectar, Talento.Hab_ArmorIgnore),
                new OpcaoTalentos(Talento.Brutalidade, Talento.Finta, Talento.Hipismo),
                new OpcaoTalentos(Talento.ResistSpell, Talento.Hab_Disarm, Talento.Hab_Dismount),
                new OpcaoTalentos(Talento.CorrerStealth, Talento.AnimalLore, Talento.Hab_MovingSHot)
            );

            // mago hiding

            AddClass(new ClassePersonagem("Mago", 40324,
             "Toca magia",
             new SkillClasse[] {
                    new SkillClasse(SkillName.Magery, 90), new SkillClasse(SkillName.EvalInt, 90),  new SkillClasse(SkillName.Meditation, 90), new SkillClasse(SkillName.MagicResist, 90),
                    new SkillClasse(SkillName.Wrestling, 70), new SkillClasse(SkillName.Macing, 70),
                    new SkillClasse(SkillName.Poisoning, 50),  new SkillClasse(SkillName.Alchemy, 40), new SkillClasse(SkillName.Herding, 30), new SkillClasse(SkillName.Healing, 60),
                    new SkillClasse(SkillName.Tailoring, 60), new SkillClasse(SkillName.Fencing, 60), new SkillClasse(SkillName.Fishing, 60), new SkillClasse(SkillName.SpiritSpeak, 30),
                    new SkillClasse(SkillName.ItemID, 60),  new SkillClasse(SkillName.Forensics, 40),  new SkillClasse(SkillName.Imbuing, 40)
            }),
              new OpcaoTalentos(Talento.Elementalismo, Talento.EstudoSagrado, Talento.ArmaduraMagica),
              new OpcaoTalentos(Talento.Cajados, Talento.Alquimista, Talento.Livros),
              new OpcaoTalentos(Talento.Ladrao, Talento.Esconderijo, Talento.Investigador),
              new OpcaoTalentos(Talento.Curandeiro, Talento.Herbalismo, Talento.Hipismo),
              new OpcaoTalentos(Talento.Arquimago, Talento.Necromante, Talento.Feiticeiro),
              new OpcaoTalentos(Talento.Hab_ForceOfNature, Talento.Foco, Talento.Hab_PsyAttack),
              new OpcaoTalentos(Talento.Hab_Feint, Talento.Hab_Disarm, Talento.Hab_ParalizeBlow),
              new OpcaoTalentos(Talento.Regeneracao, Talento.ResistSpell, Talento.Silencioso),
              new OpcaoTalentos(Talento.Provocacao, Talento.Sabedoria, Talento.Pacificador),
              new OpcaoTalentos(Talento.Combate, Talento.Bloqueador, Talento.Envenenador),
              new OpcaoTalentos(Talento.Dispel, Talento.Hab_Infectar, Talento.MentePerfurante),
              new OpcaoTalentos(Talento.AlquimiaMagica, Talento.Sagacidade, Talento.Esquiva)
            );

            /*
            AddClass(new ClassePersonagem("Bardo", 40324,
           "Toca viola",
           new SkillClasse[] {
                    new SkillClasse(SkillName.Musicianship, 90), new SkillClasse(SkillName.Peacemaking, 90),  new SkillClasse(SkillName.Discordance, 90), new SkillClasse(SkillName.Provocation, 90),
                    new SkillClasse(SkillName.Fencing, 80),  new SkillClasse(SkillName.Wrestling, 80),  new SkillClasse(SkillName.Archery, 70), new SkillClasse(SkillName.MagicResist, 70),
                    new SkillClasse(SkillName.Tactics, 70),  new SkillClasse(SkillName.Healing, 60), new SkillClasse(SkillName.Anatomy, 60),

                    new SkillClasse(SkillName.Lumberjacking, 60), new SkillClasse(SkillName.Carpentry, 60), new SkillClasse(SkillName.Fishing, 60),
                    new SkillClasse(SkillName.Camping, 60),  new SkillClasse(SkillName.Magery, 50),
          }));

            AddClass(new ClassePersonagem("Campones", 40324,
              "Toca mao de obra",
              new SkillClasse[] {
                    new SkillClasse(SkillName.Mining, 90), new SkillClasse(SkillName.Lumberjacking, 90),  new SkillClasse(SkillName.Fishing, 90), new SkillClasse(SkillName.Tinkering, 90),  new SkillClasse(SkillName.Cooking, 90),
                    new SkillClasse(SkillName.Wrestling, 80), new SkillClasse(SkillName.Macing, 80), new SkillClasse(SkillName.Swords, 70),
                    new SkillClasse(SkillName.Tactics, 60),  new SkillClasse(SkillName.Healing, 60), new SkillClasse(SkillName.Anatomy, 60),

                    new SkillClasse(SkillName.Blacksmith, 90), new SkillClasse(SkillName.Carpentry, 90), new SkillClasse(SkillName.Alchemy, 60),
                    new SkillClasse(SkillName.Fletching, 90),  new SkillClasse(SkillName.Camping, 80),  new SkillClasse(SkillName.Tailoring, 80),
                    new SkillClasse(SkillName.Herding, 80),  new SkillClasse(SkillName.MagicResist, 40),
             }));
             */


            /*
            AddClass(new ClassePersonagem("Anteras", 40324,
                "Os Anteras existem muito antes de serem chamados de Anteras.<br>Desde o primeiro homem que derramou sangue por Kaludes que os livros sagrados são lidos e seguidos por esse seleto grupo de guerreiros.<br>O livro conta a historia do Deus dos Céus e todo seu percurso para conseguir trazer força e poder aos seus seguidores.<br>Os Aludes gostam de acreditar que Kaludes conseguiu sua mais difícil façanha, mas muitos acreditam que é a fé que dá poder aos Anteras.<br>Os segredos dos livros sagrados são passados de pais para filhos e filhas e assim essa classe segue uma linhagem pura.<br>Os guerreiros Anteras são fortes o suficiente para vestir armaduras pesadas, empunhar armas maciças e conjurar alguns feitiços provindos da certeza de que Kaludes os acompanha.<br>Todos os membros da classe se provam guerreiros habilidosos antes de serem designados a sua escolha final entre ser um Guerreiro da luz ou da sombra.<br>Ambos de extrema importância.<br><b>Anteras da Luz</b>: Os Anteras da luz são conhecidos pelo reino como protetores da justiça divina e guardadores de um conhecimento ancestral e secreto. Entre seus lemas, estão a de honrar o nome da família e proteger os mitos de Kaludes acima de todas as coisas.<br><b>Anteras da Sombra</b>: Os Anteras das sombras surgiram pouco depois dos Anteras da luz, mas eles não carregam o mesmo julgamento que eles.<br>São vistos pelos Aludes como desonrados, medíocres e aberrações. Os Aludes não podem estar mais errados, são mal compreendidos por serem regidos por Kalam, o Deus da Terra, por muitos não vistos nem como uma divindade.<br>Kalam foi peça fundamental nas conquistas de Kaludes.<br>Os Anteras das sombras foram criados pelos mesmos ensinamentos, pelo mesmo livro e pelos mesmos mestres.<br>A luz para controlar a luz, e a sombra para conter a sombra.",
                0, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Swords, SkillName.Focus, SkillName.Parry, SkillName.Macing
            }));

            AddClass(new ClassePersonagem("Durah", 40344, "Lore Durah", 1, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Swords, SkillName.Focus, SkillName.Parry, SkillName.Macing
            }));

            AddClass(new ClassePersonagem("Caeros", 40325, "Lore Caeros", 2, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.AnimalTaming, SkillName.Veterinary
            }));

            AddClass(new ClassePersonagem("Lair", 40348, "Lore Lair", 3, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.AnimalTaming, SkillName.Veterinary
            }));

            AddClass(new ClassePersonagem("Eres", 40328, "Lore Eres", 4, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Spellweaving, SkillName.Meditation, SkillName.Focus, SkillName.DetectHidden
            }));

            AddClass(new ClassePersonagem("Gimur", 40346, "Lore Gimur", 5, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Spellweaving, SkillName.Meditation, SkillName.Focus, SkillName.DetectHidden
            }));

            AddClass(new ClassePersonagem("Vussaras", 40339, "Lore Vussaras", 6, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Fencing, SkillName.Macing, SkillName.Swords, SkillName.Focus
            }));

            AddClass(new ClassePersonagem("Sodah", 40354, "Lore Sodah", 7, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Fencing, SkillName.Macing, SkillName.Swords, SkillName.Focus
            }));

            AddClass(new ClassePersonagem("Famiros", 40329, "Lore Famiros", 8, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Musicianship, SkillName.Focus, SkillName.Fencing, SkillName.Meditation
            }));

            AddClass(new ClassePersonagem("Madur", 40349, "Lore Madur", 9, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Musicianship, SkillName.Focus, SkillName.Fencing, SkillName.Meditation
            }));

            AddClass(new ClassePersonagem("Lamares", 40331, "Lore Lamares", 10, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Archery, SkillName.RemoveTrap, SkillName.Focus
            }));

            AddClass(new ClassePersonagem("Berlan", 40342, "Lore Berlan", 11, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Archery, SkillName.RemoveTrap, SkillName.Focus
            }));

            AddClass(new ClassePersonagem("Dostaras", 40334, "Lore Dostaras", 12, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Hiding, SkillName.Stealth, SkillName.Fencing, SkillName.Tactics
            }));

            AddClass(new ClassePersonagem("Vomir", 40357, "Lore Vomir", 13, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Hiding, SkillName.Stealth, SkillName.Fencing, SkillName.Tactics
            }));

            AddClass(new ClassePersonagem("Pidaras", 40323, "Lore Pidaras", 14, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Macing, SkillName.Parry, SkillName.Focus
            }));

            AddClass(new ClassePersonagem("Colan", 40352, "Lore Colan", 15, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Macing, SkillName.Parry, SkillName.Focus
            }));

            AddClass(new ClassePersonagem("Amiros", 40333, "Lore Amiros", 16, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Magery, SkillName.Meditation
            }));

            AddClass(new ClassePersonagem("Ostah", 40343, "Lore Ostah", 17, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Magery, SkillName.Meditation
            }));

            AddClass(new ClassePersonagem("Idures", 40335, "Lore Idures", 18, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Swords, SkillName.Focus, SkillName.Tactics
            }));

            AddClass(new ClassePersonagem("Murah", 40350, "Lore Murah", 19, new SkillName[] {
                SkillName.AnimalLore, SkillName.Begging, SkillName.Camping, SkillName.Healing, SkillName.Forensics, SkillName.MagicResist, SkillName.Snooping, SkillName.Stealing, SkillName.Lockpicking, SkillName.Wrestling, SkillName.Tracking, SkillName.Swords, SkillName.Focus, SkillName.Tactics
            }));

            AddClass(new ClassePersonagem("Ysbares", 40338, "Lore Ysbares", 20, new SkillName[] {
                SkillName.Cooking, SkillName.Fishing, SkillName.Herding, SkillName.Inscribe, SkillName.Cartography, SkillName.Tailoring, SkillName.Fletching, SkillName.Carpentry, SkillName.Lumberjacking, SkillName.ArmsLore, SkillName.Alchemy, SkillName.Mining
            }));

            AddClass(new ClassePersonagem("Argur", 40341, "Lore Argur", 21, new SkillName[] {
                SkillName.Cooking, SkillName.Fishing, SkillName.Herding, SkillName.Inscribe, SkillName.Cartography, SkillName.Tailoring, SkillName.Fletching, SkillName.Carpentry, SkillName.Lumberjacking, SkillName.ArmsLore, SkillName.Alchemy, SkillName.Mining
            }));
            */
        }
    }
}
