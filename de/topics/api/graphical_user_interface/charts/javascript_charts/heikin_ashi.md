# Heikin-Ashi-Kerzen

Heikin-Ashi-Kerzen ("Durchschnittsbalken") werden aus den rohen OHLC-Werten berechnet, um Rauschen zu glätten: Aufeinanderfolgende Kerzen gleicher Farbe machen Trends leichter lesbar – um den Preis, den tatsächlichen Eröffnungs- und Schlusskurs zu verbergen. Sie werden mit der gewöhnlichen Candlestick-Serie gezeichnet, die mit transformierten Werten gespeist wird.

## Live-Demo

```chart-demo heikin-ashi
```

## Einrichtung

Berechnen Sie die Heikin-Ashi-Werte und speisen Sie sie in eine `CandlestickSeries` ein:

```js
function heikinAshi(candles) {
  const out = [];
  let prevOpen = candles[0].open, prevClose = candles[0].close;
  for (const c of candles) {
    const haClose = (c.open + c.high + c.low + c.close) / 4;
    const haOpen = (prevOpen + prevClose) / 2;
    out.push({
      time: c.time,
      open: haOpen,
      high: Math.max(c.high, haOpen, haClose),
      low: Math.min(c.low, haOpen, haClose),
      close: haClose,
    });
    prevOpen = haOpen; prevClose = haClose;
  }
  return out;
}

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350',
});
series.setData(heikinAshi(candles));

chart.timeScale().fitContent();
```

## Siehe auch

- [JavaScript-Charts](../javascript_charts.md)
- [Kerzenchart](candlestick.md)
- [Renko](renko.md)
