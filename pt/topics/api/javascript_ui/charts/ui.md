# Interface do gráfico

`createChartUi` monta em torno do motor uma interface pronta a usar: títulos dos painéis, legenda com cursor em cruz, menu de contexto, menu do tipo de gráfico e diálogo de indicadores. O motor limita-se a desenhar; tudo o que o rodeia vive num ponto de entrada separado, `@stocksharp/chart/ui` — uma página com um único gráfico em miniatura não paga por isso.

![A legenda, um painel de indicador e os menus do gráfico à volta do motor](../../../../images/javascript_charts_ui.png)

## Ligação

A camada é distribuída num subcaminho próprio do pacote e exige a sua folha de estilos:

```ts
import { createChartUi, standaloneHost } from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';
```

Sem um empacotador, ligue o pacote para navegador `dist/sschartui.js` — publica o objeto global `SSChartUI`. Tem de ser carregado **depois** de `dist/sschart.js`: a camada lê o motor a partir do objeto global publicado por esse ficheiro, em vez de transportar uma segunda cópia dele.

## Criação e atualização

A camada precisa do elemento onde o gráfico foi criado, do anfitrião da página, de uma origem de preço para o menu de contexto e da lista de tipos de gráfico para o menu da legenda:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  ChartType,
  createChartUi,
  localChartUiStorage,
  standaloneHost,
  type ChartUi,
} from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';

declare const candles: { time: number; open: number; high: number; low: number; close: number; volume?: number }[];

const container = document.querySelector<HTMLElement>('#chart')!;

const chart = createChart(container, { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
series.setData(candles);

const ui: ChartUi = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [
    { value: ChartType.Candle, label: 'Candles', icon: 'bi bi-bar-chart-fill' },
    { value: ChartType.Line, label: 'Line', icon: 'bi bi-graph-up' },
  ],
  storage: localChartUiStorage('sschart'),
});

ui.setCandles(candles);
```

`setCandles` é chamado de novo sempre que muda a janela de velas — outro instrumento, outro tipo de gráfico, uma página de histórico que acabou de chegar. Uma única chamada atualiza tanto o motor de indicadores como a legenda.

Opções de `createChartUi`:

| Opção | Finalidade |
|---|---|
| `container` | Elemento onde o gráfico foi criado; é à volta dele que os painéis são construídos. |
| `host` | Tradução, formatação e mensagens. |
| `priceSource` | Píxel → preço para o menu de contexto. |
| `chartTypes` | Entradas do menu de tipo de gráfico pela ordem de apresentação; uma lista vazia não desenha o menu. |
| `storage` | Onde ficam guardados os indicadores favoritos e os modelos. Por predefinição, em memória. |
| `dialogRoot` | Estrutura própria para o diálogo de indicadores. Sem ela, a estrutura é construída e acrescentada ao `body`. |
| `modal` | Implementação própria da abertura e do fecho do diálogo. |
| `provideItems` | Entradas da página no menu de contexto — acima das entradas da própria camada. |

## O que devolve createChartUi

O resultado são esses mesmos objetos, já ligados entre si; qualquer um deles pode ser alcançado diretamente.

| Campo | O que é |
|---|---|
| `engine` | `IndicatorEngine` — cálculo e ciclo de vida dos indicadores. |
| `renderer` | `IndicatorRenderer` — as séries com que as saídas dos indicadores são desenhadas. |
| `paneManager` | `ChartPaneManager` — títulos dos painéis, os seus menus e a sua reposição. |
| `legend` | `ChartLegend` — a linha OHLCV e os valores dos indicadores sob o cursor em cruz. |
| `dialog` | `IndicatorDialog` — catálogo, pesquisa, parâmetros e indicadores ativos. |
| `menu` | `ChartContextMenu` — o menu do botão direito. |
| `indicators` | `IndicatorController` — edição anulável dos indicadores já adicionados. |
| `templates` | `IndicatorTemplateController` — modelos transferíveis de definições de indicador. |

## Métodos públicos

- `setCandles(candles)` — entregar a janela de velas atual ao motor de indicadores e à legenda.
- `showIndicators()` — abrir o diálogo de indicadores.
- `dispose()` — retirar o menu, o diálogo, a legenda e os painéis; a estrutura de diálogo criada pela camada é removida.

## Anfitrião da página

Nenhum módulo da camada recorre a objetos globais: as palavras, os números e as mensagens vêm de `ChartUiHost` — um objeto com os campos `translate`, `formatters` e `notify`.

```ts
import { consoleNotify, createTranslate, defaultChartFormatters, type ChartUiHost } from '@stocksharp/chart/ui';

