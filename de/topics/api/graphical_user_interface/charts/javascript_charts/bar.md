# OHLC-Balken

OHLC-Balken zeigen dieselben vier Preise wie Candlesticks, jedoch ohne den gefüllten Körper: Eine vertikale Linie erstreckt sich über die High-Low-Spanne, ein linker Strich markiert den Eröffnungskurs und ein rechter Strich den Schlusskurs. Sie halten das Chart übersichtlich und zeigen dennoch Eröffnungs- und Schlusskurs jedes Balkens.

## Live-Demo

```chart-demo bar
```

## Einrichtung

Fügen Sie eine `BarSeries` hinzu und übergeben Sie ihr `{ time, open, high, low, close }`-Punkte:

```js
const series = chart.addSeries(SSChart.BarSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

Der Balken wird als steigend eingefärbt, wenn der Schlusskurs auf oder über dem Eröffnungskurs liegt, andernfalls als fallend.

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Linie](line.md)
