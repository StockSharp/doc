# 图表外壳

`createChartUi` 在引擎周围搭起一套现成的界面：面板标题、带十字线的图例、上下文菜单、图表类型菜单和指标对话框。引擎只负责绘制；它周围的一切都存放在单独的入口点 `@stocksharp/chart/ui` 中——只有一条迷你走势线的页面不必为此付出代价。

![围绕引擎的图例、指标窗格与图表菜单](../../../../images/javascript_charts_ui.png)

## 引入

这一层作为包的独立子路径提供，并需要它自己的样式表：

```ts
import { createChartUi, standaloneHost } from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';
```

如果不使用打包器，请引入浏览器包 `dist/sschartui.js`——它发布全局对象 `SSChartUI`。它必须在 `dist/sschart.js` **之后**加载：这一层从该文件发布的全局对象中读取引擎，而不是自带引擎的第二份副本。

## 创建和更新

这一层需要：创建图表所在的元素、页面宿主、供上下文菜单使用的价格来源，以及供图例菜单使用的图表类型列表：

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

每当 K 线窗口发生变化时都要重新调用 `setCandles`——换了标的、切换了图表类型、加载到了新的历史页。一次调用会同时更新指标引擎和图例。

`createChartUi` 的选项：

| 选项 | 用途 |
|---|---|
| `container` | 创建图表所在的元素；各面板围绕它构建。 |
| `host` | 翻译、格式化和消息。 |
| `priceSource` | 供上下文菜单使用的像素 → 价格换算。 |
| `chartTypes` | 图表类型菜单项，按显示顺序排列；列表为空则不绘制菜单。 |
| `storage` | 收藏的指标和模板保存在哪里。默认保存在内存中。 |
| `dialogRoot` | 指标对话框的自定义标记。不提供时会构建标记并添加到 `body` 中。 |
| `modal` | 打开和关闭对话框的自定义实现。 |
| `provideItems` | 上下文菜单中属于页面的条目——位于该层自身条目之上。 |

## createChartUi 返回什么

返回结果就是那些已经彼此关联好的对象；可以直接取用其中任意一个。

| 字段 | 是什么 |
|---|---|
| `engine` | `IndicatorEngine` — 指标的计算和生命周期。 |
| `renderer` | `IndicatorRenderer` — 用于绘制指标输出的序列。 |
| `paneManager` | `ChartPaneManager` — 面板标题、它们的菜单和恢复。 |
| `legend` | `ChartLegend` — OHLCV 行以及十字线下的指标数值。 |
| `dialog` | `IndicatorDialog` — 目录、搜索、参数和已启用的指标。 |
| `menu` | `ChartContextMenu` — 右键菜单。 |
| `indicators` | `IndicatorController` — 对已添加指标的可撤销编辑。 |
| `templates` | `IndicatorTemplateController` — 可迁移的指标设置模板。 |

## 公共方法

- `setCandles(candles)` — 把当前的 K 线窗口传给指标引擎和图例。
- `showIndicators()` — 打开指标对话框。
- `dispose()` — 移除菜单、对话框、图例和面板；由该层创建的对话框标记会被删除。

## 页面宿主

该层的任何模块都不访问全局对象：文字、数字和消息都来自 `ChartUiHost`——一个带有 `translate`、`formatters` 和 `notify` 字段的对象。

```ts
import { consoleNotify, createTranslate, defaultChartFormatters, type ChartUiHost } from '@stocksharp/chart/ui';

const host: ChartUiHost = {
  translate: createTranslate({ 'Indicators': '指标', 'Add indicator…': '添加指标…' }),
  formatters: { ...defaultChartFormatters, price: value => value.toFixed(2) },
  notify: consoleNotify,
};
```

词典是扁平的，并以英文原文作为键：未命中的键返回其自身，也就是一个可读的英文字符串，而不是缺失翻译的占位标记。占位符按位置替换——`{0}`、`{1}`。

