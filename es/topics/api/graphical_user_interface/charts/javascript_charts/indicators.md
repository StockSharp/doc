# Indicadores

El gráfico incluye un catálogo de unos 160 indicadores técnicos. Puede calcular cualquiera de ellos con el `IndicatorRuntime` público sobre sus velas y luego dibujar los resultados usted mismo como series ordinarias: superposiciones en el panel de precios u osciladores en sus propios subpaneles.

## Demostración en vivo

Bandas de Bollinger sobre las velas, con RSI y MACD en paneles separados debajo.

```chart-demo indicators
```

## Configuración

Importe el runtime y las definiciones de indicadores que necesite, ejecute cada uno sobre las velas y trace las salidas. Alimente el runtime con entradas `{ time, value }` donde `value` es la vela:

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

Cada definición declara sus propios parámetros e identificadores de salida (Bollinger emite `upper`/`middle`/`lower`, RSI un único `oscillator`, MACD `macd`/`signal`/`histogram`). Para datos en tiempo real, conserve el `IndicatorRuntime` y llame a `runtime.update({ time, value }, isFinal)` por cada barra en lugar de recalcular.

## Véase también

- [Gráficos JavaScript](../javascript_charts.md)
- [Banda](band.md)
- [Relleno de historial](backfill.md)
