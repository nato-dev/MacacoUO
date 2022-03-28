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

                    new SkillClasse(SkillName.Camping, 60), new SkillClasse(SkillName.Bowcraft, 40), new SkillClasse(SkillName.RemoveTrap, 60),
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

            AddClass(new ClassePersonagem("Bardo", 40324,
           "Toca viola",
           new SkillClasse[] {
                    new SkillClasse(SkillName.Musicianship, 90), new SkillClasse(SkillName.Peacemaking, 90),  new SkillClasse(SkillName.Discordance, 90), new SkillClasse(SkillName.Provocation, 90),
                    new SkillClasse(SkillName.Fencing, 80),  new SkillClasse(SkillName.Wrestling, 80),  new SkillClasse(SkillName.Archery, 70), new SkillClasse(SkillName.MagicResist, 70),
                    new SkillClasse(SkillName.Tactics, 70),  new SkillClasse(SkillName.Healing, 60), new SkillClasse(SkillName.Anatomy, 60),

                    new SkillClasse(SkillName.Lumberjacking, 60), new SkillClasse(SkillName.Carpentry, 60), new SkillClasse(SkillName.Fishing, 60),
                    new SkillClasse(SkillName.Camping, 60),  new SkillClasse(SkillName.Magery, 50),
          }));

            /*
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
        }
    }
}
