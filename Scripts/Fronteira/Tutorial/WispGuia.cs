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
    public class WispGuia : BaseCreature, IConditionalVisibility
    {
        public static void Configure()
        {
            if (Shard.RP)
                return;

            EventSink.ResourceHarvestSuccess += EventSink_ResourceHarvestSuccess;
            EventSink.CraftSuccess += EventSink_CraftSuccess;
        }

        public virtual void Moongate() { }
        public virtual void QuestCasa() { }
        public virtual void CompletaJill() { }


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
            this.IgnoreMobiles = true;
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

        Point3D Coxeiro = new Point3D(3526, 2577, 7);
        Point3D Ferreiro = new Point3D(3469, 2540, 10);

        Point3D Dg = new Point3D(3509, 2747, 0);
        Point3D DgIn = new Point3D(5950, 342, -22);
        Point3D Arena = new Point3D(3792, 2771, 6);

        Point3D Zeh = new Point3D(3506, 2476, 28);
        Point3D Orcs = new Point3D(1450, 1284, 10);
        Point3D Mago = new Point3D(1235, 1362, 10);
        Point3D Banco = new Point3D(1346, 1375, 10);
        Point3D Jill = new Point3D(1270, 1363, 10);
        Point3D Mina = new Point3D(1313, 1315, 0);
        Point3D Tailor = new Point3D(1241, 1382, 0);
        Point3D Bigorna = new Point3D(1317, 1344, 0);
        Point3D Alavanca = new Point3D(5457, 140, 20);
        Point3D Ank = new Point3D(5757, 234, 20);

        public int PEGAR_CAVALO = 110;
        public int FERREIRO = 111;
        public int TEMPLATE = 112;
        public int BANCO = 119;
        public int PALADINO_1 = 1111;
        public int PALADINO_2 = 1112;
        public int QUEST = 113;
        public int QUEST_MATOU = 114;
        public int QUEST_FIM = 115;

        public int ALAVANCA = 1140;
        public int SAIPROT = 1141;
        public int TESOURO = 1142;
        public int GUARDAR_BANCO = 1143;

        public int PEGAR_MAGE = 116;
        public int MATAR_ORC = 117;
        public int FALANDO = 118;
        public int PEGAR_QUEST_EXODO = 1110;

        // CAMINHO WORKER
        public int MINE = 1120;
        public int GETORE = 1121;

        public int SMELT = 1122;
        public int MACE = 1123;
        public int SELL = 1124;
        public int PLANT = 1125;
        public int TAILOR = 1126;

        public int QUEST_TAMER = 1130;

        public int RandomFala = 0;

        public string[] Falas = new String[] {
            "Sabia que so voce pode me ver ? Hi hi"
        };

        public void VerificaLocal()
        {
            if (Jogador.PassoWispGuia == PEGAR_CAVALO && Coxeiro.GetDistance(Jogador.Location) < 6)
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
                Jogador.PassoWispGuia = FERREIRO;
            }
            if (Jogador.PassoWispGuia == QUEST_TAMER && Coxeiro.GetDistance(Jogador.Location) < 6)
            {
                Fala("Muito bom, agora basta pegar a quest do treinador de animais");
                Fala("Complete a quest do treinador de animais para aprender como funciona !");
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                Jogador.PassoWispGuia = FALANDO;
            }
            if (Jogador.PassoWispGuia == GUARDAR_BANCO && Banco.GetDistance(Jogador.Location) < 6)
            {
                Fala("Guarde seu loot e vamos para a proxima aventura !!");
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                Jogador.PassoWispGuia = PEGAR_MAGE;
            }
            if (Jogador.PassoWispGuia == SAIPROT && Ank.GetDistance(Jogador.Location) < 10)
            {
                Fala("Eu lembro daqui !!! Existe um monstro seboso nesse lugar que come items !");
                Fala("Ei, que tal mata-lo ?? Quem sabe poderemos achar algo que ele comeu !");
                Jogador.PassoWispGuia = TESOURO;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
            }
            else if (Jogador.PassoWispGuia == FERREIRO && Ferreiro.GetDistance(Jogador.Location) < 6)
            {
                Fala("Digite 'Comprar' perto do NPC para comprar items. Tome algumas moedas aqui.");
                Jogador.Backpack.DropItem(new Gold(500));
                Jogador.PlaySound(0x2E6);
                Fala("Se voce tiver skills de trabalho, pode falar 'trabalho' para trabalhar pro NPC ou 'recompensas'.");
                Jogador.PassoWispGuia = 2;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }

                SetCooldown("Jogador.PassoWispGuia", TimeSpan.FromSeconds(10));
            }
            else if (Jogador.PassoWispGuia == MINE && Mina.GetDistance(Jogador.Location) < 6)
            {
                Fala("Abra seu paperdoll e sua mochila, depois arraste sua picareta para sua mao.");
                Fala("Voce pode clicar 2x na picareta e depois 2x no chao para minerar !!");
                Fala("Vamos la, pegue alguns minerios de ferro !!");
                Jogador.PassoWispGuia = GETORE;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                SetCooldown("Jogador.PassoWispGuia", TimeSpan.FromSeconds(20));
            }
            else if (Jogador.PassoWispGuia == ALAVANCA && Alavanca.GetDistance(Jogador.Location) < 6)
            {
                Fala("Acho que eh esta alavanca que o zeh estava se referindo !!.");
                Fala("Que exitante !");
                Jogador.PassoWispGuia = SAIPROT;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                SetCooldown("Jogador.PassoWispGuia", TimeSpan.FromSeconds(20));
            }
        }

        public override void OnThink()
        {
            if (Jogador == null)
            {
                if (ControlMaster != null && ControlMaster is PlayerMobile)
                {
                    Jogador = (PlayerMobile)ControlMaster;
                    Jogador.Wisp = this;
                    if (!IsPetFriend(Jogador))
                        AddPetFriend(Jogador);
                    this.AIObject.EndPickTarget(Jogador, Jogador, OrderType.Follow);
                    this.AceitaOrdens = false;
                }
                else
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
            if (Shard.DebugEnabled)
            {
                Shard.Debug("Distancia do mestre " + Jogador.Name + ": " + dist, this);
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

            if (Jogador.Region is DungeonRegion || Jogador.Region is DungeonGuardedRegion)
            {
                if (!Jogador.HasAction(typeof(LightCycle)))
                {
                    var spell = new NightSightSpell(this, null);
                    spell.InstantTarget = Jogador;
                    spell.Cast();
                }
            }

            if (!IsCooldown("Jogador.PassoWispGuia"))
            {
                SetCooldown("Jogador.PassoWispGuia", TimeSpan.FromSeconds(30));
                if (Jogador.PassoWispGuia == PEGAR_CAVALO)
                {
                    Fala("Ei, o que acha de conseguir um cavalo ? Siga a seta no canto do mapa, eu arrumo um pra voce !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Coxeiro);
                    Jogador.QuestArrow.Update();
                }
                if (Jogador.PassoWispGuia == FERREIRO)
                {
                    Fala("Agora, vamos aprender a comprar equipamento !");
                    Fala("Siga ate o NPC Ferreiro ! Veja a setinha novamente no canto da sua tela !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    Jogador.QuestArrow.Update();
                }
                else if (Jogador.PassoWispGuia == GETORE)
                {
                    Fala("Voce pode clicar 2x na picareta e depois 2x no chao para minerar !!");
                }
                else if (Jogador.PassoWispGuia == TESOURO)
                {
                    if (Jogador.Region == null || Jogador.Region.Name == null || !Jogador.Region.Name.Contains("Endium"))
                    {
                        Fala("Ei, vamos explorar a dungeon do esgoto mais a fundo, abrindo aquela alavanca na sala do pentagrama !");
                    }
                    else
                    {
                        Fala("Existe um monstro por aqui, um monstro seboso devorador de pessoas...");
                    }
                }
                else if (Jogador.PassoWispGuia == GETORE)
                {
                    Fala("Vamos explorar, quem sabe nao achamos algo legal !");
                }
                if (Jogador.PassoWispGuia == TEMPLATE)
                {
                    if (Jogador.Profession == StarterKits.MERC)
                    {
                        Fala("Agora, vamos aprender a minerar ! Que tal ir a mina ?");
                        Jogador.PassoWispGuia = MINE;
                        Jogador.QuestArrow = new QuestArrow(Jogador, Mina);
                        Jogador.QuestArrow.Update();
                    }
                    else
                    {
                        Jogador.PassoWispGuia = BANCO;
                    }
                    //Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    //Jogador.QuestArrow.Update();
                }

                if (Jogador.PassoWispGuia == BANCO)
                {
                    Fala("Vamos ao banco agora. La voce guarda todo seu dinheiro.");
                    Fala("Va perto do banco e fale 'banco' para abrir sua caixa bancaria.");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Banco);
                    Jogador.QuestArrow.Update();
                    //Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    //Jogador.QuestArrow.Update();
                }

                if (Jogador.PassoWispGuia == SMELT)
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

                if (Jogador.PassoWispGuia == QUEST)
                {
                    if (QuestHelper.HasCompleted(Jogador, typeof(SapatoLindoQ)))
                    {
                        Jogador.PassoWispGuia = ALAVANCA;
                        return;
                    }

                    Fala("Siga a setinha e clique 2x no NPC para pegar a missao !");

                    Jogador.QuestArrow = new QuestArrow(Jogador, Zeh);
                    Jogador.QuestArrow.Update();
                }

                if (Jogador.PassoWispGuia == QUEST_MATOU)
                {
                    if (QuestHelper.HasCompleted(Jogador, typeof(SapatoLindoQ)))
                    {
                        Jogador.PassoWispGuia = ALAVANCA;
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

                if (Jogador.PassoWispGuia == QUEST_FIM)
                {
                    if (QuestHelper.HasCompleted(Jogador, typeof(SapatoLindoQ)))
                    {
                        Jogador.PassoWispGuia = ALAVANCA;
                        return;
                    }

                    Fala("Volte a cidade e arraste o sapato que voce pegou no mago para o ze para pegar a recompensa !");
                }

                else if (Jogador.PassoWispGuia == MACE)
                {
                    Fala("Encontre uma bigorna e use seu martelo de ferreiro para fazer algo !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Bigorna);
                    Jogador.QuestArrow.Update();
                }
                else if (Jogador.PassoWispGuia == GUARDAR_BANCO)
                {
                    Fala("Vamos ao banco da cidade guardar o loot ?");
                }
                else if (Jogador.PassoWispGuia == PLANT)
                {
                    Fala("Plante as sementes que lhe dei !");
                    Fala("Voce planta clicando 2x na semente depois 2x em um gramado");
                }
                else if (Jogador.PassoWispGuia == SELL)
                {
                    Fala("Vamos ao ferreiro vender o que voce criou !");
                    Fala("Digite 'vender' para o ferreiro para vender o item !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
                    Jogador.QuestArrow.Update();
                }
                else if (Jogador.PassoWispGuia == QUEST_TAMER)
                {
                    Fala("Agora que voce sabe os basicos de combate, que tal aprimorar suas habilidades de domador ?");
                    Fala("Va ate o treinador de animais e pegue a missao dele !");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Coxeiro);
                    Jogador.QuestArrow.Update();
                }
                else if (Jogador.PassoWispGuia == ALAVANCA)
                {
                    if (Jogador.Region == null || !(Jogador.Region is DungeonGuardedRegion))
                    {
                        Fala("O Zeh te deu uma dica de algo secreto na dungeon que voce estava, que tal ir verificar no esgoto novamente ?");
                        if (Jogador.QuestArrow != null)
                        {
                            Jogador.QuestArrow.Stop();
                            Jogador.QuestArrow = null;
                        }
                    }
                    else
                    {
                        Fala("Parece que o zeh falou algo sobre uma teia de aranha e um pentagrama...");
                        Jogador.QuestArrow = new QuestArrow(Jogador, Alavanca);
                        Jogador.QuestArrow.Update();
                    }
                }
                else if (Jogador.PassoWispGuia == PEGAR_MAGE)
                {
                    if (Jogador.Skills.Magery.Value >= 45)
                    {
                        if (Jogador.Profession == StarterKits.TAMER)
                            Jogador.PassoWispGuia = QUEST_TAMER;
                        else
                            Jogador.PassoWispGuia = MATAR_ORC;
                    }
                    else
                    {
                        Fala("Antes de ir para a dungeon dos orcs, voce vai precisar aprender usar magias !.");
                        Fala("Va ate um mago na cidade, clique com botao direito nele, Train, depois Magery, depois jogue 500 moedas em cima dele.");
                        Jogador.QuestArrow = new QuestArrow(Jogador, Mago);
                        Jogador.QuestArrow.Update();
                    }
                }

                if (Jogador.PassoWispGuia == MATAR_ORC)
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
                }
                else if (Jogador.PassoWispGuia == TAILOR)
                {
                    Fala("Use os algodoes perto da roda de tecer, depois os fios perto da maquina de tecer para fabricar tecido.");
                    Fala("Depois use um kit de costura para criar algo");
                    Jogador.QuestArrow = new QuestArrow(Jogador, Tailor);
                    Jogador.QuestArrow.Update();
                }

                if (Jogador.PassoWispGuia == FALANDO || Jogador.PassoWispGuia == PEGAR_QUEST_EXODO)
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
                    if (Jogador.PassoWispGuia == PEGAR_QUEST_EXODO && !IsCooldown("fala2"))
                    {
                        SetCooldown("fala2", TimeSpan.FromMinutes(1));
                        Fala("E se quiser uma aventura de verdade, fale com a Jill no centro da cidade de Rhodes");
                    }
                }
            }
        }

        public void Planta()
        {
            return;
            if (Jogador.PassoWispGuia != PLANT)
                return;
            Fala("Otimo ! Isso pode demorar entre 1 e 2 horas para crescer...");
            Fala("Voce pode usar o algodao no alfaiate para criar tecido, tome alguns algodoes, e vamos la !");
            Fala("So nao se esqueca de sua planta !");
            Jogador.AddToBackpack(new Cotton(30));
            Jogador.PassoWispGuia = TAILOR;

        }

        public void Vende()
        {
            return;
            if (Jogador.PassoWispGuia != SELL)
                return;

            Fala("Assim voce pode conseguir um dinheirinho...");
            Fala("Mas vamos la vou lhe ensinar outras maneiras de ganhar dinheiro !");
            Fala("Tome aqui algumas sementes para voce plantar.");
            Jogador.AddToBackpack(new CottonSeeds());
            Jogador.AddToBackpack(new CottonSeeds());
            Jogador.PassoWispGuia = PLANT;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void CraftaBS()
        {
            return;
            if (Jogador.PassoWispGuia != MACE)
                return;

            Jogador.PassoWispGuia = SELL;
            Fala("Otimo, agora devemos ir ao NPC Ferreiro vender isso !");
            Jogador.QuestArrow = new QuestArrow(Jogador, Ferreiro);
            Jogador.QuestArrow.Update();
        }

        public void Minera()
        {
            return;
            if (Jogador.PassoWispGuia != GETORE)
                return;
            Jogador.PassoWispGuia = SMELT;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void Tailora()
        {
            return;
            if (Jogador.PassoWispGuia != TAILOR)
                return;
            Fala("Muito bom ! Voce ja sabe as bases de como trabalhar ! Agora so focar em ganhar dinheiro !!");
            Jogador.PassoWispGuia = FALANDO;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public void Smelta()
        {
            return;
            if (Jogador.PassoWispGuia != SMELT)
                return;
            Fala("Ui, que fogaum, hi hi");
            Fala("Como voce eh inexperiente, talvez perca um pouco do minerio. Agora encontre uma bigorna e faca uma maca de ferro.");
            Jogador.PassoWispGuia = MACE;
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public virtual void FalaJill()
        {
            return;
            if (Jogador.PassoWispGuia != PEGAR_QUEST_EXODO)
                return;
            Jogador.PassoWispGuia = FALANDO;
            Fala("Acho que nessa aventura voce vai precisar de pelo menos +2 pessoas ! Hihi");
            if (Jogador.QuestArrow != null)
            {
                Jogador.QuestArrow.Stop();
                Jogador.QuestArrow = null;
            }
        }

        public virtual void AbreBanco()
        {
            return;
            if (Jogador.PassoWispGuia == BANCO)
            {
                Fala("muito bem ! Voce pode transformar seu dinheiro em cheques falando 'cheque <valor>' e voltar o cheque a dinheiro depois.");
                Fala("Mas deixe isso pra depois, vamos para um pouco de adrenalina !");
                Jogador.Backpack.DropItem(new Gold(500));
                Jogador.PlaySound(0x2E6);
                Jogador.PassoWispGuia = QUEST;
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
            }
        }

        public virtual void MataMerda()
        {
            return;
            if (Jogador.PassoWispGuia == TESOURO)
            {
                Fala("Muito bom !!!! Voce eh muito bom !!!!");
                Fala("Veja o que pode encontrar nesse corpo e vamos a cidade para guardar no banco !!");
                Jogador.PassoWispGuia = GUARDAR_BANCO;
            }
        }

        public virtual void MataOrc()
        {
            return;
            if (Jogador.PassoWispGuia == MATAR_ORC)
            {
                Fala("Uau voce conseguiu matar um ! Se treinar podera ficar muito forte !");
                Fala("Mas se quiser um desafio de verdade, fale com a Jill no centro da codade de Rhodes, hihi");

                Jogador.PassoWispGuia = PEGAR_QUEST_EXODO;

                Jogador.QuestArrow = new QuestArrow(Jogador, Jill);
                Jogador.QuestArrow.Update();
            }
        }

        public virtual void EntregaSapato()
        {
            return;
            if (Jogador.PassoWispGuia == QUEST_FIM)
            {
                Fala("Voce e muito bom ! Tome mais algumas moedas de ouro, talvez va precisar. Lembre-se de coloca-las no Banco !!");
                Fala("Parece que o ze te deu uma dica de algo secreto...");
                Jogador.Backpack.DropItem(new Gold(500));
                Jogador.PlaySound(0x2E6);
                Jogador.PassoWispGuia = ALAVANCA;
            }
        }

        public virtual void MataMago()
        {
            return;
            if (Jogador.PassoWispGuia == QUEST_MATOU)
            {
                Fala("Muito bom ! Parece que os sapatos do ze estavam com esse mago !");
                Fala("Volte a cidade e arraste o sapato para o ze para pegar a recompensa !");
                Jogador.PassoWispGuia = 5;
            }
        }

        public virtual void QuestNoob()
        {
            return;
            if (Jogador.PassoWispGuia == QUEST)
            {
                Fala("Muito bom, vamos a dungeon agora, a entrada e logo na caverna acima !!! ");
                if (Jogador.QuestArrow != null)
                {
                    Jogador.QuestArrow.Stop();
                    Jogador.QuestArrow = null;
                }
                Jogador.PassoWispGuia = QUEST_MATOU;
            }
        }

        public void TrocaTemplate(int template)
        {
            return;
            if (Jogador.PassoWispGuia == TEMPLATE)
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
                    Jogador.PassoWispGuia = BANCO;
                    SetCooldown("Jogador.PassoWispGuia", TimeSpan.FromSeconds(60));
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
            SetCooldown("Jogador.PassoWispGuia", TimeSpan.FromMilliseconds(1));
            SetCooldown("falachata", TimeSpan.FromMilliseconds(1));
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile)
                from.SendGump(new GumpFada((PlayerMobile)from));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
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

        public bool CanBeSeenBy(PlayerMobile m)
        {
            return m != null && m.AccessLevel > AccessLevel.VIP || (m.Wisp != null && m.Wisp.Serial == this.Serial) || (Jogador != null && this.Jogador == m);
        }
    }
}
