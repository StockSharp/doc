# Velas Heikin-Ashi

Las velas Heikin-Ashi ("barra promedio") se calculan a partir del OHLC bruto para suavizar el ruido: las velas consecutivas del mismo color facilitan la lectura de las tendencias, a costa de ocultar la apertura y el cierre reales. Se dibujan con la serie de velas japonesas habitual, alimentada con los valores transformados.

## Demostración en vivo

```chart-demo heikin-ashi
```

## Configuración

Calcule los valores Heikin-Ashi y alimente con ellos una `CandlestickSeries`:

```js
function heikinAshi(candles) {
  const out = [];
  let prevOpen = candles[0].open, prevClose = candles[0].close;
  for (const c of candles) {
    const haClose = (c.open + c.high + c.low + c.close) / 4;
    const haOpen = (prevOpen + prevClose) / 2;
    out.push({
      time: c.time,
      open: haOpen,
      high: Math.max(c.high, haOpen, haClose),
      low: Math.min(c.low, haOpen, haClose),
      close: haClose,
    });
    prevOpen = haOpen; prevClose = haClose;
  }
  return out;
}

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350',
});
series.setData(heikinAshi(candles));

chart.timeScale().fitContent();
```

## Véase también

- [Gráficos en JavaScript](../javascript_charts.md)
- [Velas](candlestick.md)
- [Renko](renko.md)
