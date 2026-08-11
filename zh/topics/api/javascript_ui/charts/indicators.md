# 指标

图表内置了约 160 个技术指标的目录。你可以通过公开的 `IndicatorRuntime` 在你的 K线数据上计算其中任意一个，然后将结果作为普通的系列自行绘制——既可以作为价格窗格上的叠加层，也可以作为独立子窗格中的振荡指标。

## 在线演示

在 K线上叠加布林带，下方独立窗格中显示 RSI 和 MACD。

```chart-demo indicators
```

## 配置

导入运行时和你所需的指标定义，在 K线数据上逐个运行它们，然后绘制输出结果。向运行时传入 `{ time, value }` 形式的输入，其中 `value` 是 K线：

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import { IndicatorRuntime, BollingerBandsIndicator, RelativeStrengthIndexIndicator, MacdIndicator } from '@stocksharp/chart/indicators';

// 在 K线数据上计算单个指标；返回按输出 id 分组的数据点。
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  const points = runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of points) {
    if (p.time == null || p.value == null) continue;   // 预热阶段的柱线不产生任何输出
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// 布林带 (20, 2) 作为价格窗格上的叠加包络线。
const bb = compute(BollingerBandsIndicator, { length: 20, stdDev: 2 }, candles);   // 输出：upper、middle、lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) 位于独立的子窗格中。
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);       // 输出：oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) 位于第二个子窗格中。
const macdPane = chart.addPane();
const macd = compute(MacdIndicator, { fastLength: 12, slowLength: 26, signalLength: 9 }, candles);   // 输出：macd、signal、histogram
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

每个定义都声明了自己的参数和输出 id（布林带发出 `upper`/`middle`/`lower`，RSI 发出单个 `oscillator`，MACD 发出 `macd`/`signal`/`histogram`）。对于实时数据，请保留 `IndicatorRuntime`，并对每根 K线调用 `runtime.update({ time, value }, isFinal)`，而不是重新计算。

## 参见

- [JavaScript 图表](../charts.md)
- [带状图（Band）](band.md)
- [历史数据回填](backfill.md)
