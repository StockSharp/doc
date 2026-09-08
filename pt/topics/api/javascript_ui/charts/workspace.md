# Vários gráficos

`MultiChartWorkspace` dispõe vários gráficos independentes numa grelha dentro de um mesmo contentor, liga-os pelo instrumento e pelo período e sincroniza o intervalo visível e o cursor em cruz. A classe é distribuída pelo ponto de entrada `@stocksharp/chart/workspace`, juntamente com os restantes controladores do espaço de trabalho — painéis, indicadores, modelos, comparação de instrumentos e navegação pelo histórico.

O espaço de trabalho é dono apenas dos gráficos de topo. Os painéis de indicadores continuam a ser mecânica interna do seu gráfico: não contam como células, não participam na disposição e não são sincronizados.

## Criação e atualização

O contentor e a fábrica de gráficos são obrigatórios. A fábrica recebe `{ id, index, host }` e devolve uma célula — o próprio gráfico, um controlador de dados opcional e uma função de libertação opcional (por predefinição é chamado `chart.remove()`):

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { ChartDataController, type IChartDataSource } from '@stocksharp/chart/data';
import { MultiChartWorkspace } from '@stocksharp/chart/workspace';

declare const dataSource: IChartDataSource;

const workspace = new MultiChartWorkspace({
  container: document.querySelector<HTMLElement>('#workspace')!,
  count: 4,
  columns: 2,                                   // null — grelha automática, próxima de um quadrado
  links: { symbol: true, resolution: false },   // instrumento comum, período próprio em cada célula
  sync: { range: true, crosshair: true },
  createChart: ({ host }) => {
    const chart = createChart(host, { timeScale: { timeVisible: true } });
    const series = chart.addSeries(CandlestickSeries, {
      upColor: '#26a69a',
      downColor: '#ef5350',
    });
    const data = new ChartDataController({ chart, series, dataSource, initialCount: 300 });

    return {
      chart,
      data,
      dispose: () => {
        data.dispose();
        chart.remove();
      },
    };
  },
});

const [first] = workspace.cells();
await workspace.setSelection(first.id, { symbol: 'BTC@IMEX', resolution: '1h' });

workspace.subscribe(snapshot => {
  console.log(snapshot.activeId, snapshot.columns, snapshot.rows, snapshot.errors.length);
});
```

`setCount` e `setColumns` alteram o tamanho da grelha separadamente e `setLayout({ count, columns })` fá-lo numa única operação. O contentor é formatado como grelha CSS; os estilos originais são memorizados e repostos em `dispose`. Pode haver de 1 a 64 células; a última célula não pode ser eliminada.

## Ligação e sincronização

`links` descreve o que é transmitido às restantes células quando o instrumento muda: `symbol` e `resolution` ligam-se de forma independente. Uma célula para a qual a fábrica não devolveu `data` não participa na ligação.

`sync` ativa a transmissão do intervalo visível (`range`) e da posição do cursor em cruz (`crosshair`). Depois de aplicado, o intervalo é relido do gráfico recetor: uma célula com histórico mais curto corta a janela pedida, e no instantâneo é publicado aquilo que está realmente visível.

A célula ativa é definida por `activate` e também automaticamente por `pointerdown` e `focusin` dentro da célula. Alterar `links` ou `sync` distribui de imediato pelas restantes o estado atual da célula ativa.

Os erros de sincronização não interrompem o funcionamento das outras células, acumulando-se em `snapshot.errors` — os últimos 32. Cada registo tem `cellId`, `kind` (`WorkspaceSyncErrorKind`: `selection`, `range`, `crosshair`, `lifecycle`) e `error`. A lista é limpa com `clearErrors`.

## Métodos públicos

- `snapshot()` — o estado completo: número de células, colunas e linhas, célula ativa, `links`, `sync`, células e erros.
- `cells()` — instantâneos das células: `id`, `index`, `active`, `selection`, `visibleRange`, `crosshairTime`.
- `chart(id)` / `host(id)` — o gráfico e o elemento DOM da célula.
- `add(id?)` — acrescentar uma célula; sem argumento, o identificador é gerado.
- `remove(id)` — eliminar uma célula.
- `setCount(count)`, `setColumns(columns)`, `setLayout(layout)` — alterar a grelha.
- `activate(id)` — tornar a célula ativa.
- `setLinks(options)`, `setSync(options)` — alternar a ligação e a sincronização.
- `setSelection(id, selection)` — definir o instrumento e o período da célula e distribuí-los pelas ligações.
- `clearErrors()` — limpar os erros acumulados.
- `subscribe(listener)` / `unsubscribe(listener)` — subscrição do instantâneo de estado.
- `dispose()` — libertar as células e repor os estilos do contentor.

## Restantes controladores da camada

- `PaneController` — gestão anulável (undo/redo) dos painéis do gráfico: `resizePair`, `reorder`, `moveSeries`, `setState`, `toggleMinimized`, `toggleMaximized`. Funciona através da pilha de comandos comum do gráfico e não recria o conteúdo dos painéis.
- `IndicatorController` — edição validada dos indicadores sobre o motor de cálculo: `update`, `setParameters`, `setSource`, `moveToPane`, `setPriceScale`, `setVisible`, `setOutputStyle`. Cada alteração entra na pilha de comandos e o instantâneo contém as definições dos parâmetros, o estado da origem e os estilos das saídas.
- `IndicatorCatalogController` — pesquisa no catálogo de indicadores (`search` por texto, categoria e marca de favorito) e favoritos guardados pelo anfitrião: `loadFavorites`, `setFavorite`, `toggleFavorite`.
- `IndicatorTemplateController` — modelos transferíveis de definições de indicador: `create`, `replace`, `rename`, `remove`, `apply`, `load`. O método `apply` transfere os parâmetros, a origem, a visibilidade e os estilos das saídas, mas deixa deliberadamente inalterados o painel e a escala de preços do destino.
- `serializeIndicatorTemplates`, `deserializeIndicatorTemplates`, `normalizeIndicatorTemplateDocument`, `INDICATOR_TEMPLATE_SCHEMA_VERSION` — serialização e validação do documento versionado de modelos.
- `CompareController` — sobreposição de vários instrumentos no mesmo gráfico: `add`, `remove`, `setPrimary`, `setColor`, `setVisible`, `reload`, `loadMoreBefore`, `legend`. O modo de normalização é definido por `setMode` (`CompareMode.Percentage` ou `CompareMode.IndexedTo100`) e a forma de alinhar o tempo por `setAlignment` (`CompareAlignment.Chart` ou `CompareAlignment.PrimarySession`). Cada instrumento tem o seu `ChartDataController` e a sua própria subscrição.
- `ChartNavigator` — navegação pelo histórico sem ligação ao DOM: `setRange`, `selectPreset`, `goToDate`, `cancel`. O controlador carrega por si as páginas de histórico em falta (por predefinição, não mais de 100 por operação) e publica um modelo de vista geral a partir de um número limitado de amostras (por predefinição, 600). As predefinições `1D`, `5D`, `1M`, `3M`, `6M`, `YTD`, `1Y`, `5Y`, `All` são devolvidas por `defaultNavigatorPresets`; o resultado da operação é descrito por `NavigatorNavigationOutcome` (`applied`, `clamped`, `page-limit`, `empty`, `cancelled`) e o alinhamento da data por `NavigatorDateAlignment`.

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Preenchimento de histórico](backfill.md)
- [Indicadores](indicators.md)
