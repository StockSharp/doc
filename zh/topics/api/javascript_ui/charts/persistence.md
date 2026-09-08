# 保存布局

`ChartStatePersistence` 把图表的布局——选项、面板、价格刻度、序列、指标和图形对象——收集成一份经过校验的 JSON 快照，并可以把它还原回去。柱体数据不会进入快照：保存的是配置，而行情来自你自己的数据源。

该层既不拥有存储，也不规定键的命名规则。写到哪里（文件、后端、`localStorage`、IndexedDB）以及如何划分快照（按布局、按标的、按用户）由应用决定——通过 `ChartStateStorage` 的实现和 `key` 函数。

## 创建和更新

从入口点 `@stocksharp/chart/persistence` 导入：

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
  console.log(restored.drawings.skipped);   // 未知类型的对象被跳过，而不是让整个还原失败
}
```

类型参数 `TContext` 就是你传给 `save`、`load` 和 `remove` 的内容；`key` 函数把上下文变成键字符串，并且必须返回非空字符串。`pretty: true` 会写出带缩进的 JSON。

`DrawingController` 必须是负责图表绘制的那个实例——否则保存下来的将是一个空的对象集合。

## 适配器

`ChartStatePersistence` 既不了解图表的原生 API，也不了解指标引擎：它通过两个适配器工作。现成的实现包含在同一个模块中，但如果你的架构不同，也可以传入自己的实现——接口 `ChartStateLayoutAdapter`（`capture`、`restore`）和 `ChartStateIndicatorAdapter`（`capture`、`clear`、`restore`）都是公开的。

**`NativeChartLayoutAdapter`** 负责采集和还原图表本身的布局：图表选项、面板及其顺序、高度、最小高度和状态（`normal`、`minimized`、`maximized`），价格刻度的设置，以及序列及其类型、面板、刻度和样式选项。构造函数选项：

- `chart` — 图表实例（必填）。
- `mainPaneId` — 在还原中得以保留的根面板的标识符；默认为 `main`，否则取第一个面板。
- `createSeries(series, pane)` — 用自定义方式创建序列以代替类型注册表，适用于需要把序列接到数据源的场景。
- `includeSeries(series)` — 过滤器：返回 `false` 的序列既不会被保存，也不会在还原时被删除。
- `onRemoveSeries(series)` — 当还原不得不摘下某个外部序列（因为它所在的面板不属于要加载的布局）时被调用。
- `onUnknownSeries(series)` — 序列类型不在注册表中。

带有 `persist: false` 选项的序列会被排除在快照之外，与被 `includeSeries` 过滤掉的序列一样。

**`IndicatorEngineStateAdapter`** 保存指标的配置——类型、参数、绘制样式、与面板和刻度的关联、可见性和数据源——但不保存计算出的数值：还原之后它们会被重新计算。构造函数选项：

- `engine` — 实现了 `IndicatorEnginePersistenceApi`（`getIndicators`、`removeAll`、`add`、`setVisible`）的指标引擎。
- `resolveTargetPaneId(indicator)` — 当标识符不一致时，把保存的面板映射到宿主的面板。
- `onUnknownIndicator(indicator)` — 引擎无法创建该类型的指标。
- `onUnknownStyle(indicator, styleId)` — 样式中出现了该指标没有的标识符。

以另一个指标的输出为输入的指标会在其后还原：适配器会自行为数据源链条排序，并在引用指向不存在的指标或图中存在环时报错。

## 状态格式与迁移

快照由 `ChartStateV1` 类型描述，字段为 `schemaVersion`、`chartOptions`、`panes`、`series`、`indicators`、`drawings`；当前的架构版本是常量 `CHART_STATE_SCHEMA_VERSION`（等于 1）。

- `serializeChartState(state, { pretty })` — 校验状态并把它转换成 JSON 字符串。
- `deserializeChartState(value, { migrations })` — 解析字符串（也接受已经准备好的对象），把迁移执行到当前版本并校验结果。
- `normalizeChartStateV1(value)` — 校验并冻结状态：多余的键、重复的标识符、指向不存在面板的引用以及一个面板都没有的布局都会被拒绝。
- `normalizePersistedObject(value, path, { omitUndefined })` — 把任意 JSON 深拷贝成不可变对象；禁止环、非数字值、过深的嵌套以及 `__proto__`、`prototype`、`constructor` 这些键。

旧快照通过逐步迁移升级。公共注册表 `chartStateMigrations` 已经包含从版本 0 到版本 1 的转换，自己的步骤这样注册：

```ts
import {
  ChartStateMigrationRegistry,
  deserializeChartState,
} from '@stocksharp/chart/persistence';

const migrations = new ChartStateMigrationRegistry();
migrations.register(1, state => ({ ...state, schemaVersion: 2 }));

const state = deserializeChartState(json, { migrations });
```

每个迁移把状态恰好向前推进一个版本，并且必须设置新的 `schemaVersion` 值。版本高于所支持版本的快照不会被接受加载。

## 公共方法

- `snapshot()` — 把图表的当前状态收集为 `ChartStateV1`，不访问存储。
- `restore(state)` — 把状态应用到图表；返回 `{ state, drawings }`，其中 `drawings` 包含 `restored` 和 `skipped` 两个列表。
- `save(context)` — 采集快照、序列化并按计算出的键写入存储；返回已保存的状态。
- `load(context)` — 按键读取记录、执行迁移并还原它；没有记录时返回 `null`。
- `remove(context)` — 从存储中删除记录。

还原的顺序是固定的：先释放指标，然后还原面板和序列的布局，接着是指标，最后是图形对象。

该模块还导出用于描述快照和适配器的类型：`ChartStateLayoutSnapshot`、`ChartStateRestoreResult`、`ChartStatePersistenceOptions`、`PersistedPane`、`PersistedPriceScale`、`PersistedSeries`、`PersistedIndicator`、`PersistedDrawing`、`PersistedChartOptions`、`PersistedSeriesOptions`、`PersistedIndicatorParameters`、`PersistedIndicatorStyles`、`PersistedObject`、`PersistedJsonValue`、`PersistableIndicatorEntry`、`RawChartState`、`ChartStateMigration`、`MaybePromise`。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [指标](indicators.md)
- [历史数据回填](backfill.md)
