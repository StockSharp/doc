# Indicadores

El catálogo de aproximadamente 160 indicadores técnicos vive en un paquete aparte, [@stocksharp/indicators](https://www.npmjs.com/package/@stocksharp/indicators). Llega junto con el gráfico como dependencia, pero las definiciones hay que importarlas precisamente de él. Cualquier definición la calcula el `IndicatorRuntime` público sobre sus velas, y el resultado lo dibuja usted mismo con series ordinarias: como superposición en el panel de precios o como oscilador en un subpanel propio.

## Demostración en vivo

Bandas de Bollinger sobre las velas, con RSI y MACD en paneles separados debajo.

```chart-demo indicators
```

## Configuración

Importe el runtime y las definiciones que necesite, ejecute cada una sobre las velas y trace las salidas. Al runtime se le alimenta con pares `{ time, value }`, donde `value` es la vela:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import {
  IndicatorRuntime,
  BollingerBandsIndicator,
  RelativeStrengthIndexIndicator,
  MacdHistogramIndicator,
} from '@stocksharp/indicators';

// Calcula un indicador sobre las velas; devuelve sus puntos agrupados por id de salida.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of runtime.points()) {
    if (p.time == null || p.value == null) continue;   // las barras de calentamiento no emiten nada
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bandas de Bollinger (20, 2) como envolvente superpuesta en el panel de precios.
const bb = compute(BollingerBandsIndicator, { length: 20, width: 2 }, candles);   // salidas: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) en su propio subpanel.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);     // salida: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) en un segundo subpanel.
const macdPane = chart.addPane();
const macd = compute(
  MacdHistogramIndicator,
  { shortMaLength: 12, longMaLength: 26, signalMaLength: 9 },                    // salidas: macd, signal, histogram
  candles);
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

## Parámetros y salidas

Cada definición declara sus propios parámetros e identificadores de salida, y hay que acceder a ellos por esos identificadores y no por nombres descriptivos:

| Definición | Parámetros | Salidas | Panel |
|---|---|---|---|
| `BollingerBandsIndicator` | `length` (20), `width` (2), `upBandWidth`, `lowBandWidth` | `upper`, `middle`, `lower` | superposición |
| `RelativeStrengthIndexIndicator` | `length` (15) | `oscillator` | propio |
| `MacdHistogramIndicator` | `shortMaLength` (12), `longMaLength` (26), `signalMaLength` (9) | `macd`, `signal`, `histogram` | propio |

> [!CAUTION]
> Un identificador de parámetro desconocido **se descarta en silencio** y el indicador se calcula con el valor predeterminado. No habrá error: habrá una línea incorrecta. Por ese mismo motivo, en las bandas de Bollinger la desviación se indica con la clave `width` y no con `stdDev`, y en el RSI la longitud predeterminada es 15 y no 14: si necesita el período habitual, hay que pasarlo explícitamente, como en el ejemplo anterior.

La lista completa de definiciones con sus parámetros la entrega el catálogo:

```js
import { getClientCatalog } from '@stocksharp/indicators';

for (const item of getClientCatalog()) {
  console.log(item.id, item.name, item.parameters.map(p => p.id));
}
```

## Datos en tiempo real

No hace falta un recálculo completo en cada barra. Conserve el `IndicatorRuntime` y llame a `update`, que devuelve un parche con lo que ha cambiado:

```js
const runtime = new IndicatorRuntime({ definition: RelativeStrengthIndexIndicator, parameters: { length: 14 } });
runtime.resetStreaming(history.map(c => ({ time: c.time, value: c })));

// Barra sin cerrar: false significa un valor preliminar que se sustituirá en la siguiente llamada.
runtime.update({ time: bar.time, value: bar }, false);

// La barra se ha cerrado: el valor pasa a ser definitivo.
const patch = runtime.update({ time: bar.time, value: bar }, true);
```

Mientras la barra no se cierra, el runtime mantiene un punto preliminar y lo sustituye en cada llamada, por lo que el historial no crece con las actualizaciones dentro de una misma barra. `discardPreview()` retira el punto preliminar y `correct(index, input)` recalcula una barra histórica que ha llegado con una corrección.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Banda](band.md)
- [Relleno de historial](backfill.md)
