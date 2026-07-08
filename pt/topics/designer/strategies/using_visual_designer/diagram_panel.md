# Designer de estratégias

O principal processo de conceção de uma estratégia e dos seus elementos componentes decorre no painel **Scheme**, através da combinação de blocos e linhas de ligação. O painel Scheme é composto pelos painéis: **Palette**, **Designer** e **Properties**.

![Designer Designer schemes strategies and component elements 00](../../../../images/designer_designer_schemes_strategies_and_component_elements_00.png)

## Painel Palette

O painel **Palette** contém os blocos a partir dos quais as estratégias são criadas. Todos os elementos da paleta estão divididos em categorias, descritas na secção [Descrição dos blocos](elements.md). Para adicionar um bloco ao painel **Designer**, clique com o botão direito do rato no bloco pretendido e, sem soltar o botão, arraste-o para o painel **Designer**. Depois disso, o elemento será automaticamente selecionado e os seus parâmetros serão mostrados na janela para editar as propriedades do bloco.

## Painel Designer

O painel **Designer** é onde ocorre todo o processo de criação de uma estratégia através da combinação de blocos e ligações (linhas). Ele representa visualmente o esquema da estratégia. As informações detalhadas sobre a criação de uma estratégia são descritas na secção [Criar um algoritmo a partir de blocos](first_strategy.md).

## Painel Properties

O painel **Properties** apresenta os parâmetros do bloco selecionado no painel **Designer**. Quando um bloco é selecionado no painel **Designer**, a sua moldura fica preta.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_00.png)

O painel **Properties** pode ser apresentado em dois modos: *Basic settings* e *Advanced settings*.

Por predefinição, ao construir um esquema, as propriedades são inicialmente apresentadas no modo *basic settings*. Para mudar para o modo *advanced settings*, tem de clicar no título correspondente.

No modo *basic settings*, são apresentadas apenas as propriedades mais necessárias do bloco. Por exemplo, para o bloco [Velas](elements/data_sources/candles.md), serão apresentados o timeframe, a flag para receber apenas velas formadas, a flag para a possibilidade de construir velas a partir de um timeframe menor e a flag para subscrever velas por sinal.

No modo *advanced settings*, serão apresentadas todas as propriedades do bloco disponíveis para alteração e definição.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_01.png)

Todos os blocos contêm um conjunto de propriedades predefinidas, que ficam visíveis no modo *advanced settings*:

- **Name** - o nome do elemento, apresentado no designer.
- **Logging level** - o nível de registo para este elemento.
- **Parameters** - apresenta os parâmetros do elemento em elementos de nível superior.
- **Sockets** - apresenta os sockets do elemento em elementos de nível superior.

As informações detalhadas sobre as propriedades de cada bloco são descritas na secção [Descrição dos blocos](elements.md).

## Ver também

[Descrição dos blocos](elements.md)
