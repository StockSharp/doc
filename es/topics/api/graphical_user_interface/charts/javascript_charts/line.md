# Línea

Una serie de línea une un único valor por barra —normalmente el cierre— en una polilínea continua. Es la forma más limpia de mostrar una tendencia o de trazar una serie derivada, como una media móvil.

## Demostración en vivo

```chart-demo line
```

## Configuración

Añade una `LineSeries` y aliméntala con puntos `{ time, value }`:

```js
const series = chart.addSeries(SSChart.LineSeries, {
  color: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

Cualquier conjunto de datos de un solo valor funciona aquí — sustituye `c.close` por el valor de un indicador para superponerlo sobre el gráfico de precios.

## Véase también

- [Gráficos en JavaScript](../javascript_charts.md)
- [Área](area.md)
- [Banda](band.md)
