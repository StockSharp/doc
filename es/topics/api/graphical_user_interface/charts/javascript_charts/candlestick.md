# Velas

Las velas japonesas son la serie de precios predeterminada: cada barra se dibuja como un cuerpo entre la apertura y el cierre, con mechas hasta el máximo y el mínimo, coloreada de alza o de baja. Son la forma más densa en información de leer el precio y el punto de partida para la mayoría de los gráficos.

## Demo en vivo

```chart-demo candlestick
```

## Configuración

Añade una `CandlestickSeries` y aliméntala con puntos `{ time, open, high, low, close }` (el tiempo va en segundos Unix):

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
});

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  borderUpColor: '#26a69a',
  borderDownColor: '#ef5350',
  wickUpColor: '#26a69a',
  wickDownColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

Llama a `series.update({ time, open, high, low, close })` para enviar una barra en tiempo real: la misma marca de tiempo reemplaza la última vela, mientras que una marca de tiempo más reciente añade una nueva.

## Véase también

- [Gráficos JavaScript](../javascript_charts.md)
- [Barras OHLC](bar.md)
- [Heikin-Ashi](heikin_ashi.md)
