using System;
using Server.Engines.Quests;
using Server.Factions;
using Server.Gumps;
using Server.Gumps.Newbie;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Regions;
using Server.Spells.First;

namespace Server.Mobiles
{
    [CorpseName("a wisp corpse")]
    public class WispGuia : BaseCreature
    {
        public static void Configure()
        {
            if (Shard.RP)
                return;

            EventSink.ResourceHarvestSuccess += EventSink_ResourceHarvestSuccess;
            EventSink.CraftSuccess += EventSink_CraftSuccess;
        }

        private static void EventSink_CraftSuccess(CraftSuccessEventArgs e)
        {
            if (e.Crafter is PlayerMobile)
            {
                var wisp = ((PlayerMobile)e.Crafter).Wisp;
                if (wisp == null)
                    return;

                if (e.Tool != null && (e.Tool is SmithHammer || e.Tool is SledgeHammer))
                {
                    wisp.CraftaBS();
                }
                else if (e.Tool != null && (e.Tool is SewingKit))
                {
                    wisp.Tailora();
                }
            }
        }

        private static void EventSink_ResourceHarvestSuccess(ResourceHarvestSuccessEventArgs e)
        {
            var player = e.Harvester as PlayerMobile;
            if (e.Resource.GetType() == typeof(IronOre))
            {
                if (player.Wisp != null)
                {
                    player.Wisp.Minera();
                }
            }
        }

        public PlayerMobile Jogador { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Passo { get; set; }

        [Constructable]
        public WispGuia(PlayerMobile player)
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.05, 0.05)
        {
            this.Name = "Fada dos Noobs";
            this.Body = 58;
            this.Jogador = player;
            this.SetStr(100, 100);
            this.SetDex(196, 225);
            this.SetInt(196, 225);

            this.SetHits(1000);

            this.SetDamage(1, 2);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Energy, 50);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 20, 40);
            this.SetResistance(ResistanceType.Cold, 10, 30);
            this.SetResistance(ResistanceType.Poison, 5, 10);
            this.SetResistance(ResistanceType.Energy, 50, 70);

            this.SetSkill(SkillName.EvalInt, 80.0);
            this.SetSkill(SkillName.Magery, 80.0);
            this.SetSkill(SkillName.MagicResist, 80.0);
            this.SetSkill(SkillName.Tactics, 80.0);
            this.SetSkill(SkillName.Wrestling, 80.0);

            this.Fame = 4000;
            this.Karma = 0;

