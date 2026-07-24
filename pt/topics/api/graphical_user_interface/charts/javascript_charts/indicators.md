# Indicadores

O gráfico vem com um catálogo de cerca de 160 indicadores técnicos. Você calcula qualquer um deles com o `IndicatorRuntime` público sobre seus candles e, em seguida, desenha os resultados você mesmo como séries comuns — sobreposições no painel de preço ou osciladores em seus próprios sub-painéis.

## Demonstração ao vivo

Bollinger Bands sobre os candles, com RSI e MACD em painéis separados abaixo.

```chart-demo indicators
```

## Configuração

Importe o runtime e as definições de indicadores de que você precisa, execute cada um sobre os candles e plote as saídas. Alimente o runtime com entradas `{ time, value }`, onde `value` é o candle:

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

Cada definição declara seus próprios parâmetros e ids de saída (Bollinger emite `upper`/`middle`/`lower`, RSI um único `oscillator`, MACD `macd`/`signal`/`histogram`). Para dados em tempo real, mantenha o `IndicatorRuntime` e chame `runtime.update({ time, value }, isFinal)` por barra em vez de recalcular.

## Veja também

- [Gráficos JavaScript](../javascript_charts.md)
- [Band](band.md)
- [Preenchimento de histórico](backfill.md)