const host: ChartUiHost = {
  translate: createTranslate({ 'Indicators': 'Indicadores', 'Add indicator…': 'Adicionar indicador…' }),
  formatters: { ...defaultChartFormatters, price: value => value.toFixed(2) },
  notify: consoleNotify,
};
```

O dicionário é plano e indexado pelo texto original em inglês: uma chave sem resposta devolve-se a si própria, ou seja, uma cadeia inglesa legível e não um marcador de tradução em falta. A substituição é posicional — `{0}`, `{1}`.

- `standaloneHost` — um anfitrião que trata de tudo sozinho: texto em inglês, formatação pela grandeza do número, mensagens na consola.
- `identityTranslate` — tradução para uma página com um único idioma; a substituição dos marcadores é preservada.
- `defaultChartFormatters` — `price`, `volume` e `time` (segundos Unix, formato `YYYY-MM-DD HH:MM`).
- `consoleNotify` — envio de mensagens para a consola do navegador; níveis `success`, `info`, `warning`, `error`.
- `createPlainModalController(root)` — abertura e fecho do diálogo para uma página sem biblioteca modal própria: fundo escurecido e fecho por `Escape`. Um clique fora da janela não fecha o diálogo.

## Armazenamento

Os indicadores favoritos e os modelos são guardados através de `ChartUiStorage` — duas funções, `load(key)` e `save(key, value)`.

- `inMemoryChartUiStorage` — o valor predefinido: os dados vivem tanto quanto a página.
- `localChartUiStorage(prefix)` — invólucro sobre `localStorage` com um prefixo, para que dois gráficos na mesma página não sobreponham os favoritos um do outro.

## Tipo de gráfico

`ChartTypeSwitcher` redesenha a mesma janela de barras como velas, barras, linha, área, Heikin-Ashi, Renko ou Point & Figure. Mudar de tipo é usar outro desenhador, pelo que a série é criada de novo e a instância anterior fica inválida:

```ts
import {
  ChartType,
  ChartTypeSwitcher,
  allChartTypes,
  defaultChartTypePalette,
  parseChartType,
} from '@stocksharp/chart/ui';

const switcher = new ChartTypeSwitcher({
  chart,
  series,
  initialType: ChartType.Candle,
  availableTypes: allChartTypes,
  palette: defaultChartTypePalette,
  host: standaloneHost,
});
switcher.setRawCandles(candles);

switcher.onSeriesChanged(next => ui.menu.setPriceSource(next));

ui.legend.onChartTypeChange = value => {
  const type = parseChartType(value);
  if (type === null) return;

  switcher.switchType(type);
  ui.setCandles(switcher.getIndicatorCandles());
};
```

`parseChartType` lê o tipo a partir de uma cadeia que a página guardou por si — uma disposição gravada ou o atributo de um botão — e devolve `null` se esse tipo não existir. `getIndicatorCandles` entrega as barras sobre as quais os indicadores são calculados depois da mudança: Renko e Point & Figure reconstroem as barras originais em barras próprias, e `isDerivedChartType(type)` responde se foi esse o caso — essas barras também não têm volume. Os restantes métodos: `getCurrentSeries`, `getCurrentType`, `getAvailableTypes`, `updatePrice`.

## Menu de contexto

`ChartContextMenu` não acrescenta entradas próprias — quem as dá é a página, através de `provideItems`, devolvendo grupos de entradas; entre grupos é desenhado um separador e os grupos vazios não custam nada. Uma entrada é descrita pela chave `key`, pelo texto `label`, pelos opcionais `icon`, `tone` e `disabled` e pelo método `invoke`. O tom é definido pelos valores de `ChartContextMenuTone`: `Neutral`, `Positive`, `Negative`.

```ts
import { ChartContextMenuTone, createChartUi, standaloneHost } from '@stocksharp/chart/ui';

const ui = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [],
  provideItems: context => [[
    {
      key: 'buy',
      label: `Comprar a ${context.priceText}`,
      icon: 'bi bi-arrow-up-circle',
      tone: ChartContextMenuTone.Positive,
      invoke: () => placeOrder('buy', context.price),
    },
  ]],
});
```

A estas entradas `createChartUi` acrescenta o seu próprio grupo — `Add indicator…` e `Add pane…`, ambos através de `host.translate`. `ChartContextMenuMode` distingue o menu sobre o gráfico de preços (`Chart`, existe preço sob o cursor) do menu sobre o título de um subpainel (`Pane`, não existe preço). Métodos: `init`, `setPriceSource`, `openAt`, `close`, `dispose`.

## Restantes exportações

- `ChartLegend` e `fullscreenMenuLayer` — a legenda e a camada onde o seu menu flutuante abre (o elemento em ecrã inteiro, se existir, caso contrário o `body`). Métodos da legenda: `init`, `setRawCandles`, `setChartType`, `setIndicatorEngine`, `refresh`, `dispose`; retornos de chamada `onEditIndicator` e `onChartTypeChange`.
- `ChartPaneManager` — camada sobre os painéis internos do motor: `init`, `addPane`, `removePane`, `restorePane`, `getChart`, `getPanes`, `getPaneByMeasure`, `setPaneTitle`, `getValuesElement`, `legendLayer`, `resize`, `dispose`.
- `IndicatorDialog` e `createIndicatorCatalogController` — o diálogo de indicadores e o modelo do seu catálogo. Métodos do diálogo: `show`, `showForPane`, `showEdit`, `hide`, `dispose`.
- `IndicatorEngine`, `IndicatorRenderer`, `IndicatorSettings` — a mecânica dos indicadores comandada pelo diálogo. É publicada tanto aqui como em `@stocksharp/chart/indicators`.
- Os tipos `LegendBar`, `LegendChartType`, `LegendChart`, `LegendPaneHost`, `LegendIndicatorEngine`, `IndicatorPaneChart`, `IndicatorPaneHost`, `ChartContextMenuProvider`, `PriceCoordinateSource`, `ChartTypePalette`, `ModalController` — contratos estruturais: uma página que disponha os painéis ou calcule os indicadores por si implementa-os com os seus próprios objetos.

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Indicadores](indicators.md)
- [Preenchimento de histórico](backfill.md)
- [Candlestick](candlestick.md)
