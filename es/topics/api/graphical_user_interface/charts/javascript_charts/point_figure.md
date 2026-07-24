# Point and Figure

Los gráficos Point & Figure eliminan por completo el tiempo y dibujan columnas de X (alcistas) y O (bajistas). Una columna sigue creciendo mientras el precio se mueve en su dirección en cajas enteras; un número `reversal` de cajas en su contra inicia una nueva columna. El resultado resalta los soportes, las resistencias y las rupturas.

## Demostración en vivo

```chart-demo point-figure
```

## Configuración

Añade una `PointFigureSeries` con un `boxSize` y un `reversal`, y luego aliméntala con velas en bruto — la serie construye las columnas por sí misma:

```js
const series = chart.addSeries(SSChart.PointFigureSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
  reversal: 2,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

`boxSize` establece el precio por cada X/O; `reversal` es cuántas cajas en contra de la columna actual se necesitan para iniciar una nueva (3 es el valor clásico).

## Véase también

- [Gráficos JavaScript](../javascript_charts.md)
- [Renko](renko.md)
- [Velas japonesas](candlestick.md)
