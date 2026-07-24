# OHLC bars

OHLC bars show the same four prices as candlesticks without the filled body: a vertical line spans the high–low range, a left tick marks the open and a right tick marks the close. They keep the chart light while still showing every bar's open and close.

## Live demo

```chart-demo bar
```

## Setup

Add a `BarSeries` and feed it `{ time, open, high, low, close }` points:

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

The bar is colored up when the close is at or above the open and down otherwise.

## See also

- [JavaScript charts](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Line](line.md)
