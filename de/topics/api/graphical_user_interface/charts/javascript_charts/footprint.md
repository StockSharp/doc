# Footprint

Ein Footprint-Chart öffnet jeden Balken, um das an jedem Preis innerhalb des Balkens gehandelte Volumen anzuzeigen, aufgeteilt in Bid und Ask. Die Imbalance-Färbung hebt hervor, wo aggressive Käufer oder Verkäufer dominiert haben — das ist der Kern der Order-Flow-Analyse.

## Live-Demo

Zoomen Sie hinein, um die einzelnen Zellen zu lesen — ein Footprint ist nur bei einer Handvoll Balken gleichzeitig lesbar.

```chart-demo footprint
```

![Footprint-Chart](../../../../../images/chart_footprint.png)

## Einrichtung

Fügen Sie eine `FootprintSeries` hinzu und speisen Sie sie mit exakten Order-Flow-Balken. Jeder Balken trägt `dataMode: 'exact'`, seine OHLC-Werte und ein `levels`-Array aus `{ price, bidVolume, askVolume, tradeCount }`:

```js
const series = chart.addSeries(SSChart.FootprintSeries, {
  tickSize: 0.25,
  mode: SSChart.FootprintDisplayMode.BidAsk,       // BidAsk | Delta | Total | Ladder
  detailLevel: SSChart.FootprintDetailLevel.Auto,  // Auto | Numbers | Heatmap | Summary
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showUnfinishedAuctions: true,
});

series.setData(exactBars);
```

`mode` bestimmt, was jede Zelle anzeigt (Bid × Ask, Delta, Total oder eine Ladder); `detailLevel` tauscht numerisches Detail gegen Dichte, während Sie zoomen — `Auto` schaltet automatisch um.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Volume Profile](volume_profile.md)
- [TPO (Market Profile)](tpo.md)