            this.VirtualArmor = 40;
        }

        public WispGuia(Serial serial)
            : base(serial)
        {
        }

        public void Fala(string msg)
        {
            if (this.Jogador != null)
                this.PrivateOverheadMessage(MessageType.Regular, 72, false, msg, this.Jogador.NetState);
            this.Jogador.PlaySound(466);
        }

        Point3D Coxeiro = new Point3D(1300, 1399, 0);
        Point3D Ferreiro = new Point3D(1334, 1343, 10);
        Point3D Zeh = new Point3D(1405, 1337, 10);
        Point3D Orcs = new Point3D(1450, 1284, 10);
        Point3D Mago = new Point3D(1235, 1362, 10);
        Point3D Banco = new Point3D(1346, 1375, 10);
        Point3D Jill = new Point3D(1270, 1363, 10);
        Point3D Mina = new Point3D(1313, 1315, 0);
        Point3D Tailor = new Point3D(1241, 1382, 0);
        Point3D Bigorna = new Point3D(1317, 1344, 0);
        Point3D Alavanca = new Point3D(5457, 140, 20);
        Point3D Ank = new Point3D(5757, 234, 20);

        public int PEGAR_CAVALO = 0;
        public int FERREIRO = 1;
        public int TEMPLATE = 2;
        public int BANCO = 9;
        public int PALADINO_1 = 11;
        public int PALADINO_2 = 12;
        public int QUEST = 3;
        public int QUEST_MATOU = 4;
        public int QUEST_FIM = 5;

        public int ALAVANCA = 40;
        public int SAIPROT = 41;
        public int TESOURO = 42;
        public int GUARDAR_BANCO = 43;

        public int PEGAR_MAGE = 6;
        public int MATAR_ORC = 7;
        public int FALANDO = 8;
        public int PEGAR_QUEST_EXODO = 10;

        // CAMINHO WORKER
        public int MINE = 20;
        public int GETORE = 21;

        public int SMELT = 22;
        public int MACE = 23;
        public int SELL = 24;
        public int PLANT = 25;
        public int TAILOR = 26;

        public int QUEST_TAMER = 30;

        public int RandomFala = 0;

        public string[] Falas = new String[] {
            "Sabia que so voce pode me ver ? Hi hi"
        };

        public void VerificaLocal()
        {
            if (Passo == PEGAR_CAVALO && Coxeiro.GetDistance(Jogador.Location) < 6)
            {
                Fala("Tome aqui, um cavalo !");
                Fala("Clique duas vezes no cavalo para montar !");
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                var c = new Horse();
                c.SetControlMaster(Jogador);
                c.MoveToWorld(this.Location, this.Map);
                Passo = FERREIRO;
            }
            if (Passo == QUEST_TAMER && Coxeiro.GetDistance(Jogador.Location) < 6)
            {
                Fala("Muito bom, agora basta pegar a quest do treinador de animais");
                Fala("Complete a quest do treinador de animais para aprender como funciona !");
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                Passo = FALANDO;
            }
            if (Passo == GUARDAR_BANCO && Banco.GetDistance(Jogador.Location) < 6)
            {
                Fala("Guarde seu loot e vamos para a proxima aventura !!");
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                Passo = PEGAR_MAGE;
            }
            if (Passo == SAIPROT && Ank.GetDistance(Jogador.Location) < 10)
            {
                Fala("Eu lembro daqui !!! Existe um monstro seboso nesse lugar que come items !");
                Fala("Ei, que tal mata-lo ?? Quem sabe poderemos achar algo que ele comeu !");
                Passo = TESOURO;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
            }
            else if (Passo == FERREIRO && Ferreiro.GetDistance(Jogador.Location) < 6)
            {
                Fala("Digite 'Comprar' perto do NPC para comprar items. Tome algumas moedas aqui.");
                Jogador.Backpack.DropItem(new Gold(500));
                Jogador.PlaySound(0x2E6);
                Fala("Se voce tiver skills de trabalho, pode falar 'trabalho' para trabalhar pro NPC ou 'recompensas'.");
                Passo = 2;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }

                SetCooldown("passo", TimeSpan.FromSeconds(10));
            }
            else if (Passo == MINE && Mina.GetDistance(Jogador.Location) < 6)
            {
                Fala("Abra seu paperdoll e sua mochila, depois arraste sua picareta para sua mao.");
                Fala("Voce pode clicar 2x na picareta e depois 2x no chao para minerar !!");
                Fala("Vamos la, pegue alguns minerios de ferro !!");
                Passo = GETORE;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                SetCooldown("passo", TimeSpan.FromSeconds(20));
            }
            else if (Passo == ALAVANCA && Alavanca.GetDistance(Jogador.Location) < 6)
            {
                Fala("Acho que eh esta alavanca que o zeh estava se referindo !!.");
                Fala("Que exitante !");
                Passo = SAIPROT;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                SetCooldown("passo", TimeSpan.FromSeconds(20));
            }
        }

        public override void OnThink()
        {
            if (Jogador == null)
            {
                if(ControlMaster != null && ControlMaster is PlayerMobile)
                {
                    Jogador = (PlayerMobile)ControlMaster;
                    Jogador.Wisp = this;
                    if (!IsPetFriend(Jogador))
                        AddPetFriend(Jogador);
                    this.AIObject.EndPickTarget(Jogador, Jogador, OrderType.Follow);
                    this.AceitaOrdens = false;
                }  else
                {
                    return;
                }    
            }

            if (Jogador.Profession == 0)
                return;

            if (IsCooldown("fala"))
                return;

            if (IsCooldown("pensa"))
                return;

            var dist = this.GetDistance(Jogador);
            if(Shard.DebugEnabled)
            {
                Shard.Debug("Distancia do mestre " +Jogador.Name+": " + dist, this);
            }
            if (dist > 20)
            {
                this.DebugSay("Teleportando");
                this.MoveToWorld(Jogador.Location, Jogador.Map);
            }
            else
            {
                this.DebugSay("Seguindo");
                this.AIObject.EndPickTarget(Jogador, Jogador, OrderType.Follow);
            }

            SetCooldown("pensa", TimeSpan.FromSeconds(3));
            SetHits(1000);

            VerificaLocal();

            if(Jogador.Region is DungeonRegion || Jogador.Region is DungeonGuardedRegion)
            {
                if(!Jogador.HasAction(typeof(LightCycle))) {
                    var spell = new NightSightSpell(this, null);
                    spell.InstantTarget = Jogador;
                    spell.Cast();
                }
            }

            if (!IsCooldown("passo"))
            {
                SetCooldown("passo", TimeSpan.FromSeconds(30));
                if (Passo == PEGAR_CAVALO)
                {
                    Fala("Ei, o que acha de conseguir um cavalo ? Siga a seta no canto do mapa, eu arrumo um pra voce !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Coxeiro);
                    Jogador.QuestArrow.Update();
                }
                if (Passo == FERREIRO)
                {
                    Fala("Agora, vamos aprender a comprar equipamento !");
                    Fala("Siga ate o NPC Ferreiro ! Veja a setinha novamente no canto da sua tela !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    Jogador.QuestArrow.Update();
                }
                else if (Passo == GETORE)
                {
                    Fala("Voce pode clicar 2x na picareta e depois 2x no chao para minerar !!");
                }
                else if (Passo == TESOURO)
                {
                    if(Jogador.Region == null || Jogador.Region.Name == null || !Jogador.Region.Name.Contains("Endium"))
                    {
                        Fala("Ei, vamos explorar a dungeon do esgoto mais a fundo, abrindo aquela alavanca na sala do pentagrama !");
                    } else
                    {
                        Fala("Existe um monstro por aqui, um monstro seboso devorador de pessoas...");
                    }
                }
                else if (Passo == GETORE)
                {
                    Fala("Vamos explorar, quem sabe nao achamos algo legal !");
                }
                if (Passo == TEMPLATE)
                {
                    if (Jogador.Profession == StarterKits.MERC)
                    {
                        Fala("Agora, vamos aprender a minerar ! Que tal ir a mina ?");
                        Passo = MINE;
                        Jogador.QuestArrow = new QuestArrow(Jogador, Mina);
                        Jogador.QuestArrow.Update();
                    }     
                    else
                    {
                        Passo = BANCO;
                    }
                    //Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    //Jogador.QuestArrow.Update();
                }

                if (Passo == BANCO)
                {
                    Fala("Vamos ao banco agora. La voce guarda todo seu dinheiro.");
                    Fala("Va perto do banco e fale 'banco' para abrir sua caixa bancaria.");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Banco);
                    Jogador.QuestArrow.Update();
                    //Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    //Jogador.QuestArrow.Update();
                }

                if (Passo == SMELT)
                {
                    Fala("Va proximo de uma forja e clique 2x nos minerios para fundi-los em barras.");
                    Fala("Quando voce tiver skill em mineracao 60 ou mais ira encontrar outros tipos de minerios, hihi.");
                    //Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    //Jogador.QuestArrow.Update();
                    if (Jogador.QuestArrow != null)
                    {
                        Jogador.QuestArrow.Stop();
                        Jogador.QuestArrow = null;
                    }
                }

                if (Passo == QUEST)
                {
                    if (QuestHelper.HasCompleted(Jogador, typeof(SapatoLindoQ))) {
                        Passo = ALAVANCA;
                        return;
                    }

                    Fala("Siga a setinha e clique 2x no NPC para pegar a missao !");

                    Jogador.QuestArrow = new QuestArrow(Jogador, Zeh);
                    Jogador.QuestArrow.Update();
                }

                if (Passo == QUEST_MATOU)
                {
                    if(QuestHelper.HasCompleted(Jogador, typeof(SapatoLindoQ))) {
                        Passo = ALAVANCA;
                        return;
                    }

                    if (Jogador.Region == null || !(Jogador.Region is DungeonGuardedRegion))
                    {
                        Fala("A dungeon iniciante fica perto do NPC ze, perto do riacho a direita da cidade. Vamos la tentar recuperar o sapato do ze ?");
                    }
                    else
                    {
                        switch (Utility.Random(2))
                        {
                            case 0: Fala("A dungeon iniciante tem protecao de guardas, mas por isto os monstros nao tem nada de valor. Se quiser dinheiro vamos em outra dungeon depois."); break;
                            case 1: Fala("Voce upa skills mais rapido em dungeons e mais devagar em cidades. Upa mais rapido ainda matando mobs !"); break;
                        }
                    }
                }

                if (Passo == QUEST_FIM)
                {
                    if (QuestHelper.HasCompleted(Jogador, typeof(SapatoLindoQ))) {
                        Passo = ALAVANCA;
                        return;
                    }

                    Fala("Volte a cidade e arraste o sapato que voce pegou no mago para o ze para pegar a recompensa !");
                }

                else if (Passo == MACE)
                {
                    Fala("Encontre uma bigorna e use seu martelo de ferreiro para fazer algo !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Bigorna);
                    Jogador.QuestArrow.Update();
                }
                else if (Passo == GUARDAR_BANCO)
                {
                    Fala("Vamos ao banco da cidade guardar o loot ?");
                }
                else if (Passo == PLANT)
                {
                    Fala("Plante as sementes que lhe dei !");
                    Fala("Voce planta clicando 2x na semente depois 2x em um gramado");
                }
                else if (Passo == SELL)
                {
                    Fala("Vamos ao ferreiro vender o que voce criou !");
                    Fala("Digite 'vender' para o ferreiro para vender o item !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    Jogador.QuestArrow.Update();
                }
                else if (Passo == QUEST_TAMER)
                {
                    Fala("Agora que voce sabe os basicos de combate, que tal aprimorar suas habilidades de domador ?");
                    Fala("Va ate o treinador de animais e pegue a missao dele !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Coxeiro);
                    Jogador.QuestArrow.Update();
                }
                else if (Passo == ALAVANCA)
                {
                    if(Jogador.Region == null || !(Jogador.Region is DungeonGuardedRegion))
                    {
                        Fala("O Zeh te deu uma dica de algo secreto na dungeon que voce estava, que tal ir verificar no esgoto novamente ?");
                        if (Jogador.QuestArrow != null)
                        {
                            Jogador.QuestArrow.Stop();
                            Jogador.QuestArrow = null;
                        }
                    } else
                    {
                        Fala("Parece que o zeh falou algo sobre uma teia de aranha e um pentagrama...");
                        Jogador.QuestArrow = new QuestArrow(Jogador, Alavanca);
                        Jogador.QuestArrow.Update();
                    }
                }
                else if (Passo == PEGAR_MAGE)
                {
                    if (Jogador.Skills.Magery.Value >= 45)
                    {
                        if (Jogador.Profession == StarterKits.TAMER)
                            Passo = QUEST_TAMER;
                        else
                            Passo = MATAR_ORC;
                    }
                    else
                    {
                        Fala("Antes de ir para a dungeon dos orcs, voce vai precisar aprender usar magias !.");
                        Fala("Va ate um mago na cidade, clique com botao direito nele, Train, depois Magery, depois jogue 500 moedas em cima dele.");
                        Jogador.QuestArrow = new QuestArrow(Jogador, Mago);
                        Jogador.QuestArrow.Update();
                    }
                }

                if (Passo == MATAR_ORC)
                {
                    if (Jogador.Region != null && Jogador.Region is DungeonRegion)
                    {
                        Fala("Os monstros aqui tem peculiaridades, voce tem que aprender como combate-las !");
                        Fala("Tente envenenar os orcs para eles nao se curarem !");
                    }
                    else
                    {
                        Fala("Certo, vamos a dungeon dos orcs agora ! Tome cuidado !");
                        Fala("Certifique-se que voce trouxe bandagens para se curar e tem a magia Poison no seu livro de magias pronta para usar !");
                        Jogador.QuestArrow = new QuestArrow(Jogador, Orcs);
                        Jogador.QuestArrow.Update();
                    }
                } else if (Passo == TAILOR)
                {
                    Fala("Use os algodoes perto da roda de tecer, depois os fios perto da maquina de tecer para fabricar tecido.");
                    Fala("Depois use um kit de costura para criar algo");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Tailor);
                    Jogador.QuestArrow.Update();
                }

                if (Passo == FALANDO || Passo == PEGAR_QUEST_EXODO)
                {
                    if (!IsCooldown("falachata"))
                    {
                        SetCooldown("falachata", TimeSpan.FromMinutes(5));
                        switch (Utility.Random(10))
                        {
                            case 0: Fala("Upe sempre em dungeons, voce aprende as skills mais rapido matando monstros !"); break;
                            case 1: Fala("Quando tiver 70 nas skills das armas, voce pode usar as habilidades de arma clicando no seu paperdoll (ALT + P) e depois no livro lilas no seu pe. !"); break;
                            case 2: Fala("Sabia que as moedas de ouro , quando usando .grupo, sao mais faceis de conseguir ?"); break;
                            case 3: Fala("Voce pode formar grupos usando o comando .grupo, o ouro de dungeons sera dividido entre o grupo !"); break;
                            case 4: Fala("Se voce quiser trabalhar, voce pode fazer Ordens de Compra nos npcs se tiver skills de trabalho !"); break;
                            case 5: Fala("Sabia que voce pode ter sua casa e sua fazenda ? Mas vai precisar juntar dinheiro..."); break;
                            case 6: Fala("Ganhei dinheiro trabalhando ou matando monstros, voce que sabe..."); break;
                            case 7: Fala("Voce pode minerar na mina dos orcs, mas vai precisar fazer com que eles nao te ataquem..."); break;
                            case 8: Fala("Uma boa maneira de se juntar um dinheiro inicial e a skill BEGGING..."); break;
                            case 9: Fala("Fale com a NPC JILL no centro da cidade para pegar uma quest epica!"); break;
                        }
                    }
                    if (Passo == PEGAR_QUEST_EXODO && !IsCooldown("fala2"))
                    {
                        SetCooldown("fala2", TimeSpan.FromMinutes(1));
                        Fala("E se quiser uma aventura de verdade, fale com a Jill no centro da cidade de Rhodes");
                    }
                }
            }
        }

        public void Planta ()
        {
            if (Passo != PLANT)
                return;
            Fala("Otimo ! Isso pode demorar entre 1 e 2 horas para crescer...");
            Fala("Voce pode usar o algodao no alfaiate para criar tecido, tome alguns algodoes, e vamos la !");
            Fala("So nao se esqueca de sua planta !");
            Jogador.AddToBackpack(new Cotton(30));
            Passo = TAILOR;
          
        }

        public void Vende()
        {
            if (Passo != SELL)
                return;

            Fala("Assim voce pode conseguir um dinheirinho...");
            Fala("Mas vamos la vou lhe ensinar outras maneiras de ganhar dinheiro !");
            Fala("Tome aqui algumas sementes para voce plantar.");
            Jogador.AddToBackpack(new CottonSeeds());
            Jogador.AddToBackpack(new CottonSeeds());
            Passo = PLANT;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void CraftaBS()
        {
            if (Passo != MACE)
                return;

            Passo = SELL;
            Fala("Otimo, agora devemos ir ao NPC Ferreiro vender isso !");
            Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
            Jogador.QuestArrow.Update();
        }

        public void Minera()
        {
            if (Passo != GETORE)
                return;
            Passo = SMELT;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void Tailora()
        {
            if (Passo != TAILOR)
                return;
            Fala("Muito bom ! Voce ja sabe as bases de como trabalhar ! Agora so focar em ganhar dinheiro !!");
            Passo = FALANDO;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void Smelta()
        {
            if (Passo != SMELT)
                return;
            Fala("Ui, que fogaum, hi hi");
            Fala("Como voce eh inexperiente, talvez perca um pouco do minerio. Agora encontre uma bigorna e faca uma maca de ferro.");
            Passo = MACE;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void FalaJill()
        {
            if (Passo != PEGAR_QUEST_EXODO)
                return;
            Passo = FALANDO;
            Fala("Acho que nessa aventura voce vai precisar de pelo menos +2 pessoas ! Hihi");
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void AbreBanco()
        {
            if (Passo == BANCO)
            {
                Fala("muito bem ! Voce pode transformar seu dinheiro em cheques falando 'cheque <valor>' e voltar o cheque a dinheiro depois.");
                Fala("Mas deixe isso pra depois, vamos para um pouco de adrenalina !");
                Jogador.Backpack.DropItem(new Gold(500));
                Jogador.PlaySound(0x2E6);
                Passo = QUEST;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
            }
        }

        public void MataMerda()
        {
            if(Passo == TESOURO)
            {
                Fala("Muito bom !!!! Voce eh muito bom !!!!");
                Fala("Veja o que pode encontrar nesse corpo e vamos a cidade para guardar no banco !!");
                Passo = GUARDAR_BANCO;
            }
        }

        public void MataOrc()
        {
            if (Passo == MATAR_ORC)
            {
                Fala("Uau voce conseguiu matar um ! Se treinar podera ficar muito forte !");
                Fala("Mas se quiser um desafio de verdade, fale com a Jill no centro da codade de Rhodes, hihi");

                Passo = PEGAR_QUEST_EXODO;

                Jogador.QuestArrow = new QuestArrow(Jogador, Jill);
                Jogador.QuestArrow.Update();
            }
        }

        public void EntregaSapato()
        {
            if (Passo == QUEST_FIM)
            {
                Fala("Voce e muito bom ! Tome mais algumas moedas de ouro, talvez va precisar. Lembre-se de coloca-las no Banco !!");
                Fala("Parece que o ze te deu uma dica de algo secreto...");
                Jogador.Backpack.DropItem(new Gold(500));
                Jogador.PlaySound(0x2E6);
                Passo = ALAVANCA;
            }
        }

        public void MataMago()
        {
            if (Passo == QUEST_MATOU)
            {
                Fala("Muito bom ! Parece que os sapatos do ze estavam com esse mago !");
                Fala("Volte a cidade e arraste o sapato para o ze para pegar a recompensa !");
                Passo = 5;
            }
        }

        public void QuestNoob()
        {
            if (Passo == QUEST)
            {
                Fala("Muito bom, vamos a dungeon agora, a entrada e logo na caverna acima !!! ");
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                Passo = 4;
            }
        }

        public void TrocaTemplate(int template)
        {
            if (Passo == TEMPLATE)
            {
                if (template == 1)
                {
                    Fala("Muito bem, guerreiros usam espadas tome aqui uma. Voce tambem pode comprar no Ferreiro falando 'Comprar' perto dele mas a qualidade dos NPCS nao e muito boa...");
                    Jogador.ClearHands();
                    var espada = new Longsword();
                    espada.Name = "Espada da Fada";
                    var som = espada.GetLiftSound(Jogador);
                    Jogador.PlaySound(som);
                    espada.Quality = ItemQuality.Exceptional;
                    Jogador.EquipItem(espada);
                    Passo = BANCO;
                    SetCooldown("passo", TimeSpan.FromSeconds(60));
                }
                else
                {
                    Fala("Essa template e boa tambem, mas nao seria melhor escolher Guerreiro por hora ?");
                    Fala("Crie outra template como GUERREIRO");
                }
            }
        }


        public void ResetaCds()
        {
            SetCooldown("pensa", TimeSpan.FromMilliseconds(1));
            SetCooldown("fala", TimeSpan.FromMilliseconds(1));
            SetCooldown("passo", TimeSpan.FromMilliseconds(1));
            SetCooldown("falachata", TimeSpan.FromMilliseconds(1));
        }

        public override void OnDoubleClick(Mobile from)
        {
            if(from is PlayerMobile)
                from.SendGump(new GumpFada((PlayerMobile)from));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(Passo);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
            Passo = reader.ReadInt();
            var master = GetMaster();
            if (master != null && master is PlayerMobile)
            {
                var p = (PlayerMobile)master;
                p.Wisp = this;
                this.Jogador = p;
                if (!IsPetFriend(p))
                    AddPetFriend(p);
                this.AIObject.EndPickTarget(p, p, OrderType.Follow);
                this.AceitaOrdens = false;
            }
            else
            {
                this.Delete();
            }
        }
    }
}
