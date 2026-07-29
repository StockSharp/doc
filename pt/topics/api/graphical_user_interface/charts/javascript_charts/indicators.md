# Indicadores

O gráfico inclui um catálogo com cerca de 160 indicadores técnicos. É possível calcular qualquer um deles sobre as velas com o `IndicatorRuntime` público e, em seguida, representar os resultados como séries comuns — sobreposições no painel de preços ou osciladores nos respetivos subpainéis.

## Demonstração em direto

Bandas de Bollinger sobre as velas, com RSI e MACD em painéis separados abaixo.

```chart-demo indicators
```

## Configuração

Importe o runtime e as definições dos indicadores de que necessita, execute cada um sobre as velas e represente as saídas. Forneça ao runtime entradas `{ time, value }`, em que `value` é a vela:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import { IndicatorRuntime, BollingerBandsIndicator, RelativeStrengthIndexIndicator, MacdIndicator } from '@stocksharp/chart/indicators';

// Calcula um indicador sobre as velas; devolve os respetivos pontos agrupados por id de saída.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  const points = runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of points) {
    if (p.time == null || p.value == null) continue;   // barras de aquecimento não emitem nada
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bandas de Bollinger (20, 2) como envelope sobreposto no painel de preço.
const bb = compute(BollingerBandsIndicator, { length: 20, stdDev: 2 }, candles);   // saídas: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) no respetivo subpainel.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);       // saída: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) num segundo subpainel.
const macdPane = chart.addPane();
const macd = compute(MacdIndicator, { fastLength: 12, slowLength: 26, signalLength: 9 }, candles);   // saídas: macd, signal, histogram
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

Cada definição declara os respetivos parâmetros e IDs de saída (Bollinger emite `upper`/`middle`/`lower`, RSI um único `oscillator`, MACD `macd`/`signal`/`histogram`). Para dados em tempo real, mantenha o `IndicatorRuntime` e invoque `runtime.update({ time, value }, isFinal)` por barra, em vez de recalcular.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Banda](band.md)
- [Preenchimento de histórico](backfill.md)
