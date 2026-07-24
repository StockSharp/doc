# Point and Figure

Point-&-Figure-Charts verzichten vollständig auf die Zeit und zeichnen Spalten aus X (steigend) und O (fallend). Eine Spalte wächst weiter, solange sich der Preis in ihrer Richtung um ganze Boxen bewegt; eine Zahl von `reversal` Boxen gegen sie beginnt eine neue Spalte. Das Ergebnis hebt Unterstützungen, Widerstände und Ausbrüche hervor.

## Live-Demo

```chart-demo point-figure
```

## Einrichtung

Fügen Sie eine `PointFigureSeries` mit einer `boxSize` und einem `reversal` hinzu und übergeben Sie ihr dann die rohen Kerzen — die Serie baut die Spalten selbst auf:

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

`boxSize` legt den Preis pro X/O fest; `reversal` gibt an, wie viele Boxen gegen die aktuelle Spalte nötig sind, um eine neue zu beginnen (3 ist der klassische Wert).

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Renko](renko.md)
- [Candlestick](candlestick.md)
