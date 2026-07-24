# Linie

Eine Linienserie verbindet einen einzelnen Wert pro Balken — üblicherweise den Schlusskurs — zu einer durchgehenden Linie. Sie ist die sauberste Möglichkeit, einen Trend darzustellen oder eine abgeleitete Serie wie einen gleitenden Durchschnitt zu zeichnen.

## Live-Demo

```chart-demo line
```

## Einrichtung

Fügen Sie eine `LineSeries` hinzu und speisen Sie sie mit `{ time, value }`-Punkten:

```js
const series = chart.addSeries(SSChart.LineSeries, {
  color: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

Jeder Datensatz mit einem einzelnen Wert funktioniert hier — ersetzen Sie `c.close` durch einen Indikatorwert, um ihn über dem Kurschart darzustellen.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Fläche](area.md)
- [Band](band.md)
