# Barras OHLC

Las barras OHLC muestran los mismos cuatro precios que las velas, pero sin el cuerpo relleno: una línea vertical abarca el rango máximo–mínimo, una marca a la izquierda indica la apertura y una marca a la derecha indica el cierre. Mantienen el gráfico ligero mostrando a la vez la apertura y el cierre de cada barra.

## Demostración en vivo

```chart-demo bar
```

## Configuración

Añada una `BarSeries` y aliméntela con puntos `{ time, open, high, low, close }`:

```js
const series = chart.addSeries(SSChart.BarSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

La barra se colorea al alza cuando el cierre es igual o superior a la apertura, y a la baja en caso contrario.

## Véase también

- [Gráficos en JavaScript](../javascript_charts.md)
- [Velas](candlestick.md)
- [Línea](line.md)
