# Histograma

Un histograma dibuja una barra vertical por cada punto a partir de un valor base. Su uso más común es el volumen, donde cada barra se colorea según si la vela cerró al alza o a la baja, aunque sirve para cualquier magnitud por barra.

## Demostración en vivo

```chart-demo histogram
```

## Configuración

Añada una `HistogramSeries` y aliméntela con puntos `{ time, value, color? }`; un `color` por punto anula el color de la serie:

```js
const series = chart.addSeries(SSChart.HistogramSeries, {
  priceFormat: { type: 'volume' },
});

series.setData(candles.map(c => ({
  time: c.time,
  value: c.volume,
  color: c.close >= c.open ? 'rgba(38,166,154,0.7)' : 'rgba(239,83,80,0.7)',
})));

chart.timeScale().fitContent();
```

Para mostrar el volumen debajo de un gráfico de precios en lugar de por separado, coloque el histograma en una escala de precios superpuesta y fíjelo en la parte inferior:

```js
const volume = chart.addSeries(SSChart.HistogramSeries, { priceScaleId: '', priceFormat: { type: 'volume' } });
volume.priceScale().applyOptions({ scaleMargins: { top: 0.82, bottom: 0 } });
```

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Área](area.md)
- [Velas](candlestick.md)
