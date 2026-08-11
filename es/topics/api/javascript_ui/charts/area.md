# Área

Una serie de área es una línea con la región inferior rellena mediante un degradado vertical. Se lee como un gráfico de líneas, pero enfatiza la magnitud de un único valor a lo largo del tiempo, lo que resulta adecuado para curvas de capital y vistas generales de precios.

## Demostración en vivo

```chart-demo area
```

## Configuración

Añada una `AreaSeries` y aliméntela con puntos `{ time, value }`; `topColor`/`bottomColor` definen el degradado:

```js
const series = chart.addSeries(SSChart.AreaSeries, {
  topColor: 'rgba(74,158,255,0.3)',
  bottomColor: 'rgba(74,158,255,0.02)',
  lineColor: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Línea](line.md)
- [Histograma](histogram.md)
