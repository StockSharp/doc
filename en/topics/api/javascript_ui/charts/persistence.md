# Saving the layout

`ChartStatePersistence` collects the chart layout — options, panes, price scales, series, indicators, and drawings — into one validated JSON snapshot and restores it back. Bar data does not go into the snapshot: the configuration is saved, while the quotes come from your own source.

The layer owns neither the storage nor the key naming rule. Where to write (a file, a backend, `localStorage`, IndexedDB) and how to separate the snapshots (by layout, by instrument, by user) is decided by the application — through a `ChartStateStorage` implementation and the `key` function.

## Creating and updating

The import comes from the `@stocksharp/chart/persistence` entry point:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { DrawingController } from '@stocksharp/chart/drawings';
import {
  ChartStatePersistence,
  NativeChartLayoutAdapter,
  IndicatorEngineStateAdapter,
  type ChartStateStorage,
  type IndicatorEnginePersistenceApi,
} from '@stocksharp/chart/persistence';

declare const indicatorEngine: IndicatorEnginePersistenceApi;

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {});
chart.addSeries(CandlestickSeries, { id: 'price', upColor: '#26a69a', downColor: '#ef5350' });

const storage: ChartStateStorage = {
  load: key => localStorage.getItem(key),
  save: (key, value) => { localStorage.setItem(key, value); },
  remove: key => { localStorage.removeItem(key); },
};

const persistence = new ChartStatePersistence<{ layoutId: string; symbol: string }>({
  layout: new NativeChartLayoutAdapter({ chart, mainPaneId: 'main' }),
  indicators: new IndicatorEngineStateAdapter({ engine: indicatorEngine }),
  drawings: new DrawingController({ chart }),
  storage,
  key: ({ layoutId, symbol }) => `chart:${layoutId}:${symbol}`,
  pretty: true,
});

const context = { layoutId: 'desk', symbol: 'BTC@IMEX' };

await persistence.save(context);

const restored = await persistence.load(context);
if (restored !== null) {
  console.log(restored.state.panes.length);
  console.log(restored.drawings.skipped);   // drawings of unknown types were skipped, not lost to a failed restore
}
```

The `TContext` type parameter is what you pass to `save`, `load`, and `remove`; the `key` function turns the context into a key string and must return a non-empty string. `pretty: true` writes the JSON with indentation.

The `DrawingController` must be the same instance that serves the drawing on the chart — otherwise an empty set of drawings is saved.

## Adapters

`ChartStatePersistence` knows neither the native chart API nor the indicator engine: it works through two adapters. The ready implementations are part of the same module, but with an architecture of your own you can pass your own — the `ChartStateLayoutAdapter` (`capture`, `restore`) and `ChartStateIndicatorAdapter` (`capture`, `clear`, `restore`) interfaces are open.

**`NativeChartLayoutAdapter`** captures and returns the layout of the chart itself: the chart options, the panes with their order, height, minimum height, and state (`normal`, `minimized`, `maximized`), the price scale settings, and the series with their type, pane, scale, and style options. The constructor options:

- `chart` — the chart instance (mandatory).
- `mainPaneId` — the identifier of the root pane that survives the restore; `main` by default, otherwise the first pane.
- `createSeries(series, pane)` — creating a series yourself instead of using the type registry, when the series has to be wired to a data source.
- `includeSeries(series)` — a filter: a series for which `false` is returned is neither saved nor removed on restore.
- `onRemoveSeries(series)` — called for a foreign series that the restore had to detach anyway, because its pane is not part of the layout being loaded.
- `onUnknownSeries(series)` — the series type is missing from the registry.

A series with the `persist: false` option is excluded from the snapshot the same way as one rejected by the `includeSeries` filter.

**`IndicatorEngineStateAdapter`** saves the indicator configuration — type, parameters, drawing styles, the binding to a pane and a scale, visibility, and the source — but not the computed values: they are recomputed after the restore. The constructor options:

- `engine` — the indicator engine implementing `IndicatorEnginePersistenceApi` (`getIndicators`, `removeAll`, `add`, `setVisible`).
- `resolveTargetPaneId(indicator)` — matching a saved pane with a host pane when the identifiers differ.
- `onUnknownIndicator(indicator)` — the engine could not create an indicator of such a type.
- `onUnknownStyle(indicator, styleId)` — the styles contained an identifier the indicator does not have.

An indicator computed over the output of another indicator is restored after it: the adapter orders the source chain itself and reports an error when a reference leads to a missing indicator or the graph has a cycle.

## State format and migrations

The snapshot is described by the `ChartStateV1` type with the `schemaVersion`, `chartOptions`, `panes`, `series`, `indicators`, `drawings` fields; the current schema version is the `CHART_STATE_SCHEMA_VERSION` constant (equal to 1).

- `serializeChartState(state, { pretty })` — validate the state and turn it into a JSON string.
- `deserializeChartState(value, { migrations })` — parse the string (or accept an already-built object), run the migrations up to the current version, and validate the result.
- `normalizeChartStateV1(value)` — validation and freezing of the state: extraneous keys, duplicate identifiers, references to non-existent panes, and a layout without a single pane are rejected.
- `normalizePersistedObject(value, path, { omitUndefined })` — a deep copy of arbitrary JSON into an immutable object; cycles, non-numeric values, excessive nesting, and the `__proto__`, `prototype`, `constructor` keys are forbidden.

Old snapshots are lifted by step-by-step migrations. The shared `chartStateMigrations` registry already carries the step from version 0 to version 1, and your own steps are registered like this:

```ts
import {
  ChartStateMigrationRegistry,
  deserializeChartState,
} from '@stocksharp/chart/persistence';

const migrations = new ChartStateMigrationRegistry();
migrations.register(1, state => ({ ...state, schemaVersion: 2 }));

const state = deserializeChartState(json, { migrations });
```

Every migration moves the state exactly one version forward and must set the new `schemaVersion` value. A snapshot whose version is above the supported one is not accepted for loading.

## Public methods

- `snapshot()` — collect the current chart state into a `ChartStateV1` without touching the storage.
- `restore(state)` — apply the state to the chart; returns `{ state, drawings }`, where `drawings` holds the `restored` and `skipped` lists.
- `save(context)` — take a snapshot, serialize it, and write it into the storage under the computed key; returns the saved state.
- `load(context)` — read the entry by key, run the migrations, and restore it; `null` when there is no entry.
- `remove(context)` — delete the entry from the storage.

The order of the restore is fixed: first the indicators are released, then the layout of panes and series is restored, then the indicators, and the drawings last.

The module also exports the types describing the snapshot and the adapters: `ChartStateLayoutSnapshot`, `ChartStateRestoreResult`, `ChartStatePersistenceOptions`, `PersistedPane`, `PersistedPriceScale`, `PersistedSeries`, `PersistedIndicator`, `PersistedDrawing`, `PersistedChartOptions`, `PersistedSeriesOptions`, `PersistedIndicatorParameters`, `PersistedIndicatorStyles`, `PersistedObject`, `PersistedJsonValue`, `PersistableIndicatorEntry`, `RawChartState`, `ChartStateMigration`, `MaybePromise`.

## See also

- [JavaScript charts](../charts.md)
- [Indicators](indicators.md)
- [History backfill](backfill.md)
