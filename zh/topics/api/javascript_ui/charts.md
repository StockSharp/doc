# JavaScript 图表

[StockSharp JS 交易图表](https://github.com/StockSharp/JS-Charts) 是一个独立、无依赖的浏览器图表库。它以 [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) 的名称发布在 npm 上，并提供 StockSharp 网页终端所使用的 `sschart` canvas 引擎。可在[在线演示](https://stocksharp.github.io/JS-Charts/demo/)中查看可运行的版本。

![StockSharp JavaScript 交易图表](../../../images/javascript_charts.jpg)

与 `StockSharp.Xaml.Charting` 中的 Windows 组件不同，该库在浏览器中运行，并直接绘制到 HTML `canvas` 上。引擎通过全局对象 `SSChart`（来自 `dist/sschart.js`）暴露，也可以作为 ECMAScript 模块从 npm 包中导入（`import { createChart, CandlestickSeries } from '@stocksharp/chart'`）。

## 在线演示

下面的图表就是在本页上运行的真实引擎——带成交量直方图和移动平均线的K线图。拖动可滚动，使用滚轮缩放，点击（右上角的）展开按钮可全屏显示。

```chart-demo overview
```

## 功能特性

- 完整的价格系列：K线图、OHLC 柱状图、折线图、面积图、直方图、通道（band），以及衍生的 Heikin-Ashi、Renko 和 Point & Figure 类型。
- 精确的订单流研究：footprint、成交量分布和 TPO（市场分布图）。
- 通过 `setData` 和 `update` 实现历史数据加载和实时更新。
- 成交标记、价格线、十字光标、缩放、滚动以及自动区间计算。
- 拥有约 160 种计算实现的指标引擎。
- 叠加指标、同步的振荡器窗格，以及由十字光标驱动的图例。
- 浅色和深色主题、右键菜单、指标对话框以及图表类型切换。

## 安装

从 npm 安装该包并导入 ES 模块：

```bash
npm install @stocksharp/chart
```

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
```

或者，在不使用打包工具的情况下，如下所示，通过 `<script>` 标签引入预构建的 `SSChart` 全局对象。

## 向页面添加图表

构建会生成 `dist/sschart.js`，它会导出 `window.SSChart`。传递给 API 的时间值是以秒为单位的 Unix 时间戳。

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

创建图表、添加系列并加载数据：

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
  crosshair: { mode: SSChart.CrosshairMode.Normal },
});

const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  borderVisible: false,
});

candles.setData([
  { time: 1704153600, open: 120, high: 122, low: 119, close: 121 },
  { time: 1704240000, open: 121, high: 124, low: 120, close: 123 },
]);

candles.update({
  time: 1704326400,
  open: 123,
  high: 123.5,
  low: 122.8,
  close: 123.2,
});

chart.timeScale().fitContent();
```

使用当前时间戳调用 `update` 会替换最后一个点。使用更新的时间戳则会追加一个点。

## 图表模式

每种系列类型都有自己的专题，包含在线演示以及用于配置它的 JavaScript：

- [K线图](charts/candlestick.md) — 经典的 OHLC K线。
- [OHLC 柱状图](charts/bar.md) — 在垂直区间柱上标出开/收价刻度。
- [折线图](charts/line.md) — 一条穿过各收盘价的折线。
- [面积图](charts/area.md) — 带渐变填充的折线。
- [直方图](charts/histogram.md) — 垂直柱，通常表示成交量。
- [带状图（Band）](charts/band.md) — 上/下轨通道（包络线、布林带）。
- [Heikin-Ashi K线图](charts/heikin_ashi.md) — 过滤噪声的平滑K线。
- [Renko](charts/renko.md) — 由价格驱动的砖块，与时间无关。
- [点数图 (Point and Figure)](charts/point_figure.md) — 表示价格变动的 X/O 列。
- [足迹图（Footprint）](charts/footprint.md) — 每根柱内每个价位的买 × 卖成交量。
- [成交量分布图](charts/volume_profile.md) — 按价位的成交量，包含 POC（point of control）和价值区。
- [TPO（市场剖面图）](charts/tpo.md) — 每个交易时段在各价位停留的时间。

除了系列类型之外，图表还提供了拥有约 160 种研究的[指标引擎](charts/indicators.md)，以及在滚动时加载更早柱线的[惰性历史回填](charts/backfill.md)。

关于由同一 Web 技术栈渲染的可视化策略编辑器，请参阅 [JavaScript 框图](diagram.md)。

## 完整的终端图表技术栈

`src/chart` 下的模块为基础引擎扩展了终端功能：

- `IndicatorEngine`、指标渲染器、设置以及计算目录。
- 用于K线图、柱状图、折线图、面积图、Heikin-Ashi、Renko 和 Point & Figure 的图表类型切换器。
- 图例、同步的副窗格、右键菜单以及指标选择对话框。
- 在实时数据变化时重新计算活动指标。

可参考 `src/chart/app.ts` 作为完整技术栈的集成示例。

## 从源码构建

克隆仓库并使用内置的 npm 脚本：

```bash
git clone https://github.com/StockSharp/JS-Charts.git
cd JS-Charts
npm install
npm run build
npm test
npm run serve
```

开发服务器会在 `http://localhost:8791/demo/index.html` 提供该演示。

## 另请参阅

- [JavaScript 框图](diagram.md)
- [JS-Charts 仓库](https://github.com/StockSharp/JS-Charts)
- [在线演示](https://stocksharp.github.io/JS-Charts/demo/)
- [Windows 图表组件](../graphical_user_interface/charts.md)
