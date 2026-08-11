# Fläche

Eine Flächenserie ist eine Linie, deren Bereich darunter mit einem vertikalen Farbverlauf gefüllt ist. Sie liest sich wie ein Liniendiagramm, betont jedoch die Größenordnung eines einzelnen Werts im Zeitverlauf, was sich für Equity-Kurven und Preisübersichten eignet.

## Live-Demo

```chart-demo area
```

## Einrichtung

Fügen Sie eine `AreaSeries` hinzu und speisen Sie sie mit `{ time, value }`-Punkten; `topColor`/`bottomColor` definieren den Farbverlauf:

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

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Linie](line.md)
- [Histogramm](histogram.md)
