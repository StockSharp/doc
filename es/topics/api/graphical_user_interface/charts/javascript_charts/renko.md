# Renko

Los gráficos Renko se construyen a partir de *ladrillos* de precio de tamaño fijo e ignoran el tiempo: se añade un nuevo ladrillo solo cuando el precio se mueve el tamaño de caja, por lo que el ruido lateral se colapsa y las tendencias destacan. Cada ladrillo abarca una caja en la dirección del movimiento.

## Demostración en vivo

```chart-demo renko
```

## Configuración

Añade una `RenkoSeries` con un `boxSize` y aliméntala con velas `{ time, open, high, low, close }` en bruto — la serie construye los ladrillos por sí misma:

```js
const series = chart.addSeries(SSChart.RenkoSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

Elige `boxSize` a partir del rango de precios del instrumento: demasiado pequeño produce ruido, demasiado grande oculta los movimientos. Una regla común es una fracción del rango medio de las barras.

## Véase también

- [Gráficos JavaScript](../javascript_charts.md)
- [Point and Figure](point_figure.md)
- [Heikin-Ashi](heikin_ashi.md)
