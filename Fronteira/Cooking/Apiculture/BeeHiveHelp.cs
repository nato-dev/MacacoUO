using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Multis;
using Server.Targeting;

namespace Server.Engines.Apiculture
{	
	public class apiBeeHiveHelpGump : Gump
	{
		public apiBeeHiveHelpGump( Mobile from, int type ) : base( 20, 20 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddBackground(37, 25, 386, 353, 3600);
			AddLabel(177, 42, 92, @"Ajuda");

			AddItem(32, 277, 3311);
			AddItem(30, 193, 3311);
			AddItem(29, 107, 3311);
			AddItem(28, 24, 3311);
			AddItem(386, 277, 3307);
			AddItem(387, 191, 3307);
			AddItem(388, 108, 3307);
			AddItem(385, 26, 3307);

			AddHtml( 59, 67, 342, 257, HelpText(type), true, true);
			AddButton(202, 333, 247, 248, 0, GumpButtonType.Reply, 0);
		}

		public string HelpText(int type)
		{
			string text = "";

			switch( type )
			{
				case 0:
				{

                    text += "<p> <b> Apicultura </b> é a ciência (e alguns dizem arte) da criação de abelhas, também conhecida como <b> apicultura </b>. As abelhas vivem juntas em grupos chamados <b> colônias </b> e moram em <b> colméias </b>. Cuidar de uma colméia não é tão fácil quanto parece, embora possa ser uma experiência muito gratificante. Começar no caminho da <b> apicultor </b>, tudo o que precisamos é de uma <b> ação de colméia </b> e uma área com muitas <b> flores </b> e <b> água </b>. </p> "; text += "<p>There are 3 distinct stages in a beehive's development:</p>";
                        text += "<p> <b> Colonização </b> - a colméia envia batedores para inspecionar a área e encontrar fontes de flores e água. </p>";
                        text += "<p> <b> Ninhada </b> - a postura dos ovos começa com força total à medida que a colméia se prepara para iniciar a produção em larga escala. </p>"; text += "<p><b>Producing</b> - after a hive reaches maturity, it begins producing excess amounts of honey and wax.</p>";
                        text += "<p> A saúde de uma colméia é medida de duas maneiras: <b> sobre toda a saúde </b> e <b> população de abelhas </b>. </p>";
                        text += "<p> <b> Sobre toda a saúde </b> oferece uma indicação do bem-estar médio das abelhas: </p>";
                        text += "<p> <b> Próspera </b> - as abelhas são extremamente saudáveis. Uma colônia próspera produz mel e cera a uma taxa maior. </p>"; text += "<p><b>Healthy</b> - the bees are healthy and producing excess honey and wax.</p>";
                        text += "<p> <b> Doença </b> - as abelhas estão doentes e não produzem mais recursos em excesso. </p>";
                        text += "<p> <b> Morrendo </b> - se algo não for feito rapidamente, a população de abelhas começará a diminuir. </p>";
                        text += "<p> <b> População de abelhas </b> é uma estimativa aproximada do número de abelhas em uma colméia. Mais abelhas nem sempre significam melhor para uma colméia grande e mais difícil de manter. Mais água e flores são necessários na área para apoiar uma colméia grande (o alcance que uma colméia pode procurar por flores e a água aumenta à medida que a colméia aumenta) Se as condições piorarem o suficiente, uma colônia de abelhas <b> escapará </b> , deixando uma colméia vazia para trás. </p> ";
                        text += "<p> Como qualquer coisa viva, as abelhas são suscetíveis a ataques de forças externas. Seja parasitas ou doenças, o apicultor tem uma infinidade de ferramentas à sua disposição. </p>";
                        text += "<p> <b> Poções de Maior Cura </b> podem ser usadas para combater doenças como mau hálito e disenteria. Essas poções também podem ser usadas para neutralizar o excesso de veneno. </p>"; text += "<p><b>Greater Poison</b> potions can be used to combat insects (such as the wax moth) or parasites (such as the bee louse) that infest a hive.  Care must be used!  Too many poison potions can harm the bees.</p>";
                        text += "<p> <b> Poções de maior força </b> podem ser usadas para aumentar a imunidade de uma colméia a infestações e doenças. </p>";
                        text += "<p> <b> Poções de Cura Maior </b> podem ser usadas para ajudar a curar as abelhas. </p>";
                        text += "<p> <b> Poções de maior agilidade </b> dão às abelhas energia extra, permitindo que trabalhem mais. Isso aumentará a produção de mel e cera, além de aumentar o alcance que as abelhas podem procurar por flores e água. </p> ";
                        text += "<p> O gerenciamento e o cuidado da colméia são feitos usando o <b> Apiculture gump </b>. Quase todos os aspectos da colméia podem ser monitorados daqui. No lado esquerdo da gump estão os ícones de status : </p> ";
                        text += "<p> <b> Produção </b> - esse botão exibe a <b> broca de produção </b> onde o apicultor pode colher os bens que a colméia tem a oferecer. </p>";
                        text += "<p> <b> Infestação </b> - um hífen vermelho ou amarelo aqui significa que a colméia está infestada por parasitas ou outros insetos. Use <b> venenos </b> para matar as pragas. </ p > ";
                        text += "<p> <b> Doença </b> - um hífen vermelho ou amarelo aqui significa que a colméia está atualmente doente. O uso de <b> poções de cura </b> ajudará as abelhas a combater a doença. </ p> "; text += "<p><b>Water</b> - this icon displays the availability of water in the area.  Be warned, water breeds disease carrying bacteria, so too much water can make a hive more susceptible to disease.</p>";
                        text += "<p> <b> Flores </b> - este ícone fornece uma indicação da quantidade de flores disponíveis para a colméia. As abelhas usam flores e seus subprodutos para quase todas as funções da colméia, incluindo construção e alimentação Porém, muitas flores na área podem colocar as abelhas em contato com mais parasitas e insetos. </p> ";
                        text += "<p> <b> Notas: </b> uma única colméia de abelhas pode suportar até 100 mil abelhas. Uma colméia saudável pode viver indefinidamente; no entanto, uma colméia mais antiga é mais suscetível a infestações e doenças. </ p> ";
                        text += "<p> A <b> verificação de crescimento de uma colméia </b> é realizada uma vez por dia durante uma economia mundial. O canto superior direito do <b> Apiculture Gump </b> exibe os resultados dos últimos verificação de crescimento: </p> ";
                        text += "<p> <b> <basefont color = # FF0000>! </basefont> </b> Não é saudável </p>";
                        text += "<p> <b> <basefont color = # FFFF00>! </basefont> </b> Baixos recursos </p>"; text += "<p><b><basefont color=#FF0000>- </basefont></b>Population decrease</p>";
                        text += "<p> <b> <basefont color = # 00FF00> + </basefont> </b> Crescimento populacional </p>";
                        text += "<p> <b> <basefont color = # 0000FF> + </basefont> </b> Aumento de estágio / produção de recursos </p>"; break;
				}
				case 1:
				{
                        text += "<p> A cera de abelha, na sua forma bruta, diretamente da colméia, está cheia de impurezas, dificultando o trabalho. O processo de purificação da cera crua é chamado de <b> renderização </b>. </p>";
                        text += "<p> Depois que uma colméia amadurece e começa a produzir excesso de cera, o <b> Apicultor </b> pode raspar a cera da colméia usando uma <b> ferramenta de colméia </b>. </p> ";
                        text += "<p> Esta cera de abelha crua pode ser colocada em um <b> pequeno pote de cera </b>. Quando aplicada a uma fonte de calor, a cera crua derrete permitindo que o apicultor remova as impurezas, conhecido como <b > favela </b>. </p> ";
                        text += "<p> Com a pureza removida, a cera restante processada pode ser transformada em cera de abelha pura. Essa cera é adequada para uso em qualquer número de aplicações. </p>"; break;
				}
			}

			return text;
		}
	}
}
