# Visualizar e exportar

Os dados recebidos pelo [Hydra](../../hydra.md) podem ser visualizados em painéis especiais.

Para isso, no separador **Comum**, clique num dos seguintes botões: [Ticks](view_and_export/ticks.md), [Livros de ordens](view_and_export/order_books.md), [Geração de velas](candles_generation.md), [Log de ordens](view_and_export/order_log.md), [Level 1](view_and_export/level_1_.md), [Notícias](view_and_export/news.md), [Transações](view_and_export/transactions.md), [Painel de opções](view_and_export/option_desk.md), [Indicadores](view_and_export/indicators.md), [Posições](view_and_export/positions.md).

Ou clique com o botão direito do rato no tipo de dados necessário, como mostrado na figura, ou faça duplo clique no tipo de dados necessário.

![hydra view export](../../../images/hydra_view_export.png)

Cada painel contém uma interface geral das definições, como se segue:

![Hydra exportação 00](../../../images/hydra_export_00.png)

- A linha superior indica o armazenamento de dados de mercado e o respetivo formato (BIN ou CSV).
- A linha inferior define o período para o qual os dados serão solicitados. Ao clicar no botão **Selecionar instrumento**, aparecerá a janela de seleção de instrumentos, na qual pode selecionar um ou vários instrumentos. Se forem selecionados vários instrumentos, durante a exportação posterior para Excel ou CSV o programa ordenará automaticamente os dados dos diferentes instrumentos para ficheiros diferentes.
- Se, ao construir uma tabela com dados, a quantidade de dados descarregados exceder o limite definido, aparecerá uma janela no ecrã:![hydra tick limit](../../../images/hydra_tick_limit.png)

  é necessário aumentar o limite de dados descarregados.
- Se os dados tiverem sido recebidos de origens cujo fuso horário não corresponda ao fuso horário atual, pode ajustar o fuso horário. Depois da construção, os dados serão mostrados no fuso selecionado pelo utilizador. ![hydra TZ](../../../images/hydra_tz.png)
- Como várias origens não oferecem a possibilidade de descarregar alguns dados, o programa disponibiliza o campo [Construir a partir de](any_market_data_types.md). Usando este campo, o utilizador pode construir dados de mercado a partir de outro tipo de dados de mercado. A mesma função pode ser usada para construir dados de mercado sem descarregamento adicional, usando como base dados já existentes.
- Depois de selecionar os parâmetros acima, deve clicar no botão ![Hydra botão Procurar](../../../images/hydra_find.png).![Hydra candles por período](../../../images/hydra_candles_tf.png)

Usando o menu de contexto, pode configurar vários parâmetros da tabela de valores de dados de mercado: agrupamento de linhas, colunas disponíveis, formato de apresentação, etc.

![Hydra contexto de exportação](../../../images/hydra_export_context.png)
