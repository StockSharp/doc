# JavaScript 图表

[StockSharp JavaScript 交易图表](https://github.com/StockSharp/Charts)是一个独立的浏览器图表库。它包含无运行时依赖的 canvas 引擎 `sschart`，以及 StockSharp Web 终端使用的图表模块。可以通过[在线演示](https://stocksharp.github.io/Charts/demo/)查看可运行版本。

![StockSharp JavaScript 交易图表](../../../../images/javascript_charts.jpg)

与 `StockSharp.Xaml.Charting` 中的 Windows 组件不同，该库在浏览器中运行，并直接绘制到 HTML `canvas`。引擎通过全局对象 `SSChart` 公开，在 TypeScript 构建中也可以从 `src/sschart.ts` 导入。

## 功能

- K线、OHLC 条形、折线、面积、直方图、Renko、点数图、成交量分布、聚类和箱形序列。
- 使用 `setData` 和 `update` 加载历史数据并实时更新。
- 成交标记、价格线、十字光标、缩放、滚动和自动范围计算。
- 包含约 160 种计算实现的指标引擎。
- 叠加指标、同步振荡器窗格以及由十字光标驱动的图例。
- 浅色和深色主题、上下文菜单、指标对话框以及图表类型切换。

## 将图表添加到页面

构建会生成 `dist/sschart.js` 并发布 `window.SSChart`。传给 API 的时间值是以秒为单位的 Unix 时间戳。

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

创建图表、添加序列并加载数据：

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
  crosshair: { mode: SSChart.CrosshairMode.Normal },
});

const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#00c853',
  downColor: '#ff3d57',
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

使用当前时间戳调用 `update` 会替换最后一个数据点。使用更新的时间戳则会追加数据点。

## 完整的终端图表模块

`src/chart` 下的模块为基础引擎扩展了终端功能：

- `IndicatorEngine`、指标渲染器、设置和计算目录。
- K线、条形、折线、面积、Heikin-Ashi、Renko、点数图、聚类和箱形图类型切换。
- 图例、同步辅助窗格、上下文菜单和指标选择对话框。
- 实时数据变化时重新计算活动指标。

完整模块的集成方式可参考 `src/chart/app.ts`。

## 从源代码构建

克隆仓库并使用其中的 npm 脚本：

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

开发服务器会在 `http://localhost:8791/demo/index.html` 提供演示。

## 另请参阅

- [Charts 仓库](https://github.com/StockSharp/Charts)
- [在线演示](https://stocksharp.github.io/Charts/demo/)
- [Windows 图表组件](../charts.md)
