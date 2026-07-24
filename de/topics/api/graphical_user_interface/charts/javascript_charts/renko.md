# Renko

Renko-Charts werden aus preisbasierten *Bausteinen* (Bricks) fester Größe aufgebaut und ignorieren die Zeit: Ein neuer Baustein wird nur hinzugefügt, wenn sich der Preis um die Boxgröße bewegt, sodass seitliches Rauschen zusammenfällt und Trends hervortreten. Jeder Baustein umfasst eine Box in Richtung der Preisbewegung.

## Live-Demo

```chart-demo renko
```

## Einrichtung

Fügen Sie eine `RenkoSeries` mit einer `boxSize` hinzu und speisen Sie rohe `{ time, open, high, low, close }`-Kerzen ein — die Serie baut die Bausteine selbst auf:

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

Wählen Sie `boxSize` anhand der Preisspanne des Instruments: zu klein erzeugt Rauschen, zu groß verbirgt Bewegungen. Eine gängige Regel ist ein Bruchteil der durchschnittlichen Bar-Spanne.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Point-&-Figure-Chart](point_figure.md)
- [Heikin-Ashi-Kerzen](heikin_ashi.md)
