# 多个图表

`MultiChartWorkspace` 把多个相互独立的图表以网格形式排布在同一个容器中，按标的和时间框架把它们关联起来，并同步可见区间和十字线。该类由入口点 `@stocksharp/chart/workspace` 提供，同它一起提供的还有工作区的其余控制器——面板、指标、模板、标的对比和历史导航。

工作区只拥有顶层图表。指标面板仍然是各自图表的内部构造：它们不算作单元格，不参与布局，也不参与同步。

## 创建和更新

容器和图表工厂是必填的。工厂收到 `{ id, index, host }` 并返回一个单元格——图表本身、可选的数据控制器以及可选的释放函数（默认调用 `chart.remove()`）：

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { ChartDataController, type IChartDataSource } from '@stocksharp/chart/data';
import { MultiChartWorkspace } from '@stocksharp/chart/workspace';

declare const dataSource: IChartDataSource;

const workspace = new MultiChartWorkspace({
  container: document.querySelector<HTMLElement>('#workspace')!,
  count: 4,
  columns: 2,                                   // null — 自动生成接近正方形的网格
  links: { symbol: true, resolution: false },   // 共用标的，每个单元格有自己的时间框架
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

`setCount` 和 `setColumns` 分别改变网格的尺寸，`setLayout({ count, columns })` 则一次完成。容器被设置为 CSS 网格；原有样式会被记住，并在 `dispose` 中恢复。单元格数量可以是 1 到 64；不能删除最后一个单元格。

## 关联与同步

`links` 描述更换标的时哪些内容会传播到其余单元格：`symbol` 和 `resolution` 各自独立开启。工厂没有返回 `data` 的单元格不参与关联。

`sync` 开启可见区间（`range`）和十字线位置（`crosshair`）的传播。区间在应用之后会从接收方图表重新读取：历史较短的单元格会裁剪所请求的窗口，而快照中发布的是实际显示的内容。

活动单元格由 `activate` 指定，也会在单元格内部发生 `pointerdown` 和 `focusin` 时自动切换。修改 `links` 或 `sync` 会立即把活动单元格的当前状态分发给其余单元格。

同步错误不会中断其余单元格的工作，而是累积到 `snapshot.errors` 中——保留最近 32 条。每条记录都有 `cellId`、`kind`（`WorkspaceSyncErrorKind`：`selection`、`range`、`crosshair`、`lifecycle`）和 `error`。列表通过调用 `clearErrors` 清空。

## 公共方法

- `snapshot()` — 完整状态：单元格数量、列数和行数、活动单元格、`links`、`sync`、各单元格以及错误。
- `cells()` — 单元格快照：`id`、`index`、`active`、`selection`、`visibleRange`、`crosshairTime`。
- `chart(id)` / `host(id)` — 单元格的图表和 DOM 元素。
- `add(id?)` — 添加单元格；不带参数时自动生成标识符。
- `remove(id)` — 删除单元格。
- `setCount(count)`、`setColumns(columns)`、`setLayout(layout)` — 修改网格。
- `activate(id)` — 把某个单元格设为活动。
- `setLinks(options)`、`setSync(options)` — 切换关联与同步。
- `setSelection(id, selection)` — 设置单元格的标的和时间框架，并按关联分发它们。
- `clearErrors()` — 清空已累积的错误。
- `subscribe(listener)` / `unsubscribe(listener)` — 订阅状态快照。
- `dispose()` — 释放单元格并恢复容器的样式。

## 该层的其余控制器

- `PaneController` — 对图表面板的可撤销（undo/redo）管理：`resizePair`、`reorder`、`moveSeries`、`setState`、`toggleMinimized`、`toggleMaximized`。它通过图表的公共命令栈工作，不会重建面板的内容。
- `IndicatorController` — 在计算引擎之上对指标进行可校验的编辑：`update`、`setParameters`、`setSource`、`moveToPane`、`setPriceScale`、`setVisible`、`setOutputStyle`。每一次修改都进入命令栈，快照中包含参数定义、数据源状态和输出样式。
- `IndicatorCatalogController` — 在指标目录中搜索（按文本、类别和收藏标志的 `search`）以及由宿主保存的收藏：`loadFavorites`、`setFavorite`、`toggleFavorite`。
- `IndicatorTemplateController` — 可迁移的指标设置模板：`create`、`replace`、`rename`、`remove`、`apply`、`load`。`apply` 方法会迁移参数、数据源、可见性和输出样式，但有意不改动目标的面板和价格刻度。
- `serializeIndicatorTemplates`、`deserializeIndicatorTemplates`、`normalizeIndicatorTemplateDocument`、`INDICATOR_TEMPLATE_SCHEMA_VERSION` — 带版本的模板文档的序列化与校验。
- `CompareController` — 在同一图表上叠加多个标的：`add`、`remove`、`setPrimary`、`setColor`、`setVisible`、`reload`、`loadMoreBefore`、`legend`。归一化方式由 `setMode` 指定（`CompareMode.Percentage` 或 `CompareMode.IndexedTo100`），时间对齐方式由 `setAlignment` 指定（`CompareAlignment.Chart` 或 `CompareAlignment.PrimarySession`）。每个标的都有自己的 `ChartDataController` 和自己的订阅。
- `ChartNavigator` — 不依赖 DOM 的历史导航：`setRange`、`selectPreset`、`goToDate`、`cancel`。控制器会自行加载缺失的历史页（默认每次操作不超过 100 页），并发布一个由有限数量采样构成的概览模型（默认 600 个）。现成的预设 `1D`、`5D`、`1M`、`3M`、`6M`、`YTD`、`1Y`、`5Y`、`All` 由 `defaultNavigatorPresets` 返回；操作结果由 `NavigatorNavigationOutcome`（`applied`、`clamped`、`page-limit`、`empty`、`cancelled`）描述，日期的对齐方式由 `NavigatorDateAlignment` 描述。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [历史数据回填](backfill.md)
- [指标](indicators.md)
