# Primeiro arranque

Na primeira execução, aparece a seguinte janela para selecionar fontes de dados. Também pode abrir esta janela no separador **Comum**, selecionando **Adicionar \=\> Fontes**.

![Hydra adicionar fonte](../../images/hydra_source_add.png)

Na janela, assinale as fontes necessárias. Pode utilizar filtros por região, board, tipo de dados, pagamento, em tempo real ou não. Quando a seleção estiver concluída, clique em **OK**. Em seguida, o programa irá propor a ativação dos utilitários. Para obter mais detalhes sobre o trabalho com utilitários, consulte a secção [Utilitários](tasks.md). Clique em **OK**.

![Captura de tela de Primeiro arranque 1](../../images/hydra_first_started_utilities00.png)

Depois disso, as fontes serão adicionadas ao painel esquerdo da janela principal da aplicação.

![Hydra Início rápido 01](../../images/hydra_quick_start_01.png)

## Transferir uma ferramenta para carregar dados de mercado:

Antes de iniciar a transferência de dados de mercado, é necessário configurar os instrumentos para os quais pretende obter dados de mercado.

Depois de adicionar fontes de dados de mercado, os painéis das fontes adicionadas serão abertos na parte central, onde é apresentada uma lista de instrumentos. Se o painel estiver fechado, para o abrir faça duplo clique no logótipo da fonte na lista do lado esquerdo do programa.

Por exemplo, transfira o instrumento AAPL@NASDAQ a partir de uma fonte de dados suportada.

![Hydra escolher dados de mercado](../../images/hydra_choose_market_data.png)

> [!TIP]
> IMPORTANTE\! Os dados são transferidos apenas para os instrumentos adicionados à lista de instrumentos

1. Adicionar instrumentos.

   No primeiro arranque, o programa irá propor transferir todos os instrumentos de uma só vez para a fonte selecionada. Posteriormente, o próprio utilizador transferirá os instrumentos. Inicialmente, a base de dados de instrumentos em [Hydra](../hydra.md) está vazia; existe apenas um instrumento auxiliar **ALL@ALL**. Quando este instrumento é selecionado, os dados serão transferidos para todos os instrumentos disponíveis para esta fonte.

   Para adicionar um instrumento, clique no botão **Adicionar** ![Hydra botão Adicionar](../../images/hydra_add.png). Em seguida, será aberta uma janela para transferir o instrumento. ![Captura de tela de Primeiro arranque 2](../../images/hydra_securities.png)

   Para transferir os instrumentos, é necessário clicar no botão **Transferir instrumentos** correspondente.

   Depois disso, será apresentado no ecrã um menu no qual o utilizador pode selecionar **Transferir todos os instrumentos**.![Hydra selecionar todos os instrumentos](../../images/hydra_securities_choose_all.png)

   Ou, para algumas fontes, [configurar](prepare_for_download/instruments_list.md) os instrumentos que pretende transferir.

   Depois de os instrumentos serem recebidos, a janela terá o seguinte aspeto.![Hydra lista completa de instrumentos](../../images/hydra_security_full_list.png)

   Serão listados todos os instrumentos disponíveis para adicionar. Para uma pesquisa rápida, pode introduzir o respetivo nome no campo apropriado.

   Para selecionar um instrumento, faça duplo clique nele e este será movido para o lado direito da lista.![Hydra lista completa de instrumentos 00](../../images/hydra_security_full_list_00.png)

   Em seguida, será movido para o lado direito da tabela.![Hydra lista completa de instrumentos 01](../../images/hydra_security_full_list_01.png)

   Os instrumentos selecionados serão apresentados na tabela **Instrumentos**, que é uma tabela com estrutura em árvore. O elemento principal é o instrumento; o elemento adicional são os tipos de dados de mercado que serão recebidos para esse instrumento.
2. Para cada instrumento selecionado, deve selecionar os tipos de dados de mercado necessários para transferência.

   Se nem todos os parâmetros necessários do instrumento estiverem definidos, o ícone ![Captura de tela de Primeiro arranque 3](../../images/hydra_zero.png) aparecerá na coluna esquerda da linha do instrumento. ![Hydra escolher tipo de dados de mercado](../../images/hydra_type_market_data_choose.png)

   Vamos selecionar a transferência de **Ticks** e **Velas período 5**.

   Na parte inferior da janela da fonte existe um painel com botões para configurar os dados e instrumentos a receber. ![Hydra Início rápido 02 00](../../images/hydra_quick_start_02_00.png)

   Neste painel podem ser executadas as seguintes operações:
   - Configurar a quantidade de informação recebida através dos botões: **Negócios, Livros de ofertas, Velas, Registo de ordens, Level 1, Transações próprias**. As listas de tipos de dados de mercado disponíveis variam consoante a fonte.
   - Especificar o período necessário para as velas carregadas. O período das velas recebidas é diferente para fontes diferentes.![Hydra Início rápido 02](../../images/hydra_quick_start_02.png)
   - Definir o período necessário para a transferência dos dados de mercado. O período também pode ser configurado diretamente na janela de dados de mercado. Para isso, deve selecionar o início e o fim do período.

     Se o utilizador não especificar a data de fim do período, o programa transfere todos os dados disponíveis até à data atual. Se a fonte suportar a transmissão de dados de mercado em tempo real, então, se não existir data de fim para o período, os dados de mercado serão transferidos em tempo real.

     Vamos definir o período para o qual é necessário transferir os dados de mercado.![Hydra Início rápido 02 01](../../images/hydra_quick_start_02_01.png)
   - Especificar a partir de que dados serão construídos os dados de mercado. Se este parâmetro não for especificado, serão recebidas as velas disponíveis na fonte. Se o utilizador especificar o tipo de dados de mercado, as velas serão construídas a partir do tipo de dados de mercado especificado. Por exemplo, as velas podem ser construídas a partir do preço da última transação, do spread do livro de ordens (normalmente para o mercado Forex), da volatilidade ou do melhor preço.

     Esta função é conveniente se a fonte não permitir receber dados para construir velas. Neste caso, as velas são construídas com base nos valores médios dos dados.![Hydra tipo de construção de candle](../../images/hydra_candle_build_type.png)

     O utilizador também tem a possibilidade de selecionar um [tipo personalizado](prepare_for_download/custom_candles.md) de velas para personalizar os dados recebidos.
   - Depois de selecionar um instrumento, o tipo de dados de mercado e definir o período, deve clicar no botão **Iniciar**. Depois disso, a transferência dos dados de mercado será iniciada.

   O processo de trabalho pode ser observado no separador especial **Registos**, fixado na parte inferior do programa. Além disso, os logs são guardados em ficheiros na pasta local.

![Hydra principal start](../../images/hydra_main_start.png)

Além disso, o utilizador pode adicionar [fontes adicionais](data_sources/select_source.md).

Depois de os dados de mercado serem transferidos, o utilizador pode [ver dados de mercado](working_with_data/view_and_export.md), [construir velas](working_with_data/candles_generation.md) , guardar ou [exportar em vários formatos](working_with_data/export_data.md).

**Ver [tutorial em vídeo](videos/first_start.md)**.
