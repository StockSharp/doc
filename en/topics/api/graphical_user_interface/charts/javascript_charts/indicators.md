# Indicators

The chart ships a catalog of about 160 technical indicators. You compute any of them with the public `IndicatorRuntime` over your candles, then draw the results yourself as ordinary series — overlays on the price pane, or oscillators in their own sub-panes.

## Live demo

Bollinger Bands over the candles, with RSI and MACD in separate panes below.

```chart-demo indicators
```

## Setup

Import the runtime and the indicator definitions you need, run each over the candles, and plot the outputs. Feed the runtime `{ time, value }` inputs where `value` is the candle:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import { IndicatorRuntime, BollingerBandsIndicator, RelativeStrengthIndexIndicator, MacdIndicator } from '@stocksharp/chart/indicators';

// Compute one indicator over the candles; returns its points grouped by output id.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  const points = runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of points) {
    if (p.time == null || p.value == null) continue;   // warm-up bars emit nothing
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bollinger Bands (20, 2) as an overlay envelope on the price pane.
const bb = compute(BollingerBandsIndicator, { length: 20, stdDev: 2 }, candles);   // outputs: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) in its own sub-pane.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);       // output: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) in a second sub-pane.
const macdPane = chart.addPane();
const macd = compute(MacdIndicator, { fastLength: 12, slowLength: 26, signalLength: 9 }, candles);   // outputs: macd, signal, histogram
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

Each definition declares its own parameters and output ids (Bollinger emits `upper`/`middle`/`lower`, RSI a single `oscillator`, MACD `macd`/`signal`/`histogram`). For real-time data keep the `IndicatorRuntime` and call `runtime.update({ time, value }, isFinal)` per bar instead of recomputing.

## See also

- [JavaScript charts](../javascript_charts.md)
- [Band](band.md)
- [History backfill](backfill.md)
