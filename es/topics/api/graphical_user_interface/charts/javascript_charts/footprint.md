# Footprint

Un gráfico de footprint despliega cada barra para mostrar el volumen negociado en cada precio dentro de ella, dividido en bid y ask. El coloreado por desequilibrio (imbalance) resalta dónde dominaron los compradores o vendedores agresivos, que es la esencia de la lectura del order flow.

## Demostración en vivo

Haga zoom para leer las celdas individuales — el footprint solo es legible en unas pocas barras a la vez.

```chart-demo footprint
```

![Gráfico de footprint](../../../../../images/chart_footprint.png)

## Configuración

Añada una `FootprintSeries` y aliméntela con barras exactas de order flow. Cada barra lleva `dataMode: 'exact'`, su OHLC y un array `levels` de `{ price, bidVolume, askVolume, tradeCount }`:

```js
const series = chart.addSeries(SSChart.FootprintSeries, {
  tickSize: 0.25,
  mode: SSChart.FootprintDisplayMode.BidAsk,       // modo de visualización: BidAsk | Delta | Total | Ladder
  detailLevel: SSChart.FootprintDetailLevel.Auto,  // nivel de detalle: Auto | Numbers | Heatmap | Summary
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showUnfinishedAuctions: true,
});

series.setData(exactBars);
```

`mode` elige lo que muestra cada celda (bid × ask, delta, total o una escalera); `detailLevel` intercambia detalle numérico por densidad a medida que hace zoom — `Auto` cambia automáticamente.

## Véase también

- [Gráficos en JavaScript](../javascript_charts.md)
- [Perfil de volumen](volume_profile.md)
- [TPO (Perfil de mercado)](tpo.md)
