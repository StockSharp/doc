# Indicators

The catalog of about 160 technical indicators lives in a separate package, [@stocksharp/indicators](https://www.npmjs.com/package/@stocksharp/indicators). It arrives together with the chart as a dependency, but the definitions have to be imported from it. Any definition is computed with the public `IndicatorRuntime` over your candles, and you draw the results yourself as ordinary series — overlays on the price pane, or oscillators in their own sub-panes.

## Live demo

Bollinger Bands over the candles, with RSI and MACD in separate panes below.

```chart-demo indicators
```

## Setup

Import the runtime and the definitions you need, run each over the candles, and plot the outputs. Feed the runtime `{ time, value }` inputs where `value` is the candle:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import {
  IndicatorRuntime,
  BollingerBandsIndicator,
  RelativeStrengthIndexIndicator,
  MacdHistogramIndicator,
} from '@stocksharp/indicators';

// Compute one indicator over the candles; returns its points grouped by output id.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of runtime.points()) {
    if (p.time == null || p.value == null) continue;   // warm-up bars emit nothing
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bollinger Bands (20, 2) as an overlay envelope on the price pane.
const bb = compute(BollingerBandsIndicator, { length: 20, width: 2 }, candles);   // outputs: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) in its own sub-pane.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);     // output: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) in a second sub-pane.
const macdPane = chart.addPane();
const macd = compute(
  MacdHistogramIndicator,
  { shortMaLength: 12, longMaLength: 26, signalMaLength: 9 },                    // outputs: macd, signal, histogram
  candles);
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

## Parameters and outputs

Every definition declares its own parameters and output ids, and they have to be addressed by those ids rather than by descriptive names:

| Definition | Parameters | Outputs | Pane |
|---|---|---|---|
| `BollingerBandsIndicator` | `length` (20), `width` (2), `upBandWidth`, `lowBandWidth` | `upper`, `middle`, `lower` | overlay |
| `RelativeStrengthIndexIndicator` | `length` (15) | `oscillator` | own |
| `MacdHistogramIndicator` | `shortMaLength` (12), `longMaLength` (26), `signalMaLength` (9) | `macd`, `signal`, `histogram` | own |

> [!CAUTION]
> An unfamiliar parameter id is **dropped silently**, and the indicator computes on its default value. There will be no error — there will be a wrong line. For the same reason the Bollinger deviation is set through the `width` key rather than `stdDev`, and the RSI default length is 15 rather than 14: if you want the customary period, pass it explicitly, as in the example above.

The full list of definitions with their parameters is given out by the catalog:

```js
import { getClientCatalog } from '@stocksharp/indicators';

for (const item of getClientCatalog()) {
  console.log(item.id, item.name, item.parameters.map(p => p.id));
}
```

## Real-time data

A full recomputation on every bar is not needed. Keep the `IndicatorRuntime` and call `update` — it returns a patch with what has changed:

```js
const runtime = new IndicatorRuntime({ definition: RelativeStrengthIndexIndicator, parameters: { length: 14 } });
runtime.resetStreaming(history.map(c => ({ time: c.time, value: c })));

// An unclosed bar: false means a preliminary value that the next call will replace.
runtime.update({ time: bar.time, value: bar }, false);

// The bar has closed — the value becomes final.
const patch = runtime.update({ time: bar.time, value: bar }, true);
```

While a bar is not closed, the runtime holds a preliminary point and replaces it on every call, so the history does not grow from updates within one bar. `discardPreview()` removes the preliminary point, and `correct(index, input)` recomputes a historical bar that arrived with a correction.

## See also

- [JavaScript charts](../charts.md)
- [Band](band.md)
- [History backfill](backfill.md)
