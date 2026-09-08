# 指标

约 160 个技术指标的目录位于单独的 [@stocksharp/indicators](https://www.npmjs.com/package/@stocksharp/indicators) 包中。它作为依赖随图表一起安装，但指标定义必须从它导入。任何定义都由公开的 `IndicatorRuntime` 在你的 K线数据上计算，而结果则由你自己用普通序列绘制——既可以叠加在价格窗格上，也可以作为独立子窗格中的振荡指标。

## 在线演示

在 K线上叠加布林带，下方独立窗格中显示 RSI 和 MACD。

```chart-demo indicators
```

## 设置

导入运行时和你所需的定义，在 K线数据上逐个运行它们，然后绘制输出结果。向运行时传入 `{ time, value }` 形式的输入，其中 `value` 是 K线：

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import {
  IndicatorRuntime,
  BollingerBandsIndicator,
  RelativeStrengthIndexIndicator,
  MacdHistogramIndicator,
} from '@stocksharp/indicators';

// 在 K线数据上计算单个指标；返回按输出 id 分组的数据点。
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of runtime.points()) {
    if (p.time == null || p.value == null) continue;   // 预热阶段的柱线不产生任何输出
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// 布林带 (20, 2) 作为价格窗格上的叠加包络线。
const bb = compute(BollingerBandsIndicator, { length: 20, width: 2 }, candles);   // 输出：upper、middle、lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) 位于独立的子窗格中。
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);     // 输出：oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) 位于第二个子窗格中。
const macdPane = chart.addPane();
const macd = compute(
  MacdHistogramIndicator,
  { shortMaLength: 12, longMaLength: 26, signalMaLength: 9 },                    // 输出：macd、signal、histogram
  candles);
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

## 参数和输出

每个定义都声明自己的参数和输出标识符，访问它们必须用这些标识符，而不是描述性的名称：

| 定义 | 参数 | 输出 | 窗格 |
|---|---|---|---|
| `BollingerBandsIndicator` | `length` (20)、`width` (2)、`upBandWidth`、`lowBandWidth` | `upper`、`middle`、`lower` | 叠加 |
| `RelativeStrengthIndexIndicator` | `length` (15) | `oscillator` | 独立 |
| `MacdHistogramIndicator` | `shortMaLength` (12)、`longMaLength` (26)、`signalMaLength` (9) | `macd`、`signal`、`histogram` | 独立 |

> [!CAUTION]
> 未知的参数标识符会被**静默丢弃**，指标随即按默认值计算。不会有任何报错——只会得到一条错误的线。同样的原因，布林带的偏差用 `width` 键指定而不是 `stdDev`，而 RSI 的默认长度是 15 而不是 14：如果需要常用的周期，就必须像上面的例子那样显式传入。

完整的定义列表及其参数可以从目录中获取：

```js
import { getClientCatalog } from '@stocksharp/indicators';

for (const item of getClientCatalog()) {
  console.log(item.id, item.name, item.parameters.map(p => p.id));
}
```

## 实时数据

不需要对每根柱线做完整的重算。保留 `IndicatorRuntime` 并调用 `update`——它返回一个只包含变化内容的补丁：

```js
const runtime = new IndicatorRuntime({ definition: RelativeStrengthIndexIndicator, parameters: { length: 14 } });
runtime.resetStreaming(history.map(c => ({ time: c.time, value: c })));

// 未收盘的柱线：false 表示这是一个预览值，会被下一次调用替换。
runtime.update({ time: bar.time, value: bar }, false);

// 柱线已收盘——该值成为最终值。
const patch = runtime.update({ time: bar.time, value: bar }, true);
```

只要柱线还没收盘，运行时就会保留一个预览点，并在每次调用时替换它，因此同一根柱线内的多次更新不会让历史增长。`discardPreview()` 会移除预览点，`correct(index, input)` 会重新计算某根带修正到达的历史柱线。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [带状图（Band）](band.md)
- [历史数据回填](backfill.md)
