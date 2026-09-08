# Indicadores

O catálogo com cerca de 160 indicadores técnicos vive num pacote separado, [@stocksharp/indicators](https://www.npmjs.com/package/@stocksharp/indicators). Chega juntamente com o gráfico, como dependência, mas as definições têm de ser importadas exatamente a partir dele. Qualquer definição é calculada pelo `IndicatorRuntime` público sobre as suas velas, e o resultado é representado por si com séries comuns — como sobreposição no painel de preços ou como oscilador num subpainel separado.

## Demonstração em direto

Bandas de Bollinger sobre as velas, com RSI e MACD em painéis separados abaixo.

```chart-demo indicators
```

## Configuração

Importe o runtime e as definições de que necessita, execute cada uma sobre as velas e represente as saídas. Forneça ao runtime entradas `{ time, value }`, em que `value` é a vela:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import {
  IndicatorRuntime,
  BollingerBandsIndicator,
  RelativeStrengthIndexIndicator,
  MacdHistogramIndicator,
} from '@stocksharp/indicators';

// Calcula um indicador sobre as velas; devolve os respetivos pontos agrupados por id de saída.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of runtime.points()) {
    if (p.time == null || p.value == null) continue;   // barras de aquecimento não emitem nada
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bandas de Bollinger (20, 2) como envelope sobreposto no painel de preço.
const bb = compute(BollingerBandsIndicator, { length: 20, width: 2 }, candles);   // saídas: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) no respetivo subpainel.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);     // saída: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) num segundo subpainel.
const macdPane = chart.addPane();
const macd = compute(
  MacdHistogramIndicator,
  { shortMaLength: 12, longMaLength: 26, signalMaLength: 9 },                    // saídas: macd, signal, histogram
  candles);
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

## Parâmetros e saídas

Cada definição declara os seus próprios parâmetros e identificadores de saída, e é por esses identificadores que se lhes deve aceder, não por nomes descritivos:

| Definição | Parâmetros | Saídas | Painel |
|---|---|---|---|
| `BollingerBandsIndicator` | `length` (20), `width` (2), `upBandWidth`, `lowBandWidth` | `upper`, `middle`, `lower` | sobreposição |
| `RelativeStrengthIndexIndicator` | `length` (15) | `oscillator` | separado |
| `MacdHistogramIndicator` | `shortMaLength` (12), `longMaLength` (26), `signalMaLength` (9) | `macd`, `signal`, `histogram` | separado |

> [!CAUTION]
> Um identificador de parâmetro desconhecido é **descartado em silêncio** e o indicador é calculado com o valor predefinido. Não haverá erro — haverá uma linha errada. É pela mesma razão que, nas bandas de Bollinger, o desvio se indica pela chave `width` e não por `stdDev`, e que o RSI tem por predefinição um comprimento de 15 e não de 14: se quiser o período habitual, tem de o passar explicitamente, como no exemplo acima.

A lista completa das definições com os respetivos parâmetros é dada pelo catálogo:

```js
import { getClientCatalog } from '@stocksharp/indicators';

for (const item of getClientCatalog()) {
  console.log(item.id, item.name, item.parameters.map(p => p.id));
}
```

## Dados em tempo real

Não é preciso recalcular tudo a cada barra. Guarde o `IndicatorRuntime` e chame `update` — devolve um patch com aquilo que mudou:

```js
const runtime = new IndicatorRuntime({ definition: RelativeStrengthIndexIndicator, parameters: { length: 14 } });
runtime.resetStreaming(history.map(c => ({ time: c.time, value: c })));

// Barra por fechar: false significa um valor provisório, que será substituído na chamada seguinte.
runtime.update({ time: bar.time, value: bar }, false);

// A barra fechou — o valor torna-se definitivo.
const patch = runtime.update({ time: bar.time, value: bar }, true);
```

Enquanto a barra não fecha, o runtime mantém o ponto provisório e substitui-o a cada chamada, pelo que o histórico não cresce com as atualizações dentro da mesma barra. `discardPreview()` retira o ponto provisório e `correct(index, input)` recalcula uma barra histórica que chegou corrigida.

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Banda](band.md)
- [Preenchimento de histórico](backfill.md)
