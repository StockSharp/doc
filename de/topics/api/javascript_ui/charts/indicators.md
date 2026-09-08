# Indikatoren

Der Katalog von etwa 160 technischen Indikatoren lebt im eigenen Paket [@stocksharp/indicators](https://www.npmjs.com/package/@stocksharp/indicators). Es kommt zusammen mit dem Chart als Abhängigkeit mit, doch die Definitionen müssen genau daraus importiert werden. Jede Definition berechnen Sie mit der öffentlichen `IndicatorRuntime` über Ihre Kerzen und zeichnen das Ergebnis anschließend selbst als gewöhnliche Serien — als Overlay auf dem Kurs-Panel oder als Oszillator in einem eigenen Unter-Panel.

## Live-Demo

Bollinger-Bänder über den Kerzen, mit RSI und MACD in separaten Panels darunter.

```chart-demo indicators
```

## Einrichtung

Importieren Sie die Runtime und die benötigten Definitionen, führen Sie jede über die Kerzen aus und stellen Sie die Ausgaben dar. Übergeben Sie der Runtime `{ time, value }`-Eingaben, wobei `value` die Kerze ist:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import {
  IndicatorRuntime,
  BollingerBandsIndicator,
  RelativeStrengthIndexIndicator,
  MacdHistogramIndicator,
} from '@stocksharp/indicators';

// Berechnet einen Indikator über die Kerzen; liefert dessen Punkte gruppiert nach Output-ID zurück.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of runtime.points()) {
    if (p.time == null || p.value == null) continue;   // Aufwärm-Bars liefern nichts
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bollinger-Bänder (20, 2) als überlagernde Hüllkurve auf dem Kurs-Panel.
const bb = compute(BollingerBandsIndicator, { length: 20, width: 2 }, candles);   // Ausgaben: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) in einem eigenen Unter-Panel.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);     // Ausgabe: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) in einem zweiten Unter-Panel.
const macdPane = chart.addPane();
const macd = compute(
  MacdHistogramIndicator,
  { shortMaLength: 12, longMaLength: 26, signalMaLength: 9 },                    // Ausgaben: macd, signal, histogram
  candles);
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

## Parameter und Ausgaben

Jede Definition deklariert ihre eigenen Parameter und Ausgabe-IDs, und angesprochen werden müssen sie über genau diese IDs und nicht über beschreibende Namen:

| Definition | Parameter | Ausgaben | Panel |
|---|---|---|---|
| `BollingerBandsIndicator` | `length` (20), `width` (2), `upBandWidth`, `lowBandWidth` | `upper`, `middle`, `lower` | Overlay |
| `RelativeStrengthIndexIndicator` | `length` (15) | `oscillator` | eigenes |
| `MacdHistogramIndicator` | `shortMaLength` (12), `longMaLength` (26), `signalMaLength` (9) | `macd`, `signal`, `histogram` | eigenes |

> [!CAUTION]
> Eine unbekannte Parameter-ID wird **stillschweigend verworfen**, und der Indikator rechnet mit dem Standardwert. Es gibt keinen Fehler — es gibt eine falsche Linie. Aus demselben Grund wird bei den Bollinger-Bändern die Abweichung über den Schlüssel `width` und nicht über `stdDev` gesetzt, und beim RSI ist die Länge standardmäßig 15 und nicht 14: Wird die gewohnte Periode benötigt, muss sie ausdrücklich übergeben werden, wie im Beispiel oben.

Die vollständige Liste der Definitionen mit ihren Parametern liefert der Katalog:

```js
import { getClientCatalog } from '@stocksharp/indicators';

for (const item of getClientCatalog()) {
  console.log(item.id, item.name, item.parameters.map(p => p.id));
}
```

## Echtzeitdaten

Eine vollständige Neuberechnung pro Bar ist nicht nötig. Behalten Sie die `IndicatorRuntime` bei und rufen Sie `update` auf — es gibt einen Patch mit dem zurück, was sich geändert hat:

```js
const runtime = new IndicatorRuntime({ definition: RelativeStrengthIndexIndicator, parameters: { length: 14 } });
runtime.resetStreaming(history.map(c => ({ time: c.time, value: c })));

// Nicht geschlossener Bar: false bedeutet einen vorläufigen Wert, der beim nächsten Aufruf ersetzt wird.
runtime.update({ time: bar.time, value: bar }, false);

// Der Bar hat geschlossen — der Wert wird endgültig.
const patch = runtime.update({ time: bar.time, value: bar }, true);
```

Solange ein Bar nicht geschlossen ist, hält die Runtime einen vorläufigen Punkt und ersetzt ihn bei jedem Aufruf, deshalb wächst die Historie durch Aktualisierungen innerhalb eines Bars nicht. `discardPreview()` entfernt den vorläufigen Punkt, `correct(index, input)` berechnet einen historischen Bar neu, der mit einer Korrektur eingetroffen ist.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Band](band.md)
- [Nachladen der Historie](backfill.md)