- `standaloneHost` — 一切都自己负责的宿主：英文文本、按数值大小格式化、把消息输出到控制台。
- `identityTranslate` — 供单语言页面使用的翻译；占位符替换仍然保留。
- `defaultChartFormatters` — `price`、`volume` 和 `time`（Unix 秒，格式为 `YYYY-MM-DD HH:MM`）。
- `consoleNotify` — 把消息输出到浏览器控制台；级别有 `success`、`info`、`warning`、`error`。
- `createPlainModalController(root)` — 为没有自带模态窗口库的页面提供对话框的打开和关闭：遮罩层以及按 `Escape` 关闭。点击窗口之外不会关闭对话框。

## 存储

收藏的指标和模板通过 `ChartUiStorage` 保存——它由 `load(key)` 和 `save(key, value)` 两个函数组成。

- `inMemoryChartUiStorage` — 默认值：数据的存活时间与页面相同。
- `localChartUiStorage(prefix)` — 带前缀的 `localStorage` 封装，使页面上的两个图表不会互相覆盖收藏内容。

## 图表类型

`ChartTypeSwitcher` 会把同一个柱体窗口用K线图、柱状图、折线图、面积图、Heikin-Ashi、Renko 或 Point & Figure 重新绘制。更换类型意味着换一个绘制器，因此序列会被重新创建，而它此前的实例随之失效：

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

`parseChartType` 从页面自己保存的字符串——保存的布局或按钮属性——中读取类型，若不存在这种类型则返回 `null`。`getIndicatorCandles` 给出切换之后用于计算指标的柱体：Renko 和 Point & Figure 会把原始柱体重建成自己的柱体，而 `isDerivedChartType(type)` 回答是否发生了这种重建——这类柱体也没有成交量。其余方法有：`getCurrentSeries`、`getCurrentType`、`getAvailableTypes`、`updatePrice`。

## 上下文菜单

`ChartContextMenu` 不添加自己的条目——它们由页面通过 `provideItems` 提供，返回若干条目分组；分组之间会绘制分隔线，空分组不产生任何开销。条目由键 `key`、文本 `label`、可选的 `icon`、`tone`、`disabled` 以及方法 `invoke` 描述。色调由 `ChartContextMenuTone` 的取值指定：`Neutral`、`Positive`、`Negative`。

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
      label: `以 ${context.priceText} 买入`,
      icon: 'bi bi-arrow-up-circle',
      tone: ChartContextMenuTone.Positive,
      invoke: () => placeOrder('buy', context.price),
    },
  ]],
});
```

`createChartUi` 会在这些条目之上加入自己的一组——`Add indicator…` 和 `Add pane…`，两者都经过 `host.translate`。`ChartContextMenuMode` 区分价格图上的菜单（`Chart`，光标下有价格）和子面板标题上的菜单（`Pane`，没有价格）。方法有：`init`、`setPriceSource`、`openAt`、`close`、`dispose`。

## 其余导出

- `ChartLegend` 和 `fullscreenMenuLayer` — 图例，以及它的浮动菜单所打开到的层（若存在全屏元素则用它，否则用 `body`）。图例的方法：`init`、`setRawCandles`、`setChartType`、`setIndicatorEngine`、`refresh`、`dispose`；回调 `onEditIndicator` 和 `onChartTypeChange`。
- `ChartPaneManager` — 对引擎内置面板的封装：`init`、`addPane`、`removePane`、`restorePane`、`getChart`、`getPanes`、`getPaneByMeasure`、`setPaneTitle`、`getValuesElement`、`legendLayer`、`resize`、`dispose`。
- `IndicatorDialog` 和 `createIndicatorCatalogController` — 指标对话框及其目录的模型。对话框的方法：`show`、`showForPane`、`showEdit`、`hide`、`dispose`。
- `IndicatorEngine`、`IndicatorRenderer`、`IndicatorSettings` — 由对话框驱动的指标机制。这里和 `@stocksharp/chart/indicators` 都会发布它们。
- 类型 `LegendBar`、`LegendChartType`、`LegendChart`、`LegendPaneHost`、`LegendIndicatorEngine`、`IndicatorPaneChart`、`IndicatorPaneHost`、`ChartContextMenuProvider`、`PriceCoordinateSource`、`ChartTypePalette`、`ModalController` — 结构性契约：自行布置面板或自行计算指标的页面，用自己的对象来实现它们。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [指标](indicators.md)
- [历史数据回填](backfill.md)
- [K线图](candlestick.md)
