#region AuthorHeader

#endregion AuthorHeader
using System;
using System.Collections;

namespace Server.Leilaum
{
    /// <summary>
    /// Provides access to localized text used by the system
    /// </summary>
    public class StringTable
    {
        private Hashtable m_Table;

        public StringTable()
        {
            m_Table = new Hashtable();
            m_Table.Add(0, "Servico de Delivery");
            m_Table.Add(1, "Entregando...");
            m_Table.Add(2, "Coloque o dinheiro no banco");
            m_Table.Add(3, "Coloque o item no banco");
            m_Table.Add(4, "Item");
            m_Table.Add(5, "Ouro");
            m_Table.Add(6, "Visualizar janela de leiloes");
            m_Table.Add(7, "Fechar");
            m_Table.Add(8, "Bem vindo a casa de leiloes");
            m_Table.Add(9, "Vender Item");
            m_Table.Add(10, "Ver Items");
            m_Table.Add(11, "Ver seus Items");
            m_Table.Add(12, "Ver seus leiloes");
            m_Table.Add(13, "Ver suas pendencias");
            m_Table.Add(14, "Sair");
            m_Table.Add(15, "O sistema de leiloes fechou.");
            m_Table.Add(16, "Procurar");
            m_Table.Add(17, "Ordenar");
            m_Table.Add(18, "Pag. {0}/{1}"); // Page 1/3 - used when displaying more than one page
            m_Table.Add(19, "Mostrando {0} items"); // {0} is the number of items displayed in an auction listing
            m_Table.Add(20, "Sem items para mostrar");
            m_Table.Add(21, "Anterior");
            m_Table.Add(22, "Proxima");
            m_Table.Add(23, "Wops, deu alguma merda...");
            m_Table.Add(24, "O item expirou - por favor atualize a janela.");
            m_Table.Add(25, "Sistema de mensagens");
            m_Table.Add(26, "Leilao:");
            m_Table.Add(27, "Ver detalhes");
            m_Table.Add(28, "Indisponivel");
            m_Table.Add(29, "Detalhes:");
            m_Table.Add(30, "Tempo restante para decisoes: {0} dias e {1} horas.");
            m_Table.Add(31, "Este leilao nao existe mais.");
            m_Table.Add(32, "Busca da casa de Leiloes");
            m_Table.Add(33, "Digite o que deseja buscar (em branco para tudo)");
            m_Table.Add(34, "Limite por tipo:");
            m_Table.Add(35, "Mapas");
            m_Table.Add(36, "Artefatos");
            m_Table.Add(37, "Power Scrolls");
            m_Table.Add(38, "Recursos");
            m_Table.Add(39, "Joias");
            m_Table.Add(40, "Armas");
            m_Table.Add(41, "Armaduras");
            m_Table.Add(42, "Escudos");
            m_Table.Add(43, "Reagentes");
            m_Table.Add(44, "Pocoes");
            m_Table.Add(45, "BOD (Large)");
            m_Table.Add(46, "BOD (Small)");
            m_Table.Add(47, "Cancelar");
            m_Table.Add(48, "Buscar apenas no resultado atual");
            m_Table.Add(49, "Sistema de Ordenamento");
            m_Table.Add(50, "Nome");
            m_Table.Add(51, "Ascendente");
            m_Table.Add(52, "Descendente");
            m_Table.Add(53, "Data");
            m_Table.Add(54, "Mais Antigos");
            m_Table.Add(55, "Mais Novos");
            m_Table.Add(56, "Tempo Restante");
            m_Table.Add(57, "Mais curto antes");
            m_Table.Add(58, "Mais longo antes");
            m_Table.Add(59, "Numero de lances");
            m_Table.Add(60, "Poucos antes");
            m_Table.Add(61, "Muitos antes");
            m_Table.Add(62, "Valor minimo para lance");
            m_Table.Add(63, "Menor antes");
            m_Table.Add(64, "Maior antes");
            m_Table.Add(65, "Valor do maior lance");
            m_Table.Add(66, "Cancelar ordenacao");
            m_Table.Add(67, "Item {0} de {1}"); // Number of items inside a container - auction view gump
            m_Table.Add(68, "Lance inicial");
            m_Table.Add(69, "Reserva");
            m_Table.Add(70, "Maior lance");
            m_Table.Add(71, "Sem lances ainda");
            m_Table.Add(72, "Web Link");
            m_Table.Add(73, "{0} Dias {1} Horas"); // 5 Days 2 Hours
            m_Table.Add(74, "{0} Horas"); // 18 Hours
            m_Table.Add(75, "{0} Minutos"); // 50 Minutes
            m_Table.Add(76, "{0} Segundos"); // 10 Seconds
            m_Table.Add(77, "Pendente");
            m_Table.Add(78, "N/A");
            m_Table.Add(79, "Lance neste item:");
            m_Table.Add(80, "Ver lances");
            m_Table.Add(81, "Descricao do dono");
            m_Table.Add(82, "Item Hue");
            m_Table.Add(83, "[Client 3D nao mostra o item hue]");
            m_Table.Add(84, "Este leilao esta fechado e nao aceita mais lances");
            m_Table.Add(85, "Lance invalido. Lance nao aceito.");
            m_Table.Add(86, "Historico de lances");
            m_Table.Add(87, "Quem");
            m_Table.Add(88, "Valor");
            m_Table.Add(89, "Voltar ao leilao");
            m_Table.Add(90, "Creatures Division");
            m_Table.Add(91, "Stable the pet");
            m_Table.Add(92, "Use this ticket to stable your pet.");
            m_Table.Add(93, "Stabled pets must be claimed"); // This and the following form one sentence
            m_Table.Add(94, "within a week time from the stable.");
            m_Table.Add(95, "Voce nao pagara por este servico.");
            m_Table.Add(96, "AUCTION SYSTEM TERMINATION");
            m_Table.Add(97, "<basefont color=#FFFFFF>Voce esta prestes a parar o sistema de leiloes rodando no servidor. Isto causara o fechamento de todos os leiloes. Todos os itens serao devolvidos aos respectivos donos e os donos dos maiores lances terao seu dinheiro devolvido. Voce tem certeza que gostaria de fazer isto?");
            m_Table.Add(98, "Sim, eu gostaria de parar o sistema de leiloes");
            m_Table.Add(99, "Nao, deixe o sistema rodando");
            m_Table.Add(100, "Configuracao de um novo leilao");
            m_Table.Add(101, "Duracao");
            m_Table.Add(102, "Dias");
            m_Table.Add(103, "Descricao (Opcional)");
            m_Table.Add(104, "Web Link (Opcional)");
            m_Table.Add(106, "Eu li os termos deste leilao e gostaria"); // This and the following form one sentence
            m_Table.Add(107, "de continuar e salvar este leilao.");
            m_Table.Add(108, "Cancelar e sair.");
            m_Table.Add(109, "O lance inicial deve ser de ao menos 1gp.");
            m_Table.Add(110, "O valor de reserva deve ser igual ou superior ao lance inicial.");
            m_Table.Add(111, "Um leilao deve durar ao menos {0} dias.");
            m_Table.Add(112, "Um leilao pode durar no maximo {0} dias.");
            m_Table.Add(113, "Por favor informe um nome para seu leilao.");
            m_Table.Add(114, "O valor de reserva informado e muito grande. Diminua-o ou aumente o lance inicial.");
            m_Table.Add(115, "O sistema esta fechado");
            m_Table.Add(116, "O item representado por este cheque nao existe mais por razoes fora da alcada do sistema de leiloes");
            m_Table.Add(117, "O conteudo deste cheque foi entregue em seu banco.");
            m_Table.Add(118, "Nao foi possivel adicionar o item ao seu banco. Verifique que ha espaco suficiente para aguenta-lo.");
            m_Table.Add(119, "Seus poderes divinos permitem voce acessar este cheque.");
            m_Table.Add(120, "Este cheque somente pode ser usado por seu dono.");
            m_Table.Add(121, "Voce nao deveria remover este item manualmente. Nunca.");
            m_Table.Add(122, "Cheque de ouro do sistema de leiloes");
            m_Table.Add(123, "Voce foi excedido no leilao de {0}. Seu lance era {1}.");
            m_Table.Add(124, "Sistema de leiloes parado. Retornando seu lance de {1} ouros por {0}");
            m_Table.Add(125, "Leilao de {0} foi cancelado por voce ou pelo dono. Devolvendo seu lance.");
            m_Table.Add(126, "Seu lance de {0} por {1} nao atingiu o valor de reserva e o dono decidiu nao aceitar sua oferta");
            m_Table.Add(127, "Este leilao estava em um estado pendente devido ao valor de reserva nao ter sido atingido ou um ou mais itens foram removidos. Nenhuma acao foi tomada pelas partes envolvidas e portanto foi finalizado.");
            m_Table.Add(128, "O leilao foi cancelado devido ao item leiloado ter sido removido do mundo.");
            m_Table.Add(129, "Voce vendeu {0} pelo sistema de leiloes. O lance mais alto foi {1}.");
            m_Table.Add(130, "{0} nao e uma razao valida para um leilao de um cheque de ouro");
            m_Table.Add(131, "Creature Check from the Auction System");
            m_Table.Add(132, "Item Check from the Auction System");
            m_Table.Add(133, "Seu leilao de {0} terminou sem lances.");
            m_Table.Add(134, "Seu leilao de {0} foi cancelado");
            m_Table.Add(135, "O sistema de leilao foi parado e seus itens leiloados retornados para voce. ({0})");
            m_Table.Add(136, "O leilao foi cancelado porque o item leiloado foi removido do mundo.");
            m_Table.Add(137, "Voce comprou {0} atraves do sistema de leiloes. Seu lance foi de {1}.");
            m_Table.Add(138, "{0} is not a valid reason for an auction item check");
            m_Table.Add(139, "Voce nao pode leiloar criaturas que nao sao suas.");
            m_Table.Add(140, "YVoce nao pode leiloar criaturas mortas");
            m_Table.Add(141, "Voce nao pode leiloar criaturas sumonadas");
            m_Table.Add(142, "You can't auction familiars");
            m_Table.Add(143, "Por favor retire a mochila de seu animal primeiro");
            m_Table.Add(144, "O animal representado por este cheque nao existe mais");
            m_Table.Add(145, "Desculpe-nos, estamos fechados no momento. Tente novamente mais tarde.");
            m_Table.Add(146, "Este item nao existe mais");
            m_Table.Add(147, "Control Slots : {0}<br>"); // For a pet
            m_Table.Add(148, "Bondable : {0}<br>");
            m_Table.Add(149, "Str : {0}<br>");
            m_Table.Add(150, "Dex : {0}<br>");
            m_Table.Add(151, "Int : {0}<br>");
            m_Table.Add(152, "Quantidade: {0}<br>");
            m_Table.Add(153, "Usos disponiveis : {0}<br>");
            m_Table.Add(154, "Spell : {0}<br>");
            m_Table.Add(155, "Cargas : {0}<br>");
            m_Table.Add(156, "Feito por {0}<br>");
            m_Table.Add(157, "Resource : {0}<br>");
            m_Table.Add(158, "Qualidade : {0}<br>");
            m_Table.Add(159, "Hit Points : {0}/{1}<br>");
            m_Table.Add(160, "Durabilidade : {0}<br>");
            m_Table.Add(161, "Pretecao: {0}<br>");
            m_Table.Add(162, "Cargas de veneno : {0} [{1}]<br>");
            m_Table.Add(163, "Alcance : {0}<br>");
            m_Table.Add(164, "Dano : {0}<br>");
            m_Table.Add(165, "Accurate<br>"); // Accuracy level, might want to leave as is
            m_Table.Add(166, "{0} Accurate<br>"); // Will become: Supremely Accurate/Extremely Accurate
            m_Table.Add(167, "Slayer : {0}<br>");
            m_Table.Add(168, "Map : {0}<br>");
            m_Table.Add(169, "Spell Count : {0}");
            m_Table.Add(170, "Localizacao invalida");
            m_Table.Add(171, "Invalido");
            m_Table.Add(172, "O item selecionado foi removido e sera guardado em custodia");
            m_Table.Add(173, "Voce cancelou o leilao e seu item foi devolvido");
            m_Table.Add(174, "Voce cancelou o leilao e seu animal foi devolvido");
            m_Table.Add(175, "You don't have enough control slots to bid on that creature");
            m_Table.Add(176, "Seu lance nao e alto o suficiente");
            m_Table.Add(177, "Seu lance nao alcanca o lance minimo");
            m_Table.Add(178, "Your stable is full. Please free some space before claiming this creature.");
            m_Table.Add(179, "Seu lance foi ultrapassado. Um cheque de leilao de {0} gp foi depositado em sua mochila ou banco. Veja os detalhes do leilao se quiser dar outro lance.");
            m_Table.Add(180, "Seu leilao terminou mas o maior lance nao alcancou o valor de reserva especificado. Voce agora tem a opcao de vender ou nao seu item.<br><br>O maior lance foi de {0}. Sua reserva era de {1}.");
            m_Table.Add(181, "<br><br>Alguns dos itens leiloados foram deletados durante o leilao. O comprador tera que aceitar o novo leilao antes de ser finalizado.");
            m_Table.Add(182, "Sim, eu gostaria de vender meu item mesmo que o valor de reserva nao foi atingido");
            m_Table.Add(183, "Nao, eu nao gostaria de vender meu item e quero ele de volta");
            m_Table.Add(184, "Seu lance nao atingiu o valor de reserva especificado pelo dono. O dono do item ira ter que decidir se vendera ou nao.<br><br>Seu lance foi de {1}. A reserva e de {2}.");
            m_Table.Add(185, "Feche este mensagem");
            m_Table.Add(186, "Voce participou e ganhou um leilao. Contudo, devido a fatores externos um ou mais items leiloados nao existem mais. Por favor revise o leilao usando o botao de visualizar detalhes e decida se gostaria de comprar mesmo assim ou nao.<br><br>Seu lance foi de {0}");
            m_Table.Add(187, "<br><br>Seu lance nao atingiu o valor de reserva especificado pelo dono. Ele nao tera que decidir se quer vender ou nao");
            m_Table.Add(188, "Sim, eu gostaria de comprar mesmo assim");
            m_Table.Add(189, "Nao, eu nao quero comprar e gostaria do meu dinheiro de volta");
            m_Table.Add(190, "Alguns dos items leiloados nao existem mais devido a fatores externos. O comprador ira decidir se ira comprar ou nao.");
            m_Table.Add(191, "Por favor selecione o item que voce gostaria de leiloar...");
            m_Table.Add(192, "Voce nao pode ter mais de {0} leiloes ativos em sua conta");
            m_Table.Add(193, "Voce somente pode leiloar items");
            m_Table.Add(194, "Voce nao pode leiloar isto");
            m_Table.Add(195, "Um dos items leiloados nao e identificado");
            m_Table.Add(196, "Um dos items dentro do recipiente nao e autorizado na casa de leiloes");
            m_Table.Add(197, "Voce nao pode vender recipientes com items dentro de outros recipientes");
            m_Table.Add(198, "Voce somente pode leiloar items que estao em sua mochila ou banco");
            m_Table.Add(199, "Voce nao possui dinheiro suficiente em seu banco para dar este lance");
            m_Table.Add(200, "O sistema de leiloes esta parado no momento");
            m_Table.Add(201, "Remover");
            m_Table.Add(202, "Voce deu lance em um leilao que foi removido pela administracao. Seu lance esta sendo devolvido a voce.");
            m_Table.Add(203, "Seu leilao foi fechado pela administracao e seu item foi devolvido a voce.");
            m_Table.Add(204, "Seu lance precisa ser pelo menos {0} maior que o lance atual");
            m_Table.Add(205, "Voce nao pode leiloar items que nao se movem");
            m_Table.Add(206, "Props");
            m_Table.Add(207, "O leilao selecionado nao esta mais ativo. Por favor atualize a lista de leiloes.");

            // VERSION 1.7 Begins here

            m_Table.Add(208, "Permitir comprar agora por:");
            m_Table.Add(209, "Se voce escolher usar a opcao Permitir comprar agora, informe um valor maior que o valor de reserva");
            m_Table.Add(210, "Compre este item agora por {0} gp");

            m_Table.Add(105, @"<basefont color=#FF0000>Acordo do leilao<br>
<basefont color=#FFFFFF>Por finalizando e enviando este formulario voce concordo em fazer parte do sistema de leiloes. O item leiloado sera removido de seu inventario e ira ser devolvido para voce somente se voce cancelar este leilao, for infrutifero e o item nao for vendido ou se a administracao forcar uma parada do sistema de leilao.
<basefont color=#FF0000>Lance inicial:<basefont color=#FFFFFF> Este e o lance minimo aceito por este item. Informe um valor razoavel e possivelmente menor do que esperaria pelo item no final do leilao.
<basefont color=#FF0000>Reserva:<basefont color=#FFFFFF> Este valor nao sera conhecido por outros e voce deve considerar como um preco razoavel pelo item. Se o lance final atingir este valor a venda sera automaticamente finalizada pelo sistema. Se por outro lado o lance vencedor esta entre o lance inicial e o valor de reserva, sera dada a opcao a voce para escolher se aceita o valor ou nao. Voce tera 7 dias depois do fim do leilao para tomar um decisao. Se nao o fizer, o sistema de leiloes ira assumir que voce decidiu nao vender e ira devolver o item a voce e o dinheiro do lance vencedor. Compradores nao irao ver o valor de reserva mas somente uma mensagem dizendo se foi atingido ou nao.
<basefont color=#FF0000>Duracao:<basefont color=#FFFFFF> Este valor especifica quantos dias o leilao ira durar desde sua data de criacao. No fim deste periodo, o sistema ira finalizar a venda, devolver o item e o valor do lance vencedor ou aguardar por uma decisao caso o valor de reserva nao tenha sido atingido.
<basefont color=#FF0000>Comprar agora:<basefont color=#FFFFFF> Esta opcao habilitar voce informar a que preco voce esta disposto a vender o item antes do fim do leilao. Se o comprar esta disposto a pagar este preco, ele ira comprar o item sem a necessidade de dar lances.
<basefont color=#FF0000>Nome:<basefont color=#FFFFFF> Este deve ser um nome curto definindo seu leilao. O sistema ira sugerir um nome baseado no item que esta vendendo mas voce pode querer altera-lo em algumas circunstancias.
<basefont color=#FF0000>Descricao:<basefont color=#FFFFFF> Voce pode escrever quase qualquer coisa que queira sobre seu item. Lembre-se que as propriedades do item sao vistas ao passar o mouse sobre o item, isto estara disponivel aos interessados automaticamente, entao nao ha necessidade de descrever isto.
<basefont color=#FF0000>Web Link:<basefont color=#FFFFFF> Voce pode adicionar um link para este leilao, no caso de haver uma pagina com mais detalhes ou discussao sobre o item.
<br>
Uma vez que voce salve este leilao, voce nao conseguira seu item de volta ate o fim do leilao. Tenha certeza que voce entenda o que isto significa antes de salvar.");

            m_Table.Add(211, "Voce nao tem dinheiro suficiente em seu banco para comprar este item");
            m_Table.Add(212, "Voce nao tem espaco suficiente em seu manco para fazer este deposito. Por favor libere algum espaco e tente novamente.");
            m_Table.Add(213, "Controle de leilao");
            m_Table.Add(214, "Propriedades");
            m_Table.Add(215, "Conta : {0}");
            m_Table.Add(216, "Dono do leilao : {0}");
            m_Table.Add(217, "Online");
            m_Table.Add(218, "Offline");
            m_Table.Add(219, "Items do leilao");
            m_Table.Add(220, "Coloque-o em sua mochila");
            m_Table.Add(221, "Coloque o item de volta no sistema");
            m_Table.Add(222, "Leilao");
            m_Table.Add(223, "Finalizar leilao agora");
            m_Table.Add(224, "Fechar e devolver item ao dono");
            m_Table.Add(225, "Fechar e colocar item em sua mochila");
            m_Table.Add(226, "Fechar e deletar o item");
            m_Table.Add(227, "Painel de administracao do leilao");

            //
            // VERSION 1.8
            //
            m_Table.Add(228, "{0} moedas de ouro fora retiradas de sua conta no banco como pagamento por este servico.");
            m_Table.Add(229, "Voce nao tem dinheiro suficiente para pagar por este servico. O custo deste leilao e de {0}.");
            m_Table.Add(230, "Seu lance foi colocado muito proximo do final do leilao, por isso a duracao do leilao foi extendida para aceitar novos lances.");

            //
            // VERSION 1.13
            //
            m_Table.Add(231, "Recipiente: {0}");
        }

        /// <summary>
        /// Gets the localized string for the Auction System
        /// </summary>
        public string this[int index]
        {
            get
            {
                string s = m_Table[index] as string;

                if (s == null)
                    return "Localization Missing";
                else
                    return s;
            }
        }
    }
}
