# Candlestick

Candlesticks sind die Standard-Preisserie: Jeder Balken wird als Körper zwischen Eröffnungs- und Schlusskurs mit Dochten bis zum Hoch und Tief gezeichnet und je nach Aufwärts- oder Abwärtsbewegung eingefärbt. Sie sind die informationsdichteste Art, den Preis abzulesen, und der Ausgangspunkt für die meisten Charts.

## Live-Demo

```chart-demo candlestick
```

## Einrichtung

Füge eine `CandlestickSeries` hinzu und speise sie mit `{ time, open, high, low, close }`-Punkten (die Zeit ist in Unix-Sekunden):

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
});

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  borderUpColor: '#26a69a',
  borderDownColor: '#ef5350',
  wickUpColor: '#26a69a',
  wickDownColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

Rufe `series.update({ time, open, high, low, close })` auf, um einen Echtzeit-Balken einzuspielen: Derselbe Zeitstempel ersetzt die letzte Kerze, ein neuerer Zeitstempel hängt eine neue an.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [OHLC-Balken](bar.md)
- [Heikin-Ashi](heikin_ashi.md)
