# Indikatoren

Das Chart liefert einen Katalog von etwa 160 technischen Indikatoren mit. Sie berechnen jeden davon mit der öffentlichen `IndicatorRuntime` über Ihre Kerzen und zeichnen die Ergebnisse anschließend selbst als gewöhnliche Serien — als Overlays auf dem Kurs-Panel oder als Oszillatoren in eigenen Unter-Panels.

## Live-Demo

Bollinger-Bänder über den Kerzen, mit RSI und MACD in separaten Panels darunter.

```chart-demo indicators
```

## Einrichtung

Importieren Sie die Runtime und die benötigten Indikatordefinitionen, führen Sie jede über die Kerzen aus und stellen Sie die Ausgaben dar. Übergeben Sie der Runtime `{ time, value }`-Eingaben, wobei `value` die Kerze ist:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import { IndicatorRuntime, BollingerBandsIndicator, RelativeStrengthIndexIndicator, MacdIndicator } from '@stocksharp/chart/indicators';

// Berechnet einen Indikator über die Kerzen; liefert dessen Punkte gruppiert nach Output-ID zurück.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  const points = runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of points) {
    if (p.time == null || p.value == null) continue;   // Aufwärm-Bars liefern nichts
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bollinger-Bänder (20, 2) als überlagernde Hüllkurve auf dem Kurs-Panel.
const bb = compute(BollingerBandsIndicator, { length: 20, stdDev: 2 }, candles);   // Ausgaben: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) in einem eigenen Unter-Panel.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);       // Ausgabe: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) in einem zweiten Unter-Panel.
const macdPane = chart.addPane();
const macd = compute(MacdIndicator, { fastLength: 12, slowLength: 26, signalLength: 9 }, candles);   // Ausgaben: macd, signal, histogram
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

Jede Definition deklariert ihre eigenen Parameter und Ausgabe-IDs (Bollinger liefert `upper`/`middle`/`lower`, RSI einen einzelnen `oscillator`, MACD `macd`/`signal`/`histogram`). Behalten Sie für Echtzeitdaten die `IndicatorRuntime` bei und rufen Sie pro Bar `runtime.update({ time, value }, isFinal)` auf, anstatt neu zu berechnen.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Band](band.md)
- [Nachladen der Historie](backfill.md)
