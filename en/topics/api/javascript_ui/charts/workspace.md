# Multiple charts

`MultiChartWorkspace` lays several independent charts out as a grid in one container, links them by instrument and timeframe, and synchronizes the visible range and the crosshair. The class ships in the `@stocksharp/chart/workspace` entry point together with the rest of the workspace controllers — panes, indicators, templates, instrument comparison, and history navigation.

The workspace owns only the top-level charts. Indicator panes remain the internal business of their own chart: they do not count as cells, do not take part in the layout, and are not synchronized.

## Creating and updating

The container and the chart factory are mandatory. The factory receives `{ id, index, host }` and returns a cell — the chart itself, an optional data controller, and an optional release function (`chart.remove()` is called by default):

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { ChartDataController, type IChartDataSource } from '@stocksharp/chart/data';
import { MultiChartWorkspace } from '@stocksharp/chart/workspace';

declare const dataSource: IChartDataSource;

const workspace = new MultiChartWorkspace({
  container: document.querySelector<HTMLElement>('#workspace')!,
  count: 4,
  columns: 2,                                   // null — an automatic, close to square grid
  links: { symbol: true, resolution: false },   // a shared instrument, a timeframe of its own per cell
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

`setCount` and `setColumns` change the grid size separately, and `setLayout({ count, columns })` does it in one operation. The container is styled as a CSS grid; the original styles are remembered and restored in `dispose`. There may be from 1 to 64 cells; the last cell cannot be removed.

## Linking and synchronization

`links` describes what is carried over to the other cells when the instrument changes: `symbol` and `resolution` are enabled independently. A cell whose factory did not return `data` takes no part in the linking.

`sync` enables carrying over the visible range (`range`) and the crosshair position (`crosshair`). After being applied, the range is read back from the receiving chart: a cell with a shorter history trims the requested window, and what is actually shown is published in the snapshot.

The active cell is set by `activate`, and also automatically by `pointerdown` and `focusin` inside a cell. Changing `links` or `sync` immediately hands the current state of the active cell out to the others.

Synchronization errors do not interrupt the work of the other cells but accumulate in `snapshot.errors` — the last 32 of them. Every entry has a `cellId`, a `kind` (`WorkspaceSyncErrorKind`: `selection`, `range`, `crosshair`, `lifecycle`), and an `error`. The list is emptied by a `clearErrors` call.

## Public methods

- `snapshot()` — the full state: cell count, columns and rows, the active cell, `links`, `sync`, the cells, and the errors.
- `cells()` — the cell snapshots: `id`, `index`, `active`, `selection`, `visibleRange`, `crosshairTime`.
- `chart(id)` / `host(id)` — the chart and the DOM element of a cell.
- `add(id?)` — add a cell; without an argument the identifier is generated.
- `remove(id)` — remove a cell.
- `setCount(count)`, `setColumns(columns)`, `setLayout(layout)` — change the grid.
- `activate(id)` — make a cell active.
- `setLinks(options)`, `setSync(options)` — toggle the linking and the synchronization.
- `setSelection(id, selection)` — set the instrument and the timeframe of a cell and hand them out along the links.
- `clearErrors()` — clear the accumulated errors.
- `subscribe(listener)` / `unsubscribe(listener)` — subscription to the state snapshot.
- `dispose()` — release the cells and restore the container styles.

## The other controllers of the layer

- `PaneController` — undoable (undo/redo) management of the chart panes: `resizePair`, `reorder`, `moveSeries`, `setState`, `toggleMinimized`, `toggleMaximized`. It works through the shared command stack of the chart and does not recreate the pane contents.
- `IndicatorController` — validated editing of the indicators on top of the computation engine: `update`, `setParameters`, `setSource`, `moveToPane`, `setPriceScale`, `setVisible`, `setOutputStyle`. Every change goes into the command stack, and the snapshot carries the parameter definitions, the source state, and the output styles.
- `IndicatorCatalogController` — search over the indicator catalog (`search` by text, category, and the favourite flag) and the favourites, saved by the host: `loadFavorites`, `setFavorite`, `toggleFavorite`.
- `IndicatorTemplateController` — portable templates of indicator settings: `create`, `replace`, `rename`, `remove`, `apply`, `load`. The `apply` method carries over the parameters, the source, the visibility, and the output styles, but deliberately leaves the pane and the price scale of the target unchanged.
- `serializeIndicatorTemplates`, `deserializeIndicatorTemplates`, `normalizeIndicatorTemplateDocument`, `INDICATOR_TEMPLATE_SCHEMA_VERSION` — serialization and validation of the versioned template document.
- `CompareController` — overlaying several instruments on one chart: `add`, `remove`, `setPrimary`, `setColor`, `setVisible`, `reload`, `loadMoreBefore`, `legend`. The normalization mode is set by `setMode` (`CompareMode.Percentage` or `CompareMode.IndexedTo100`), and the way the time is aligned by `setAlignment` (`CompareAlignment.Chart` or `CompareAlignment.PrimarySession`). Every instrument has its own `ChartDataController` and its own subscription.
- `ChartNavigator` — history navigation with no ties to the DOM: `setRange`, `selectPreset`, `goToDate`, `cancel`. The controller loads the missing pages of history itself (no more than 100 per operation by default) and publishes an overview model built from a limited number of samples (600 by default). The ready `1D`, `5D`, `1M`, `3M`, `6M`, `YTD`, `1Y`, `5Y`, `All` presets are returned by `defaultNavigatorPresets`; the result of an operation is described by `NavigatorNavigationOutcome` (`applied`, `clamped`, `page-limit`, `empty`, `cancelled`), and the date alignment by `NavigatorDateAlignment`.

## See also

- [JavaScript charts](../charts.md)
- [History backfill](backfill.md)
- [Indicators](indicators.md)
